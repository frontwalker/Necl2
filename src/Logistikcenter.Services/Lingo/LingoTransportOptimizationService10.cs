using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using Logistikcenter.Domain;

namespace Logistikcenter.Services.Lingo
{
    [StructLayout(LayoutKind.Sequential)]
    internal class CallbackData
    {
        public int nIterations;

        public CallbackData()
        {
            nIterations = 0;
        }
    }

    internal class LngCallback
    {
        // Lingo callback function to display the current iteration count
        public static int MyCallback(int pLingoEnv, int nReserved, IntPtr pMyData)
        {            
            var cb = new CallbackData();
            Marshal.PtrToStructure(pMyData, cb);

            int nIterations = -1;
            int nErr = lingo.LSgetCallbackInfoLng(pLingoEnv, lingo.LS_IINFO_ITERATIONS_LNG, ref nIterations);

            if (nErr == lingo.LSERR_NO_ERROR_LNG && nIterations != cb.nIterations)
                cb.nIterations = nIterations;

            Marshal.StructureToPtr(cb, pMyData, true);

            return 0;
        }
    }

    public class LingoTransportOptimizationService : ITransportOptimizationService
    {
        private struct OutFileRow
        {
            public long Origin;
            public long Destination;
            public long LegId;
        }

        private struct ShortageRow
        {
            public long Origin;
            public long Destination;
            public double Shortage;
        }

        private int _pLingoEnv;        
        private readonly IRepository _repository;
        private readonly FileInfo _modelPath;
        private readonly DirectoryInfo _inputFileFolder;        
        private readonly DirectoryInfo _logFileFolder;

        private IList<Leg> _legs;
        
        private DateTime _zeroTime;
        private int _numLegs;
        private double[] _minTrp;
        private double[] _maxTrp;
        private double[] _cost;
        private double[] _duration;
        private double[] _startTime;
        private string _legIdString = string.Empty;
        private string _transportString = string.Empty;
        private string _nodeString;

        public LingoTransportOptimizationService(IRepository repository, string modelPath, string inputFileFolder, string logFileFolder)
        {            
            _repository = repository;
            _modelPath = new FileInfo(modelPath);
            _inputFileFolder = new DirectoryInfo(inputFileFolder);
            _logFileFolder = new DirectoryInfo(logFileFolder);

            EnsureFilesAndFoldersExists();
        }

        private void EnsureFilesAndFoldersExists()
        {
            if (!File.Exists(_modelPath.FullName))
                throw new Exception(string.Format("Could not find model file at {0}", _modelPath.FullName));

            if (!Directory.Exists(_inputFileFolder.FullName))
                Directory.CreateDirectory(_inputFileFolder.FullName);

            if (!Directory.Exists(_logFileFolder.FullName))
                Directory.CreateDirectory(_logFileFolder.FullName);
        }

        static void CheckIfError(int errorCode)
        {
            if (errorCode != lingo.LSERR_NO_ERROR_LNG)
                throw new LingoException();
        }

        public void LoadData(DateTime minStartTime, DateTime maxEndTime)
        {
            _legs = _repository.Query<Leg>().Where(l => l.DepartureTime >= minStartTime && l.ArrivalTime <= maxEndTime).ToList();
            
            _zeroTime = DateTime.Now;

            _numLegs = _legs.Count;

            _minTrp = new double[_numLegs];
            _maxTrp = new double[_numLegs];
            _cost = new double[_numLegs];
            _duration = new double[_numLegs];
            _startTime = new double[_numLegs];

            long currentLeg = 0;
            var destinations = new List<long>();

            foreach(var leg in _legs)
            {
                _minTrp[currentLeg] = 1;
                _maxTrp[currentLeg] = leg.TotalCapacity - leg.UsedCapacity;
                _cost[currentLeg] = leg.Cost;
                _duration[currentLeg] = leg.Duration.Hours;

                TimeSpan timeToDeparture = leg.DepartureTime - _zeroTime;
                _startTime[currentLeg] = timeToDeparture.TotalHours;
                _legIdString += string.Format("{0} ", leg.Id);

                _transportString += string.Format("{0} {1} {2} 1\r\n", leg.Origin.Id, leg.Destination.Id, leg.Id);

                destinations.Add(leg.Origin.Id);
                destinations.Add(leg.Destination.Id);

                currentLeg++;                
            }

            destinations.Sort();

            long previousId = -1;

            foreach(var destinationId in destinations)
            {
                if (destinationId > previousId)
                {
                    _nodeString += string.Format("{0} ", destinationId);
                    previousId = destinationId;
                }
            }            
        }

