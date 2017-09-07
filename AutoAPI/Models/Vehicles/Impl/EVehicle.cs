using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoAPI.Controllers;
using AutoAPI.Models.Context;
using AutoAPI.Models.Vehicles.Model;
using AutoAPI.Models.Vehicles.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AutoAPI.Models.Vehicles.Impl
{
    //Entity Framework-based vehicle operations
    public class EVehicle : IVehicle
    {
        private EContext _context;

        public EVehicle(EContext context)
        {
            _context = context;
        }

       

        public async Task<bool> Save(Vehicle vehicle)
        {
            try
            {
         
                _context.Entry(vehicle).State = string.IsNullOrEmpty(vehicle.Id) ? EntityState.Added : EntityState.Modified;
                _context.Vehicles.Add(vehicle);
                
                int affected = await _context.SaveChangesAsync();
                if (affected > 0)
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                //Exception (ex) should be logged if required .. 
                //Type of exceptions can be also identified and proper reaction is gonna be handled according to requirements
            }

            return false;
        }

        public async Task<long> Count()
        {
            return await _context.Vehicles.CountAsync();
        }

        public async Task<Vehicle> GetById(string id)
        {
            return await _context.Vehicles.Where(v => v.Id == id).FirstOrDefaultAsync();
        }

        public async Task<bool> Exists(string id)
        {
            return await _context.Vehicles.AnyAsync(v => v.Id == id);
        }

        public async Task<IEnumerable<Vehicle>> GetAll()
        {
            return await _context.Vehicles.ToListAsync();
        }

       

        //only added for entity frameowrk here 
        public async Task<IEnumerable<Vehicle>> GetByCriteria(VehicleSearchCriteria vehicle)
        {

            return await _context.Vehicles.Where(v => 
                                                 (vehicle == null || string.IsNullOrEmpty(vehicle.Id) || v.Id == vehicle.Id) && 
                                                 (vehicle == null || string.IsNullOrWhiteSpace(vehicle.Make) ||  v.Make == vehicle.Make) &&
                                                 (vehicle == null || string.IsNullOrWhiteSpace(vehicle.Model) || v.Model == vehicle.Model) && 
                                                 (vehicle == null || !vehicle.Year.HasValue || v.Year.Value == vehicle.Year.Value)).ToListAsync();
            
           
        }


        public async Task<bool> DeleteById(string id)
        {
            try
            {
                var vehicle = await _context.Vehicles.Where(v => v.Id == id).FirstOrDefaultAsync();
                if (vehicle == null)
                    throw new KeyNotFoundException("vehicle not found");
                
                _context.Vehicles.Remove(vehicle);
                int affected = _context.SaveChanges();
                if(affected <= 0)
                    return false;
            }
            catch (Exception ex)
            {    
                //Exception should be logged somewhere
                throw;
            }

            return true;
        }
    }
}