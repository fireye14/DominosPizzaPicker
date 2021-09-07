using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace DominosPizzaPicker.Client.Helpers
{
    public class Disposable : IDisposable
    {
        private bool isDisposed = false;

        public event EventHandler Disposed;

        public Disposable()
        {
            Disposed += Disposable_Disposed;
        }

        ~Disposable()
        {
            Dispose(false);
        }

        private void Disposable_Disposed(object sender, EventArgs e)
        {
            Dispose();
        }

        public void Dispose()
        {
            Disposed -= Disposable_Disposed;
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (isDisposed) return;            

            isDisposed = true;

            Disposed?.Invoke(this, EventArgs.Empty);
        }
    }
}
