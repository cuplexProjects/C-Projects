(function() {
  var Main,
    __bind = function(fn, me){ return function(){ return fn.apply(me, arguments); }; };

  Main = (function() {
    function Main() {
      this.mainMenuSetPage = __bind(this.mainMenuSetPage, this);
      this.generateAlphanumericRandom = __bind(this.generateAlphanumericRandom, this);
    }

    Main.prototype.init = function() {
      $("#htmlencode-input-button").bind("click", this.htmlEncodeText);
      $("#alphanumeric-random-button").bind("click", this.generateAlphanumericRandom);
      return $("#top-navbar").find(".dropdown-menu").bind("click", this.mainMenuSetPage);
    };

    Main.prototype.htmlEncodeText = function(event) {
      var postData;
      postData = new Object();
      postData.Content = $("#htmlencode-input").val();
      return $.post(apiBaseURL + "utility/htmlEncode/", postData, "json").done(function(data) {
        return $("#htmlencode-output").val(data);
      });
    };

    Main.prototype.generateAlphanumericRandom = function(event) {
      var length;
      length = parseInt($("#alphanumeric-random-length").val());
      return $.get(apiBaseURL + "randomdata/generateAlphanumericRandom/" + length, function(data) {
        return $("#alphanumeric-random-output").val(data.replace(/\"/g, ''));
      });
    };

    Main.prototype.mainMenuSetPage = function(event) {
      var menuItem;
      menuItem = $(event.target).parents("ul");
      $(".page-container").find(".page").removeClass("active");
      switch (menuItem.attr("id")) {
        case "page-menu-1":
          $("#converters-page").addClass("active");
          break;
        case "page-menu-2":
          $("#random-data-page").addClass("active");
          break;
        case "page-menu-3":
          $("#geo-location-page").addClass("active");
          break;
        case "page-menu-4":
          $("#ip-lookup-page").addClass("active");
      }
      return console.log(menuItem);
    };

    return Main;

  })();

  jQuery(function() {
    window.main = new Main();
    return window.main.init();
  });

}).call(this);

//# sourceMappingURL=application.js.map
