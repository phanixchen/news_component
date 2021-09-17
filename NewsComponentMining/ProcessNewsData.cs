using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using CS_Class;
using TokenLib;

namespace NewsComponentMining
{
    public partial class ProcessNewsData : Form
    {
        public ProcessNewsData()
        {
            InitializeComponent();
        }

        private void bBrowse_Click(object sender, EventArgs e)
        {
            if (fChooseNewsFolder.ShowDialog() == DialogResult.OK)
            {
                tbFolder.Text = fChooseNewsFolder.SelectedPath;

                lFiles.Items.Clear();


                foreach (string strfile in Directory.GetFiles(tbFolder.Text))
                {
                    if (strfile.ToLower().IndexOf("].txt") >= 0)
                    {
                        lFiles.Items.Add(strfile.Replace(tbFolder.Text, ""));
                    }
                }
            }

        }

        private void lFiles_SelectedIndexChanged(object sender, EventArgs e)
        {
            StreamReader sr;
            string[] arrComponentFile;
            string[] sep = { " ", "●", "←", "/", "(" };
            int i, j, k;
            int iMode;  //1: 人物 P, 2: 交通工具 T, 3: 物品 I, 4: 場景 S
            TextBox tb = tbP;

            foreach (string strfile in Directory.GetFiles(tbFolder.Text))
            {
                if (strfile.ToLower().IndexOf("].txt") < 0 && strfile.IndexOf(lFiles.SelectedItem.ToString().Substring(0, 10)) >= 0)
                {
                    //MessageBox.Show(strfile);
                    sr = new StreamReader(strfile, Encoding.UTF8);
                    tbContent.Text = sr.ReadToEnd();
                    sr.Close();

                    tbI.Text = "";
                    tbP.Text = "";
                    tbS.Text = "";
                    tbT.Text = "";
                    sr = new StreamReader(tbFolder.Text + lFiles.SelectedItem.ToString(), Encoding.UTF8);
                    arrComponentFile = sr.ReadToEnd().Replace("\r", "").Split('\n');

                    for (i = 0; i < arrComponentFile.Length; i++)
                    {
                        if (arrComponentFile[i].Trim() == "")
                            continue;


                        switch (arrComponentFile[i].Substring(0, 2))
                        {
                            case "人物":
                                iMode = 1;
                                tb = tbP;
                                break;

                            case "交通":
                                iMode = 2;
                                tb = tbT;
                                break;

                            case "場景":
                                iMode = 4;
                                tb = tbS;
                                break;

                            case "物件":
                                iMode = 3;
                                tb = tbI;
                                break;

                            case "--":
                            case "編號":
                                break;

                            default:
                                if (arrComponentFile[i].Substring(2).Trim() == "")
                                    continue;

                                try
                                {
                                    tb.Text = tb.Text + arrComponentFile[i].Substring(2).Trim().Split(sep, StringSplitOptions.RemoveEmptyEntries)[0].Trim() + "\r\n";
                                }
                                catch
                                {
                                    tb.Text = tb.Text + arrComponentFile[i].Substring(2).Trim() + "\r\n";
                                }
                                break;
                        }
                    }
                }
            }
        }


        private string sql;
        //private SQLConn cn = new SQLConn("10.91.21.40", "sa", "sa", "SE_TEST");
        private SQLConn cn = new SQLConn("localhost", "sa", "lovedddog", "SE");
        private TokenGen tg = new TokenGen();

        private void bSave_Click(object sender, EventArgs e)
        {
            DataTable dt1 = new DataTable();
            DataTable dt2 = new DataTable();

            char[] sep1 = {'\\'};
            char[] sep2 = { '\n' };
            char[] sep3 = { ';' };
            string[] arrPath = tbFolder.Text.Split(sep1, StringSplitOptions.RemoveEmptyEntries);
            string strDate = arrPath[arrPath.Length - 1];
            strDate = strDate.Substring(0, 4) + "/" + strDate.Substring(4, 2) + "/" + strDate.Substring(6);
            string strNewsID = lFiles.SelectedItem.ToString().Substring(2, lFiles.SelectedItem.ToString().IndexOf("].") - 2);

            sql = "Insert into NewsComponentMining_NewsContent (NewsDate, NewsID, NewsContent) values " +
                  "('" + strDate + "', '" + strNewsID + "', '" + tbContent.Text.Trim().Replace("'", "''") + "');";
            cn.Exec(sql);



            //News Components
            string strComponentID;
            
            int i, j;
            string[] arrComponent = null;

            for (j = 1; j <= 4; j++)
            {
                switch (j)
                {
                    case 1:
                        arrComponent = tbP.Text.Trim().Split(sep2, StringSplitOptions.RemoveEmptyEntries);
                        break;
                    case 2:
                        arrComponent = tbT.Text.Trim().Split(sep2, StringSplitOptions.RemoveEmptyEntries);
                        break;
                    case 3:
                        arrComponent = tbI.Text.Trim().Split(sep2, StringSplitOptions.RemoveEmptyEntries);
                        break;
                    case 4:
                        arrComponent = tbS.Text.Trim().Split(sep2, StringSplitOptions.RemoveEmptyEntries);
                        break;
                }


                for (i = 0; i < arrComponent.Length; i++)
                {
                    sql = "Select * from NewsComponentMining_Component Where Component = '" + arrComponent[i].Trim().ToUpper().Replace("'", "''") + "';";
                    cn.Exec(sql, ref dt1);

                    if (dt1.Rows.Count == 0)
                    {
                        sql = "Insert into NewsComponentMining_Component (Component) values ('" + arrComponent[i].Trim().ToUpper().Replace("'", "''") + "');";
                        cn.Exec(sql);

                        sql = "Select * from NewsComponentMining_Component Where Component = '" + arrComponent[i].Trim().ToUpper().Replace("'", "''") + "';";
                        cn.Exec(sql, ref dt1);
                    }

                    strComponentID = dt1.Rows[0]["ComponentID"].ToString();

                    sql = "Insert into NewsComponentMining_NewsComponentR (NewsDate, NewsID, ComponentID, ComponentType) values " +
                          "('" + strDate + "', '" + strNewsID + "', '" + strComponentID + "', " + j.ToString() + ");";
                    cn.Exec(sql);
                }
            }


            //News Term
            string tokens = tg.GenerateNoSave(tbContent.Text);

            string[] arrToken = tokens.Split(sep3, StringSplitOptions.RemoveEmptyEntries);
            string strTermID;

            for (i = 0; i < arrToken.Length; i++)
            {
                sql = "Select * from NewsComponentMining_Term Where Term = '" + arrToken[i].Trim().ToUpper().Replace("'", "''") + "';";
                cn.Exec(sql, ref dt1);

                if (dt1.Rows.Count == 0)
                {
                    sql = "Insert into NewsComponentMining_Term (Term) values ('" + arrToken[i].Trim().ToUpper().Replace("'", "''") + "');";
                    cn.Exec(sql);

                    sql = "Select * from NewsComponentMining_Term Where Term = '" + arrToken[i].Trim().ToUpper().Replace("'", "''") + "';";
                    cn.Exec(sql, ref dt1);
                }

                strTermID = dt1.Rows[0]["TermID"].ToString();

                sql = "Insert into NewsComponentMining_NewsTermR (NewsDate, NewsID, TermID, TermSeq) values " +
                      "('" + strDate + "', '" + strNewsID + "', '" + strTermID + "', " + (i+1).ToString() + ");";
                cn.Exec(sql);
            }


            MessageBox.Show("done");
        }
    }
}