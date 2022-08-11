using System;
using WorkingService.AppService.Interfaces.Settings;

namespace WorkingService.AppService.Services.Settings
{
    public class SettingsService : ISettingsService
    {
        public SettingsService()
        {
            FileName = @"C:\scripts\WorkingScript.script";
        }

        public string FileName { get; set; }
    }
}
