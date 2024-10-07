using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using VisStatsBL.Interfaces;
using VisStatsBL.Model;
using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace VisStatsDL_File
{
    public class FileProcessor : IFileProcessor
    {
        public List<string> LeesSoorten(string fileName)
        {
            try
            {
                List<string> soorten = new List<string>();
                using (StreamReader sr = new StreamReader(fileName))
                {
                    string line;

                    while ((line = sr.ReadLine()) != null)
                    {
                        soorten.Add(line.Trim());
                    }
                }

                return soorten;

            }
            catch (Exception ex)
            {
                throw new Exception("FileProcessor-LeesSoorten", ex); //zowel custom bericht als originele error zal getoont worden
            }
        }

        public List<string> LeesHavens(string fileName)
        {
            try
            {
                List<string> havens = new List<string>();
                using (StreamReader sr = new StreamReader(fileName))
                {
                    string line;

                    while((line = sr.ReadLine()) != null)
                    {
                        havens.Add(line.Trim());
                    }

                    return havens;
                }
                
            } catch (Exception ex)
            {
                throw new Exception("FileProcessor-LeesHavens", ex);
            }
        }

        public List<VisStatsDataRecord> LeesStatistieken(string fileName, List<Vissoort> vissoorten, List<Haven> havens)
        {
            try
            {
                Dictionary<string, Vissoort> soortenD = vissoorten.ToDictionary(x => x.Naam, x => x); //laatste stukje met x, is een landa functie.
                Dictionary<string, Haven> havensD = havens.ToDictionary(x => x.Naam, x => x);// neem de 'naam' van wat we binnenkrijgen (key1), steek het object in de dictionary (key2)

                Dictionary<(string, int, int, string), VisStatsDataRecord> data = new(); //haven,jaar,maand,soort (key1) is een tuple (variabele zonder naam)
                using (StreamReader sr = new StreamReader(fileName))
                {
                    string line;
                    int jaar = 0, maand = 0;
                    List<string> havensTXT = new List<string>();
                    while ((line = sr.ReadLine()) != null)
                    {
                        //lees tot het begin van de maand wordt gevonden
                        //------------202201-------------
                        if (Regex.IsMatch(line, @"^-+\d{6}-+")) //beginnen met meerdere streepkes (-) dan 6 cijfers en dan weer meerdere streepkes (-)
                        { // ^ staat voor beginnen met - en + staat voor meerdere -

                            jaar = Int32.Parse(Regex.Match(line, @"\d{4}").Value);
                            maand = Int32.Parse(Regex.Match(line, @"(\d{2})-+").Groups[1].Value); //De extra haakjes is voor groepjes te maken, Groups[0] is het volledige lijntje, dan hebben we een groep gemaakt door die haakjes hierdoor nemen we enkel de eerste 2 cijfers.

                            havensTXT.Clear(); // lijst leegmaken omdat we aan een nieuwe maand/jaar begonnen zijn.
                        }

                        //lees de verschillende havens in
                        //vissoorten|Totaal van de havens||Oostende||Zeebrugge||Niewpoort||
                        else if (line.Contains("Vissoorten|Totaal van de havens"))
                        {
                            //We steken onze regex in een variabele omdat we hem meedere keren gaan moeten herhalen.
                            string pattern = @"\|([A-Za-z]+)\|"; // ([A-Za-z]+) betekent, meerdere hoofdletters en letters
                            MatchCollection matches = Regex.Matches(line, pattern);// zoek in de lijn het bovenstaande patroon, als je er meerdere vindt stop die in een collection(MatchCollection).
                            foreach (Match match in matches) //Collection overlopen en in een lijst stoppen.
                            {
                                havensTXT.Add(match.Groups[1].Value); //Groups[1] slaat weer op het groepje die we gemaakt hebben in de REGEX expressie, (De 'ronde haakjes' )
                            }
                        }

                        //lees data
                        //Schelvis|5521|11318,0199999999999999997|1828&4987,73000000000005|3693|6330,2900000002|-|-
                        else
                        {
                            string[] element = line.Split('|');
                            //eerste element is de naam van de vissoort dus element opzoeken in onze dictionary van vissoorten.
                            if (soortenD.ContainsKey(element[0]))
                            {
                                for (int i = 0; i < havensTXT.Count; i++)
                                {
                                    if (havensD.ContainsKey(havensTXT[i]))
                                    {
                                        if (data.ContainsKey((havensTXT[i], jaar, maand, element[0])))
                                        // Als de data de key al bevat kunnen we gewoon toevoegen.
                                        {
                                            data[(havensTXT[i], jaar, maand, element[0])].Gewicht += ParseValue(element[(i * 2) + 3]);
                                            data[(havensTXT[i], jaar, maand, element[0])].Waarde += ParseValue(element[(i * 2) + 4]);
                                        }
                                        else
                                        // bevat de data niet de nodige key dan zitten we op de lijn 'andere Soorten' en deze hebben we niet nodig.
                                        {
                                            data.Add((havensTXT[i], jaar, maand, element[0]), new VisStatsDataRecord(soortenD[element[0]], havensD[havensTXT[i]], jaar, maand, ParseValue(element[(i * 2) + 3]), ParseValue(element[(i * 2) + 4])));
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                return data.Values.ToList();
            }
            catch (Exception ex)
            {

                throw new Exception("Fileprocessor-LeesMonthlyResults", ex);
            }
        }

        private double ParseValue(string value) //Voor het geval dat er geen waarde wordt ingelezen (-) deze moeten we veranderen door 0.0
        {
            if (double.TryParse(value, out double d)) return d;
            else return 0.0;
        }
    }
}
