var LinkNames = [];

function CallDetails(type, id) {
    type = type + "s";
    window.location = window.location.origin + "/" + type + "/Details/" + id;
}

function CallNotificationEdit(id) {
    window.location = window.location.origin + "/Notifications/Edit/" + id;
}
function checkECOUserInputs(x) { //this will check for the inputs of the user
    if (x.id == "btnSubmit") {
        $("#Status option").removeAttr("selected");//reset the status option
        $("#Status option[value='3']").attr("selected", "selected");//set the status to the option 3, which is awaiting approval
    }
    else if (x.id == "btnSave") {//same thing as above
        $("#Status option").removeAttr("selected");
        $("#Status option[value='0']").attr("selected", "selected"); //but sets the status option to 0, which is Draft
    }

    var aList = document.getElementsByName("ApproversList"); //first get all elements with the ApproversList name
    var ValidationDiv = $('#validationDiv'); //gets the ValidationDiv, this will be used to append the items missing
    var list = document.createElement('ul', { id: "validationOrderedList", class: "list-inline" }); //creates an <ul>
    var areasChecked = null;
    var deviationChecked = null;
    //this will check for the checkbox for the affected areas, returning true if they are selected or null if not;
    $(".AreaCheckbox").each(function () { if ($(this).prop("checked")) { areasChecked = true; return false; } else { areasChecked = null; } })
    $(".Deviation").each(function () { if ($(this).prop("checked")) { deviationChecked = true; return false; } else { deviationChecked = null; } })
    var Fields = { //these are the fields that will be checked.
        "Change Type": $("#ChangeTypeId").val() !== null ? $("#ChangeTypeId").val() : null, //simple check if the value is null, returning the value or null
        "Priority Level": $("#PriorityLevel").val() !== null ? $("#PriorityLevel").val() : null,
        "Implementation Type": $("#ImplementationType").val() !== null ? $("#ImplementationType").val() : null,
        "Planned Implementation Date": new Date($("#PlannedImplementationDate").val()) > Date.now() ? $("#PlannedImplementationDate").val() : null,
        "Permanent Change": $("#PermanentChange").val(), //true or false for the following:
        "BOM Required": $("#BOMRequired").val(),
        "Product Validation Testing Required": $("#ProductValidationTestingRequired").val(),
        "Customer Approval Required": $("#CustomerApproval").val(),
        "Description": $("#Description").val(), //text fields
        "Reason For Change": $("#ReasonForChange").val(),
        //"Links": $("#URLRepresentation").children().length > 0 ? $("#URLRepresentation").children().length : null, //counts the list of links in the representation div
        "Areas Affected": areasChecked, //if any Area has been checked, return the value or null
        "Approvers": $(aList).find("option:selected").length, //gets how many approvers were selected
        "Notifications": $('#UsersToBeNotified').val().length > 0 ? $("#UsersToBeNotified").val().length : null // same as above but with notifications, returning null if < 0
    };
    console.log(Fields["Areas Affected"])

    if (deviationChecked) {
        if (new Date($("#DeviationDate").val()) > Date.now()) {

        } else {

            var DeviationItem = document.createElement('li');
            DeviationItem.append(document.createTextNode("Deviation Date "))
            list.append(DeviationItem)
        }
        if ($('#DeviationQuantity').val().length > 0) {

        } else {
            var DeviationItem = document.createElement('li');
            DeviationItem.append(document.createTextNode("Deviation Quantity "))
            list.append(DeviationItem)
        }
    }

    for (var field in Fields) { //loops through the fields
        if (Fields.hasOwnProperty(field)) {
            var fieldValue = Fields[field]; //the value in the field
            if (fieldValue == null || fieldValue == undefined || fieldValue == "" || fieldValue == "null") { //checks for null, undefined, empty and "null"
                var listItem = document.createElement('li', { class: "list-group-item" }); //creates an item for the list
                listItem.append(document.createTextNode(field)); //creates a text node for the item with the field name
                list.append(listItem); //appends the item to the list
            }
            else {
            }
        }
    }
    for (var reqApprover in requiredApprovers) {
        if ($("select[id='" + reqApprover + "']").val().length > 0) { //checks if the required approvers have been selected
        }
        else { //or display an alert in the list
            var approverListItem = document.createElement('li');
            approverListItem.append(document.createTextNode("Approvers for " + reqApprover))
            list.append(approverListItem)
        }
    }
    if (list.hasChildNodes()) { //if the list has any items inside it
        ValidationDiv.html("The following fields do not have a value:");
        ValidationDiv.append(list);
        window.scrollTo(0, 0);
    }
    else {
        $('#formCreate').submit(); //if it doesn't have items, means it is good to be sent to the controller
    }
}
function checkECRUserInputs(x) {
    if (x.id == "btnSubmit") {
        $("#Status option").removeAttr("selected");
        $("#Status option[value='3']").attr("selected", "selected");
    }
    else if (x.id == "btnSave") {
        $("#Status option").removeAttr("selected");
        $("#Status option[value='0']").attr("selected", "selected");
    }

    var aList = document.getElementsByName("ApproversList");
    var areasChecked = null;
    //this will check for the checkbox for the affected areas, returning true if they are selected or null if not;
    $(".AreaCheckbox").each(function () { if ($(this).prop("checked")) { areasChecked = true; return false; } else { areasChecked = null; } })
    var ValidationDiv = $('#validationDiv');
    var list = document.createElement('ul', { id: "validationOrderedList", class: "list-inline" });
    console.log(new Date($("#PlannedImplementationDate").val()))
    console.log(Date.now())
    var Fields = {
        "Change Type": $("#ChangeTypeId").val() !== null ? $("#ChangeTypeId").val() : null,
        "Priority Level": $("#PriorityLevel").val() !== null ? $("#PriorityLevel").val() : null,
        "Implementation Type": $("#ImplementationType").val() !== null ? $("#ImplementationType").val() : null,
        "Planned Implementation Date": new Date($("#PlannedImplementationDate").val()) > Date.now() ? $("#PlannedImplementationDate").val() : null,
        "Permanent Change": $("#PermanentChange").val(),
        "BOM Required": $("#BOMRequired").val(),
        "Product Validation Testing Required": $("#ProductValidationTestingRequired").val(),
        "Customer Impact": $("#CustomerImpact").val(),
        "Description": $("#Description").val(),
        "Reason For Change": $("#ReasonForChange").val(),
        //"Links": $("#URLRepresentation").children().length > 0 ? $("#URLRepresentation").children().length : null,
        "Areas Affected": areasChecked,
        "Approvers": $(aList).find("option:selected").length,
        "Notifications": $('#UsersToBeNotified').val().length > 0 ? $("#UsersToBeNotified").val().length : null
    };
    console.log(Fields["Areas Affected"])
    for (var field in Fields) {
        if (Fields.hasOwnProperty(field)) {
            var fieldValue = Fields[field];
            if (fieldValue == null || fieldValue == undefined || fieldValue == "" || fieldValue == "null") {
                var listItem = document.createElement('li', { class: "list-group-item" });
                listItem.append(document.createTextNode(field));
                list.append(listItem);
            } else {
            }
        }
    }
    for (var reqApprover in requiredApprovers) {
        if ($("select[id='" + reqApprover + "']").val().length > 0) {

        }
        else {
            var approverListItem = document.createElement('li');
            approverListItem.append(document.createTextNode("Approvers for " + reqApprover))
            list.append(approverListItem)
        }
    }
    if (list.hasChildNodes()) {
        ValidationDiv.html("The following fields do not have a value:");
        ValidationDiv.append(list);
        window.scrollTo(0, 0);
    }
    else {
        $('#formCreate').submit();
    }
}
function RejectPopup() {
    try {
        $('#myModal').modal('show')
    } catch (error) {
        prompt(error)
    }
}
function PopulateReason() {
    try {
        var reason = $('#approverejectReason').val()
        $('#RejectReason').val(reason)
    }
    catch (error) {
        prompt(error)
    }
}
//Approver popup
function ApproverPopup() {
    try {
        $('#approvalModal').modal('show')
    } catch (error) {
        prompt(error)
    }
}
function PopulateApproverReason() {
    try {
        var reason = $('#approverReason').val()
        $('.ApproverNote').val(reason)
    }
    catch (error) {
        prompt(error)
    }
}
//
function checkECNUserInputs(x) {
    if (x.id == "btnSubmit") {
        $("#Status option").removeAttr("selected");
        $("#Status option[value='3']").attr("selected", "selected");
    }
    else if (x.id == "btnSave") {
        $("#Status option").removeAttr("selected");
        $("#Status option[value='0']").attr("selected", "selected");
    }

    var aList = document.getElementsByName("UserRoleIds");
    console.log(aList);
    //this will check for the checkbox for the affected areas, returning true if they are selected or null if not;
    $(".AreaCheckbox").each(function () { if ($(this).prop("checked")) { areasChecked = true; return false; } else { areasChecked = null; } })
    var ValidationDiv = $('#validationDiv');
    var list = document.createElement('ul', { id: "validationOrderedList", class: "list-inline" });
    var Fields = {
        "Change Type": $("#ChangeTypeId").val() !== null ? $("#ChangeTypeId").val() : null,
        "Model Name": $("#ModelName").val() !== null ? $("#ModelName").val() : null,
        "Model Number": $("#ModelNumber").val() !== null ? $("#ModelNumber").val() : null,
        "Date Of Notice": new Date($("#DateOfNotice").val()) !== "Invalid Date" ? $("#DateOfNotice").val() : null,
        "PTCRB Resubmission Required": $("#PTCRBResubmissionRequired").val(),
        "Description": $("#Description").val(),
        "Impact of Missing Required Approval Date": $("#ImpactMissingReqApprovalDate").val(),
        "Approvers": $(aList).find("option:selected").length,
    };
    console.log(Fields["Areas Affected"])
    for (var field in Fields) {
        if (Fields.hasOwnProperty(field)) {
            var fieldValue = Fields[field];
            if (fieldValue == null || fieldValue == undefined || fieldValue == "" || fieldValue == "null") {
                var listItem = document.createElement('li', { class: "list-group-item" });
                listItem.append(document.createTextNode(field));
                list.append(listItem);
            } else {
            }
        }
    }
    for (var reqApprover in requiredApprovers) {
        if ($("select[id='" + reqApprover + "']").val().length > 0) {

        }
        else {
            var approverListItem = document.createElement('li');
            approverListItem.append(document.createTextNode("Approvers for " + reqApprover))
            list.append(approverListItem)
        }
    }
    if (list.hasChildNodes()) {
        ValidationDiv.html("The following fields do not have a value:");
        ValidationDiv.append(list);
        window.scrollTo(0, 0);
    }
    else {
        $('#formCreate').submit();
    }
}

