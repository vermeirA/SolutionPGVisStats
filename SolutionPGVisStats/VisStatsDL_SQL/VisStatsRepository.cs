using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisStatsBL.Interfaces;
using VisStatsBL.Model;

namespace VisStatsDL_SQL
{
    public class VisStatsRepository : IVisStatsRepository
    {
        private string connectionString;

        public VisStatsRepository(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public bool HeeftVissoort(Vissoort vis)
        {
            string SQL = "Select count(*) from Soort where naam = @naam";

            using (SqlConnection conn = new SqlConnection(connectionString))

            using (SqlCommand cmd = conn.CreateCommand())
            {
                try
                {
                    conn.Open();
                    cmd.CommandText = SQL;
                    cmd.Parameters.Add(new SqlParameter("@naam", SqlDbType.NVarChar));
                    cmd.Parameters["@naam"].Value = vis.Naam;

                    // ExcecuteScalar: Eerste kolom eerste rij, willen een waarde opvragen!
                    int n = (int)cmd.ExecuteScalar();
                    if (n > 0) return true; else return false;

                }
                catch (Exception ex)
                {
                    throw new Exception("HeeftVisSoort", ex);
                }
            }
        }

        public void SchrijfSoort(Vissoort vis)
        {
            //SQL Data importeren 

            string SQL = "INSERT INTO [Soort](naam) VAlUES(@naam)";

            // connectie gaat een commando creëeren

            using (SqlConnection conn = new SqlConnection(connectionString)) 

                using(SqlCommand cmd = conn.CreateCommand()) 
            {
                try
                {
                    conn.Open();
                    cmd.CommandText = SQL;
                    cmd.Parameters.Add(new SqlParameter("@naam", SqlDbType.NVarChar));
                    cmd.Parameters["@naam"].Value = vis.Naam;

                    // ExecuteNonQuery want we hoeven hier geen query uit te voeren, enkel de naam in de lijst zetten.
                    cmd.ExecuteNonQuery();

                } 
                catch (Exception ex)
                {
                    throw new Exception("SchrijfSoort", ex);
                }
            }
        }

        public bool HeeftHaven(Haven haven)
        {
            string SQL = "Select count(*) from Haven where naam = @naam";

            using (SqlConnection conn = new SqlConnection(connectionString))

            using (SqlCommand cmd = conn.CreateCommand())
            {
                try
                {
                    conn.Open();
                    cmd.CommandText = SQL;
                    cmd.Parameters.Add(new SqlParameter("@naam", SqlDbType.NVarChar));
                    cmd.Parameters["@naam"].Value = haven.Naam;

                    // ExcecuteScalar: Eerste kolom eerste rij, willen een waarde opvragen!
                    int n = (int)cmd.ExecuteScalar();
                    if (n > 0) return true; else return false;

                }
                catch (Exception ex)
                {
                    throw new Exception("HeeftVisSoort", ex);
                }
            }
        }

        public void SchrijfHaven(Haven haven)
        {
            //SQL Data importeren 

            string SQL = "INSERT INTO [Haven](naam) VAlUES(@naam)";

            // connectie gaat een commando creëeren

            using (SqlConnection conn = new SqlConnection(connectionString))

            using (SqlCommand cmd = conn.CreateCommand())
            {
                try
                {
                    conn.Open();
                    cmd.CommandText = SQL;
                    cmd.Parameters.Add(new SqlParameter("@naam", SqlDbType.NVarChar));
                    cmd.Parameters["@naam"].Value = haven.Naam;

                    // ExecuteNonQuery want we hoeven hier geen query uit te voeren, enkel de naam in de lijst zetten.
                    cmd.ExecuteNonQuery();

                }
                catch (Exception ex)
                {
                    throw new Exception("SchrijfHaven", ex);
                }
            }
        }
    }
}
