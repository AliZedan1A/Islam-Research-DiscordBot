using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IslamReasearchBot.DataModules
{
    [PrimaryKey(nameof(Id))]
    internal class ImagesModualDB
    {
        public int Id { get; set; }
        public string ImageTitle { get; set; }
        public string ImageURL { get; set; }
        public int ResarchID { get; set; }

    }
}