function AddLink() {
    var url = "";
    url = $("#linkURL").val(); //get the val
    if (url != "") {
        if (!url.match(/^(http:\/\/www\.|https:\/\/www\.|http:\/\/|https:\/\/)[a-z0-9]+([\-\.]{1}[a-z0-9]+)*\.[a-z]{2,5}(:[0-9]{1,5})?(\/.*)?$/gim)) { //check if url is really an url
            $("#urlValidation").text("This link is not a valid url. It should follow this pattern: https://www.example.com");
        }
        else {
            var linkName = $("#linkName").val(); //get the name of the link
            if (linkName != "") {
                var nameExists = LinkNames.find(function (e) {
                    return e == linkName
                })
                if (nameExists == undefined) { //if the name is not in the array
                    $("#urlValidation").text("");
                    var input = document.createElement('input'); //create an input that will be in the POST
                    input.name = "LinkUrls[" + linkName + "]" //dictonary of names and links
                    input.value = url; //the value that will be sent
                    input.type = "hidden" //don't show the input

                    var listItem = document.createElement('li'); //create the list item
                    listItem.className = "list-group-item"; //add class

                    var spanText = document.createElement('span'); //create the span
                    spanText.textContent = linkName + ": "; //add the link name to it

                    var anchor = document.createElement("a"); //create the anchor
                    anchor.href = url; //href => where the link points to
                    anchor.textContent = url; //the text of it 
                    anchor.target = '_blank'; //open in a new tab
                    var i = document.createElement('i'); //X icon
                    i.className = "glyphicon glyphicon-remove" //glyphicon-remove == X
                    i.setAttribute('onclick', 'removeUrl(this)') //onclick to call the removeurl

                    listItem.append(spanText); //append the elements to the listitem
                    listItem.append(anchor);
                    listItem.append(i);
                    listItem.append(input);

                    $("#URLRepresentation").append(listItem); //append the listitem
                    LinkNames.push(linkName); //add the linkname to the array
                    $("#linkURL").val("");
                    $("#linkName").val("");
                }
                else {
                    $("#urlValidation").text("A link with this Name has already been added."); //message to the user
                }
            }
            else {
                $("#urlValidation").text("Name cannot be empty."); //message to the user
            }
        }
    }
}

