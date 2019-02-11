# CoffeeScript
class Main
   
    init:() ->        
        $("#htmlencode-input-button").bind("click", @htmlEncodeText)            
        $("#alphanumeric-random-button").bind("click", @generateAlphanumericRandom)            
        $("#top-navbar").find(".dropdown-menu").bind("click", @mainMenuSetPage)

    htmlEncodeText:(event) ->
        postData = new Object()
        postData.Content =  $("#htmlencode-input").val()  

        $.post(apiBaseURL + "utility/htmlEncode/", postData, "json").done (data)->
            $("#htmlencode-output").val(data)


    generateAlphanumericRandom: (event) =>
        length = parseInt($("#alphanumeric-random-length").val());        

        $.get(apiBaseURL + "randomdata/generateAlphanumericRandom/"+length, (data)->
            $("#alphanumeric-random-output").val(data.replace(/\"/g, '')))

    mainMenuSetPage: (event) =>
        menuItem = $(event.target).parents("ul");
        
        $(".page-container").find(".page").removeClass("active")
        
        switch menuItem.attr("id")
            when "page-menu-1" 
                $("#converters-page").addClass("active")
            when "page-menu-2" 
                $("#random-data-page").addClass("active")
            when "page-menu-3" 
                $("#geo-location-page").addClass("active")
            when "page-menu-4" 
                $("#ip-lookup-page").addClass("active")            
        
        console.log(menuItem)

jQuery ->
  window.main = new Main();
  window.main.init() 