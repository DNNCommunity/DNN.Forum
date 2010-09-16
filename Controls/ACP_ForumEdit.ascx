<%@ Register TagPrefix="dnnforum" Namespace="DotNetNuke.Modules.Forum.Controls" Assembly="DotNetNuke.Modules.Forum" %>
<%@ Register TagPrefix="dnn" TagName="label" Src="~/controls/LabelControl.ascx" %>
<%@ Register TagPrefix="Portal" TagName="URL" Src="~/controls/URLControl.ascx" %>
<%@ Register TagPrefix="Portal" TagName="Tracking" Src="~/controls/URLTrackingControl.ascx" %>
<%@ Register TagPrefix="forum" TagName="ACPmenu" src="~/DesktopModules/Forum/Controls/ACP_Menu.ascx" %>
<%@ Control language="vb" CodeBehind="ACP_ForumEdit.ascx.vb" AutoEventWireup="false" Explicit="true" Inherits="DotNetNuke.Modules.Forum.ACP.ForumEdit" %>
<%@ Register TagPrefix="DNN" Namespace="DotNetNuke.UI.WebControls" Assembly="DotNetNuke.WebControls" %>
<%@ Register Assembly="DotNetNuke.Web" Namespace="DotNetNuke.Web.UI.WebControls" TagPrefix="dnnweb" %>
<%@ Register assembly="Telerik.Web.UI" namespace="Telerik.Web.UI" tagprefix="telerik" %>
<div class="ACP-ForumEdit">
    <table cellpadding="0" cellspacing="0" width="100%" border="0" class="Forum_SearchContainer" >
		<tr valign="top">
			<td class="Forum_UCP_Left"><forum:ACPmenu ID="ACPmenu" runat="server" /></td>
			<td class="Forum_UCP_Right">
				<table cellspacing="0" cellpadding="0" border="0" width="100%">
					<tr>
					    <td class="Forum_UCP_Header">
							<asp:Label id="lblTitle" runat="server" resourcekey="EditForum" EnableViewState="false" />
						</td>
					</tr>
					<tr>
					    <td class="Forum_UCP_HeaderInfo" align="left">                           
							<telerik:RadTabStrip ID="rtsForum" runat="server" Skin="Vista" MultiPageID="rmpForumSettings" SelectedIndex="0" CausesValidation="false" />		
							<telerik:RadMultiPage ID="rmpForumSettings" runat="server" SelectedIndex="0">
								<telerik:RadPageView ID="rpvGeneral" runat="server">
									 <table id="tblGeneral" cellspacing="0" cellpadding="0" width="100%" runat="server" class="Forum_Row_AdminBox">
										<tr id="rowForumID" runat="server" visible="False">
											<td width="35%">
												<span class="Forum_Row_AdminText">
													<dnn:label id="plForumID" runat="server" controlname="txtForumID"></dnn:label>
												</span>
											</td>
											<td align="left">
												<asp:textbox id="txtForumID" runat="server" cssclass="Forum_NormalTextBox" width="250px" Enabled="False" />
											</td>
										</tr>
										<tr>
											<td width="35%">
												<span class="Forum_Row_AdminText">
													<dnn:Label id="plEnableForum" runat="server" Suffix=":" controlname="chkActive"></dnn:Label>
												</span>
											</td>
											<td align="left">
												<asp:checkbox id="chkActive" runat="server" CssClass="Forum_NormalTextBox" />
											</td>
										</tr>
										<tr>
											<td width="35%">
												<span class="Forum_Row_AdminText">
													<dnn:label id="plGroupName" runat="server" Suffix=":" controlname="ddlGroup"></dnn:label>
												</span>
											</td>
											<td align="left">
												<asp:dropdownlist id="ddlGroup" Runat="server" CssClass="Forum_NormalTextBox" Width="250px" AutoPostBack="true" />
											</td>
										</tr>
										<tr id="rowParentForum" runat="server">
											<td width="35%">
												<span class="Forum_Row_AdminText">
													<dnn:label id="plParentForumName" runat="server" Suffix=":" controlname="ddlParentForum"></dnn:label>
												</span>
											</td>
											<td align="left">
												<asp:dropdownlist id="ddlParentForum" Runat="server" CssClass="Forum_NormalTextBox" Width="250px" AutoPostBack="true" />
											</td>
										</tr>
										<tr>
											<td width="35%">
												<span class="Forum_Row_AdminText">
													<dnn:label id="plForumName" runat="server" Suffix=":" controlname="txtForumName" ></dnn:label>
												</span>
											</td>
											<td align="left">
												<asp:textbox id="txtForumName" runat="server" cssclass="Forum_NormalTextBox" width="250px" MaxLength="255" />
												<asp:RequiredFieldValidator ID="valName" runat="server" ErrorMessage="*" CssClass="NormalRed" Display="Dynamic" ControlToValidate="txtForumName" />
											</td>
										</tr>
										<tr>
										    <td width="35%" valign="top">
											   <span class="Forum_Row_AdminText">
												  <dnn:label id="plForumDescription" runat="server" Suffix=":" controlname="txtForumDescription"></dnn:label>
											   </span>
										    </td>
										    <td align="left">
												  <asp:textbox id="txtForumDescription" runat="server" cssclass="Forum_NormalTextBox" width="250px" Rows="3" TextMode="MultiLine" MaxLength="2048" />
											</td>
										</tr>
										<tr>
											<td width="35%">
												<span class="Forum_Row_AdminText">
													<dnn:label ID="plForumCreatorName" runat="server" controlname="txtForumCreatorName" Suffix=":" />
												</span>
											</td>
											<td align="left">
												<asp:TextBox ID="txtForumCreatorName" runat="server" cssclass="Forum_NormalTextBox" Enabled="False" Width="250px" />
											</td>
										</tr>
										<tr>
											<td width="35%">
												<span class="Forum_Row_AdminText">
													<dnn:label id="plCreatedDate" runat="server" Suffix=":" controlname="txtCreatedDate" ></dnn:label>
												</span>
											</td>
											<td align="left">
												<asp:textbox id="txtCreatedDate" runat="server" cssclass="Forum_NormalTextBox" Enabled="False" Width="250px" />
											</td>
										</tr>
										<tr>
											<td width="35%">
												<span class="Forum_Row_AdminText">
													<dnn:label id="plUpdatedName" runat="server" Suffix=":" controlname="txtUpdatedName" ></dnn:label>
												</span>
											</td>
											<td align="left">
												<asp:textbox id="txtUpdatedName" runat="server" cssclass="Forum_NormalTextBox" width="250px" Enabled="False" />
											</td>
										</tr>
										<tr>
											<td width="35%">
												<span class="Forum_Row_AdminText">
													<dnn:label id="plUpdatedDate" runat="server" Suffix=":" controlname="txtUpdatedDate" ></dnn:label>
												</span>
											</td>
											<td align="left">
												<asp:textbox id="txtUpdatedDate" runat="server" cssclass="Forum_NormalTextBox" width="250px" Enabled="False" />
											</td>
										</tr>
									</table>
								</telerik:RadPageView>
								<telerik:RadPageView ID="rpvOptions" runat="server">
									<table id="tblBehavior" cellspacing="0" cellpadding="0" width="100%" runat="server" class="Forum_Row_AdminBox">
										<tr>
											<td width="35%">
												<span class="Forum_Row_AdminText">
													<dnn:label id="plForumType" runat="server" Suffix=":" controlname="ddlForumType" ></dnn:label>
												</span>
											</td>
											<td align="left">
												<asp:dropdownlist id="ddlForumType" runat="server" CssClass="Forum_NormalTextBox" Width="250px" AutoPostBack="True" />
											</td>
										</tr>
										<tr id="rowForumBehavior" runat="server"> 
											<td width="35%">
												<span class="Forum_Row_AdminText">
													<dnn:label id="plForumBehavior" runat="server" Suffix=":" controlname="ddlForumBehavior"></dnn:label>
												</span>
											</td>
											<td align="left">
												<asp:dropdownlist id="ddlForumBehavior" runat="server" CssClass="Forum_NormalTextBox" Width="250px" AutoPostBack="True" />
											</td>
										</tr>
										<tr id="rowForumLink" runat="server">
											<td width="35%">
												<span class="Forum_Row_AdminText">
													<dnn:label id="plForumLink" runat="server" Suffix=":" controlname="txtForumLink" ></dnn:label>
												</span>
											</td>
											<td align="left">
												<portal:url id="ctlURL" runat="server" showtabs="False" urltype="F" shownewwindow="True"></portal:url>
											</td>
										</tr>
										<tr id="rowLinkTracking" runat="server">
											<td align="center" colspan="2">
												<portal:tracking id="ctlTracking" runat="server"></portal:tracking>
											</td>
										</tr>
										<tr id="rowPolls" runat="server">
											<td width="35%">
												<span class="Forum_Row_AdminText">
													<dnn:label id="plAllowPolls" runat="server" Suffix=":" controlname="chkAllowPolls" MaxLength="245"></dnn:label>
												</span>
											</td>
											<td align="left">
												<asp:checkbox id="chkAllowPolls" runat="server" CssClass="Forum_NormalTextBox" />
											</td>
										</tr>
										<tr id="rowThreadStatus" runat="server">
											<td width="35%">
												<span class="Forum_Row_AdminText">
													<dnn:label id="plEnableForumsThreadStatus" runat="server" Suffix=":" controlname="chkEnableForumsThreadStatus" ></dnn:label>
												</span>
											</td>
											<td  align="left">
												<asp:checkbox id="chkEnableForumsThreadStatus" runat="server" CssClass="Forum_NormalTextBox"></asp:checkbox>
											</td>
										</tr>
										<tr id="rowRating" runat="server">
											<td width="35%">
												<span class="Forum_Row_AdminText">
													<dnn:label id="plEnableForumsRating" runat="server" Suffix=":" controlname="chkEnableForumsRating" ></dnn:label>
												</span>
											</td>
											<td  align="left">
												<asp:checkbox id="chkEnableForumsRating" runat="server" CssClass="Forum_NormalTextBox" />
											</td>
										</tr>
										<tr id="rowEnableRSS" runat="server">
											<td width="35%">
												<span class="Forum_Row_AdminText">
													<dnn:label id="plEnableRSS" runat="server" Suffix=":" controlname="chkEnableRSS"></dnn:label>
												</span>
											</td>
											<td align="left">
												<asp:checkbox id="chkEnableRSS" runat="server" CssClass="Forum_NormalTextBox" />
											</td>
										</tr>
										<tr id="rowEnableSitemap" runat="server">
											<td width="35%">
												<span class="Forum_Row_AdminText">
													<dnn:label id="plEnableSitemap" runat="server" Suffix=":" controlname="chkEnableSitemap"></dnn:label>
												</span>
											</td>
											<td align="left">
												<asp:checkbox id="chkEnableSitemap" runat="server" CssClass="Forum_NormalTextBox" AutoPostBack="true" />
											</td>
										</tr>
										<tr id="rowSitemapPriority" runat="server">
											<td width="35%">
												<span class="Forum_Row_AdminText">
													<dnn:label id="plSitemapPriority" runat="server" Suffix=":" controlname="txtSitemapPriority"></dnn:label>
												</span>
											</td>
											<td align="left">
												<telerik:RadNumericTextBox ID="textSitemapPriority" runat="server" MinValue="0" MaxValue="1" />
											</td>
										</tr>
										<tr id="rowPermissions" runat="server">
											<td align="center" valign="top" colspan="2">
												<dnnforum:forumpermissionsgrid id="dgPermissions" runat="server"></dnnforum:forumpermissionsgrid>
												<br /><asp:Label id="lblPrivateNote" runat="server" CssClass="Normal" resourcekey="lblPrivateNote" EnableViewState="false" />
											</td>
											</tr>
										<tr>
											<td width="35%">
												<br />
												<span class="Forum_Row_AdminText">
														<dnn:label id="plForumPermTemplate" runat="server" resourcekey="plForumPermTemplate" controlname="ddlForumPermTemplate" suffix=":"></dnn:label>
												</span>
											</td>
											<td align="left">
												<br />
												<asp:dropdownlist id="ddlForumPermTemplate" runat="server" CssClass="Forum_NormalTextBox" Width="250px" AutoPostBack="True" DataTextField="Name" DataValueField="ForumID" />
											</td>
										</tr>
									</table>
								</telerik:RadPageView>
								<telerik:RadPageView ID="rpvEmail" runat="server">
									<table id="tblEmail" cellspacing="0" cellpadding="0" width="100%" runat="server" class="Forum_Row_AdminBox">
										<tr>
											<td width="35%">
												<span class="Forum_Row_AdminText">
													<dnn:label ID="plEmailAddress" runat="server" controlname="txtEmailAddress" Suffix=":" />
												</span>
											</td>
											<td align="left">
												<asp:TextBox ID="txtEmailAddress" runat="server" MaxLength="100" cssclass="Forum_NormalTextBox" Width="250px" />
												<asp:RequiredFieldValidator ID="valAddy" runat="server" ErrorMessage="*" CssClass="NormalRed" Display="Dynamic" ControlToValidate="txtEmailAddress" />
												<asp:RegularExpressionValidator ID="valEmailAddy" runat="server" ControlToValidate="txtEmailAddress" CssClass="NormalRed" Display="Dynamic" SetFocusOnError="True" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" resourcekey="valEmailAddy.ErrorMessage" />
											</td>
										</tr>
										<tr>
										    <td width="35%">
												<span class="Forum_Row_AdminText">
													<dnn:label ID="plEmailFriendlyFrom" runat="server" controlname="txtEmailFriendlyFrom" Suffix=":" />
												</span>
											</td>
										    <td align="left">
											    <asp:TextBox ID="txtEmailFriendlyFrom" runat="server" MaxLength="50" cssclass="Forum_NormalTextBox" Width="250px" />
											    <asp:RequiredFieldValidator ID="valDisplay" runat="server" ErrorMessage="*" CssClass="NormalRed" Display="Dynamic" ControlToValidate="txtEmailFriendlyFrom" />
											</td>
										</tr>
										<tr runat="server" id="rowSubsribeByDefault" visible="false">
											<td width="35%">
												<span class="Forum_Row_AdminText">
													<dnn:label ID="plNotifyByDefault" runat="server" controlname="txtEmailAddress" Suffix=":" />
												</span>
											</td>
											<td align="left">
												<asp:CheckBox ID="chkNotifyByDefault" runat="server" cssclass="Forum_NormalTextBox" />
											</td>
										</tr>
										<tr runat="server" id="rowEmailStatusChange" visible="false">
											<td width="35%">
												<span class="Forum_Row_AdminText">
													<dnn:label ID="plEmailStatusChange" runat="server" controlname="txtEmailAddress" Suffix=":" />
												</span>
											</td>
											<td align="left">
												<asp:CheckBox ID="chkEmailStatusChange" runat="server" cssclass="Forum_NormalTextBox" />
											</td>
										</tr>
										<tr runat="server" id="rowEmailServer" visible="false">
											<td width="35%">
												<span class="Forum_Row_AdminText">
													<dnn:label ID="plEmailServer" runat="server" controlname="txtEmailServer" Suffix=":" />
												</span>
											</td>
											<td align="left">
												<asp:TextBox ID="txtEmailServer" runat="server" MaxLength="150" cssclass="Forum_NormalTextBox" Width="250px" />
											</td>
										</tr>
										<tr runat="server" id="rowEmailUser" visible="false">
											<td width="35%">
												<span class="Forum_Row_AdminText">
													<dnn:label ID="plEmailUser" runat="server" controlname="txtEmailUser" Suffix=":" />
												</span>
											</td>
											<td align="left">
												<asp:TextBox ID="txtEmailUser" runat="server" MaxLength="100" cssclass="Forum_NormalTextBox" Width="250px" />
											</td>
										</tr>
										<tr runat="server" id="rowEmailPass" visible="false">
											<td width="35%">
												<span class="Forum_Row_AdminText">
													<dnn:label ID="plEmailPass" runat="server" controlname="txtEmailPass" Suffix=":" />
												</span>
											</td>
											<td align="left">
												<asp:TextBox ID="txtEmailPass" runat="server" MaxLength="50" cssclass="Forum_NormalTextBox" Width="250px" TextMode="Password" />
											</td>
										</tr>
										<tr runat="server" id="rowEmailPort" visible="false">
											<td width="35%">
												<span class="Forum_Row_AdminText">
													<dnn:label ID="plEmailPort" runat="server" controlname="txtEmailPort" Suffix=":" />
												</span>
											</td>
											<td align="left">
												<asp:TextBox ID="txtEmailPort" runat="server" MaxLength="5" cssclass="Forum_NormalTextBox" Width="50px" />
											</td>
										</tr>
										<tr runat="server" id="rowEmailSSL" visible="false">
											<td width="35%">
												<span class="Forum_Row_AdminText">
													<dnn:label ID="plEmailEnableSSL" runat="server" controlname="chkEmailEnableSSL" Suffix=":" />
												</span>
											</td>
											<td align="left">
												<asp:Checkbox ID="chkEmailEnableSSL" runat="server" cssclass="Forum_NormalTextBox" />
											</td>
										</tr>
										<tr runat="server" id="rowEmailAuth" visible="false">
											<td width="35%">
												<span class="Forum_Row_AdminText">
													<dnn:label ID="plEmailAuth" runat="server" controlname="txtForumEmailFriendly" Suffix=":" />
												</span>
											</td>
											<td align="left">
												<asp:DropDownList ID="ddlEmailAuth" runat="server" CssClass="Forum_NormalTextBox" />
											</td>
										</tr>
									</table>
								</telerik:RadPageView>
				   			</telerik:RadMultiPage>
							<div align="center">
								<asp:linkbutton class="CommandButton primary-action" id="cmdAdd" runat="server" resourcekey="cmdAdd" />
								<asp:linkbutton class="CommandButton primary-action" id="cmdUpdate" runat="server" resourcekey="cmdUpdate" />&nbsp;
								<asp:linkbutton class="CommandButton" id="cmdDelete" runat="server" resourcekey="cmdDelete" CausesValidation="false" />
							</div>
						</td>
					</tr>
				</table>
			</td>
		</tr>
		<tr>
			<td align="center" colspan="2">
				<asp:LinkButton ID="cmdHome" runat="server" CssClass="CommandButton" resourcekey="cmdHome" />
			</td>
		</tr>
	</table>
</div>