$('document').ready(function () {

    //style the table rows:
    var rows = document.getElementsByClassName("status");

    for (var i = 0; i < rows.length; i++) {
        row = rows[i];
        if (row.textContent.trim() == "Approved") {
            row.style.backgroundColor = "#98fb98"; //if it was approved, change the background
        }
        else if (row.textContent.trim().includes("Rejected")) {
            row.style.backgroundColor = "#fb9898"; //if the change has been rejected, change the background
        }
    }

    var observe;
    if (window.attachEvent) {
        observe = function (element, event, handler) {
            element.attachEvent('on' + event, handler);
        };
    }
    else {
        observe = function (element, event, handler) {
            element.addEventListener(event, handler, false);
        };
    }
    $(function () {
        $('textarea[data-editor="ck"]').each(function () {
            autoresize(this.id);
        });
    });
    function autoresize(x) {
        var text = document.getElementById(x);
        function resize() {

            text.style.height = 'auto';
            text.style.height = text.scrollHeight + 'px';
        }
        /* 0-timeout to get the already changed text */
        function delayedResize() {
            window.setTimeout(resize, 0);
        }
        observe(text, 'change', resize);
        observe(text, 'cut', delayedResize);
        observe(text, 'paste', delayedResize);
        observe(text, 'drop', delayedResize);
        observe(text, 'keydown', delayedResize);

        text.focus();
        text.select();
        resize();
        deselectAll();
    }
    function deselectAll() {
        var element = document.activeElement;

        if (element && /INPUT|TEXTAREA/i.test(element.tagName)) {
            if ('selectionStart' in element) {
                element.selectionEnd = element.selectionStart;
            }
            element.blur();
        }

        if (window.getSelection) { // All browsers, except IE <=8
            window.getSelection().removeAllRanges();
        } else if (document.selection) { // IE <=8
            document.selection.empty();
        }
    }

    var ImplementationDate = document.getElementById("PlannedImplementationDate");
    if (ImplementationDate) {
        ImplementationDate.min = new Date().toISOString().split("T")[0];
    }
    jQuery('[data-confirm]').click(function (e) {
        if (!confirm(jQuery(this).attr("data-confirm"))) {
            e.preventDefault();
        }
    });

    $(':disabled').css("backgroundColor", "transparent").css("cursor", "default");//.css("backgroundColor", "#f9f9f9");

    //$(".InputSearchField").on("focus", function () {
    //    $(this).animate({
    //        width: "+=50px"
    //    }, 500);
    //})
    //$(".InputSearchField").on("blur", function () {
    //    $(this).animate({
    //        width: "-=50px"
    //    }, 500);
    //})

    $(".selectDevices").click(function () { //when one of the GO7/GO8/GO9 buttons is clicked, do this 
        var span = $(this);
        span.toggleClass("btn-default btn-primary"); //toggle one of the classes to change the backgroung


        var divDevices = span.parentsUntil("div").parent(); //get the div with all devices

        var spanText = $(this).text().trim(); //get the text of the button
        var allCheckboxes = divDevices.find(".checkboxProduct"); //get the checkboxes that belong to the devices div

        allCheckboxes.each(function (index, element) { //foreach checkbox
            var checkboxText = $(element).next().text().trim(); //get the text
            if (checkboxText.includes(spanText)) { //check if it matches the button clicked
                $(element).click(); //click the checkbox
            }
        })
    })

    $(".checkboxProduct").on("click", function () { //whenever a product is clicked
        var checked = this.checked; //get the values
        var element = $(this);
        element.prop("checked", checked); //set the checked property
        element.checked = checked;
        element.change(); //call the change() method from jquery
    })


    $(".btnCategory").click(function () { //when a category is clicked
        var divName = $(this).attr("name"); //get the div name
        var div = $("div[name='" + divName + "'");
        if (div.hasClass("hidden")) { //check if it is hidden
            div.removeClass("hidden"); //display it
        }
        else {
            div.addClass("hidden"); //hide it
            unselectProductsInCategory(div); //unselect the products
        }
    })

    $(".btnFamilyAll").change(function () { //select all products in the family
        var spanParent = $(this).parent(); //get the parent of the span
        var divProds = spanParent.nextAll("div"); //get the next div
        var prodsCheckbox = divProds.find(":checkbox"); //find all checkboxes in the div
        switch (this.checked) {
            case false:
                prodsCheckbox.each(function () {
                    this.checked = false; //set the checked attr
                });
                spanParent.toggleClass("glyphicon-ok glyphicon-remove"); //toggle the classes to change the display
                prodsCheckbox.change(); //trigger the change in the prodducts
                break;
            case true:
                prodsCheckbox.each(function () { //same as above
                    this.checked = true;
                })
                spanParent.toggleClass("glyphicon-ok glyphicon-remove");
                prodsCheckbox.change();
                break;
            default:
                console.log("btnFamily.checked = " + $(this).checked)
                break;
        }
    })

    $(".btnFamilyInvert").change(function () { //invert the selection in the family
        var divProds = $(this).parent().nextAll("div");
        var prodsCheckbox = divProds.find(":checkbox"); //select the checkboxes
        prodsCheckbox.each(function () {
            this.checked = !this.checked; //invert the checked attribute
        })
        prodsCheckbox.change(); //trigger the change of the checkbox
    })

    function unselectProductsInCategory(divCategory) {
        var prodCheckbox = $(divCategory).find(":checkbox");
        prodCheckbox.each(function () {
            this.checked = false;
            $(this).attr("checked", false).change();
        })

        $(".selectDevices").each(function (i, el) { //function to revert the background
            if ($(el).hasClass("btn-primary")) {
                $(el).toggleClass("btn-primary btn-default");
            }
        })

        $(".btn.glyphicon").each(function myfunction(i, el) { //function to revert the remove icon
            if ($(el).hasClass("glyphicon-remove")) {
                $(el).toggleClass("glyphicon-remove glyphicon-ok")
            }
        })
    }

    $('form').bind('submit', function () { //function to allow the disabled fields to  appear on the POST
        $(this).find('input').prop('disabled', false);
        $(this).find('select').prop('disabled', false);
    });

    $('.navbar-toggle').click(function () { //when the toggle button is clicked
        if ($('.navbar-collapse').hasClass('in')) {
            $('.navbar-collapse').slideUp("slow");
            $('.navbar-collapse').removeClass('in');
        }
        else {
            $('.navbar-collapse').slideDown("slow");
            $('.navbar-collapse').addClass('in');
        }
    })
    $("#btnRejectChange").click(function () { //when an approver rejects the change
        $("#approverOption").val(false);
    })
    $("#btnApproveChange").click(function () { //when an approver approves the change
        $("#approverOption").val(true);
    })

    //$('#LinkUrls option').attr("selected", "true");

    var elements = document.getElementsByClassName("linkSpan");
    $(elements).each(function (e, element) {
        LinkNames.push($(element).attr("name")) //push the name of the link to the array if it already exists
    })

    $('#linkURL').keypress(function (event) { //adding new links to the change:
        var url = "";
        if (event.which == 13) { //Enter Key press
            event.preventDefault(); //prevent submit
            url = $("#linkURL").val(); //get the val
            if (url != "") {
                if (!url.match(/^(http:\/\/www\.|https:\/\/www\.|http:\/\/|https:\/\/)[a-z0-9]+([\-\.]{1}[a-z0-9]+)*\.[a-z]{2,5}(:[0-9]{1,5})?(\/.*)?$/gim)) { //check if url is really an url
                    $("#urlValidation").text("This link is not a valid url. It should follow this pattern: https://www.example.com");
                }
                else {
                    var linkName = $("#linkName").val(); //get the name of the link
                    if (linkName != "") {
                        var nameExists = LinkNames.find(function (e) {
                            return e == linkName
                        })
                        if (nameExists == undefined) { //if the name is not in the array
                            $("#urlValidation").text("");
                            var input = document.createElement('input'); //create an input that will be in the POST
                            input.name = "LinkUrls[" + linkName + "]" //dictonary of names and links
                            input.value = url; //the value that will be sent
                            input.type = "hidden" //don't show the input

                            var listItem = document.createElement('li'); //create the list item
                            listItem.className = "list-group-item"; //add class

                            var spanText = document.createElement('span'); //create the span
                            spanText.textContent = linkName + ": "; //add the link name to it

                            var anchor = document.createElement("a"); //create the anchor
                            anchor.href = url; //href => where the link points to
                            anchor.textContent = url; //the text of it 
                            anchor.target = '_blank'; //open in a new tab
                            var i = document.createElement('i'); //X icon
                            i.className = "glyphicon glyphicon-remove" //glyphicon-remove == X
                            i.setAttribute('onclick', 'removeUrl(this)') //onclick to call the removeurl

                            listItem.append(spanText); //append the elements to the listitem
                            listItem.append(anchor);
                            listItem.append(i);
                            listItem.append(input);

                            $("#URLRepresentation").append(listItem); //append the listitem
                            LinkNames.push(linkName); //add the linkname to the array
                            $("#linkURL").val("");
                            $("#linkName").val("");
                        }
                        else {
                            $("#urlValidation").text("A link with this Name has already been added."); //message to the user
                        }
                    }
                    else {
                        $("#urlValidation").text("Name cannot be empty."); //message to the user
                    }
                }
            }
        }
        //$('#LinkUrls option').attr("selected", "true");
    })
})
function removeUrl(element) { //function called when the X is clicked
    var li = $(element).parent(); //get the list item
    var span = $(element).prev(); //get the span
    var spanText = span.text().trim();
    var found = LinkNames.indexOf(spanText); //find the name in the array of names
    LinkNames.splice(found, 1); // remove the name
    li.remove(); //remove the list item from the list
}

//function hideNewLinks() {
//    if ($("#URLRepresentation").children().length > 0) {
//        $('#newLinks').removeAttr("hidden");
//    }
//    else {
//        $('#newLinks').attr("hidden", true);
//    }
//}
var requiredApprovers = {};

