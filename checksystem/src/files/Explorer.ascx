<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="Explorer.ascx.cs" Inherits="main.files.Explorer" %>
<div class="files-history"><div class="files-history-inner">
	<asp:Repeater runat="server" ID="FilesList" ItemType="main.db.File">
		<ItemTemplate><div><a href="<%#:Item.Url%>" target="_blank" class="file"><span class="file-img"></span><span class="file-text"><%#:Item.Name%><span class="file-ext"><%#:Item.Ext%></span></span></a></div></ItemTemplate>
	</asp:Repeater>
</div></div>