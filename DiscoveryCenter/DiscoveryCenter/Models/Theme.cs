﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DiscoveryCenter.Models
{
    public class Theme
    {
        [Key]
        public int Id { get; set; }

        public ThemeName Name { get; set; }

        public string CssBundleName { get; set; }

        public string JsBundleName { get; set; }

        public string SurveyView { get; set; }

        public string WelcomeView { get; set; }

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