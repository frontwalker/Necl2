using System;
using Logistikcenter.Services.Lingo;

namespace Logistikcenter.Tests.Lingo
{
    public abstract class LingoEngine
    {
        private readonly int pLingoEnv;

        protected LingoEngine()
        {
            pLingoEnv = lingo.LScreateEnvLng();
            lingo.LSopenLogFileLng(pLingoEnv, @"c:\temp\lingo.log");      
        }
    }


    public class PrototypCostTests
    {
        private const string ModelPath = @"C:\Dev\Logistikcenter.Mvc\src\Logistikcenter.Tests\Lingo\PrototypCost\prototypCost.lng";
        readonly int pLingoEnv;

        public PrototypCostTests()
        {
            pLingoEnv = lingo.LScreateEnvLng();
            lingo.LSopenLogFileLng(pLingoEnv, @"c:\temp\lingo.prototypCost.log");      
        }

        public void Solve()
        {
            try
            {
                lingo.LSclearPointersLng(pLingoEnv);





                string cScript = string.Format("set echoin 1 \n take {0} \n go \n quit \n", ModelPath);
                lingo.LSexecuteScriptLng(pLingoEnv, cScript);
            }
            catch (Exception)
            {
                
                throw;
            }
            finally
            {
                lingo.LScloseLogFileLng(pLingoEnv);
                lingo.LSclearPointersLng(pLingoEnv);
                lingo.LSdeleteEnvLng(pLingoEnv);                
            }            
        }
    }
}