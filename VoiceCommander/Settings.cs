using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoiceCommander
{
    static class Settings
    {
        public static string CompatibleCultureInfo = "en-US";
        public static string CompatibleISOLanguageName = "en";

        public static string HomePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "VoiceCommander");
        public static string ConfigFilesPath = Path.Combine(HomePath, "Configs");
        public static string ExampleConfigPath = Path.Combine(ConfigFilesPath, "example.conf");

        public static string TitleThumbnail = @"
 __      __   _           _____                                          _           
 \ \    / /  (_)         / ____|                                        | |          
  \ \  / /__  _  ___ ___| |     ___  _ __ ___  _ __ ___   __ _ _ __   __| | ___ _ __ 
   \ \/ / _ \| |/ __/ _ \ |    / _ \| '_ ` _ \| '_ ` _ \ / _` | '_ \ / _` |/ _ \ '__|
    \  / (_) | | (_|  __/ |___| (_) | | | | | | | | | | | (_| | | | | (_| |  __/ |   
     \/ \___/|_|\___\___|\_____\___/|_| |_| |_|_| |_| |_|\__,_|_| |_|\__,_|\___|_|   
                                                                                     
                                                                                     ";
    }
}
