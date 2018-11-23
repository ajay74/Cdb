using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Text;
using Courby.Core.Data;
using Courby.Data;
using Courby.Security;
using Courby.Exception;
using Courby;

namespace Courby.Core.Data
{
    public static class User
    {
        static readonly string USERSKEY = ConfigurationManager.AppSettings["USERKEY"].ToString();

        // Create User
        public static Guid CreateUser(string emailAddress, string password)
        {
            try
            {
               SqlParameter[] result = Connection.ExecuteProcedureNonQuery("UserCreateBase",
                   true,
                   new Connection.ParamData() { name = "@emailAddress", value = Encryption.Encrypt(emailAddress, USERSKEY) },
                        new Connection.ParamData() { name = "@password", value = Encryption.Encrypt(password, emailAddress) },
                        new Connection.ParamData() { name = "@userId", value = Guid.Empty, output = true });

                return result != null ? (Guid)result[0].Value : Guid.Empty; 

            }
            catch (System.Exception error)
            {
                CourbyException cError = new CourbyException(error);
            }

            return Guid.Empty;
        }
        /// <summary>
        ///  UserUpdateDetail
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="emailAddress"></param>
        /// <param name="password"></param>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <param name="dateOfBirth"></param>
        /// <returns></returns>
        public static int UpdateUser(Guid userId, string emailAddress, string password,
            string firstName, string lastName, DateTime dateOfBirth) => Connection.ExecuteProcedureNonQuery("UserUpdateDetail",
                new Connection.ParamData() { name = "@userId", value = userId },
                new Connection.ParamData() { name = "@password", value = Encryption.Encrypt(password, emailAddress) },
                new Connection.ParamData() { name = "@firstName", value = Encryption.Encrypt(firstName,password) },
                new Connection.ParamData() { name = "@lastName", value = Encryption.Encrypt(lastName,password) },
                new Connection.ParamData() { name = "@dob", value = dateOfBirth }
                );
        // Confirm Email
        public static int ConfirmEmail(Guid userId, string emailAddress) =>  Connection.ExecuteProcedureNonQuery("UserValidateEmail",
                new Connection.ParamData() { name = "@userId", value = userId },
                new Connection.ParamData() { name = "@emailAddress", value = emailAddress });

        // Delete User
        // Change user details.
        public static int ChangeEmailPassword(Guid userId, string newEmail, string oldEmail, string newPassword, string oldPassword)
        {
            return -1;
        }
    }
}
