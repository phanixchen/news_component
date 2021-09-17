using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Collections;
using System.Data;
using System.Threading;
using CS_Class;
using WebPageFetchLibrary;
using TokenLib;
//using Gapi.Search;


namespace NewsComponentGeneratorLib
{
    public class ThreadWithState
    {
        public float fMIscore;
        private float class_term_count;
        private string term, class_term, strKeywordid;
        private SQLConn cn;
        private string sql;
        private URLProcessing up = new URLProcessing();
        private string strpage;

        private TokenGen tg = new TokenGen();

        public ThreadWithState(string _term, string _termKeywordid, string _classterm, float _count_classterm, string db_address, string db_id, string db_password, string db_dbname)
        {
            cn = new SQLConn(db_address, db_id, db_password, db_dbname);
            term = _term;
            class_term = _classterm;
            strKeywordid = _termKeywordid;
            class_term_count = _count_classterm;
        }

        public void ThreadProc()
        {
            float fGoogleCountOfKeyword;
            DataTable dt = new DataTable();
            int m, n;

            try
            {
                //fGoogleCountOfKeyword = (float)((Searcher.Search(SearchType.Web, "\"" + term + "\"")).EstimatedResultCount);
                strpage = up.FetchURL("http://ajax.googleapis.com/ajax/services/search/web?v=1.0&q=\"" + term + "\"");
                m = strpage.IndexOf("estimatedResultCount");
                try
                {
                    m = strpage.IndexOf("\":\"", m);
                    n = strpage.IndexOf("\",", m);
                    fGoogleCountOfKeyword = Convert.ToSingle(strpage.Substring(m + 3, n - m - 3));
                }
                catch
                {
                    fGoogleCountOfKeyword = 0;
                }


                if (fGoogleCountOfKeyword == 0)
                {
                    fMIscore = 0;
                }
                else
                {
                    try
                    {
                        //fMIscore = (float)((Searcher.Search(SearchType.Web, "\"" + class_term + "\" \"" + term + "\"")).EstimatedResultCount) / (fGoogleCountOfKeyword * class_term_count);
                        strpage = up.FetchURL("http://ajax.googleapis.com/ajax/services/search/web?v=1.0&q=\"" + class_term + "\" \"" + term + "\"");
                        m = strpage.IndexOf("estimatedResultCount");
                        try
                        {
                            m = strpage.IndexOf("\":\"", m);
                            n = strpage.IndexOf("\",", m);
                            fMIscore = Convert.ToSingle(strpage.Substring(m + 3, n - m - 3)) / (fGoogleCountOfKeyword * class_term_count);
                        }
                        catch
                        {
                            fMIscore = 0;
                        }
                    }
                    catch
                    {
                        fMIscore = 0;
                    }
                }
            }
            catch (DivideByZeroException ex)
            {
                fMIscore = 0;
            }

            //這些詞一開始不知道確定的詞性，所以給 U(Unknown)
            sql = "Select * from News_Keyword_Class Where Class_name = '" + class_term + "' AND news_keyword_id = '" + strKeywordid + "' " +
                  "AND ntype = 'U';";
            cn.Exec(sql, ref dt);

            if (dt.Rows.Count == 0)
            {
                sql = "Insert into News_Keyword_Class (Class_name, news_keyword_id, ntype, count, google_score, FinalScore) values " +
                      "('" + class_term + "', '" + strKeywordid + "', 'U', '1', '" + fMIscore.ToString() + "', '" + fMIscore.ToString() + "');";
                cn.Exec(sql);
            }

            return;
        }
    }

    public class ThreadWordProcess
    {
        private string term, term2, term3, pos;
        private double dP, dS, dI, dT;
        private SQLConn cn;
        private string sql;
        private string strKeywordid, strNewsId;
        private string[] arrP, arrI, arrT, arrS;
        private long[] iP, iI, iT, iS;
        private bool bAdvSearch;
        private Queue qPerson, qItem, qTrans, qScene;
        string _db_address, _db_id, _db_password, _db_dbname;

        public ThreadWordProcess(string _term, string _term2, string _term3, bool _bAdvSearch,
                                 string _pos, double _dP, double _dI, double _dT, double _dS,
                                 string db_address, string db_id, string db_password, string db_dbname,
                                 string[] _arrP, string[] _arrI, string[] _arrT, string[] _arrS,
                                 long[] _iP, long[] _iI, long[] _iT, long[] _iS,
                                 ref Queue _qPerson, ref Queue _qItem, ref Queue _qTrans, ref Queue _qScene,
                                 string _strNewsId)
        {
            cn = new SQLConn(db_address, db_id, db_password, db_dbname);
            _db_address = db_address; _db_id = db_id; _db_password = db_password; _db_dbname = db_dbname;
            term = _term;
            term2 = _term2;
            term3 = _term3;
            pos = _pos;
            dP = _dP; dI = _dI; dT = _dT; dS = _dS;

            bAdvSearch = _bAdvSearch;

            arrP = _arrP; arrI = _arrI; arrT = _arrT; arrS = _arrS;
            iP = _iP; iI = _iI; iT = _iT; iS = _iS;

            qPerson = _qPerson; qItem = _qItem; qTrans = _qTrans; qScene = _qScene;

            strNewsId = _strNewsId;
        }

