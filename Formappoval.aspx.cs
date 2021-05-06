using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FirestoneWebTemplate
{
    public partial class Formappoval : System.Web.UI.Page
    {
                    public static int offsetValue = 0;

        SqlConnection con = new SqlConnection(@"Data Source=SQL95-AG-PLT;Initial Catalog=PartsR3;Integrated Security=True");
        Dictionary<string, string> dictionary = new Dictionary<string, string>();

        protected void Page_Load(object sender, EventArgs e)
        {
            bind_drp_reuq();
            bind_drp_app_choices();

            btn_next_record.Enabled = false;
            GetLatestData(offsetValue);
        }

        public void bind_drp_reuq()
        {
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }

            SqlCommand command = new SqlCommand("SELECT DISTINCT [tblRequestor].[INITIALS], [tblRequestor].[NAME] FROM [tblRequestor]", con);
            SqlDataAdapter sda = new SqlDataAdapter(command);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            drp_reuq.DataSource = dt;
            drp_reuq.DataTextField = "NAME";
            drp_reuq.DataValueField = "INITIALS";
            drp_reuq.DataBind();
        }

        public void bind_drp_app_choices()
        {
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }

            SqlCommand command = new SqlCommand("SELECT tblRequestor.INITIALS, tblRequestor.NAME, tblRequestor.ApprovalLevel FROM tblRequestor ORDER BY tblRequestor.INITIALS;", con);
            SqlDataAdapter sda = new SqlDataAdapter(command);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            drp_app_choices.DataSource = dt;
            drp_app_choices.DataTextField = "NAME";
            drp_app_choices.DataValueField = "ApprovalLevel";
            drp_app_choices.DataBind();
        }

        public void GetLatestData(int offset)
        {
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }

            SqlCommand command = new SqlCommand("SELECT *  FROM [tblParts] order by id desc OFFSET " + offset + "  ROWS FETCH NEXT 1 ROWS ONLY", con);
            SqlDataAdapter sda = new SqlDataAdapter(command);
            DataTable dt = new DataTable();
            sda.Fill(dt);

            foreach (DataRow row in dt.Rows)
            {
                if (row["ORI_DATE"] != DBNull.Value)
                {
                    txt_ori_date.Text = row["ORI_DATE"].ToString();
                }
                else
                {
                    txt_ori_date.Text = "";
                }

                if (row["REQUESTOR"] != DBNull.Value)
                {
                    drp_reuq.Text = row["REQUESTOR"].ToString().Trim();
                }
                else
                {
                    drp_reuq.Text = null;
                }

                if (row["BinLocation"] != DBNull.Value)
                {
                    txt_bin_location.Text = row["BinLocation"].ToString();
                }
                else
                {
                    txt_bin_location.Text = "";
                }

                if (row["SAFETY"] != DBNull.Value)
                {
                    txt_priority.Text = row["SAFETY"].ToString();
                }
                else
                {
                    txt_priority.Text = "";
                }


                if (row["ID"] != DBNull.Value)
                {
                    txt_id.Text = row["ID"].ToString();
                }
                else
                {
                    txt_id.Text = "";
                }

                if (row["MIN"] != DBNull.Value)
                {
                    txt_min.Text = row["MIN"].ToString();
                }
                else
                {
                    txt_min.Text = "";
                }

                if (row["MAX"] != DBNull.Value)
                {
                    txt_max.Text = row["MAX"].ToString();
                }
                else
                {
                    txt_max.Text = "";
                }

                if (row["COM_NUMBER"] != DBNull.Value)
                {
                    txt_common.Text = row["COM_NUMBER"].ToString();
                }
                else
                {
                    txt_common.Text = "";
                }

                if (row["Lead_Time"] != DBNull.Value)
                {
                    txt_lead_time.Text = row["Lead_Time"].ToString();
                }
                else
                {
                    txt_lead_time.Text = "";
                }

                if (row["PRICE"] != DBNull.Value)
                {
                    txt_price.Text = row["PRICE"].ToString();
                }
                else
                {
                    txt_price.Text = "";
                }

                if (row["PART_DESC"] != DBNull.Value)
                {
                    txt_part_desc.Text = row["PART_DESC"].ToString();
                }
                else
                {
                    txt_part_desc.Text = "";
                }

                if (row["PART_DESC1"] != DBNull.Value)
                {
                    txt_part_desc1.Text = row["PART_DESC1"].ToString();
                }
                else
                {
                    txt_part_desc1.Text = "";
                }

                if (row["PART_DESC2"] != DBNull.Value)
                {
                    txt_part_desc2.Text = row["PART_DESC2"].ToString();
                }
                else
                {
                    txt_part_desc2.Text = "";
                }

                if (row["PART_DESC3"] != DBNull.Value)
                {
                    txt_part_desc3.Text = row["PART_DESC3"].ToString();
                }
                else
                {
                    txt_part_desc3.Text = "";
                }

                if (row["PART_DESC4"] != DBNull.Value)
                {
                    txt_part_desc4.Text = row["PART_DESC4"].ToString();
                }
                else
                {
                    txt_part_desc4.Text = "";
                }


                if (row["WHY_STOCK"] != DBNull.Value)
                {
                    txt_why_stock.Text = row["WHY_STOCK"].ToString();
                }
                else
                {
                    txt_why_stock.Text = "";
                }

                if (row["ApprovalLevel"] != DBNull.Value)
                {
                    txt_approval_level.Text = row["ApprovalLevel"].ToString();
                }
                else
                {
                    txt_approval_level.Text = "";
                }

                if (row["SENT_TO_DATE"] != DBNull.Value)
                {
                    txt_sent_to_date.Text = row["SENT_TO_DATE"].ToString();
                }
                else
                {
                    txt_sent_to_date.Text = "";
                }

                if (row["ApprovalTracking"] != DBNull.Value)
                {
                    txt_approval_tracking.Text = row["ApprovalTracking"].ToString();
                }
                else
                {
                    txt_approval_tracking.Text = "";
                }

                if (row["ApprovalCue"] != DBNull.Value)
                {
                    txt_cue.Text = row["ApprovalCue"].ToString();
                }
                else
                {
                    txt_cue.Text = "";
                }

                if (row["EnvApp"] != DBNull.Value)
                {
                    ch_env_app.Checked = Convert.ToBoolean(row["EnvApp"].ToString());
                }
                else
                {
                    ch_env_app.Checked = false;
                }

                if (row["COMMENTS"] != DBNull.Value)
                {
                    txt_comments.Text = row["COMMENTS"].ToString();
                }
                else
                {
                    txt_comments.Text = "";
                }
            }
        }

        protected void btn_previous_record_Click(object sender, EventArgs e)
        {
            offsetValue = offsetValue + 1;

            if (offsetValue > 0)
            {
                btn_next_record.Enabled = true;
            }

            GetLatestData(offsetValue);
        }

        protected void btn_next_record_Click(object sender, EventArgs e)
        {
            offsetValue = offsetValue - 1;

            if (offsetValue == 0)
            {
                btn_next_record.Enabled = false;
            }
            else
            {
                btn_next_record.Enabled = true;
            }

            GetLatestData(offsetValue);
        }

    }
    
}