using System;
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
    public partial class delete : System.Web.UI.Page
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
                int pId = Convert.ToInt32(Request["id"].ToString());

                double uPrice = Convert.ToDouble(GetProductsById(pId).Rows[0]["UnitPrice"].ToString());
                string pName = GetProductsById(pId).Rows[0]["ProductName"].ToString();
          
                txtpId.Text = pId.ToString();
                txtpName.Text = pName;
                txtPrice.Text = uPrice.ToString();
                txtpId.Enabled = false;
                txtpName.Enabled = false;
                txtPrice.Enabled = false;
                ddlCate.Enabled = false;
                //load to ddl
                ddlCate.DataSource = GetAllCate();
                ddlCate.DataTextField = "CategoryName";
                ddlCate.DataValueField = "CategoryID";
                ddlCate.DataBind();
            }
        }
        public static int DeleteOrder(int pId)
        {
             string sql = "DELETE FROM dbo.[Order Details] WHERE ProductID = @pId";
                SqlParameter param = new SqlParameter("@pId", SqlDbType.Int);
                param.Value = pId;

                /*hoat dong gan gia tri cho cac phan tu 'param'
                for (int i = 0; i < arrayList.Count; i++)
                {
                    param[i].Value = arrayList[i];
                }*/
               
            
            return ExecuteSQL(sql, param);
        }
        public static int DeleteProduct(int pId)
        {
           
                string sql = "DELETE FROM dbo.Products WHERE ProductID = @pId";
                SqlParameter param = new SqlParameter("@pId", SqlDbType.Int);
                param.Value = pId;

                /*hoat dong gan gia tri cho cac phan tu 'param'
                for (int i = 0; i < arrayList.Count; i++)
                {
                    param[i].Value = arrayList[i];
                }*/
               return ExecuteSQL(sql, param);
            
             
        }
        protected void btnBack_Click(object sender, EventArgs e)
        {
             Response.Redirect("Index.aspx");
                
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(Request["id"].ToString());
            if (DeleteOrder(id) > 0)
            {
                if (DeleteProduct(id) > 0)
                {
                    Response.Redirect("Index.aspx");
                }
            }
        }
    }
}