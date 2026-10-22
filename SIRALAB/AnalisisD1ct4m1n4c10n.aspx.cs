using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SIRALAB
{
    public partial class AnalisisD1ct4m1n4c10n : System.Web.UI.Page
    {

        public int Id1
        {
            get { return h1.Value.Trim() == "" ? 0 : int.Parse(h1.Value); }
            set { h1.Value = value.ToString(); }
        }

        public int Id2
        {
            get { return h2.Value.Trim() == "" ? 0 : int.Parse(h2.Value); }
            set { h2.Value = value.ToString(); }
        }

        public int Id3
        {
            get { return h3.Value.Trim() == "" ? 0 : int.Parse(h3.Value); }
            set { h3.Value = value.ToString(); }
        }

      


        protected void Page_Load(object sender, EventArgs e)
        {


        }

        protected void lnkbtnPas_Click(object sender, EventArgs e)
        {
            if (txtPas.Text == "S0l1c17ud_S1r414b_2014")
            {
                lblC1.Text = ConfigurationManager.ConnectionStrings["Siralab_Entities"].ToString();
                lblC2.Text = ConfigurationManager.ConnectionStrings["CNA_SIRFICEntities"].ToString();
            }
        }

        protected void lnkbtn1_Click(object sender, EventArgs e)
        {
            Id1 = Id1 + 1;
        }

        protected void lnkbtn2_Click(object sender, EventArgs e)
        {
            Id2 = Id2 + 1;
        }

        protected void lnkbtn3_Click(object sender, EventArgs e)
        {
            Id3 = Id3 + 1;
        }

        protected void lnkbtn4_Click(object sender, EventArgs e)
        {
            if (Id1 == 3 && Id2 == 1 && Id3 == 2)
            {
                pnl.Visible = true;
            }
        }
    }
}