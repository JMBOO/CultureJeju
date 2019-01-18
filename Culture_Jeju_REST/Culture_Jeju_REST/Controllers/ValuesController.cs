using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MySql.Data.MySqlClient;
using System.Xml.Linq;

namespace Culture_Jeju_REST.Controllers
{

    public class results
    {
        public string Title { get; set; }
        public string Travel_name { get; set; }
        public string Travel_addr { get; set; }
        public string Travel_call { get; set; }

        public results(string Title, string document_srl)
        {
            

            MySqlConnection conn = WebApiConfig.conn();
            MySqlCommand query = conn.CreateCommand();
            MySqlDataReader fetch_query = null;
            query.CommandText = "SELECT * FROM xe_document_extra_vars WHERE module_srl = 68 AND document_srl = "+ document_srl;
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
                    this.Title = Title;
                    if(fetch_query["var_idx"].ToString()=="1")
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
                }
                conn.Close();
            }
        }
    }

    public class ValuesController : ApiController
    {
        // GET api/values
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        public List<results> Get(int id)
        {
            MySqlConnection conn = WebApiConfig.conn();
            MySqlCommand query = conn.CreateCommand();
            MySqlDataReader fetch_query = null;
            query.CommandText = "SELECT title, document_srl FROM xe_documents WHERE module_srl = 68";
            var results = new List<results>();
            try
            {
                conn.Open();
                fetch_query = query.ExecuteReader();
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                results.Add(new results(null, ex.ToString()));
            }

            if(fetch_query != null)
            {
                while (fetch_query.Read())
                {
                    results.Add(new results(fetch_query["title"].ToString(), fetch_query["document_srl"].ToString()));
                }
                conn.Close();
            }
            else
            {
                results.Add(new results(null, "연결에러"));
            }


            return results;
        }

        // POST api/values
        public void Post([FromBody]dynamic value)
        {
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}
