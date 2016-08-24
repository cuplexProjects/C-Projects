awaitAjaxUpdate = (srcElem) ->
  _awaitAjaxUpdate new ajaxUpdateDataHolder(srcElem, srcElem.html(), replaceVotingCtrlText)
_awaitAjaxUpdate = (ajax_data) ->
  unless ajax_data.srcElem.html() is ajax_data.orgData
    ajax_data.onComplete()
  else
    setTimeout (->
      _awaitAjaxUpdate ajax_data
    ), 50
ajaxUpdateDataHolder = (srcElem, orgData, onComplete) ->
  @orgData = orgData
  @srcElem = srcElem
  @onComplete = onComplete
replaceVotingCtrlText = ->
  votingControl = $j("#voting_control")
  if votingControl.text().indexOf("0 people found this useful.") >= 0
    votingControl.html votingControl.html().replace("0 people found this useful.", "Found this useful?")
    lnk = votingControl.find("a:contains(' - ')")
    lnk.html "<b>Let us know!</b>"  if lnk.size() > 0
showFooterPopup = (pageNo) ->
  url = ""
  switch pageNo
    when 1
      url = "https://cuplex.se/voc/cookies_and_privacy.html"
    when 2
      url = "https://cuplex.se/voc/terms_of_use.html"
    when 3
      url = "https://cuplex.se/voc/copyright.html"
    else
      return
  iFrame = $j("<div class=\"iframepopup\"><div class=\"modal_iframe_head\"><img src=\"https://cuplex.se/voc/images/popup_close.gif\" onclick=\"$j('.modal_bg').trigger('click');\"> </div><div class=\"iframepopup_bg\"></div><iframe src=\"" + url + "\" frameborder=\"0\" scrolling=\"yes\" width=\"100%\" height=\"100%\" seamless></iframe></div>")
  modal_bg = $j("<div class=\"modal_bg\"></div>").bind("click", ->
    $j(this).unbind().remove()
    $j(".iframepopup").fadeOut().remove()
  ).insertBefore($j("#page"))
  iFrame.hide()
  iFrame.insertAfter(modal_bg).center().fadeIn()
showModalImage = (imgSrc) ->
  modal_bg = $j("<div class=\"modal_bg\"></div>").bind("click", ->
    $j(this).unbind().remove()
    $j(".modal_img").fadeOut().remove()
  ).insertBefore($j("#page"))
  imgbox = $j("<div class=\"modal_img\"><div class=\"modal_img_bg\"></div><div class=\"modal_img_head\"><img src=\"https://cuplex.se/voc/images/popup_close.gif\" onclick=\"$j('.modal_bg').trigger('click');\"> </div><img src=\"" + imgSrc + "\" class=\"modal_img_frame\"></div>")
  imgbox.find(".modal_img_frame").load ->
    rescaleImage this

  imgbox.hide()
  imgbox.insertAfter(modal_bg).center().fadeIn()
rescaleImage = (imgDomElement) ->
  winHeight = parseInt($j(window).height() * 0.9, 10)
  winWidth = parseInt($j(window).width() * 0.9, 10)
  img = $j(imgDomElement)
  imgWidth = img.width()
  imgHeight = img.height()
  imgProportions = (imgWidth + 1.0) / (imgHeight + 1.0)
  
  #Resize and maintain aspect ratio.
  if imgWidth > winWidth or imgHeight > winHeight
    imgWidth = Math.min(imgWidth, winWidth)
    imgHeight = Math.min(imgHeight, winHeight)
    
    #Scale x or y?
    if imgWidth / imgHeight > imgProportions
      imgWidth = parseInt(imgWidth / ((imgWidth / imgHeight) / imgProportions), 10)
    else
      imgHeight = parseInt(imgHeight / (imgProportions / (imgWidth / imgHeight)), 10)
    img.attr "width", imgWidth + "px"
    img.attr "height", imgHeight + "px"
    img.parent().center()
setLanguageLink = (locale) ->
  if locale.id is 1
    $j("#contentcolumn").prepend "<div class=\"lang_link\"><a href=\"https://volvooncall-help-se.volvocars.com/home\" rel=\"nofollow\">Svenska</a></div>"
  else
    $j("#contentcolumn").prepend "<div class=\"lang_link\"><a href=\"https://volvooncall-help.volvocars.com/home\" rel=\"nofollow\">English</a></div>"
_gaq = _gaq or []
_gaq.push ["_setAccount", "UA-38050167-1"]
_gaq.push ["_trackPageview"]
(->
  ga = document.createElement("script")
  ga.type = "text/javascript"
  ga.async = true
  ga.src = ((if "https:" is document.location.protocol then "https://ssl" else "http://www")) + ".google-analytics.com/ga.js"
  s = document.getElementsByTagName("script")[0]
  s.parentNode.insertBefore ga, s
)()
$j("#suggestions_query").remove()
$j("#suggestion_submit").remove()
postbackSearch = $j("input#query").size() > 0
if postbackSearch
  $j("input#query").remove()
  $j("#buttonsubmit.button.search.primary").remove()
