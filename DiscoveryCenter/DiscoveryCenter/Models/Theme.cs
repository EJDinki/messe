using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DiscoveryCenter.Models
{
    public class Theme
    {
        /// <summary>
        /// Themes help customize the layout and a few sounds of surveys.
        /// The constructor is here because the strings must be empty instead of null if not set.
        /// </summary>
        public Theme()
        {

            //Set the SRCs of the following files to empty until set by user so no null pointers occur.
            PrevButtonAudio = "";
            NextButtonAudio = "";
            FinishAudio = "";
            Logo = "";
        }

        [Key]
        public int Id { get; set; }

        public String Name { get; set; }

        public string CssFileName { get; set; }

        public string JsFileName { get; set; }

        public string PrevButtonAudio { get; set; }

        public string NextButtonAudio { get; set; }

        public string FinishAudio { get; set; }

        public string Logo { get; set; }
    }

    //public enum ThemeName
    //{
    //    Adult,
    //    Child
    //}

    public sealed class ThemeName
    {
        public static readonly ThemeName Adult = new ThemeName("Adult");
        public static readonly ThemeName Child = new ThemeName("Child");

        //never call this!!!
        public ThemeName() { }

        private ThemeName(string value)
        {
            Value = value;
        }

        public ThemeName getThemeName(string tn)
        {
            if (tn == Adult.Value)
                return Adult;
            else if (tn == Child.Value)
                return Child;
            else 
                return null;
        }

        public string Value { get; private set; }
    }
}