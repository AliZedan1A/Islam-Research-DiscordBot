using Discord;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IslamReasearchBot.DataModules
{
    [PrimaryKey(nameof(Id))]
    internal class ServersLog
    {
        public int Id { get; set; }
        public ulong? ServerID { get; set; }
        public string? Title { get; set; }
        public string ?Content { get; set; }
        public DateTime PublishDate { get; set; }
        public ulong? ChannelID { get; set; }
        public bool IsPublish { get; set; }
        public ulong? UserSheredID { get; set; }
        public string RoleName { get; set; }

    }
}
