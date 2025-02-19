using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace EasySaveWPFApp.Controller
{
    internal static class LanguageResourceController
    {
        public static void SetLanguage(string cultureCode)
        {
            string currentCulture = CultureInfo.CurrentCulture.Name;
            if (string.IsNullOrEmpty(cultureCode) || cultureCode == currentCulture)
            {
                return;
            }
            Thread.CurrentThread.CurrentCulture = new CultureInfo(cultureCode);
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(cultureCode);
        }
    }
}