        public void ThreadProc()
        {
            DataTable dt = new DataTable();
            //SearchResults searchResults;

            float fMIscoreSum;
            Thread[] t;

            string[] arrtmp = null;
            long[] itmp = null;
            int j, k;

            #region 開始處理動詞、名詞

            #region 先將關鍵詞存入資料庫(ntype 不詳，等到feedback 的時候再補)
            //關鍵詞
            sql = "Select * from News_keyword where keyword = '" + term + "';";
            cn.Exec(sql, ref dt);

            if (dt.Rows.Count <= 0)
            {
                sql = "Insert into News_keyword (keyword) values ('" + term + "');";
                cn.Exec(sql);

                sql = "Select * from News_keyword where keyword = '" + term + "';";
                cn.Exec(sql, ref dt);
            }
            strKeywordid = dt.Rows[0]["news_keyword_id"].ToString();


            //exist in news <--> keyword relation?
            sql = "Select * from News_keywordr where NewsId = '" + strNewsId + "' AND news_keyword_id = '" + strKeywordid + "' and type = 1;";
            cn.Exec(sql, ref dt);

            if (dt.Rows.Count <= 0)
            {
                sql = "Insert into news_keywordr (NewsId, news_keyword_id, type, pos) values ('" + strNewsId + "', '" +
                      strKeywordid + "', 1, '" + pos + "');";
            }
            else
            {
                sql = "Update news_keywordr Set count = count + 1 where NewsId = '" + strNewsId + "' AND news_keyword_id = '" + strKeywordid + "' and type = 1;";
            }
            cn.Exec(sql);
            #endregion




            #region 檢查 xnl[i].ChildNodes[0].InnerText + xnl[i+1].ChildNodes[0].InnerText + xnl[i+2].ChildNodes[0].InnerText 是不是已經有在資料庫的 news_keyword_class 表格中，有的話直接套用
            // xnl[i].ChildNodes[0].InnerText : term
            try
            {
                sql = "select top 1 ntype, FinalScore from news_keyword a inner join news_keyword_class b on a.news_keyword_id = b.news_keyword_id and " +
                      "a.keyword = '" + term + term2 + term3 + "' order by FinalScore desc;";
                cn.Exec(sql, ref dt);


                if (dt.Rows.Count >= 1)
                {
                    if (dt.Rows[0]["ntype"].ToString().ToUpper() == "P" && Convert.ToDouble(dt.Rows[0]["FinalScore"]) >= dP)
                    {
                        qPerson.Enqueue(term + term2 + term3);
                    }
                    else if (dt.Rows[0]["ntype"].ToString().ToUpper() == "T" && Convert.ToDouble(dt.Rows[0]["FinalScore"]) >= dT)
                    {
                        qTrans.Enqueue(term + term2 + term3);
                    }
                    else if (dt.Rows[0]["ntype"].ToString().ToUpper() == "S" && Convert.ToDouble(dt.Rows[0]["FinalScore"]) >= dS)
                    {
                        qScene.Enqueue(term + term2 + term3);
                    }
                    else if (dt.Rows[0]["ntype"].ToString().ToUpper() == "I" && Convert.ToDouble(dt.Rows[0]["FinalScore"]) >= dI)
                    {
                        qItem.Enqueue(term + term2 + term3);
                    }

                    //如果是未判定的詞，則交由後面做處理
                    if (dt.Rows[0]["ntype"].ToString().ToUpper() != "U")
                    {
                        return;
                    }
                }
            }
            catch
            {
            }
            #endregion

            #region 檢查 xnl[i].ChildNodes[0].InnerText + xnl[i+1].ChildNodes[0].InnerText 是不是已經有在資料庫的 news_keyword_class 表格中，有的話直接套用
            // xnl[i].ChildNodes[0].InnerText : term
            try
            {
                sql = "select top 1 ntype, FinalScore from news_keyword a inner join news_keyword_class b on a.news_keyword_id = b.news_keyword_id and " +
                      "a.keyword = '" + term + term2 + "' order by FinalScore desc;";
                cn.Exec(sql, ref dt);


                if (dt.Rows.Count >= 1)
                {
                    if (dt.Rows[0]["ntype"].ToString().ToUpper() == "P" && Convert.ToDouble(dt.Rows[0]["FinalScore"]) >= dP)
                    {
                        qPerson.Enqueue(term + term2);
                    }
                    else if (dt.Rows[0]["ntype"].ToString().ToUpper() == "T" && Convert.ToDouble(dt.Rows[0]["FinalScore"]) >= dT)
                    {
                        qTrans.Enqueue(term + term2);
                    }
                    else if (dt.Rows[0]["ntype"].ToString().ToUpper() == "S" && Convert.ToDouble(dt.Rows[0]["FinalScore"]) >= dS)
                    {
                        qScene.Enqueue(term + term2);
                    }
                    else if (dt.Rows[0]["ntype"].ToString().ToUpper() == "I" && Convert.ToDouble(dt.Rows[0]["FinalScore"]) >= dI)
                    {
                        qItem.Enqueue(term + term2);
                    }

                    //如果是未判定的詞，則交由後面做處理
                    if (dt.Rows[0]["ntype"].ToString().ToUpper() != "U")
                    {
                        return;
                    }
                }
            }
            catch
            {
            }
            #endregion

            #region 檢查 xnl[i].ChildNodes[0].InnerText 是不是已經有在資料庫的 news_keyword_class 表格中，有的話直接套用
            // xnl[i].ChildNodes[0].InnerText : term
            sql = "select top 1 ntype, FinalScore from news_keyword a inner join news_keyword_class b on a.news_keyword_id = b.news_keyword_id and " +
                  "a.keyword = '" + term + "' order by FinalScore desc;";
            cn.Exec(sql, ref dt);


            if (dt.Rows.Count >= 1)
            {
                if (dt.Rows[0]["ntype"].ToString().ToUpper() == "P" && Convert.ToDouble(dt.Rows[0]["FinalScore"]) >= dP)
                {
                    qPerson.Enqueue(term);
                }
                else if (dt.Rows[0]["ntype"].ToString().ToUpper() == "T" && Convert.ToDouble(dt.Rows[0]["FinalScore"]) >= dT)
                {
                    qTrans.Enqueue(term);
                }
                else if (dt.Rows[0]["ntype"].ToString().ToUpper() == "S" && Convert.ToDouble(dt.Rows[0]["FinalScore"]) >= dS)
                {
                    qScene.Enqueue(term);
                }
                else if (dt.Rows[0]["ntype"].ToString().ToUpper() == "I" && Convert.ToDouble(dt.Rows[0]["FinalScore"]) >= dI)
                {
                    qItem.Enqueue(term);
                }

                //如果是未判定的詞，則交由後面做處理
                if (dt.Rows[0]["ntype"].ToString().ToUpper() != "U")
                {
                    return;
                }
            }
            #endregion

            #region 沒有在資料庫的 news_keyword_class 中出現過，那麼就另外判斷
            if (pos.IndexOf("NOUN") >= 0 && term.IndexOf("，") < 0 && bAdvSearch == true)
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

                    #region 計算 MIscore, 同時加入 news_keyword_class 表格中
                    fMIscoreSum = 0;


                    //threading!!!!!
                    ThreadWithState[] tws = new ThreadWithState[arrtmp.Length];
                    t = new Thread[arrtmp.Length];
                    for (j = 0; j < arrtmp.Length; j++)
                    {
                        tws[j] = new ThreadWithState(term, strKeywordid, arrtmp[j], itmp[j], _db_address, _db_id, _db_password, _db_dbname);
                        t[j] = new Thread(new ThreadStart(tws[j].ThreadProc));
                        t[j].Start();


                        //避免同時太多 request
                        Thread.Sleep(250);
                        //Thread.Sleep(1000);
                    }

                    for (j = 0; j < arrtmp.Length; j++)
                    {
                        t[j].Join();
                        t[j].Abort();
                        t[j] = null;

                        fMIscoreSum = fMIscoreSum + tws[j].fMIscore;
                    }
                    #endregion

                    #region 檢查有沒有達到分類平均分數, 如果有的話加到分類裡頭去
                    bool bIdentified = false;
                    switch (k)
                    {
                        case 0:
                            if (dP <= ((double)fMIscoreSum) / ((double)arrtmp.Length))
                            {
                                qPerson.Enqueue(term);

                                bIdentified = true;
                            }
                            break;
                        case 1:
                            if (dI <= ((double)fMIscoreSum) / ((double)arrtmp.Length))
                            {
                                qItem.Enqueue(term);

                                bIdentified = true;
                            }
                            break;
                        case 2:
                            if (dT <= ((double)fMIscoreSum) / ((double)arrtmp.Length))
                            {
                                qTrans.Enqueue(term);

                                bIdentified = true;
                            }
                            break;
                        case 3:
                            if (dS <= ((double)fMIscoreSum) / ((double)arrtmp.Length))
                            {
                                qScene.Enqueue(term);

                                bIdentified = true;
                            }
                            break;
                    }
                    #endregion

                    if (bIdentified == true)
                        break;
                }
            }
            #endregion

