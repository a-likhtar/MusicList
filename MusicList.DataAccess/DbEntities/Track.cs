using System;
using System.Collections.Generic;
using System.Text;

namespace MusicList.DataAccess.DbEntities
{
    public class Track
    {
        public int Id { get; set; }
        public string Performer { get; set; }
        public string Name { get; set; }
        public string Genre { get; set; }
        public DateTime Year { get; set; }
    }
}
