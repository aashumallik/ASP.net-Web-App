
//SecurityService_JS.js///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

//ENUM for permission types:
var PermissionType = {
    CLOCK_CARD: { value: 1 },
    WINDOWS_USER: { value: 2 },
    AD_GROUP: { value: 3 },
    JOB_CLASS: { value: 4 },
    DEPARTMENT: { value: 5 },
    RFID_BADGE: { value: 6 },
    BUSINESS_AREA: { value: 7 },
    UID: { value: 8 },
    JOB_CATEGORY: { value: 9 }
};
//ENUM for TeammateType types:
var TeammateType = {
    CLOCK_CARD: { value: 1 },
    WINDOWS_USER: { value: 2 },
    AD_GROUP: { value: 3 },
    JOB_CLASS: { value: 4 },
    DEPARTMENT: { value: 5 },
    RFID_BADGE: { value: 6 },
    BUSINESS_AREA: { value: 7 },
    UID: { value: 8 },
    JOB_CATEGORY: { value: 9 }
};


//Is application configured in the Web.config for the security service?
function isConfiguredToUseService() {

    var flag = false;
    $.ajax({
        type: 'POST',
        url: 'Services/SecurityService.asmx/isConfiguredToUseService',
        async: false,
        contentType: 'application/json; charset=utf-8',
        global: true,
        success: function (response) {
            flag = response.d;
        },
        error: function (error) {
            alert("An error has occurred trying to determine if security is configured.");
        }
    });

    return flag;

}

//call to SecurityService.CS to set the currentTeammate in session/cookie
function setCurrentTeammate(value, Teammatetype) {

    var retVal = false;

    $.ajax({
        type: 'POST',
        url: 'Services/SecurityService.asmx/setCurrentTeammate',
        async: false,
        dataType: 'json',
        data: JSON.stringify({ value: value, Teammatetype: Teammatetype.value }),
        contentType: 'application/json; charset=utf-8',
        global: true,
        success: function (response) {
            retVal = response.d;
        },
        error: function (error) {
            alert("An error has occurred trying to set the current teammate");
            retVal = false;
        }
    });

    return retVal;

}

//call to SecurityService.CS to get the current Permission in session/cookie
function setCurrentPermissions(value, PermissionType, appId) {

    var retVal = false;

    $.ajax({
        type: 'POST',
        url: 'Services/SecurityService.asmx/setCurrentPermissions',
        async: false,
        dataType: 'json',
        data: JSON.stringify({ value: value, PermissionType: PermissionType.value, appId: appId }),
        contentType: 'application/json; charset=utf-8',
        global: true,
        success: function (response) {
            retVal = response.d;
        },
        error: function (error) {
            alert("An error has occurred trying to set the current permissions");
            retVal = false;
        }
    });

    return retVal;

}

//setCurrentDropDownSelection
//call to SecurityService.CS to set the current drop down list to a permission they have chosen
function setCurrentDropDownSelection(value) {

    $.ajax({
        type: 'POST',
        url: 'Services/SecurityService.asmx/setCurrentDropDownSelection',
        async: false,
        dataType: 'json',
        data: JSON.stringify({ value: value }),
        contentType: 'application/json; charset=utf-8',
        global: true,
        success: function (response) {
        },
        error: function (error) {
            alert("An error has occurred trying to set the current drop down list");
        }
    });

}

//call to SecurityService.CS to get the currentTeammate in session/cookie
function getCurrentTeammate() {

    var Teammate;

    $.ajax({
        type: 'POST',
        url: 'Services/SecurityService.asmx/getCurrentTeammate',
        async: false,
        dataType: 'json',
        contentType: 'application/json; charset=utf-8',
        global: true,
        success: function (response) {
            Teammate = response;
        },
        error: function (error) {
            //alert("An error has occurred trying to get the current teammate");
        }
    });

    if (Teammate == null) {
        return null;
    } else {
        return Teammate.d;
    }


}


