using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ArtOfTest.WebAii.Controls.HtmlControls;
using ArtOfTest.WebAii.Controls.HtmlControls.HtmlAsserts;
using ArtOfTest.WebAii.Core;
using ArtOfTest.WebAii.ObjectModel;
using ArtOfTest.WebAii.TestAttributes;
using ArtOfTest.WebAii.TestTemplates;
using ArtOfTest.WebAii.Win32.Dialogs;

using ArtOfTest.WebAii.Silverlight;
using ArtOfTest.WebAii.Silverlight.UI;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DiscoveryCenterTests.ObjectRepository
{
    public class SurveyListPage
    {
        private BaseTest testManager;

        public SurveyListPage(BaseTest test)
        {
            testManager = test;
        }

        public HtmlInputSubmit ConfirmDelete
        {
            get
            {
                return testManager.Find.ByAttributes<HtmlInputSubmit>("type=submit", "value=Delete");
            }
        }

        public HtmlAnchor GetEditFor(int surveyId)
        {
            return testManager.Find.ById<HtmlAnchor>("edit_" + surveyId);
        }

        public HtmlAnchor GetViewFor(int surveyId)
        {
            return testManager.Find.ById<HtmlAnchor>("view_" + surveyId);
        }

        public HtmlAnchor GetDeleteFor(int surveyId)
        {
            return testManager.Find.ById<HtmlAnchor>("delete_" + surveyId);
        }
    }
}
