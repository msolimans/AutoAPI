using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoAPI.Infrastructure;
using AutoAPI.Infrastructure.Caching;
using AutoAPI.Infrastructure.Configurations;
using AutoAPI.Models;
using AutoAPI.Models.Context;
using AutoAPI.Models.Vehicles.Impl;
using AutoAPI.Models.Vehicles.Model;
using AutoAPI.Models.Vehicles.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace AutoAPI.Controllers.APIs
{
    //we can separate everything under "Areas" where it should hold views, controllers, models, viewmodels for every subject 
    [Route("[controller]")]
    public class VehiclesController : Controller
    {
        private readonly IVehicle _vehicleService;
        private readonly ICache _cache;
        private AppSettings _appSettings;

        public VehiclesController(IVehicle vehicleService, ICache cache, IOptions<AppSettings> appSettings)
        {
            _vehicleService = vehicleService;
            _cache = cache;
            _appSettings = appSettings.Value;
        }
        

        // GET /vehicles
        [HttpGet]
        public async Task<IEnumerable<Vehicle>> Get(VehicleSearchCriteria criteria)
        {
            //HATOES can be used here to add location for each item (it is kind of details for the resource)
            return await _vehicleService.GetByCriteria(criteria);
        }

        // GET /vehicles/5xxxx
        [HttpGet("{id}")]
        public async Task<ActionResult> Get(string id)
        {
            //check if data exists into memory first, if so retrieve it 
            var v = _cache.Get(id);
            if (v != null)
            {
                Response.Headers["x-auto-cache"] = "hit";
                return Json(new {success = true, message = "Returned Successfully", result = v});
            }


            v = await _vehicleService.GetById(id);
            if (v == null)
            {
                Response.StatusCode = 404; //not found
                return Json(new {success = false, message = "NOT FOUND"});
            }

            //cache data into memory for future use.
            _cache.Store(id, v);

            Response.StatusCode = 200; //OK, by default
            Response.Headers["x-auto-cache"] = "miss";
            return Json(new {success = true,  message = "Returned Successfully", result = v});
        }


        // POST /vehicles
        [HttpPost]
        public async Task<IActionResult> Post(Vehicle vehicle)
        {
            if (await _vehicleService.Save(vehicle))
            {
                Response.StatusCode = 201;
                //HATOES is used here to identify the location of new and created resources s 
                return Json(new
                {
                    success = true,
                    message = "Successfully Created",
                    location = Url.Action("Get", new {id = vehicle.Id})
                });
            }

            Response.StatusCode = 500;
            return Json(new {success = false, message = "Error occured while saving into database"});
        }

        // PUT /vehicles (updates)
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(string id, Vehicle vehicle)
        {
            if (!(await _vehicleService.Exists(id)))
            {
                Response.StatusCode = 404; //not found 
                return Json(new {success = false, message = "Vehicle doesn't exist"});
            }


            _vehicleService.Save(vehicle);

            Response.StatusCode = 200; //204 (No Content) could be also used
            //HATOES is used here to identify the location of new and created resources s 
            return Json(new
            {
                success = true,
                message = "Updated Successfully",
                location = Url.Action("Get", new {id = vehicle.Id})
            });
        }

        // DELETE /vehicles/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(string id)
        {
            try
            {
                //delete from db and cache if exists 
                if (await _vehicleService.DeleteById(id) )
                {
                     _cache.Remove(id);
                        
                    //accepted response code, also 200 (OK) code be used too 
                    Response.StatusCode = 202;
                    return Json(new {success = true, message = "Deleted Successfully"});
                }
                else
                {
                    Response.StatusCode = 500;
                    return Json(new
                    {
                        success = false,
                        message = "Error occured while deleting vehicle, plz try again later "
                    });
                }
            }
            catch (KeyNotFoundException)
            {
                Response.StatusCode = 404;
                return Json(new {success = false, message = "Not Found!"});
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                return Json(new {success = false, message = "Error!" + ex.Message});
            }
        }
    }
}