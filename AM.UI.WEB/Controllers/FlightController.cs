using AM.ApplicationCore.Domain;
using AM.ApplicationCore.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Numerics;
using AM.Infrastructure;
using AM.ApplicationCore.Services;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace AM.UI.WEB.Controllers
{
    public class FlightController : Controller
    {

        private readonly IServiceFlight _flightService;
        private readonly IServicePlane _planeService;
 

        public FlightController(IServiceFlight flightService,IServicePlane iServicePlane)
        {
            _flightService = flightService;
            _planeService = iServicePlane;
        }



        // GET: FlightController
        public ActionResult Index(DateTime? datedep)
        {
            /*if (datedep != null)
            {
                var datedepart = ((DateTime)datedep).ToShortDateString();
                return View(_flightService.getFlightByDate(datedepart.ToString()).ToList());
            }
            else*/
            return View(_flightService.GetAll().ToList());
        }

        // GET: FlightController/Details/5
        public ActionResult Details(int id)
        {
            var flight = _flightService.GetById((int)id);
            if (flight == null)
            {
                return NotFound();
            }
            return View(flight);
        }

        // GET: FlightController/Create
        public ActionResult Create()
        {
            var planelist = new List<AM.ApplicationCore.Domain.Plane>();
            planelist = _planeService.GetAll().ToList();
            ViewBag.Plane = new SelectList(planelist, "PlaneId", "PlaneType");
            return View();
        }

        // POST: FlightController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Flight flight,IFormFile airlineImage)
        {
            if (airlineImage !=null)
            {
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", airlineImage.FileName);
                Stream stream = new FileStream(path, FileMode.Create);
                airlineImage.CopyTo(stream);
                flight.Airline = airlineImage.FileName;
            }
            try
            {
                _flightService.Add(flight);
                _flightService.Commit();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }

 
        }

        // GET: FlightController/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var flight = _flightService.GetById((int)id);
            if (flight == null)
            {
                return NotFound();
            }
            var planelist = new List<AM.ApplicationCore.Domain.Plane>();
            planelist = _planeService.GetAll().ToList();
            ViewBag.Plane = new SelectList(planelist, "PlaneId", "PlaneType");
            return View(flight);

        }

        // POST: FlightController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            var flight = _flightService.GetById((int)id);

            try
            {

                _flightService.Update(flight);
                _flightService.Commit();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: FlightController/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var flight = _flightService.GetById((int)id);
            if (flight == null)
            {
                return NotFound();
            }

            return View(flight);
        }

        // POST: FlightController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                var flight = _flightService.GetById((int)id);
                _flightService.Delete(flight);
                _flightService.Commit();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
