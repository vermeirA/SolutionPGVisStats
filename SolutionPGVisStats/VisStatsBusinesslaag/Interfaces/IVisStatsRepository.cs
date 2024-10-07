using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisStatsBL.Model;

namespace VisStatsBL.Interfaces
{
    public interface IVisStatsRepository
    {
        bool HeeftVissoort(Vissoort vis);
        void SchrijfSoort(Vissoort vis);

        bool HeeftHaven(Haven haven);
        void SchrijfHaven(Haven haven);

        bool IsOpgeladen(string fileName);

        List<Haven> LeesHavens();
        List<Vissoort> LeesVissoorten();

        void SchrijfStatistieken(List<VisStatsDataRecord> data, string fileName);
    }
}
