using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace WebAppCore31.Helper
{
    public class CustomStream : Stream
    {
        public SqlDataReader sqlDataReader;
        public SqlTransaction sqlTransaction;
        public SqlFileStream sqlFileStream;

        public CustomStream()
        {
        }

        public override bool CanRead
        {
            get { return sqlFileStream.CanRead; }
        }

        public override bool CanSeek
        {
            get { return sqlFileStream.CanSeek; }
        }

        public override bool CanWrite
        {
            get { return sqlFileStream.CanWrite; }
        }

        public override long Length
        {
            get { return sqlFileStream.Length; }
        }

        public override long Position
        {
            get { return sqlFileStream.Position; }
            set { sqlFileStream.Position = value; }
        }
        protected override void Dispose(bool disposing)
        {
            sqlDataReader.Close();
            sqlFileStream.Close();
        }
        public override void Flush()
        {
            sqlFileStream.Flush();
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            return sqlFileStream.Seek(offset, origin);
        }

        public override void SetLength(long value)
        {
            sqlFileStream.SetLength(value);
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            return sqlFileStream.Read(buffer, offset, count);
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            sqlFileStream.Write(buffer, offset, count);
        }
    }
}
