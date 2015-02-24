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
    public class SurveyTakePage
    {
        private BaseTest testManager;

        public SurveyTakePage(BaseTest test)
        {
            testManager = test;
        }

        public HtmlDiv ThankYouDiv
        {
            get
            {
                return testManager.Find.ByAttributes<HtmlDiv>("class=jumbotron");
            }
        }

        public HtmlAnchor StartSurvey
        {
            get
            {
                return testManager.Find.ById<HtmlAnchor>("Btn_Start");
            }
        }

        public HtmlAnchor NextQuestion
        {
            get
            {
                return testManager.Find.ByAttributes<HtmlAnchor>("class=right carousel-control pull-right");
            }
        }

        public HtmlAnchor PreviousQuestion
        {
            get
            {
                return testManager.Find.ByAttributes<HtmlAnchor>("class=left carousel-control pull-left");
            }
        }

        private HtmlDiv ActiveQuestionDiv
        {
            get
            {
                HtmlDiv carousel = testManager.Find.ByAttributes<HtmlDiv>("class=carousel-inner");
                HtmlDiv active = carousel.Find.ByAttributes<HtmlDiv>("class=item active");

                while (active == null)
                {
                    carousel.Refresh();
                    active = carousel.Find.ByAttributes<HtmlDiv>("class=item active");
                }
                  
                return active;
            }
        }

        public HtmlInputSubmit SubmitSurvey
        {
            get
            {
                return testManager.Find.ByAttributes<HtmlInputSubmit>("type=submit");
            }
        }

        /// <summary>
        /// Returns the checkbox at the ChoiceIndex indicated for the ActiveQuestion
        /// </summary>
        /// <param name="choiceIndex">Index of the choice (0 indexed)</param>
        /// <returns></returns>
        public HtmlInputCheckBox GetCheckBoxAtIndex(int choiceIndex)
        {
            var buttonList = ActiveQuestionDiv.Find.AllByAttributes<HtmlInputCheckBox>("name=~.IsSelected");
            return buttonList[choiceIndex];
        }

        /// <summary>
        /// Returns the radiobutton at the ChoiceIndex indicated for the ActiveQuestion
        /// </summary>
        /// <param name="choiceIndex">Index of the choice (0 indexed)</param>
        /// <returns></returns>
        public HtmlInputRadioButton GetRadioButtonAtIndex(int choiceIndex)
        {
            var buttonList = ActiveQuestionDiv.Find.AllByAttributes("id=~_Answer");
            return new HtmlInputRadioButton(buttonList[choiceIndex]);
        }

        /// <summary>
        /// Gets the slider handle for the ActiveQuestion
        /// </summary>
        /// <returns></returns>
        public HtmlDiv GetSliderHandle()
        {
            return ActiveQuestionDiv.Find.ByAttributes<HtmlDiv>("class=slider-handle min-slider-handle round");
        }

        public HtmlDiv GetSliderTrack()
        {
            return ActiveQuestionDiv.Find.ByAttributes<HtmlDiv>("class=slider-track");
        }

        /// <summary>
        /// Returns the textArea for the active shortAnswer question
        /// </summary>
        /// <returns></returns>
        public HtmlTextArea GetShortAnswerArea()
        {
            return ActiveQuestionDiv.Find.ById<HtmlTextArea>("~_Answer");
        }
    }
}