new_form = "<input id=\"suggestions_query\" name=\"suggestions_query\" type=\"text\" placeholder=\"Search\"><input class=\"button search primary\" id=\"suggestion_submit\" name=\"commit\" type=\"submit\" value=\"\">"
if postbackSearch
  new_form = new_form.replace("placeholder=\"Search\"", "placeholder=\"Search in this category\"")
  $j("#searchform").prepend new_form
else
  $j("#suggest_form").prepend new_form
$j("#solution_suggestion").detach().insertBefore $j("#contentcolumn").find(".forum_tabs:first")
$j("h2#pinned_topics_title").html "Looking for answers?"
footer = "<div class=\"voc_footer\"><a href=\"javascript:void(0);\" onclick=\"showFooterPopup(3);\">© 1998-2013 Volvo Cars Corporation</a><a href=\"javascript:void(0);\" onclick=\"showFooterPopup(1);\">Privacy & Cookies</a><a href=\"javascript:void(0);\" onclick=\"showFooterPopup(2);\">Terms of Use</a><div class=\"applink_button_wrapper\"><a href=\"http://www.windowsphone.com/sv-se/store/app/volvo-on-call/d367904a-070d-42ef-9dd1-7fae68b318a2\" target=\"_blank\" class=\"app_link\"><img src=\"https://cuplex.se/voc/images/btn_Appstore_Windows.png\" alt=\"Windows phone link\" /></a><a href=\"https://play.google.com/store/apps/details?id=se.volvo.vcc&feature=search_result#?t=W251bGwsMSwxLDEsInNlLnZvbHZvLnZjYyJd\" target=\"_blank\" class=\"app_link\"><img src=\"https://cuplex.se/voc/images/btn_Appstore_Google.png\" alt=\"Google play link\" /></a><a href=\"https://itunes.apple.com/se/app/volvo-on-call/id439635293?mt=8\" target=\"_blank\" class=\"app_link\"><img src=\"https://cuplex.se/voc/images/btn_Appstore_iOS.png\" alt=\"Appstore link\" /></a></div><a href=\"/anonymous_requests/new\">Contact Us</a><a href=\"/access/unauthenticated\">Login</a></div>"
footer = footer.replace("<a href=\"/access/unauthenticated\">Login</a>", "<a href=\"/access/logout\">Sign out</a><a id=\"agent_link\" href=\"/agent\">Agent View</a>")  unless currentUser.isAnonymous
$j("#footer").empty().hide()
sidebar = $j("#sidebar")
ie8orLower = parseInt($j.browser.version, 10) <= 8
isOnFirstPage = false
jQuery.fn.center = ->
  @css "position", "absolute"
  @css "top", ($j(window).height() - @height()) / 2 + $j(window).scrollTop() + "px"
  @css "left", ($j(window).width() - @width()) / 2 + $j(window).scrollLeft() + "px"
  this

WebFontConfig = custom:
  families: ["volvo_sans_pro"]
  urls: ["https://cuplex.se/voc/fonts/fontstyles.css"]

(->
  wf = document.createElement("script")
  wf.src = ((if "https:" is document.location.protocol then "https" else "http")) + "://ajax.googleapis.com/ajax/libs/webfont/1/webfont.js"
  wf.type = "text/javascript"
  wf.async = "true"
  s = document.getElementsByTagName("script")[0]
  s.parentNode.insertBefore wf, s
)()
searchForm = "<form action=\"/categories/search\" id=\"searchform\" method=\"get\"><input id=\"suggestions_query\" name=\"suggestions_query\" type=\"text\" placeholder=\"Search in this category\"><input class=\"button search primary\" id=\"suggestion_submit\" name=\"commit\" type=\"submit\" value=\"\"><div style=\"margin:0;padding:0;display:inline\"><input name=\"utf8\" type=\"hidden\" value=\"&amp;#x2713;\"></div><input id=\"for_search\" name=\"for_search\" type=\"hidden\" value=\"1\"></form>"
if $j("#top:visible").size() > 0
  $j(window).scroll ->
    top_header_height = $j(".top_header").height()
    if sidebar.children().size() > 0 and sidebar.height() < $j(window).height()
      y = $j(this).scrollTop() - top_header_height
      sidebar.stop()
      y = 0  if y < 0
      sidebar.animate
        top: y + "px"
      , 1000, "swing"

  $j("a:contains(\"edit\")").bind "click", ->
    window.setTimeout "$j('#container').trigger('resize');", 250

  $j("#container").resize ->
    if (sidebar.position().top + sidebar.height()) > $j("#container").height()
      y = $j("#container").height() - sidebar.height()
      y = 0  if y < 0
      sidebar.stop()
      sidebar.css "top", y + "px"

Event.observe window, "load", ->
  if $j("#container #ticketform h2").size() > 0
    $j("#container #ticketform h2")[0].update "Submit a request for assistance."
    $j("#container #sidebar .side-box-content")[0].update "<h2><b>Information</b></h2><p>Fields marked with an asterisk (*) are mandatory. For faster help, be detailed in the description of your problem. </p><p>We operate Mon-Fri from 8.00-17.00 (CET) and will try to answer your question as quickly as possible. You will receive the answer to your question via email.</p> <p>Please note that this form is only for questions related to Volvo On Call. For other problems with your Volvo, we refer to your local dealer. We will answer requests written in English.</p> "

