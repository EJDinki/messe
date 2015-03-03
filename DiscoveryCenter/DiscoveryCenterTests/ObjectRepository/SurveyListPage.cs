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

        public HtmlAnchor AddSurvey
        {
            get
            {
                return testManager.Find.ById<HtmlAnchor>("addItem");
            }
        }

        public HtmlAnchor EditSurvey
        {
            get
            {
                return testManager.Find.ById<HtmlAnchor>("editSurvey");
            }
        }

        public HtmlAnchor ViewSurvey
        {
            get
            {
                return testManager.Find.ById<HtmlAnchor>("viewSurvey");
            }
        }

        public HtmlAnchor DeleteSurvey
        {
            get
            {
                return testManager.Find.ById<HtmlAnchor>("deleteSurvey");
            }
        }

        public HtmlAnchor DuplicateSurvey
        {
            get
            {
                return testManager.Find.ById<HtmlAnchor>("duplicateSurvey");
            }
        }

        public HtmlListItem GetSurveyRow(int surveyId)
        {
            return testManager.Find.ById<HtmlListItem>("selectRow_" + surveyId);
        }
    }
}