//call to SecurityService.CS to get the currentTeammate in session/cookie
function getCurrentTeammateIFRAME() {

    var Teammate;

    $.ajax({
        type: 'POST',
        url: 'Services/SecurityService.asmx/getCurrentTeammateIFRAME',
        async: false,
        dataType: 'json',
        contentType: 'application/json; charset=utf-8',
        global: true,
        success: function (response) {
            Teammate = response;
        },
        error: function (error) {
            //alert("An error has occurred trying to get the current teammate");
        }
    });

    if (Teammate == null) {
        return null;
    } else {
        return Teammate.d;
    }


}


//call to SecurityService.CS to get the current Permission in session/cookie
function getCurrentPermissions() {

    var Permissions;

    $.ajax({
        type: 'POST',
        url: 'Services/SecurityService.asmx/getCurrentPermissions',
        async: false,
        dataType: 'json',
        contentType: 'application/json; charset=utf-8',
        global: true,
        success: function (response) {
            Permissions = response;
        },
        error: function (error) {
            alert("An error has occurred trying to get the current permissions - getCurrentPermissions");
        }
    });

    return Permissions.d;

}

//call to SecurityService.CS to get the current Permission in session/cookie
function getCurrentPermissionsIFRAME() {

    var Permissions;

    $.ajax({
        type: 'POST',
        url: 'Services/SecurityService.asmx/getCurrentPermissionsIFRAME',
        async: false,
        dataType: 'json',
        contentType: 'application/json; charset=utf-8',
        global: true,
        success: function (response) {
            Permissions = response;
        },
        error: function (error) {
            alert("An error has occurred trying to get the current permissions - getCurrentPermissions");
        }
    });

    return Permissions.d;

}


//call to SecurityService.CS to get the current Permission in session/cookie
function getCurrentPermissionsForDDL() {

    var Permissions;

    $.ajax({
        type: 'POST',
        url: 'Services/SecurityService.asmx/getCurrentPermissionsForDDL',
        async: false,
        dataType: 'json',
        contentType: 'application/json; charset=utf-8',
        global: true,
        success: function (response) {
            Permissions = response;
        },
        error: function (error) {
            alert("An error has occurred trying to get the current permissions -getCurrentPermissionsForDDL");
        }
    });

    return Permissions.d;

}


//call to SecurityService.CS to get the current selection if one has been set..
function getCurrentSelection() {

    var Permissions;

    $.ajax({
        type: 'POST',
        url: 'Services/SecurityService.asmx/getCurrentDropDownSelection',
        async: false,
        dataType: 'json',
        contentType: 'application/json; charset=utf-8',
        global: true,
        success: function (response) {
            Permissions = response;
        },
        error: function (error) {
            alert("An error has occurred trying to get the current permissions - getCurrentDropDownSelection");
        }
    });

    return Permissions.d;

}


//call to SecurityService.CS to get other teammate info
function getOtherTeammate(value, Teammatetype) {

    var Teammate;

    $.ajax({
        type: 'POST',
        url: 'Services/SecurityService.asmx/getOtherTeammate',
        async: false,
        dataType: 'json',
        data: JSON.stringify({ value: value, Teammatetype: Teammatetype.value }),
        contentType: 'application/json; charset=utf-8',
        global: true,
        success: function (response) {
            Teammate = response;
        },
        error: function (error) {
            alert("An error has occurred trying to get other teammate(s)");
        }
    });

    return Teammate.d;

}


// begin Impersonation
function beginImpersonation(user, PermissionType, appId, ImpersonatedPermission) {

    $.ajax({
        type: 'POST',
        url: 'Services/SecurityService.asmx/beginImpersonation',
        async: false,
        dataType: 'json',
        data: JSON.stringify({ user: user, pt: PermissionType.value, appId: appId, ImpersonatedPermission: ImpersonatedPermission }),
        contentType: 'application/json; charset=utf-8',
        global: true,
        success: function (response) {
        },
        error: function (error) {
            alert("An error has occurred trying to begin impersonation");
        }
    });

}


// end Impersonation
function endImpersonation(user, appId, ImpersonatedPermission) {

    $.ajax({
        type: 'POST',
        url: 'Services/SecurityService.asmx/endImpersonation',
        async: false,
        dataType: 'json',
        data: JSON.stringify({ user: user, appId: appId, ImpersonatedPermission: ImpersonatedPermission }),
        contentType: 'application/json; charset=utf-8',
        global: true,
        success: function (response) {
        },
        error: function (error) {
            alert("An error has occurred trying to end impersonation");
        }
    });

}



