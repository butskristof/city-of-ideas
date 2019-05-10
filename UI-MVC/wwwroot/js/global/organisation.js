import Page from "../util/page";

Page.query("#circleAntwerpen").addEventListener("click", function () {
    document.getElementById("theImageAntwerpen").style = "background-image: url('/img/pexels-photo-167676.jpeg')";
});

Page.query("#circleBrussel").addEventListener("click", function () {
    document.getElementById("theImageAntwerpen").style = "background-image: url('/img/brussel.jpeg')";
});

