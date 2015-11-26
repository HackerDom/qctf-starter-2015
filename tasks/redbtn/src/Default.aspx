<%@ Page Language="C#" AutoEventWireup="false" CodeBehind="Default.aspx.cs" Inherits="redbtn.Default" MasterPageFile="Main.Master" %>
<asp:Content runat="server" ContentPlaceHolderID="Title">◘◖▱ ◈◓▤▫◗◌</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="Body">
<div class="container">
<form method="POST">
	<button type="submit" class="red-button"></button>
	<label for="password" class="password-label">◔□◗▹◒ ▦◓▪▿▥▻◆◈ ◈▶ ◍◚▱▢◅▹▼▶◗▹ ▢▫○▢ ◜▢□▹◁◉▷◀ ◐▴▤▥◉</label>
	<input id="password" name="password" type="password" class="password-text-box" maxlength="8" required pattern=".{1,8}"/>
	<asp:PlaceHolder runat="server" ID="Error" Visible="False">
		<div class="error"><%:ErrorText%></div>
	</asp:PlaceHolder>
</form>
</div>
</asp:Content>