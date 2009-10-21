<%@ Register TagPrefix="DNN" Namespace="DotNetNuke.UI.WebControls" Assembly="DotNetNuke.WebControls" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<%@ Control Language="vb" AutoEventWireup="false" Codebehind="Forum_Search.ascx.vb" Inherits="DotNetNuke.Modules.Forum.SearchPage" %>
<asp:Literal ID="litCSSLoad" runat="server" />
<table class="Forum_Container" id="tblMain" cellspacing="0" cellpadding="0" width="100%"
	align="center">
	<tr>
		<td valign="top" align="center" width="100%">
			<table id="tblContent" cellspacing="0" cellpadding="4" width="100%">
				<tr>
					<td class="Forum_Row_AdminL" valign="middle" width="125">
					    <span class="Forum_Row_AdminText">
					        <dnn:label id="plPostDates" Suffix=":" runat="server"></dnn:label>
						</span>
					</td>
					<td class="Forum_Row_AdminR" valign="middle" align="left">
					    <table border="0">
					        <tr>
					          <td valign="middle" align="left">
					               <asp:label id="lblStartDate" runat="server" resourcekey="lblStartDate" CssClass="Forum_Row_AdminText" />
					          </td>
					          <td valign="middle" align="left">
					               <asp:textbox id="txtFromDate" cssclass="Forum_NormalTextBox" runat="server" width="75px" />
					          </td>
					    	    <td valign="middle" align="left">
					    	        <asp:hyperlink id="cmdCalFrom" resourcekey="cmdCalFrom" runat="server" />
					    	    </td>
					    	    <td valign="middle" align="left">
					                <asp:label id="lblEndDate" runat="server" resourcekey="lblEndDate" CssClass="Forum_Row_AdminText" />
					            </td>
					    	    <td valign="middle" align="left">
					                <asp:textbox id="txtToDate" cssclass="Forum_NormalTextBox" runat="server" width="75px" />
					            </td>
					    	    <td valign="middle" align="left">
					    	        <asp:hyperlink id="cmdCalTo" resourcekey="cmdCalTo" runat="server" />
					    	    </td>
					    	    <td valign="middle" align="left">
					    	        <asp:comparevalidator id="valStartDate" cssclass="NormalRed" runat="server" resourcekey="valStartDate" display="Dynamic" type="Date" operator="DataTypeCheck" errormessage="Invalid Start date" controltovalidate="txtFromDate" />
			                        <asp:comparevalidator id="valEndDate" cssclass="NormalRed" runat="server" resourcekey="valEndDate" display="Dynamic" type="Date" operator="DataTypeCheck" errormessage="<br />Invalid expiry date" controltovalidate="txtToDate" />
			                        <asp:comparevalidator id="valDates" cssclass="NormalRed" runat="server" resourcekey="valDates" display="Dynamic" type="Date" operator="GreaterThanEqual" errormessage="<br />Expiry Date must be Greater than Effective Date" controltovalidate="txtToDate" controltocompare="txtFromDate" />  
					    	    </td>
					    	</tr>
					    </table>
					 </td>
				</tr>
				<tr>
					<td class="Forum_Row_AdminL" valign="top" width="150">
						<span class="Forum_Row_AdminText">
							<dnn:label id="plUserSuggest" runat="server"  Suffix=":" controlname="txtForumUserSuggest"></dnn:label>
						</span></td>
					<td class="Forum_Row_AdminR" valign="top" align="left">
						<DNN:DNNTEXTSUGGEST id="txtForumUserSuggest" runat="server" Width="250px" LookupDelay="250" MaxSuggestRows="20" CssClass="Forum_NormalTextBox" >	
						</DNN:DNNTEXTSUGGEST>
						<br />
						<br />
					</td>
				</tr>
				<tr>
					<td class="Forum_Row_AdminL" valign="top" width="150">
						<span class="Forum_Row_AdminText">
							<dnn:label id="plSubject" Suffix=":" runat="server" controlname="txtSubject"></dnn:label>
						</span>
					</td>
					<td class="Forum_Row_AdminR" valign="top" align="left">
						<asp:textbox id="txtSubject" cssclass="Forum_NormalTextBox" runat="server" width="250" />
					</td>
				</tr>
				<tr>
					<td class="Forum_Row_AdminL" valign="top" width="150">
						<span class="Forum_Row_AdminText">
							<dnn:label id="plBody" Suffix=":" runat="server" controlname="txtSearch"></dnn:label>
						</span></td>
					<td class="Forum_Row_AdminR" valign="top" align="left">
						<asp:textbox id="txtSearch" cssclass="Forum_NormalTextBox" runat="server" width="250" rows="2" height="48px" />
					</td>
				</tr>
               <tr>
                   <td class="Forum_Row_AdminL" valign="top" width="150">
					<span class="Forum_Row_AdminText">
						<dnn:label id="plThreadStatus" Suffix=":" runat="server" controlname="ddlThreadStatus"></dnn:label>
					</span>
                   </td>
                   <td align="left" class="Forum_Row_AdminR" valign="top">
                       <asp:DropDownList ID="ddlThreadStatus" runat="server" CssClass="Forum_NormalTextBox" Width="250px">
                       </asp:DropDownList></td>
               </tr>
				<tr>
					<td class="Forum_Row_AdminL" valign="top" width="150"><span class="Forum_Row_AdminText"><dnn:label id="plForums" Suffix=":" runat="server" controlname="txtForumID"></dnn:label>
						</span></td>
					<td class="Forum_Row_AdminR" valign="top" align="left">
						<DNN:DNNTree ID="ForumTree" runat="server"></DNN:DNNTree>
					</td>
				</tr>
			</table>
			<asp:label id="lblInfo" cssclass="NormalRed" runat="server"></asp:label></td>
	</tr>
	<tr>
		<td valign="middle" align="center" class="Forum_Row_Admin_Foot">
			<asp:linkbutton class="CommandButton" id="cmdSearch" resourcekey="cmdSearch" runat="server"></asp:linkbutton>&nbsp;
           <asp:linkbutton class="CommandButton" id="cmdCancel" resourcekey="cmdCancel" runat="server"></asp:linkbutton></td>
	</tr>
</table>

