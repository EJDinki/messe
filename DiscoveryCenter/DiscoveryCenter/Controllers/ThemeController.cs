using DiscoveryCenter.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DiscoveryCenter.Controllers
{
    [Authorize]
    public class ThemeController : Controller
    {
        private SurveyContext db = new SurveyContext();
        private static readonly int themesPerPage = 10;

        // GET: Theme
        public ActionResult Index(int id = 0)
        {
            return View(getTuple(id));
        }

        public Tuple<IEnumerable<Theme>, int, int> getTuple(int id)
        {
            int numPages = (db.Themes.Count() / themesPerPage);
            numPages += (db.Themes.Count() % themesPerPage > 0 || numPages == 0) ? 1 : 0;

            //Handle out of bounds cases
            if (id < 0)
                id = 0;
            else if (id >= numPages)
                id = numPages - 1;

            var currentPageList = (from s in db.Themes orderby s.Name select s).Skip(id * themesPerPage).Take(themesPerPage).ToList();
            Tuple<IEnumerable<Theme>, int, int> tuple = new Tuple<IEnumerable<Theme>, int, int>(currentPageList, numPages, id);
            return tuple;
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
            bool noEntries = theme == null;
            if (noEntries)
            {
                theme = new Theme();
            }

            theme.CssFileName = themeVM.Theme.CssFileName;
            theme.JsFileName = themeVM.Theme.JsFileName;
            theme.Name = themeVM.Theme.Name;
            theme.Logo = themeVM.Theme.Logo;
            theme.PrevButtonAudio = themeVM.Theme.PrevButtonAudio;
            theme.NextButtonAudio = themeVM.Theme.NextButtonAudio;
            theme.FinishAudio = themeVM.Theme.FinishAudio;

            if (noEntries)
                db.Themes.Add(theme);
            


            db.SaveChanges();

            return RedirectToAction("Edit", new {id = theme.Id});
        }

        public ActionResult Delete(int id = 0)
        {
            Theme theme = db.Themes.Find(id);
            var surveys = db.Surveys.Where(s => s.ThemeId == id).ToList();
            if (surveys.Any())
            {
                string errorMessage = "Cannot delete because the following surveys use this theme: ";

                foreach(var survey in surveys)
                {
                    errorMessage += survey.Name + ", ";
                }

                ModelState.AddModelError("Theme", errorMessage);
            }
            else if (theme.Name == "Mobile")
            {
                ModelState.AddModelError("Theme", "The mobile theme is used to rate Exhibits from smartphones. It cannot be deleted.");
            }

            if (!ModelState.IsValid)
                return View("Index", getTuple(0));

            String cssPath = Server.MapPath(@"~/Content/" + theme.CssFileName);
            String jsPath = Server.MapPath(@"~/Scripts/" + theme.JsFileName);

            System.IO.File.Delete(cssPath);
            System.IO.File.Delete(jsPath);

            db.Themes.Remove(theme);
            db.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}