$j(document).ready ->
  flashMsg = $j("#flash > #notice")
  flashMsg.delay(2000).fadeIn 1000
  flashMsg.html flashMsg.html().replace(/Request #\d+ ".+" created/g, "Your request has been created and a confirmation email has been sent to you. If you would like to add any details, respond to this email")  if flashMsg and flashMsg.html()
  if Zendesk isnt null and Zendesk.tab is "home"
    $j("div#category_20085466 .column ul li.fade_truncation_outer.articles,div#category_20085466 .column ul li.fade_truncation_outer.questions").hide()
    $j("div#category_20085466 .column").css
      width: "auto"
      height: "auto"

    $j("#category_20085466").parent().addClass "faq_column"
    $j("#category_20074573").parent().addClass "using_voc"
    sidebar.hide()
    $j("h2#pinned_topics_title").replaceWith "<h2>Popular topics:</h2>"
    isOnFirstPage = true
  else
    if ie8orLower
      sidebar.css "left", sidebar.position().left + "px"
      sidebar.css "position", "absolute"
    else
      sidebar.css "position", "relative"
  if Zendesk isnt null
    if Zendesk.tab is "new"
      $j("#ticket_header").prepend "<a href=\"/home\">Home</a>&nbsp;/"
    else if Zendesk.tab is ""
      pattern = "/access/unauthenticated$"
      if document.location.toString().match(pattern)
        elem = $j(".content.content_grey").find("h2:contains('Log in to Volvo On Call Self Service')")
        elem.html elem.html().replace(/Log in to Volvo On Call Self Service/, "<a href=\"/home\">Home</a>&nbsp;/&nbsp;Log in to Volvo On Call Self Service")  if elem.size() > 0
  if $j("#top:visible").size() > 0
    $j("#top").html "<div class=\"top_header\"><div class=\"volvo_home_lnk\" onclick=\"document.location='/home'\"></div><div class=\"header_links\"><ul><li class=\"hide_on_home\"><a href=\"/home\">Home</a></li><li><a href=\"/entries/22987852-What-is-Volvo-On-Call-\">What is Volvo On Call?</a></li><li><a href=\"/entries/23171528-Get-started-with-Volvo-On-Call\">Get Started</a></li><li><a href=\"/forums/21614081-How-to\">User Guides</a></li><li><a href=\"/categories/20085466-FAQ-Forums\">FAQ Forums</a></li></ul></div><div class=\"top_search hide_on_home\"><form action=\"/categories/search\" id=\"menu_searchform\" method=\"get\"><input id=\"menu_suggestions_query\" name=\"suggestions_query\" type=\"text\" placeholder=\"Search\"><input class=\"button menu_search menu\" id=\"menu_suggestion_submit\" name=\"commit\" type=\"submit\" value=\"\"><div style=\"margin:0;padding:0;display:inline\"><input name=\"utf8\" type=\"hidden\" value=\"&amp;#x2713;\"></div>    <input id=\"for_search\" name=\"for_search\" type=\"hidden\" value=\"1\"> </form></div></div>"
    $j("#container").append footer
    $j("#flash_messages").addClass "flash_messages_class"
    if $j.browser.msie and parseInt($j.browser.version, 10) <= 7
      $j(".applink_button_wrapper").css "display", "inline"
      $j(".voc_footer").css "width", "1000px"
      $j(".faded_truncation").hide()  if parseInt($j.browser.version, 10) <= 6
    $j(".hide_on_home").hide()  if isOnFirstPage
    $j("p.button-item>.button.small:contains('Add Article')").text "Add new topic"
    jQuery.ajax
      url: "/users/current.xml"
      async: true
      dataType: "xml"
      success: (data, textStatus, jqXHR) ->
        role = parseInt($j(data).find("roles").text(), 10)
        $j("#agent_link").css "display", "inline-block"  if role is 2 or role is 4

  ticketDeflect = $j("#ticket-deflect")
  if ticketDeflect.size() > 0
    ticketDeflectTitle = ticketDeflect.prev()
    ticketDeflectTitle.removeAttr "style"
    ticketDeflectBox = $j("<div class=\"ticket_deflect_box contact_us\"><h2><a href=\"/anonymous_requests/new\">Contact Us</a></h2></div>")
    ticketDeflectBox.insertAfter ticketDeflectTitle
    ticketDeflectTitle.addClass "caps"
    ticketDeflectTitle.detach()
    ticketDeflect.remove()
    ticketDeflectBox.prepend ticketDeflectTitle
    $j(".frame").css "padding-bottom", "5px"
  $j("div.frame").find("strong:only-child").parent("p").addClass "pstrong"
  awaitAjaxUpdate $j("#voting_control")
  $j("img.image_zoom").bind "click", ->
    showModalImage $j(this).attr("src")