/*! NightChallenge2015 | SKB Kontur */

Date.prototype.toMinutesTime = function() {
	function numToString(num) {
		return "" + (num > 9 ? num : "0" + num);
	}
	var hour = this.getHours();
	var minutes = this.getMinutes();
	return numToString(hour) + ":" + numToString(minutes);
}

Date.prototype.toUTCSecondsTime = function() {
	function numToString(num) {
		return "" + (num > 9 ? num : "0" + num);
	}
	var hour = this.getUTCHours();
	var minutes = this.getUTCMinutes();
	var seconds = this.getUTCSeconds();
	return numToString(hour) + ":" + numToString(minutes) + ":" + numToString(seconds);
}

function fixIEHeight($div) {
	$div.height($div.parent().outerHeight());
}

function htmlEncode(value) {
	return $('<div/>').text(value).html().replace('"', '&quot;').replace("'", '&apos;');
}

var gtimer;
var END;

function setTimer(endstr) {
	END = new Date(endstr);
	var $img = $(".main-img");

	function setTimerText() {
		var now = new Date();
		var value = END - now + (now.getTimezoneOffset() * 60);
		if(value < 0) value = 0;
		$img.text(new Date(value).toUTCSecondsTime());
	}

	if(!gtimer) gtimer = setInterval(setTimerText, 1000);
}

