(function(d){var k=d.scrollTo=function(a,i,e){d(window).scrollTo(a,i,e)};k.defaults={axis:'xy',duration:parseFloat(d.fn.jquery)>=1.3?0:1};k.window=function(a){return d(window)._scrollable()};d.fn._scrollable=function(){return this.map(function(){var a=this,i=!a.nodeName||d.inArray(a.nodeName.toLowerCase(),['iframe','#document','html','body'])!=-1;if(!i)return a;var e=(a.contentWindow||a).document||a.ownerDocument||a;return d.browser.safari||e.compatMode=='BackCompat'?e.body:e.documentElement})};d.fn.scrollTo=function(n,j,b){if(typeof j=='object'){b=j;j=0}if(typeof b=='function')b={onAfter:b};if(n=='max')n=9e9;b=d.extend({},k.defaults,b);j=j||b.speed||b.duration;b.queue=b.queue&&b.axis.length>1;if(b.queue)j/=2;b.offset=p(b.offset);b.over=p(b.over);return this._scrollable().each(function(){var q=this,r=d(q),f=n,s,g={},u=r.is('html,body');switch(typeof f){case'number':case'string':if(/^([+-]=)?\d+(\.\d+)?(px|%)?$/.test(f)){f=p(f);break}f=d(f,this);case'object':if(f.is||f.style)s=(f=d(f)).offset()}d.each(b.axis.split(''),function(a,i){var e=i=='x'?'Left':'Top',h=e.toLowerCase(),c='scroll'+e,l=q[c],m=k.max(q,i);if(s){g[c]=s[h]+(u?0:l-r.offset()[h]);if(b.margin){g[c]-=parseInt(f.css('margin'+e))||0;g[c]-=parseInt(f.css('border'+e+'Width'))||0}g[c]+=b.offset[h]||0;if(b.over[h])g[c]+=f[i=='x'?'width':'height']()*b.over[h]}else{var o=f[h];g[c]=o.slice&&o.slice(-1)=='%'?parseFloat(o)/100*m:o}if(/^\d+$/.test(g[c]))g[c]=g[c]<=0?0:Math.min(g[c],m);if(!a&&b.queue){if(l!=g[c])t(b.onAfterFirst);delete g[c]}});t(b.onAfter);function t(a){r.animate(g,j,b.easing,a&&function(){a.call(this,n,b)})}}).end()};k.max=function(a,i){var e=i=='x'?'Width':'Height',h='scroll'+e;if(!d(a).is('html,body'))return a[h]-d(a)[e.toLowerCase()]();var c='client'+e,l=a.ownerDocument.documentElement,m=a.ownerDocument.body;return Math.max(l[h],m[h])-Math.min(l[c],m[c])};function p(a){return typeof a=='object'?a:{top:a,left:a}}})(jQuery);
var starVotingEventsHandled = false;
var popupWindowIsVisible = false;
var searchTextChanged = false;

$(document).ready(function() {
    //disableSelection($(".leftContent").get(0));

    $(document).keyup(function(e) {
        if (popupWindowIsVisible && e.keyCode == 27) {
            HidePopup();
        }
    });
    $(window).resize(function() {
        if (popupWindowIsVisible) {
            CenterPopupWindowInBrowser();
        }
    });
    $("#modalBackground").click(function(e) {
        if (popupWindowIsVisible) {
            HidePopup();
        }
    });
    $("#popupContentClose").click(function(e) {
        if (popupWindowIsVisible) {
            HidePopup();
        }
    });
    $("#modalWindowWrapper").draggable({ containment: "html",
        cursor: 'move',
        opacity: 0.7,
        start: function(event, ui) {
            $(this).append("<div id='mask' style='position:absolute;top:0;left:0;height:100%;width:100%;'></div>");
        },
        stop: function(event, ui) {
            $("#mask").remove();
        }
    });

    $("#menuPanel").hover(
        function() {
            $(this).children().stop();
            $(this).children().fadeTo('slow', 1);
        },
        function() {
            $(this).children().fadeTo('slow', 0.4);
        }
    );

    $("#menuPanel").children().delay(1000).fadeTo('slow', 0.4);
});

function disableSelection(target) {
    if (typeof target.onselectstart != "undefined") //IE route
        target.onselectstart = function() { return false }
    else if (typeof target.style.MozUserSelect != "undefined") //Firefox route
        target.style.MozUserSelect = "none"
    else //All other route (ie: Opera)
        target.onmousedown = function() { return false }
    target.style.cursor = "default"
}

