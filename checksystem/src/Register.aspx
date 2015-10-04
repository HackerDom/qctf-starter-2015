<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="false" CodeBehind="Register.aspx.cs" Inherits="main.Register" %>
<asp:Content ID="Content2" ContentPlaceHolderID="Body" runat="server">
<div class="page-wrap">
	<div class="page">
		<div class="login-logo"></div>
		<div class="login-title">Силы Космической Безопасности</div>
		<div class="login-title2">Периметр</div>
		<form id="js-reg-form" class="login-form relative">
			<div class="login-note">регистрация в системе: введите логин</div>
			<div class="login-pass-wrap">
				<div id="js-auth-fail" class="lbl lbl-err chat-lbl-err js-err"></div>
				<input type="text" name="login" class="login-pass" autofocus required maxlength="32"/>
			</div>
		</form>
		<div class="login-warn">СЕКРЕТНЫЙ УРОВЕНЬ ДОСТУПА</div>
	</div>
</div>
</asp:Content>