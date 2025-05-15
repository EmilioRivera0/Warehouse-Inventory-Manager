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
    [Authorize(Roles = "Admin")]
    public class HistoryController(ApplicationDbContext context) : Controller
    {
        private readonly ApplicationDbContext _context = context;

        // GET: List all Movements
        public async Task<IActionResult> Index()
        {
            return View(await _context.HistorySet.ToListAsync());
        }
    }
}
