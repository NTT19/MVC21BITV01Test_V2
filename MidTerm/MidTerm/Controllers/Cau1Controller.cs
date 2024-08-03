using Microsoft.AspNetCore.Mvc;
using MidTerm.Entity;
using MidTerm.ViewModel;

namespace MidTerm.Controllers
{
    public class Cau1Controller : Controller
    {
        private readonly MyeStoreContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public Cau1Controller(MyeStoreContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet("/hanghoas/all/ascending")]
        public IActionResult HangHoaAscending()
        {
            var data = _context.HangHoas
                 .OrderBy(c => c.TenHh)
                .ToList();
            return View(data);
        }

        public async Task<IActionResult> Create(HangHoaViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Kiểm tra và đảm bảo MaLoai tồn tại trong bảng Loai
                var loai = await _context.Loais.FindAsync(model.MaLoai);
                if (loai == null)
                {
                    loai = new Loai
                    {
                        TenLoai = "Tên Loại Mặc Định" // Thay đổi giá trị này cho phù hợp
                    };

                    _context.Loais.Add(loai);
                    await _context.SaveChangesAsync();

                    // Lấy lại giá trị MaLoai mới tạo
                    model.MaLoai = loai.MaLoai;
                }

                // Kiểm tra và đảm bảo MaNcc tồn tại trong bảng NhaCungCap
                var nhaCungCap = await _context.NhaCungCaps.FindAsync(model.MaNcc);
                if (nhaCungCap == null)
                {
                    nhaCungCap = new NhaCungCap
                    {
                        TenCongTy = "Tên NCC Mặc Định" // Thay đổi giá trị này cho phù hợp
                    };

                    _context.NhaCungCaps.Add(nhaCungCap);
                    await _context.SaveChangesAsync();

                    // Lấy lại giá trị MaNcc mới tạo
                    model.MaNcc = nhaCungCap.MaNcc;
                }

                string uniqueFileName = null;
                if (model.Hinh != null)
                {
                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "HinhAnh/HangHoa");
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                        Console.WriteLine($"Created directory: {uploadsFolder}");
                    }

                    uniqueFileName = Guid.NewGuid().ToString() + "_" + model.Hinh.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    try
                    {
                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await model.Hinh.CopyToAsync(fileStream);
                        }
                        Console.WriteLine($"Saved file to: {filePath}");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error saving file: {ex.Message}");
                    }
                }

                HangHoa newHangHoa = new HangHoa
                {
                    TenHh = model.TenHh,
                    TenAlias = model.TenAlias,
                    MaLoai = model.MaLoai,
                    MoTaDonVi = model.MoTaDonVi,
                    DonGia = model.DonGia,
                    Hinh = uniqueFileName,
                    NgaySx = model.NgaySx,
                    GiamGia = model.GiamGia,
                    SoLanXem = model.SoLanXem,
                    MoTa = model.MoTa,
                    MaNcc = model.MaNcc
                };

                _context.HangHoas.Add(newHangHoa);
                await _context.SaveChangesAsync();

                return RedirectToAction("View");
            }

            foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
            {
                Console.WriteLine(error.ErrorMessage);
            }

            return View(model);
        }


        public IActionResult View()
        {
            var data = _context.HangHoas != null ? _context.HangHoas.ToList() : new List<HangHoa>();
            return View(data);
        }
    }
}
