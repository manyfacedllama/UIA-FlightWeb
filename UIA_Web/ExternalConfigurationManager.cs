using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using UIA_Web.SettingsStore;
namespace UIA_Web
{ 

    public class ExternalConfigurationManager
    {
        // An abstraction of the configuration store.
        private readonly ISettingsStore settings;
        private readonly ISubject<KeyValuePair<string, string>> changed;

        private readonly TimeSpan interval;


        private Dictionary<string, string> settingsCache;
        private string currentVersion;

        public ExternalConfigurationManager() : this(new BlobSettingsStore(), TimeSpan.FromSeconds(15))
        {
        }

        public ExternalConfigurationManager(ISettingsStore settings, TimeSpan interval)
        {
            this.settings = settings;
            this.interval = interval;
            this.CheckForConfigurationChanges();
            this.changed = new Subject<KeyValuePair<string, string>>();
        }



        public IObservable<KeyValuePair<string, string>> Changed => this.changed.AsObservable();
        public string GetAppSetting(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException(nameof(key), "Value cannot be null or empty.");
            }


            string value;

                this.settingsCache.TryGetValue(key, out value);

            return value;
        }
        public void ConfigChangeMonitor()
        {
                this.CheckForConfigurationChanges();
            }

        private void CheckForConfigurationChanges()
        {
            try
            {
                // It is assumed that updates are infrequent.
                // To avoid race conditions in refreshing the cache synchronize access to the in memory cache

                var latestVersion = this.settings.GetVersion();

                // If the versions are the same, nothing has changed in the configuration.
                if (this.currentVersion == latestVersion) return;

                // Get the latest settings from the settings store and publish changes.
                var latestSettings = this.settings.FindAll();

                // Refresh the settings cache.
                    if (this.settingsCache != null)
                    {
                        //Notify settings changed
                        latestSettings.Except(this.settingsCache).ToList().ForEach(kv => this.changed.OnNext(kv));
                    }
                    this.settingsCache = latestSettings;
              
                // Update the current version.
                this.currentVersion = latestVersion;
            }
            catch (Exception ex)
            {
                this.changed.OnError(ex);
            }
        }
    }
}
