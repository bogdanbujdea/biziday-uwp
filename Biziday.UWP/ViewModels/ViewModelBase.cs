using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Caliburn.Micro;

namespace Biziday.UWP.ViewModels
{
    public class ViewModelBase: Screen
    {
        private bool _isBusy;
        private Dictionary<string, RequestState> _requests = new Dictionary<string, RequestState>();

        public bool IsBusy
        {
            get { return _isBusy; }
            set
            {
                if (value == _isBusy) return;
                _isBusy = value;
                NotifyOfPropertyChange(() => IsBusy);
            }
        }

        protected void EndWebRequest([CallerMemberName]string label = "")
        {
            _requests[label] = RequestState.Finished;
            IsBusy = false;
        }

        protected async void StartWebRequest([CallerMemberName]string label = "")
        {
            _requests[label] = RequestState.InProgress;
            await Task.Delay(1000);
            CheckIfRequestIsDone(label);
        }

        private void CheckIfRequestIsDone(string label)
        {
            if (_requests[label] == RequestState.InProgress)
                IsBusy = true;
        }
    }
}