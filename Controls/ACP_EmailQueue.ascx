<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<%@ Control language="vb" CodeBehind="ACP_EmailQueue.ascx.vb" AutoEventWireup="false" Explicit="true" Inherits="DotNetNuke.Modules.Forum.ACP.EmailQueue" %>
<%@ Register TagPrefix="wrapper" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<div class="ACP-EmailQueue">
	<table cellpadding="0" cellspacing="0" width="100%" border="0">
		<tr>
			<td class="Forum_UCP_Header">
				<asp:Label id="lblTitle" runat="server" resourcekey="lblTitle" EnableViewState="false" />
			</td>
		</tr>
		<tr>
			<td class="Forum_UCP_HeaderInfo">
				<table id="tblEmailGeneral" cellspacing="0" cellpadding="0" width="100%">
					<tr id="rowID" runat="server" visible="false">
						<td width="35%">
						    <span class="Forum_Row_AdminText">
							   <dnn:label id="plScheduleItemID" runat="server" controlname="txtScheduleItemID" suffix=":"></dnn:label>
							</span>
						</td>
						<td align="left">
						    <asp:textbox id="txtScheduleItemID" runat="server" CssClass="Forum_NormalTextBox" width="50px" Enabled="false" /> 
						</td>
					</tr>
					<tr>
						<td width="35%">
						    <span class="Forum_Row_AdminText">
							   <dnn:label id="plTaskDeleteDays" runat="server" controlname="txtTaskDeleteDays" suffix=":"></dnn:label>
							</span>
						</td>
						<td align="left">
                            <wrapper:RadNumericTextBox ID="rntxtbxTaskDeleteDays" runat="server" MaxLength="4" NumberFormat-DecimalDigits="0" ShowSpinButtons="true" MinValue="1" MaxValue="1096"/>
						    <asp:RequiredFieldValidator ID="valTasks" runat="server" ErrorMessage="*" CssClass="NormalRed" Display="Dynamic" ControlToValidate="rntxtbxTaskDeleteDays" EnableViewState="false" />
						</td>
					</tr>
					 <tr>
						<td width="35%">
						    <span class="Forum_Row_AdminText">
							   <dnn:Label ID="plEmailDeleteDays" runat="server" ControlName="txtEmailDeleteDays" Suffix=":"></dnn:Label>
						    </span>
						</td>
						<td align="left">
                            <wrapper:RadNumericTextBox ID="rntxtbxEmailDeleteDays" runat="server" MaxLength="4" NumberFormat-DecimalDigits="0" ShowSpinButtons="true" MinValue="1" MaxValue="1096"/>
						    <asp:RequiredFieldValidator ID="valEmails" runat="server" ErrorMessage="*" CssClass="NormalRed" Display="Dynamic" ControlToValidate="rntxtbxEmailDeleteDays" EnableViewState="false" />
						</td>
					 </tr>
				</table>
				<div align="center">
					<asp:linkbutton class="CommandButton" id="cmdUpdate" runat="server" resourcekey="cmdUpdate" EnableViewState="false" />
				</div>
				<div align="center">
					<asp:Label ID="lblUpdateDone" runat="server" CssClass="NormalRed" Visible="false" resourcekey="lblUpdateDone" EnableViewState="false" />
				</div>
			</td>
		</tr>
	</table>
</div>