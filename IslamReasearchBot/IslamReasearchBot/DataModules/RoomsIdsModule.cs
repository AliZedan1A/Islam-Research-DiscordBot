using Discord.Interactions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MaxLengthAttribute = System.ComponentModel.DataAnnotations.MaxLengthAttribute;

namespace IslamReasearchBot.DataModules
{
    [PrimaryKey(nameof(ID))]
    internal class CatigoryIdsModule
    {
        public int ID { get; set; }

        [MaxLength(20)]
        public ulong ServerId { get; set; }
        [MaxLength(20)]
        public ulong CatigoryId { get; set; }
    }
}
