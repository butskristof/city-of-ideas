function showOrganisatie() {
    // Get the checkbox
    let checkBox = document.getElementById("cb-organisatie");
    // Get the output text
    let text = document.getElementById("div1");

    // If the checkbox is checked, display the output text
    if (checkBox.checked === true){
        text.style.display = "block";
    } else {
        text.style.display = "none";
    }
}