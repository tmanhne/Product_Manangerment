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
    public partial class Index : System.Web.UI.Page
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
        public static DataTable GetAllCate()
        {
            string sql = "SELECT * FROM dbo.Categories";
            //SELECT * FROM dbo.Categories
            return GetDataBySql(sql);
        }
        public static DataTable GetProductsByCatId(int catId)
        {           
            string sql = "SELECT * FROM dbo.Products WHERE CategoryID = " + catId;
            return GetDataBySql(sql);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                // load data from GetAllCate() vao ddlCate(dropDownList)
                DblCate.DataSource = GetAllCate();

                DblCate.DataTextField = "CategoryName";
                DblCate.DataValueField = "CategoryID";
                DblCate.DataBind();

                // load data from getProductByCatId(catId) vao dataGriftView
                int catID = Convert.ToInt32(DblCate.SelectedValue);
                dvTable.DataSource = GetProductsByCatId(catID);
                dvTable.DataBind();
            }
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

        protected void DblCate_SelectedIndexChanged(object sender, EventArgs e)
        {
            int catID = Convert.ToInt32(DblCate.SelectedValue);
            dvTable.DataSource = GetProductsByCatId(catID);
            dvTable.DataBind();
        }
        SqlConnection conn = new SqlConnection(@"Data Source=localhost; Initial Catalog= Northwind; Integrated Security=SSPI");
        public void deleOrder(int pID)
        {
            string sql = "DELETE FROM dbo.[Order Details] WHERE ProductID = " + pID;
            conn.Open();
            SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.ExecuteNonQuery();
            conn.Close();
            conn.Dispose();
            
        }
        public void Delete(int pID)
        {
            deleOrder(pID);
            string sql = "DELETE FROM dbo.Products WHERE ProductID = " + pID;
            conn.Open();
            SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.ExecuteNonQuery();
            conn.Close();
            conn.Dispose();
            
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
        protected void pageLoadc()
        {
            DblCate.DataSource = GetAllCate();

            DblCate.DataTextField = "CategoryName";
            DblCate.DataValueField = "CategoryID";
            DblCate.DataBind();

            // load data from getProductByCatId(catId) vao dataGriftView
            int catID = Convert.ToInt32(DblCate.SelectedValue);
            dvTable.DataSource = GetProductsByCatId(catID);
            dvTable.DataBind();
        }
        protected void dvTable_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int pID = Convert.ToInt32(dvTable.DataKeys[e.RowIndex].Values[0].ToString());
            DeleteOrder(pID);
            DeleteProduct(pID);
            pageLoadc();
            
        }

        protected void dvTable_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Button buttonCommandField = e.Row.Cells[0].Controls[0] as Button;
                buttonCommandField.Attributes["onClick"] =
                       string.Format("return confirm('Are you want delete ')");
            }
        }
    }
}