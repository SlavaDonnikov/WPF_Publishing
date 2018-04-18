using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Publishing
{
    public class Videos
    {
        public string NewVideoName { get; set; }
        public string NewVideoDuration { get; set; }

        public Videos() { }
        public Videos(string name, string duration)
        {
            NewVideoName = name; NewVideoDuration = duration;
        }
    }
}
