using System;
using System.IO;
using System.Text;
using System.Threading;

namespace ChimpLab.Extensions.Configuration.Json.Tests
{
    public class ManagedConfigFile : IDisposable
    {
        bool _disposed;
        private string path;
        private FileStream stream;

        public ManagedConfigFile(string path, string contents)
        {
            for (int i = 0; i < 5; i++)
            {
                try
                {
                    SetupStream(path, contents);
                }
                catch (IOException)
                {
                    Thread.Sleep(100);
                }

                if(stream != null)
                    break;

            }
            
        }

        private void SetupStream(string path, string contents)
        {
            this.path = path;
            this.stream = System.IO.File.OpenWrite(path);
            stream.SetLength(0);
            stream.Flush(true);
            byte[] data = new UTF8Encoding(true).GetBytes(contents);
            stream.Write(data, 0, data.Length);
            stream.Flush(true);
            stream.Close();
        }

        public void Dispose()
        {
            Dispose(true);
            //GC.SuppressFinalize(this);
        }

        ~ManagedConfigFile()
        {
            Dispose(false);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                // free other managed objects that implement
                // IDisposable only
                stream.Dispose();
                System.IO.File.Delete(path);
            }

            // release any unmanaged objects
            // set the object references to null

            _disposed = true;
        }
    }
}