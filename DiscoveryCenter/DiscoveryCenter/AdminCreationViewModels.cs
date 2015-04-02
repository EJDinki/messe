using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DiscoveryCenter.Models
{
    public class ChoiceBoxViewModel
    {
        /// <summary>
        /// Used to uniquely identify a Choice Text to a question
        /// </summary>
        public string NameAndId { get; set; }

        /// <summary>
        /// Used to uniquely identify an image drop down list to a question
        /// </summary>
        public string ImgChoiceId { get; set; }

        /// <summary>
        /// Value of the Choice Text
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Src to locate the related image
        /// </summary>
        public string ImageName { get; set; }

        /// <summary>
        /// Drop down list model for all images located in the Content/images/choiceImage folder
        /// </summary>
        public List<System.Web.Mvc.SelectListItem> AvailableImages { get; set; }

        public bool AllowDelete { get; set; }
    }
}