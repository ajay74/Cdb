using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Text;
using Courby.Exception;
using Courby.Data;
using Courby.Security;


namespace Courby.Business.Data
{
    public static class Business
    {
        // These specify the encryption keys used.
        static readonly string BUSINESSKEY = ConfigurationManager.AppSettings["BUSINESSKEY"].ToString();
        static readonly string USERSKEY = ConfigurationManager.AppSettings["USERKEY"].ToString();

        /// <summary>
        /// Adds a new business user.
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static Dictionary<string,Guid> AddBusiness(string email, string BusinessName, string password)
        {
            Dictionary<string, Guid> retValues = new Dictionary<string, Guid>();

            try
            { 
                SqlParameter[] result = Connection.ExecuteProcedureNonQuery("BusinessCreateBase", true,
                    new Connection.ParamData() { name = "@BusinessName", value = Security.Encryption.Encrypt(BusinessName, BUSINESSKEY) },
                    new Connection.ParamData() { name = "@emailAddress", value = Security.Encryption.Encrypt(email, USERSKEY) },
                    new Connection.ParamData() { name = "@password", value = Security.Encryption.Encrypt(password, email) },
                    new Connection.ParamData() { name = "@businessId", value = Guid.Empty, output = true },
                    new Connection.ParamData() { name = "@userId", value = Guid.Empty, output = true });
                
                for (int i = 0; i < result.Length; i++)
                    if (result[i].ParameterName == "@businessId" || result[i].ParameterName == "@userId")
                        retValues.Add(result[i].ParameterName, (Guid)result[i].Value);

                return retValues;
            }
            catch (System.Exception error)
            {
                new CourbyException(error);
            }

            // Return default
            retValues = new Dictionary<string, Guid>();
            retValues.Add("@businessId", Guid.Empty);
            retValues.Add("@userId", Guid.Empty);

            return retValues;
        }


        /// <summary>
        /// Get business data.
        /// </summary>
        /// <param name="businessId">ID To the business</param>
        /// <returns></returns>
        public static System.Data.SqlClient.SqlDataReader GetBusiness(Guid businessId)
        {
             return Connection.ExecuteProcedure("GetBusniess",
                    new Connection.ParamData() { name = "@businessId", value = businessId });

        }

        /// <summary>
        /// Confirms email address.
        /// </summary>
        /// <param name="email"></param>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public static int ConfirmEmail(string email, Guid UserId)
        {
            int retVal = Connection.ExecuteProcedureNonQuery("BusinessCreateBase",
                new Connection.ParamData() { name = "@userId", value = UserId },
                new Connection.ParamData() { name = "@emailAddress", value = Security.Encryption.Encrypt(email, BUSINESSKEY) });

            return retVal;
        }
    }
}
