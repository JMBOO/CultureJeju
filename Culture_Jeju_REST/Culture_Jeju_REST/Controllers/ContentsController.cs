using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Culture_Jeju_REST.Controllers
{


    public class Contents
    {
        public string Title { get; set; }
        public string Travel_name { get; set; }
        public string document_srl { get; set; }
        public string Travel_addr { get; set; }
        public string Travel_call { get; set; }
        public string Content { get; set; }


        public Contents(string Title, string document_srl, string Content)
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
                    this.Title = Title;
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
                    this.Content = Content;
                }
                conn.Close();
            }
        }

    }
    public class ContentsController : ApiController
    {

        // GET api/values
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        public List<Contents> Get(int id)
        {
            MySqlConnection conn = WebApiConfig.conn();
            MySqlCommand query = conn.CreateCommand();
            MySqlDataReader fetch_query = null;
            query.CommandText = "SELECT title, document_srl, content FROM xe_documents WHERE module_srl = 68 and document_srl = "+ id.ToString();
            var results = new List<Contents>();
            try
            {
                conn.Open();
                fetch_query = query.ExecuteReader();
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                results.Add(new Contents(null, null, ex.ToString()));
            }

            if(fetch_query != null)
            {
                while (fetch_query.Read())
                {
                    results.Add(new Contents(fetch_query["title"].ToString(), fetch_query["document_srl"].ToString(), fetch_query["content"].ToString()));
                }
                conn.Close();
            }
            else
            {
                results.Add(new Contents(null, null, "연결에러"));
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
