using System.Linq;
using System.Web.Mvc;
using MVC.Models;
using MVC.Models.ViewModels;

namespace MVC.Controllers
{
    public class HomeController : Controller
    {
        private NorthwindEntities db = new NorthwindEntities();
        public FileContentResult GetImage(int id)
        {
            var category = db.Categories.FirstOrDefault(x => x.CategoryID == id);
            if (category == null || category.Picture == null)
            {
                return null;
            }
            using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
            {
                byte[] imagebyte = category.Picture;
                ms.Write(imagebyte, 78, imagebyte.Length - 78);
                return File(ms.ToArray(), "image/jpeg");
            }
        }
        public ActionResult Details(int? id)
        {
            if (id.HasValue)
            {
                CategoryDetailsViewModel viewModel = new CategoryDetailsViewModel();
                viewModel.CategoryData = db.Categories.FirstOrDefault(x => x.CategoryID == id.Value);
                viewModel.ProductCollection = db.Products.Where(x => x.CategoryID == id.Value).OrderBy(x => x.ProductID).ToList();
                return View(viewModel);
            }
            return RedirectToAction("Index");
        }
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}