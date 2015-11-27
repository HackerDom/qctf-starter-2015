<%@ Page Language="C#" AutoEventWireup="false" CodeBehind="Default.aspx.cs" Inherits="main.Default" MasterPageFile="Main.Master" %>
<%@ Import Namespace="main.utils" %>
<%@ Register TagPrefix="web" Src="chat/Chat.ascx" TagName="Chat" %>
<%@ Register TagPrefix="web" Src="files/Explorer.ascx" TagName="Explorer" %>
<asp:Content runat="server" ContentPlaceHolderID="Body">
<div class="header">
	<div class="header-avatar" style="background-image:url(/static/img/avatars/<%:Avatar ?? "default.gif"%>)"></div>
	<div class="header-title"><%:Login%></div>
	<%:GetStars()%></div>
<table class="main-tbl">
	<colgroup><col class="main-tbl-lcol"/><col class="main-tbl-rcol"/></colgroup>
	<tr><td colspan="2" class="main-tbl-header-push"></td></tr>
	<tr>
		<td rowspan="3" class="main-tbl-cell main-tbl-container">
			<web:Chat runat="server" ID="Chat"/>
		</td>
		<td class="main-tbl-cell main-cptn">
			<div class="lbl lbl-tag main-tbl-lbl">Онлайн</div>
		</td>
	</tr>
	<tr>
		<td class="main-tbl-cell main-files">
			<div class="lbl lbl-tag main-tbl-lbl">Файлы</div>
			<web:Explorer runat="server" ID="Explorer"/>
		</td>
	</tr>
	<tr><td class="main-tbl-cell main-img <%:HasBombTimer ? "main-img_erth" : null%>"></td></tr>
</table>
<asp:PlaceHolder runat="server" ID="StartBombTimer" Visible="False"><script>setTimer("<%:new HtmlString(EndTime.ToJsDate())%>");</script></asp:PlaceHolder>
</asp:Content>