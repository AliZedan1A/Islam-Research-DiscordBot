using Discord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IslamReasearchBot.DataModules
{
    internal class Resarch
    {
        public string Title { get {return Title; }
            set { }
        }
        public ComponentBuilder GUI { get; set; }
        public List<string> Imgs { get; set; }
        public void AddImg(string URL)
        {
            Imgs.Add(URL);
        }
        public DateTime PublishDate { get; set; }
        public ulong UserSheredID { get; set; }

    }
}
