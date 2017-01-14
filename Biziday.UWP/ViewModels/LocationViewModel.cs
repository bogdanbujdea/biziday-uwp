using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Biziday.UWP.Modules.App.Dialogs;
using Biziday.UWP.Modules.News.Models;
using Biziday.UWP.Modules.News.Services;
using Biziday.UWP.Repositories;

namespace Biziday.UWP.ViewModels
{
    public class LocationViewModel : ViewModelBase
    {
        private readonly ISettingsRepository _settingsRepository;
        private readonly INewsClassifier _newsClassifier;
        private readonly IUserNotificationService _userNotificationService;
        private ObservableCollection<Area> _areas;
        private Area _selectedArea;
        private bool _moldovaIsChecked;
        private bool _europeIsChecked;
        private bool _otherIsChecked;

        public LocationViewModel(ISettingsRepository settingsRepository, INewsClassifier newsClassifier, IUserNotificationService userNotificationService)
        {
            _settingsRepository = settingsRepository;
            _newsClassifier = newsClassifier;
            _userNotificationService = userNotificationService;
        }

        protected override async void OnActivate()
        {
            base.OnActivate();
            try
            {
                StartWebRequest();
                var areasReport = await _newsClassifier.RetrieveAreas();
                if (areasReport.IsSuccessful)
                {
                    Areas = new ObservableCollection<Area>(areasReport.Content);
                }
                else await _userNotificationService.ShowErrorMessageDialogAsync(areasReport.ErrorMessage);
                SetPreviousChoice();
            }
            catch (Exception)
            {

            }
            finally
            {
                EndWebRequest();
            }
        }

        private void SetPreviousChoice()
        {
            var areaId = _settingsRepository.GetData<int>(SettingsKey.AreaId);
            if (areaId != 0)
            {
                var state = Areas.FirstOrDefault(a => a.Id == areaId);
                if (state != null)
                {
                    SelectedArea = state;
                }
                else
                {
                    if (areaId == NewsClassifier.AreaMoldova)
                        MoldovaIsChecked = true;
                    else if (areaId == NewsClassifier.AreaExternEurope)
                        EuropeIsChecked = true;
                    else if (areaId == NewsClassifier.AreaOtherContinent)
                        OtherIsChecked = true;
                }
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
                    SelectState(value);
                }
                else
                {
                    _selectedArea = null;
                }
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

        private async void SelectState(Area area)
        {
            if (await ChangeLocation(area.Id))
            {
                _selectedArea = area;
                MoldovaIsChecked = false;
                EuropeIsChecked = false;
                OtherIsChecked = false;
                _settingsRepository.SetData(SettingsKey.AreaId, area.Id);
                NotifyOfPropertyChange(() => SelectedArea);
            }
        }

        private async Task<bool> ChangeLocation(int areaId)
        {
            try
            {
                StartWebRequest();
                var report = await _newsClassifier.SelectArea(areaId);
                if (report.IsSuccessful == false)
                {
                    await _userNotificationService.ShowErrorMessageDialogAsync(report.ErrorMessage);
                }
                else
                {
                    _settingsRepository.SetData(SettingsKey.AreaId, areaId);
                }
                return report.IsSuccessful;
            }
            finally
            {
                EndWebRequest();
            }
        }

        public async Task SelectMoldova()
        {
            if (await ChangeLocation(NewsClassifier.AreaMoldova))
            {
                MoldovaIsChecked = true;
                SelectedArea = null;
            }
            else MoldovaIsChecked = false;
        }

        public async Task SelectEurope()
        {
            if (await ChangeLocation(NewsClassifier.AreaExternEurope))
            {
                EuropeIsChecked = true;
                SelectedArea = null;
            }
            else EuropeIsChecked = false;
        }

        public async Task SelectOtherContinent()
        {
            if (await ChangeLocation(NewsClassifier.AreaOtherContinent))
            {
                OtherIsChecked = true;
                SelectedArea = null;
            }
            else OtherIsChecked = false;
        }
    }

    internal enum RequestState
    {
        InProgress,
        Finished
    }
}