$(function () {
    // start code for: If a validator is selected remove that user from approvar list and vice versa
    $("#Validators_Software,#Software").on("select2:select", function () {
        if (this.id === "Validators_Software") {
            var approvers = $("#Software > optgroup > option");
            var selectedoptions = $("#Validators_Software option:selected");
            //var selectOption = selectedoptions.length - 1;
            //selectOption = selectOption > 0 ? selectOption : 0;
            selectOption = selectedoptions[0];
            $.each(approvers, function () {
                if ($(this).text() == $(selectOption).text()) {
                    $(this).attr("disabled", true);
                }
            });
            $("#Software").select2();
        }
        if (this.id === "Software") {
            selectedoptions = $("#Software option:selected").val();
            //var selectOption = selectedoptions.length - 1;
            //selectOption = selectOption > 0 ? selectOption : 0;
            var selectOption = selectedoptions[0];
            var validators = $("#Validators_Software > optgroup > option");
            $.each(validators, function () {
                if ($(this).text() == $(selectOption).text()) {
                    $(this).attr("disabled", "disabled");
                }
            });
            $("#Validators_Software").trigger("change");
        }
    });

    $("#Validators_Software,#Software").on("select2:unselect", function () {
        if (this.id === "Validators_Software") {
            var approvers = $("#Software > optgroup > option");
            var val = $("#Validators_Software option:selected").text();
            if (val = "") {
                $.each(approvers, function () {
                    $(this).attr("disabled", false);
                });
            }

            $("#Software").select2();
        }
        if (this.id === "Software") {
            var val = $("#Software option:selected").text();
            var validators = $("#Validators_Software > optgroup > option");
            $.each(validators, function () {
                if ($(this).text() == val) {
                    $(this).attr("disabled", "");
                }
            });
            $("#Validators_Software").select2();
        }
    });
    //// end code for: If a validator is selected remove that user from approvar list and vice versa


    //// Code for Deviation
    //var requiredDeviation = {};
    $(".Deviation").each(function () {
        var bool = $(this).val().toLowerCase() == "true" ? true : false; //get the value and transform it to a boolean
        DeviationAndAreas(bool);
    })
    $(".Deviation").on("click", function () {//when the deviation is clicked
        var value = $(this).val().toLowerCase(); //get the value to lower
        var bool_value = value == "true" ? true : false //do a check for the bool value
        $(this).val(!bool_value); //reverse the value

        DeviationAndAreas(!bool_value); //display or hide deviation quantity and date
        $(this).change();
    })
    function DeviationAndAreas(bool) {
        if (bool) {
            $("#deviationDetails").removeClass("hidden"); //remove the hidden class
            //requiredDeviation = true;
        }
        else {
            $("#deviationDetails").addClass("hidden");
            //requiredDeviation = false;
        }
    }

    //////////////////////////////////////////////////////////////////////////////////////
    $(".AreaCheckbox").each(function () {
        var bool = $(this).val().toLowerCase() == "true" ? true : false; //get the value and transform it to a boolean
        var groupName = this.parentElement.innerText.trim();
        ApproversAndAreas(bool, groupName);
    })

    $(".ckboxPreventClick").on("click", function (e) {
        e.preventDefault();
    })

    $(".AreaCheckbox").on("click", function () {//when the area is clicked
        var value = $(this).val().toLowerCase(); //get the value to lower
        var bool_value = value == "true" ? true : false //do a check for the bool value
        $(this).val(!bool_value); //reverse the value
        var groupName = this.parentElement.innerText.trim(); //get the groupName
        ApproversAndAreas(!bool_value, groupName); //display or hide the approvers according to the group name
        $(this).change();
    })

    function ApproversAndAreas(bool, groupName) {
        if (bool) {
            displayApprovers(groupName);
            displayValidator(groupName);
            requiredApprovers[groupName] = true; //add a new required
        }
        else {
            hideApprovers(groupName);
            hideValidator(groupName);
            delete requiredApprovers[groupName]; //remove the required approvers group
        }
    }

    function displayApprovers(groupName) {
        $("#listApproverInline li[name='" + groupName + "']").removeClass("hidden"); //remove the hidden class
    }
    function hideApprovers(groupName) {
        var liElement = $("#listApproverInline li[name='" + groupName + "']");
        liElement.addClass("hidden");
        $(liElement).children("select").val(null).trigger('change');
    }
    function displayValidator(groupName) {
        $("#listValidatorInline li[name='" + groupName + "']").removeClass("hidden"); //remove the hidden class
    }
    function hideValidator(groupName) {
        var liElement = $("#listValidatorInline li[name='" + groupName + "']");
        liElement.addClass("hidden");
        $(liElement).children("select").val(null).trigger('change');
    }



    $('.MainTable').DataTable({ //DataTable for the Main page
        order: [[4, 'desc']],
        "columnDefs": [
            { "orderable": false, "targets": [0, 1, 2, 3, 4, 5, 6, 7, 8] },
            { "width": '3%', "targets": [0] },
            { "width": '2%', "targets": [1] },
            { "width": '5%', "targets": [2] },
            { "width": '40%', "targets": [3] },
            { "width": '10%', "targets": [4, 5, 6, 7, 8] }, //sizes for the columns
            {
                "targets": [3],
                render: function (data, type) {
                    return type === 'display' && data.length > 50 ?
                        data.substr(0, 50) + '…' :
                        data;
                }
            }
        ],
        initComplete: function () {
            this.api().columns().every(function () { //for every column
                var column = this;
                var select = $('<select class="form-control tableSelect"><option value="">' + $(column.header()).html() + '</option></select>') //create a new select element
                    .appendTo($(column.header()).empty()) //appending to an empty header
                    .on('change', function () { //whenever the select is changed
                        var val = $.fn.dataTable.util.escapeRegex( //read about this in the datatables website
                            $(this).val()
                        );
                        column
                            .search(val ? '^' + val + '$' : '', true, false) //regex
                            .draw();
                    });

                column.data().unique().sort().each(function (d) {
                    select.append('<option value="' + d + '">' + d + '</option>') //options in the select
                });
            });
        }
    });

    $(".AuditLogTable").DataTable({
        autoWidth: false,
        "columnDefs": [
            { "width": '15%', "targets": [0] },
            { "width": '10%', "targets": [1] },
            { "width": '5%', "targets": [2] },
            { "width": '10%', "targets": [3] },
            { "width": '30%', "targets": [4, 5] } //sizes for the columns
        ]
    }
    ); //transform into a datatable

    $(".ProductsTable").DataTable();
    $(".UserTable").DataTable({
        paging: false,
        "ordering": false,
        "bInfo": false
    });
    $(".PartialTable").DataTable();
    $(".PartialPrintTable").DataTable({
        paging: false,
        "ordering": false,
        "searching": false,
        "bInfo": false
    });

    $(".ManageNotificationsTable").DataTable(); //transform into a datatable

    $(".SelectControl").select2({ //make .SelectControl elements into select2 elements
        placeholder: 'Select an Option', //custom placeholder
        width: 'element',
        allowClear: true //allow X button to clear
    })
    //$('.LinksSelectControl').select2({ //this is interesting if you want to allow the user to type links as they go into a select2 control
    //    tags:true,
    //    createTag: function (params) {
    //        var term = $.trim(params.term);
    //        console.log(term);
    //        if (term === '') {
    //            return null;
    //        }
    //        //checks if the text contains https:// or http:// in the beginning
    //        if (term.indexOf("https://") === 0 || term.indexOf("http://") === 0) {
    //            return {
    //                id: term,
    //                text: term
    //            }
    //        }
    //    },
    //    insertTag: function (data, tag) {
    //        data.push(tag);
    //    }
    //});
})

//Create and Edit ECN:

$(function () {
    var dateOfNotice = document.getElementById("DateOfNotice"); //gets the input element for the Date of Notice field
    if (dateOfNotice !== null) {
        dateOfNotice.min = new Date().toISOString().split("T")[0]; //sets the minimum date of notice to be today.
    }
    $('.hideSelect').hide(); //hides the approvers because no Change Type has been selected
    $("#ChangeTypeId").on("change", function () { //when the Change Type is selected
        var ChangeType = $(this).find(":selected").text(); //get the text of the selected option
        var SelectList = $(".approvers"); //gets the lists with the approvers class
        for (var i = 0; i < SelectList.length; i++) {
            if ($(SelectList[i]).attr("id") == ChangeType) { //checks if the id of the list matches the change type selected, then
                $(SelectList[i]).parent().show(); //shows the <span> that the <select> is inside of
                $(SelectList[i]).attr("name", "UserRoleIds") //changes the name attribute to "UserRoleIds" -> this will be sent in the POST
            }
            else { //does the opposite of the above,
                $(SelectList[i]).parent().hide(); //hides the <span>
                $(SelectList[i]).val("").change(); //resets the option in the <select>
                $(SelectList[i]).attr("name", ""); //removes the name UserRoleIds from the <select
            }
        }
    })
})


