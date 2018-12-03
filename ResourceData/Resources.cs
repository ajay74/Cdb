using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Courby.Data;

namespace Courby.Resource.Data
{
    public static class Resources
    {
        /// <summary>
        /// Resources_GetLanguage
        /// </summary>
        /// <param name="culture"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static Dictionary<string, string> GetLanguageResourceValue(string culture, string key)
        {
            Dictionary<string, string> language = new Dictionary<string, string>();

            language.Add("key", "");
            language.Add("value", "");

            using (SqlDataReader data = Connection.ExecuteProcedure("Resources_GetLanguage",
                new Connection.ParamData() { name = "@key", value = key },
                new Connection.ParamData() { name = "@culture", value = culture }
            ))
            {
                while (data.Read())
                { // Only 1 row should be returned.
                    language["key"] = data["key"].ToString();
                    language["value"] = data["value"].ToString();

                    break;
                }
            }

            return language;
        }

        /// <summary>
        /// Builds a dictionary object of the page of language keys.
        /// </summary>
        /// <param name="culture"></param>
        /// <param name="ResourceKeys"></param>
        /// <returns></returns>
        public static Dictionary<string, string> GetLanguageResourceValues(string culture, string pageKey)
        {
            Dictionary<string, string> language = new Dictionary<string, string>();

            language.Add("key", "");
            language.Add("value", "");

            using (SqlDataReader data = Connection.ExecuteProcedure("Resources_GetPageResources",
                new Connection.ParamData() { name = "@page", value = pageKey },
                new Connection.ParamData() { name = "@culture", value = culture }
            ))
            {
                while (data.Read())
                { // Only multiple rows are returned.
                    language["key"] = data["key"].ToString();
                    language["value"] = data["value"].ToString();

                    break;
                }
            }

            return language;
        }
    }
}
