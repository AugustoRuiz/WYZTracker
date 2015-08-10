using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;
using System.Windows.Forms;
using System.ComponentModel;
using System.Threading;
using System.Collections;

namespace WYZTracker
{
    public class LocalizationManager
    {
        /// <summary>
        /// Static constructor, initializes supported cultures.
        /// </summary>
        static LocalizationManager()
        {
            supportedCultures = new List<string>() { "en", "es", "de" };
        }

        /// <summary>
        /// List of supported cultures.
        /// </summary>
        private static List<string> supportedCultures;

        /// <summary>
        /// Gets the list of supported cultures.
        /// </summary>
        public static List<string> SupportedCultures
        {
            get
            {
                return supportedCultures;
            }
        }

        public static string GetCurrentCultureName()
        {
            return Thread.CurrentThread.CurrentUICulture.Name;
        }

        /// <summary>
        /// Localizes the currently open forms and sets the current culture to the specified one 
        /// if supported. Otherwise, the first culture in the supported cultures list is used.
        /// </summary>
        /// <param name="cultureName">The culture name.</param>
        public static void LocalizeApplication(string cultureName)
        {
            if (string.IsNullOrEmpty(cultureName))
            {
                cultureName = Thread.CurrentThread.CurrentUICulture.Name;
            }

            CultureInfo newCulture = new CultureInfo(cultureName);
            while (!newCulture.IsNeutralCulture)
            {
                newCulture = newCulture.Parent;
            }

            // Look for the culture by full name.
            if (!supportedCultures.Contains(newCulture.Name))
            {
                newCulture = new CultureInfo(supportedCultures[0]);
            }

            Thread.CurrentThread.CurrentUICulture = newCulture;
        }

        /// <summary>
        /// Dumps the neutral cultures.
        /// </summary>
        internal static void DumpNeutralCultures()
        {
            foreach (CultureInfo ci in CultureInfo.GetCultures(CultureTypes.NeutralCultures))
            {
                System.Diagnostics.Debug.WriteLine(string.Format("{0}\t{1}", ci.Name, ci.EnglishName));
            }
        }

    }
}