////Function to make a field restricted only numeric
//// Restricts input for the set of matched elements to the given inputFilter function.
$(function ($) {
    jQuery.fn.inputFilter = function (inputFilter) {
        return this.on("input keydown keyup mousedown mouseup select contextmenu drop", function () {
            if (inputFilter(this.value)) {
                this.oldValue = this.value;
                this.oldSelectionStart = this.selectionStart;
                this.oldSelectionEnd = this.selectionEnd;
            } else if (this.hasOwnProperty("oldValue")) {
                this.value = this.oldValue;
                this.setSelectionRange(this.oldSelectionStart, this.oldSelectionEnd);
            } else {
                this.value = "";
            }
        });
    };
}(jQuery));

























//var LinkNames = [];

//function CallDetails(type, id) {
//    type = type + "s";
//    window.location = window.location.origin + "/" + type + "/Details/" + id;
//}

//function CallNotificationEdit(id) {
//    window.location = window.location.origin + "/Notifications/Edit/" + id;
//}
//function checkECOUserInputs(x) { //this will check for the inputs of the user
//    if (x.id == "btnSubmit") {
//        $("#Status option").removeAttr("selected");//reset the status option
//        $("#Status option[value='3']").attr("selected", "selected");//set the status to the option 3, which is awaiting approval
//    }
//    else if (x.id == "btnSave") {//same thing as above
//        $("#Status option").removeAttr("selected");
//        $("#Status option[value='0']").attr("selected", "selected"); //but sets the status option to 0, which is Draft
//    }

//    var aList = document.getElementsByName("ApproversList"); //first get all elements with the ApproversList name
//    var ValidationDiv = $('#validationDiv'); //gets the ValidationDiv, this will be used to append the items missing
//    var list = document.createElement('ul', { id: "validationOrderedList", class: "list-inline" }); //creates an <ul>
//    var areasChecked = null;
//    //this will check for the checkbox for the affected areas, returning true if they are selected or null if not;
//    $(".AreaCheckbox").each(function () { if ($(this).prop("checked")) { areasChecked = true; return false; } else { areasChecked = null; } })
//    var Fields = { //these are the fields that will be checked.
//        "Change Type": $("#ChangeTypeId").val() !== null ? $("#ChangeTypeId").val() : null, //simple check if the value is null, returning the value or null
//        "Priority Level": $("#PriorityLevel").val() !== null ? $("#PriorityLevel").val() : null,
//        "Implementation Type": $("#ImplementationType").val() !== null ? $("#ImplementationType").val() : null,
//        "Planned Implementation Date": new Date($("#PlannedImplementationDate").val()) > Date.now() ? $("#PlannedImplementationDate").val() : null,
//        "Permanent Change": $("#PermanentChange").val(), //true or false for the following:
//        "BOM Required": $("#BOMRequired").val(),
//        "Product Validation Testing Required": $("#ProductValidationTestingRequired").val(),
//        "Customer Approval Required": $("#CustomerApproval").val(),
//        "Description": $("#Description").val(), //text fields
//        "Reason For Change": $("#ReasonForChange").val(),
//        //"Links": $("#URLRepresentation").children().length > 0 ? $("#URLRepresentation").children().length : null, //counts the list of links in the representation div
//        "Areas Affected": areasChecked, //if any Area has been checked, return the value or null
//        "Approvers": $(aList).find("option:selected").length, //gets how many approvers were selected
//        "Notifications": $('#UsersToBeNotified').val().length > 0 ? $("#UsersToBeNotified").val().length : null // same as above but with notifications, returning null if < 0
//    };
//    console.log(Fields["Areas Affected"])
//    for (var field in Fields) { //loops through the fields
//        if (Fields.hasOwnProperty(field)) {
//            var fieldValue = Fields[field]; //the value in the field
//            if (fieldValue == null || fieldValue == undefined || fieldValue == "" || fieldValue == "null") { //checks for null, undefined, empty and "null"
//                var listItem = document.createElement('li', { class: "list-group-item" }); //creates an item for the list
//                listItem.append(document.createTextNode(field)); //creates a text node for the item with the field name
//                list.append(listItem); //appends the item to the list
//            }
//            else {
//            }
//        }
//    }
//    for (var reqApprover in requiredApprovers) {
//        if ($("select[id='" + reqApprover + "']").val().length > 0) { //checks if the required approvers have been selected
//        }
//        else { //or display an alert in the list
//            var approverListItem = document.createElement('li');
//            approverListItem.append(document.createTextNode("Approvers for " + reqApprover))
//            list.append(approverListItem)
//        }
//    }
//    if (list.hasChildNodes()) { //if the list has any items inside it
//        ValidationDiv.html("The following fields do not have a value:");
//        ValidationDiv.append(list);
//        window.scrollTo(0, 0);
//    }
//    else {
//        $('#formCreate').submit(); //if it doesn't have items, means it is good to be sent to the controller
//    }
//}
//function checkECRUserInputs(x) {
//    if (x.id == "btnSubmit") {
//        $("#Status option").removeAttr("selected");
//        $("#Status option[value='3']").attr("selected", "selected");
//    }
//    else if (x.id == "btnSave") {
//        $("#Status option").removeAttr("selected");
//        $("#Status option[value='0']").attr("selected", "selected");
//    }

//    var aList = document.getElementsByName("ApproversList");
//    var areasChecked = null;
//    //this will check for the checkbox for the affected areas, returning true if they are selected or null if not;
//    $(".AreaCheckbox").each(function () { if ($(this).prop("checked")) { areasChecked = true; return false; } else { areasChecked = null; } })
//    var ValidationDiv = $('#validationDiv');
//    var list = document.createElement('ul', { id: "validationOrderedList", class: "list-inline" });
//    console.log(new Date($("#PlannedImplementationDate").val()))
//    console.log(Date.now())
//    var Fields = {
//        "Change Type": $("#ChangeTypeId").val() !== null ? $("#ChangeTypeId").val() : null,
//        "Priority Level": $("#PriorityLevel").val() !== null ? $("#PriorityLevel").val() : null,
//        "Implementation Type": $("#ImplementationType").val() !== null ? $("#ImplementationType").val() : null,
//        "Planned Implementation Date": new Date($("#PlannedImplementationDate").val()) > Date.now() ? $("#PlannedImplementationDate").val() : null,
//        "Permanent Change": $("#PermanentChange").val(),
//        "BOM Required": $("#BOMRequired").val(),
//        "Product Validation Testing Required": $("#ProductValidationTestingRequired").val(),
//        "Customer Impact": $("#CustomerImpact").val(),
//        "Description": $("#Description").val(),
//        "Reason For Change": $("#ReasonForChange").val(),
//        //"Links": $("#URLRepresentation").children().length > 0 ? $("#URLRepresentation").children().length : null,
//        "Areas Affected": areasChecked,
//        "Approvers": $(aList).find("option:selected").length,
//        "Notifications": $('#UsersToBeNotified').val().length > 0 ? $("#UsersToBeNotified").val().length : null
//    };
//    console.log(Fields["Areas Affected"])
//    for (var field in Fields) {
//        if (Fields.hasOwnProperty(field)) {
//            var fieldValue = Fields[field];
//            if (fieldValue == null || fieldValue == undefined || fieldValue == "" || fieldValue == "null") {
//                var listItem = document.createElement('li', { class: "list-group-item" });
//                listItem.append(document.createTextNode(field));
//                list.append(listItem);
//            } else {
//            }
//        }
//    }
//    for (var reqApprover in requiredApprovers) {
//        if ($("select[id='" + reqApprover + "']").val().length > 0) {

