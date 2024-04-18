using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PVegas2K25ProTour
{
    public class PlayerRecord
    {
        public string User { get; set; }
        public int Strokes { get; set; }

        public int Coins { get; set; }

        public int TotalHolesCompleted { get; set; }

        public int TotalStrokesLifetime {  get; set; }
    }

}
