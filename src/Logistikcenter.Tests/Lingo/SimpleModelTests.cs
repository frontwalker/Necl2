using Logistikcenter.Services.Lingo;
using Xunit;

namespace Logistikcenter.Tests.Lingo
{
    public class SimpleModelTests
    {
        private const string ModelPath = @"C:\Dev\Logistikcenter.Mvc\src\Logistikcenter.Tests\Lingo\Simple\SIMPLE.lng";

        readonly int pLingoEnv;
        int nError = -1, nPointersNow = -1;
        double dObjective = -1, dStatus = -1;

        [Fact]
        public void LingoShouldBeAbleToSolveTheSimpleModel()
        {
            Assert.True(Solve() != "Unable to solve!");            
        }

        public SimpleModelTests()
        {
            pLingoEnv = lingo.LScreateEnvLng();
            lingo.LSopenLogFileLng(pLingoEnv, @"c:\temp\lingo.log");           
        }

        public string Solve()
        {
                        
            lingo.LSclearPointersLng(pLingoEnv);

            unsafe
            {
                fixed (

                    // Model data that gets referenced by the
                    // template model, simple.lng
                    double* dProfit = new double[2],
                    dLimit = new double[2],
                    dLabor = new double[2],
                    dProduce = new double[2])
                {

                    dProfit[0] = 100; dProfit[1] = 150;
                    dLimit[0] = 100; dLimit[1] = 200;
                    dLabor[0] = 1; dLabor[1] = 2;

                    // Pass Lingo the pointer to the objective coefficients (refer
                    // to the template model, simple.lng)
                    nError = lingo.LSsetPointerLng(pLingoEnv, ref dProfit[0], ref nPointersNow);

                    // Pass a pointer to the production limits
                    nError = lingo.LSsetPointerLng(pLingoEnv, ref dLimit[0], ref nPointersNow);

                    // Pointer to the labor utilization coefficients
                    nError = lingo.LSsetPointerLng(pLingoEnv, ref dLabor[0], ref nPointersNow);

                    // Point to dObjective, where Lingo will return the objective value
                    nError = lingo.LSsetPointerLng(pLingoEnv, ref dObjective, ref nPointersNow);

                    // Pointer to the solution status code
                    nError = lingo.LSsetPointerLng(pLingoEnv, ref dStatus, ref nPointersNow);

                    // Point to the variable value array
                    nError = lingo.LSsetPointerLng(pLingoEnv, ref dProduce[0], ref nPointersNow);

                    // Here is the script we want LINGO to run. 
                    string cScript = "set echoin 1 \n take "+ ModelPath +" \n go \n quit \n";

                    // Run the script
                    nError = lingo.LSexecuteScriptLng(pLingoEnv, cScript);
                    
                    // Close the log file
                    lingo.LScloseLogFileLng(pLingoEnv);

                    // Close LingoEnv
                    lingo.LSclearPointersLng(pLingoEnv);
                    lingo.LSdeleteEnvLng(pLingoEnv);

                    // Any problems?
                    if (nError != 0 || dStatus != lingo.LS_STATUS_GLOBAL_LNG)
                    {
                        return "Unable to solve!";
                    }
                    else
                    {                        
                        var result = string.Format("\nStandards: {0} \nTurbos: {1} \n\nProfit: {2} \n",dProduce[0], dProduce[1], dObjective);
                        return result;
                    }
                }
            }            
        }
    }
}