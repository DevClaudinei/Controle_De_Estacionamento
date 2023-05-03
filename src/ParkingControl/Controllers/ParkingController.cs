using Application.Models.Request.Parking;
using AppServices.Services.Interfaces;
using DomainServices.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace ParkingControl.Controllers
{
    [Authorize]
    public class ParkingController : Controller
    {
        private readonly IParkingAppService _parkingAppService;

        public ParkingController(IParkingAppService parkingAppService)
        {
            _parkingAppService = parkingAppService ?? throw new ArgumentNullException(nameof(parkingAppService));
        }

        // GET: Parking
        public IActionResult Index()
        {
            return View(_parkingAppService.GetAll());
        }

        // GET: Parking/Details/5
        public async Task<IActionResult> Details(long id)
        {
            try
            {
                return View(await _parkingAppService.Get(id));
            }
            catch (NotFoundException e)
            {
                return View("Error404", e);
            }
        }

        public async Task<IActionResult> GetByPlate(string plate)
        {
            try
            {
                return View(await _parkingAppService.GetByPlate(plate));
            }
            catch (NotFoundException e)
            {
                return View("Error404", e);
            }
        }

        // GET: Parking/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Parking/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CheckInRequest checkInRequest)
        {
            if (ModelState.IsValid)
            {
                try
                {
                   var parkingId = await _parkingAppService.CheckInParking(checkInRequest);
                }
                catch (BadRequestException e)
                {
                    return View("Error500", e);
                }

                return RedirectToAction(nameof(Index));
            }

            return View(checkInRequest);
        }

        // GET: Parking/Edit/5
        public async Task<IActionResult> Edit(long id)
        {
            try
            {
                var parkingToCheckOut = await _parkingAppService.GetById(id);

                return View(parkingToCheckOut);
            }
            catch (NotFoundException e)
            {
                return View("Error404", e);
            }
        }

        // POST: Parking/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(long id, CheckOutRequest checkOutRequest)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _parkingAppService.CheckOutParking(id, checkOutRequest);
                }
                catch (BadRequestException e)
                {
                    return View("Error500", e);
                }
                return RedirectToAction(nameof(Index));
            }
            return View(checkOutRequest);
        }

        // GET: Parking/Delete/5
        public async Task<IActionResult> Delete(long id)
        {
            try
            {
                var parkingToExclude = await _parkingAppService.Get(id);

                return View(parkingToExclude);
            }
            catch (NotFoundException e)
            {
                return View("Error404", e);
            }
        }

        // POST: Parking/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(long id)
        {
            try
            {
                _parkingAppService.ExcludeParkingInfo(id);
            }
            catch (BadRequestException e)
            {
                return View("Error500", e);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