        public void MinimizeCost(IEnumerable<TransportUnit> transportUnits, int maxNumberOfSolutions)
        {
            string logId = Guid.NewGuid().ToString();

            int nPointersNow = -1;
            double dStatus = -1;
            double maxTripTime = double.MaxValue;

            transportUnits.ToList().ForEach(t => t.ProposedRoutes.Clear());

            int solutionIndex;

            for (solutionIndex = 0; solutionIndex < maxNumberOfSolutions; solutionIndex++)
            {
                try
                {
                    System.Threading.Monitor.Enter(this);
                    
                    _pLingoEnv = lingo.LScreateEnvLng();

                    if (_pLingoEnv == 0)
                        throw new UnableToCreateLingoEnvironmentException();
                    
                    // Open LINGO's log file  

                    var logFile = Path.Combine(_logFileFolder.FullName, string.Format("lingo.{0}.{1}.log", logId, solutionIndex));

                    int errorCode = lingo.LSopenLogFileLng(_pLingoEnv, logFile);
                    CheckIfError(errorCode);

                    // Let Lingo know we have a callback function
                    //var cbd = new CallbackData();
                    //var cb = new lingo.typCallback(LngCallback.MyCallback);

                    //errorCode = lingo.LSsetCallbackSolverLng(_pLingoEnv, cb, cbd);
                    //CheckIfError(errorCode);

                    // must pin lingo's transfer areas in memory
                    unsafe
                    {
                        fixed (                        
                            double* pResult = new double[_numLegs],
                                    pMinTrp = _minTrp,
                                    pMaxTrp = _maxTrp,
                                    pCost = _cost,
                                    pDuration = _duration,
                                    pStartTime = _startTime,
                                    pRequestedVolume = new double[transportUnits.Count()],
                                    pTotalTripTime = new double[transportUnits.Count()],
                                    pMinStartTime = new double[transportUnits.Count()]
                               )
                        {
                            //clean mem.areas from previous runs
                            errorCode = lingo.LSclearPointersLng(_pLingoEnv);
                            CheckIfError(errorCode);

                            var inputFilePath = Path.Combine(_inputFileFolder.FullName, "logistikcenter.txt");

                            using (var inputFile = new StreamWriter(inputFilePath, false))
                            {
                                string demandString = string.Empty;

                                //nodes: TODO
                                inputFile.WriteLine("{0} ~", _nodeString);

                                //leg_id:n
                                inputFile.WriteLine("{0} ~", _legIdString);

                                //com type: TODO
                                inputFile.WriteLine("1 2 ~");

                                //transport
                                inputFile.WriteLine("{0} ~", _transportString);

                                //com cust sup
                                int currentUnit = 0;

                                foreach (TransportUnit transportUnit in transportUnits)
                                {
                                    demandString += (string.Format("1 {0} {1}\r\n", transportUnit.Origin.Id, transportUnit.Destination.Id));
                                    pRequestedVolume[currentUnit] = transportUnit.Cargo.Volume;

                                    TimeSpan span = (transportUnit.MaxDeliveryTime - _zeroTime);
                                    pTotalTripTime[currentUnit] = span.TotalHours;

                                    span = (transportUnit.MinPickupTime - _zeroTime);
                                    pMinStartTime[currentUnit] = span.TotalHours;

                                    currentUnit++;
                                }

                                //demand
                                inputFile.WriteLine("{0} ~", demandString);
                            }

                            // Pointer to the min totalCapacity                        
                            errorCode = lingo.LSsetPointerLng(_pLingoEnv, ref pMinTrp[0], ref nPointersNow);
                            CheckIfError(errorCode);

                            // Pointer to the max totalCapacity
                            errorCode = lingo.LSsetPointerLng(_pLingoEnv, ref pMaxTrp[0], ref nPointersNow);
                            CheckIfError(errorCode);

                            // Pointer to the cost
                            errorCode = lingo.LSsetPointerLng(_pLingoEnv, ref pCost[0], ref nPointersNow);
                            CheckIfError(errorCode);

                            // Pointer to the duration 
                            errorCode = lingo.LSsetPointerLng(_pLingoEnv, ref pDuration[0], ref nPointersNow);
                            CheckIfError(errorCode);

                            // Pointer to the starttime 
                            errorCode = lingo.LSsetPointerLng(_pLingoEnv, ref pStartTime[0], ref nPointersNow);
                            CheckIfError(errorCode);

                            // Pointer to the customer demanded volume
                            errorCode = lingo.LSsetPointerLng(_pLingoEnv, ref pRequestedVolume[0], ref nPointersNow);
                            CheckIfError(errorCode);

                            // Pointer to the customer demanded max time
                            errorCode = lingo.LSsetPointerLng(_pLingoEnv, ref pTotalTripTime[0], ref nPointersNow);
                            CheckIfError(errorCode);

                            // Pointer to the customer demanded max time
                            errorCode = lingo.LSsetPointerLng(_pLingoEnv, ref pMinStartTime[0], ref nPointersNow);
                            CheckIfError(errorCode);

                            // max trip time
                            errorCode = lingo.LSsetPointerLng(_pLingoEnv, ref maxTripTime, ref nPointersNow);
                            CheckIfError(errorCode);

                            // Pointer to the solution status code
                            errorCode = lingo.LSsetPointerLng(_pLingoEnv, ref dStatus, ref nPointersNow);
                            CheckIfError(errorCode);

                            // Total trip time
                            double totalTripTime = -1;
                            errorCode = lingo.LSsetPointerLng(_pLingoEnv, ref totalTripTime, ref nPointersNow);
                            CheckIfError(errorCode);

                            // Here is the script we want LINGO to run. 
                            string cScript = string.Format("set echoin 1 \n take {0} \n go \n quit \n", _modelPath.FullName);

                            // Run the script
                            errorCode = lingo.LSexecuteScriptLng(_pLingoEnv, cScript);
                            CheckIfError(errorCode);
                                                        
                            // Any problems?
                            if (errorCode != 0)
                                throw new FaildToSolveOptimizationException();

                            //hittade vi en lösning?
                            if (dStatus == lingo.LS_STATUS_GLOBAL_LNG || dStatus == lingo.LS_STATUS_LOCAL_LNG)
                            {
                                //get results
                                FetchResults(transportUnits);

                                //set max trip time to force another solution
                                maxTripTime = totalTripTime;
                            }
                        }
                    }
                }
                finally
                {
                    lingo.LScloseLogFileLng(_pLingoEnv);
                    lingo.LSclearPointersLng(_pLingoEnv);
                    lingo.LSdeleteEnvLng(_pLingoEnv);

                    System.Threading.Monitor.Exit(this);
                }
            }
        }

