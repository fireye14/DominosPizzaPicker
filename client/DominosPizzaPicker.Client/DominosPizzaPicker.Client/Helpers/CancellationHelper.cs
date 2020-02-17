using System;
using System.Threading;

namespace DominosPizzaPicker.Client.Helpers
{
    public class CancellationHelper
    {
        public CancellationTokenSource Source { get; set; }
        public CancellationToken Token
        {
            get { return Source != null ? Source.Token : CancellationToken.None; }
        }

        public void Init()
        {
            if (Source != null) return;
            Source = new CancellationTokenSource();
        }

        public void Cancel()
        {
            if (Source == null) return;            
            Source.Cancel();        
        }

        public void Dispose()
        {
            if (Source == null) return;
            Source.Dispose();
            Source = null;            
        }

        // Used to periodically check if cancellation was requested
        public void ThrowIfCancellationRequested()
        {
            if (Source == null) return;
            Token.ThrowIfCancellationRequested();
        }

        public bool IsCancellationRequested
        {
            get { return Source != null ? Token.IsCancellationRequested : false; }
        }
    }
}