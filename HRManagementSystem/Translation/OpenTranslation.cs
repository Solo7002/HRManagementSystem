using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;

namespace HRManagementSystem.Translation
{
    internal static class OpenTranslation
    {
        private static Dictionary<string, Dictionary<string, string>> translations;

        static OpenTranslation()
        {
            translations = new Dictionary<string, Dictionary<string, string>>();

            LoadTranslation("en");
            LoadTranslation("ua");
            LoadTranslation("de");
            LoadTranslation("fr");
            LoadTranslation("it");
            LoadTranslation("es");
            LoadTranslation("ja");
        }

        private static void LoadTranslation(string language)
        {
            try
            {
                Dictionary<string, string> TranslationByLanguage = JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText($"../../Translation/Languages/{language}.json"));
                translations[language] = TranslationByLanguage;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public static string GetTranslation(string language, string tag)
        {
            Dictionary<string, string> TranslationByLanguage;
            string TranslatedWord;
            if (translations.TryGetValue(language, out TranslationByLanguage))
            {
                if (TranslationByLanguage.TryGetValue(tag, out TranslatedWord))
                {
                    return TranslatedWord;
                }
            }

            return "Translation not found";
        }
    }
}