//        }
//        else {
//            var approverListItem = document.createElement('li');
//            approverListItem.append(document.createTextNode("Approvers for " + reqApprover))
//            list.append(approverListItem)
//        }
//    }
//    if (list.hasChildNodes()) {
//        ValidationDiv.html("The following fields do not have a value:");
//        ValidationDiv.append(list);
//        window.scrollTo(0, 0);
//    }
//    else {
//        $('#formCreate').submit();
//    }
//}
//function RejectPopup() {
//    try {
//        $('#myModal').modal('show')
//    } catch (error) {
//        prompt(error)
//    }
//}
//function checkECNUserInputs(x) {
//    if (x.id == "btnSubmit") {
//        $("#Status option").removeAttr("selected");
//        $("#Status option[value='3']").attr("selected", "selected");
//    }
//    else if (x.id == "btnSave") {
//        $("#Status option").removeAttr("selected");
//        $("#Status option[value='0']").attr("selected", "selected");
//    }

//    var aList = document.getElementsByName("UserRoleIds");
//    console.log(aList);
//    //this will check for the checkbox for the affected areas, returning true if they are selected or null if not;
//    $(".AreaCheckbox").each(function () { if ($(this).prop("checked")) { areasChecked = true; return false; } else { areasChecked = null; } })
//    var ValidationDiv = $('#validationDiv');
//    var list = document.createElement('ul', { id: "validationOrderedList", class: "list-inline" });
//    var Fields = {
//        "Change Type": $("#ChangeTypeId").val() !== null ? $("#ChangeTypeId").val() : null,
//        "Model Name": $("#ModelName").val() !== null ? $("#ModelName").val() : null,
//        "Model Number": $("#ModelNumber").val() !== null ? $("#ModelNumber").val() : null,
//        "Date Of Notice": new Date($("#DateOfNotice").val()) !== "Invalid Date" ? $("#DateOfNotice").val() : null,
//        "PTCRB Resubmission Required": $("#PTCRBResubmissionRequired").val(),
//        "Description": $("#Description").val(),
//        "Impact of Missing Required Approval Date": $("#ImpactMissingReqApprovalDate").val(),
//        "Approvers": $(aList).find("option:selected").length,
//    };
//    console.log(Fields["Areas Affected"])
//    for (var field in Fields) {
//        if (Fields.hasOwnProperty(field)) {
//            var fieldValue = Fields[field];
//            if (fieldValue == null || fieldValue == undefined || fieldValue == "" || fieldValue == "null") {
//                var listItem = document.createElement('li', { class: "list-group-item" });
//                listItem.append(document.createTextNode(field));
//                list.append(listItem);
//            } else {
//            }
//        }
//    }
//    for (var reqApprover in requiredApprovers) {
//        if ($("select[id='" + reqApprover + "']").val().length > 0) {

//        }
//        else {
//            var approverListItem = document.createElement('li');
//            approverListItem.append(document.createTextNode("Approvers for " + reqApprover))
//            list.append(approverListItem)
//        }
//    }
//    if (list.hasChildNodes()) {
//        ValidationDiv.html("The following fields do not have a value:");
//        ValidationDiv.append(list);
//        window.scrollTo(0, 0);
//    }
//    else {
//        $('#formCreate').submit();
//    }
//}
//$('document').ready(function () {

//    //style the table rows:
//    var rows = document.getElementsByClassName("status");

//    for (var i = 0; i < rows.length; i++) {
//        row = rows[i];
//        if (row.textContent.trim() == "Approved") {
//            row.style.backgroundColor = "#98fb98"; //if it was approved, change the background
//        }
//        else if (row.textContent.trim().includes("Rejected")) {
//            row.style.backgroundColor = "#fb9898"; //if the change has been rejected, change the background
//        }
//    }

//    var observe;
//    if (window.attachEvent) {
//        observe = function (element, event, handler) {
//            element.attachEvent('on' + event, handler);
//        };
//    }
//    else {
//        observe = function (element, event, handler) {
//            element.addEventListener(event, handler, false);
//        };
//    }
//    $(function () {
//        $('textarea[data-editor="ck"]').each(function () {
//            autoresize(this.id);
//        });
//    });
//    function autoresize(x) {
//        var text = document.getElementById(x);
//        function resize() {
//            text.style.height = 'auto';
//            text.style.height = text.scrollHeight + 'px';
//        }
//        /* 0-timeout to get the already changed text */
//        function delayedResize() {
//            window.setTimeout(resize, 0);
//        }
//        observe(text, 'change', resize);
//        observe(text, 'cut', delayedResize);
//        observe(text, 'paste', delayedResize);
//        observe(text, 'drop', delayedResize);
//        observe(text, 'keydown', delayedResize);

//        text.focus();
//        text.select();
//        resize();
//        deselectAll();
//    }
//    function deselectAll() {
//        var element = document.activeElement;

//        if (element && /INPUT|TEXTAREA/i.test(element.tagName)) {
//            if ('selectionStart' in element) {
//                element.selectionEnd = element.selectionStart;
//            }
//            element.blur();
//        }

//        if (window.getSelection) { // All browsers, except IE <=8
//            window.getSelection().removeAllRanges();
//        } else if (document.selection) { // IE <=8
//            document.selection.empty();
//        }
//    }

//    var ImplementationDate = document.getElementById("PlannedImplementationDate");
//    if (ImplementationDate) {
//        ImplementationDate.min = new Date().toISOString().split("T")[0];
//    }
//    jQuery('[data-confirm]').click(function (e) {
//        if (!confirm(jQuery(this).attr("data-confirm"))) {
//            e.preventDefault();
//        }
//    });

//    $(':disabled').css("backgroundColor", "transparent").css("cursor", "default");//.css("backgroundColor", "#f9f9f9");

//    //$(".InputSearchField").on("focus", function () {
//    //    $(this).animate({
//    //        width: "+=50px"
//    //    }, 500);
//    //})
//    //$(".InputSearchField").on("blur", function () {
//    //    $(this).animate({
//    //        width: "-=50px"
//    //    }, 500);
//    //})

//    $(".selectDevices").click(function () { //when one of the GO7/GO8/GO9 buttons is clicked, do this 
//        var span = $(this);
//        span.toggleClass("btn-default btn-primary"); //toggle one of the classes to change the backgroung


//        var divDevices = span.parentsUntil("div").parent(); //get the div with all devices

//        var spanText = $(this).text().trim(); //get the text of the button
//        var allCheckboxes = divDevices.find(".checkboxProduct"); //get the checkboxes that belong to the devices div

//        allCheckboxes.each(function (index, element) { //foreach checkbox
//            var checkboxText = $(element).next().text().trim(); //get the text
//            if (checkboxText.includes(spanText)) { //check if it matches the button clicked
//                $(element).click(); //click the checkbox
//            }
//        })
//    })

//    $(".checkboxProduct").on("click", function () { //whenever a product is clicked
//        var checked = this.checked; //get the values
//        var element = $(this);
//        element.prop("checked", checked); //set the checked property
//        element.checked = checked;
//        element.change(); //call the change() method from jquery
//    })


//    $(".btnCategory").click(function () { //when a category is clicked
//        var divName = $(this).attr("name"); //get the div name
//        var div = $("div[name='" + divName + "'");
//        if (div.hasClass("hidden")) { //check if it is hidden
//            div.removeClass("hidden"); //display it
//        }
//        else {
//            div.addClass("hidden"); //hide it
//            unselectProductsInCategory(div); //unselect the products
//        }
//    })

//    $(".btnFamilyAll").change(function () { //select all products in the family
//        var spanParent = $(this).parent(); //get the parent of the span
//        var divProds = spanParent.nextAll("div"); //get the next div
//        var prodsCheckbox = divProds.find(":checkbox"); //find all checkboxes in the div
//        switch (this.checked) {
//            case false:
//                prodsCheckbox.each(function () {
//                    this.checked = false; //set the checked attr
//                });
//                spanParent.toggleClass("glyphicon-ok glyphicon-remove"); //toggle the classes to change the display
//                prodsCheckbox.change(); //trigger the change in the prodducts
//                break;
//            case true:
//                prodsCheckbox.each(function () { //same as above
//                    this.checked = true;
//                })
//                spanParent.toggleClass("glyphicon-ok glyphicon-remove");
//                prodsCheckbox.change();
//                break;
//            default:
//                console.log("btnFamily.checked = " + $(this).checked)
//                break;
//        }
//    })

