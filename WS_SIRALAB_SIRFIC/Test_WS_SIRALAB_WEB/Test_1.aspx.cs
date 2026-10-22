using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Threading;
using System.Threading.Tasks;
using Test_WS_SIRALAB_WEB.WS_SIRALAB_Test;

namespace Test_WS_SIRALAB_WEB
{
    public partial class Test_1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btn_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < 2; i++)
            {
                Test();
            }
        }

        private void Test()
        {
            IwsSIRALABClient S = new IwsSIRALABClient();
            try
            {
                var t1 = DateTime.Now;
                var Result = S.Obten_Informacion(26, new DateTime(2015, 1, 1), new DateTime(2015, 2, 1), "01TAB100302/99ZZZZ99", "2.2", 4, enumTipoEvaluacion.Evaluar_LFD);
                object sync = new object();
                string error = lbl.Text;

                List<PeriodResult> pr = new List<PeriodResult>();
                Parallel.ForEach(pr, p =>
                {
                    lock (sync)
                    {
                        if (p.error)
                        {
                            error += "<br />" + p.exception.Message;
                        }
                        else
                        {
                            error += "<br /> No hubo error";
                        } 
                    }
                });

                /*foreach (var res in Result)
                {
                    if (res.error)
                    {
                        error += "<br />" + res.exception.Message;
                    }
                    else
                    {
                        error += "<br /> No hubo error";
                    }
                }*/

                var t2 = DateTime.Now;
                var t3 = t2 - t1;
                lbl.Text = error + " " + t3.Seconds.ToString();
            }
            catch (FaultException ex)
            {
                lbl.Text = ex.Message;
            }
            finally
            {
                S.Close();
            }
        }

        protected void btnPDF_Click(object sender, EventArgs e)
        {
            IwsSIRALABClient S = new IwsSIRALABClient();

            int AnalisisID = int.Parse(txt.Text.Trim() != "" ? txt.Text.Trim() : "0");
            var Result = S.Archivo_PDF_Analisis(AnalisisID);
            S.Close();

            string fileName = "Análisis_" + AnalisisID.ToString() + "_" + DateTime.Now.Ticks.ToString() + ".pdf";
            
            Response.Clear();
            Response.HeaderEncoding = System.Text.Encoding.Default;
            Response.ContentType = "application/pdf;charset=utf-8";
            Response.AddHeader("content-disposition", "inline;filename=" + fileName);

            Response.Buffer = true;
            Response.BinaryWrite(Result);

            Response.End(); 
        }
    }
}