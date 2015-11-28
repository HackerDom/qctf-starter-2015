<%@ Page Language="C#" AutoEventWireup="false" CodeBehind="Scores.aspx.cs" Inherits="main.Scores" MasterPageFile="Main.Master" %>
<asp:Content runat="server" ContentPlaceHolderID="Meta">
	<meta http-equiv="refresh" content="30"/>
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="Body">
	<div class="header"><div class="header-logo"></div><div class="header-title">СКБ Периметр</div></div>
	<div class="scores"><div class="scores-inner">
	<div class="scores-title">Прогресс</div>
	<asp:Repeater runat="server" ID="ScoresList" ItemType="main.Score">
		<HeaderTemplate><table class="scores-tbl"><colgroup><col class="scores-imgcol"/><col class="scores-namecol"/><col class="scores-starscol"/><col class="scores-prcentcol"/></colgroup><tbody></HeaderTemplate>
		<ItemTemplate><tr><td class="scores-img" style="background-image:url(/static/img/avatars/<%#:Item.Avatar ?? "default.gif"%>)"><div class="scores-place"><%#:Container.ItemIndex + 1%></div></td><td class="scores-name"><%#:Item.Name%><br/><span class="scores-area"><%#:Item.Area%></span></td><td><%#:GetStars(Item.Stars)%></td><td class="scores-prcent"><%#:Item.Value.ToString("P0")%></td></tr></ItemTemplate>
		<FooterTemplate></tbody></table></FooterTemplate>
	</asp:Repeater>
	</div></div>
	<script>$(".scores-inner").perfectScrollbar();</script>
</asp:Content>