<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="Chat.ascx.cs" Inherits="main.chat.Chat" %>
<%@ Import Namespace="main" %>
<%@ Register TagPrefix="web" TagName="ChatMessage" Src="ChatMessage.ascx" %>
<div class="chat-history">
	<div class="chat-history-inner">
		<asp:Repeater runat="server" ID="Dialog" ItemType="main.db.Msg">
			<HeaderTemplate><table class="chat-msg-tbl"><colgroup><col class="chat-msg-timecol"/><col class="chat-msg-textcol"/><col class="chat-msg-timecol"/></colgroup><tbody></HeaderTemplate>
			<ItemTemplate><web:ChatMessage runat="server" Msg="<%#Item%>"/></ItemTemplate>
			<FooterTemplate></tbody></table></FooterTemplate>
		</asp:Repeater>
	</div>
</div>
<form id="js-chat"><table class="chat-form">
	<tr>
		<td class="relative">
			<div id="js-send-fail" class="lbl lbl-err chat-lbl-err js-err"></div>
			<textarea class="chat-question" name="question" maxlength="<%:Settings.MaxMsgLength%>"></textarea>
			<div class="chat-send-note">Ctrl-Enter to send</div>
		</td>
		<td><input class="chat-send-btn" type="submit" value=""/></td>
	</tr>
</table></form>