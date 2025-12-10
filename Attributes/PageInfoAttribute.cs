using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeghToolsUi.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    internal class PageInfoAttribute : Attribute
    {
        public bool IsFooterPage { get; }
        public string PageTitle { get; }
        public string PageIcon { get; }

        public PageInfoAttribute(bool isFooterPage, string pageTitle, string pageIcon)
        {
            IsFooterPage = isFooterPage;
            PageTitle = pageTitle;
            PageIcon = pageIcon;
        }
    }
}