// log out IE: kill all current session and cookie data
function logOut() {

    $.ajax({
        type: 'POST',
        url: 'Services/SecurityService.asmx/logOut',
        async: false, //this is make it wait on service response before continuing
        dataType: 'json',
        contentType: 'application/json; charset=utf-8',
        global: true,
        success: function (response) {
        },
        error: function (error) {
            alert("An error has occurred trying to log out");
        }
    });

}



//Load the initial security items for the template
function LoadTemplateSecurityDDL() {

    //add impersonation on change event to the security ddl
    $("#DefaultPage_Header_ddlSecurity").change(function () {
        setCurrentDropDownSelection(document.getElementById("DefaultPage_Header_ddlSecurity").value);
        //This will determine wheter or not to show the Manage Security Menu Item
        var getCurrentlySetPermissions = getCurrentPermissions(); //get the impersonated permission if one
        var CanManageLowerPermissionsFlag = false;

        if (getCurrentlySetPermissions != null) {
            for (var i = 0; i < getCurrentlySetPermissions.length; i++) {
                if (getCurrentlySetPermissions[i].CanManageLowerPermissions == "True") {
                    CanManageLowerPermissionsFlag = true;
                    break;
                }
            }
        }

        //Show if flag is true.. try catch because it wont work in PA and default page, as control isnt there
        if (CanManageLowerPermissionsFlag == true) {
            try {
                document.getElementById("SSMC_Assignment").style.display = "inline";
            } catch (e) {
                alert("LoadTemplateSecurityDDL() SSMC_Assignment element.. " + e);
            }
        }
        else {
            try {
                document.getElementById("SSMC_Assignment").style.display = "none";
            } catch (e) {
                alert("LoadTemplateSecurityDDL() SSMC_Assignment element.. " + e);
            }

        }

        //Now if on main page, don't reload screen, otherwise do to set security on objects
        if (window.location.href.substring(window.location.href.lastIndexOf('/') + 1) == '') { //we have main page
            //do squat           
        }
        else {
            //go ahead and reload
            window.location.reload();
        }

    });

    //set width of security ddl so it's consistent regardless of char count of the permission..
    $("#DefaultPage_Header_ddlSecurity").width(110);

    //get current permission object and populate the security ddl
    var currentPermissionObjectDDL = getCurrentPermissionsForDDL(); //this will get data from encrypted cookie

    //prevent duplicates
    if (currentPermissionObjectDDL != null) {
        try {
            var permDDL = [];
            var addflag;
            for (var i = 0; i < currentPermissionObjectDDL.length; i++) {
                addflag = true;
                for (var a = 0; a < permDDL.length; a++) {
                    if (permDDL[a] == currentPermissionObjectDDL[i].Permission)
                        addflag = false;
                }
                if (addflag == true) {
                    permDDL.push(currentPermissionObjectDDL[i].Permission);
                }
            }

        } catch (e) {
            //we have lost our session or cookies etc.. Send them back to to default.aspx
            document.location.href = "/";
        }


        //This will determine wheter or not to show the Manage Security Menu Item
        var getCurrentlySetPermissions = getCurrentPermissions(); //get the impersonated permission if one
        var CanManageLowerPermissionsFlag = false;

        if (getCurrentlySetPermissions != null) {
            for (var i = 0; i < getCurrentlySetPermissions.length; i++) {
                if (getCurrentlySetPermissions[i].CanManageLowerPermissions == "True") {
                    CanManageLowerPermissionsFlag = true;
                    break;
                }
            }
        }

        //Show if flag is true.. try catch because it wont work in PA and default page, as control isnt there
        if (CanManageLowerPermissionsFlag == true) {
            try {
                document.getElementById("SSMC_Assignment").style.display = "inline";
            } catch (e) {

            }
        }
        else {
            try {
                document.getElementById("SSMC_Assignment").style.display = "none";
            } catch (e) {

            }

        }


        try {

            //populate the ddl
            var permOptions = "";
            for (var i = 0; i < permDDL.length; i++) {
                permOptions += "<option value='" + permDDL[i] + "'>" + permDDL[i] + "</option>";
            }

            document.getElementById("DefaultPage_Header_ddlSecurity").innerHTML = permOptions;

            //now try and show the chosen selection from the user. If none is chosen then it will load the default highest permission
            var currentSelection = getCurrentSelection();
            if (currentSelection !== null || currentSelection != "") {
                document.getElementById("DefaultPage_Header_ddlSecurity").value = currentSelection//[i].Permission;
            }

            //update the welcome label..
            var Teammate = getCurrentTeammate();
            document.getElementById("DefaultPage_Header_lblUser").innerText = "Welcome " + Teammate.firstName + " " + Teammate.lastName;

            //alert(document.getElementById('ctl08_ctl01_lblUser').innerText);

        } catch (e) {

        }

    }

}




