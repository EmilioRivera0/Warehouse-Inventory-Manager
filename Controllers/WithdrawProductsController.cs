using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Warehouse_Inventory_Manager.Data;
using Warehouse_Inventory_Manager.Models;

namespace Warehouse_Inventory_Manager.Controllers
{
    // Controller and its views are accessible only by Warehouse Staff
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
            ViewData["Name"] = product.Name;
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

            if (withdrawAmount > product.Stock)
            {
                ViewData["Name"] = product.Name;
                ViewData["Stock"] = product.Stock;
                ViewData["Error"] = "Withdraw amount must be less than or equal to Current Stock";
                return View();
            }

            product.Stock -= withdrawAmount;

            // get user object
            
            WarehouseUser user = await _context.Users.FindAsync(int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)));

            if (user == null)
                return Unauthorized();

            // add history record
            History history = new()
            {
                Type = "W",
                IdProduct = id,
                Product = product,
                IdUser = user.Id,
            };

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product);
                    _context.HistorySet.Add(history);
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
