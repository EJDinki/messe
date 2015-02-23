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
    public class LoginPage
    {
        private BaseTest testManager;

        public LoginPage(BaseTest test)
        {
            testManager = test;
        }

        public HtmlInputText Username
        {
            get
            {
                return testManager.Find.ById<HtmlInputText>("UserName");
            }
        }

        public HtmlInputPassword Password
        {
            get
            {
                return testManager.Find.ById<HtmlInputPassword>("Password");
            }
        }

        public HtmlInputSubmit LogIn
        {
            get
            {
                return testManager.Find.ByAttributes<HtmlInputSubmit>("value=Log in");
            }
        }

        public HtmlInputCheckBox RememberMe
        {
            get
            {
                return testManager.Find.ById<HtmlInputCheckBox>("RememberMe");
            }
        }

        public HtmlDiv ValidationSummary
        {
            get
            {
                return testManager.Find.ByAttributes<HtmlDiv>("class=validation-summary-errors");
            }
        }
    }
}
