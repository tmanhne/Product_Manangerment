using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WorkingDatabase
{
    public partial class update : System.Web.UI.Page
    {
        public static SqlConnection GetConnection()
        {
            string strCon = ConfigurationManager.ConnectionStrings["Database_Web"].ToString();
            return new SqlConnection(strCon);
        }
        public static DataTable GetDataBySql(string sql)
        {
            SqlCommand cmd = new SqlCommand(sql, GetConnection());
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;
            DataSet ds = new DataSet();
            da.Fill(ds);
            return ds.Tables[0];
        }
        public static int ExecuteSQL(string sql, params SqlParameter[] sqlParameters)
        {
            SqlCommand cmd = new SqlCommand(sql, GetConnection());
            //prepare sql
            cmd.Parameters.AddRange(sqlParameters);
            //Open connect
            cmd.Connection.Open();
            int result = cmd.ExecuteNonQuery();
            cmd.Connection.Close();
            return result;
        }
        public static DataTable GetProductsById(int pId)
        {
            string sql = "SELECT * FROM dbo.Products WHERE CategoryID = " + pId;
            return GetDataBySql(sql);
        }
        public static DataTable GetAllCate()
        {
            string sql = "SELECT * from Categories";
            return GetDataBySql(sql);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                int pId = Convert.ToInt32(Request["ProductID"].ToString());
               
                double uPrice = Convert.ToDouble(GetProductsById(pId).Rows[0]["UnitPrice"].ToString());
                string pName = GetProductsById(pId).Rows[0]["ProductName"].ToString();
                bool disContinued = Convert.ToBoolean(GetProductsById(pId).Rows[0]["Discontinued"].ToString());

                txtpId.Text = pId.ToString();
                txtpName.Text = pName;
                txtPrice.Text = uPrice.ToString();
                ckbContinued.Checked = disContinued;

                //load to ddl
                ddlCate.DataSource = GetAllCate();
                ddlCate.DataTextField = "CategoryName";
                ddlCate.DataValueField = "CategoryID";
                ddlCate.DataBind();

                //select cate
                ddlCate.SelectedValue = GetProductsById(pId).Rows[0]["CategoryID"].ToString();
            }
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(txtpId.Text);
            string name = txtpName.Text;
            double price = Convert.ToDouble(txtPrice.Text);
            int cId = Convert.ToInt32(ddlCate.SelectedValue);
            bool disC = ckbContinued.Checked == true ? true : false;
            //check validate

            //update
            ArrayList list = new ArrayList() { name, price, cId, disC, id };
            if(UpdateProduct(list) > 0)
            {
                Response.Redirect("Index.aspx");
            }
        }

        public static int UpdateProduct(ArrayList list)
        {
            string sql = "UPDATE Products SET ProductName=@name, CategoryID=@cid, UnitPrice=@price, " +
                "Discontinued=@dis WHERE ProductID=@pid";
            SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter("@name", SqlDbType.NVarChar),  
                new SqlParameter("@price", SqlDbType.Money),
                new SqlParameter("@cid", SqlDbType.Int),
                new SqlParameter("@dis", SqlDbType.Bit),
                new SqlParameter("@pid", SqlDbType.Int)
            };
            //hoat dong gan gia tri cho cac phan tu 'param'
            for (int i = 0; i < list.Count; i++)
            {
                param[i].Value = list[i];
            }
            return ExecuteSQL(sql, param);
        }
    }
}