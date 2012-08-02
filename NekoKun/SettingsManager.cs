using System;
using System.Collections.Generic;
using System.Text;

namespace NekoKun
{
    public static class SettingsManager
    {
        public static SettingsFile GlobalSettings;
        public static SettingsFile ProjectSettings;
        private static string settingDirectory = System.IO.Path.Combine(System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "NekoKun"), "Settings");
        private static string settingFilename = System.IO.Path.Combine(settingDirectory, "Settings.xml");
        private static SettingsHelper helper;

        static SettingsManager()
        {
            if (!System.IO.Directory.Exists(settingDirectory))
            {
                System.IO.Directory.CreateDirectory(settingDirectory);
            }
            GlobalSettings = new SettingsFile(settingFilename);
            GlobalSettings.IsProjectFile = false;
        }

        public static SettingsHelper Settings
        {
            get { return helper != null ? helper : (helper = new SettingsHelper()); }
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