﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DiscoveryCenter.Models
{
    public class ReportsViewModel
    {
        public ReportsViewModel()
        {
            Reports = new List<ReportViewModel>();
        }
        public string SurveyName { get; set; }
        public List<ReportViewModel> Reports { get; set; }
    }

    public class ReportViewModel
    {
        public ReportViewModel()
        {
            Counts = new Dictionary<string, int>();
        }
        public string Text { get; set; }

        public int QuestionIndex { get; set; }

        public Dictionary<string, int> Counts { get; set; }
    }
}