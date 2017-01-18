using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using Windows.Storage;
using Windows.System.Threading;

namespace Biziday.NewsTask
{
    public sealed class NewsBackgroundTask : IBackgroundTask
    {

        public void Run(IBackgroundTaskInstance taskInstance)
        {
            Debug.WriteLine("Hello");
        }

        //
        // Handles background task cancellation.
        //
        private void OnCanceled(IBackgroundTaskInstance sender, BackgroundTaskCancellationReason reason)
        {
           
        }
    }
}
