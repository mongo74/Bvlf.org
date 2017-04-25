using System;
using System.Web.UI;

namespace bvlf_v2
{
    public partial class index : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void cmdGoGetit_Click(object sender, EventArgs e)
        {
            lblInfo.Text = "hoppa";
        }
    }
}