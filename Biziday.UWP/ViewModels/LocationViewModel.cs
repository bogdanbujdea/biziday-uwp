using System.Collections.ObjectModel;
using Biziday.UWP.Modules.News.Models;
using Biziday.UWP.Modules.News.Services;
using Biziday.UWP.Repositories;

namespace Biziday.UWP.ViewModels
{
    public class LocationViewModel : ViewModelBase
    {
        private readonly ISettingsRepository _settingsRepository;
        private readonly INewsClassifier _newsClassifier;
        private ObservableCollection<Area> _areas;
        private Area _selectedArea;
        private bool _moldovaIsChecked;
        private bool _europeIsChecked;
        private bool _otherIsChecked;

        public LocationViewModel(ISettingsRepository settingsRepository, INewsClassifier newsClassifier)
        {
            _settingsRepository = settingsRepository;
            _newsClassifier = newsClassifier;
        }

        protected override async void OnActivate()
        {
            base.OnActivate();
            var areasReport = await _newsClassifier.RetrieveAreas();
            if (areasReport.IsSuccessful)
            {
                Areas = new ObservableCollection<Area>(areasReport.Content);
            }
        }

        #region Properties

        public Area SelectedArea
        {
            get { return _selectedArea; }
            set
            {
                if (Equals(value, _selectedArea)) return;
                if (value != null)
                {
                    MoldovaIsChecked = false;
                    EuropeIsChecked = false;
                    OtherIsChecked = false;
                }
                _selectedArea = value;
                NotifyOfPropertyChange(() => SelectedArea);
            }
        }

        public ObservableCollection<Area> Areas
        {
            get { return _areas; }
            set
            {
                if (Equals(value, _areas)) return;
                _areas = value;
                NotifyOfPropertyChange(() => Areas);
            }
        }

        public bool MoldovaIsChecked
        {
            get { return _moldovaIsChecked; }
            set
            {
                if (value == _moldovaIsChecked) return;
                _moldovaIsChecked = value;
                NotifyOfPropertyChange(() => MoldovaIsChecked);
            }
        }

        public bool EuropeIsChecked
        {
            get { return _europeIsChecked; }
            set
            {
                if (value == _europeIsChecked) return;
                _europeIsChecked = value;
                NotifyOfPropertyChange(() => EuropeIsChecked);
            }
        }

        public bool OtherIsChecked
        {
            get { return _otherIsChecked; }
            set
            {
                if (value == _otherIsChecked) return;
                _otherIsChecked = value;
                NotifyOfPropertyChange(() => OtherIsChecked);
            }
        }

        #endregion

        public void SelectMoldova()
        {
            MoldovaIsChecked = true;
            SelectedArea = null;
        }

        public void SelectEurope()
        {
            EuropeIsChecked = true;
            SelectedArea = null;
        }

        public void SelectOtherContinent()
        {
            OtherIsChecked = true;
            SelectedArea = null;
        }
    }
}