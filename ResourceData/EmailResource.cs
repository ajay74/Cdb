using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using Courby.Data;

namespace Courby.Resource.Email
{
    public static class EmailResource
    {
        /// <summary>
        /// Gets an email resource.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public static Dictionary<string, string> GetEmailResouce (string key, string culture)
        {
            Dictionary<string, string> email = new Dictionary<string, string>();

            // Set default values.
            email.Add("subject", "");
            email.Add("body", "");

            // Create connection.
            using (SqlDataReader data = Connection.ExecuteProcedure("Resources_GetEmail",
                new Connection.ParamData() { name = "@key", value = key },
                new Connection.ParamData() { name = "@culture", value = culture }
            ))
            {
               while (data.Read())
                { // Only 1 row should be returned.
                    email["body"] = data["body"].ToString();
                    email["subject"] = data["subject"].ToString();

                    break;
                }
            }

            return email;
        }
    }
}
