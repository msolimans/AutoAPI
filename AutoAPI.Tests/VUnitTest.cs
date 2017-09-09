using System;
using System.Collections.Generic;
using System.Linq;
using AutoAPI.Controllers;
using AutoAPI.Controllers.APIs;
using AutoAPI.Infrastructure.Caching;
using AutoAPI.Models.Context;
using AutoAPI.Models.Vehicles.Impl;
using AutoAPI.Models.Vehicles.Model;
using AutoAPI.Models.Vehicles.ViewModel;
using Xunit;

namespace AutoAPI.Tests
{
    public class VUnitTest
    {
        private ICache _cache;
        private IVehicle _vehicle;
        
        public VUnitTest(ICache cache, IVehicle vehicle)
        {
            _cache = cache;
            _vehicle = vehicle;
        }
        
      
        
        
        [Fact]
        public void Get_ShouldReturnNotReturnAnythingVehicles()
        {
          VehiclesController vc = new VehiclesController(_vehicle, _cache);
            var vcs = vc.Get(new VehicleSearchCriteria()
            {
                Year = 2017
            }).GetAwaiter().GetResult();
            
            Assert.InRange(vcs.Count(), 1,10);
        }
    }
}