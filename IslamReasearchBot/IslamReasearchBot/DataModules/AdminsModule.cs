using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IslamReasearchBot.DataModules
{
    [PrimaryKey(nameof(ID))]
    internal class AdminsModule
    {
        public int ID { get; set; }

        [MaxLength(20)]
        public ulong ServerId { get; set; }
        [MaxLength(20)]
        public ulong UserId { get; set; }
    }
}
