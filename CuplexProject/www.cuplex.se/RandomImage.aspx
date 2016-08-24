<%@ Page Title="" Language="C#" MasterPageFile="~/Default.master" AutoEventWireup="true" CodeFile="RandomImage.aspx.cs" Inherits="RandomImagePage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <script type="text/javascript">
        var activeImg = 1;
        var isChangingImage = false;
        $(document).ready(function() {
            $("#ImageUrlTextBox").click(function() { $(this).focus(); $(this).select(); });
            $("#ImageContainer").click(SetRandomImage);
            $("#Img2").fadeOut(1);
            SetRandomImage();
        });
    </script>
    <div id="RandomImageWrapper">
        <div id="ImageContainer">
            <img id="Img1" class="imgSwap" alt="" src="" />
            <img id="Img2" class="imgSwap" alt="" src="" />
        </div>
    </div>
    <div id="ImgInfoPanelWrapper">
        <div id="ImgInfoPanel">
            <div>
            <span class="label">Direktlänk: </span>
            <input type="text" id="ImageUrlTextBox" class="ImageUrlTextBox" readonly="readonly" />
            
            <div class="lightPadding">
                <asp:Button ID="PrevButton" CssClass="randImgButton" OnClientClick="ShowPreviousImage();return false;" runat="server" />
                <asp:Button ID="NextButton" CssClass="randImgButton" OnClientClick="SetRandomImage();return false;" runat="server" />
            </div>
            </div>
        </div>
    </div>
</asp:Content>