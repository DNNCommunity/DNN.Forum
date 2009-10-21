<%@ Register TagPrefix="dnn" TagName="TextEditor" Src="~/controls/TextEditor.ascx"%>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<%@ Control language="vb" CodeBehind="PM_Edit.ascx.vb" AutoEventWireup="false" Explicit="True" Inherits="DotNetNuke.Modules.Forum.PMAdd" %>
<%@ Register TagPrefix="DNN" Namespace="DotNetNuke.UI.WebControls" Assembly="DotNetNuke.WebControls" %>
<asp:Literal ID="litCSSLoad" runat="server" />
<asp:Panel ID="pnlContainer" runat="server">
	<table id="tblMain" cellspacing="0" cellpadding="0" width="100%">
	    <tr>
	        <td>
	            <table cellpadding="0" cellspacing="0" width="100%">
	                <tr>
	                	<td class="Forum_HeaderCapLeft"><asp:image id="imgLeftCap" runat="server" /></td>
		                <td class="Forum_Header" width="1px"><asp:image id="imgHeader" runat="server" /></td>
		                <td class="Forum_Header" width="100%"><asp:label ID="lblHeader" runat="server" CssClass="Forum_HeaderText" /></td>
		                <td class="Forum_HeaderCapRight"><asp:image id="imgRightCap" runat="server" /></td>
	                </tr>
	            </table>
	        </td>
		</tr>
		<tr id="rowOldPost" runat="server">
			<td valign="top" align="left">
				<table id="tblOriginalPost" cellspacing="0" cellpadding="0" width="100%" border="0">
					<tr>
						<td>
							<table cellspacing="0" cellpadding="0" border="0">
								<tr>
									<td align="left" class="Forum_Row_AdminL" width="200">
									    <span class="Forum_Row_AdminText">
										   <dnn:label id="plAuthor" runat="server" suffix=":" controlname="lblAuthor"></dnn:label>
										</span>
									</td>
									<td valign="top" class="Forum_Row_AdminR" align="left" width="80%">
									    <asp:HyperLink id="hlAuthor" runat="server" CssClass="Forum_Profile" Target="_blank"></asp:HyperLink>
									</td>
								</tr>
			                    <tr>
				                   <td align="left" class="Forum_Row_AdminL" width="200" valign="top">
					                  <span class="Forum_Row_AdminText">
						                 <dnn:Label ID="plOriginalMessage" runat="server" ControlName="lblMessage" suffix=":" />
					                  </span>
				                   </td>
				                   <td align="left" class="Forum_Row_AdminR" valign="top" width="80%">
					                  <asp:label id="lblMessage" runat="server" CssClass="Forum_Normal"></asp:label>
				                   </td>
			                    </tr>
							</table>
						</td>
					</tr>
				</table>
			</td>
		</tr>
		<tr id="rowNewPost" runat="server">
			<td valign="top" align="left">
				<table id="tblEditContent" cellspacing="0" cellpadding="0" width="100%" border="0">
					<tr>
						<td class="Forum_Row_AdminL" width="200px">
						    <span class="Forum_Row_AdminText">
								<dnn:label id="plRecipient" runat="server" suffix=":" controlname="txtRecipient" resourcekey="plRecipient"></dnn:label>
							</span>
						</td>
						<td class="Forum_Row_AdminR" align="left" width="80%">
							<DNN:DNNTEXTSUGGEST id="txtForumUserSuggest" runat="server" Width="300px" LookupDelay="250" MaxSuggestRows="20" CssClass="Forum_NormalTextBox" >	
							</DNN:DNNTEXTSUGGEST>
						</td>
					</tr>
					<tr id="rowSubject" runat="server">
						<td class="Forum_Row_AdminL" width="200px">
						    <span class="Forum_Row_AdminText">
							   <dnn:label id="plSubject" runat="server" suffix=":" controlname="txtSubject"></dnn:label>
							</span>
						</td>
						<td class="Forum_Row_AdminR" align="left" width="80%">
						    <asp:textbox id="txtSubject" runat="server" cssclass="Forum_NormalTextBox" 
								width="300px" maxlength="100"></asp:textbox>
						    <asp:requiredfieldvalidator id="valSubject" runat="server" resourcekey="valSubject" CssClass="NormalRed" ControlToValidate="txtSubject"></asp:requiredfieldvalidator>
					    </td>
					</tr>
					<tr>
						<td class="Forum_Row_Admin" valign="top" align="left" width="100%" colspan="2">
						    <dnn:texteditor id="teContent" runat="server" width="100%" height="300px" ></dnn:texteditor>
						</td>
					</tr>
				</table>
			</td>
		</tr>
		<tr id="rowPreview" runat="server" visible="false">
			<td valign="top" align="left">
				<table id="tblPreview" cellspacing="0" cellpadding="0" width="100%">
					<tr>
						<td class="Forum_Row_Admin" style="padding:10px 10px 10px 10px">
						    <asp:label id="lblPreview" Runat="server" CssClass="Forum_Normal"></asp:label>
						</td>
					</tr>
				</table>
			</td>
		</tr>
		<tr>
			<td align="center" class="Forum_Row_Admin_Foot">
				&nbsp;<asp:label id="lblInfo" CssClass="NormalRed" runat="server" />&nbsp;
			</td>
		</tr>
		<tr>
		    <td align="center">
		        <asp:linkbutton Cssclass="CommandButton" id="cmdSubmit" runat="server" resourcekey="cmdSendMsg" />
		        <asp:linkbutton Cssclass="CommandButton" id="cmdPreview" runat="server" resourcekey="cmdPreview" CausesValidation="false" />
		        <asp:linkbutton Cssclass="CommandButton" id="cmdBackToEdit" runat="server" resourcekey="cmdReturnToEdit" CausesValidation="False" />
		        <asp:linkbutton Cssclass="CommandButton" id="cmdCancel" runat="server" resourcekey="cmdCancel" CausesValidation="False" EnableViewState="false" />
		    </td>
		</tr>
	</table>
</asp:Panel>