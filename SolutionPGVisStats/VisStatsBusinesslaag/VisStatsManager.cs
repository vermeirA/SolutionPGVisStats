using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisStatsBL.Exceptions;
using VisStatsBL.Interfaces;
using VisStatsBL.Model;

namespace VisStatsBL
{
    public class VisStatsManager
    {
        private IFileProcessor fileProcessor;
        private IVisStatsRepository visStatsRepository;

        public VisStatsManager(IFileProcessor fileProcessor, IVisStatsRepository visStatsRepository) //dependency injection
        {
            this.fileProcessor = fileProcessor;
            this.visStatsRepository = visStatsRepository;
        }

        public void UploadVissoorten(string fileName)
        {
            List<string> soorten = fileProcessor.LeesSoorten(fileName);
            List<Vissoort> vissoorten = MaakVissoorten(soorten);

            foreach(Vissoort vis in vissoorten)
            {
                if (!visStatsRepository.HeeftVissoort(vis))
                {
                    visStatsRepository.SchrijfSoort(vis);
                }
            }
        }

        private List<Vissoort> MaakVissoorten(List<string> soorten)
        {
            Dictionary<string, Vissoort> visSoorten = new();

            foreach(string soort in soorten)
            {
                if (!visSoorten.ContainsKey(soort))
                {
                    try { visSoorten.Add(soort, new Vissoort(soort)); }
                    catch (DomeinException)
                    {
                        // do stuff feedback 
                    }
                }
            }

            return visSoorten.Values.ToList();
        }

        public void UploadHavens(string fileName)
        {
            List<string> havens = fileProcessor.LeesHavens(fileName);
            List<Haven> havensoorten = MaakHavens(havens); 

            foreach (Haven haven in havensoorten)
            {
                if (!visStatsRepository.HeeftHaven(haven))
                {
                    visStatsRepository.SchrijfHaven(haven);
                }
            }
        }

        private List<Haven> MaakHavens(List<string> havens)
        {
            Dictionary<string, Haven> havenSoorten = new();

            foreach (string haven in havens)
            {
                if (!havenSoorten.ContainsKey(haven))
                {
                    try { havenSoorten.Add(haven, new Haven(haven)); }
                    catch (DomeinException)
                    {
                        // do stuff feedback 
                    }
                }
            }

            return havenSoorten.Values.ToList();
        }

        public void UploadStatistieken(string fileName)
        {
            try
            {
                if (!visStatsRepository.IsOpgeladen(fileName))
                {
                    List<Vissoort> soorten = visStatsRepository.LeesVissoorten();
                    List<Haven> havens = visStatsRepository.LeesHavens();
                    List<VisStatsDataRecord> data = fileProcessor.LeesStatistieken(fileName, soorten, havens);
                    visStatsRepository.SchrijfStatistieken(data, fileName);
                }
            }
            catch (Exception ex)
            {
                throw new ManagerExceptions("UploadStatistieken", ex);
            }
        }
    }
}
