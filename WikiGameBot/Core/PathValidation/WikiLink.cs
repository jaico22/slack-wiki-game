using System;
using System.Collections.Generic;
using System.Text;

namespace WikiGameBot.Core.PathValidation
{
    public class WikiLink
    {
        /// <summary>
        /// URL For Link
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Text on link on parent page
        /// </summary>
        public string LinkText { get; set; }

        /// <summary>
        /// Title of Article Page
        /// </summary>
        public string PageTitle { get; set; }
    }
}
