<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<%@ Control language="vb" CodeBehind="ACP_EmailQueue.ascx.vb" AutoEventWireup="false" Explicit="true" Inherits="DotNetNuke.Modules.Forum.ACP.EmailQueue" %>
<table cellpadding="0" cellspacing="0" width="100%" border="0">
	<tr>
		<td class="Forum_UCP_Header">
			<asp:Label id="lblTitle" runat="server" resourcekey="lblTitle" EnableViewState="false" />
		</td>
     </tr>
     <tr>
		<td class="Forum_UCP_HeaderInfo">
			<table id="tblEmailGeneral" cellspacing="0" cellpadding="0" width="100%">
				<tr>
					<td width="30%">
					    <span class="Forum_Row_AdminText">
					        <dnn:label id="plTaskDeleteDays" runat="server" controlname="txtTaskDeleteDays" suffix=":"></dnn:label>
						</span>
					</td>
					<td align="left" width="70%">
					    <asp:textbox id="txtTaskDeleteDays" runat="server" CssClass="Forum_NormalTextBox" width="50px" MaxLength="4" EnableViewState="false" />
					    <asp:RequiredFieldValidator ID="valTasks" runat="server" ErrorMessage="*" CssClass="NormalRed" Display="Dynamic" ControlToValidate="txtTaskDeleteDays" EnableViewState="false" />
		                <asp:RegularExpressionValidator ID="valIntTasks" runat="server" ControlToValidate="txtTaskDeleteDays" CssClass="NormalRed" resourcekey="NumericValidation.ErrorMessage" ValidationExpression="[0-9]{1,}" Display="Dynamic" EnableViewState="false" />	    
					</td>
				</tr>
                <tr>
                    <td width="30%">
                        <span class="Forum_Row_AdminText">
                            <dnn:Label ID="plEmailDeleteDays" runat="server" ControlName="txtEmailDeleteDays" Suffix=":"></dnn:Label>
                        </span>
                    </td>
                    <td align="left" width="70%">
                        <asp:TextBox ID="txtEmailDeleteDays" runat="server" CssClass="Forum_NormalTextBox" Width="50px" MaxLength="4" EnableViewState="false" />
                        <asp:RequiredFieldValidator ID="valEmails" runat="server" ErrorMessage="*" CssClass="NormalRed" Display="Dynamic" ControlToValidate="txtEmailDeleteDays" EnableViewState="false" />
                        <asp:RegularExpressionValidator ID="valIntEmails" runat="server" ControlToValidate="txtEmailDeleteDays" CssClass="NormalRed" resourcekey="NumericValidation.ErrorMessage" ValidationExpression="[0-9]{1,}" Display="Dynamic" EnableViewState="false" />
                    </td>
                </tr>
			</table>
		     <div align="center">
	               <asp:linkbutton class="CommandButton" id="cmdUpdate" runat="server" resourcekey="cmdUpdate" EnableViewState="false" />
	          </div>
			<div align="center">
				<asp:Label ID="lblUpdateDone" runat="server" CssClass="NormalRed" Visible="false" resourcekey="lblUpdateDone" EnableViewState="false" />
			</div>
		</td>
     </tr>
</table>