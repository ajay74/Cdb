using System;
using Courby.Data;


namespace Courby.Core.Data
{
    public static class AddressType
    {
        /// <summary>
        /// Adds an address type to the system.
        /// </summary>
        /// <param name="addressTypeName"></param>
        /// <returns></returns>
        public static int Add(string addressTypeName) => Courby.Data.Connection.ExecuteProcedureNonQuery
                ("InsertAddressType", new Connection.ParamData() { name = "@addressTypeName", value = addressTypeName, output = false });
        /// <summary>
        /// Removes a address type from the system.
        /// </summary>
        /// <param name="addressTypeName"></param>
        /// <returns></returns>
        public static int Delete(string addressTypeName) => Courby.Data.Connection.ExecuteProcedureNonQuery
                ("DeleteAddressType",
                    new Connection.ParamData() { name = "@addressTypeId", value = DBNull.Value, output = false },
                    new Connection.ParamData() { name = "@addressTypeId", value = addressTypeName, output = false });
        /// <summary>
        /// Removes a address type from the system.
        /// </summary>
        /// <param name="addressTypeId"></param>
        /// <returns></returns>
        public static int Delete(int addressTypeId) => Courby.Data.Connection.ExecuteProcedureNonQuery
                ("DeleteAddressType",
                    new Connection.ParamData() { name = "@addressTypeId", value = addressTypeId, output = false },
                    new Connection.ParamData() { name = "@addressTypeId", value = DBNull.Value, output = false });

        /// <summary>
        /// Gets the address types table stream.
        /// </summary>
        /// <returns></returns>
        public static System.Data.SqlClient.SqlDataReader GetAddressTypes() => Courby.Data.Connection.ExecuteProcedure("GetAddressTypes");

    }
}
