<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<%@ Control language="vb" CodeBehind="ACP_EmailTemplate.ascx.vb" AutoEventWireup="false" Explicit="true" Inherits="DotNetNuke.Modules.Forum.ACP.EmailTemplate" %>
<%@ Register TagPrefix="dnn" TagName="TextEditor" Src="~/controls/TextEditor.ascx"%>
<%@ Register TagPrefix="forum" TagName="ACPmenu" src="~/DesktopModules/Forum/Controls/ACP_Menu.ascx" %>
<asp:Literal ID="litCSSLoad" runat="server" />
<table cellpadding="0" cellspacing="0" width="100%" border="0">
	<tr valign="top">
		<td class="Forum_UCP_Left">
		<forum:ACPmenu ID="ACPmenu" runat="server" />
	</td>
		<td class="Forum_UCP_Right">
			<table cellpadding="0" cellspacing="0" width="100%" border="0">
				<tr>
					<td class="Forum_UCP_Header">
						<asp:Label id="lblTitle" runat="server" resourcekey="lblTitle" EnableViewState="false" />
					</td>
				</tr>
				<tr>
					<td class="Forum_UCP_HeaderInfo">
						<table id="tblEmailTemplates" cellspacing="0" cellpadding="0" width="100%" runat="server">
				            <tr id="rowDefaults" runat="server">
					            <td class="Forum_Row_AdminL" width="20%">
					            <span class="Forum_Row_AdminText">
							            <dnn:label id="plDefaults" runat="server" suffix=":" controlname="rblstDefaults"></dnn:label>
						            </span></td>
					            <td class="Forum_Row_AdminR" align="left" width="80%">
						            <asp:RadioButtonList id="rblstDefaults" runat="server" AutoPostBack="True" />
						          </td>
				            </tr>
				            <tr>
					            <td class="Forum_Row_AdminL" width="20%">
					            <span class="Forum_Row_AdminText">
								<dnn:label id="plEmailTemplate" runat="server" controlname="ddlEmailTemplate" suffix=":"></dnn:label>
						            </span></td>
					            <td class="Forum_Row_AdminR" align="left" width="80%">
								<asp:dropdownlist id="ddlEmailTemplate" runat="server" CssClass="Forum_NormalTextBox" AutoPostBack="True" width="350px" DataTextField="EmailTemplateName" DataValueField="EmailTemplateID" />
							</td>
				            </tr>
				            <tr>
					            <td class="Forum_Row_AdminL" width="20%">
					            <span class="Forum_Row_AdminText">
								<dnn:label id="plEmailSubject" runat="server" controlname="txtEmailSubject" suffix=":"></dnn:label>
						            </span>
						     </td>
					            <td class="Forum_Row_AdminR" align="left" width="80%">
								<asp:textbox id="txtEmailSubject" runat="server" width="350px" Columns="26" cssclass="Forum_NormalTextBox" EnableViewState="false" />&nbsp;
						            <asp:requiredfieldvalidator id="valSubject" runat="server" CssClass="NormalRed" resourcekey="valSubject" ControlToValidate="txtEmailSubject" />
						     </td>
				            </tr>
				            <tr>
					            <td class="Forum_Row_AdminL" valign="top" width="20%">
					            <span class="Forum_Row_AdminText">
								<dnn:label id="plEmailHTMLBody" runat="server" controlname="teContent" suffix=":"></dnn:label>
						            </span></td>
					            <td class="Forum_Row_AdminR" align="left" width="80%">
					            <span class="Forum_Row_AdminText">
								<dnn:texteditor id="teContent" runat="server" width="100%" height="300px"></dnn:texteditor>
						          </span></td>
				            </tr>
				            <tr>
					            <td class="Forum_Row_AdminL" valign="top" width="20%">
								<span class="Forum_Row_AdminText"><dnn:label id="plEmailTextBody" runat="server" controlname="txtEmailTextBody" suffix=":"></dnn:label>
								</span>
								</td>
					            <td class="Forum_Row_AdminR" align="left" width="80%">
					            <asp:textbox id="txtEmailTextBody" runat="server" width="100%" Columns="26" cssclass="Forum_NormalTextBox" TextMode="MultiLine" Height="160px" />
					          </td>
				            </tr>
				            <tr>
					            <td class="Forum_Row_AdminL" valign="top" width="20%">
								<span class="Forum_Row_AdminText">
									<dnn:label id="plAvailableKeywords" runat="server" controlname="dlKeywords" suffix=":"></dnn:label>
								</span>
							</td>
					            <td class="Forum_Row_AdminR" align="left" width="80%">
					                <asp:DataList id="dlKeywords" runat="server">
					                    <AlternatingItemStyle CssClass="DataGrid_AlternatingItem"></AlternatingItemStyle>
					                    <FooterStyle CssClass="DataGrid_Footer"></FooterStyle>
					                    <ItemStyle CssClass="DataGrid_Item"></ItemStyle>
					                    <ItemTemplate>
						                     <b><asp:Label Runat="server" ID="lblToken" /></b>
						                     <asp:Label Runat="server" ID="lblDescription" />
					                    </ItemTemplate>
				                    </asp:DataList>
					            </td>
				            </tr>
			            </table>
						<div class="Forum_Row_Admin_Foot" align="center">
						<asp:linkbutton class="CommandButton" id="cmdUpdate" runat="server" resourcekey="cmdUpdate" />
					</div>
						<div align="center">
						<asp:Label ID="lblUpdateDone" runat="server" CssClass="NormalRed" Visible="false" resourcekey="lblUpdateDone" />
					</div>
					</td>
				</tr>
			</table>
		</td>
	</tr>
</table>