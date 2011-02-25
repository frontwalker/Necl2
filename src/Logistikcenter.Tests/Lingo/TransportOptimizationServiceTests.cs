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
        private readonly ITransportOptimizationService _transportOptimizationService;
        private readonly FakeLegRepository _repository;
        private IList<TransportUnit> _transportUnits;

        public TransportOptimizationServiceTests()
        {
            _repository = new FakeLegRepository();
            _repository.Add(new Leg("101", new ShippingAgent(), CarrierType.Truck, new Destination("Sundsvall"), new Destination("Umeå"), new DateTime(2011, 01, 02), new DateTime(2011, 01, 31), 65, 11));

            _transportOptimizationService = new LingoTransportOptimizationService(_repository, @"C:\Dev\Logistikcenter.Mvc\src\Logistikcenter.Services\Lingo\Models\prototypCost.lng", @"c:\temp\tests", @"c:\temp\tests\logs");
            _transportUnits = new List<TransportUnit>();
        }

        [Fact]
        public void MyTest()
        {
            _transportOptimizationService.LoadData(new DateTime(2011,01,01), new DateTime(2011,12,31));
            _transportOptimizationService.MinimizeCost(_transportUnits, 3);
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
    }
}
