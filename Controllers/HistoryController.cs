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
    // Only Admins can access this conntroller and its views
    [Authorize(Roles = "Admin")]
    public class HistoryController(ApplicationDbContext context) : Controller
    {
        private readonly ApplicationDbContext _context = context;

        // GET: List all Movements
        // Action Method accepts request with or without "option" parameter
        [Route("/History")]
        [Route("/History/{option?}")]
        public async Task<IActionResult> Index(string option = null!)
        {
            List<History> historyList;

            if (option != null && option != "A")
            {
                if (option == "I")
                {
                    Console.WriteLine("Increment");
                }
                else
                {
                    Console.WriteLine("Withdraw");
                }
                historyList = await _context.HistorySet.Where(it => it.Type == option).ToListAsync();
            }
            else
            {
                historyList = await _context.HistorySet.ToListAsync();
            }

            return View(historyList);
        }
    }
}
