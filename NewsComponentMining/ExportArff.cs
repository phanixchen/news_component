using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using CS_Class;
using System.IO;

namespace NewsComponentMining
{
    public partial class ExportArff : Form
    {
        public ExportArff()
        {
            InitializeComponent();
        }

        private string sql;
        //private SQLConn cn = new SQLConn("10.91.21.40", "sa", "sa", "SE_TEST");
        private SQLConn cn = new SQLConn("localhost", "sa", "lovedddog", "SE");

        private void bExport_Click(object sender, EventArgs e)
        {
            DataTable dt1 = new DataTable();
            DataTable dt2 = new DataTable();
            DataTable dt3 = new DataTable();
            DataTable dt4 = new DataTable();
            int i, j, k, m, n, iTotalAtts;

            StreamWriter sw = new StreamWriter("data.arff", false);

            sw.WriteLine("@relation TermComponent");
            sw.WriteLine("");

            sql = "select distinct T.Termid from (Select NewsDate, NewsID, TermID, count(TermID) as TermCount from NewsComponentMining_NewsTermR " +
                  "WHERE newsdate between '" + tbFrom.Text.Trim() + "' and '" + tbTo.Text.Trim() + "' AND NewsID like '" + tbType.Text.Trim() + "%' " +
                  "Group by NewsDate, NewsID, TermID) as T Where T.TermCount > 1 Order by T.Termid;";
            cn.Exec(sql, ref dt4);

            for (i = 0; i < dt4.Rows.Count; i++)
            {
                sw.WriteLine("@attribute term" + dt4.Rows[i]["Termid"].ToString() + " {min1, 2_3, 4_7, 8_max}");
            }
            iTotalAtts = dt4.Rows.Count;
            //sw.WriteLine("@attribute class string");

            sql = "Select distinct ComponentID from NewsComponentMining_NewsComponentR " + 
                  "WHERE newsdate between '" + tbFrom.Text.Trim() + "' and '" + tbTo.Text.Trim() + "' AND ComponentType = '" + tbComponentType.Text + "';";
            cn.Exec(sql, ref dt1);
            sw.Write("@attribute class {");
            for (i = 0; i < dt1.Rows.Count; i++)
            {
                if (i == 0)
                {
                    sw.Write(dt1.Rows[i]["ComponentID"].ToString());
                }
                else
                {
                    sw.Write("," + dt1.Rows[i]["ComponentID"].ToString());
                }
            }
            sw.WriteLine("}");

            sw.WriteLine("");
            sw.WriteLine("@data");


            sql = "Select * from NewsComponentMining_NewsContent Where newsdate between '" + tbFrom.Text.Trim() + "' and '" + tbTo.Text.Trim() + "' AND NewsID like '" + tbType.Text.Trim() + "%';";
            cn.Exec(sql, ref dt1);

            for (i = 0; i < dt1.Rows.Count; i++)
            {
                sql = "Select * from NewsComponentMining_NewsComponentR Where ComponentType = '" + tbComponentType.Text + "' AND " +
                      "NewsDate = '" + Convert.ToDateTime(dt1.Rows[i]["NewsDate"].ToString()).ToShortDateString() + "' AND NewsID = '" + dt1.Rows[i]["NewsID"].ToString() + "';";
                cn.Exec(sql, ref dt2);


                for (j = 0; j < dt2.Rows.Count; j++)
                {
                    sw.Write("{");

                    for (m = 0; m < dt4.Rows.Count; m++)
                    {
                        sql = "Select count(TermID) as xTermCount from NewsComponentMining_NewsTermR " +
                              "Where NewsDate = '" + Convert.ToDateTime(dt1.Rows[i]["NewsDate"].ToString()).ToShortDateString() + "' AND NewsID = '" + dt1.Rows[i]["NewsID"].ToString() + "' " +
                              "AND TermId = '" + dt4.Rows[m]["TermID"].ToString() + "';";
                        cn.Exec(sql, ref dt3);


                        switch (Convert.ToInt32(dt3.Rows[0]["xTermCount"]))
                        {
                            case 0:
                                break;

                            case 1:
                                sw.Write(m.ToString() + " min1, ");
                                break;

                            case 2:
                            case 3:
                                sw.Write(m.ToString() + " 2_3, ");
                                break;

                            case 4:
                            case 5:
                            case 6:
                            case 7:
                                sw.Write(m.ToString() + " 4_7, ");
                                break;

                            default:
                                sw.Write(m.ToString() + " 8_max, ");
                                break;
                        }
                    }


                    sw.WriteLine(iTotalAtts.ToString() + " " + dt2.Rows[j]["ComponentID"].ToString() + "}");
                }
            }

            sw.Close();

            MessageBox.Show("done");
        }

