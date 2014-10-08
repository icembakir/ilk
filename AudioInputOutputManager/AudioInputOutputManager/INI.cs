using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace AudioInputOutputManager
{
    class INI
    {
        public static INI GetInstance()
        {
            if (ini == null)
            {
                ini = new INI();
            }
            return ini;
        }

        private INI()
        {
            StreamReader sr = new StreamReader(filename);
            while (!sr.EndOfStream)
            {
                string line = sr.ReadLine();
                string[] lines = line.Split('=');
                if (lines.Length == 2)
                {
                    string key = lines[0].Trim().Replace("[", "").Replace("]", "").Trim();
                    string value = lines[1].Trim().Replace("\"", "").Trim();
                    iniValues[key] = value;
                }
            }
        }

        public string GetValue(String key)
        {
            return iniValues[key];
        }

        private static INI ini=null;
        private static string filename = "Audio.ini";
        private Dictionary<string, string> iniValues = new Dictionary<string, string>();
    }
}