$(function() {
	/* Registration */

	$("#js-reg-form").submit(function(event) {
		event.preventDefault();
		$.ajax({
			url: "/auth?signup=1",
			type: "POST",
			data: $(this).serialize()
		}).done(function(text) {
			if(text && text.length) {
				var data = JSON.parse(text);
				if(data && data.text) {
					$(".login-warn").html("ACCESS GRANTED<br/>Your password is: " + htmlEncode(data.text) + "<br/><b><a href='/' style='font-size:24px'>ENTER</a></b>");
				}
			}
		}).fail(function(xhr) {
			var error;
			var text = xhr.responseText;
			if(text && text.length) {
				var data = JSON.parse(text);
				if(data && data.error)
					error = data.error;
			}
			$("#js-auth-fail").text(error ? error : "Unknown error").stop(true, true).show(200).delay(3000).hide(200);
		});
	});


	/* Login */

	$("#js-login-form").submit(function(event) {
		event.preventDefault();
		$.ajax({
			url: "/auth",
			type: "POST",
			data: $(this).serialize()
		}).done(function() {
			window.location = "/";
		}).fail(function(xhr) {
			var error;
			var text = xhr.responseText;
			if(text && text.length) {
				var data = JSON.parse(text);
				if(data && data.error)
					error = data.error;
			}
			$("#js-auth-fail").text(error ? error : "Unknown error").stop(true, true).show(200).delay(3000).hide(200);
		});
	});


	/* Chat */

	$(".chat-history").perfectScrollbar();
	$(".files-history").perfectScrollbar();

	$(".chat-question").keydown(function(event) {
		if(event.which == 13 && event.ctrlKey) {
			event.preventDefault();
			$("#js-chat").submit();
		}
	});

	function scrollBottom($div) {
		$div.stop(true, true).animate({ scrollTop: $div.prop("scrollHeight") }, 300);
	}

	function scrollChatBottom() {
		scrollBottom($(".chat-history"));
	}
	function scrollFilesBottom() {
		scrollBottom($(".files-history"));
	}

	function highlight($div) {
		$div.css({ "background-color": "rgba(255, 100, 100, 0.25)" }).delay(3000).animate({ backgroundColor: "transparent" }, 1000);
	}

	function addScore() {
		$(".header").append($("<span class='star'></span>"));
	}
	function addWait() {
		var $wait = $("<tr class='chat-msg-wait'><td colspan='3'>Waiting for response...</td></tr>");
		$(".chat-msg-tbl tbody").append($wait);
		$('.chat-history').perfectScrollbar("update");
		scrollChatBottom();
	}
	function removeWait() {
		$(".chat-msg-wait").remove();
	}
	function addQuestion(time, text) {
		var $time = $("<td class='chat-msg-time'></td>").text(time);
		var $text = $("<td colspan='2' class='chat-msg-text chat-msg-question'></td>").html(htmlEncode(text).replace(/\r?\n/g, "<br/>"));
		var $row = $("<tr></tr>");
		$row.append($text).append($time);
		$(".chat-msg-tbl tbody").append($row);
		$(".chat-history").perfectScrollbar("update");
		scrollChatBottom();
	}
	function addAnswer(time, text) {
		var $time = $("<td class='chat-msg-time'></td>").text(time);
		var $text = $("<td colspan='2' class='chat-msg-text'></td>").html(htmlEncode(text).replace(/\r?\n/g, "<br/>"));
		var $row = $("<tr></tr>");
		$row.append($time).append($text);
		$(".chat-msg-tbl tbody").append($row);
		$(".chat-history").perfectScrollbar("update");
		scrollChatBottom();
		highlight($text);
	}
	function addFile(file) {
		if($(".files-history-inner").find(".file[href='" + file.url + "']").length) //NOTE: Distinct by URL
			return;
		var $text = $("<span class='file-text'></span>").text(file.name);
		$text.append($("<span class='file-ext'></span>").text(file.ext ? file.ext : ""));
		var $file = $("<a class='file' target='_blank'></a>").attr("href", file.url).append($("<span class='file-img'></span>")).append($text);
		var $div = $("<div></div>").append($file);
		$(".files-history-inner").append($div);
		$(".files-history").perfectScrollbar("update");
		scrollFilesBottom();
		highlight($text);
	}

	function update(text) {
		if(text && text.length) {
			var data = JSON.parse(text);
			if(data) {
				if(data.score) addScore();
				if(data.msgs) {
					for(var i = 0; i < data.msgs.length; i++) {
						var msg = data.msgs[i];
						addAnswer(msg.time, msg.text);
					}
				}
				if(data.files) {
					for(var j = 0; j < data.files.length; j++) {
						addFile(data.files[j]);
					}
				}
				if(data.timer) {
					$(".main-img").addClass("main-img_erth");
					setTimer(data.timer);
				} else {
					$(".main-img").removeClass("main-img_erth");
				}
			}
		}
	}

	setInterval(function() {
		$.ajax({
			url: "/check",
			type: "POST"
		}).done(update);
	}, 60000);

	$("#js-chat").submit(function(event) {
		event.preventDefault();
		$.ajax({
			url: "/send",
			type: "POST",
			data: $(this).serialize()
		}).done(function(text) {
			if(text && text.length) {
				$(".main-cptn").stop(true, true).addClass("main-cptn_talk").delay(4000).queue(function() { $(".main-cptn").removeClass("main-cptn_talk"); });
				addQuestion(new Date().toMinutesTime(), $(".chat-question").val());
				addWait();
				setTimeout(function() {
					removeWait();
					update(text);
				}, 2000);
				$(".chat-question").val("");
			}
		}).fail(function(xhr) {
			var error;
			var text = xhr.responseText;
			if(text && text.length) {
				var data = JSON.parse(text);
				if(data && data.error)
					error = data.error;
			}
			$("#js-send-fail").text(error ? error : "Unknown error").stop(true, true).show(200).delay(3000).hide(200);
		});
	});

	$(function() {
		//NOTE: HATE IE!
		if(/*@cc_on!@*/false || !!(navigator.userAgent.match(/Trident/))) {
			var $chat = $(".chat-history");
			var $files = $(".files-history");
			$(window).resize(function() {
				fixIEHeight($chat);
				fixIEHeight($files);
			});
			fixIEHeight($chat);
			fixIEHeight($files);
		}

		scrollChatBottom();
		scrollFilesBottom();
	});

});