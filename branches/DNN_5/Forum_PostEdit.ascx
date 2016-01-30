<%@ Register TagPrefix="dnn" TagName="TextEditor" Src="~/controls/TextEditor.ascx"%>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<%@ Register TagPrefix="forum" TagName="Attachment" Src="~/DesktopModules/Forum/controls/AttachmentControl.ascx" %>
<%@ Control language="vb" CodeBehind="Forum_PostEdit.ascx.vb" AutoEventWireup="True" Inherits="DotNetNuke.Modules.Forum.PostEdit" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.UI.WebControls" Assembly="DotNetNuke" %>
<%@ Register TagPrefix="dnnweb" Assembly="DotNetNuke.Web.Deprecated" Namespace="DotNetNuke.Web.UI.WebControls" %>
<div class="dnnForm Post-Edit dnnClear">
	<div class="dnnFormItem dnnFormHelp dnnClear"><p class="dnnFormRequired"><span><%=LocalizeString("RequiredFields")%></span></p></div>
	<div id="divNewPost" runat="server">
		<div class="dnnFormItem">
			<dnn:label id="plForum" runat="server" Suffix=":" controlname="ddlForum" />
			<asp:dropdownlist id="ddlForum" runat="server" />
		</div>
		<div class="dnnFormItem">
			<dnn:label id="plSubject" runat="server" Suffix=":" controlname="txtSubject" />
			<asp:textbox id="txtSubject" runat="server" maxlength="100" CssClass="dnnFormRequired" />
			<asp:requiredfieldvalidator id="valSubject" runat="server" resourcekey="valSubject" CssClass="dnnFormMessage dnnFormError" ControlToValidate="txtSubject" />
		</div>
		<div class="dnnFormItem">
			<dnn:label id="plBody" runat="server" Suffix=":" controlname="teContent" />
			<div class="dnnLeft">
				<dnn:texteditor id="teContent" runat="server" width="450" height="350px" />
			</div>
		</div>
		<div class="dnnFormItem" id="divAttachments" runat="server">
			<dnn:label id="plAttachments" runat="server" suffix=":" controlname="txtAttachments" />
			<div class="dnnLeft">
				<forum:Attachment ID="ctlAttachment" runat="server" Width="400px" />
			</div>
		</div>
		<div class="dnnFormItem" id="divPinned" runat="server">
			<dnn:label id="plPinned" runat="server" Suffix=":" controlname="chkPinned" />
			<asp:checkbox id="chkIsPinned" Runat="server" />
		</div>
		<div class="dnnFormItem" id="divNotify" runat="server">
			<dnn:label id="plNotify" runat="server" Suffix=":" controlname="chkNotify" />
			<asp:checkbox id="chkNotify" runat="server" />
		</div>
		<div class="dnnFormItem" id="divClose" runat="server">
			<dnn:label id="plClose" runat="server" Suffix=":" controlname="chkClose" />
			<asp:checkbox id="chkIsClosed" Runat="server" />
		</div>
		<div class="dnnFormItem" id="divThreadStatus" runat="server">
			<dnn:Label ID="plThreadStatus" runat="server" ControlName="ddlThreadStatus" Suffix=":" />
			<dnnweb:DnnComboBox ID="dnncbThreadStatus" runat="server" AutoPostBack="true" CausesValidation="false" />
		</div>
		<div class="dnnFormItem" id="divTagging" runat="server">
			<dnn:Label ID="plTerms" runat="server" ControlName="tsTerms" Suffix=":" />
			<dnnweb:TermsSelector ID="tsTerms" runat="server" Height="250" Width="400px" />
		</div>
		<div class="dnnFormItem" id="divPoll" runat="server">
			<table id="tblPoll" runat="server" class="Forum_Border" cellpadding="0" cellspacing="0" width="100%">
				<tr>
					<td colspan="2">
						<table cellpadding="0" cellspacing="0" width="100%">
							<tr>
								<td class="Forum_AltHeaderCapLeft">&nbsp;</td>
								<td class="Forum_AltHeader" width="100%">
									<table width="100%" cellpadding="0" cellspacing="0" border="0">
										<tr>
											<td>
												<asp:image id="imgAltHeaderPoll" runat="server" />
											</td>
											<td align="left" width="100%">&nbsp;
												<asp:label id="lblPostPollHeader" Runat="server" resourcekey="lblPostPollHeader" CssClass="Forum_AltHeaderText" />
											</td>
										</tr>
									</table>
								</td>
								<td class="Forum_AltHeaderCapRight">&nbsp;</td>
							</tr>
						</table>
					</td>
				</tr>
				<tr valign="top">
					<td class="Forum_Row_AdminL" width="200">
						<span class="Forum_Row_AdminText">
							<dnn:Label ID="plQuestion" runat="server" ControlName="txtQuestion" Suffix=":" />
						</span>
					</td>
					<td align="left" class="Forum_Row_AdminR" width="80%">
						<asp:TextBox ID="txtQuestion" runat="server" CssClass="Forum_NormalTextBox" Height="50px" MaxLength="500" TextMode="MultiLine" Width="350px" />
						<asp:TextBox ID="txtPollID" runat="server" Visible="false" /> 
					</td>
				</tr>
				<tr>
					<td class="Forum_Row_AdminL" width="200">
						<span class="Forum_Row_AdminText">
							<dnn:Label ID="plAnswers" runat="server" ControlName="dgAnswers" Suffix=":" />
						</span>
					</td>
					<td align="left" class="Forum_Row_AdminR" width="80%">
						<table border="0" cellspacing="0" cellpadding="0">
							<tr>
								<td>
									<asp:DataGrid id="dgAnswers" AutoGenerateColumns="false" width="350px" CellPadding="4"
										GridLines="None" cssclass="DataGrid_Container" Runat="server">
										<headerstyle cssclass="NormalBold" verticalalign="Top" horizontalalign="Left" />
										<itemstyle cssclass="DataGrid_Item" horizontalalign="Left" />
										<alternatingitemstyle cssclass="DataGrid_AlternatingItem" />
										<edititemstyle cssclass="NormalTextBox" />
										<selecteditemstyle cssclass="NormalRed" />
										<footerstyle cssclass="DataGrid_Footer" />
										<pagerstyle cssclass="DataGrid_Pager" />
										<columns>
											<dnn:imagecommandcolumn CommandName="Delete" KeyField="AnswerID" />
											<dnn:imagecommandcolumn CommandName="MoveDown" HeaderText="Dn" KeyField="AnswerID" />
											<dnn:imagecommandcolumn CommandName="MoveUp" HeaderText="Up" KeyField="AnswerID" />
											<dnn:textcolumn DataField="Answer" HeaderText="Answer" Width="200px"></dnn:textcolumn>
										</columns>
									</asp:DataGrid>
								</td>
							</tr>
						</table>
					</td>
				</tr>
				<tr>
					<td class="Forum_Row_AdminL" width="200">
						<span class="Forum_Row_AdminText">
							<dnn:Label ID="plNewAnswer" runat="server" ControlName="txtAddAnswer" Suffix=":" />
						</span>
					</td>
					<td align="left" class="Forum_Row_AdminR" width="80%">
						<asp:TextBox runat="server" ID="txtAddAnswer" CssClass="Forum_NormalTextBox" Width="300px" MaxLength="200" />
						<asp:LinkButton ID="cmdAddAnswer" runat="server" CausesValidation="False" CssClass="CommandButton" resourcekey="cmdAddAnswer" />
						<asp:Label ID="lblNoAnswer" runat="server" CssClass="NormalRed" Visible="False" resourcekey="lblNoAnswer" />
					</td>
				</tr>
				<tr valign="top">
					<td class="Forum_Row_AdminL" width="200">
						<span class="Forum_Row_AdminText">
							<dnn:Label ID="plTakenMessage" runat="server" ControlName="txtTakenMessage" Suffix=":" />
						</span>
					</td>
					<td align="left" class="Forum_Row_AdminR" width="80%">
						<asp:TextBox ID="txtTakenMessage" runat="server" CssClass="Forum_NormalTextBox" Height="50px" MaxLength="500" TextMode="MultiLine" Width="350px" />
					</td>
				</tr>
				<tr>
					<td class="Forum_Row_AdminL" width="200">
						<span class="Forum_Row_AdminText">
							<dnn:Label ID="plShowResults" runat="server" ControlName="chkShowResults" Suffix=":" />
						</span>
					</td>
					<td align="left" class="Forum_Row_AdminR" width="80%">
						<asp:CheckBox ID="chkShowResults" runat="server" CssClass="Forum_NormalTextBox" />
					</td>
				</tr>                 
				<tr>
					<td class="Forum_Row_AdminL" width="200">
						<span class="Forum_Row_AdminText">
							<dnn:Label ID="plEndDate" runat="server" ControlName="txtEndDate" Suffix=":" />
						</span>
					</td>
					<td align="left" class="Forum_Row_AdminR" width="80%">
						<table cellpadding="0" cellspacing="0" border="0" id="PollEndDate">
							<tr>
								<td>
									<asp:TextBox ID="txtEndDate" runat="server" CssClass="Forum_NormalTextBox" Width="150px" />
								</td>
								<td>
									&nbsp;<asp:HyperLink ID="cmdCalEndDate" runat="server" resourcekey="cmdCalEndDate" />
								</td>
							</tr>
						</table>
					</td>                           
				</tr>
				<tr>
					<td class="Forum_Row_Admin_Foot" colspan="2">&nbsp;</td>
				</tr>
			</table>
		</div>
	</div>
	<div id="divPreview" runat="server">
		<table id="tblPreview" runat="server" class="Forum_Border" cellpadding="0" cellspacing="0" width="100%" visible="false">
			<tr>
				<td>
					<table cellpadding="0" cellspacing="0" width="100%">
						<tr>
							<td class="Forum_HeaderCapLeft">
								<asp:Image ID="imgPrevSpaceL" runat="server" />
							</td>
							<td class="Forum_Header" width="100%">
								<table width="100%" cellpadding="0" cellspacing="0" border="0">
									<tr>
										<td>
											<asp:image id="imgAltHeaderPreview" runat="server" />
										</td>
										<td align="left" width="100%">&nbsp;
											<asp:label id="lblPreviewHead" Runat="server" resourcekey="lblPreviewHead" CssClass="Forum_HeaderText" />
										</td>
									</tr>
								</table>
							</td>
							<td class="Forum_HeaderCapRight">
								<asp:Image ID="imgPrevSpaceR" runat="server" />
							</td>
						</tr>
					</table>
				</td>
			</tr>
			<tr>
				<td>
					<table cellspacing="0" cellpadding="0" border="0" width="100%">
							<tr valign="top">
								<td width="80%" class="Forum_Row_Admin" align="left">
									<div style="padding: 10px 10px 10px 0px">
										<asp:label id="lblPreview" Runat="server" CssClass="Forum_Normal" />
									</div>
								</td>
							</tr>
						</table>
				</td>
			</tr>
		</table>
	</div>
	<div class="dnnFormItem" id="divModerate" runat="server" visible="false">
		<asp:label id="lblModerate" Runat="server" CssClass="dnnFormMessage dnnFormWarning" resourcekey="lblModerate"/>
	</div>
	<ul class="dnnActions dnnClear">
		<li><asp:linkbutton cssclass="dnnPrimaryAction" id="cmdSubmit" runat="server" resourcekey="cmdSubmit" /></li>
		<li><asp:linkbutton cssclass="dnnSecondaryAction" id="cmdBackToForum" runat="server" resourcekey="cmdBackToForum" /></li>
		<li><asp:linkbutton cssclass="dnnSecondaryAction" id="cmdBackToEdit" runat="server" resourcekey="cmdReturnToEdit" CausesValidation="False" /></li>
		<li><asp:linkbutton cssclass="dnnSecondaryAction" id="cmdCancel" runat="server" resourcekey="cmdCancel" CausesValidation="False" /></li>
		<li><asp:linkbutton cssclass="dnnSecondaryAction" id="cmdPreview" runat="server" resourcekey="cmdPreview" CausesValidation="false" /></li>
	</ul>
	<div id="divOldPost" runat="server">
		<table id="tblOldPost" runat="server" class="Forum_Border" cellpadding="0" cellspacing="0" width="100%">
			<tr>
				<td>
					<table cellpadding="0" cellspacing="0" width="100%">
						<tr>
							<td class="Forum_HeaderCapLeft">
						<asp:Image ID="imgReplyLeft" runat="server" />
					</td>
							<td class="Forum_Header" width="100%">
								<table width="100%" cellpadding="0" cellspacing="0" border="0">
									<tr>
										<td>
											<asp:image id="imgAltHeaderReply" runat="server" />
										</td>
										<td align="left" width="100%">&nbsp;
											<asp:label id="lblPostReplyHeader" Runat="server" resourcekey="lblPostReplyHeader" CssClass="Forum_HeaderText" />&nbsp;
											<asp:HyperLink id="hlAuthor" runat="server" CssClass="Forum_HeaderText" Target="_blank" />
										</td>
									</tr>
								</table>
							</td>
							<td class="Forum_HeaderCapRight"><asp:Image ID="imgReplyRight" runat="server" /></td>
						</tr>
					</table>
				</td>
			</tr>
			<tr>
				<td>
					<table cellspacing="0" cellpadding="0" border="0" width="100%">
						<tr valign="top">
							<td width="80%" class="Forum_Row_Admin" align="left">
								<div style="padding: 10px 10px 10px 0px">
									<asp:label id="lblMessage" runat="server" CssClass="Forum_Normal" />
								</div>
							</td>
						</tr>
					</table>
				</td>
			</tr>
		</table>
	</div>
</div>