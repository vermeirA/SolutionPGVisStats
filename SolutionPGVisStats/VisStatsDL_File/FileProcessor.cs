﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisStatsBL.Interfaces;

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
    }
}
