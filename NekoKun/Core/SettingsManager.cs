using System;
using System.Collections.Generic;
using System.Text;

namespace NekoKun
{
    public static class SettingsManager
    {
        public static SettingsFile GlobalSettings;
        public static SettingsFile ProjectSettings;

        private static SettingsHelper helper;

        static SettingsManager()
        {
            GlobalSettings = new SettingsFile(
                System.IO.Path.Combine(
                    StorageManager.GetUserDirectory("Settings"),
                    "Settings.xml"
                ),
                false
            );
        }

        public static SettingsHelper Settings
        {
            get { return helper != null ? helper : (helper = new SettingsHelper()); }
        }

        public static T GetSetting<T>(string key)
        {
            if (ProjectSettings != null && ProjectSettings[key] is T)
                return (T)ProjectSettings[key];
            if (GlobalSettings != null && GlobalSettings[key] is T)
                return (T)GlobalSettings[key];
            return default(T);
        }

        public class SettingsHelper
        {
            internal SettingsHelper() { }
            public object this[string key]
            {
                get
                {
                    object re;
                    if (ProjectSettings != null)
                    {
                        re = ProjectSettings[key];
                        if(re != null) return re;
                    }
                    if (GlobalSettings != null)
                    {
                        re = GlobalSettings[key];
                        if (re != null) return re;
                    }
                    return null;
                }
            }
        }
    }
}

/*
 * 原来雷达喵是可爱的男孩子～
 */