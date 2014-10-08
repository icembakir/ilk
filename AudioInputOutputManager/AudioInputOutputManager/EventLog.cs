using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace AudioInputOutputManager
{
    class EventLog
    {
        public EventLog(string filename)
        {
            this.filename = filename;
        }

        public void WriteEntry(String entry)
        {
            //try to delete the file created 1 week ago
            string oldFilePath = this.filename + DateTime.Now.AddDays(-7).ToString("yyyyMMdd");
            if (File.Exists(oldFilePath))
                File.Delete(oldFilePath);

            File.AppendAllText(filename + DateTime.Now.ToString("yyyyMMdd"), DateTime.Now.ToString("[dd.MM.yyyy HH:mm]")+" "+entry+"\r\n");
        }

        private string filename;
    }
}
