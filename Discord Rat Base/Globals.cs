using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discord_Rat_Base
{
    class Settings
    {
        public static string Bot_Token = ""; // <<< Bot Token 
        public static string Prefixes = "!"; // <<< Command Prefix
        public static string WakeUpMsg = "Yippe i am online"; // <<< Start up Message
        public static ulong Guild = 0; // <<< Discord Server
        public static string appConfig = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData); // <<< Folder Path it saves data
    }

    class Globals
    {
        public static ulong Channel = 0;
        public static string WebHook = "0";
    }
}
