using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IslamReasearchBot.DataModules
{
    [PrimaryKey(nameof(ID))]
    internal class ServerConfigrationModuel
    {
        public int ID { get; set; }
        public ulong ServerID { get; set; }
        public bool IsActive { get; set; }
    }
}