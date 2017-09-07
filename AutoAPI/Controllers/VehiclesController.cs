using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoAPI.Models;
using AutoAPI.Models.Context;
using AutoAPI.Models.Vehicles.Impl;
using AutoAPI.Models.Vehicles.Model;
using Microsoft.AspNetCore.Mvc;

namespace AutoAPI.Controllers
{
   
    [Route("[controller]")]
    public class VehiclesController : Controller
    {
        private readonly IVehicle _vehicleService;
        
        public VehiclesController(IVehicle vehicleService)
        {
            _vehicleService = vehicleService;
        }
        
        // GET /vehicles
        [HttpGet]
        public async Task<IEnumerable<Vehicle>> Get()
        {
            //return new string[] {"value1", "value2"};
            
            //HATOES can be used here to add location for each item (it is kind of details for the resource)
            return await _vehicleService.GetAll();

        }

        // GET /vehicles/5xxxx
        [HttpGet("{id}")]
        public async Task<ActionResult> Get(string id)
        {
            
            var v = await _vehicleService.GetById(id);
            if (v == null)
            {
                Response.StatusCode = 404; //not found
                return Json(new {success = false, message = "NOT FOUND"});
            }

            Response.StatusCode = 200;//OK, by default 

            return Json(new {success = true, result = v, message = "Returned Successfully"});
        }
        

        // POST /vehicles
        [HttpPost]
        public  async Task<IActionResult> Post(Vehicle  vehicle)
        {
            if (await _vehicleService.Save(vehicle))
            {
                Response.StatusCode = 201;
                //HATOES is used here to identify the location of new and created resources s 
                return Json(new {success = true, message = "Successfully Created", location = Url.Action("Get", new {id = vehicle.Id})});
            }
            
            Response.StatusCode = 500;
            return Json(new {success = false, message = "Error occured while saving into database"});
        }

        // PUT /vehicles (updates)
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(string id,  Vehicle vehicle)
        {
            if (!(await _vehicleService.Exists(id)))
            {
                Response.StatusCode = 404; //not found 
                return Json(new {success = false, message = "Vehicle doesn't exist"});
            }
                
            
            
            _vehicleService.Save(vehicle);

            Response.StatusCode = 200;//204 (No Content) could be also used
            //HATOES is used here to identify the location of new and created resources s 
            return Json(new {success = true, message = "Updated Successfully", location = Url.Action("Get", new {id = vehicle.Id})});

            
        }

        // DELETE /vehicles/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(string  id)
        {
            try
            {
                if (await _vehicleService.DeleteById(id))
                {
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