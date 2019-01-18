using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Culture_Jeju_REST.Controllers
{
    
    public class ContentsList
    {
        public string Title { get; set; }
        public string imgsrc { get; set; }
        public string Travel_name { get; set; }
        public string Travel_addr { get; set; }
        public string Travel_call { get; set; }
        public string document_srl { get; set; }


        public ContentsList(string Title, string document_srl)
        {

            MySqlConnection conn = WebApiConfig.conn();
            MySqlCommand query = conn.CreateCommand();
            MySqlDataReader fetch_query = null;
            query.CommandText = "select xe_document_extra_vars.module_srl, xe_document_extra_vars.var_idx, " +
                "xe_document_extra_vars.lang_code, xe_document_extra_vars.value, xe_files.uploaded_filename from " +
                "xe_document_extra_vars join xe_files on xe_files.module_srl = xe_document_extra_vars.module_srl " +
                "where xe_document_extra_vars.module_srl = 68 and xe_document_extra_vars.document_srl = " + document_srl +
                " and xe_files.upload_target_srl = " + document_srl+";";
            try
            {
                conn.Open();
                fetch_query = query.ExecuteReader();
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                this.Title = "쿼리에러 : " + ex.Message;
            }

            if (fetch_query != null)
            {
                while (fetch_query.Read())
                {
                    this.Title = Title;
                    this.imgsrc = "jmnass.iptime.org:9880/xe" + fetch_query["uploaded_filename"].ToString();
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
                    this.document_srl = document_srl;
                }
                conn.Close();
            }
        }

    }
    public class ListController : ApiController
    {
        // GET: api/List
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/List/5
        public List<ContentsList> Get(int id)
        {
            MySqlConnection conn = WebApiConfig.conn();
            MySqlCommand query = conn.CreateCommand();
            MySqlDataReader fetch_query = null;
            query.CommandText = "SELECT title, document_srl FROM xe_documents WHERE module_srl = 68";
            var results = new List<ContentsList>();
            try
            {
                conn.Open();
                fetch_query = query.ExecuteReader();
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                results.Add(new ContentsList(null, ex.ToString()));
            }

            if (fetch_query != null)
            {
                while (fetch_query.Read())
                {
                    results.Add(new ContentsList(fetch_query["title"].ToString(), fetch_query["document_srl"].ToString()));
                }
                conn.Close();
            }
            else
            {
                results.Add(new ContentsList(null, "연결에러"));
            }


            return results;
        }

        // POST: api/List
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/List/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/List/5
        public void Delete(int id)
        {
        }
    }
}
