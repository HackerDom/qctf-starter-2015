<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="ChatMessage.ascx.cs" Inherits="main.chat.ChatMessage" %>
<%@ Import Namespace="main.utils" %>
<tr>
	<asp:PlaceHolder runat="server" ID="LeftTime" Visible="False"><td class="chat-msg-time"><%:Msg.Time.ToMinutesTime()%></td></asp:PlaceHolder>
	<td colspan="2" class="chat-msg-text <%:IsQuestion ? "chat-msg-question" : null%>"><%:Text%></td>
	<asp:PlaceHolder runat="server" ID="RightTime" Visible="False"><td class="chat-msg-time text-right"><%:Msg.Time.ToMinutesTime()%></td></asp:PlaceHolder>
</tr>