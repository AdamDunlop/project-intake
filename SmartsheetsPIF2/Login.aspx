@page
@model SmartsheetsPIF2.LoginModel
@{
}


<body>

    <form id="login" runat="server">
        <div>
            Username:
            <asp:TextBox id="TextBox1" runat="server"></asp:TextBox>
            <br />
            Password:
            <asp:TextBox id="TextBox2" runat="server"></asp:TextBox>
    
            <asp:Button id="Button1" runat="server" Text="Login"></asp:TextBox>
        </div>
    </form>
</body>