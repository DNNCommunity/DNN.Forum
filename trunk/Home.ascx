<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Home.ascx.cs" Inherits="DotNetNuke.Modules.Forums.Home" %>
<div class="dnnForumsHome" id="dnnForumsHome">
    <p>Sample Content here.</p>
   <a href="<%# Model.TopicListLink %>">Test</a>
</div>

<%# this.Model.ForumListHtml %>