//    $(".btnFamilyInvert").change(function () { //invert the selection in the family
//        var divProds = $(this).parent().nextAll("div");
//        var prodsCheckbox = divProds.find(":checkbox"); //select the checkboxes
//        prodsCheckbox.each(function () {
//            this.checked = !this.checked; //invert the checked attribute
//        })
//        prodsCheckbox.change(); //trigger the change of the checkbox
//    })

//    function unselectProductsInCategory(divCategory) {
//        var prodCheckbox = $(divCategory).find(":checkbox");
//        prodCheckbox.each(function () {
//            this.checked = false;
//            $(this).attr("checked", false).change();
//        })

//        $(".selectDevices").each(function (i, el) { //function to revert the background
//            if ($(el).hasClass("btn-primary")) {
//                $(el).toggleClass("btn-primary btn-default");
//            }
//        })

//        $(".btn.glyphicon").each(function myfunction(i, el) { //function to revert the remove icon
//            if ($(el).hasClass("glyphicon-remove")) {
//                $(el).toggleClass("glyphicon-remove glyphicon-ok")
//            }
//        })
//    }

//    $('form').bind('submit', function () { //function to allow the disabled fields to  appear on the POST
//        $(this).find('input').prop('disabled', false);
//        $(this).find('select').prop('disabled', false);
//    });

//    $('.navbar-toggle').click(function () { //when the toggle button is clicked
//        if ($('.navbar-collapse').hasClass('in')) {
//            $('.navbar-collapse').slideUp("slow");
//            $('.navbar-collapse').removeClass('in');
//        }
//        else {
//            $('.navbar-collapse').slideDown("slow");
//            $('.navbar-collapse').addClass('in');
//        }
//    })
//    $("#btnRejectChange").click(function () { //when an approver rejects the change
//        $("#approverOption").val(false);
//    })
//    $("#btnApproveChange").click(function () { //when an approver approves the change
//        $("#approverOption").val(true);
//    })

//    //$('#LinkUrls option').attr("selected", "true");

//    var elements = document.getElementsByClassName("linkSpan");
//    $(elements).each(function (e, element) {
//        LinkNames.push($(element).attr("name")) //push the name of the link to the array if it already exists
//    })

//    $('#linkURL').keypress(function (event) { //adding new links to the change:
//        var url = "";
//        if (event.which == 13) { //Enter Key press
//            event.preventDefault(); //prevent submit
//            url = $("#linkURL").val(); //get the val
//            if (url != "") {
//                if (!url.match(/^(http:\/\/www\.|https:\/\/www\.|http:\/\/|https:\/\/)[a-z0-9]+([\-\.]{1}[a-z0-9]+)*\.[a-z]{2,5}(:[0-9]{1,5})?(\/.*)?$/gim)) { //check if url is really an url
//                    $("#urlValidation").text("This link is not a valid url. It should follow this pattern: https://www.example.com");
//                }
//                else {
//                    var linkName = $("#linkName").val(); //get the name of the link
//                    if (linkName != "") {
//                        var nameExists = LinkNames.find(function (e) {
//                            return e == linkName
//                        })
//                        if (nameExists == undefined) { //if the name is not in the array
//                            $("#urlValidation").text("");
//                            var input = document.createElement('input'); //create an input that will be in the POST
//                            input.name = "LinkUrls[" + linkName + "]" //dictonary of names and links
//                            input.value = url; //the value that will be sent
//                            input.type = "hidden" //don't show the input

//                            var listItem = document.createElement('li'); //create the list item
//                            listItem.className = "list-group-item"; //add class

//                            var spanText = document.createElement('span'); //create the span
//                            spanText.textContent = linkName + ": "; //add the link name to it

//                            var anchor = document.createElement("a"); //create the anchor
//                            anchor.href = url; //href => where the link points to
//                            anchor.textContent = url; //the text of it 
//                            anchor.target = '_blank'; //open in a new tab
//                            var i = document.createElement('i'); //X icon
//                            i.className = "glyphicon glyphicon-remove" //glyphicon-remove == X
//                            i.setAttribute('onclick', 'removeUrl(this)') //onclick to call the removeurl

//                            listItem.append(spanText); //append the elements to the listitem
//                            listItem.append(anchor);
//                            listItem.append(i);
//                            listItem.append(input);

//                            $("#URLRepresentation").append(listItem); //append the listitem
//                            LinkNames.push(linkName); //add the linkname to the array
//                            $("#linkURL").val("");
//                            $("#linkName").val("");
//                        }
//                        else {
//                            $("#urlValidation").text("A link with this Name has already been added."); //message to the user
//                        }
//                    }
//                    else {
//                        $("#urlValidation").text("Name cannot be empty."); //message to the user
//                    }
//                }
//            }
//        }
//        //$('#LinkUrls option').attr("selected", "true");
//    })
//})
//function removeUrl(element) { //function called when the X is clicked
//    var li = $(element).parent(); //get the list item
//    var span = $(element).prev(); //get the span
//    var spanText = span.text().trim();
//    var found = LinkNames.indexOf(spanText); //find the name in the array of names
//    LinkNames.splice(found, 1); // remove the name
//    li.remove(); //remove the list item from the list
//}

////function hideNewLinks() {
////    if ($("#URLRepresentation").children().length > 0) {
////        $('#newLinks').removeAttr("hidden");
////    }
////    else {
////        $('#newLinks').attr("hidden", true);
////    }
////}
//var requiredApprovers = {};

//$(function () {

//    //// start code for: If a validator is selected remove that user from approvar list and vice versa
//    $("#Validators_Software,#Software").on("select2:select", function () {
//        if (this.id === "Validators_Software") {
//            var approvers = $("#Software > optgroup > option");
//            var val = $("#Validators_Software option:selected").text();
//            $.each(approvers, function () {
//                if ($(this).text() == val) {
//                    $(this).attr("disabled", "disabled");
//                }
//            });
//            $("#Software").trigger("change");
//        }
//        if (this.id === "Software") {
//            var val = $("#Software option:selected").text();
//            var validators = $("#Validators_Software > optgroup > option");
//            $.each(validators, function () {
//                if ($(this).text() == val) {
//                    $(this).attr("disabled", "disabled");
//                }
//            });
//            $("#Validators_Software").trigger("change");
//        }
//    });    //$("#Validators_Software,#Software").on("select2:unselect", function () {
//        if (this.id === "Validators_Software") {
//            var approvers = $("#Software > optgroup > option");
//            var val = $("#Validators_Software option:selected").text();
//            $.each(approvers, function () {
//                if ($(this).text() == val) {
//                    $(this).attr("disabled", "");
//                }
//            });
//            $("#Software").trigger("change");
//        }
//        if (this.id === "Software") {
//            var val = $("#Software option:selected").text();
//            var validators = $("#Validators_Software > optgroup > option");
//            $.each(validators, function () {
//                if ($(this).text() == val) {
//                    $(this).attr("disabled", "");
//                }
//            });
//            $("#Validators_Software").trigger("change");
//        }
//    });
//    //// end code for: If a validator is selected remove that user from approvar list and vice versa


//    //var validatorsData = [];
//    //var approversData = [];
//    //$(document).ready(function () {
//    //    var optionsValidtors = $("#Validators_Software > optgroup > option");
//    //    var optionsApprovers = $("#Software > optgroup > option");
//    //    $.each(optionsValidtors, function () {
//    //        validatorsData.push({
//    //            Text: $(this).text(),
//    //            Value: $(this).val()
//    //        })
//    //    });

