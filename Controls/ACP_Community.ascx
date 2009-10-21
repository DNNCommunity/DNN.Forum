<%@ Control language="vb" CodeBehind="ACP_Community.ascx.vb" AutoEventWireup="false" Inherits="DotNetNuke.Modules.Forum.ACP.Community" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<table cellpadding="0" cellspacing="0" width="100%" border="0">
	<tr>
		<td class="Forum_UCP_Header">
			<asp:Label id="lblTitle" runat="server" resourcekey="lblTitle" />
		</td>
     </tr>
     <tr>
		<td>    
		    <table id="tblCommunity" cellspacing="0" cellpadding="0" width="100%" runat="server">
	            <tr id="rowUserOnline" runat="server" visible="false">
		            <td class="Forum_Row_AdminL" width="35%">
		                <span class="Forum_Row_AdminText">
		                    <dnn:label id="plUserOnline" runat="server" text="Enable User Online?" controlname="chkUserOnline"></dnn:label>
			            </span>
			        </td>
		            <td class="Forum_Row_AdminR" valign="middle" align="left">
		                <asp:checkbox id="chkUserOnline" runat="server" CssClass="Forum_NormalTextBox" />
		            </td>
	            </tr>
	            <tr>
		            <td class="Forum_Row_AdminL" width="35%">
		                <span class="Forum_Row_AdminText">
				            <dnn:label id="plEnablePMSystem" runat="server" Suffix=":" controlname="chkEnablePMSystem"></dnn:label>
			            </span>
			        </td>
		            <td class="Forum_Row_AdminR" valign="middle" align="left">
			            <asp:checkbox id="chkEnablePMSystem" runat="server" CssClass="Forum_NormalTextBox" />
			        </td>
	            </tr>
	            <tr>
		            <td class="Forum_Row_AdminL" width="35%">
		                <span class="Forum_Row_AdminText">
				            <dnn:label id="plEnableMemberList" runat="server" Suffix=":" controlname="chkEnableMemberList"></dnn:label>
			            </span>
			        </td>
		            <td class="Forum_Row_AdminR" valign="middle" align="left">
			            <asp:checkbox id="chkEnableMemberList" runat="server" CssClass="Forum_NormalTextBox" />
			        </td>
	            </tr>
	            <tr id="rowEmoticonEnable" runat="server">
		            <td class="Forum_Row_AdminL" width="35%">
		                <span class="Forum_Row_AdminText">
				            <dnn:label id="plEnableEmoticons" runat="server" Suffix=":" controlname="chkEnableEmoticons"></dnn:label>
			            </span>
			        </td>
		            <td class="Forum_Row_AdminR" valign="middle" align="left">
			            <asp:checkbox id="chkEnableEmoticons" runat="server" CssClass="Forum_NormalTextBox" AutoPostBack="true" />
			        </td>
	            </tr>
	            <tr id="rowEmoticonPath" runat="server">
		           <td class="Forum_Row_AdminL" width="35%">
			          <span class="Forum_Row_AdminText">
				         <dnn:label id="plEmoticonPath" runat="server" controlname="txtEmoticonPath" Suffix=":"></dnn:label>
			          </span>
		           </td>
		           <td class="Forum_Row_AdminR" align="left" width="65%">
			          <asp:TextBox ID="txtEmoticonPath" runat="server" CssClass="Forum_NormalTextBox" Width="250px" MaxLength="255"></asp:TextBox>
			          <asp:RequiredFieldValidator ID="valEmoticonPath" runat="server" ErrorMessage="*" ControlToValidate="txtEmoticonPath" CssClass="NormalRed" Display="Dynamic"></asp:RequiredFieldValidator>
		           </td>
		        </tr>
		        	<tr id="rowEmoticonMaxFileSize" runat="server">
		           <td class="Forum_Row_AdminL" width="35%">
			          <span class="Forum_Row_AdminText">
				         <dnn:label id="plEmoticonMaxFileSize" runat="server" controlname="txtEmoticonMaxFileSize" Suffix=":"></dnn:label>
			          </span>
		           </td>
		           <td class="Forum_Row_AdminR" align="left" width="65%">
			          <asp:TextBox ID="txtEmoticonMaxFileSize" runat="server" CssClass="Forum_NormalTextBox" Width="50px" MaxLength="4" />
					<asp:RequiredFieldValidator ID="valEmoticonFileSize" runat="server" ErrorMessage="*" ControlToValidate="txtEmoticonMaxFileSize" CssClass="NormalRed" Display="Dynamic" EnableViewState="false" />
		           </td>
		        </tr>
		    </table>
		    <div class="Forum_Row_Admin_Foot" align="center">
			     <asp:linkbutton cssclass="CommandButton" id="cmdUpdate" runat="server" text="Update" resourcekey="cmdUpdate" />
			</div>
			<div align="center">
				<asp:Label ID="lblUpdateDone" runat="server" CssClass="NormalRed" Visible="false" resourcekey="lblUpdateDone" />
            </div>
		</td>
     </tr>
</table>