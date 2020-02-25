$(function () {
    $(document).on("click", "#chBxSelectRow", function (e) {
        var grid = $(e.target).closest("div[data-role='grid']");

        setCheckBoxState(this, e);
        $("#chBxSelectAll", grid).prop("checked", (grid.find("input:checkbox.gridCheckBox:not(:checked)").length == 0));
    });

    $(document).on("click", "#chBxSelectAll", function (e) {
        var grid = $(e.target).closest("div[data-role='grid']");

        if (grid.attr("multiselect") == "true" || grid.attr("multiselect") == undefined) {
            grid.find("input:checkbox.gridCheckBox").each(function () {
                this.checked = e.target.checked;
                setCheckBoxState(this, e);
            });
        }
    });

    $("[id^=grid]").on("mouseover", "tr", function () {
        setCheckBoxVisibility(this, true);
    });

    $("[id^=grid]").on("mouseout", "tr", function () {
        setCheckBoxVisibility(this, $(this).find(".gridCheckBox").prop("checked"));
    });

    function setCheckBoxState(checkBox, e) {
        var grid = $(e.target).closest("div[data-role='grid']");
        var selectedIds = grid.prop("selectedRows") == undefined || grid.prop("selectedRows") == "" ? [] : grid.prop("selectedRows").split(",");

        var row = $(checkBox).closest("tr");
        var dataItem = grid.data().kendoGrid.dataItem(row);

        if (grid.attr("multiselect") == "false") {
            selectedIds = [];

            grid.find("input:checkbox.gridCheckBox").not(checkBox).each(function () {
                this.checked = false;
                $(this).css("visibility", "hidden");
            });
        }

        var index = selectedIds.indexOf(dataItem.id.toString());
        if (checkBox.checked) {
            if (index < 0) {
                selectedIds.push(dataItem.id.toString());
            }
        } else {
            if (index >= 0) {
                selectedIds.splice(index, 1);
            }
        }

        setCheckBoxVisibility(row, checkBox.checked);
        grid.prop("selectedRows", selectedIds.toString());
    }

    function setCheckBoxVisibility(row, isVisible) {
        var checkBoxVisibility = isVisible ? "visible" : "hidden";
        $(row).find(".gridCheckBox").css("visibility", checkBoxVisibility);
    }

    $(document).on("click", "#imgBtSavePreferences", function (e) {
        var grid = $(e.target).parents("form").find("div[data-role='grid']");
        var columns = grid.data().kendoGrid.columns;

        var visibleColumns = [];
        $.each(columns, function () {
            if (!this.hidden && this.field != null) {
                visibleColumns.push(this.field);
            }
        });

        $("#hidGridVisibleColumns").val(visibleColumns.toString());
    });

    $(document).on("click", "#imgBtDelete,#imgBtRemove", function (e) {
        deleteRecord(e, true);
    });
});

function onDataBound() {
    var grid = $("#" + this.wrapper.attr("id"));

    $.cookie("PAGE-" + grid.attr("id"), JSON.stringify(this.dataSource.page()), { path: "/" });
    $.cookie("SORT-" + grid.attr("id"), JSON.stringify(this.dataSource.sort()), { path: "/" });
    $.cookie("FILTER-" + grid.attr("id"), JSON.stringify(this.dataSource.filter()), { path: "/" });

    var view = this.dataSource.view();
    var selectedIds = grid.prop("selectedRows") == undefined ? [] : grid.prop("selectedRows").split(",");

    $.each(view, function () {
        if (this.id != undefined && selectedIds.indexOf(this.id.toString()) >= 0) {
            grid.data().kendoGrid.tbody.find("tr[data-uid='" + this.uid + "']").find(".gridCheckBox").attr("checked", true);
            grid.data().kendoGrid.tbody.find("tr[data-uid='" + this.uid + "']").find(".gridCheckBox").css("visibility", "visible");
        }
    });

    grid.find("thead th").map(function () {
        $(this).attr("title", $(this).attr("data-title"));
    });

    grid.find("tbody td").map(function () {
        $(this).attr("title", $(this).text());
    });

    $("#chBxSelectAll", grid).prop("checked", (grid.find("input:checkbox.gridCheckBox:not(:checked)").length == 0));
}

function getPage(gridId) {
    var cookie = $.cookie("PAGE-" + gridId);
    return cookie == undefined ? 0 : cookie;
}

function getSort(gridId) {
    var cookie = $.cookie("SORT-" + gridId);
    return cookie == undefined ? null : JSON.parse(cookie);
}

function getFilter(gridId) {
    var cookie = $.cookie("FILTER-" + gridId);
    return cookie == undefined ? null : JSON.parse(cookie);
}

function removeCookies(gridIds) {
    var gridIdList = gridIds.indexOf(",") >= 0 ? gridIds.split(",") : [gridIds];

    $.each(gridIdList, function () {
        var gridId = this;

        var cookies = document.cookie.split(";");
        $.each(cookies, function () {
            var cookieName = decodeURIComponent(this.split("=")[0]).trim();
            if (cookieName == "FILTER-" + gridId || cookieName == "SORT-" + gridId || cookieName == "PAGE-" + gridId || cookieName == "SELECTED-" + gridId) {
                $.removeCookie(cookieName, { path: "/" });
            }
        });
    });
}

function deleteRecord(e, isMultipleDelete) {
    var grid = $(e.target).parents("form").find("div[data-role='grid']");
    var controllerName = $(e.target).parents("form").attr("action");

    var dataObject = isMultipleDelete ? { ids: grid.prop("selectedRows").split(',') } : { id: grid.prop("selectedRows") }

    $.ajax({
        url: controllerName + "/Delete",
        data: dataObject,
        traditional: true,
        type: "POST",
        success: function (message) {
            $("#error-display").text(message);

            grid.data().kendoGrid.dataSource.read();
            grid.prop("selectedRows", null);
        }
    });
}