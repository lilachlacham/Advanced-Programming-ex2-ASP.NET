﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ex2.Data;
using ex2.Models;
using ex2.Services;

namespace ex2.Controllers
{
    public class RatesController : Controller
    {
        private IRateService service;
        private float currentRate;

        public RatesController() {
            service = new RateService();
        }

        // GET: Rates
        public IActionResult Index() {
            List<Rate> rates = service.GetAll();
            float rate = 0;
            int numOfRates = rates.Count;
            
            for (int i = 0; i<numOfRates; i++)
            {
                rate += rates[i].Score;
            }
            if(numOfRates == 0)
            {
                rate = 0;
            }
            else
            {
                rate = rate / numOfRates;
            } 
            ViewData["Message"] = rate;
            currentRate = rate;
            return View(rates);
        }
        [HttpPost]
        public IActionResult Index(string query)
        {
            List<Rate> rates = service.GetAll();
            if (rates == null)
            {
                ViewData["Message"] = currentRate;
                return View(rates);
            }
            List<Rate> filterRates = new List<Rate>();
            for (int i = 0; i<rates.Count; i++)
            {
                if (rates[i].Text.Contains(query))
                {
                    filterRates.Add(rates[i]);
                }
            }
            ViewData["Message"] = currentRate;
            return View(filterRates);
        }

        // GET: Rates/Details/5
        public IActionResult Details(int id)
        {
            return View(service.Get(id));
        }

        // GET: Rates/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Rates/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(int score, string text)
        {
            service.Add(text, score);
            return RedirectToAction(nameof(Index));

        }

        // GET: Rates/Edit/5
        public IActionResult Edit(int id)
        {

            return View(service.Get(id));
        }

        // POST: Rates/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("Id,Score,Text,UserName,UserId,Date")] Rate rate)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    service.Edit(rate);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RateExists(rate.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(rate);
        }

        // GET: Rates/Delete/5
        public IActionResult Delete(int id)
        {
            return View(service.Get(id));
        }

        // POST: Rates/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            service.Delete(id);
            return RedirectToAction(nameof(Index));
        }

        private bool RateExists(int id)
        {
            Rate rate = service.Get(id);
            if (rate != null)
            {
                return true;
            }
            return false;
        }
    }
}