//    //    $.each(optionsApprovers, function () {
//    //        approversData.push({
//    //            Text: $(this).text(),
//    //            Value: $(this).val()
//    //        })
//    //    });
//    //});
//    //$("#Validators_Software,#Software").on("select2:select", function () {
//    //    if (this.id === "Validators_Software") {//then same approver should be disabled
//    //        var approvers = [];
//    //        var val = $("#Validators_Software option:selected").text();
//    //        $.each(approversData, function () {
//    //            if (this.Text !== val) {
//    //                approvers.push(this);
//    //            }
//    //        });
//    //        $("#Software").select2('destroy');
//    //        $("#Software").val(approvers);
//    //        $("#Software").trigger("change");
//    //    }
//    //    if (this.id === "Software") {//then same validator should be disabled
//    //        var val = $("#Software option:selected").text();
//    //        var validators = $("#Validators_Software > optgroup > option");
//    //        $.each(validators, function () {
//    //            if ($(this).text() == val) {
//    //                $(this).attr("disabled", true);
//    //            }
//    //        });
//    //        $("#Validators_Software").trigger("change");
//    //    }
//    //});

//    //$("#Validators_Software,#Software").on("select2:unselect", function () {
//    //    if (this.id === "Validators_Software") {
//    //        var approvers = $("#Software > optgroup > option");
//    //        var val = $("#Validators_Software option:selected").text();
//    //        $.each(approvers, function () {
//    //            if ($(this).text() == val) {
//    //                $(this).attr("disabled", false);
//    //            }
//    //        });
//    //        $("#Software").trigger("change");
//    //    }
//    //    if (this.id === "Software") {
//    //        var val = $("#Software option:selected").text();
//    //        var validators = $("#Validators_Software > optgroup > option");
//    //        $.each(validators, function () {
//    //            if ($(this).text() == val) {
//    //                $(this).attr("disabled", false);
//    //            }
//    //        });
//    //        $("#Validators_Software").trigger("change");
//    //    }
//    //});

//    $(".AreaCheckbox").each(function () {
//        var bool = $(this).val().toLowerCase() == "true" ? true : false; //get the value and transform it to a boolean
//        var groupName = this.parentElement.innerText.trim();
//        ApproversAndAreas(bool, groupName);
//    })

//    $(".ckboxPreventClick").on("click", function (e) {
//        e.preventDefault();
//    })

//    $(".AreaCheckbox").on("click", function () {//when the area is clicked
//        var value = $(this).val().toLowerCase(); //get the value to lower
//        var bool_value = value == "true" ? true : false //do a check for the bool value
//        $(this).val(!bool_value); //reverse the value
//        var groupName = this.parentElement.innerText.trim(); //get the groupName
//        ApproversAndAreas(!bool_value, groupName); //display or hide the approvers according to the group name
//        $(this).change();
//    })

//    function ApproversAndAreas(bool, groupName) {
//        if (bool) {
//            displayApprovers(groupName);
//            displayValidator(groupName);
//            requiredApprovers[groupName] = true; //add a new required
//        }
//        else {
//            hideApprovers(groupName);
//            hideValidator(groupName);
//            delete requiredApprovers[groupName]; //remove the required approvers group
//        }
//    }

//    function displayApprovers(groupName) {
//        $("#listApproverInline li[name='" + groupName + "']").removeClass("hidden"); //remove the hidden class
//    }
//    function hideApprovers(groupName) {
//        var liElement = $("#listApproverInline li[name='" + groupName + "']");
//        liElement.addClass("hidden");
//        $(liElement).children("select").val(null).trigger('change');
//    }
//    function displayValidator(groupName) {
//        $("#listValidatorInline li[name='" + groupName + "']").removeClass("hidden"); //remove the hidden class
//    }
//    function hideValidator(groupName) {
//        var liElement = $("#listValidatorInline li[name='" + groupName + "']");
//        liElement.addClass("hidden");
//        $(liElement).children("select").val(null).trigger('change');
//    }



//    $('.MainTable').DataTable({ //DataTable for the Main page
//        order: [[4, 'desc']],
//        "columnDefs": [
//            { "orderable": false, "targets": [0, 1, 2, 3, 4, 5, 6, 7, 8] },
//            { "width": '3%', "targets": [0] },
//            { "width": '2%', "targets": [1] },
//            { "width": '5%', "targets": [2] },
//            { "width": '40%', "targets": [3] },
//            { "width": '10%', "targets": [4, 5, 6, 7, 8] }, //sizes for the columns
//            {
//                "targets": [3],
//                render: function (data, type) {
//                    return type === 'display' && data.length > 50 ?
//                        data.substr(0, 50) + '…' :
//                        data;
//                }
//            }
//        ],
//        initComplete: function () {
//            this.api().columns().every(function () { //for every column
//                var column = this;
//                var select = $('<select class="form-control tableSelect"><option value="">' + $(column.header()).html() + '</option></select>') //create a new select element
//                    .appendTo($(column.header()).empty()) //appending to an empty header
//                    .on('change', function () { //whenever the select is changed
//                        var val = $.fn.dataTable.util.escapeRegex( //read about this in the datatables website
//                            $(this).val()
//                        );
//                        column
//                            .search(val ? '^' + val + '$' : '', true, false) //regex
//                            .draw();
//                    });

//                column.data().unique().sort().each(function (d) {
//                    select.append('<option value="' + d + '">' + d + '</option>') //options in the select
//                });
//            });
//        }
//    });

//    $(".AuditLogTable").DataTable({
//        autoWidth: false,
//        "columnDefs": [
//            { "width": '15%', "targets": [0] },
//            { "width": '10%', "targets": [1] },
//            { "width": '5%', "targets": [2] },
//            { "width": '10%', "targets": [3] },
//            { "width": '30%', "targets": [4, 5] } //sizes for the columns
//        ]
//    }
//    ); //transform into a datatable

//    $(".ProductsTable").DataTable();
//    $(".UserTable").DataTable({
//        paging: false,
//        "ordering": false,
//        "bInfo": false
//    });
//    $(".PartialTable").DataTable();
//    $(".PartialPrintTable").DataTable({
//        paging: false,
//        "ordering": false,
//        "searching": false,
//        "bInfo": false
//    });

//    $(".ManageNotificationsTable").DataTable(); //transform into a datatable

//    $(".SelectControl").select2({ //make .SelectControl elements into select2 elements
//        placeholder: 'Select an Option', //custom placeholder
//        width: 'element',
//        allowClear: true //allow X button to clear
//    })
//    //$('.LinksSelectControl').select2({ //this is interesting if you want to allow the user to type links as they go into a select2 control
//    //    tags:true,
//    //    createTag: function (params) {
//    //        var term = $.trim(params.term);
//    //        console.log(term);
//    //        if (term === '') {
//    //            return null;
//    //        }
//    //        //checks if the text contains https:// or http:// in the beginning
//    //        if (term.indexOf("https://") === 0 || term.indexOf("http://") === 0) {
//    //            return {
//    //                id: term,
//    //                text: term
//    //            }
//    //        }
//    //    },
//    //    insertTag: function (data, tag) {
//    //        data.push(tag);
//    //    }
//    //});
////})

////Create and Edit ECN:

//$(function () {
//    var dateOfNotice = document.getElementById("DateOfNotice"); //gets the input element for the Date of Notice field
//    if (dateOfNotice !== null) {
//        dateOfNotice.min = new Date().toISOString().split("T")[0]; //sets the minimum date of notice to be today.
//    }
//    $('.hideSelect').hide(); //hides the approvers because no Change Type has been selected
//    $("#ChangeTypeId").on("change", function () { //when the Change Type is selected
//        var ChangeType = $(this).find(":selected").text(); //get the text of the selected option
//        var SelectList = $(".approvers"); //gets the lists with the approvers class
//        for (var i = 0; i < SelectList.length; i++) {
//            if ($(SelectList[i]).attr("id") == ChangeType) { //checks if the id of the list matches the change type selected, then
//                $(SelectList[i]).parent().show(); //shows the <span> that the <select> is inside of
//                $(SelectList[i]).attr("name", "UserRoleIds") //changes the name attribute to "UserRoleIds" -> this will be sent in the POST
//            }
//            else { //does the opposite of the above,
//                $(SelectList[i]).parent().hide(); //hides the <span>
//                $(SelectList[i]).val("").change(); //resets the option in the <select>
//                $(SelectList[i]).attr("name", ""); //removes the name UserRoleIds from the <select
//            }
//        }
//    })
//})