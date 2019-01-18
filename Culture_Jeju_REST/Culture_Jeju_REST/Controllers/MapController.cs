using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Xml.Linq;
using static Culture_Jeju_REST.Parsing_Location;

namespace Culture_Jeju_REST.Controllers
{
    public class Mapresults
    {
        public string Travel_name { get; set; }
        public string Travel_addr { get; set; }
        public string Travel_call { get; set; }
        public string document_srl { get; set; }
        public string lat { get; set; }
        public string lng { get; set; }
        public Mapresults(string document_srl)
        {
            MySqlConnection conn = WebApiConfig.conn();
            MySqlCommand query = conn.CreateCommand();
            MySqlDataReader fetch_query = null;

            query.CommandText = "SELECT * FROM xe_document_extra_vars WHERE module_srl = 68 AND document_srl = " + document_srl;
            try
            {
                conn.Open();
                fetch_query = query.ExecuteReader();
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
            }

            if (fetch_query != null)
            {
                while (fetch_query.Read())
                {
                    this.document_srl = document_srl;
                    if (fetch_query["var_idx"].ToString() == "1")
                    {
                        this.Travel_name = fetch_query["value"].ToString();
                    }
                    if (fetch_query["var_idx"].ToString() == "2")
                    {
                        this.Travel_addr = fetch_query["value"].ToString();
                    }
                    if (fetch_query["var_idx"].ToString() == "3")
                    {
                        this.Travel_call = fetch_query["value"].ToString();
                    }
                    if (fetch_query["var_idx"].ToString() == "4")
                    {
                        this.lat = fetch_query["value"].ToString();
                    }
                    if (fetch_query["var_idx"].ToString() == "5")
                    {
                        this.lng = fetch_query["value"].ToString();
                    }
                }
                conn.Close();
            }
        }
    }
    public class MapController : ApiController
    {
        // GET: api/Map
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Map/5
        public List<Mapresults> Get(int id)
        {
            MySqlConnection conn = WebApiConfig.conn();
            MySqlCommand query = conn.CreateCommand();
            MySqlDataReader fetch_query = null;
            query.CommandText = "SELECT document_srl FROM xe_documents WHERE module_srl = 68";
            var results = new List<Mapresults>();
            try
            {
                conn.Open();
                fetch_query = query.ExecuteReader();
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                results.Add(new Mapresults(ex.ToString()));
            }

            if (fetch_query != null)
            {
                while (fetch_query.Read())
                {
                    results.Add(new Mapresults(fetch_query["document_srl"].ToString()));
                }
                conn.Close();
            }
            else
            {
                results.Add(new Mapresults("연결에러"));
            }


            return results;
        }

        // POST: api/Map
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Map/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Map/5
        public void Delete(int id)
        {
        }
    }
}
