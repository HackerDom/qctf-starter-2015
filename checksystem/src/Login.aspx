<%@ Page Language="C#" AutoEventWireup="false" CodeBehind="Login.aspx.cs" Inherits="main.Login" MasterPageFile="Main.Master" %>
<asp:Content runat="server" ContentPlaceHolderID="Body">
<div class="page-wrap">
	<div class="page">
		<div class="login-logo"></div>
		<div class="login-title">Силы Космической Безопасности</div>
		<div class="login-title2">Периметр</div>
		<form id="js-login-form" class="login-form relative">
			<div class="login-note">введите код доступа</div>
			<div class="login-pass-wrap">
				<div id="js-auth-fail" class="lbl lbl-err chat-lbl-err js-err"></div>
				<input type="password" name="pass" class="login-pass" autofocus required maxlength="12"/>
			</div>
		</form>
		<div class="login-warn">СЕКРЕТНЫЙ УРОВЕНЬ ДОСТУПА</div>
	</div>
</div>
</asp:Content>