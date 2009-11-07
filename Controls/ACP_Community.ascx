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
			            <asp:checkbox id="chkEnableMemberList" runat="server" CssClass="Forum_NormalTextBox" AutoPostBack="true" />
			        </td>
	            </tr>
	            	<tr id="rowEnableExtDirectory" runat="server">
				<td class="Forum_Row_AdminL" width="35%">
					<span class="Forum_Row_AdminText">
						<dnn:label id="plEnableExtDirectory" runat="server" Suffix=":" 
						controlname="chkEnableExtDirectory"></dnn:label>
					</span>
				</td>
				<td class="Forum_Row_AdminR" valign="middle" align="left">
						<asp:checkbox id="chkEnableExtDirectory" runat="server" 
							CssClass="Forum_NormalTextBox" AutoPostBack="True" />
					</td>
			  </tr>
			  <tr id="rowExtDirectoryPageID" runat="server">
					<td class="Forum_Row_AdminL" width="35%">
						<span class="Forum_Row_AdminText">
							<dnn:label id="plExtDirectoryPageID" runat="server" Suffix=":" 
							controlname="ddlExtDirectoryPageID"></dnn:label>
						</span>
					</td>
					<td class="Forum_Row_AdminR" valign="middle" align="left">
						<asp:dropdownlist id="ddlExtDirectoryPageID" runat="server" cssclass="Forum_NormalTextBox" DataTextField="TabName" DataValueField="TabId" Width="250px" />
					</td>
				</tr>
			  <tr id="rowExtDirectoryParamName" runat="server">
					<td class="Forum_Row_AdminL" width="35%">
						<span class="Forum_Row_AdminText">
							<dnn:label id="plExtDirectoryParamName" runat="server" Suffix=":" controlname="txtExtDirectoryParamName"></dnn:label>
						</span>
					</td>
					<td class="Forum_Row_AdminR" valign="middle" align="left">
						<asp:TextBox id="txtExtDirectoryParamName" runat="server" CssClass="Forum_NormalTextBox" />
					</td>
				</tr>
			  <tr id="rowExtDirectoryParamValue" runat="server">
					<td class="Forum_Row_AdminL" width="35%">
						<span class="Forum_Row_AdminText">
							<dnn:label id="plExtDirectoryParamValue" runat="server" Suffix=":" 
							controlname="txtExtDirectoryParamValue"></dnn:label>
						</span>
					</td>
					<td class="Forum_Row_AdminR" valign="middle" align="left">
						<asp:TextBox id="txtExtDirectoryParamValue" runat="server" 
							CssClass="Forum_NormalTextBox" />
					</td>
				</tr>
	            <tr id="rowEnableExtProfile" runat="server">
			     <td class="Forum_Row_AdminL" width="35%">
			          <span class="Forum_Row_AdminText">
					     <dnn:label id="plEnableExtProfilePage" runat="server" Suffix=":" 
						controlname="chkEnableExtProfilePage"></dnn:label>
				     </span>
				</td>
				<td class="Forum_Row_AdminR" valign="middle" align="left">
                              <asp:checkbox id="chkEnableExtProfilePage" runat="server" CssClass="Forum_NormalTextBox" AutoPostBack="True" />
					</td>
			  </tr>
			  <tr id="rowExtProfilePageID" runat="server">
				     <td class="Forum_Row_AdminL" width="35%">
				          <span class="Forum_Row_AdminText">
						     <dnn:label id="plExtProfilePageID" runat="server" Suffix=":" 
							controlname="ddlExtProfilePageID"></dnn:label>
					     </span>
					</td>
				     <td class="Forum_Row_AdminR" valign="middle" align="left">
                              <asp:dropdownlist id="ddlExtProfilePageID" runat="server" cssclass="Forum_NormalTextBox" DataTextField="TabName" DataValueField="TabId" Width="250px" />
					</td>
			     </tr>
			  <tr id="rowExtProfileUserParam" runat="server">
				     <td class="Forum_Row_AdminL" width="35%">
				          <span class="Forum_Row_AdminText">
						     <dnn:label id="plExtProfileUserParam" runat="server" Suffix=":" controlname="txtExtProfileUserParam"></dnn:label>
					     </span>
					</td>
				     <td class="Forum_Row_AdminR" valign="middle" align="left">
					     <asp:TextBox id="txtExtProfileUserParam" runat="server" CssClass="Forum_NormalTextBox" />
					</td>
			     </tr>
			   	<tr id="rowExtProfileParamName" runat="server">
				     <td class="Forum_Row_AdminL" width="35%">
				          <span class="Forum_Row_AdminText">
						     <dnn:label id="plExtProfileParamName" runat="server" Suffix=":" controlname="txtExtProfileParamName"></dnn:label>
					     </span>
					</td>
				     <td class="Forum_Row_AdminR" valign="middle" align="left">
					     <asp:TextBox id="txtExtProfileParamName" runat="server" CssClass="Forum_NormalTextBox" />
					</td>
			     </tr>
			     <tr id="rowExtProfileParamValue" runat="server">
				     <td class="Forum_Row_AdminL" width="35%">
				          <span class="Forum_Row_AdminText">
						     <dnn:label id="plExtProfileParamValue" runat="server" Suffix=":" controlname="txtExtProfileParamValue"></dnn:label>
					     </span>
					</td>
				     <td class="Forum_Row_AdminR" valign="middle" align="left">
					     <asp:TextBox id="txtExtProfileParamValue" runat="server" CssClass="Forum_NormalTextBox" />
					</td>
			     </tr>				
			  <tr id="rowEnableExtPM" runat="server">
				<td></td>
				<td></td>
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