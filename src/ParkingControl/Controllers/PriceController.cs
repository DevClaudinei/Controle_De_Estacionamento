using Application.Models.Request.Price;
using AppServices.Services.Interfaces;
using DomainServices.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace ParkingControl.Controllers
{
    [Authorize]
    public class PriceController : Controller
    {
        private readonly IPriceAppService _priceAppService;

        public PriceController(IPriceAppService priceAppService)
        {
            _priceAppService = priceAppService ?? throw new ArgumentNullException(nameof(priceAppService));
        }

        // GET: Price
        public IActionResult Index()
        {
            return View(_priceAppService.GetAll());
        }

        // GET: Price/Details/5
        public async Task<IActionResult> Details(long id)
        {
            try
            {
                return View(await _priceAppService.GetById(id));
            }
            catch (NotFoundException e)
            {
                return View("Error404", e);
            }
        }

        // GET: Price/Details/
        public async Task<IActionResult> DetailsByValidity(DateTime departureTime)
        {
            try
            {
                return View(await _priceAppService.GetPriceByValidity(departureTime));
            }
            catch (NotFoundException e)
            {
                return View("Error404", e);
            }
        }

        // GET: Price/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Price/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreatePriceRequest createPriceRequest)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var priceId = await _priceAppService.CreatePrice(createPriceRequest);
                }
                catch (BadRequestException e)
                {
                    return View("Error500", e);
                }

                RedirectToAction(nameof(Index));
            }

            return View(createPriceRequest);
        }

        // GET: Price/Edit/5
        public async Task<IActionResult> Edit(long id)
        {
            try
            {
                var priceToUpdate = await _priceAppService.Get(id);

                return View(priceToUpdate);
            }
            catch (NotFoundException e)
            {
                return View("Error404", e);
            }
        }

        // POST: Price/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(long id, UpdatePriceRequest updatePriceRequest)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _priceAppService.UpdatePrice(id, updatePriceRequest);
                }
                catch (NotFoundException e)
                {
                    return View("Error404", e);
                }
                return RedirectToAction(nameof(Index));
            }
            return View(updatePriceRequest);
        }

        // GET: Price/Delete/5
        public async Task<IActionResult> Delete(long id)
        {
            try
            {
                var priceToExclude = await _priceAppService.GetById(id);

                return View(priceToExclude);
            }
            catch (BadRequestException e)
            {
                return View("Error404", e);
            }
        }

        // POST: Price/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(long id)
        {
            try
            {
                _priceAppService.ExcludePrice(id);
            }
            catch (NotFoundException e)
            {
                return View("Error404", e);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
