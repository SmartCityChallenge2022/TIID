using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TIID
{
    public class Stanica
    {
        public string NazivStanice { get; set; }
        public int VrijemeDoIduce { get; set; }
        public Stanica(string naziv, int vrijeme)
        {
            NazivStanice = naziv;
            VrijemeDoIduce = vrijeme;
        }
    }
}
