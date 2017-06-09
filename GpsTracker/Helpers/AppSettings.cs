using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO.IsolatedStorage;
using System.Windows;
using System.Windows.Media;

namespace GPSTracker.Helpers
{
    public class AppSettings
    {
        // Our isolated storage settings 
        IsolatedStorageSettings settings;

        /// <summary> 
        /// Constructor that gets the application settings. 
        /// </summary> 
        public AppSettings()
        {
            try
            {
                // Get the settings for this application. 
                settings = IsolatedStorageSettings.ApplicationSettings;

            }
            catch (Exception e)
            {
                Debug.WriteLine("Exception while using IsolatedStorageSettings: " + e.ToString());
            }
        }

        /// <summary> 
        /// Update a setting value for our application. If the setting does not 
        /// exist, then add the setting. 
        /// </summary> 
        /// <param name="Key"></param> 
        /// <param name="value"></param> 
        /// <returns></returns> 
        public bool AddOrUpdateValue(string Key, Object value)
        {
            bool valueChanged = false;

            // If the key exists 
            if (settings.Contains(Key))
            {
                // If the value has changed 
                if (settings[Key] != value)
                {
                    // Store the new value 
                    settings[Key] = value;
                    valueChanged = true;
                }
            }
            // Otherwise create the key. 
            else
            {
                settings.Add(Key, value);
                valueChanged = true;
            }

            return valueChanged;
        }


        /// <summary> 
        /// Get the current value of the setting, or if it is not found, set the  
        /// setting to the default setting. 
        /// </summary> 
        /// <typeparam name="valueType"></typeparam> 
        /// <param name="Key"></param> 
        /// <returns></returns> 
        public valueType GetValue<valueType>(string Key)
        {
            valueType value;

            // If the key exists, retrieve the value. 
            if (settings.Contains(Key))
            {
                value = (valueType)settings[Key];
            }
            else
            {
                value = default(valueType);
            }
            return value;
        }


        /// <summary> 
        /// Save the settings. 
        /// </summary> 
        public void Save()
        {
            settings.Save();
        }

        /// <summary> 
        /// Returns true if setting exists. 
        /// </summary> 
        public bool Contains(string Key)
        {
            if(settings.Contains(Key))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}
