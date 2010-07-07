<%@ Control language="vb" CodeBehind="ACP_Community.ascx.vb" AutoEventWireup="false" Inherits="DotNetNuke.Modules.Forum.ACP.Community" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<div class="ACP-Community">
	<table cellpadding="0" cellspacing="0" width="100%" border="0">
		<tr>
			<td class="Forum_UCP_Header">
				<asp:Label id="lblTitle" runat="server" resourcekey="lblTitle" />
			</td>
		</tr>
		<tr>
			<td class="Forum_UCP_HeaderInfo">    
			    <table id="tblCommunity" cellspacing="0" cellpadding="0" width="100%" runat="server">
				  <tr id="rowUserOnline" runat="server" visible="false">
					  <td width="35%">
						 <span class="Forum_Row_AdminText">
							<dnn:label id="plUserOnline" runat="server" text="Enable User Online?" controlname="chkUserOnline"></dnn:label>
						  </span>
					   </td>
					  <td valign="middle" align="left">
						 <asp:checkbox id="chkUserOnline" runat="server" CssClass="Forum_NormalTextBox" />
					  </td>
				  </tr>
				  <tr id="rowEnableExtProfile" runat="server">
					<td width="35%">
						<span class="Forum_Row_AdminText">
							<dnn:label id="plEnableExtProfilePage" runat="server" Suffix=":" 
							controlname="chkEnableExtProfilePage"></dnn:label>
						</span>
					</td>
					<td align="left">
							<asp:checkbox id="chkEnableExtProfilePage" runat="server" CssClass="Forum_NormalTextBox" AutoPostBack="True" />
						</td>
				  </tr>
				  <tr id="rowExtProfilePageID" runat="server">
						<td width="35%">
							<span class="Forum_Row_AdminText">
								<dnn:label id="plExtProfilePageID" runat="server" Suffix=":" 
								controlname="ddlExtProfilePageID"></dnn:label>
							</span>
						</td>
						<td valign="middle" align="left">
							<asp:dropdownlist id="ddlExtProfilePageID" runat="server" cssclass="Forum_NormalTextBox" DataTextField="TabName" DataValueField="TabId" Width="250px" />
						</td>
					</tr>
				  <tr id="rowExtProfileUserParam" runat="server">
						<td  width="35%">
							<span class="Forum_Row_AdminText">
								<dnn:label id="plExtProfileUserParam" runat="server" Suffix=":" controlname="txtExtProfileUserParam"></dnn:label>
							</span>
						</td>
						<td valign="middle" align="left">
							<asp:TextBox id="txtExtProfileUserParam" runat="server" CssClass="Forum_NormalTextBox" />
						</td>
					</tr>
			   		<tr id="rowExtProfileParamName" runat="server">
						<td width="35%">
							<span class="Forum_Row_AdminText">
								<dnn:label id="plExtProfileParamName" runat="server" Suffix=":" controlname="txtExtProfileParamName"></dnn:label>
							</span>
						</td>
						<td valign="middle" align="left">
							<asp:TextBox id="txtExtProfileParamName" runat="server" CssClass="Forum_NormalTextBox" />
						</td>
					</tr>
					<tr id="rowExtProfileParamValue" runat="server">
						<td width="35%">
							<span class="Forum_Row_AdminText">
								<dnn:label id="plExtProfileParamValue" runat="server" Suffix=":" controlname="txtExtProfileParamValue"></dnn:label>
							</span>
						</td>
						<td valign="middle" align="left">
							<asp:TextBox id="txtExtProfileParamValue" runat="server" CssClass="Forum_NormalTextBox" />
						</td>
					</tr>				
			    </table>
			    <div align="center">
					<asp:linkbutton cssclass="CommandButton primary-action" id="cmdUpdate" runat="server" text="Update" resourcekey="cmdUpdate" />
				</div>
			    <div align="center">
					<asp:Label ID="lblUpdateDone" runat="server" CssClass="NormalRed" Visible="false" resourcekey="lblUpdateDone" />
			  </div>
			</td>
		</tr>
	</table>
</div>