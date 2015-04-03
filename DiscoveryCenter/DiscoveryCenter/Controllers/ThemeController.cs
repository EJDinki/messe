using DiscoveryCenter.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DiscoveryCenter.Controllers
{
    public class ThemeController : Controller
    {
        private SurveyContext db = new SurveyContext();
        private static readonly int themesPerPage = 10;

        // GET: Theme
        public ActionResult Index(int id = 0)
        {

            int numPages = (db.Themes.Count() / themesPerPage) + 1;

            //Handle out of bounds cases
            if (id < 0)
                id = 0;
            else if (id >= numPages)
                id = numPages - 1;

            var currentPageList = (from s in db.Themes orderby s.Name select s).Skip(id * themesPerPage).Take(themesPerPage).ToList();
            Tuple<IEnumerable<Theme>, int, int> tuple = new Tuple<IEnumerable<Theme>, int, int>(currentPageList, numPages, id);

            return View(tuple);
        }

        public ActionResult Edit(int? id)
        {
            ThemeViewModel tvm = new ThemeViewModel();
            Theme theme = db.Themes.Find(id);
            if (theme == null)
            {
                tvm.Theme = new Theme();
                return View(tvm);
            }
            String cssPath = Server.MapPath(@"~/Content/" + theme.CssFileName);
            tvm.CssText = System.IO.File.ReadAllText(cssPath);
            String jsPath = Server.MapPath(@"~/Scripts/" + theme.JsFileName);
            tvm.JsText = System.IO.File.ReadAllText(jsPath);
            tvm.Theme = theme;

            return View(tvm);
        }

        [HttpPost]
        public ActionResult Edit(ThemeViewModel themeVM)
        {
            string cssPath, jsPath;

            //CSS
            if(String.IsNullOrEmpty(themeVM.Theme.CssFileName))
                themeVM.Theme.CssFileName = System.IO.Path.GetRandomFileName() + ".css";

            cssPath = Server.MapPath(@"~/Content/" + themeVM.Theme.CssFileName);
            System.IO.File.WriteAllText(cssPath, themeVM.CssText);

            //JS
            if (String.IsNullOrEmpty(themeVM.Theme.JsFileName))
                themeVM.Theme.JsFileName = System.IO.Path.GetRandomFileName() + ".js";
                
            jsPath = Server.MapPath(@"~/Scripts/" + themeVM.Theme.JsFileName);
            System.IO.File.WriteAllText(jsPath, themeVM.JsText);

            //update record
            Theme theme = db.Themes.Find(themeVM.Theme.Id);
            if (theme == null)
            {
                theme = themeVM.Theme;
                db.Themes.Add(theme);
            }
            else
                theme = themeVM.Theme;

            db.SaveChanges();

            return RedirectToAction("Edit", new {id = theme.Id});
        }

        public ActionResult Delete(int id = 0)
        {
            Theme theme = db.Themes.Find(id);
            db.Themes.Remove(theme);
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        public ActionResult Preview(int id)
        {
            SurveyViewModel model = null;

            model = new SurveyViewModel();
            model.SurveyName = "Test Survey";
            model.SurveyDescription = "This is a test survey to preview the page styling";
            model.Theme = db.Themes.Find(id);

            return View("../Home/Index", model);
        }
    }
}