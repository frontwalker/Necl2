using System;
using System.Collections.Generic;
using System.Linq;
using Logistikcenter.Domain;
using Logistikcenter.Services;
using Logistikcenter.Services.Lingo;
using Xunit;

namespace Logistikcenter.Tests.Lingo
{    
    public class TransportOptimizationServiceTests
    {
        private readonly LingoTransportOptimizationServiceForTest _transportOptimizationService;
        private readonly FakeLegRepository _repository;
        private IList<TransportUnit> _transportUnits;

        public TransportOptimizationServiceTests()
        {
            _repository = new FakeLegRepository();

            var legs = new List<Leg>
                           {
                               new Leg("4", new ShippingAgent(), CarrierType.Truck, new Destination(2, "Sundsvall"),
                                       new Destination(3, "Stockholm"), new DateTime(2010, 12, 01, 8, 0, 0),
                                       new DateTime(2010, 12, 01, 15, 0, 0), 50, 100),
                               new Leg("103", new ShippingAgent(), CarrierType.Rail, new Destination(2, "Sundsvall"),
                                       new Destination(3, "Stockholm"), new DateTime(2011, 04, 01, 2, 0, 0),
                                       new DateTime(2011, 04, 01, 8, 0, 0), 120, 10),
                               new Leg("104", new ShippingAgent(), CarrierType.Rail, new Destination(2, "Sundsvall"),
                                       new Destination(3, "Stockholm"), new DateTime(2011, 04, 01, 12, 0, 0),
                                       new DateTime(2011, 04, 01, 18, 0, 0), 125, 16),
                               new Leg("105", new ShippingAgent(), CarrierType.Truck, new Destination(2, "Sundsvall"),
                                       new Destination(3, "Stockholm"), new DateTime(2011, 04, 01, 10, 0, 0),
                                       new DateTime(2011, 04, 01, 16, 0, 0), 135, 10),
                               new Leg("106", new ShippingAgent(), CarrierType.Truck, new Destination(2, "Sundsvall"),
                                       new Destination(3, "Stockholm"), new DateTime(2011, 04, 01, 14, 0, 0),
                                       new DateTime(2011, 04, 01, 20, 0, 0), 145, 20)
                           };
            
            legs.ForEach(l => _repository.Add(l));

            _transportOptimizationService = new LingoTransportOptimizationServiceForTest(_repository, @"C:\Dev\Necl2\src\Logistikcenter.Services\Lingo\Models\prototypCost.lng", @"c:\temp\tests", @"c:\temp\tests\logs");
            _transportOptimizationService.SetCurrentDateTime(new DateTime(2011,02,25,14,34,0));

            _transportUnits = new List<TransportUnit>
                                  {
                                      new TransportUnit(new Destination(2, "Sundsvall"), new Destination(2, "Stockholm"),
                                                        new DateTime(2011, 04, 01), new DateTime(2011, 04, 01),
                                                        new CargoDefinition(10))
                                  };
        }

        [Fact]
        public void MyTest()
        {
            _transportOptimizationService.LoadData(new DateTime(2011,01,01), new DateTime(2011,12,31));
            _transportOptimizationService.MinimizeCost(_transportUnits, 3);

            Assert.True(_transportUnits[0].ProposedRoutes.Count == 0);
        }
    }

    public class LingoTransportOptimizationServiceForTest : LingoTransportOptimizationService
    {
        public LingoTransportOptimizationServiceForTest(IRepository repository, string modelPath, string inputFileFolder, string logFileFolder) : base(repository, modelPath, inputFileFolder, logFileFolder)
        {
        }

        private DateTime _currentDateTime;

        public void SetCurrentDateTime(DateTime dateTime)
        {
            _currentDateTime = dateTime;
        }

        public override DateTime Now()
        {
            return _currentDateTime;
        }
    }

    public class FakeLegRepository : IRepository
    {
        private readonly IList<Leg> _data = new List<Leg>();

        public void Add(Leg leg)
        {
            _data.Add(leg);
        }
                
        public IQueryable<T> Query<T>()
        {
            return (IQueryable<T>) _data.AsQueryable();            
        }

        public void Save(object entity)
        {
            throw new NotImplementedException();
        }

        public void Update(object entity)
        {
            throw new NotImplementedException();
        }

        public void Delete<T>(long id)
        {
            throw new NotImplementedException();
        }

        public void DeleteById<T>(long id)
        {
            throw new NotImplementedException();
        }

        public void Delete(object entity)
        {
            throw new NotImplementedException();
        }
    }
}
