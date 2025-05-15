using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Warehouse_Inventory_Manager.Data;
using Warehouse_Inventory_Manager.Models;

namespace Warehouse_Inventory_Manager.Controllers
{
    [Authorize(Roles = "Warehouse Staff")]
    public class WithdrawProductsController(ApplicationDbContext context) : Controller
    {
        private readonly ApplicationDbContext _context = context;

        // GET: List all Products
        public async Task<IActionResult> Index()
        {
            // Only show active products
            return View(await _context.ProductsSet.Where(it => it.Status == 1).ToListAsync());
        }

        // Display Withdraw Product Page
        public async Task<IActionResult> WithdrawProduct(int? id)
        {
            Products product = await _context.ProductsSet.FindAsync(id);

            if (product == null)
                return NotFound();
            
            ViewData["Stock"] = product.Stock;

            return View();
        }

        // POST: Update Stock of Specified Product on Withdraw
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> WithdrawProduct(int id)
        {
            Products product = await _context.ProductsSet.FindAsync(id);
            
            if (product == null)
                return NotFound();

            int withdrawAmount = int.Parse(Request.Form["WithdrawAmount"]);
            Console.WriteLine(withdrawAmount);

            if (withdrawAmount > product.Stock)
            {
                ViewData["Stock"] = product.Stock;
                ViewData["Error"] = "Withdraw amount must be less than or equal to current Stock";
                return View();
            }

            product.Stock -= withdrawAmount;

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductsExists(product.Id))
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
            return View(product);
        }

        private bool ProductsExists(int id)
        {
            return _context.ProductsSet.Any(e => e.Id == id);
        }
    }
}
