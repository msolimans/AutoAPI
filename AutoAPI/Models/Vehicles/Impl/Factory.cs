using System.Collections.Generic;
using System.Threading.Tasks;
using AutoAPI.Models.Vehicles.Impl;
using AutoAPI.Models.Vehicles.Model;

namespace AutoAPI.Models
{
    //manual factory implementation for hooking up specific strategy to be used 
    //I did not use it though, as I used built in dependency injection in dotnet core.
    public class Factory : IVehicle
    {
        private IVehicle _vehicle;

        //strategy, which type of vehicle you want to save 
        void SetVehicle(IVehicle vehicle)
        {
            _vehicle = vehicle;
        }

        public Task<bool> Save(Vehicle vehicle)
        {
            return _vehicle.Save(vehicle);
        }

        public Task<Vehicle> GetById(string id)
        {
            return _vehicle.GetById(id);
        }

        public Task<bool> Exists(string id)
        {
            return _vehicle.Exists(id);
        }

        public Task<IEnumerable<Vehicle>> GetAll()
        {
            return _vehicle.GetAll();
        }

        public Task<bool> DeleteById(string id)
        {
            return _vehicle.DeleteById(id);
        }

        public Task<long> Count()
        {
            return _vehicle.Count();
        }
    }
}