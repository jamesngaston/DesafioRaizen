using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DesafioRaizen.Data;
using DesafioRaizen.Models;
using DesafioRaizen.DTO;
using DesafioRaizen.Services.Interfaces;
using DesafioRaizen.Services;
using DesafioRaizen.Filters;

namespace DesafioRaizen.Controllers
{
    public class CustomerController : Controller
    {
        private readonly ICustomerService _service;


        public CustomerController(ICustomerService service)
        {
            _service = service;
        }

        // GET: Customer
        public async Task<IActionResult> Index()
        {
            CustomerFilter filter = new CustomerFilter();
            List<CustomerDto> model = new();
            (await _service.GetPaged(filter, null)).ForEach(c => model.Add(c));
            return View(model);
        }

        // GET: Customer/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _service.Get(id.Value);
            if (customer == null)
            {
                return NotFound();
            }
            CustomerDto dto = customer;
            return View(dto);
        }

        // GET: Customer/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Customer/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Email,BirthDate,CEP")] CustomerDto customerDto)
        {
            if (ModelState.IsValid)
            {
                await _service.Add(customerDto);
                return RedirectToAction(nameof(Index));
            }
            return View(customerDto);
        }

        // GET: Customer/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _service.Get(id.Value);
            if (customer == null)
            {
                return NotFound();
            }
            return View((CustomerDto)customer);
        }

        // POST: Customer/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Email,BirthDate,CEP")] CustomerDto customerDto)
        {
            if (id != customerDto.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _service.Update(id, customerDto);
                }
                catch
                {
                    return NotFound();
                }
                return RedirectToAction(nameof(Index));
            }
            return View(customerDto);
        }

        // GET: Customer/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            CustomerDto customer = await _service.Get(id.Value);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // POST: Customer/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var customer = await _service.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}