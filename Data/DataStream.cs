using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Threading;

namespace Courby.Data
{
    public class DataStream : IDisposable
    {
        #region Members

        System.Data.SqlClient.SqlDataReader _stream;
        List<DataTable> _resultSets = new List<DataTable>();
        ManualResetEvent _waitHandle = new ManualResetEvent(false);
        Thread _thread;

        bool _dataReady = false;
        bool _process = false;

        #endregion

        #region Properties

        /// <summary>
        /// Returns true if data has been returned.
        /// </summary>
        public bool DataReady
        {
            get { return _dataReady; }
        }
        /// <summary>
        /// Gets the data table
        /// </summary>
        public List<DataTable> Data
        {
            get { return _resultSets; }
        }
          

        #endregion

        public DataStream (System.Data.SqlClient.SqlDataReader stream)
        {
            _stream = stream;
            _thread = new Thread(StreamData);
            _thread.Start();
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _waitHandle.WaitOne();
                _thread.Abort();
                _waitHandle.Close();

                for (int i = 0; i < _resultSets.Count; i++)
                    _resultSets[0].Dispose();

                _resultSets = new List<DataTable>();
                if (!_stream.IsClosed) _stream.Close();

            }
        }

        void StreamData()
        {
            _waitHandle.WaitOne();

            while (_stream.HasRows)
            {
                DataTable resultSet = _stream.GetSchemaTable();
                object[] values = new object[_stream.FieldCount -1];

                while (_stream.Read())
                {
                    _stream.GetValues(values);
                    resultSet.Rows.Add(values);
                    _waitHandle.WaitOne(100);
                }

                _dataReady = true;
                _resultSets.Add(resultSet);
                _stream.NextResult();
            }

            _dataReady = true;
        }
    }
}
