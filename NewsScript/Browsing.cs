using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using CS_Class;
using WebPageFetchLibrary;

namespace NewsScript
{
    public partial class Browsing : Form
    {
        public Browsing()
        {
            InitializeComponent();
            funcUpdate_dgnews();
        }

        private int iSelectedRowIndex = -1;
        private string newsid = "";
        private string sql;
        private SQLConn cn = new SQLConn("10.91.21.40", "sa", "sa", "se");
        
        private void dgnews_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                try
                {
                    iSelectedRowIndex = e.RowIndex;
                    funcShowNewsData(Convert.ToInt32(dgnews.Rows[e.RowIndex].Cells["newsid"].Value));
                    newsid = Convert.ToString(dgnews.Rows[e.RowIndex].Cells["newsid"].Value);
                    tabControl1.SelectedIndex = 1;
                }
                catch
                {
                }
            }
            else
            {
                return;
            }
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedIndex == 0)
            {
                funcUpdate_dgnews();
            }
            //else if (tabControl1.SelectedIndex == 2)
            //{
            //    funcShowNewsData();
            //}
        }

        private void funcUpdate_dgnews()
        {
            DataTable dt = new DataTable();

            sql = "Select * from news_content order by newsid;";
            cn.Exec(sql, ref dt);

            dgnews.DataSource = dt;
            dgnews.Update();


            if (iSelectedRowIndex != -1)
            {
                //設定 datagrid scroll down 到 iSelectedNewsid
                dgnews.FirstDisplayedScrollingRowIndex = iSelectedRowIndex;
                dgnews.Refresh();
                dgnews.CurrentCell = dgnews.Rows[iSelectedRowIndex].Cells[0];
                dgnews.Rows[iSelectedRowIndex].Selected = true;
            }
        }

        //顯示新聞資料, 斷詞切字
        private void funcShowNewsData(int _newsid)
        {
            DataTable dt = new DataTable();
            XmlDocument xd = new XmlDocument();
            XmlNodeList xnl;
            int i, j, k;

            lNewsid.Text = _newsid.ToString();

            sql = "select * from news_content where newsid = '" + _newsid.ToString() + "';";
            cn.Exec(sql, ref dt);

            YahooSegApi ysa = new YahooSegApi("556JcanV34EOFemk_DMG1pkoMhR_XHsChsC639PgPRXZMUrpor4LlCQJOlWIB0m_hQ--");
            //YahooSegApi ysa = new YahooSegApi("b2H9zvzV34HiVnyiQaNoF_XwUat82S9d70b5WhK4qnJmSk.bOPr.GY6CEFct6hwOsg--");

            xd.LoadXml(ysa.WordSegmentation(dt.Rows[0]["news_content"].ToString().Replace("\n", "，")));
            tbNews.Text = dt.Rows[0]["news_content"].ToString().Replace("\n", "\r\n");

            xnl = xd.ChildNodes[1].ChildNodes;

            tbPOS.Text = "";
            tbPOSKeyword.Text = "";
            tbUserAdded.Text = "";

            for (i = 0; i < xnl.Count; i++)
            {
                if (xnl[i].ChildNodes.Count == 5)
                {
                    if (xnl[i].ChildNodes[0].InnerText != "，")
                    {
                        //tbPOS.Text = tbPOS.Text + xnl[i].ChildNodes[0].InnerText;
                        if ((xnl[i].ChildNodes[4].InnerText.IndexOf("VERB") >= 0 && xnl[i].ChildNodes[4].InnerText.IndexOf("ADV") < 0) ||
                             xnl[i].ChildNodes[4].InnerText.IndexOf("ADJ") >= 0 ||
                            (xnl[i].ChildNodes[4].InnerText.IndexOf("NOUN") >= 0 && //xnl[i].ChildNodes[4].InnerText.IndexOf("PROPER_NOUN") < 0 &&
                             xnl[i].ChildNodes[4].InnerText.IndexOf("NOUN_TIME") < 0 && xnl[i].ChildNodes[4].InnerText.IndexOf("PRONOUN") < 0)
                            )
                        {
                            tbPOS.Text = tbPOS.Text + xnl[i].ChildNodes[0].InnerText + "\r\n";
                        }
                    }
                }
            }
            tbPOS.Text = tbPOS.Text.Trim();

            sql = "Select * from news_keywordr a inner join news_keyword b on a.newsid = '" + _newsid + "' and a.news_keyword_id = b.news_keyword_id " +
                  "and a.type = 1 order by a.news_keyword_id;";
            cn.Exec(sql, ref dt);

            for (i = 0; i < dt.Rows.Count; i++)
            {
                tbPOSKeyword.Text = tbPOSKeyword.Text + dt.Rows[i]["keyword"].ToString() + "\t(" + dt.Rows[i]["pos"].ToString() + ")(" + dt.Rows[i]["ntype"].ToString() + ")\r\n";
            }

            sql = "Select * from news_keywordr a inner join news_keyword b on a.newsid = '" + _newsid + "' and a.news_keyword_id = b.news_keyword_id " +
                  "and a.type = 3 order by a.news_keyword_id;";
            cn.Exec(sql, ref dt);

            for (i = 0; i < dt.Rows.Count; i++)
            {
                tbUserAdded.Text = tbUserAdded.Text + dt.Rows[i]["keyword"].ToString() + "(" + dt.Rows[i]["ntype"].ToString() + ")\r\n";
            }
        }

        private void dgnews_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                if (MessageBox.Show("確認刪除", "是否真的要刪除？", MessageBoxButtons.OKCancel) == DialogResult.OK)
                {
                    sql = "Delete from news_content where newsid = '" + newsid + "';";
                    cn.Exec(sql);
                    dgnews.Rows.RemoveAt(iSelectedRowIndex);
                }
            }
        }

        private void dgnews_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            newsid = dgnews.Rows[e.RowIndex].Cells[0].Value.ToString();
            iSelectedRowIndex = e.RowIndex;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string[] arrSep = { "\r\n" };
            string[] arrTerm = tbPOSKeyword.Text.Trim().Split(arrSep, StringSplitOptions.RemoveEmptyEntries);
            string[] tmp;
            int i, j, k;
            DataTable dt1 = new DataTable();
            string keywordid;

            //先刪掉舊有的關鍵詞
            sql = "delete from news_keywordr where newsid = '" + newsid + "';";
            cn.Exec(sql);

            //斷詞切字後的關鍵詞
            for (i = 0; i < arrTerm.Length; i++)
            {
                tmp = arrTerm[i].Trim().Split('(');

                tmp[0] = tmp[0].Replace("'", "").Trim();

                if (tmp[0] == "")
                    continue;

                tmp[1] = tmp[1].Trim();
                tmp[1] = tmp[1].Substring(0, tmp[1].Length - 1);

                tmp[2] = tmp[2].Trim();
                tmp[2] = tmp[2].Substring(0, tmp[2].Length - 1).ToUpper();

                sql = "Select * from news_keyword where keyword = '" + tmp[0] + "';";
                cn.Exec(sql, ref dt1);

                if (dt1.Rows.Count == 0)
                {
                    sql = "Insert into news_keyword (keyword) values ('" + tmp[0] + "');";
                    cn.Exec(sql);

                    sql = "Select * from news_keyword where keyword = '" + tmp[0] + "';";
                    cn.Exec(sql, ref dt1);
                }
                keywordid = dt1.Rows[0]["news_keyword_id"].ToString();

                sql = "Select * from news_keywordr where newsid = '" + newsid + "' and news_keyword_id = '" + keywordid + "' and type = 1;";
                cn.Exec(sql, ref dt1);

                if (dt1.Rows.Count == 0)
                {
                    sql = "Insert into news_keywordr (NewsId, news_keyword_id, type, pos, ntype) values ('" + newsid + "', '" + keywordid + "', 1, '" + tmp[1] + "', '" + tmp[2] + "');";
                    cn.Exec(sql);
                }
                else
                {
                    sql = "update news_keywordr set count = count + 1 where newsid = '" + newsid + "' and news_keyword_id = '" + keywordid + "' and type = 1;";
                    cn.Exec(sql);
                }
            }


            //使用者自訂的關鍵詞
            arrTerm = tbUserAdded.Text.Trim().Split(arrSep, StringSplitOptions.RemoveEmptyEntries);
            for (i = 0; i < arrTerm.Length; i++)
            {
                arrTerm[i] = arrTerm[i].Trim();
                if (arrTerm[i] == "")
                    continue;

                tmp = arrTerm[i].Split('(');

                tmp[0] = tmp[0].Trim();

                sql = "Select * from news_keyword where keyword = '" + tmp[0] + "';";
                cn.Exec(sql, ref dt1);

                if (dt1.Rows.Count == 0)
                {
                    sql = "Insert into news_keyword (keyword) values ('" + tmp[0] + "');";
                    cn.Exec(sql);

                    sql = "Select * from news_keyword where keyword = '" + tmp[0] + "';";
                    cn.Exec(sql, ref dt1);
                }
                keywordid = dt1.Rows[0]["news_keyword_id"].ToString();

                sql = "Select * from news_keywordr where newsid = '" + newsid + "' and news_keyword_id = '" + keywordid + "' and type = 3;";
                cn.Exec(sql, ref dt1);

                if (dt1.Rows.Count == 0)
                {
                    if (tmp.Length == 1)
                    {
                        sql = "Insert into news_keywordr (NewsId, news_keyword_id, type) values ('" + newsid + "', '" + keywordid + "', 3);";
                    }
                    else
                    {
                        tmp[1] = tmp[1].Trim();
                        sql = "Insert into news_keywordr (NewsId, news_keyword_id, type, ntype) values ('" + newsid + "', '" + keywordid + "', 3, '" + tmp[1].Substring(0, tmp[1].Length - 1).ToUpper() + "');";
                    }
                    cn.Exec(sql);
                }
                else
                {
                    sql = "update news_keywordr set count = count + 1 where newsid = '" + newsid + "' and news_keyword_id = '" + keywordid + "' and type = 3;";
                    cn.Exec(sql);
                }
            }

            MessageBox.Show("完成");
        }
    }
}