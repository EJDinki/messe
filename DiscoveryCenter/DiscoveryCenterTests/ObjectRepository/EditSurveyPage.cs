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
    public class EditSurveyPage
    {
        private BaseTest testManager;
        private HtmlUnorderedList divQuestionList; 


        public EditSurveyPage(BaseTest test)
        {
            testManager = test;
            divQuestionList =testManager.Find.ById<HtmlUnorderedList>("draggablePanelList");
        }

        public HtmlInputText SurveyName
        {
            get
            {
                return testManager.Find.ById<HtmlInputText>("Name");
            }
        }

        public HtmlButton SaveButton
        {
            get
            {
                return testManager.Find.ById<HtmlButton>("save");
            }
        }

        public HtmlAnchor AddQuestion
        {
            get
            {
                return testManager.Find.ById<HtmlAnchor>("addItem");
            }
        }

        public HtmlDiv ValidationSummary
        {
            get
            {
                return testManager.Find.ById<HtmlDiv>("valSum");
            }
        }

        public HtmlDiv GetDraggableQHeader(int questionIndex)
        {
            var qList = divQuestionList.ChildNodes;

            return new HtmlDiv(qList[questionIndex - 1].Children[0]);
        }

        public HtmlDiv GetQBody(int questionIndex)
        {
            return new HtmlDiv(GetDraggableQHeader(questionIndex).BaseElement.GetNextSibling());
        }

    }
}
