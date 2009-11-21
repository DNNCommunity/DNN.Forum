<%@ Register TagPrefix="DNN" Namespace="DotNetNuke.UI.WebControls" Assembly="DotNetNuke.WebControls" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<%@ Control Language="vb" AutoEventWireup="false" Codebehind="Forum_Search.ascx.vb" Inherits="DotNetNuke.Modules.Forum.SearchPage" %>
<%@ Register assembly="Telerik.Web.UI" namespace="Telerik.Web.UI" tagprefix="telerik" %>
<asp:Literal ID="litCSSLoad" runat="server" />
<table class="Forum_SearchContainer" id="tblMain" cellspacing="0" cellpadding="0" width="100%"
	align="center">
	<tr>
		<td valign="top" align="center" width="100%">
			<table id="tblContent" cellspacing="0" cellpadding="4" width="100%">
				<tr>
					<td valign="middle" width="125">
					    <span class="Forum_Row_AdminText">
					        <dnn:label id="plPostDates" Suffix=":" runat="server"></dnn:label>
						</span>
					</td>
					<td valign="middle" align="left">
					    <table border="0">
					        <tr>
					          <td valign="middle" align="left">
					               <asp:label id="lblStartDate" runat="server" resourcekey="lblStartDate" CssClass="Forum_Row_AdminText" />
					          </td>
					          <td valign="middle" align="left">
					               <telerik:RadDatePicker ID="rdpFrom" Skin="Web20" runat="server" Width="100px" />
					          </td>
					    	    <td valign="middle" align="left">
					                <asp:label id="lblEndDate" runat="server" resourcekey="lblEndDate" CssClass="Forum_Row_AdminText" />
					            </td>
					    	    <td valign="middle" align="left">
					                <telerik:RadDatePicker ID="rdpTo" Skin="Web20" runat="server" Width="100px" />
					            </td>

					    	</tr>
					    </table>
					 </td>
				</tr>
				<tr>
					<td valign="top" width="150">
						<span class="Forum_Row_AdminText">
							<dnn:label id="plUserSuggest" runat="server"  Suffix=":" controlname="txtForumUserSuggest"></dnn:label>
						</span></td>
					<td valign="top" align="left">
						<DNN:DNNTEXTSUGGEST id="txtForumUserSuggest" runat="server" Width="250px" LookupDelay="250" MaxSuggestRows="20" CssClass="Forum_NormalTextBox" >	
						</DNN:DNNTEXTSUGGEST>
						<br />
						<br />
					</td>
				</tr>
				<tr>
					<td valign="top" width="150">
						<span class="Forum_Row_AdminText">
							<dnn:label id="plSubject" Suffix=":" runat="server" controlname="txtSubject"></dnn:label>
						</span>
					</td>
					<td valign="top" align="left">
						<asp:textbox id="txtSubject" cssclass="Forum_NormalTextBox" runat="server" width="250" />
					</td>
				</tr>
				<tr>
					<td valign="top" width="150">
						<span class="Forum_Row_AdminText">
							<dnn:label id="plBody" Suffix=":" runat="server" controlname="txtSearch"></dnn:label>
						</span></td>
					<td valign="top" align="left">
						<asp:textbox id="txtSearch" cssclass="Forum_NormalTextBox" runat="server" width="250" rows="2" height="48px" />
					</td>
				</tr>
               <tr>
                   <td valign="top" width="150">
					<span class="Forum_Row_AdminText">
						<dnn:label id="plThreadStatus" Suffix=":" runat="server" controlname="ddlThreadStatus"></dnn:label>
					</span>
                   </td>
                   <td align="left" valign="top">
					<telerik:RadComboBox ID="rcbThreadStatus" runat="server" Width="250px" Skin="Web20" />
                    </td>
               </tr>
				<tr>
					<td valign="top" width="150"><span class="Forum_Row_AdminText"><dnn:label id="plForums" Suffix=":" runat="server" controlname="txtForumID"></dnn:label>
						</span></td>
					<td valign="top" align="left">
						<telerik:RadTreeView ID="rtvForums" runat="server" Skin="Web20" CheckBoxes="true" />
					</td>
				</tr>
			</table>
			<asp:label id="lblInfo" cssclass="NormalRed" runat="server"></asp:label></td>
	</tr>
	<tr>
		<td valign="middle" align="center" >
			<asp:linkbutton class="CommandButton" id="cmdSearch" resourcekey="cmdSearch" runat="server"></asp:linkbutton>&nbsp;
           <asp:linkbutton class="CommandButton" id="cmdCancel" resourcekey="cmdCancel" runat="server"></asp:linkbutton></td>
	</tr>
</table>

