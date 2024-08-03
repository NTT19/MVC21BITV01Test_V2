using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MidTerm.Entity;
using MidTerm.ViewModel;

namespace MidTerm.Controllers
{


    public class Cau3Controller : Controller
    {

        private readonly MyeStoreContext _context;

        public Cau3Controller(MyeStoreContext context)
        {
            _context = context;
        }

        [HttpGet("Order/Details/{id}")]
        public async Task<IActionResult> Details(int id)
        {
            var orderDetails = await _context.ChiTietHds
                .Where(c => c.MaHd == id)
                .Select(c => new OrderDetailViewModel
                {
                    MaHH = c.MaHh,
                    TenHH = c.MaHhNavigation.TenHh,
                    MoTaDonVi = c.MaHhNavigation.MoTaDonVi,
                    DonGia = c.DonGia,
                    SoLuong = c.SoLuong,
                    ThanhTien = c.SoLuong * c.DonGia
                })
                .ToListAsync();

            if (orderDetails == null || !orderDetails.Any())
            {
                return NotFound();
            }

            return View(orderDetails);
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
