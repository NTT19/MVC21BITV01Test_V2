using Microsoft.AspNetCore.Mvc;
using MidTerm.Entity;
using MidTerm.ViewModel;

namespace MidTerm.Controllers
{
    public class Cau2Controller : Controller
    {
        private readonly MyeStoreContext _context;
        public Cau2Controller(MyeStoreContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Create(NhaCungCapViewModel model)
        {
            if (ModelState.IsValid)
            {
                NhaCungCap newNhaCungCap = new NhaCungCap
                {
                    MaNcc = model.MaNcc,
                    TenCongTy = model.TenCongTy,
                    Logo = model.Logo,
                    NguoiLienLac = model.NguoiLienLac,
                    Email = model.Email,
                    DienThoai = model.DienThoai,
                    DiaChi = model.DiaChi,
                    MoTa = model.MoTa
                };

                _context.NhaCungCaps.Add(newNhaCungCap);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(View));
            }

            return View(model);
        }

        public IActionResult View()
        {
            var data = _context.NhaCungCaps != null ? _context.NhaCungCaps.ToList() : new List<NhaCungCap>();
            return View(data);
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
