<%@ Register TagPrefix="dnnforum" Namespace="DotNetNuke.Modules.Forum.Controls" Assembly="DotNetNuke.Modules.Forum" %>
<%@ Register TagPrefix="dnn" TagName="label" Src="~/controls/LabelControl.ascx" %>
<%@ Register TagPrefix="Portal" TagName="URL" Src="~/controls/URLControl.ascx" %>
<%@ Register TagPrefix="Portal" TagName="Tracking" Src="~/controls/URLTrackingControl.ascx" %>
<%@ Register TagPrefix="forum" TagName="ACPmenu" src="~/DesktopModules/Forum/Controls/ACP_Menu.ascx" %>
<%@ Control language="vb" CodeBehind="ACP_ForumEdit.ascx.vb" AutoEventWireup="false" Explicit="true" Inherits="DotNetNuke.Modules.Forum.ACP.ForumEdit" %>
<%@ Register TagPrefix="DNN" Namespace="DotNetNuke.UI.WebControls" Assembly="DotNetNuke.WebControls" %>
<%@ Register Assembly="DotNetNuke.Web.Deprecated" Namespace="DotNetNuke.Web.UI.WebControls" TagPrefix="dnnweb" %>
<%@ Register TagPrefix="wrapper" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<div class="ACP-ForumEdit">
	<table cellpadding="0" cellspacing="0" width="100%" border="0" class="Forum_SearchContainer" >
		<tr valign="top">
			<td class="Forum_UCP_Left"><forum:ACPmenu ID="ACPmenu" runat="server" /></td>
			<td class="Forum_UCP_Right">
				<div class="dnnForm dnnClear">
					<h2 class="dnnFormSectionHead"><asp:Label id="lblTitle" runat="server" resourcekey="EditForum" EnableViewState="false" /></h2>
					<wrapper:RadTabStrip ID="rtsForum" runat="server" Skin="Sitefinity" MultiPageID="rmpForumSettings" SelectedIndex="0" CausesValidation="false" />		
					<dnnweb:DnnMultiPage ID="rmpForumSettings" runat="server" SelectedIndex="0">
						<dnnweb:DnnPageView ID="rpvGeneral" runat="server">
							<div class="tabGeneral">
								<div class="dnnFormItem dnnFormHelp dnnClear"><p class="dnnFormRequired"><span><%=LocalizeString("RequiredFields")%></span></p></div>
								<fieldset>
									<div class="dnnFormItem" id="divForumID" runat="server" visible="false">
										<dnn:label id="plForumID" runat="server" controlname="txtForumID" />
										<asp:textbox id="txtForumID" runat="server" Enabled="False" />
									</div>
									<div class="dnnFormItem">
										<dnn:Label id="plEnableForum" runat="server" Suffix=":" controlname="chkActive" />
										<asp:checkbox id="chkActive" runat="server" />
									</div>
									<div class="dnnFormItem">
										<dnn:label id="plGroupName" runat="server" Suffix=":" controlname="ddlGroup" />
										<asp:DropDownList ID="rcbGroup" runat="server" AutoPostBack="true" />
									</div>
									<div class="dnnFormItem" id="divParentForum" runat="server">
										<dnn:label id="plParentForumName" runat="server" Suffix=":" controlname="ddlParentForum" />
										<asp:DropDownList ID="rcbParentForum" runat="server" AutoPostBack="true" />
									</div>
									<div class="dnnFormItem">
										<dnn:label id="plForumName" runat="server" Suffix=":" controlname="txtForumName"  />
										<asp:textbox id="txtForumName" runat="server" CssClass="dnnFormRequired" MaxLength="255" />
										<asp:RequiredFieldValidator ID="valName" runat="server" ErrorMessage="*" CssClass="dnnFormMessage dnnFormerror" Display="Dynamic" ControlToValidate="txtForumName" SetFocusOnError="true" />
									</div>
									<div class="dnnFormItem">
										<dnn:label id="plForumDescription" runat="server" Suffix=":" controlname="txtForumDescription" />
										<asp:textbox id="txtForumDescription" runat="server" Rows="8" TextMode="MultiLine" MaxLength="2048" />
									</div>
									<div class="dnnFormItem">
										<dnn:label ID="plForumCreatorName" runat="server" controlname="txtForumCreatorName" Suffix=":" />
										<asp:TextBox ID="txtForumCreatorName" runat="server" Enabled="False" />
									</div>
									<div class="dnnFormItem">
										<dnn:label id="plCreatedDate" runat="server" Suffix=":" controlname="txtCreatedDate" />
										<asp:textbox id="txtCreatedDate" runat="server" Enabled="False" />
									</div>
									<div class="dnnFormItem">
										<dnn:label id="plUpdatedName" runat="server" Suffix=":" controlname="txtUpdatedName" />
										<asp:textbox id="txtUpdatedName" runat="server" Enabled="False" />
									</div>
									<div class="dnnFormItem">
										<dnn:label id="plUpdatedDate" runat="server" Suffix=":" controlname="txtUpdatedDate" />
										<asp:textbox id="txtUpdatedDate" runat="server" Enabled="False" />
									</div>
								</fieldset>
							</div>
						</dnnweb:DnnPageView>
						<wrapper:RadPageView ID="rpvOptions" runat="server">
							<div class="tabOptions">
								<fieldset>
									<div class="dnnFormItem">
										<dnn:label id="plForumType" runat="server" Suffix=":" controlname="ddlForumType" />
										<asp:DropDownList ID="rcbForumType" runat="server" AutoPostBack="true" />
									</div>
									<div class="dnnFormItem" id="divForumBehavior" runat="server">
										<dnn:label id="plForumBehavior" runat="server" Suffix=":" controlname="ddlForumBehavior" />
										<asp:DropDownList ID="rcbForumBehavior" runat="server" AutoPostBack="true" />
									</div>
									<div class="dnnFormItem" id="divForumLink" runat="server">
										<dnn:label id="plForumLink" runat="server" Suffix=":" controlname="txtForumLink" />
										<portal:url id="ctlURL" runat="server" showtabs="False" urltype="F" shownewwindow="True" />
									</div>
									<div class="dnnFormItem" id="divLinkTracking" runat="server">
										<portal:tracking id="ctlTracking" runat="server"></portal:tracking>
									</div>
									<div class="dnnFormItem" id="divPolls" runat="server">
										<dnn:label id="plAllowPolls" runat="server" Suffix=":" controlname="chkAllowPolls" />
										<asp:checkbox id="chkAllowPolls" runat="server" />
									</div>
									<div class="dnnFormItem" id="divThreadStatus" runat="server">
										<dnn:label id="plEnableForumsThreadStatus" runat="server" Suffix=":" controlname="chkEnableForumsThreadStatus" />
										<asp:checkbox id="chkEnableForumsThreadStatus" runat="server" />
									</div>
									<div class="dnnFormItem" id="divRating" runat="server">
										<dnn:label id="plEnableForumsRating" runat="server" Suffix=":" controlname="chkEnableForumsRating" />
										<asp:checkbox id="chkEnableForumsRating" runat="server" />
									</div>
									<div class="dnnFormItem" id="divEnableRSS" runat="server">
										<dnn:label id="plEnableRSS" runat="server" Suffix=":" controlname="chkEnableRSS" />
										<asp:checkbox id="chkEnableRSS" runat="server" />
									</div>
									<div class="dnnFormItem" id="divEnableSitemap" runat="server">
										<dnn:label id="plEnableSitemap" runat="server" Suffix=":" controlname="chkEnableSitemap" />
										<asp:checkbox id="chkEnableSitemap" runat="server" AutoPostBack="true" />
									</div>
									<div class="dnnFormItem" id="divSitemapPriority" runat="server">
										<dnn:label id="plSitemapPriority" runat="server" Suffix=":" controlname="txtSitemapPriority" />
										<dnnweb:DnnNumericTextBox ID="textSitemapPriority" runat="server" MinValue="0" MaxValue="1" NumberFormat-DecimalDigits="2" IncrementSettings-Step=".1" />
									</div>
									<div id="divPermissions" runat="server">
										<dnnforum:forumpermissionsgrid id="dgPermissions" runat="server"></dnnforum:forumpermissionsgrid>
										<asp:Label id="lblPrivateNote" runat="server" CssClass="Normal" resourcekey="lblPrivateNote" EnableViewState="false" />
									</div>
									<div class="dnnFormItem">
										<dnn:label id="plForumPermTemplate" runat="server" controlname="ddlForumPermTemplate" suffix=":" />
										<asp:DropDownList ID="rcbForumPermTemplate" runat="server" AutoPostBack="true" DataTextField="Name" DataValueField="ForumID" />
									</div>
								</fieldset>
							</div>
						</wrapper:RadPageView>
						<wrapper:RadPageView ID="rpvEmail" runat="server">
							<div class="tabEmail">
								<div class="dnnFormItem dnnFormHelp dnnClear"><p class="dnnFormRequired"><span><%=LocalizeString("RequiredFields")%></span></p></div>
								<fieldset>
									<div class="dnnFormItem">
										<dnn:label ID="plEmailAddress" runat="server" controlname="txtEmailAddress" Suffix=":" />
										<asp:TextBox ID="txtEmailAddress" runat="server" MaxLength="100" CssClass="dnnFormRequired" />
										<asp:RequiredFieldValidator ID="valAddy" runat="server" ErrorMessage="*" CssClass="dnnFormMessage dnnFormError" Display="Dynamic" ControlToValidate="txtEmailAddress" SetFocusOnError="true" />
										<asp:RegularExpressionValidator ID="valEmailAddy" runat="server" ControlToValidate="txtEmailAddress" CssClass="dnnFormMessage dnnFormValidationSummary" Display="Dynamic"  ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" resourcekey="valEmailAddy.ErrorMessage" SetFocusOnError="true" />
									</div>
									<div class="dnnFormItem">
										<dnn:label ID="plEmailFriendlyFrom" runat="server" controlname="txtEmailFriendlyFrom" Suffix=":" />
										<asp:TextBox ID="txtEmailFriendlyFrom" runat="server" MaxLength="50" CssClass="dnnFormRequired" />
										<asp:RequiredFieldValidator ID="valDisplay" runat="server" ErrorMessage="*" CssClass="dnnFormMessage dnnFormError" Display="Dynamic" ControlToValidate="txtEmailFriendlyFrom" />
									</div>
									<div class="dnnFormItem" id="divSubscribeByDefault" runat="server">
										<dnn:label ID="plNotifyByDefault" runat="server" controlname="txtEmailAddress" Suffix=":" />
										<asp:CheckBox ID="chkNotifyByDefault" runat="server" />
									</div>
									<div class="dnnFormItem" id="divEmailStatusChange" runat="server" visible="false">
										<dnn:label ID="plEmailStatusChange" runat="server" controlname="txtEmailAddress" Suffix=":" />
										<asp:CheckBox ID="chkEmailStatusChange" runat="server" />
									</div>
									<div class="dnnFormItem" id="divEmailServer" runat="server" visible="false">
										<dnn:label ID="plEmailServer" runat="server" controlname="txtEmailServer" Suffix=":" />
										<asp:TextBox ID="txtEmailServer" runat="server" MaxLength="150" />
									</div>
									<div class="dnnFormItem" id="divEmailUser" runat="server" visible="false">
										<dnn:label ID="plEmailUser" runat="server" controlname="txtEmailUser" Suffix=":" />
										<asp:TextBox ID="txtEmailUser" runat="server" MaxLength="100" />
									</div>
									<div class="dnnFormItem" id="divEmailPass" runat="server" visible="false">
										<dnn:label ID="plEmailPass" runat="server" controlname="txtEmailPass" Suffix=":" />
										<asp:TextBox ID="txtEmailPass" runat="server" MaxLength="50" TextMode="Password" />
									</div>
									<div class="dnnFormItem" id="divEmailPort" runat="server" visible="false">
										<dnn:label ID="plEmailPort" runat="server" controlname="txtEmailPort" Suffix=":" />
										<asp:TextBox ID="txtEmailPort" runat="server" MaxLength="5" />
									</div>
									<div class="dnnFormItem" id="divEmailSSL" runat="server" visible="false">
										<dnn:label ID="plEmailEnableSSL" runat="server" controlname="chkEmailEnableSSL" Suffix=":" />
										<asp:Checkbox ID="chkEmailEnableSSL" runat="server" />
									</div>
									<div class="dnnFormItem" id="divEmailAuth" runat="server" visible="false">
										<dnn:label ID="plEmailAuth" runat="server" controlname="txtForumEmailFriendly" Suffix=":" />
										<asp:DropDownList ID="ddlEmailAuth" runat="server" CssClass="Forum_NormalTextBox" />
									</div>
									<div class="dnnFormItem">
										
									</div>
								</fieldset>
							</div>
						</wrapper:RadPageView>
					</dnnweb:DnnMultiPage>
					<ul class="dnnActions dnnClear">
						<li><asp:linkbutton class="dnnPrimaryAction" id="cmdAdd" runat="server" resourcekey="cmdAdd" /></li>
						<li><asp:linkbutton class="dnnPrimaryAction" id="cmdUpdate" runat="server" resourcekey="cmdUpdate" /></li>
						<li><asp:linkbutton class="dnnSecondaryAction" id="cmdDelete" runat="server" resourcekey="cmdDelete" CausesValidation="false" /></li>
					</ul>
				</div>
			</td>
		</tr>
		<tr>
			<td align="center" colspan="2">
				<asp:HyperLink ID="cmdHome" runat="server" CssClass="dnnSecondaryAction" resourcekey="cmdHome" />
			</td>
		</tr>
	</table>
</div>