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
        private string _connectionString;

        public VisStatsRepository(string connectionString)
        {
            this._connectionString = connectionString;
        }

        public bool HeeftVissoort(Vissoort vis)
        {
            string SQL = "Select count(*) from Soort where naam = @naam";

            using (SqlConnection conn = new SqlConnection(_connectionString))

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

            using (SqlConnection conn = new SqlConnection(_connectionString)) 

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

            using (SqlConnection conn = new SqlConnection(_connectionString))

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

            using (SqlConnection conn = new SqlConnection(_connectionString))

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

        public bool IsOpgeladen(string fileName)
        {
            string SQL = "SELECT Count(*) FROM upload WHERE filename=@filename"; // @ is om dat we met een parameter werken.
            using (SqlConnection conn = new SqlConnection(_connectionString))
            using (SqlCommand cmd = conn.CreateCommand())
            {
                try
                {
                    conn.Open();
                    cmd.CommandText = SQL;
                    cmd.Parameters.Add(new SqlParameter("@filename", System.Data.SqlDbType.NVarChar));
                    cmd.Parameters["@filename"].Value = fileName.Substring(fileName.LastIndexOf("\\") + 1);
                    // cmd.Parameters.AddWithValue("@filename", filename.Substring(filename.LastIndexOf("\\") + 1)); kortere schrijfwijze voor vorige 2 lijnen. 
                    int n = (int)cmd.ExecuteScalar();
                    if (n > 0) return true; else return false;
                }
                catch (Exception ex)
                {
                    throw new Exception("IsOpgeladen", ex);
                }

            }
        }

        public List<Haven> LeesHavens()
        {
            string SQL = "SELECT * FROM Haven";
            List<Haven> havens = new List<Haven>();
            using (SqlConnection conn = new SqlConnection(_connectionString))
            using (SqlCommand cmd = conn.CreateCommand())
            {
                try
                {
                    conn.Open();
                    cmd.CommandText = SQL;
                    IDataReader reader = cmd.ExecuteReader(); //Om aan te duiden dat hij moet lezen en niet wegschrijven.
                    while (reader.Read()) // in een while loop want we willen ze allemaal lezen.
                    {
                        havens.Add(new Haven((string)reader["naam"], (int)reader["id"]));
                    }
                    return havens;
                }
                catch (Exception ex)
                {

                    throw new Exception("LeesHavens", ex);
                }
            }
        }

        public List<Vissoort> LeesVissoorten()
        {
            string SQL = "SELECT * FROM Soort";
            List<Vissoort> soorten = new List<Vissoort>();
            using (SqlConnection conn = new SqlConnection(_connectionString))
            using (SqlCommand cmd = conn.CreateCommand())
            {
                try
                {
                    conn.Open();
                    cmd.CommandText = SQL;
                    IDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        soorten.Add(new Vissoort((string)reader["naam"], (int)reader["id"]));
                    }
                    return soorten;
                }
                catch (Exception ex)
                {

                    throw new Exception("LeesVissoorten", ex);
                }
            }
        }

        public void SchrijfStatistieken(List<VisStatsDataRecord> data, string fileName)
        {
            string SQLdata = "INSERT INTO VisStats(jaar,maand,haven_id,soort_id,gewicht,waarde) VALUES (@jaar,@maand,@haven_id,@soort_id,@gewicht,@waarde)";
            string SQLupload = "INSERT INTO Upload(filename,datum,path) VALUES (@filename, @datum, @path)";
            using (SqlConnection conn = new SqlConnection(_connectionString))
            using (SqlCommand cmd = conn.CreateCommand())
            {
                try
                {
                    conn.Open();
                    cmd.CommandText = SQLdata;
                    cmd.Transaction = conn.BeginTransaction(); // omdat we bijde tabellen willen updaten EN ze MOETEN alletwee slagen anders willen we niet uploaden.
                    cmd.Parameters.Add(new SqlParameter("@jaar", SqlDbType.Int)); // parameters aanmaken.
                    cmd.Parameters.Add(new SqlParameter("@maand", SqlDbType.Int));
                    cmd.Parameters.Add(new SqlParameter("@haven_id", SqlDbType.Int));
                    cmd.Parameters.Add(new SqlParameter("@soort_id", SqlDbType.Int));
                    cmd.Parameters.Add(new SqlParameter("@gewicht", SqlDbType.Float)); // double in cSharp is een float in sql server
                    cmd.Parameters.Add(new SqlParameter("@waarde", SqlDbType.Float));
                    foreach (VisStatsDataRecord d in data)
                    {
                        cmd.Parameters["@jaar"].Value = d.Jaar;
                        cmd.Parameters["@maand"].Value = d.Maand;
                        cmd.Parameters["@haven_id"].Value = d.Haven.Id;
                        cmd.Parameters["@soort_id"].Value = d.Soort.Id;
                        cmd.Parameters["@gewicht"].Value = d.Gewicht;
                        cmd.Parameters["@waarde"].Value = d.Waarde;
                        cmd.ExecuteNonQuery();
                    }
                    cmd.CommandText = SQLupload;
                    cmd.Parameters.Clear(); // nu willen we andere parameters dus we moeten deze clearen.
                    cmd.Parameters.AddWithValue("@filename", fileName.Substring(fileName.LastIndexOf("\\") + 1)); //Hier gebruiken we addWithValue want we moeten het maar 1 keer doen. Bij het vorige zou hij dan elke keer een nieuwe variabele weer aanmaken wat 'zwaar' is voor het programma.
                    cmd.Parameters.AddWithValue("@path", fileName.Substring(fileName.LastIndexOf("\\") + 1));
                    cmd.Parameters.AddWithValue("@datum", DateTime.Now);
                    cmd.ExecuteNonQuery();
                    cmd.Transaction.Commit(); // bijde wegschrijven naar de juiste DB
                }
                catch (Exception ex)
                {
                    cmd.Transaction.Rollback(); // Als oftewel de upload of de visstats niet lukt mogen ze alletwee niet uitgevoerd worden.
                    throw new Exception("SchrijfStatistieken", ex);
                }
            }
        }
        private void ExecuteNonQuery(string query, string parameterName, string parameterValue)
        {
            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand(query, conn))
            {
                try
                {
                    conn.Open();
                    cmd.Parameters.Add(new SqlParameter(parameterName, System.Data.SqlDbType.NVarChar) { Value = parameterValue });
                    cmd.ExecuteNonQuery();
                }
                catch (SqlException ex)
                {
                    throw new Exception("An error occurred while executing a database operation.", ex);
                }
            }
        }

        private bool CheckRecordExists(string query, string parameterName, string parameterValue)
        {
            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand(query, conn))
            {
                try
                {
                    conn.Open();
                    cmd.Parameters.Add(new SqlParameter(parameterName, System.Data.SqlDbType.NVarChar) { Value = parameterValue });
                    var result = (int)cmd.ExecuteScalar();
                    return result > 0;
                }
                catch (SqlException ex)
                {
                    throw new Exception("An error occurred while checking for record existence.", ex);
                }
            }
        }
    }
}
