using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Controllers
{
    using System.Data;
    using System.Diagnostics;

    using WebApplication1.Models;

    public class Paginator
    {
        private DBHandler DBHandler = new DBHandler();
        public int Entries { get; set; }
        public long Page { get; set; }
        public long Pages { get; set; }
        public long Total { get; set; }

        public Paginator(int entries = 15, int page = 1, int pages = 1)
        {
            this.Entries = entries;
            this.Page = page;
            this.Pages = Pages;
        }

        public List<DataRow> Get(string sql)
        {
            List<DataRow> results = new List<DataRow>();

            // Count all records the query returns
            using (DataTable dt = this.DBHandler.Execute<DataTable>(CRUD.READ, "SELECT COUNT(*) FROM (" + sql + ") T2"))
            {
                if (dt.Rows.Count > 0)
                {
                    int pages = Int32.Parse(dt.Rows[0].ItemArray[0].ToString());

                    if (this.Entries <= 0)
                        this.Entries = 1;

                    this.Pages = (pages + this.Entries - 1) / this.Entries; // calculate # of pages
                    this.Total = Int32.Parse(dt.Rows[0].ItemArray[0].ToString());

                    if (this.Page <= this.Pages) // check if current page fits in the number of pages
                    {

                        sql += " ORDER BY SCOPE_IDENTITY() DESC OFFSET " + this.Entries * (this.Page - 1) +
                               " ROWS FETCH NEXT " + this.Entries + " ROWS ONLY ";
                        using (DataTable res = this.DBHandler.Execute<DataTable>(CRUD.READ, sql))
                        {
                            foreach (DataRow row in res.Rows)
                            {
                                results.Add(row);
                            }
                        }
                    }
                }
                else
                {
                    this.Pages = 1;
                    this.Total = 0;
                }
            }

            return results;
        }
    }
}