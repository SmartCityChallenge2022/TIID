using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TIID
{
    public class WeatherApiClass
    {
        public List <string> coord { get; set; }
        public List <string> weather { get; set; }
        public List<string> baza { get; set; }
        public List <string> main { get; set; }
        public List<string> visibility { get; set; }
        public List <string> wind { get; set; }
        public List <string> clouds { get; set; }
        public List<string> dt { get; set; }
        public List <string> sys { get; set; }
        public List<string> timezone { get; set; }
        public List<string> id { get; set; }
        public List<string> name { get; set; }
        public List<string> cod { get; set; }
    }
}