function getCurrentTeammateValue() {

    var value;

    $.ajax({
        type: 'POST',
        url: 'Services/SecurityService.asmx/getCurrentTeammateValue',
        async: false,
        contentType: 'application/json; charset=utf-8',
        global: true,
        success: function (response) {
            value = response.d;
        },
        error: function (error) {
            alert("An error has occurred trying to set the current drop down list");
        }
    });

    return value;


}


function getCurrentTeammateType() {

    var value;
    $.ajax({
        type: 'POST',
        url: 'Services/SecurityService.asmx/getCurrentTeammateType',
        async: false,
        contentType: 'application/json; charset=utf-8',
        global: true,
        success: function (response) {
            value = response.d;
        },
        error: function (error) {
            alert("An error has occurred trying to set the current drop down list");
        }
    });

    return value;

}


//Is application configured in the Web.config for the security service?
function GetDateTimeStringURLforIFRAME() {

    var urlString = "";
    $.ajax({
        type: 'POST',
        url: 'Services/SecurityService.asmx/GetDateTimeStringURLforIFRAME',
        async: false,
        contentType: 'application/json; charset=utf-8',
        global: true,
        success: function (response) {
            urlString = response.d;
        },
        error: function (error) {
            alert("An error has occurred trying to GetDateTimeStringURLforIFRAME");
        }
    });

    return urlString;

}


//Shawn
//Is application configured in the Web.config for the security service?
function getPermissionLevelsForApplication(appId) {

    var retVal;
    //set Permission object
    $.ajax({
        type: 'POST',
        url: 'Services/SecurityService.asmx/getPermissionLevelsForApplication',
        async: false,
        data: JSON.stringify({ appId: appId }),
        contentType: 'application/json; charset=utf-8',
        global: true,
        success: function (response) {
            retVal = response.d;
        },
        error: function (error) {
            alert("An error has occurred trying to GetDateTimeStringURLforIFRAME");
        }
    });

    return retVal;

}



function setIFrameData(url, appId) {

    //if setup complete, begin frame creation, otherwise don't do anything
    if (!url || !appId) {
        //one is null, so don't do anything with the frame...
        return;
    }
    else {
        //grab a few things and build url (src) for IFrame
        var teammateType = getCurrentTeammateType();
        var teammateValue = getCurrentTeammateValue();
        var dVal = GetDateTimeStringURLforIFRAME();
        //build ending to url
        var ending = "&tType=" + teammateType + "&tVal=" + teammateValue + "&dVal=" + dVal;

        iframeSrc = url + appId + ending;
    }

    //var iframe = $('<iframe id="SSMCIFrame" scrolling="no" frameborder="0" marginwidth="0" marginheight="0" style="margin-left: -250px; margin-top: -110px;"></iframe>');
    var iframe = $('<iframe id="SSMCIFrame" scrolling="no" frameborder="0" marginwidth="0" marginheight="0" style="margin-left: 0px; margin-top: 0px;"></iframe>');
    var dialog = $("<div></div>").append(iframe).appendTo("body").dialog({
        autoOpen: false,
        modal: true,
        resizable: false,
        width: "auto",
        height: "auto",
        close: function () {
            iframe.attr("src", "");
        }
    });
    $(".SSMCClass").on("click", function (e) {
        e.preventDefault();
        var src = iframeSrc;
        var title = 'Security Service Management Console';
        var width = 1200;
        var height = 800;
        iframe.attr({
            width: +width,
            height: +height,
            src: src
        });
        dialog.dialog("option", "title", title).dialog("open");
    });


};



/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////