<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ACP_Advertisement.ascx.vb" Inherits="DotNetNuke.Modules.Forum.ACP.Advertisement" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>

<div class="ACP-Attachment">
<table cellpadding="0" cellspacing="0" width="100%" border="0">
    <tr>
	    <td class="Forum_UCP_Header">
		    <asp:Label id="lblTitle" runat="server" resourcekey="lblTitle" EnableViewState="false" />
	    </td>
        </tr>
        <tr>
	    <td class="Forum_UCP_HeaderInfo">
	        <table id="tblPopular" cellspacing="0" cellpadding="0" width="100%" runat="server">
		        <tr>
			        <td width="35%">
			            <span class="Forum_Row_AdminText">
                            <dnn:Label ID="plAdsAfterFirstPost" runat="server" ControlName="txtAdsAfterFirstPost" Suffix=":" />
				        </span>
			        </td>
			        <td align="left" valign="middle">
                        <asp:CheckBox runat="server" ID="cbAdsAfterFirstPost" />
                    </td>
		        </tr>
                <tr>
			        <td width="35%">
			            <span class="Forum_Row_AdminText">
                            <dnn:Label ID="plAddAdverAfterPostNo" runat="server" ControlName="txtAddAdverAfterPostNo" Suffix=":" />
				        </span>
			        </td>
			        <td valign="middle" align="left">
                        <asp:TextBox runat="server" ID="tbAddAdverAfterPostNo" CssClass="Forum_NormalTextBox" Width="30px" MaxLength="2" EnableViewState="false" />
                        <asp:RequiredFieldValidator ID="valreqView" runat="server" ErrorMessage="*" CssClass="NormalRed" Display="Dynamic" ControlToValidate="tbAddAdverAfterPostNo" EnableViewState="false" />
                        <asp:RegularExpressionValidator ID="valView" runat="server" ControlToValidate="tbAddAdverAfterPostNo" CssClass="NormalRed" resourcekey="NumericValidation.ErrorMessage" ValidationExpression="[0-9]{1,}" Display="Dynamic" EnableViewState="false" />
                    </td>
		        </tr>
                <tr>
			        <td width="35%">
			            <span class="Forum_Row_AdminText">
                            <dnn:Label ID="plAdvertisementText" runat="server" ControlName="txtAdvertisementText" Suffix=":" />
				        </span>
			        </td>
			        <td valign="middle" align="left">
                        <asp:TextBox runat="server" ID="tbAdvertisementText" CssClass="Forum_NormalTextBox" Width="380px" TextMode="MultiLine" EnableViewState="false" Height="190px" />
                    </td>
		        </tr>
	        </table>
            <div align="left" style="font-size:11px">
            <asp:Label runat="server" ID="lblHelp" resourcekey="lblHelp" />
            </div>
            
	        <div align="center">
	        <asp:linkbutton cssclass="CommandButton primary-action" id="cmdUpdate" runat="server" text="Update" resourcekey="cmdUpdate" EnableViewState="false" />
	        </div>
		    <div align="center">
		    <asp:Label ID="lblUpdateDone" runat="server" CssClass="NormalRed" Visible="false" resourcekey="lblUpdateDone" EnableViewState="false" />
		    </div>
	    </td>
    </tr>
</table>
</div>