        private void FetchResults(IEnumerable<TransportUnit> transportUnits)
        {            
            IEnumerable<OutFileRow> results = ParseResultFile();
            IEnumerable<ShortageRow> shortages = ParseShortageFile();

            foreach (var unit in transportUnits)
            {
                TransportUnit transportUnit = unit;

                var rows = results.Where(row => row.Origin == transportUnit.Origin.Id && row.Destination == transportUnit.Destination.Id);
                var usedLegs = rows.Select(row => _legs.Where(l => l.Id == row.LegId).Single()).ToList();

                if (usedLegs.Count == 0)
                    return;

                double shortage = 0;

                foreach (ShortageRow row in shortages.Where(row => row.Origin == transportUnit.Origin.Id && row.Destination == transportUnit.Destination.Id))
                {
                    shortage = row.Shortage;
                }

                var route = new Route(unit, shortage, usedLegs);

                unit.ProposedRoutes.Add(route);
            }  
        }

        private IEnumerable<OutFileRow> ParseResultFile()
        {
            var result = new List<OutFileRow>();
            var resultFilePath = Path.Combine(_inputFileFolder.FullName, "logistikcenter_result.txt");

            using (var reader = new StreamReader(resultFilePath))
            {
                string line = reader.ReadLine();
                while (line != null)
                {

                    string[] columns = line.Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

                    if (columns[6].Substring(0, 1) == "1")
                    {
                        var row = new OutFileRow
                                      {
                                          Origin = long.Parse(columns[4]),
                                          Destination = long.Parse(columns[5]),
                                          LegId = long.Parse(columns[2])
                                      };

                        result.Add(row);
                    }

                    line = reader.ReadLine();
                }
            }
            return result;
        }

        private IEnumerable<ShortageRow> ParseShortageFile()
        {
            var result = new List<ShortageRow>();
            var shortageFilePath = Path.Combine(_inputFileFolder.FullName, "logistikcenter_shortage.txt");

            using (var reader = new StreamReader(shortageFilePath))
            {
                string line = reader.ReadLine();
                while (line != null)
                {
                    string[] columns = line.Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

                    var row = new ShortageRow
                                  {
                                      Origin = long.Parse(columns[1]),
                                      Destination = long.Parse(columns[2]),
                                      Shortage = double.Parse(columns[3].Replace(".", ","))
                                  };

                    result.Add(row);
                    line = reader.ReadLine();
                }
            }

            return result;
        }
    }

    public class FaildToSolveOptimizationException : LingoException {}
    public class LingoException : Exception {}
    public class UnableToCreateLingoEnvironmentException : Exception {}
}