        private void bExport2_Click(object sender, EventArgs e)
        {
            DataTable dt1 = new DataTable();
            DataTable dt2 = new DataTable();
            DataTable dt3 = new DataTable();
            DataTable dt4 = new DataTable();
            int i, j, k, m, n, iTotalAtts;

            StreamWriter sw = new StreamWriter("data.arff", false);

            sw.WriteLine("@relation TermComponent");
            sw.WriteLine("");

            sql = "select distinct T.Termid from (Select NewsDate, NewsID, TermID, count(TermID) as TermCount from NewsComponentMining_NewsTermR " +
                  "WHERE newsdate between '" + tbFrom.Text.Trim() + "' and '" + tbTo.Text.Trim() + "' AND NewsID like '" + tbType.Text.Trim() + "%' " +
                  "Group by NewsDate, NewsID, TermID) as T Where T.TermCount > 1 Order by T.Termid;";
            cn.Exec(sql, ref dt4);

            sw.Write("@attribute term {");
            for (i = 0; i < dt4.Rows.Count; i++)
            {
                if (i==0)
                {
                    sw.Write(dt4.Rows[i]["Termid"].ToString());
                }
                else
                {
                    sw.Write(", " + dt4.Rows[i]["Termid"].ToString());
                }
            }
            sw.WriteLine("}");
            iTotalAtts = dt4.Rows.Count;
            //sw.WriteLine("@attribute class string");

            sql = "Select distinct ComponentID from NewsComponentMining_NewsComponentR " +
                  "WHERE newsdate between '" + tbFrom.Text.Trim() + "' and '" + tbTo.Text.Trim() + "' AND ComponentType = '" + tbComponentType.Text + "';";
            cn.Exec(sql, ref dt1);
            sw.Write("@attribute class {");
            for (i = 0; i < dt1.Rows.Count; i++)
            {
                if (i == 0)
                {
                    sw.Write(dt1.Rows[i]["ComponentID"].ToString());
                }
                else
                {
                    sw.Write("," + dt1.Rows[i]["ComponentID"].ToString());
                }
            }
            sw.WriteLine("}");

            sw.WriteLine("");
            sw.WriteLine("@data");


            sql = "Select * from NewsComponentMining_NewsContent Where newsdate between '" + tbFrom.Text.Trim() + "' and '" + tbTo.Text.Trim() + "' AND NewsID like '" + tbType.Text.Trim() + "%';";
            cn.Exec(sql, ref dt1);

            for (i = 0; i < dt1.Rows.Count; i++)
            {
                sql = "Select * from NewsComponentMining_NewsComponentR Where ComponentType = '" + tbComponentType.Text + "' AND " +
                      "NewsDate = '" + Convert.ToDateTime(dt1.Rows[i]["NewsDate"].ToString()).ToShortDateString() + "' AND NewsID = '" + dt1.Rows[i]["NewsID"].ToString() + "';";
                cn.Exec(sql, ref dt2);


                for (j = 0; j < dt2.Rows.Count; j++)
                {
                    for (m = 0; m < dt4.Rows.Count; m++)
                    {
                        sql = "Select * from NewsComponentMining_NewsTermR " +
                              "Where NewsDate = '" + Convert.ToDateTime(dt1.Rows[i]["NewsDate"].ToString()).ToShortDateString() + "' AND NewsID = '" + dt1.Rows[i]["NewsID"].ToString() + "' " +
                              "AND TermId = '" + dt4.Rows[m]["TermID"].ToString() + "';";
                        cn.Exec(sql, ref dt3);

                        if (dt3.Rows.Count > 0)
                        {
                            sw.WriteLine(dt4.Rows[m]["TermID"].ToString() + ", " + dt2.Rows[j]["ComponentID"].ToString());
                        }
                    }
                }
            }

            sw.Close();

            MessageBox.Show("done");
        }
    }
}