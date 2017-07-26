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
            File.AppendAllText(path, contents);
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
                if(stream != null)
                    stream.Dispose();

                if(File.Exists(path))
                    File.Delete(path);
            }

            // release any unmanaged objects
            // set the object references to null

            _disposed = true;
        }
    }
}