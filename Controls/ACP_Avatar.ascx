<%@ Control Language="vb" Codebehind="ACP_Avatar.ascx.vb" AutoEventWireup="false" Inherits="DotNetNuke.Modules.Forum.ACP.Avatar" Explicit="true" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<div class="ACP-Avatar">
	<table cellpadding="0" cellspacing="0" width="100%" border="0">
		<tr>
			<td class="Forum_UCP_Header">
				<asp:Label id="lblTitle" runat="server" resourcekey="lblTitle" EnableViewState="false" />
			</td>
		<tr>
			<td class="Forum_UCP_HeaderInfo">    
				<table id="tblGeneral" cellspacing="0" cellpadding="0" width="100%" runat="server">
					<tr>
						<td  width="35%">
							<span class="Forum_Row_AdminText">
								<dnn:label id="plEnableUserAvatar" runat="server" controlname="chkEnableUserAvatar" Suffix=":"></dnn:label>
							</span>
						</td>
						<td  align="left" >
							<asp:CheckBox ID="chkEnableUserAvatar" runat="server" CssClass="Forum_NormalTextBox" AutoPostBack="true" />
						</td>
					</tr>
					<tr id="rowEnableProfileAvatar" runat="server">
						<td  width="35%">
							<span class="Forum_Row_AdminText">
								<dnn:label id="plEnableProfileAvatar" runat="server" controlname="chkEnableProfileAvatar" Suffix=":"></dnn:label>
							</span>
						</td>
						<td  align="left" >
							<asp:CheckBox ID="chkEnableProfileAvatar" runat="server" CssClass="Forum_NormalTextBox" AutoPostBack="true" />
						</td>
					</tr>
					<tr id="rowEnableProfileUserFolders" runat="server">
						<td  width="35%">
							<span class="Forum_Row_AdminText">
								<dnn:label id="plEnableProfileUserFolders" runat="server" controlname="chkEnableProfileAvatar" Suffix=":"></dnn:label>
							</span>
						</td>
						<td  align="left" >
							<asp:CheckBox ID="chkEnableProfileUserFolders" runat="server" CssClass="Forum_NormalTextBox" AutoPostBack="true" />
						</td>
					</tr>
					<tr id="rowProfileAvatarPropertyName" runat="server">
						<td  width="35%">
							<span class="Forum_Row_AdminText">
								<dnn:label id="plProfileAvatarPropertyName" runat="server" controlname="ddlProfileAvatarPropertyName" Suffix=":"></dnn:label>
							</span>
						</td>
						<td  align="left" >
							<asp:DropDownList ID="ddlProfileAvatarPropertyName" runat="server" DataTextField="PropertyName" DataValueField="PropertyName" Width="300px" CssClass="Forum_NormalTextBox" />
						</td>
					</tr>
					<tr id="rowUserAvatarPath" runat="server">
						<td  width="35%">
							<span class="Forum_Row_AdminText">
								<dnn:label id="plUserAvatarPath" runat="server" controlname="txtUserAvatarPath" Suffix=":"></dnn:label>
							</span>
						</td>
						<td  align="left" >
							<asp:TextBox ID="txtUserAvatarPath" runat="server" CssClass="Forum_NormalTextBox" Width="300px" MaxLength="255" />
							<asp:RequiredFieldValidator ID="valUsrPath" runat="server" ErrorMessage="*" ControlToValidate="txtUserAvatarPath" CssClass="NormalRed" Display="Dynamic" EnableViewState="false" />
						</td>
					</tr>
					<tr id="rowUserAvatarDimentions" runat="server">
						<td  width="35%">
							<span class="Forum_Row_AdminText">
								<dnn:label id="plUserAvatarDimensions" runat="server" controlname="txtUserAvatarWidth" Suffix=":"></dnn:label>
							</span>
						</td>
						<td  align="left" >
							<asp:TextBox ID="txtUserAvatarWidth" runat="server" CssClass="Forum_NormalTextBox" Width="50px" MaxLength="3" />
							<asp:RequiredFieldValidator ID="valUsrAvW" runat="server" ErrorMessage="*" ControlToValidate="txtUserAvatarWidth" CssClass="NormalRed" Display="Dynamic" EnableViewState="false" />
							<asp:TextBox ID="txtUserAvatarHeight" runat="server" CssClass="Forum_NormalTextBox" Width="50px" MaxLength="3" />
							<asp:RequiredFieldValidator ID="valUsrH" runat="server" ErrorMessage="*" ControlToValidate="txtUserAvatarHeight" CssClass="NormalRed" Display="Dynamic" EnableViewState="false" />
						</td>
					</tr>
					<tr id="rowUserAvatarSizeLimit" runat="server">
						<td  width="35%">
							<span class="Forum_Row_AdminText">
								<dnn:label id="plUserAvatarSizeLimit" runat="server" Suffix=":"></dnn:label>
							</span>
						</td>
						<td  align="left" >
							<asp:TextBox ID="txtUserAvatarSizeLimit" runat="server" CssClass="Forum_NormalTextBox" Width="50px" MaxLength="4" />
							<asp:RequiredFieldValidator ID="valUsrSize" runat="server" ErrorMessage="*" ControlToValidate="txtUserAvatarSizeLimit" CssClass="NormalRed" Display="Dynamic" EnableViewState="false" />
						</td>
					</tr>
					<tr id="rowUserAvatarPoolEnable" runat="server">
						<td  width="35%">
							<span class="Forum_Row_AdminText">
								<dnn:label id="plEnableUserAvatarPool" runat="server" controlname="chkEnableUserAvatarPool" Suffix=":"></dnn:label>
							</span>
						</td>
						<td align="left"  >
							<asp:CheckBox ID="chkEnableUserAvatarPool" runat="server" CssClass="Forum_NormalTextBox" AutoPostBack="true" />
						</td>
					</tr>
					<tr id="rowUserAvatarPoolPath" runat="server">
						<td  width="35%">
							<span class="Forum_Row_AdminText">
								<dnn:label id="plUserAvatarPoolPath" runat="server" controlname="txtUserAvatarPoolPath" Suffix=":"></dnn:label>
							</span>
						</td>
						<td  align="left" >
							<asp:TextBox ID="txtUserAvatarPoolPath" runat="server" CssClass="Forum_NormalTextBox" Width="300px" MaxLength="255" />
							<asp:RequiredFieldValidator ID="valUserAvatarPoolPath" runat="server" ErrorMessage="*" ControlToValidate="txtUserAvatarPoolPath" CssClass="NormalRed" Display="Dynamic" EnableViewState="false" />
						</td>
					</tr>
					<tr>
						<td  width="35%">
							<span class="Forum_Row_AdminText">
								<dnn:label id="plEnableSystemAvatar" runat="server" controlname="chkEnableSystemAvatar" Suffix=":"></dnn:label>
							</span>
						</td>
						<td  align="left" >
							<asp:CheckBox ID="chkEnableSystemAvatar" runat="server" CssClass="Forum_NormalTextBox" AutoPostBack="true" />
						</td>
					</tr>
					<tr id="rowSystemAvatarPath" runat="server">
						<td  width="35%">
							<span class="Forum_Row_AdminText">
								<dnn:label id="plSystemAvatarPath" runat="server" Suffix=":"></dnn:label>
							</span>
						</td>
						<td  align="left" >
							<asp:TextBox ID="txtSystemAvatarPath" runat="server" CssClass="Forum_NormalTextBox" Width="300px" MaxLength="255" />
							<asp:RequiredFieldValidator ID="valSysPath" runat="server" ErrorMessage="*" ControlToValidate="txtSystemAvatarPath" CssClass="NormalRed" Display="Dynamic"  EnableViewState="false"/>
						</td>
					</tr>
					<tr>
						<td  width="35%">
							<span class="Forum_Row_AdminText">
								<dnn:label id="plEnableRoleAvatar" runat="server" controlname="chkEnableRoleAvatar" Suffix=":"></dnn:label>
							</span>
						</td>
						<td  align="left" >
							<asp:CheckBox ID="chkEnableRoleAvatar" runat="server" CssClass="Forum_NormalTextBox" AutoPostBack="true" />
						</td>
					</tr>
					<tr id="rowRoleAvatarPath" runat="server">
						<td  width="35%">
							<span class="Forum_Row_AdminText">
								<dnn:label id="plRoleAvatarPath" runat="server" Suffix=":" controlname="txtRoleAvatarPath" ></dnn:label>
							</span>
						</td>
						<td  align="left" >
							<asp:TextBox ID="txtRoleAvatarPath" runat="server" CssClass="Forum_NormalTextBox" Width="300px" MaxLength="255" />
							<asp:RequiredFieldValidator ID="valRoleAvatarPath" runat="server" ErrorMessage="*" ControlToValidate="txtRoleAvatarPath" CssClass="NormalRed" Display="Dynamic" EnableViewState="false" />
						</td>
					</tr>
				</table>
				<div align="center">
					<asp:linkbutton id="cmdUpdate" runat="server" resourcekey="cmdUpdate" causesvalidation="False" cssclass="CommandButton primary-action" EnableViewState="false" />
				</div>
				<div align="center">
					<asp:Label ID="lblUpdateDone" runat="server" CssClass="NormalRed" Visible="false" resourcekey="lblUpdateDone" EnableViewState="false" />
				</div>
				<div align="center">
					<asp:CompareValidator ID="validUserDimWidth" runat="server" ControlToValidate="txtUserAvatarWidth" CssClass="NormalRed" Display="Dynamic" resourcekey="validUserDimWidth" Operator="DataTypeCheck" Type="Integer" EnableViewState="false" />
					<asp:CompareValidator ID="validUserDimHeight" runat="server" ControlToValidate="txtUserAvatarHeight" CssClass="NormalRed" Display="Dynamic" resourcekey="validUserDimHeight" Operator="DataTypeCheck" Type="Integer" EnableViewState="false" />
					<asp:CompareValidator ID="validUserSize" runat="server" ControlToValidate="txtUserAvatarSizeLimit" CssClass="NormalRed" Display="Dynamic" resourcekey="validUserSize" Operator="DataTypeCheck" Type="Integer" />
				</div>
			</td>
		</tr>
	</table>
</div>