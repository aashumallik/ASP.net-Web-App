
//*************************************************************************************************************
//* Help Message Popup
//*************************************************************************************************************
function helpMessagePopup(message) {
    var divHelpMessage = document.getElementById("fsMessage");
    var curX = event.clientX;
    var curY = event.clientY;

    divHelpMessage.style.left = (curX + 10) + "px";
    divHelpMessage.style.top = (curY - 16) + "px";
    divHelpMessage.innerHTML = message;    
    divHelpMessage.style.visibility = "visible";
}



function helpMessageClose() {
    var divHelpMessage = document.getElementById("fsMessage");
    divHelpMessage.style.visibility = "hidden";
}