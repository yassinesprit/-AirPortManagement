using AM.ApplicationCore.Domain;
using AM.ApplicationCore.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Net.Sockets;

namespace AM.UI.WEB.Controllers
{
public class TicketController : Controller
    {

        private readonly IServiceTicket _serviceTicket;
        private readonly IServiceFlight _serviceFlight;
        private readonly IServicePassenger _servicePassenger;

        public TicketController(IServiceTicket serviceTicket,IServicePassenger servicePassenger,IServiceFlight serviceFlight)
        {
            _serviceTicket = serviceTicket;
            _serviceFlight = serviceFlight;
            _servicePassenger = servicePassenger;
        }


        // GET: TicketController
        public ActionResult Index()
        {
            return View(_serviceTicket.GetAll().ToList());
        }

        // GET: TicketController/Details/5
        public ActionResult Details(int id,int flightId,int passengerId)
        {
            var ticket = _serviceTicket.GetById((int)id);
            if (ticket == null)
            {
                return NotFound();
            }
            return View(ticket);
        }

        // GET: TicketController/Create
        public ActionResult Create()
        {

            var passengerlist = _servicePassenger.GetAll().ToList();
            ViewBag.Passenger = new SelectList(passengerlist, "Id", "PassportNumber");
            var flightlist = _serviceFlight.GetAll().ToList();
            ViewBag.Flight = new SelectList(flightlist, "FlightId", "FlightId");
            return View();
        }

        // POST: TicketController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Ticket ticket)
        {
            try
            {
                _serviceTicket.Add(ticket);
                _serviceTicket.Commit();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: TicketController/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticket = _serviceTicket.GetById((int)id);
            if (ticket == null)
            {
                return NotFound();
            }
            return View(ticket);
        }

        // POST: TicketController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            var ticket = _serviceTicket.GetById((int)id);

            try
            {
                _serviceTicket.Add(ticket);
                _serviceTicket.Commit();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: TicketController/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticket = _serviceTicket.GetById((int)id);
            if (ticket == null)
            {
                return NotFound();
            }
            return View(ticket);
        }

        // POST: TicketController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            var ticket = _serviceTicket.GetById((int)id);

            try
            {
                _serviceTicket.Delete(ticket);
                _serviceTicket.Commit();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
