$(function () {
    $("#imgBtEdit").click(function (e) {
        return validateSelectedRows("Edit", true, e);
    });

    $("#imgBtCopy").click(function (e) {
        return validateSelectedRows("Copy", false, e);
    });

    $("#imgBtView").click(function (e) {
        return validateSelectedRows("View", true, e);
    });

    $("#imgBtDelete").click(function (e) {
        return confirmProcess("Delete", true, e);
    });

    $("#imgBtRemove").click(function (e) {
        return confirmProcess("Remove", true, e);
    });

    $("#imgBtCompany").click(function (e) {
        return validateSelectedRows("Assign Company", false, e);
    });

    $("#imgBtUsers").click(function (e) {
        return validateSelectedRows("Assign Users", false, e);
    });

    $("#imgBtRoles").click(function (e) {
        return validateSelectedRows("Assign Roles", false, e);
    });

    $("#imgBtSecurityItems").click(function (e) {
        return validateSelectedRows("Assign Security Items", false, e);
    });

    $("#imgBtApplicationSettings").click(function (e) {
        return validateSelectedRows("Assign Application Settings", false, e);
    });

    $("#imgBtPersons").click(function (e) {
        return validateSelectedRows("Assign Persons", false, e);
    });

    $("#imgBtAccessByAD").click(function (e) {
        return validateSelectedRows("Assign Access By AD", false, e);
    });

    $("#imgBtRestrictionByAD").click(function (e) {
        return validateSelectedRows("Assign Restriction By AD", false, e);
    });

    $("#imgBtAccessByRole").click(function (e) {
        return validateSelectedRows("Assign Access By Role", false, e);
    });

    $("#imgBtRestrictionByRole").click(function (e) {
        return validateSelectedRows("Assign Restriction By Role", false, e);
    });

    $("#imgBtAccessByUser").click(function (e) {
        return validateSelectedRows("Assign Access By User", false, e);
    });

    $("#imgBtRestrictionByUser").click(function (e) {
        return validateSelectedRows("Assign Restriction By User", false, e);
    });

    $("#imgBtSecurityItemsInUsers").click(function (e) {
        return validateSelectedRows("Assign Security Items", false, e);
    });

    $("#imgBtSingleDelete").click(function (e) {
        if (confirmProcess("Delete", false, e)) {
            deleteRecord(e, false);
        }
        else {
            return false;
        }
    });
});

function validateSelectedRows(action, isMultiSelect, e) {
    var grid = $(e.target).parents("form").find("div[data-role='grid']");
    var selectedIds = grid.prop("selectedRows");

    var selectedCount = 0;
    if (selectedIds != null && selectedIds != "") {
        selectedCount = selectedIds.split(",").length;
    }

    switch (selectedCount) {
        case 0:
            $("#error-display").html("Select a record to " + action);
            return false;
        case 1:
            break;
        default:
            if (!isMultiSelect) {
                $("#error-display").html("For the " + action + " action only one record can be selected");
                return false;
            }
    }

    grid.prop("selectedRows", selectedIds);
    $(grid.attr("hiddenSelectedRows")).val(selectedIds);
    $.cookie("SELECTED-" + grid.attr("Id"), selectedIds, { path: "/" });

    return true;
}

function confirmProcess(process, isMultiSelect, e) {
    var grid = $(e.target).parents("form").find("div[data-role='grid']");
    var bActionPass = validateSelectedRows(process, isMultiSelect, e);

    if (bActionPass) {
        bActionPass = confirm("Are you sure you want to " + process + " the selected record(s) ?");
        if (bActionPass) {
            $.cookie("SELECTED-" + grid.attr("id"), "", { path: "/" });
        }
    }

    return bActionPass;
}