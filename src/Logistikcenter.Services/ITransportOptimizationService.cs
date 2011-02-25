using System;
using System.Collections.Generic;
using Logistikcenter.Domain;

namespace Logistikcenter.Services
{
    //public interface ITransportOptimizationService
    //{
    //    void LoadData(IList<Leg> legs);
    //    bool IsInitialized();
    //    void MinimizeCost(IList<TransportUnit> transportUnits, int maxNumberOfSolutions);
    //    void MinimizeCost(TransportUnit transportUnit, int maxNumberOfSolutions);
    //    void MinimizeCost(TransportRequest reguest, int maxNumberOfSolutions);        
    //}

    public interface ITransportOptimizationService
    {
        void LoadData(DateTime minStartTime, DateTime maxEndTime);
        void MinimizeCost(IEnumerable<TransportUnit> transportUnits, int maxNumberOfSolutions);
    }
}