            #endregion
        }
    }

    public class ClassTermGenerator
    {
        private SQLConn cn;
        private string sql;
        public Queue qPerson, qItem, qTrans, qScene;
        private Queue qPriPerson, qPriItem, qPriTrans, qPriScene;
        private string _db_address, _db_id, _db_password, _db_dbname;
        public bool isFinished;
        public bool isStarted;
        //private YahooSegApi ysa = new YahooSegApi("b2H9zvzV34HiVnyiQaNoF_XwUat82S9d70b5WhK4qnJmSk.bOPr.GY6CEFct6hwOsg--");
        private const int MaxPlusCount = 50;
        private const int MinSubCount = -10;
        private const int iUserSubtractDiv = 10;
        private string strpage;
        private URLProcessing up = new URLProcessing();

        private TokenGen tg = new TokenGen();

        public ClassTermGenerator(string db_address, string db_id, string db_password, string db_dbname)
        {
            cn = new SQLConn(db_address, db_id, db_password, db_dbname);
            _db_address = db_address; _db_id = db_id; _db_password = db_password; _db_dbname = db_dbname;
            qPerson = new Queue();
            qItem = new Queue();
            qTrans = new Queue();
            qScene = new Queue();

            isFinished = false;
        }

        public void reset()
        {
            isFinished = false;
            isStarted = false;

            qPerson = new Queue();
            qItem = new Queue();
            qTrans = new Queue();
            qScene = new Queue();

            qPriPerson = new Queue();
            qPriItem = new Queue();
            qPriTrans = new Queue();
            qPriScene = new Queue();
        }

        public void GenerateClassTerm(string _NewsContent, bool _bAdvSearch, string _newsdate, string _newsid, string _COUserID)
        {
            DataTable dt = new DataTable();
            XmlDocument xd = new XmlDocument();
            XmlNodeList xnl;
            int i, j, k, m, n;
            double dP, dS, dI, dT;
            string strKeywordid, strNewsId;

            Queue qIdentifiedTerm = new Queue();

            //儲存 NewsContent
            sql = "Insert into News_Content (news_no, NewsDate, news_content, author) values " +
                  "('" + _newsid + "','" + _newsdate + "','" + _NewsContent + "','" + _COUserID + "');";
            cn.Exec(sql);

            //取得 newsid (serial id)
            sql = "select * from news_content where news_no = '" + _newsid + "' and NewsDate = '" + _newsdate + "';";
            cn.Exec(sql, ref dt);

            strNewsId = dt.Rows[0]["newsid"].ToString();


            //POS Tagging
            //xd.LoadXml(ysa.WordSegmentation(_NewsContent.Replace("\n", "，").Replace("\r", "").Replace("，，", "，").Replace("，，", "，").Replace("，，", "，").Replace("，，", "，")));
            xd.LoadXml(tg.TokenPOS(_NewsContent));

            xnl = xd.ChildNodes[1].ChildNodes;


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


            Thread[] tWords;

            //Reference terms of Classes
            string[] arrP = { "人物", "角色", "官員", "人員" };
            string[] arrS = { "單位", "組織", "部門", "建築", "場景", "位置" };
            string[] arrI = { "物品", "用品", "器官", "家具" };
            string[] arrT = { "交通工具", "運輸", "車輛" };

            //Google Count of reference terms of classes
            long[] iP = new long[arrP.Length];
            long[] iS = new long[arrS.Length];
            long[] iI = new long[arrI.Length];
            long[] iT = new long[arrT.Length];

            string[] arrtmp = null;
            long[] itmp = null;

            //SearchResults searchResults;

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
                    //searchResults = Searcher.Search(SearchType.Web, "\"" + arrtmp[j] + "\"");
                    strpage = up.FetchURL("http://ajax.googleapis.com/ajax/services/search/web?v=1.0&q=\"" + arrtmp[j] + "\"");
                    m = strpage.IndexOf("estimatedResultCount");
                    try
                    {
                        m = strpage.IndexOf("\":\"", m);
                        n = strpage.IndexOf("\",", m);
                        itmp[j] = Convert.ToInt64(strpage.Substring(m + 3, n - m - 3));
                    }
                    catch
                    {
                        itmp[j] = 0;
                    }

                    //itmp[j] = searchResults.EstimatedResultCount;
                }
            }
            #endregion

            tWords = new Thread[xnl.Count];
            ThreadWordProcess[] twp = new ThreadWordProcess[xnl.Count];

            for (i = 0; i < xnl.Count; i++)
            {
                if (xnl[i].ChildNodes.Count == 5)
                {
                    if (xnl[i].ChildNodes[0].InnerText.Trim() != "，" && xnl[i].ChildNodes[0].InnerText.Trim() != "")
                    {
                        if ((xnl[i].ChildNodes[4].InnerText.IndexOf("VERB") >= 0 && xnl[i].ChildNodes[4].InnerText.IndexOf("ADV") < 0) ||
                             xnl[i].ChildNodes[4].InnerText.IndexOf("ADJ") >= 0 ||
                            (xnl[i].ChildNodes[4].InnerText.IndexOf("NOUN") >= 0 && //xnl[i].ChildNodes[4].InnerText.IndexOf("PROPER_NOUN") < 0 &&
                             xnl[i].ChildNodes[4].InnerText.IndexOf("NOUN_TIME") < 0 && xnl[i].ChildNodes[4].InnerText.IndexOf("PRONOUN") < 0)
                            )
                        {
                            //只處理動詞、名詞
                        }
                        else
                        {
                            //不是動詞、名詞就跳過
                            continue;
                        }

                        if (qIdentifiedTerm.Contains(xnl[i].ChildNodes[0].InnerText.Trim()) == true)
                        {
                            continue;
                        }
                        qIdentifiedTerm.Enqueue(xnl[i].ChildNodes[0].InnerText.Trim());


                        string term1, term2, term3;
                        try
                        {
                            term1 = xnl[i].ChildNodes[0].InnerText.Trim();
                        }
                        catch
                        {
                            term1 = "";
                        }
                        try
                        {
                            term2 = xnl[i + 1].ChildNodes[0].InnerText.Trim();
                        }
                        catch
                        {
                            term2 = "";
                        }
                        try
                        {
                            term3 = xnl[i + 2].ChildNodes[0].InnerText.Trim();
                        }
                        catch
                        {
                            term3 = "";
                        }

                        twp[i] = new ThreadWordProcess(term1, term2, term3, _bAdvSearch,
                                                       xnl[i].ChildNodes[4].InnerText, dP, dI, dT, dS,
                                                       _db_address, _db_id, _db_password, _db_dbname, arrP, arrI, arrT, arrS, iP, iI, iT, iS,
                                                       ref qPerson, ref qItem, ref qTrans, ref qScene,
                                                       strNewsId);
                        tWords[i] = new Thread(new ThreadStart(twp[i].ThreadProc));
                        tWords[i].Start();


                        //避免同時太多 request... 稍停一下
                        //if (i % 5 == 4)
                        //{
                        Thread.Sleep(250);
                        //}

                        #region 開始處理動詞、名詞

                        //#region 先將關鍵詞存入資料庫(ntype 不詳，等到feedback 的時候再補)
                        ////關鍵詞
                        //sql = "Select * from News_keyword where keyword = '" + xnl[i].ChildNodes[0].InnerText.Trim() + "';";
                        //cn.Exec(sql, ref dt);

                        //if (dt.Rows.Count <= 0)
                        //{
                        //    sql = "Insert into News_keyword (keyword) values ('" + xnl[i].ChildNodes[0].InnerText.Trim() + "');";
                        //    cn.Exec(sql);

                        //    sql = "Select * from News_keyword where keyword = '" + xnl[i].ChildNodes[0].InnerText.Trim() + "';";
                        //    cn.Exec(sql, ref dt);
                        //}
                        //strKeywordid = dt.Rows[0]["news_keyword_id"].ToString();


                        ////exist in news <--> keyword relation?
                        //sql = "Select * from News_keywordr where NewsId = '" + strNewsId + "' AND news_keyword_id = '" + strKeywordid + "' and type = 1;";
                        //cn.Exec(sql, ref dt);

                        //if (dt.Rows.Count <= 0)
                        //{
                        //    sql = "Insert into news_keywordr (NewsId, news_keyword_id, type, pos) values ('" + strNewsId + "', '" +
                        //          strKeywordid + "', 1, '" + xnl[i].ChildNodes[4].InnerText + "');";
                        //}
                        //else
                        //{
                        //    sql = "Update news_keywordr Set count = count + 1 where NewsId = '" + strNewsId + "' AND news_keyword_id = '" + strKeywordid + "' and type = 1;";
                        //}
                        //cn.Exec(sql);
                        //#endregion


                        //if (qIdentifiedTerm.Contains(xnl[i].ChildNodes[0].InnerText.Trim()) == true)
                        //{
                        //    continue;
                        //}
                        //qIdentifiedTerm.Enqueue(xnl[i].ChildNodes[0].InnerText.Trim());

                        //#region 檢查 xnl[i].ChildNodes[0].InnerText + xnl[i+1].ChildNodes[0].InnerText + xnl[i+2].ChildNodes[0].InnerText 是不是已經有在資料庫的 news_keyword_class 表格中，有的話直接套用
                        //// xnl[i].ChildNodes[0].InnerText : term
                        //try
                        //{
                        //    sql = "select top 1 ntype, FinalScore from news_keyword a inner join news_keyword_class b on a.news_keyword_id = b.news_keyword_id and " +
                        //          "a.keyword = '" + xnl[i].ChildNodes[0].InnerText + xnl[i + 1].ChildNodes[0].InnerText + xnl[i+2].ChildNodes[0].InnerText + "' order by FinalScore desc;";
                        //    cn.Exec(sql, ref dt);


                        //    if (dt.Rows.Count >= 1)
                        //    {
                        //        if (dt.Rows[0]["ntype"].ToString().ToUpper() == "P" && Convert.ToDouble(dt.Rows[0]["FinalScore"]) >= dP)
                        //        {
                        //            qPerson.Enqueue(xnl[i].ChildNodes[0].InnerText.Trim());
                        //        }
                        //        else if (dt.Rows[0]["ntype"].ToString().ToUpper() == "T" && Convert.ToDouble(dt.Rows[0]["FinalScore"]) >= dT)
                        //        {
                        //            qTrans.Enqueue(xnl[i].ChildNodes[0].InnerText.Trim());
                        //        }
                        //        else if (dt.Rows[0]["ntype"].ToString().ToUpper() == "S" && Convert.ToDouble(dt.Rows[0]["FinalScore"]) >= dS)
                        //        {
                        //            qScene.Enqueue(xnl[i].ChildNodes[0].InnerText.Trim());
                        //        }
                        //        else if (dt.Rows[0]["ntype"].ToString().ToUpper() == "I" && Convert.ToDouble(dt.Rows[0]["FinalScore"]) >= dI)
                        //        {
                        //            qItem.Enqueue(xnl[i].ChildNodes[0].InnerText.Trim());
                        //        }

                        //        //如果是未判定的詞，則交由後面做處理
                        //        if (dt.Rows[0]["ntype"].ToString().ToUpper() != "U")
                        //        {
                        //            i = i + 2;
                        //            continue;
                        //        }
                        //    }
                        //}
                        //catch
                        //{
                        //}
                        //#endregion

                        //#region 檢查 xnl[i].ChildNodes[0].InnerText + xnl[i+1].ChildNodes[0].InnerText 是不是已經有在資料庫的 news_keyword_class 表格中，有的話直接套用
                        //// xnl[i].ChildNodes[0].InnerText : term
                        //try
                        //{
                        //    sql = "select top 1 ntype, FinalScore from news_keyword a inner join news_keyword_class b on a.news_keyword_id = b.news_keyword_id and " +
                        //          "a.keyword = '" + xnl[i].ChildNodes[0].InnerText + xnl[i + 1].ChildNodes[0].InnerText + "' order by FinalScore desc;";
                        //    cn.Exec(sql, ref dt);


                        //    if (dt.Rows.Count >= 1)
                        //    {
                        //        if (dt.Rows[0]["ntype"].ToString().ToUpper() == "P" && Convert.ToDouble(dt.Rows[0]["FinalScore"]) >= dP)
                        //        {
                        //            qPerson.Enqueue(xnl[i].ChildNodes[0].InnerText.Trim());
                        //        }
                        //        else if (dt.Rows[0]["ntype"].ToString().ToUpper() == "T" && Convert.ToDouble(dt.Rows[0]["FinalScore"]) >= dT)
                        //        {
                        //            qTrans.Enqueue(xnl[i].ChildNodes[0].InnerText.Trim());
                        //        }
                        //        else if (dt.Rows[0]["ntype"].ToString().ToUpper() == "S" && Convert.ToDouble(dt.Rows[0]["FinalScore"]) >= dS)
                        //        {
                        //            qScene.Enqueue(xnl[i].ChildNodes[0].InnerText.Trim());
                        //        }
                        //        else if (dt.Rows[0]["ntype"].ToString().ToUpper() == "I" && Convert.ToDouble(dt.Rows[0]["FinalScore"]) >= dI)
                        //        {
                        //            qItem.Enqueue(xnl[i].ChildNodes[0].InnerText.Trim());
                        //        }

                        //        //如果是未判定的詞，則交由後面做處理
                        //        if (dt.Rows[0]["ntype"].ToString().ToUpper() != "U")
                        //        {
                        //            i++;
                        //            continue;
                        //        }
                        //    }
                        //}
                        //catch
                        //{
                        //}
                        //#endregion

                        //#region 檢查 xnl[i].ChildNodes[0].InnerText 是不是已經有在資料庫的 news_keyword_class 表格中，有的話直接套用
                        //// xnl[i].ChildNodes[0].InnerText : term
                        //sql = "select top 1 ntype, FinalScore from news_keyword a inner join news_keyword_class b on a.news_keyword_id = b.news_keyword_id and " +
                        //      "a.keyword = '" + xnl[i].ChildNodes[0].InnerText + "' order by FinalScore desc;";
                        //cn.Exec(sql, ref dt);


                        //if (dt.Rows.Count >= 1)
                        //{
                        //    if (dt.Rows[0]["ntype"].ToString().ToUpper() == "P" && Convert.ToDouble(dt.Rows[0]["FinalScore"]) >= dP)
                        //    {
                        //        qPerson.Enqueue(xnl[i].ChildNodes[0].InnerText.Trim());
                        //    }
                        //    else if (dt.Rows[0]["ntype"].ToString().ToUpper() == "T" && Convert.ToDouble(dt.Rows[0]["FinalScore"]) >= dT)
                        //    {
                        //        qTrans.Enqueue(xnl[i].ChildNodes[0].InnerText.Trim());
                        //    }
                        //    else if (dt.Rows[0]["ntype"].ToString().ToUpper() == "S" && Convert.ToDouble(dt.Rows[0]["FinalScore"]) >= dS)
                        //    {
                        //        qScene.Enqueue(xnl[i].ChildNodes[0].InnerText.Trim());
                        //    }
                        //    else if (dt.Rows[0]["ntype"].ToString().ToUpper() == "I" && Convert.ToDouble(dt.Rows[0]["FinalScore"]) >= dI)
                        //    {
                        //        qItem.Enqueue(xnl[i].ChildNodes[0].InnerText.Trim());
                        //    }

                        //    //如果是未判定的詞，則交由後面做處理
                        //    if (dt.Rows[0]["ntype"].ToString().ToUpper() != "U")
                        //    {
                        //        continue;
                        //    }
                        //}
                        //#endregion

                        //#region 沒有在資料庫的 news_keyword_class 中出現過，那麼就另外判斷
                        //if (xnl[i].ChildNodes[4].InnerText.IndexOf("NOUN") >= 0 && xnl[i].ChildNodes[0].InnerText.Trim().IndexOf("，") < 0 && _bAdvSearch == true)
                        //{
                        //    for (k = 0; k <= 3; k++)
                        //    {
                        //        switch (k)
                        //        {
                        //            case 0:
                        //                arrtmp = arrP;
                        //                itmp = iP;
                        //                break;
                        //            case 1:
                        //                arrtmp = arrI;
                        //                itmp = iI;
                        //                break;
                        //            case 2:
                        //                arrtmp = arrT;
                        //                itmp = iT;
                        //                break;
                        //            case 3:
                        //                arrtmp = arrS;
                        //                itmp = iS;
                        //                break;
                        //        }

                        //        if (arrtmp == null)
                        //            continue;

                        //        #region 計算 MIscore, 同時加入 news_keyword_class 表格中
                        //        fMIscoreSum = 0;


                        //        //threading!!!!!
                        //        ThreadWithState[] tws = new ThreadWithState[arrtmp.Length];
                        //        t = new Thread[arrtmp.Length];
                        //        for (j = 0; j < arrtmp.Length; j++)
                        //        {
                        //            tws[j] = new ThreadWithState(xnl[i].ChildNodes[0].InnerText.Trim(), strKeywordid, arrtmp[j], itmp[j], _db_address, _db_id, _db_password, _db_dbname);
                        //            t[j] = new Thread(new ThreadStart(tws[j].ThreadProc));
                        //            t[j].Start();
                        //        }

                        //        for (j = 0; j < arrtmp.Length; j++)
                        //        {
                        //            t[j].Join();
                        //            t[j].Abort();
                        //            t[j] = null;

                        //            fMIscoreSum = fMIscoreSum + tws[j].fMIscore;
                        //        }
                        //        #endregion

                        //        #region 檢查有沒有達到分類平均分數, 如果有的話加到分類裡頭去
                        //        bool bIdentified = false;
                        //        switch (k)
                        //        {
                        //            case 0:
                        //                if (dP <= ((double)fMIscoreSum) / ((double)arrtmp.Length))
                        //                {
                        //                    qPerson.Enqueue(xnl[i].ChildNodes[0].InnerText.Trim());

                        //                    bIdentified = true;
                        //                }
                        //                break;
                        //            case 1:
                        //                if (dI <= ((double)fMIscoreSum) / ((double)arrtmp.Length))
                        //                {
                        //                    qItem.Enqueue(xnl[i].ChildNodes[0].InnerText.Trim());

                        //                    bIdentified = true;
                        //                }
                        //                break;
                        //            case 2:
                        //                if (dT <= ((double)fMIscoreSum) / ((double)arrtmp.Length))
                        //                {
                        //                    qTrans.Enqueue(xnl[i].ChildNodes[0].InnerText.Trim());

                        //                    bIdentified = true;
                        //                }
                        //                break;
                        //            case 3:
                        //                if (dS <= ((double)fMIscoreSum) / ((double)arrtmp.Length))
                        //                {
                        //                    qScene.Enqueue(xnl[i].ChildNodes[0].InnerText.Trim());

                        //                    bIdentified = true;
                        //                }
                        //                break;
                        //        }
                        //        #endregion

                        //        if (bIdentified == true)
                        //            break;
                        //    }
                        //}
                        //#endregion

                        //isStarted = true;
                        #endregion
                    }
                }
            }

            for (i = 0; i < xnl.Count; i++)
            {
                if (tWords[i] != null)
                {
                    tWords[i].Join();
                }
            }
            isFinished = true;
        }


        //process feedback
        public void FeedbackClassTerm(string _newsdate, string _newsid, string strOriginalTerm, string strFeedbackTerm)
        {
            DataTable dt = new DataTable();
            int i, j, k, m, n;
            string strKeywordid, strNewsId, strNtype = "";

            string[] strtmp;
            string[] arrOP, arrOI, arrOT, arrOS, arrFP, arrFI, arrFT, arrFS;
            char[] arrSep = { ';' };

            sql = "Insert into News_Component_log (NewsId, KeywordString, KeywordStringFeedBack) ";
            cn.Exec(sql);

            strtmp = strOriginalTerm.Split('#');
            arrOP = strtmp[0].Split(arrSep, StringSplitOptions.RemoveEmptyEntries);
            arrOI = strtmp[1].Split(arrSep, StringSplitOptions.RemoveEmptyEntries);
            arrOT = strtmp[2].Split(arrSep, StringSplitOptions.RemoveEmptyEntries);
            arrOS = strtmp[3].Split(arrSep, StringSplitOptions.RemoveEmptyEntries);

            strtmp = strFeedbackTerm.Split('#');
            arrFP = strtmp[0].Split(arrSep, StringSplitOptions.RemoveEmptyEntries);
            arrFI = strtmp[1].Split(arrSep, StringSplitOptions.RemoveEmptyEntries);
            arrFT = strtmp[2].Split(arrSep, StringSplitOptions.RemoveEmptyEntries);
            arrFS = strtmp[3].Split(arrSep, StringSplitOptions.RemoveEmptyEntries);

            //Reference terms of Classes
            string[] arrP = { "人物", "角色", "官員", "人員" };
            string[] arrS = { "單位", "組織", "部門", "建築", "場景", "位置" };
            string[] arrI = { "物品", "用品", "器官", "家具" };
            string[] arrT = { "交通工具", "運輸", "車輛" };

            //Google Count of reference terms of classes
            long[] iP = new long[arrP.Length];
            long[] iS = new long[arrS.Length];
            long[] iI = new long[arrI.Length];
            long[] iT = new long[arrT.Length];

            string[] arrOtmp = null, arrFtmp = null, arrtmp = null;
            long[] itmp = null;
            bool bExist;
            Queue qNewTerms = new Queue();
            string strTmpNewTerm;

            Thread[] t;
            //SearchResults searchResults;

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
                    //searchResults = Searcher.Search(SearchType.Web, "\"" + arrtmp[j] + "\"");
                    strpage = up.FetchURL("http://ajax.googleapis.com/ajax/services/search/web?v=1.0&q=\"" + arrtmp[j] + "\"");
                    m = strpage.IndexOf("estimatedResultCount");
                    try
                    {
                        m = strpage.IndexOf("\":\"", m);
                        n = strpage.IndexOf("\",", m);
                        itmp[j] = Convert.ToInt64(strpage.Substring(m + 3, n - m - 3));
                    }
                    catch
                    {
                        itmp[j] = 0;
                    }


                    //itmp[j] = searchResults.EstimatedResultCount;
                }
            }
            #endregion

            sql = "select * from news_content where news_no = '" + _newsid + "' and NewsDate = '" + _newsdate + "' Order by NewsId DESC;";
            cn.Exec(sql, ref dt);

            strNewsId = dt.Rows[0]["newsid"].ToString();

            //feedback log
            sql = "Insert into News_Component_log (NewsId, KeywordString, KeywordStringFeedBack) values ('" + strNewsId + "', '" + strOriginalTerm + "', '" + strFeedbackTerm + "');";
            cn.Exec(sql);

            for (i = 0; i < 4; i++)
            {
                switch (i)
                {
                    case 0:
                        arrOtmp = arrOP;
                        arrFtmp = arrFP;
                        strNtype = "P";
                        arrtmp = arrP;
                        itmp = iP;
                        break;

                    case 1:
                        arrOtmp = arrOI;
                        arrFtmp = arrFI;
                        strNtype = "I";
                        arrtmp = arrI;
                        itmp = iI;
                        break;

                    case 2:
                        arrOtmp = arrOT;
                        arrFtmp = arrFT;
                        strNtype = "T";
                        arrtmp = arrT;
                        itmp = iT;
                        break;

                    case 3:
                        arrOtmp = arrOS;
                        arrFtmp = arrFS;
                        strNtype = "S";
                        arrtmp = arrS;
                        itmp = iS;
                        break;
                }

                if (arrFtmp.Length == 0) continue;

                #region check whether there is a new term in feedback terms
                for (j = 0; j < arrFtmp.Length; j++)
                {
                    #region get keyword id
                    sql = "Select * from News_keyword where keyword = '" + arrFtmp[j] + "';";
                    cn.Exec(sql, ref dt);

                    if (dt.Rows.Count <= 0)
                    {
                        // not exist in the DB, i.e. a new keyword
                        sql = "Insert into News_keyword (keyword) values ('" + arrFtmp[j] + "');";
                        cn.Exec(sql);

                        sql = "Select * from News_keyword where keyword = '" + arrFtmp[j] + "';";
                        cn.Exec(sql, ref dt);
                    }
                    strKeywordid = dt.Rows[0]["news_keyword_id"].ToString();
                    #endregion


                    #region Update news_keywordr
                    sql = "select * from News_keywordr where newsid = '" + strNewsId + "' and news_keyword_id = '" + strKeywordid + "';";
                    cn.Exec(sql, ref dt);

                    if (dt.Rows.Count == 0)
                    {
                        //User keyword, insert
                        sql = "Insert into News_keywordr (NewsId, news_keyword_id, type, ntype) values ('" + strNewsId + "', '" + strKeywordid + "', 3, '" + strNtype + "');";
                        cn.Exec(sql);
                    }
                    else
                    {
                        //exists in news, update ntype
                        sql = "Update News_keywordr Set ntype = '" + strNtype + "' where newsid = '" + strNewsId + "' and news_keyword_id = '" + strKeywordid + "' and type = 1;";
                        cn.Exec(sql);
                    }
                    #endregion


                    #region update arrFtmp[j]'s ntype information in News_keyword_class
                    sql = "Select * from News_Keyword_Class where news_keyword_id = '" + strKeywordid + "' AND ntype = '" + strNtype + "';";
                    cn.Exec(sql, ref dt);

                    if (dt.Rows.Count == 0)
                    {
                        #region different nType, exist Ntype = U ?
                        sql = "Select * from News_Keyword_Class where news_keyword_id = '" + strKeywordid + "' and ntype = 'U';";
                        cn.Exec(sql, ref dt);

                        if (dt.Rows.Count > 0)
                        {
                            //modify Ntype = U --> Ntype = strNtype
                            sql = "Update News_Keyword_Class Set Ntype = '" + strNtype + "' where news_keyword_id = '" + strKeywordid + "' and ntype = 'U';";
                            cn.Exec(sql);
                        }
                        else
                        {
                            //no Ntype = U exists --> insert new Ntype = strNtype
                            sql = "Select * from News_Keyword_Class where news_keyword_id = '" + strKeywordid + "' order by ntype;";
                            cn.Exec(sql, ref dt);

                            if (dt.Rows.Count == 0)
                            {
                                #region new keyword
                                #region 加入 news_keyword_class 表格中(let Ntype = U)

                                //threading!!!!!
                                ThreadWithState[] tws = new ThreadWithState[arrtmp.Length];
                                t = new Thread[arrtmp.Length];
                                for (k = 0; k < arrtmp.Length; k++)
                                {
                                    tws[k] = new ThreadWithState(arrFtmp[j], strKeywordid, arrtmp[k], itmp[k], _db_address, _db_id, _db_password, _db_dbname);
                                    t[k] = new Thread(new ThreadStart(tws[k].ThreadProc));
                                    t[k].Start();
                                }

                                for (k = 0; k < arrtmp.Length; k++)
                                {
                                    t[k].Join();
                                    t[k].Abort();
                                    t[k] = null;
                                }
                                #endregion

                                //更新 Ntype = strNtype, UserSubtractScore++
                                sql = "Update News_Keyword_Class Set Ntype = '" + strNtype + "', UserSubtractScore = UserSubtractScore + 1 " +
                                      "where news_keyword_id = '" + strKeywordid + "' and ntype = 'U';";
                                cn.Exec(sql);

                                //重新計算 FinalScore
                                sql = "Update News_Keyword_Class Set FinalScore = google_score * (1+ UserSubtractScore/" + iUserSubtractDiv.ToString() + ") where news_keyword_id = '" + strKeywordid + "';";
                                cn.Exec(sql);
                                #endregion
                            }
                            else
                            {
                                #region not new keyword, decrease final score of other ntypes, and copy to strNtype

                                //decrease the final score of other ntypes
                                sql = "Update News_Keyword_Class set UserSubtractScore = UserSubtractScore -1 where news_keyword_id = '" + strKeywordid + "' and UserSubtractScore > " + MinSubCount.ToString() + ";";
                                cn.Exec(sql);
                                sql = "Update News_Keyword_Class Set FinalScore = google_score * (1+ UserSubtractScore/" + iUserSubtractDiv.ToString() + ") where news_keyword_id = '" + strKeywordid + "';";
                                cn.Exec(sql);
                                sql = "";


                                //Insert new ntype
                                for (k = 0; k < dt.Rows.Count; k++)
                                {
                                    if (k != 0)
                                    {
                                        if (dt.Rows[k]["ntype"].ToString() != dt.Rows[k - 1]["ntype"].ToString())
                                        {
                                            break;
                                        }
                                    }

                                    sql = sql + "Insert into News_keyword_Class (Class_name, news_keyword_id, ntype, count, google_score, UserSubtractScore, FinalScore) values " +
                                          "('" + dt.Rows[k]["Class_name"].ToString() + "', '" + strKeywordid + "', '" + strNtype + "', '" + dt.Rows[k]["count"].ToString() +
                                          "', '" + dt.Rows[k]["google_score"].ToString() + "', '1', '" + Convert.ToString(Convert.ToDouble(dt.Rows[k]["google_score"]) * (1 + 1 / iUserSubtractDiv)) + "');";
                                }
                                cn.Exec(sql);
                                #endregion
                            }
                        }
                        #endregion
                    }
                    else
                    {
                        #region same ntype
                        //check affFtmp[j] is in affOtmp?
                        bExist = false;
                        for (k = 0; k < arrOtmp.Length; k++)
                        {
                            if (arrFtmp[j].ToUpper() == arrOtmp[k].ToUpper())
                            {
                                bExist = true;
                                break;
                            }
                        }

                        if (bExist == false)
                        {
                            //increase UserSubtractScore, update final score
                            sql = "Update News_Keyword_Class Set UserSubtractScore = UserSubtractScore + 1 " +
                                  "where UserSubtractScore < " + MaxPlusCount.ToString() + " AND news_keyword_id = '" + strKeywordid + "' AND ntype = '" + strNtype + "';";
                            cn.Exec(sql);

                            sql = "Update News_Keyword_Class Set FinalScore = google_score * (1 + UserSubtractScore/" + iUserSubtractDiv.ToString() + ") " +
                                  "where news_keyword_id = '" + strKeywordid + "' AND ntype = '" + strNtype + "';";
                            cn.Exec(sql);
                        }
                        #endregion
                    }
                    #endregion
                }
                #endregion


                #region check whether there is a deleted old term
                for (j = 0; j < arrOtmp.Length; j++)
                {
                    bExist = false;
                    for (k = 0; k < arrFtmp.Length; k++)
                    {
                        if (arrOtmp[j].ToUpper() == arrFtmp[k].ToUpper())
                        {
                            bExist = true;
                            break;
                        }
                    }

                    if (bExist == false)
                    {
                        //arrtmp[j] is a deleted old term --> Update information in DB

                        //取得 keyword id
                        sql = "Select * from News_keyword where keyword = '" + arrOtmp[j] + "';";
                        cn.Exec(sql, ref dt);

                        strKeywordid = dt.Rows[0]["news_keyword_id"].ToString();


                        //是否有 ntype = U
                        sql = "Select * from News_Keyword_Class where news_keyword_id = '" + strKeywordid + "' and ntype = 'U';";
                        cn.Exec(sql, ref dt);

                        if (dt.Rows.Count > 0)
                        {
                            //有 ntype = U
                            sql = "Select * from News_Keyword_Class where news_keyword_id = '" + strKeywordid + "' and ntype = '" + strNtype + "';";
                            cn.Exec(sql, ref dt);

                            if (dt.Rows.Count > 0)
                            {
                                //有 Ntype = strNtype 也有 ntype = U
                                sql = "Delete from News_Keyword_class Where news_keyword_id = '" + strKeywordid + "' and ntype = 'U';";
                            }
                            else
                            {
                                //沒有 Ntype = strNtype 但有 ntype = U
                                sql = "Update News_keyword_class Set ntype = '" + strNtype + "' Where news_keyword_id = '" + strKeywordid + "' and ntype = 'U';";
                            }
                            cn.Exec(sql);

                            sql = "Update News_keyword_class Set UserSubtractScore = UserSubtractScore - 1 " +
                                  "Where news_keyword_id = '" + strKeywordid + "' and ntype = '" + strNtype + "' and UserSubtractScore > " + MinSubCount.ToString() + ";";
                            cn.Exec(sql);
                        }
                        else
                        {
                            //沒有 ntype = U
                            sql = "Select * from News_Keyword_Class where news_keyword_id = '" + strKeywordid + "' and ntype = '" + strNtype + "';";
                            cn.Exec(sql, ref dt);

                            if (dt.Rows.Count > 0)
                            {
                                //有 Ntype = strNtype
                                sql = "Update News_keyword_class Set UserSubtractScore = UserSubtractScore - 1 " +
                                      "Where news_keyword_id = '" + strKeywordid + "' and ntype = '" + strNtype + "' and UserSubtractScore > " + MinSubCount.ToString() + ";";
                                cn.Exec(sql);
                            }
                            else
                            {
                                //沒有 ntype = strNtype (也沒有 ntype = U)
                                #region 加入 news_keyword_class 表格中(let Ntype = U)

                                //threading!!!!!
                                ThreadWithState[] tws = new ThreadWithState[arrtmp.Length];
                                t = new Thread[arrtmp.Length];
                                for (k = 0; k < arrtmp.Length; k++)
                                {
                                    tws[k] = new ThreadWithState(arrOtmp[j], strKeywordid, arrtmp[k], itmp[k], _db_address, _db_id, _db_password, _db_dbname);
                                    t[k] = new Thread(new ThreadStart(tws[k].ThreadProc));
                                    t[k].Start();
                                }

                                for (k = 0; k < arrtmp.Length; k++)
                                {
                                    t[k].Join();
                                    t[k].Abort();
                                    t[k] = null;
                                }
                                #endregion

                                //更新 Ntype = strNtype, UserSubtractScore++
                                sql = "Update News_Keyword_Class Set Ntype = '" + strNtype + "', UserSubtractScore = UserSubtractScore - 1 " +
                                      "where news_keyword_id = '" + strKeywordid + "' and ntype = 'U';";
                                cn.Exec(sql);

                                //重新計算 FinalScore
                                //sql = "Update News_Keyword_Class Set FinalScore = google_score * (1+ UserSubtractScore/" + iUserSubtractDiv.ToString() + ") where news_keyword_id = '" + strKeywordid + "';";
                                //cn.Exec(sql);
                            }
                        }



                        sql = "Update News_keyword_class Set FinalScore = (1 + usersubtractscore / " + iUserSubtractDiv.ToString() + ") * google_score " +
                              "Where news_keyword_id = '" + strKeywordid + "' and ntype = '" + strNtype + "';";
                        cn.Exec(sql);
                    }
                }
                #endregion
            }

        }
    }
}

