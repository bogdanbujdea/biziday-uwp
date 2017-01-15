using System.Collections.Generic;

namespace Biziday.Core.Modules.App.Analytics
{
    public interface IStatisticsService
    {
        void RegisterPage(string page);
        Dictionary<EventCategory, string> EventCategories { get; set; }
        Dictionary<EventAction, string> EventActions { get; set; }
        void RegisterButtonPress(string button);
        void RegisterEvent(EventCategory category, EventAction action, string eventName);
        void RegisterEvent(EventCategory eventCategory, string action, string eventName);
    }
}