using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FirestoneWebTemplate
{
    public partial class Form_request : System.Web.UI.Page
    {
        string connectionString = @"Data Source=SQL95-AG-PLT;Initial Catalog=PartsR3;Integrated Security=True";

        SqlConnection con = new SqlConnection(@"Data Source=SQL95-AG-PLT;Initial Catalog=PartsR3;Integrated Security=True");
        Dictionary<string, string> dictionary = new Dictionary<string, string>();

        protected void Page_Load(object sender, EventArgs e)
        {
            dictionary.Add("YES", "YES");
            dictionary.Add("NO", "NO");

            bind_drp_reuq();
            bind_drp_dept();
            bind_drp_action();
            bind_drp_prev();
            bind_drp_turn_in();
            bind_drp_sap();
            bind_drp_year();
            bind_drp_next_app();
            bind_drp_unit();
            bind_drp_category();
            bind_drp_stock_at_other_plan();
            bind_drp_priority();
            bind_drp_critical();
            bind_drp_repairable();
            bind_drp_replace_other_part();
            bind_drp_used();
        }

        protected void btn_submit_Click(object sender, EventArgs e)
        {

            SqlConnection con = null;
            try
            {
                con = new SqlConnection(connectionString);

                string sql = " INSERT INTO tblParts (ID,REQUESTOR,REQUESTOR2,ORI_DATE,ApprovalCue, " +
                  " Tax_ID,ApprovalTracking,UNIT,Category,Field2,Field2Info,Field3,Field3Info, " +
                  " Field4,Field4Info,Field5,Field5Info,Field6,Field6Info,Field7,Field7Info,Field8,Field8Info, " +
                  " Field9,Field9Info,Field10,Field10Info,Field11,Field11Info,Field12,Field12Info,MIN,MAX, " +
                  " REPAIRABLE,SAFETY,EST_USAGE,UsagePer,AMT_IN_USE,CATALOG_SECT,PREV_PUR," +
                  " PRICE,PO,PO_DATE, " +
                  " SEARCHED,PART_DESC,PART_DESC1,PART_DESC2,PART_DESC3,PART_DESC4,WHY_STOCK,MAN__1_,MAN_P_N__1, " +
                  " MAN__2_,MAN_P_N__2,USED,Used_area,COM_NUMBER,USED2,Used_area2,USED3,Used_area3, " +
                  " USED4,Used_area4,OrderQty,ForStock,ForUse,QtyOnHold,EnvApp,BinLocation, " +
                  " GroupingNumber,Dept,Phone,Updated,Drawing,ActionToBeTaken,RFQ,TurnInParts,HowManyParts, " +
                  " StockedOtherPlants,MaterialGroup,Critical,ValuationClass,ReplaceOtherPart, " +
                  " COMMON_NO,Buyer,Vendor,Lead_Time,Locked,[WHERE]) VALUES (@ID, @REQUESTOR,@REQUESTOR2,@ORI_DATE, " +
                  " @ApprovalCue, @Tax_ID,@ApprovalTracking,@UNIT,@Category,@Field2, " +
                  " @Field2Info,@Field3, @Field3Info,@Field4,@Field4Info,@Field5,@Field5Info,@Field6, " +
                  " @Field6Info, @Field7, @Field7Info, @Field8,@Field8Info,@Field9, @Field9Info,@Field10," +
                  " @Field10Info, @Field11, @Field11Info, @Field12,@Field12Info, @MIN,@MAX, @REPAIRABLE, " +
                  " @SAFETY,@EST_USAGE, @UsagePer,@AMT_IN_USE,@CATALOG_SECT,@PREV_PUR," +
                  " @PRICE," +
                  " @PO, @PO_DATE, @SEARCHED,@PART_DESC,@PART_DESC1,@PART_DESC2, @PART_DESC3,@PART_DESC4," +
                  " @WHY_STOCK, @MAN__1_, @MAN_P_N__1, @MAN__2_, @MAN_P_N__2, @USED, @Used_area, " +
                  " @COM_NUMBER, @USED2, @Used_area2, @USED3, @Used_area3,@USED4, @Used_area4,@OrderQty,@ForStock, " +
                  " @ForUse, @QtyOnHold, @EnvApp, @BinLocation,@GroupingNumber,@Dept, " +
                  " @Phone, @Updated, @Drawing, @ActionToBeTaken, @RFQ, @TurnInParts, @HowManyParts, " +
                  " @StockedOtherPlants, @MaterialGroup, @Critical, @ValuationClass, @ReplaceOtherPart, " +
                  " @COMMON_NO, @Buyer, @Vendor, @Lead_Time,@Locked,@WHERE) ";

                SqlCommand cmd = new SqlCommand(sql, con);
                SqlParameter[] param = new SqlParameter[89];
                param[0] = new SqlParameter("@ID", txt_id.Text);
                param[1] = new SqlParameter("@REQUESTOR", drp_reuq.Text);
                param[2] = new SqlParameter("@REQUESTOR2", txt_req.Text);
                param[3] = new SqlParameter("@ORI_DATE", txt_date.Text);
                //param[3] = new SqlParameter("@SENT_TO_DATE", DateTime.Now); //no ui field or linked with which param
                param[4] = new SqlParameter("@ApprovalCue", txt_approval_cue.Text);
                param[5] = new SqlParameter("@Tax_ID", txt_tax_id.Text);
                //param[5] = new SqlParameter("@ApprovalLevel", drp_next_app.Text); //no ui field or linked with which param
                param[6] = new SqlParameter("@ApprovalTracking", txt_approval_tracking.Text);
                param[7] = new SqlParameter("@UNIT", drp_unit.Text);
                param[8] = new SqlParameter("@Category", drp_category.Text);
                param[9] = new SqlParameter("@Field2", txt_field2.Text);
                param[10] = new SqlParameter("@Field2Info", txt_field2_info.Text);
                param[11] = new SqlParameter("@Field3", txt_field3.Text);
                param[12] = new SqlParameter("@Field3Info", txt_field3_info.Text);
                param[13] = new SqlParameter("@Field4", txt_field4.Text);
                param[14] = new SqlParameter("@Field4Info", txt_field4_info.Text);
                param[15] = new SqlParameter("@Field5", txt_field5.Text);
                param[16] = new SqlParameter("@Field5Info", txt_field5_info.Text);
                param[17] = new SqlParameter("@Field6", txt_field6.Text);
                param[18] = new SqlParameter("@Field6Info", txt_field6_info.Text);
                param[19] = new SqlParameter("@Field7", txt_field7.Text);
                param[20] = new SqlParameter("@Field7Info", txt_field7_info.Text);
                param[21] = new SqlParameter("@Field8", txt_field8.Text);
                param[22] = new SqlParameter("@Field8Info", txt_field8_info.Text);
                param[23] = new SqlParameter("@Field9", txt_field9.Text);
                param[24] = new SqlParameter("@Field9Info", txt_field9_info.Text);
                param[25] = new SqlParameter("@Field10", txt_field10.Text);
                param[26] = new SqlParameter("@Field10Info", txt_field10_info.Text);
                param[27] = new SqlParameter("@Field11", txt_field11.Text);
                param[28] = new SqlParameter("@Field11Info", txt_field11_info.Text);
                param[29] = new SqlParameter("@Field12", txt_field12.Text);
                param[30] = new SqlParameter("@Field12Info", txt_field12_info.Text);
                param[31] = new SqlParameter("@MIN", txt_min.Text);
                param[32] = new SqlParameter("@MAX", txt_max.Text);
                param[33] = new SqlParameter("@REPAIRABLE", drp_repairable.Text);
                param[34] = new SqlParameter("@SAFETY", drp_priority.Text);
                param[35] = new SqlParameter("@EST_USAGE", txt_est.Text);
                param[36] = new SqlParameter("@UsagePer", drp_year.Text);
                param[37] = new SqlParameter("@AMT_IN_USE", txt_amount.Text);
                param[38] = new SqlParameter("@CATALOG_SECT", txt_catalog_sect.Text);
                param[39] = new SqlParameter("@PREV_PUR", drp_prev.Text);
                param[40] = new SqlParameter("@PRICE", txt_price.Text);
                param[41] = new SqlParameter("@PO", txt_po.Text);
                param[42] = new SqlParameter("@PO_DATE", txt_po_date.Text);
                param[43] = new SqlParameter("@SEARCHED", 1); //no ui field  
                param[44] = new SqlParameter("@PART_DESC", txt_part_desc.Text);
                param[45] = new SqlParameter("@PART_DESC1", txt_part_desc1.Text);
                param[46] = new SqlParameter("@PART_DESC2", txt_part_desc2.Text);
                param[47] = new SqlParameter("@PART_DESC3", txt_part_desc3.Text);
                param[48] = new SqlParameter("@PART_DESC4", txt_part_desc4.Text);
                param[49] = new SqlParameter("@WHY_STOCK", txt_why_stock.Text);
                param[50] = new SqlParameter("@MAN__1_", txt_man1.Text);
                param[51] = new SqlParameter("@MAN_P_N__1", txt_man_p_n_1.Text);
                param[52] = new SqlParameter("@MAN__2_", txt_man2.Text);
                param[53] = new SqlParameter("@MAN_P_N__2", txt_man_p_n_2.Text);
                param[54] = new SqlParameter("@USED", drp_used.Text);
                param[55] = new SqlParameter("@Used_area", txt_used_area.Text);
                //param[56] = new SqlParameter("@DTR_NUMBER", 123);//no ui field  always null
                param[56] = new SqlParameter("@COM_NUMBER", txt_common_number.Text);
                param[57] = new SqlParameter("@USED2", drp_used2.Text);
                param[58] = new SqlParameter("@Used_area2", txt_used_area2.Text);
                param[59] = new SqlParameter("@USED3", drp_used3.Text);
                param[60] = new SqlParameter("@Used_area3", txt_used_area3.Text);
                param[61] = new SqlParameter("@USED4", drp_used4.Text);
                param[62] = new SqlParameter("@Used_area4", txt_used_area4.Text);
                param[63] = new SqlParameter("@OrderQty", txt_order_qty.Text);
                param[64] = new SqlParameter("@ForStock", ch_stock_checked.Checked);
                param[65] = new SqlParameter("@ForUse", ch_user_checked.Checked);
                param[66] = new SqlParameter("@QtyOnHold", txt_stock.Text);
                //param[68] = new SqlParameter("@ACCOUNT", 1); //no ui field  always null
                //param[69] = new SqlParameter("@COMMENTS", 1); //no ui field or linked with which param
                param[67] = new SqlParameter("@EnvApp", ch_env_app.Checked);
                param[68] = new SqlParameter("@BinLocation", txt_bin_location.Text);
                param[69] = new SqlParameter("@GroupingNumber", txt_grup_number.Text);
                param[70] = new SqlParameter("@Dept", drp_dept.Text);
                param[71] = new SqlParameter("@Phone", txt_phone_number.Text);
                param[72] = new SqlParameter("@Updated", txt_update.Text);
                param[73] = new SqlParameter("@Drawing", txt_drawing.Text);
                param[74] = new SqlParameter("@ActionToBeTaken", drp_action.Text);
                param[75] = new SqlParameter("@RFQ", txt_rfq.Text);
                param[76] = new SqlParameter("@TurnInParts", drp_turn_in.Text);
                param[77] = new SqlParameter("@HowManyParts", txt_how_many.Text);
                param[78] = new SqlParameter("@StockedOtherPlants", drp_stock_at_other_plan.Text);
                //param[82] = new SqlParameter("@GroupNo", 1);//no ui field always null
                param[79] = new SqlParameter("@MaterialGroup", txt_material.Text);
                param[80] = new SqlParameter("@Critical", drp_critical.Text);
                param[81] = new SqlParameter("@ValuationClass", txt_valuation.Text);
                param[82] = new SqlParameter("@ReplaceOtherPart", drp_replace_other_part.Text);
                param[83] = new SqlParameter("@COMMON_NO", txt_common_number.Text);
                param[84] = new SqlParameter("@Buyer", txt_buyer.Text);
                param[85] = new SqlParameter("@Vendor", txt_vender.Text);
                param[86] = new SqlParameter("@Lead_Time", txt_lead_time.Text);
                param[87] = new SqlParameter("@Locked", 1); //no ui field or linked with which param hardcoded as 1 as its mandatory
                param[88] = new SqlParameter("@WHERE", txt_from_where.Text);

                for (int i = 0; i < param.Length; i++)
                {
                    Console.WriteLine("index: " + i);
                    Console.WriteLine("Value: " + param[i]);

                    cmd.Parameters.Add(param[i]);
                }

                con.Open();
                int status = cmd.ExecuteNonQuery();

                savedSucess.BorderColor = Color.Green;
                savedSucess.ForeColor = Color.Green;
                savedSucess.Text = "Saved Successfully";
            }
            catch (Exception ex)
            {
                savedSucess.BorderColor = Color.Red;
                savedSucess.ForeColor = Color.Red;
                savedSucess.Text = ex.Message;
                Console.WriteLine("Something Went Wrong" + ex);
            }
            finally
            {
                con.Close();
            }



        }

        public void bind_drp_reuq()
        {
            con.Open();
            SqlCommand command = new SqlCommand("Select * from tblRequestor", con);
            SqlDataAdapter sda = new SqlDataAdapter(command);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            drp_reuq.DataSource = dt;
            drp_reuq.DataTextField = "NAME";
            drp_reuq.DataValueField = "INITIALS";
            drp_reuq.DataBind();
        }

        public void bind_drp_dept()
        {
            SqlCommand command = new SqlCommand("SELECT tblDEPT.[DEPT#], tblDEPT.NAME FROM tblDEPT ORDER BY tblDEPT.[DEPT#]", con);
            SqlDataAdapter sda = new SqlDataAdapter(command);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            drp_dept.DataSource = dt;
            drp_dept.DataTextField = "NAME";
            drp_dept.DataValueField = "DEPT#";
            drp_dept.DataBind();
        }

        public void bind_drp_action()
        {
            var dictionary = new Dictionary<string,
              string>();
            dictionary.Add("ADD", "ADD");
            dictionary.Add("DELETE", "DELETE");
            dictionary.Add("MODIFY MAX/MIN LEVEL", "MODIFY MAX/MIN LEVEL");

            drp_action.DataSource = dictionary;
            drp_action.DataTextField = "Value";
            drp_action.DataValueField = "Key";
            drp_action.DataBind();
        }

        public void bind_drp_prev()
        {
            var dictionary = new Dictionary<string,
              string>();
            dictionary.Add("YES", "YES");
            dictionary.Add("NO", "NO");

            drp_prev.DataSource = dictionary;
            drp_prev.DataTextField = "Value";
            drp_prev.DataValueField = "Key";
            drp_prev.DataBind();
        }

        public void bind_drp_turn_in()
        {
            drp_turn_in.DataSource = dictionary;
            drp_turn_in.DataTextField = "Value";
            drp_turn_in.DataValueField = "Key";
            drp_turn_in.DataBind();
        }

        public void bind_drp_sap()
        {
            drp_sap.DataSource = dictionary;
            drp_sap.DataTextField = "Value";
            drp_sap.DataValueField = "Key";
            drp_sap.DataBind();
        }

        public void bind_drp_year()
        {
            var dictionary = new Dictionary<string,
              string>();
            dictionary.Add("MONTH", "MONTH");
            dictionary.Add("YEAR", "YEAR");

            drp_year.DataSource = dictionary;
            drp_year.DataTextField = "Value";
            drp_year.DataValueField = "Key";
            drp_year.DataBind();
        }

        public void bind_drp_next_app()
        {
            SqlCommand command = new SqlCommand("SELECT tblRequestor.INITIALS, tblRequestor.NAME, tblRequestor.ApprovalLevel FROM tblRequestor WHERE (((tblRequestor.ApprovalLevel)=2));", con);
            SqlDataAdapter sda = new SqlDataAdapter(command);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            drp_next_app.DataSource = dt;
            drp_next_app.DataTextField = "NAME";
            drp_next_app.DataValueField = "INITIALS";
            drp_next_app.DataBind();
        }

        public void bind_drp_unit()
        {
            SqlCommand command = new SqlCommand("SELECT DISTINCT [tblUnits].[ID], [tblUnits].[DESCRIPTION] FROM [tblUnits]", con);
            SqlDataAdapter sda = new SqlDataAdapter(command);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            drp_unit.DataSource = dt;
            drp_unit.DataTextField = "DESCRIPTION";
            drp_unit.DataValueField = "ID";
            drp_unit.DataBind();
        }

        public void bind_drp_category()
        {
            SqlCommand command = new SqlCommand("SELECT tblCategory.Category, tblCategory.Field2, tblCategory.Field3, tblCategory.Field4, tblCategory.Field5, tblCategory.Field6, tblCategory.Field7, tblCategory.Field8, tblCategory.Field9, tblCategory.Field10, tblCategory.Field11, tblCategory.Field12 FROM tblCategory ORDER BY tblCategory.Category;", con);
            SqlDataAdapter sda = new SqlDataAdapter(command);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            drp_category.DataSource = dt;
            drp_category.DataTextField = "Category";
            drp_category.DataValueField = "Category";
            drp_category.DataBind();
        }

        public void bind_drp_stock_at_other_plan()
        {
            SqlCommand command = new SqlCommand("SELECT tblPlant.ID, tblPlant.PLANT FROM tblPlant ORDER BY tblPlant.PLANT;", con);
            SqlDataAdapter sda = new SqlDataAdapter(command);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            drp_stock_at_other_plan.DataSource = dt;
            drp_stock_at_other_plan.DataTextField = "PLANT";
            drp_stock_at_other_plan.DataValueField = "ID";
            drp_stock_at_other_plan.DataBind();
        }

        public void bind_drp_priority()
        {
            SqlCommand command = new SqlCommand("SELECT DISTINCT [tblSafetyCodes].[Code], [tblSafetyCodes].[Description] FROM [tblSafetyCodes];", con);
            SqlDataAdapter sda = new SqlDataAdapter(command);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            drp_priority.DataSource = dt;
            drp_priority.DataTextField = "Description";
            drp_priority.DataValueField = "Code";
            drp_priority.DataBind();
        }

        public void bind_drp_critical()
        {
            drp_critical.DataSource = dictionary;
            drp_critical.DataTextField = "Value";
            drp_critical.DataValueField = "Key";
            drp_critical.DataBind();
        }

        public void bind_drp_repairable()
        {
            drp_repairable.DataSource = dictionary;
            drp_repairable.DataTextField = "Value";
            drp_repairable.DataValueField = "Key";
            drp_repairable.DataBind();
        }

        public void bind_drp_replace_other_part()
        {
            drp_replace_other_part.DataSource = dictionary;
            drp_replace_other_part.DataTextField = "Value";
            drp_replace_other_part.DataValueField = "Key";
            drp_replace_other_part.DataBind();
        }

        public void bind_drp_used()
        {
            SqlCommand command = new SqlCommand("SELECT DISTINCT [tblEquipment].[EQUIPMENT] FROM [tblEquipment]", con);
            SqlDataAdapter sda = new SqlDataAdapter(command);
            DataTable dt = new DataTable();
            sda.Fill(dt);

            drp_used.DataSource = dt;
            drp_used.DataTextField = "EQUIPMENT";
            drp_used.DataValueField = "EQUIPMENT";
            drp_used.DataBind();

            drp_used2.DataSource = dt;
            drp_used2.DataTextField = "EQUIPMENT";
            drp_used2.DataValueField = "EQUIPMENT";
            drp_used2.DataBind();

            drp_used3.DataSource = dt;
            drp_used3.DataTextField = "EQUIPMENT";
            drp_used3.DataValueField = "EQUIPMENT";
            drp_used3.DataBind();

            drp_used4.DataSource = dt;
            drp_used4.DataTextField = "EQUIPMENT";
            drp_used4.DataValueField = "EQUIPMENT";
            drp_used4.DataBind();
        }
    

    }

}