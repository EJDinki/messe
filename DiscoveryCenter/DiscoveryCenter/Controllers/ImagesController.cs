using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DiscoveryCenter.Controllers
{
    [Authorize]
    public class ImagesController : Controller
    {
        private static readonly string exhibitImagePartial = "/Content/images/exhibits/";
        private static readonly string choiceImagePartial = "/Content/images/choiceImage/";
        private static readonly string soundPartial = "/Content/audio/";

        // GET: Images
        public ActionResult Index()
        {

            return View(GetLocations());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Upload(HttpPostedFileBase file, string location)
        {
            try
            {
                if (location != "sounds")
                    Image.FromStream(file.InputStream);
                else if (!file.FileName.EndsWith("mp3"))
                    throw new Exception("Not an mp3 file and was designated as sound.");

            }
            catch
            {
                ModelState.AddModelError("InvalidFile", "The file chosen was not a valid type. Please select another file.");
                return View("Index", GetLocations());
            }

            if (location == "choices")
            {
                file.SaveAs(Path.Combine(Server.MapPath("~" + choiceImagePartial), Path.GetFileName(file.FileName)));
            }
            else if(location == "exhibits")
            {
                file.SaveAs(Path.Combine(Server.MapPath("~" + exhibitImagePartial), Path.GetFileName(file.FileName)));
            }
            else if (location == "sounds")
            {
                file.SaveAs(Path.Combine(Server.MapPath("~" + soundPartial), Path.GetFileName(file.FileName)));
            }
            return RedirectToAction("Index");
        }

        public ActionResult DeleteImage(string location)
        {
            var truePath = Server.MapPath(location);
            
            if(System.IO.File.Exists(truePath))
                System.IO.File.Delete(truePath);
            
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Tuple of 3 lists.
        /// 1) Choice Image Locations
        /// 2) Exhibit Image Locations
        /// 3) Sound Locations
        /// </summary>
        /// <returns>Tuple(choiceImageLocations,exhibitImageLocations,soundLocations)"</returns>
        private Tuple<List<string>, List<string>, List<string>> GetLocations()
        {
            List<string> storedImageNames = new List<string>();
            List<string> storedExhibitImages = new List<string>();
            List<string> storedSounds = new List<string>();

            foreach (string image in Directory.GetFiles(Server.MapPath("~/Content/images/choiceImage")))
            {
                string val = "/Content/images/choiceImage/" + Path.GetFileName(image);
                storedImageNames.Add(val);
            }

            foreach (string image in Directory.GetFiles(Server.MapPath("~/Content/images/exhibits")))
            {
                string val = "/Content/images/exhibits/" + Path.GetFileName(image);
                storedExhibitImages.Add(val);
            }

            foreach (string sound in Directory.GetFiles(Server.MapPath("~/Content/audio/")))
            {
                string val = "/Content/audio/" + Path.GetFileName(sound);
                storedSounds.Add(val);
            }

            return new Tuple<List<string>, List<string>, List<string>>(storedImageNames, storedExhibitImages, storedSounds);
        }
    }
}