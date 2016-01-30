<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<%@ Control Inherits="DotNetNuke.Modules.Forum.ACP.FilterWord" CodeBehind="ACP_FilterWord.ascx.vb" language="vb" AutoEventWireup="false" Explicit="true" %>
<%@ Register TagPrefix="dnnforum" Namespace="DotNetNuke.Modules.Forum.WebControls" Assembly="DotNetNuke.Modules.Forum" %>
<%@ Register Assembly="DotNetNuke.Web.Deprecated" Namespace="DotNetNuke.Web.UI.WebControls" TagPrefix="dnnweb" %>
<div class="dnnForm acpFilterWord dnnClear">
	<h2 class="dnnFormSectionHead"><asp:Label id="lblTitle" runat="server" resourcekey="lblTitle" EnableViewState="false" /></h2>
	<fieldset>
		<div class="dnnFormItem">
			<dnn:Label runat="server" ID="lblNewWord" suffix=":" ControlName="txtNewBadWord" />
			<asp:TextBox ID="txtNewBadWord" runat="server" />
		</div>
		<div class="dnnFormItem">
			<dnn:Label runat="server" ID="lblNewReplaceWord" suffix=":" ControlName="txtNewReplaceWord" />
			<asp:TextBox ID="txtNewReplaceWord" runat="server" />
			<asp:ImageButton ID="imgAdd" runat="server" />
		</div>
		<div class="dnnFormItem">
			<dnn:Label runat="server" ID="lblFilter" ControlName="rcbSearchType" />
			<asp:DropDownList ID="rcbSearchType" runat="server" AutoPostBack="true" />
		</div>
		<asp:datagrid id="grdBadWords" Runat="server" GridLines="None" width="100%" cssclass="dnnWordsGrid" CellPadding="0" AutoGenerateColumns="false" DataKeyField="ItemID" >
			<headerstyle cssclass="dnnGridHeader" verticalalign="Top"/>
			<itemstyle cssclass="dnnGridItem" horizontalalign="Left" />
			<alternatingitemstyle cssclass="dnnGridAltItem" />
			<edititemstyle cssclass="dnnFormInput" />
			<selecteditemstyle cssclass="dnnFormError" />
			<footerstyle cssclass="dnnGridFooter" />
			<pagerstyle cssclass="dnnGridPager" />
			<Columns>
				<asp:BoundColumn DataField="BadWord" HeaderText="BadWord" ItemStyle-CssClass="Forum_Grid_Left" />
				<asp:BoundColumn DataField="ReplacedWord" HeaderText="ReplaceWord" ItemStyle-CssClass="Forum_Grid_Middle" />
				<asp:TemplateColumn ItemStyle-HorizontalAlign="Right" ItemStyle-Width="75px" ItemStyle-CssClass="Forum_Grid_Right" >
					<itemtemplate>
						<asp:imagebutton id="cmdEdit" runat="server" causesvalidation="false" commandname="Edit" resourcekey="imgEdit" EnableViewState="false" />
						<asp:imagebutton id="cmdDelete" runat="server" causesvalidation="false" commandname="Delete" resourcekey="imgDelete" EnableViewState="false" />
					</itemtemplate>
					<edititemtemplate>
						<asp:imagebutton id="cmdUpdate" runat="server" causesvalidation="false" commandname="Update" resourcekey="imgUpdate" EnableViewState="false" />
						<asp:imagebutton id="cmdCancel" runat="server" causesvalidation="false" commandname="Cancel" resourcekey="imgCancel" EnableViewState="false" />
					</edititemtemplate>
				</asp:TemplateColumn>
			</Columns>
		</asp:datagrid>
		<dnnforum:AjaxPager ID="BottomPager" runat="server" Width="100%"/>
	</fieldset>
</div>