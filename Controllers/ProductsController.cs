using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Warehouse_Inventory_Manager.Data;
using Warehouse_Inventory_Manager.Models;

namespace Warehouse_Inventory_Manager.Controllers
{
    // Only logged users can access this controller and its views
    [Authorize]
    public class ProductsController(ApplicationDbContext context) : Controller
    {
        private readonly ApplicationDbContext _context = context;

        // GET: List all Products
        public async Task<IActionResult> Index()
        {
            // Show active and inactive products
            return View(await _context.ProductsSet.ToListAsync());
        }

        // Display Create Product Page only to Admins
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Create Product, only for Admins
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Price")] Products products)
        {
            if (ModelState.IsValid)
            {
                _context.Add(products);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(products);
        }

        // Display Increment Stock Product Page only for Admins
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> IncrementStock(int? id)
        {
            Products product = await _context.ProductsSet.FindAsync(id);

            if (product == null)
                return NotFound();

            return View();
        }

        // POST: Update Stock of Specified Product, only for Admins
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> IncrementStock(int id)
        {
            Products product = await _context.ProductsSet.FindAsync(id);
            
            if (product == null)
                return NotFound();

            int newStock = int.Parse(Request.Form["NewStock"]);

            if (newStock <= 0)
            {
                ViewData["Error"] = "Stock must be greater than 0.";
                return View();
            }

            product.Stock += newStock;

            // get user object
            WarehouseUser user = await _context.Users.FindAsync(int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)));

            if (user == null)
                return Unauthorized();

            // add history record
            History history = new()
            {
                Type = "I",
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

        // POST: Update Stock of Specified Product, only for Admins
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateStatus(int id)
        {
            Products product = await _context.ProductsSet.FindAsync(id);
            if (id != product.Id)
                return NotFound();
            
            // toggle status
            if (product.Status == 1)
                product.Status = 0; // Inactive
            else
                product.Status = 1; // Active

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
            return RedirectToAction(nameof(Index));
        }

        private bool ProductsExists(int id)
        {
            return _context.ProductsSet.Any(e => e.Id == id);
        }
    }
}
