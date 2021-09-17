using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Collections;
using System.Threading;
using CS_Class;
//using NewsComponentGeneratorLib;
using WebPageFetchLibrary;
using Gapi.Search;
using NewsComponentGenerator.NCG_WebRef;
using NewsComponentGenerator.localhost;

namespace NewsComponentGenerator
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private string sql;
        //private SQLConn cn = new SQLConn("10.91.21.40", "sa", "sa", "se");
        private SQLConn cn = new SQLConn("140.113.124.12", "sa", "lovedddog", "se");

        private YahooSegApi ysa = new YahooSegApi("b2H9zvzV34HiVnyiQaNoF_XwUat82S9d70b5WhK4qnJmSk.bOPr.GY6CEFct6hwOsg--");


        private void bAnalyse_Click(object sender, EventArgs e)
        {
            //URLProcessing up = new URLProcessing();
            //string strpage;

            DataTable dt = new DataTable();
            XmlDocument xd = new XmlDocument();
            XmlNodeList xnl;
            int i, j, k, m, n;
            //int iP, iS, iI, iT;
            double dP, dS, dI, dT;

            Queue qIdentifiedTerm = new Queue();

            xd.LoadXml(ysa.WordSegmentation(tbNews.Text.Replace("\n", "，").Replace("，，", "，")));
            
            xnl = xd.ChildNodes[1].ChildNodes;

            tbP.Text = "";
            tbS.Text = "";
            tbI.Text = "";
            tbT.Text = "";

            #region 取得各分類的平均分數
            sql = "Select * from news_keyword_class where Class_name = 'P';";
            cn.Exec(sql, ref dt);
            if (dt.Rows.Count > 0)
            {
                dP = Convert.ToDouble(dt.Rows[0]["FinalScore"]);
            }
            else
            {
                dP = 0;
            }

            sql = "Select * from news_keyword_class where Class_name = 'S';";
            cn.Exec(sql, ref dt);
            if (dt.Rows.Count > 0)
            {
                dS = Convert.ToDouble(dt.Rows[0]["FinalScore"]);
            }
            else
            {
                dS = 0;
            }

            sql = "Select * from news_keyword_class where Class_name = 'I';";
            cn.Exec(sql, ref dt);
            if (dt.Rows.Count > 0)
            {
                dI = Convert.ToDouble(dt.Rows[0]["FinalScore"]);
            }
            else
            {
                dI = 0;
            }

            sql = "Select * from news_keyword_class where Class_name = 'T';";
            cn.Exec(sql, ref dt);
            if (dt.Rows.Count > 0)
            {
                dT = Convert.ToDouble(dt.Rows[0]["FinalScore"]);
            }
            else
            {
                dT = 0;
            }
            #endregion


            SearchResults searchResults;
            float fGoogleCountOfKeyword;
            float fMIscore, fMIscoreSum;

            //Reference terms of Classes
            string[] arrP = { "人物", "角色", "官員", "人員" };
            string[] arrS = { "單位", "組織", "部門", "建築", "場景", "位置" };
            string[] arrI = { "物品", "用品", "器官", "家具" };
            string[] arrT = { "交通工具", "運輸", "車輛" };

            //Google Count of reference terms of classes
            long[] iP = new long[4];
            long[] iS = new long[6];
            long[] iI = new long[4];
            long[] iT = new long[3];

            string[] arrtmp = null;
            long[] itmp = null;

            #region 重新取得各個 reference terms 的 google count
            for (k = 0; k <= 3; k++)
            {
                switch (k)
                {
                    case 0:
                        arrtmp = arrP;
                        itmp = iP;
                        break;
                    case 1:
                        arrtmp = arrI;
                        itmp = iI;
                        break;
                    case 2:
                        arrtmp = arrT;
                        itmp = iT;
                        break;
                    case 3:
                        arrtmp = arrS;
                        itmp = iS;
                        break;
                }

                if (arrtmp == null)
                    continue;

                for (j = 0; j < arrtmp.Length; j++)
                {
                    searchResults = Searcher.Search(SearchType.Web, "\"" + arrtmp[j] + "\"");
                    itmp[j] = searchResults.EstimatedResultCount;
                }
            }
            #endregion

            for (i = 0; i < xnl.Count; i++)
            {
                if (xnl[i].ChildNodes.Count == 5)
                {
                    if (xnl[i].ChildNodes[0].InnerText.Trim() != "，" && xnl[i].ChildNodes[0].InnerText.Trim() != "" &&
                        tbI.Text.IndexOf(xnl[i].ChildNodes[0].InnerText.Trim()) < 0 && tbS.Text.IndexOf(xnl[i].ChildNodes[0].InnerText.Trim()) < 0 && 
                        tbP.Text.IndexOf(xnl[i].ChildNodes[0].InnerText.Trim()) < 0 && tbT.Text.IndexOf(xnl[i].ChildNodes[0].InnerText.Trim()) < 0 &&

                        qIdentifiedTerm.Contains(xnl[i].ChildNodes[0].InnerText.Trim()) == false
                        )
                    {
                        // xnl[i].ChildNodes[0].InnerText : term
                        sql = "select top 1 ntype, FinalScore from news_keyword a inner join news_keyword_class b on a.news_keyword_id = b.news_keyword_id and " +
                              "a.keyword = '" + xnl[i].ChildNodes[0].InnerText + "' order by UserSubtractScore desc, FinalScore desc;";
                        cn.Exec(sql, ref dt);


                        qIdentifiedTerm.Enqueue(xnl[i].ChildNodes[0].InnerText.Trim());


                        if (dt.Rows.Count >= 1)
                        {
                            if (dt.Rows[0]["ntype"].ToString().ToUpper() == "P" && Convert.ToDouble(dt.Rows[0]["FinalScore"]) >= dP)
                            {
                                tbP.Text = tbP.Text + xnl[i].ChildNodes[0].InnerText.Trim() + "\t\r\n";
                            }
                            else if (dt.Rows[0]["ntype"].ToString().ToUpper() == "T" && Convert.ToDouble(dt.Rows[0]["FinalScore"]) >= dT)
                            {
                                tbT.Text = tbT.Text + xnl[i].ChildNodes[0].InnerText.Trim() + "\t\r\n";
                            }
                            else if (dt.Rows[0]["ntype"].ToString().ToUpper() == "S" && Convert.ToDouble(dt.Rows[0]["FinalScore"]) >= dS)
                            {
                                tbS.Text = tbS.Text + xnl[i].ChildNodes[0].InnerText.Trim() + "\t\r\n";
                            }
                            else if (dt.Rows[0]["ntype"].ToString().ToUpper() == "I" && Convert.ToDouble(dt.Rows[0]["FinalScore"]) >= dI)
                            {
                                tbI.Text = tbI.Text + xnl[i].ChildNodes[0].InnerText.Trim() + "\t\r\n";
                            }

                            continue;
                        }


                        if (xnl[i].ChildNodes[4].InnerText.IndexOf("NOUN") >= 0 && xnl[i].ChildNodes[0].InnerText.Trim().IndexOf("，") < 0 && cbAdvSearch.Checked == true)
                        {
                            //沒有在資料庫中出現過，那麼就另外判斷

                            for (k = 0; k <= 3; k++)
                            {
                                switch (k)
                                {
                                    case 0:
                                        arrtmp = arrP;
                                        itmp = iP;
                                        break;
                                    case 1:
                                        arrtmp = arrI;
                                        itmp = iI;
                                        break;
                                    case 2:
                                        arrtmp = arrT;
                                        itmp = iT;
                                        break;
                                    case 3:
                                        arrtmp = arrS;
                                        itmp = iS;
                                        break;
                                }

                                if (arrtmp == null)
                                    continue;

                                #region 計算 MIscore
                                fMIscoreSum = 0;
                                for (j = 0; j < arrtmp.Length; j++)
                                {
                                    try
                                    {
                                        fGoogleCountOfKeyword = (float)((Searcher.Search(SearchType.Web, "\"" + xnl[i].ChildNodes[0].InnerText.Trim() + "\"")).EstimatedResultCount);
                                        if (fGoogleCountOfKeyword == 0)
                                        {
                                            fMIscore = 0;
                                        }
                                        else
                                        {
                                            fMIscore = (float)((Searcher.Search(SearchType.Web, "\"" + arrtmp[j] + "\" \"" + xnl[i].ChildNodes[0].InnerText.Trim() + "\"")).EstimatedResultCount) / (fGoogleCountOfKeyword * itmp[j]);
                                        }
                                    }
                                    catch (DivideByZeroException ex)
                                    {
                                        fMIscore = 0;
                                    }
                                    fMIscoreSum = fMIscoreSum + fMIscore;
                                }
                                #endregion

                                #region 檢查有沒有達到分類平均分數, 如果有的話加到分類裡頭去
                                bool bIdentified = false;
                                switch (k)
                                {
                                    case 0:
                                        if (dP <= ((double)fMIscoreSum) / ((double)arrtmp.Length))
                                        {
                                            tbP.Text = tbP.Text + xnl[i].ChildNodes[0].InnerText.Trim() + "\t\r\n";

                                            bIdentified = true;
                                        }
                                        break;
                                    case 1:
                                        if (dI <= ((double)fMIscoreSum) / ((double)arrtmp.Length))
                                        {
                                            tbI.Text = tbI.Text + xnl[i].ChildNodes[0].InnerText.Trim() + "\t\r\n";

                                            bIdentified = true;
                                        }
                                        break;
                                    case 2:
                                        if (dT <= ((double)fMIscoreSum) / ((double)arrtmp.Length))
                                        {
                                            tbT.Text = tbT.Text + xnl[i].ChildNodes[0].InnerText.Trim() + "\t\r\n";

                                            bIdentified = true;
                                        }
                                        break;
                                    case 3:
                                        if (dS <= ((double)fMIscoreSum) / ((double)arrtmp.Length))
                                        {
                                            tbS.Text = tbS.Text + xnl[i].ChildNodes[0].InnerText.Trim() + "\t\r\n";

                                            bIdentified = true;
                                        }
                                        break;
                                }
                                #endregion

                                if (bIdentified == true)
                                    break;
                            }

                            
                        }


                        //    //物品
                        //    strpage = up.FetchURL("http://www.google.com/search?as_q=\"" + xnl[i].ChildNodes[0].InnerText.Trim() + "\"+\"物品\"");
                        //    j = strpage.IndexOf("<p id=resultStats>");
                        //    j = strpage.IndexOf("of about", j);
                        //    j = strpage.IndexOf("<b>", j);
                        //    k = strpage.IndexOf("</b>", j);

                        //    try
                        //    {
                        //        iI = Convert.ToInt32(strpage.Substring(j + 3, k - j - 3).Replace(",", ""));
                        //    }
                        //    catch
                        //    {
                        //        j = strpage.IndexOf("of about", k);
                        //        j = strpage.IndexOf("<b>", j);
                        //        k = strpage.IndexOf("</b>", j);

                        //        iI = Convert.ToInt32(strpage.Substring(j + 3, k - j - 3).Replace(",", ""));
                        //    }


                        //    //人物
                        //    strpage = up.FetchURL("http://www.google.com/search?as_q=\"" + xnl[i].ChildNodes[0].InnerText.Trim() + "\"+\"人物\"");
                        //    j = strpage.IndexOf("<p id=resultStats>");
                        //    j = strpage.IndexOf("of about", j);
                        //    j = strpage.IndexOf("<b>", j);
                        //    k = strpage.IndexOf("</b>", j);

                        //    try
                        //    {
                        //        iP = Convert.ToInt32(strpage.Substring(j + 3, k - j - 3).Replace(",", ""));
                        //    }
                        //    catch
                        //    {
                        //        j = strpage.IndexOf("of about", k);
                        //        j = strpage.IndexOf("<b>", j);
                        //        k = strpage.IndexOf("</b>", j);

                        //        iP = Convert.ToInt32(strpage.Substring(j + 3, k - j - 3).Replace(",", ""));
                        //    }

                        //    //交通工具
                        //    strpage = up.FetchURL("http://www.google.com/search?as_q=\"" + xnl[i].ChildNodes[0].InnerText.Trim() + "\"+\"交通\"");
                        //    j = strpage.IndexOf("<p id=resultStats>");
                        //    j = strpage.IndexOf("of about", j);
                        //    j = strpage.IndexOf("<b>", j);
                        //    k = strpage.IndexOf("</b>", j);

                        //    try
                        //    {
                        //        iT = Convert.ToInt32(strpage.Substring(j + 3, k - j - 3).Replace(",", ""));
                        //    }
                        //    catch
                        //    {
                        //        j = strpage.IndexOf("of about", k);
                        //        j = strpage.IndexOf("<b>", j);
                        //        k = strpage.IndexOf("</b>", j);

                        //        iT = Convert.ToInt32(strpage.Substring(j + 3, k - j - 3).Replace(",", ""));
                        //    }

                        //    //場景
                        //    strpage = up.FetchURL("http://www.google.com/search?as_q=\"" + xnl[i].ChildNodes[0].InnerText.Trim() + "\"+\"場所\"");
                        //    j = strpage.IndexOf("<p id=resultStats>");
                        //    j = strpage.IndexOf("of about", j);
                        //    j = strpage.IndexOf("<b>", j);
                        //    k = strpage.IndexOf("</b>", j);

                        //    try
                        //    {
                        //        iS = Convert.ToInt32(strpage.Substring(j + 3, k - j - 3).Replace(",", ""));
                        //    }
                        //    catch
                        //    {
                        //        j = strpage.IndexOf("of about", k);
                        //        j = strpage.IndexOf("<b>", j);
                        //        k = strpage.IndexOf("</b>", j);

                        //        iS = Convert.ToInt32(strpage.Substring(j + 3, k - j - 3).Replace(",", ""));
                        //    }


                        //    if (iP == Math.Max(iP, Math.Max(iS, Math.Max(iT, iI))))
                        //    {
                        //        tbP.Text = tbP.Text + xnl[i].ChildNodes[0].InnerText.Trim() + "\t\r\n";
                        //    }
                        //    else if (iS == Math.Max(iP, Math.Max(iS, Math.Max(iT, iI))))
                        //    {
                        //        tbS.Text = tbS.Text + xnl[i].ChildNodes[0].InnerText.Trim() + "\t\r\n";
                        //    }
                        //    else if (iT == Math.Max(iP, Math.Max(iS, Math.Max(iT, iI))))
                        //    {
                        //        tbT.Text = tbT.Text + xnl[i].ChildNodes[0].InnerText.Trim() + "\t\r\n";
                        //    }
                        //    else if (iI == Math.Max(iP, Math.Max(iS, Math.Max(iT, iI))))
                        //    {
                        //        tbI.Text = tbI.Text + xnl[i].ChildNodes[0].InnerText.Trim() + "\t\r\n";
                        //    }
                        //}




                    }
                }
            }
        }

        private bool xPressed;

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.ControlKey)
            {
                xPressed = true;
            }
            else
            {
                xPressed = false;
            }
        }



        private void label1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (xPressed == true && e.Button == MouseButtons.Left)
            {
                if (DialogResult.OK == MessageBox.Show("重新更新字詞與類別相關度索引？", "重新更新字詞與類別相關度索引？", MessageBoxButtons.OKCancel))
                {
                    rebuildRelationIndex();

                    //rebuildUserKeywordAssociation();
                }
            }
        }

        private void rebuildRelationIndex()
        {
            DataTable dt1 = new DataTable();
            DataTable dt2 = new DataTable();
            int i, j, k, m, n;

            //Reference terms of Classes
            string[] arrP = {"人物", "角色", "官員", "人員"};
            string[] arrS = {"單位", "組織", "部門", "建築", "場景", "位置"};
            string[] arrI = {"物品", "用品", "器官", "家具"};
            string[] arrT = { "交通工具", "運輸", "車輛" };

            //Google Count of reference terms of classes
            long[] iP = new long[4];
            long[] iS = new long[6];
            long[] iI = new long[4];
            long[] iT = new long[3];

            //Mutual Informaiton Score
            float fMIscore;

            string[] arrtmp = null;
            long[] itmp = null;
            string strurltmp;
            SearchResults searchResults;
            float fGoogleCountOfKeyword;
            

            URLProcessing up = new URLProcessing();


            #region 重新取得各個 reference terms 的 google count
            for (k = 0; k <= 3; k++)
            {
                switch (k)
                {
                    case 0:
                        arrtmp = arrP;
                        itmp = iP;
                        break;
                    case 1:
                        arrtmp = arrI;
                        itmp = iI;
                        break;
                    case 2:
                        arrtmp = arrT;
                        itmp = iT;
                        break;
                    case 3:
                        arrtmp = arrS;
                        itmp = iS;
                        break;
                }

                if (arrtmp == null)
                    continue;

                for (j = 0; j < arrtmp.Length; j++)
                {
                    searchResults = Searcher.Search(SearchType.Web, "\"" + arrtmp[j] + "\"");
                    itmp[j] = searchResults.EstimatedResultCount;
                }
            }
            #endregion

            sql = "select b.ntype, a.keyword, a.news_keyword_id, sum(b.count) as sumcount from news_keyword a inner join news_keywordr b on " + 
                  "a.news_keyword_id = b.news_keyword_id and isnull(b.ntype, '') <> '' " +
                  "group by b.ntype, a.keyword, a.news_keyword_id order by b.ntype, a.news_keyword_id;";
            cn.Exec(sql, ref dt1);

            #region 處理計算各個 term 與 reference terms 的 MI score
            for (i = 0; i < dt1.Rows.Count; i++)
            {
                for (k = 0; k <= 3; k++)
                {
                    switch (k)
                    {
                        case 0:
                            arrtmp = arrP;
                            itmp = iP;
                            break;
                        case 1:
                            arrtmp = arrI;
                            itmp = iI;
                            break;
                        case 2:
                            arrtmp = arrT;
                            itmp = iT;
                            break;
                        case 3:
                            arrtmp = arrS;
                            itmp = iS;
                            break;
                    }

                    if (arrtmp == null)
                        continue;

                    for (j = 0; j < arrtmp.Length; j++)
                    {
                        sql = "Select * from News_keyword_class where Class_name = '" + arrtmp[j] + "' and news_keyword_id = '" + dt1.Rows[i]["news_keyword_id"].ToString() + "' " +
                              "and ntype = '" + dt1.Rows[i]["ntype"].ToString() + "';";
                        cn.Exec(sql, ref dt2);

                        bool bRecheck = false;
                        if (dt2.Rows.Count <= 0)
                        {
                            bRecheck = true;
                        }
                        else if (dt2.Rows.Count > 0)
                        {
                            TimeSpan ts = DateTime.Now.Subtract(Convert.ToDateTime(dt2.Rows[0]["lastUpdateTime"]));
                            if (ts.TotalDays > 30)
                            {
                                bRecheck = true;
                            }
                        }

                        if (bRecheck == true)
                        {
                            try
                            {
                                fGoogleCountOfKeyword = (float)((Searcher.Search(SearchType.Web, "\"" + dt1.Rows[i]["keyword"].ToString() + "\"")).EstimatedResultCount);
                                if (fGoogleCountOfKeyword == 0)
                                {
                                    fMIscore = 0;
                                }
                                else
                                {
                                    fMIscore = (float)((Searcher.Search(SearchType.Web, "\"" + arrtmp[j] + "\" \"" + dt1.Rows[i]["keyword"].ToString() + "\"")).EstimatedResultCount) / (fGoogleCountOfKeyword * itmp[j]);
                                }
                            }
                            catch (DivideByZeroException ex)
                            {
                                fMIscore = 0;
                            }


                            if (dt2.Rows.Count <= 0)
                            {
                                sql = "Insert into News_Keyword_Class (Class_name, news_keyword_id, ntype, count, google_score) values " +
                                      "('" + arrtmp[j] + "', '" + dt1.Rows[i]["news_keyword_id"].ToString() + "', '" + dt1.Rows[i]["ntype"].ToString() + "', " +
                                      "'" + dt1.Rows[i]["sumcount"].ToString() + "', '" + fMIscore.ToString() + "');";
                            }
                            else
                            {
                                sql = "Update News_Keyword_Class Set google_score = '" + fMIscore.ToString() + "', lastUpdateTime = getdate(), " +
                                      "count = '" + dt1.Rows[i]["sumcount"].ToString() + "' " +
                                      "Where Class_name = '" + arrtmp[j] + "' AND news_keyword_id = '" + dt1.Rows[i]["news_keyword_id"].ToString() + "' " +
                                      "AND ntype = '" + dt1.Rows[i]["ntype"].ToString() + "';";
                            }

                            cn.Exec(sql);
                        }
                    }
                }
            }
            #endregion


            #region 計算 final score
            sql = "Update News_keyword_class set FinalScore = google_score - UserSubtractScore;";
            cn.Exec(sql);
            sql = "Update News_keyword_class set FinalScore = 0 where FinalScore < 0;";
            cn.Exec(sql);
            #endregion


            #region 計算各類別平均分數
            sql = "Delete from News_keyword_class where news_keyword_id = -1 and ntype = '-1';";
            cn.Exec(sql);


            sql = "Select avg(finalscore) as avg_score from News_keyword_class where Class_name in (";
            for (i = 0; i < arrP.Length; i++)
            {
                sql = sql + "'" + arrP[i] + "',";
            }
            sql = sql.Substring(0, sql.Length - 1);
            sql = sql + ");";
            cn.Exec(sql, ref dt1);
            sql = "Insert into News_Keyword_class (Class_name, news_keyword_id, ntype, FinalScore) values ('P', -1, '-1', '" + dt1.Rows[0]["avg_score"].ToString() + "');";
            cn.Exec(sql);


            sql = "Select avg(finalscore) as avg_score from News_keyword_class where Class_name in (";
            for (i = 0; i < arrI.Length; i++)
            {
                sql = sql + "'" + arrI[i] + "',";
            }
            sql = sql.Substring(0, sql.Length - 1);
            sql = sql + ");";
            cn.Exec(sql, ref dt1);
            sql = "Insert into News_Keyword_class (Class_name, news_keyword_id, ntype, FinalScore) values ('I', -1, '-1', '" + dt1.Rows[0]["avg_score"].ToString() + "');";
            cn.Exec(sql);


            sql = "Select avg(finalscore) as avg_score from News_keyword_class where Class_name in (";
            for (i = 0; i < arrT.Length; i++)
            {
                sql = sql + "'" + arrT[i] + "',";
            }
            sql = sql.Substring(0, sql.Length - 1);
            sql = sql + ");";
            cn.Exec(sql, ref dt1);
            sql = "Insert into News_Keyword_class (Class_name, news_keyword_id, ntype, FinalScore) values ('T', -1, '-1', '" + dt1.Rows[0]["avg_score"].ToString() + "');";
            cn.Exec(sql);


            sql = "Select avg(finalscore) as avg_score from News_keyword_class where Class_name in (";
            for (i = 0; i < arrS.Length; i++)
            {
                sql = sql + "'" + arrS[i] + "',";
            }
            sql = sql.Substring(0, sql.Length - 1);
            sql = sql + ");";
            cn.Exec(sql, ref dt1);
            sql = "Insert into News_Keyword_class (Class_name, news_keyword_id, ntype, FinalScore) values ('S', -1, '-1', '" + dt1.Rows[0]["avg_score"].ToString() + "');";
            cn.Exec(sql);
            #endregion


            MessageBox.Show("done");
        }

        private void rebuildUserKeywordAssociation()
        {
            DataTable dt = new DataTable();
            DataTable dt2 = new DataTable();
            DataTable dt3 = new DataTable();
            //sql = "Delete from News_UserKeywordAssociation;";
            //cn.Exec(sql);

            sql = "Select distinct news_keyword_id from news_keywordr where type = 3;";
            cn.Exec(sql, ref dt);

            int i, j, k;
            string userkeywordid, newskeywordid, newsid;

            for (i = 0; i < dt.Rows.Count; i++)
            {
                userkeywordid = dt.Rows[i]["news_keyword_id"].ToString();
                //newsid = dt.Rows[i]["newsid"].ToString();

                sql = "Select news_keyword_id, sum(count) as sumcount from news_keywordr " +
                      "where newsid in (select newsid from news_keywordr where news_keyword_id = '" + userkeywordid + "') " +
                      "AND type = 1 group by news_keyword_id;";
                cn.Exec(sql, ref dt2);

                for (j = 0; j < dt2.Rows.Count; j++)
                {
                    sql = "Select * from News_UserKeywordAssociation where news_UserKeyword_id = '" + userkeywordid + "' and " +
                          "news_keyword_id = '" + dt2.Rows[j]["news_keyword_id"].ToString() + "';";
                    cn.Exec(sql, ref dt3);

                    if (dt3.Rows.Count > 0)
                    {
                        sql = "Update News_UserKeywordAssociation Set count = '" + dt2.Rows[j]["sumcount"].ToString() + "' " +
                              "where news_UserKeyword_id = '" + userkeywordid + "' and news_keyword_id = '" + dt2.Rows[j]["news_keyword_id"].ToString() + "';";
                    }
                    else
                    {
                        sql = "Insert into News_UserKeywordAssociation (news_UserKeyword_id, news_keyword_id, count) values " +
                              "('" + userkeywordid + "', '" + dt2.Rows[j]["news_keyword_id"].ToString() + "', '" + dt2.Rows[j]["sumcount"].ToString() + "');";
                    }
                    cn.Exec(sql);
                }
            }

            sql = "Update News_UserKeywordAssociation set final_count = count - subtract_count;";
            cn.Exec(sql);

            sql = "Update News_UserKeywordAssociation set final_count = 0 where final_count < 0;";
            cn.Exec(sql);


            MessageBox.Show("done");
        }


        private Thread tmain, tp, ts, ti, tt;
        //private ClassTermGenerator ctg = new ClassTermGenerator("10.91.21.40", "sa", "sa", "se");

        private string op, os, oi, ot;
        //private ClassTermGenerator ctg = new ClassTermGenerator("140.113.124.12", "sa", "lovedddog", "se");

        NCG_WebRef.NewsComponentGenerator ncg = new NCG_WebRef.NewsComponentGenerator();
        //localhost.NewsComponentGenerator ncg = new localhost.NewsComponentGenerator();


        private void bAnalyse_2_Click(object sender, EventArgs e)
        {
            if (tbNewsNo.Text.Trim() == "")
            {
                MessageBox.Show("請填入新聞編號");
                return;
            }

            tbP.Text = "";
            tbT.Text = "";
            tbS.Text = "";
            tbI.Text = "";

            ncg.Timeout = 600000;

            //ctg.reset();

            //tmain = new Thread(new ThreadStart(funcThreadMain));
            //tmain.Start();

            //tp = new Thread(new ThreadStart(funcThreadPerson));
            //tp.Start();

            //ti = new Thread(new ThreadStart(funcThreadItem));
            //ti.Start();

            //ts = new Thread(new ThreadStart(funcThreadScene));
            //ts.Start();

            //tt = new Thread(new ThreadStart(funcThreadTrans));
            //tt.Start();

            //while (ctg.isFinished == false || ctg.qItem.Count > 0 || ctg.qPerson.Count > 0 || ctg.qScene.Count > 0 || ctg.qTrans.Count > 0)
            //{
            //    Thread.Sleep(1000);
            //}
            
            //funcThreadMain(); funcThreadPerson(); funcThreadItem(); funcThreadTrans(); funcThreadScene();
            
            string tmp = ncg.GenerateClassTerm(tbNews.Text.Trim(), cbAdvSearch.Checked, DateTime.Now.ToShortDateString(), tbNewsNo.Text, Environment.UserName);
            string[] atmp = tmp.Split('#');
            tbP.Text = atmp[0].Replace(";", "\r\n").Trim();
            tbI.Text = atmp[1].Replace(";", "\r\n").Trim();
            tbT.Text = atmp[2].Replace(";", "\r\n").Trim();
            tbS.Text = atmp[3].Replace(";", "\r\n").Trim();
            op = tbP.Text;
            os = tbS.Text;
            oi = tbI.Text;
            ot = tbT.Text;
            MessageBox.Show("done");
        }

        //private void funcThreadMain()
        //{
        //    ctg.GenerateClassTerm(tbNews.Text.Trim(), cbAdvSearch.Checked, DateTime.Now.ToShortDateString(), "test", "phanix");
        //    //Thread.CurrentThread.Abort();
            
        //}

        //private void funcThreadPerson()
        //{
        //    while (ctg.isFinished == false || ctg.qPerson.Count > 0)
        //    {
        //        while (ctg.qPerson.Count > 0)
        //        {
        //            tbP.Text = tbP.Text + ctg.qPerson.Dequeue() + "\r\n";
        //        }

        //        //Thread.Sleep(2000);
        //    }

        //    //Thread.CurrentThread.Abort();
        //}

        //private void funcThreadItem()
        //{
        //    while (ctg.isFinished == false || ctg.qItem.Count > 0)
        //    {
        //        while (ctg.qItem.Count > 0)
        //        {
        //            tbI.Text = tbI.Text + ctg.qItem.Dequeue() + "\r\n";
        //        }

        //        //Thread.Sleep(2000);
        //    }

        //    //Thread.CurrentThread.Abort();
        //}

        //private void funcThreadTrans()
        //{
        //    while (ctg.isFinished == false || ctg.qTrans.Count > 0)
        //    {
        //        while (ctg.qTrans.Count > 0)
        //        {
        //            tbT.Text = tbT.Text + ctg.qTrans.Dequeue() + "\r\n";
        //        }

        //        //Thread.Sleep(2000);
        //    }

        //    //Thread.CurrentThread.Abort();
        //}

        //private void funcThreadScene()
        //{
        //    while (ctg.isFinished == false || ctg.qScene.Count > 0)
        //    {
        //        while (ctg.qScene.Count > 0)
        //        {
        //            tbS.Text = tbS.Text + ctg.qScene.Dequeue() + "\r\n";
        //        }

        //        //Thread.Sleep(2000);
        //    }

        //    //Thread.CurrentThread.Abort();
        //}

        private void bFeedback_Click(object sender, EventArgs e)
        {
            //ctg.FeedbackClassTerm(DateTime.Now.ToShortDateString(), "test",
            //                      op.Replace("\n", ";").Replace("\r", "") + "#" + oi.Replace("\n", ";").Replace("\r", "") + "#" + ot.Replace("\n", ";").Replace("\r", "") + "#" + os.Replace("\n", ";").Replace("\r", ""),
            //                      tbP.Text.Replace("\n", ";").Replace("\r", "") + "#" + tbI.Text.Replace("\n", ";").Replace("\r", "") + "#" + tbT.Text.Replace("\n", ";").Replace("\r", "") + "#" + tbS.Text.Replace("\n", ";").Replace("\r", ""));

            ncg.FeedbackClassTerm(DateTime.Now.ToShortDateString(), tbNewsNo.Text,
                                  op.Replace("\n", ";").Replace("\r", "") + "#" + oi.Replace("\n", ";").Replace("\r", "") + "#" + ot.Replace("\n", ";").Replace("\r", "") + "#" + os.Replace("\n", ";").Replace("\r", ""),
                                  tbP.Text.Replace("\n", ";").Replace("\r", "") + "#" + tbI.Text.Replace("\n", ";").Replace("\r", "") + "#" + tbT.Text.Replace("\n", ";").Replace("\r", "") + "#" + tbS.Text.Replace("\n", ";").Replace("\r", ""));
            MessageBox.Show("done");
        }

        //private void button1_Click(object sender, EventArgs e)
        //{
        //    Thread tx = new Thread(new ThreadStart(ppp));
        //    tx.Start();
        //}

        //private void ppp()
        //{
        //    Thread tx = new Thread(new ThreadStart(qqq));
        //    tx.Start();

        //    while (true) tbNews.Text = tbNews.Text + "x";
        //}

        //private void qqq()
        //{
        //    while (true) tbNews.Text = tbNews.Text + "y";
        //}
    }
}