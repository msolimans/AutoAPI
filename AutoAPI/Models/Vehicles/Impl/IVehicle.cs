using System.Collections.Generic;
using System.Threading.Tasks;
using AutoAPI.Models.Vehicles.Model;
using AutoAPI.Models.Vehicles.ViewModel;

namespace AutoAPI.Models.Vehicles.Impl
{
    //contract for vehicle operations
    public interface IVehicle
    { 
        Task<bool>  Save(Vehicle vehicle);
        Task<Vehicle> GetById(string id);
        Task<bool> Exists(string id);
        Task<IEnumerable<Vehicle>> GetAll();
        Task<IEnumerable<Vehicle>> GetByCriteria(VehicleSearchCriteria criteria);
        Task<bool> DeleteById(string id);
        Task<long> Count();
    }
}