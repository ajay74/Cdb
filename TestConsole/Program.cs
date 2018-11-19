using System;
using System.Data.SqlClient;


namespace TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Program p = new Program();

            p.ThreadTest(1000);

            Console.ReadKey();

            Console.WriteLine();

            Console.WriteLine(Data.Connection.ConnectionCount);

            Console.ReadKey();
                
        }

        public void ThreadTest(int numberOfThreads)
        {
            for (int i = 0; i < numberOfThreads; i++)
            {
                System.Threading.ParameterizedThreadStart tStart = new System.Threading.ParameterizedThreadStart(Job2);
                System.Threading.Thread thread = new System.Threading.Thread(tStart);

                thread.Start(i);
            }
        }

        void Job(object input)
        {
            using (SqlDataReader reader = Data.Connection.ExecuteProcedure("TestConnection",
                new Data.Connection.ParamData() { name = "@connectionName", value = "Program " + input },
                new Data.Connection.ParamData() { name = "@testOutputData", value = "TestOutput" }))
            {
                while (reader.Read())
                {
                    //object[] values = new object[reader.FieldCount];

                    //for (int i = 0; i < reader.FieldCount; i++)
                    //    Console.Write("{0} ({1}) | ", reader.GetValue(i), reader.GetValue(i).GetType().ToString());
                }
            }
        }

        void Job2(object data)
        {
            data += " This is my text.";
            string encrypted = Security.Encryption.Encrypt(data.ToString()  , "WpQ+J8dnA3dom9Jka0lex4yTmI08dU0UwYxTWaFlLveTH9zIEJqpUqGBVWGjFU6AZUqL02N/2jaS2xaYyASujg==");
            Console.WriteLine("{0} -> {1} -> {2}", data, encrypted, Security.Encryption.Decrypt(encrypted, "WpQ+J8dnA3dom9Jka0lex4yTmI08dU0UwYxTWaFlLveTH9zIEJqpUqGBVWGjFU6AZUqL02N/2jaS2xaYyASujg=="));
        }
    }
}
