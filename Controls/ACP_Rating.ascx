<%@ Register TagPrefix="dnnforum" Namespace="DotNetNuke.Modules.Forum.WebControls" Assembly="DotNetNuke.Modules.Forum" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<%@ Control language="vb" CodeBehind="ACP_Rating.ascx.vb" AutoEventWireup="false" Explicit="true" Inherits="DotNetNuke.Modules.Forum.ACP.Rating" %>
<table cellpadding="0" cellspacing="0" width="100%" border="0">
	<tr>
		<td class="Forum_UCP_Header">
			<asp:Label id="lblTitle" runat="server" resourcekey="lblTitle" EnableViewState="false" />
		</td>
     </tr>
     <tr>
		<td class="Forum_UCP_HeaderInfo">
			<table border="0" cellpadding="0" cellspacing="0" width="100%">
				<tr>
					<td  width="35%">
						<span class="Forum_Row_AdminText">
							<dnn:label id="plRatings" runat="server" Suffix=":" controlname="chkRatings"></dnn:label>
						</span>
					</td>
					<td align="left">
						<asp:checkbox id="chkRatings" runat="server" CssClass="Forum_NormalTextBox" EnableViewState="false" />
					</td>
				</tr>
				<tr>
					<td width="35%">
						<span class="Forum_Row_AdminText">
							<dnn:Label ID="plRatingScale" runat="server" ControlName="txtRatingScale" Suffix=":" ></dnn:Label>
						</span>
					</td>
					<td align="left">
						<asp:TextBox ID="txtRatingScale" runat="server" CssClass="Forum_NormalTextBox" MaxLength="50" Width="180px" />
					</td>
				</tr>
			</table>
			<div align="center">
				<asp:linkbutton cssclass="CommandButton" id="cmdUpdate" runat="server" text="Update" resourcekey="cmdUpdate" EnableViewState="false" />
			</div>
			<div align="center">
				<asp:Label ID="lblUpdateDone" runat="server" CssClass="NormalRed" Visible="false" resourcekey="lblUpdateDone" EnableViewState="false" />
			</div>
		</td>
	</tr>
</table>