function TextFieldActive(objSender) {
    if (!objSender.getAttribute("clear")) {
        objSender.setAttribute("oldval", objSender.value);
        objSender.setAttribute("clear", "1");
        objSender.value = "";
    }
}
function TextFieldInActive(objSender) {
    var oldvalue = objSender.getAttribute("oldval");
    var isCleared = objSender.getAttribute("clear");

    if (isCleared && objSender.value == "") {
        objSender.removeAttribute("clear")
        objSender.value = oldvalue;
        objSender.removeAttribute("oldval")
    }
}
function ShowPopup(url, width, height) {
    $('#modalBackground').fadeTo("slow", 0.75);
    
    $("#modalWindowWrapper").css({
        "width": width,
        "height": height
    });    
    
    $("#modalWindowContentPanel").attr("src", url);
    $("#modalWindowWrapper").fadeIn();
    CenterPopupWindowInBrowser();

    if ($.browser.msie)
        $("#popupContentClose").show();

    $("#modalWindowContentPanel").attr("height", (height - 25) + "px");    
    popupWindowIsVisible = true;
} 
function HidePopup() {
    $("#modalBackground").fadeOut();
    
    if($.browser.msie)
        $("#popupContentClose").hide();
    $("#modalWindowWrapper").fadeOut();

    var pollResultWindow = $("#pollResultPanel");
    if (pollResultWindow.is(":visible")) {
        pollResultWindow.fadeOut();
        $(document).unbind("click");
    }    
    
    popupWindowIsVisible = false;
}
function CenterPopupWindowInBrowser() {
    var popupWindow = $("#modalWindowWrapper");
    popupWindow.css({
        'top': $(window).height() / 2 - popupWindow.height() / 2,
        'left': $(window).width() / 2 - popupWindow.width() / 2
    });

    var pollResultWindow = $("#pollResultPanel");
    if (pollResultWindow.is(":visible")) {
        pollResultWindow.css({
            'top': $(window).height() / 2 - pollResultWindow.height() / 2,
            'left': $(window).width() / 2 - pollResultWindow.width() / 2
        });
    }
}

function RateLinkCallback(result) {
}
function RegisterStarVotingEvents() {
    if (starVotingEventsHandled)
        return;

    $(".ratingStarBlock").find("div.star:not(.done)").hover(
    function() {
        var stars = $(this).parent().find("div.star");
        var thisIndex = stars.index($(this)) + 1;

        $("a", stars).css("width", "100%");
        stars.slice(0, thisIndex).addClass("hover");
    },
    function() {
        $(this).parent().find("div.star").removeClass("hover");
    });

    $(".ratingStarBlock").find("div.star:not(.done)").click(
    function() {
        var stars = $(this).parent().find("div.star");
        stars.addClass("done");
        stars.removeClass("hover");
        stars.unbind("mouseenter mouseleave click")

        var thisIndex = stars.index($(this)) + 1;
        stars.removeClass("on");
        stars.slice(0, thisIndex).addClass("on");

    });
    
    
    starVotingEventsHandled = true;
}
function canSearch() {
    //return searchTextChanged;
    return true;
}
function ShowPollResult(pollRef) {
    CuplexService.GetPollResult(pollRef, GetPollResultCallback)
}
function GetPollResultCallback(result) {
    var pollResultDialog = $("#pollResultPanel");

    if (result && pollResultDialog) {
        popupWindowIsVisible = true;

        $("#pollCloseButton").click(function(e) { HidePopup(); return false; });
        $(document).click(function(e) { HidePopup(); return false; });

        $("#pollResultContentPanel").html(result);

        var contentWrapper = $("#pollResultContentPanel").find("#pollContentWrapper");
        if (!contentWrapper || contentWrapper.length != 1) return;

        pollResultDialog.draggable({ containment: "html",
            cursor: 'move'
        });
        pollResultDialog.fadeIn();        

        pollResultDialog.css({ 'height': (contentWrapper.height() + 50) + 'px' });
        $("#pollResultContentPanel").css({ 'height': (contentWrapper.height() + 10) + 'px' });

        CenterPopupWindowInBrowser();
    }
}
function PollVote(pollTableId) {
    var pollTable = $("#" + pollTableId);
    if (pollTable) {
        var selectedRadioButton = pollTable.find("input:radio[checked]");
        if (selectedRadioButton.length != 1) {
            alert("Välj ett alternativ innan du röstar.")
            return;
        }
        CuplexService.VoteOnPoll(parseInt(selectedRadioButton.parent().attr("poref")), VoteOnPollCallback);
    }
}
function VoteOnPollCallback(result) {
    if (result) {
        var pollRef = parseInt(result);
        if (!isNaN(pollRef) && pollRef > 0)
            ShowPollResult(pollRef);
        else
            alert("Du har redan röstat på frågan.");
    }
}

function ShowMenu() {
    
}
function setRandomImageCallback(result) {
    var objImg = $("#Img" + activeImg);
    var tmpArr = result.split(';')
    if (tmpArr.length != 2) return false;

    $("#ImageUrlTextBox").val(tmpArr[0]);
    objImg.attr("src", tmpArr[1]);    

    objImg.load(function() {
        $this = $(this);
        var gandParent = $this.parent().parent();
        var imageHeight = $this.height();
        var containerHeight = gandParent.height();

        $this.fadeIn(1000, function() { isChangingImage = false; });
        $this.siblings().fadeOut(1000);

        if (imageHeight > containerHeight) {
            gandParent.stop();
            $(document.body).scrollTo(0, 50, { queue: false });
            gandParent.delay(150).animate({ "height": (imageHeight + 4) + "px" }, 100);
        }
        else if (containerHeight > 700 && imageHeight <= 700) {
            gandParent.stop();
            $(document.body).scrollTo(0, 50, { queue: false });
            gandParent.delay(150).animate({ "height": 700 + "px" }, 200);
        }
    });

    if (activeImg == 1)
        activeImg = 2;
    else
        activeImg = 1;

}
function isDefined(variable)
{
    return eval('(typeof('+variable+') != "undefined");');
}
function SetRandomImage() {
    if (!isChangingImage) {
        isChangingImage = true;
        if (eval('(typeof(randImgUrl) != "undefined" && randImgUrl);')) {
            setRandomImageCallback(randImgUrl);
            randImgUrl = null;
        }
        else
            PageMethods.GetRandomImage(setRandomImageCallback);
    }
}
function ShowPreviousImage() {
    PageMethods.GetPrevRandomImage(setRandomImageCallback);
}