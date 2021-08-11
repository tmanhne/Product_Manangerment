<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="WorkingDatabase.Index" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>

            <asp:Label ID="Label1" runat="server" Text="Category name:"></asp:Label>
            <asp:DropDownList ID="DblCate" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DblCate_SelectedIndexChanged">
            </asp:DropDownList>
            <br />
            <br />
            <asp:GridView ID="dvTable" runat="server" Height="217px" Width="435px" DataKeyNames="CategoryID" EnableModelValidation="True" AutoGenerateColumns="False" OnRowDeleting="dvTable_RowDeleting" OnRowDataBound="dvTable_RowDataBound">
               
                 <Columns>                    
                     <asp:BoundField HeaderText="Product ID" DataField="ProductID" />
                     <asp:BoundField HeaderText="Product Name" DataField="ProductName" />
                     <asp:BoundField HeaderText="Price" DataField="UnitPrice" />
                     <asp:HyperLinkField HeaderText="Update" Text="edit" DataNavigateUrlFields="ProductID,ProductName" DataNavigateUrlFormatString="update.aspx?ProductID={0}&amp;name={1}" />
                     <asp:HyperLinkField DataNavigateUrlFields="ProductID" DataNavigateUrlFormatString="delete.aspx?id={0}" HeaderText="Delete" Text="delete"/>
                    
                     <asp:CommandField ButtonType="Button" ShowDeleteButton="True" />
                </Columns>
            </asp:GridView>
  </script>
    </form>
</body>
</html>
