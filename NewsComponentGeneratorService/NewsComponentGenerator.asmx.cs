using System;
using System.Data;
using System.Web;
using System.Collections;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.ComponentModel;
using NewsComponentGeneratorLib;

namespace NewsComponentGeneratorService
{
    /// <summary>
    /// NewsComponentGenerator 的摘要描述
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]
    public class NewsComponentGenerator : System.Web.Services.WebService
    {

        [WebMethod]
        public string HelloWorld()
        {
            return "Hello World";
        }

        //private ClassTermGenerator ctg = new ClassTermGenerator("140.113.124.12", "sa", "lovedddog", "se");
        //private ClassTermGenerator ctg = new ClassTermGenerator("localhost", "sa", "lovedddog", "se");
        //private ClassTermGenerator ctg = new ClassTermGenerator("10.91.21.40", "sa", "sa", "se");
        private ClassTermGenerator ctg = new ClassTermGenerator("10.91.21.40", "sa", "sa", "se_test");

        [WebMethod]
        public string GenerateClassTerm(string _NewsContent, bool _bAdvSearch, string _newsdate, string _newsid, string _COUserID)
        {
            ctg.reset();

            ctg.GenerateClassTerm(_NewsContent, _bAdvSearch, _newsdate, _newsid, _COUserID);

            string strReturn = "";
            int i;

            for (i = 0; i < ctg.qPerson.Count; i++)
            {
                strReturn = strReturn + ctg.qPerson.Dequeue() + ";";
            }
            strReturn = strReturn + "#";

            for (i = 0; i < ctg.qItem.Count; i++)
            {
                strReturn = strReturn + ctg.qItem.Dequeue() + ";";
            }
            strReturn = strReturn + "#";

            for (i = 0; i < ctg.qTrans.Count; i++)
            {
                strReturn = strReturn + ctg.qTrans.Dequeue() + ";";
            }
            strReturn = strReturn + "#";

            for (i = 0; i < ctg.qScene.Count; i++)
            {
                strReturn = strReturn + ctg.qScene.Dequeue() + ";";
            }

            return strReturn;
        }

        [WebMethod]
        public void FeedbackClassTerm(string _newsdate, string _newsid, string strOriginalTerm, string strFeedbackTerm)
        {
            ctg.FeedbackClassTerm(_newsdate, _newsid, strOriginalTerm, strFeedbackTerm);
        }
    }
}
