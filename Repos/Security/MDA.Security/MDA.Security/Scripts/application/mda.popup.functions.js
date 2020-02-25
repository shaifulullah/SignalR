$(function () {
    var list = [];
    var index = 0;
    var popup = null;

    var dataOperation = { CREATE: 1, READ: 2, UPDATE: 3, DELETE: 4, EXECUTE: 5, ACCESS: 6, EDIT_VIEW: 7 };

    function initialiseWindow(e) {
        popup = $("#window").kendoWindow({
            modal: true,
            width: 768,
            open: function () {
                $.ajaxSetup({ async: false });
                this.wrapper.css({ top: 10 });
            },
            close: function () {
                var grid = $(e.target).parents("form").find("div[data-role='grid']");
                grid.data().kendoGrid.dataSource.read();

                $.ajaxSetup({ async: true });
            }
        }).data().kendoWindow.center().open();
    }

    function setPagerData() {
        index = $.inArray($("#hidId").val(), list);

        $("#imgBtPrevious").prop("disabled", index <= 0);
        $("#imgBtNext").prop("disabled", index >= (list.length - 1));

        $("#lblCurrentRecord").html((index + 1) + " of " + list.length);
        $("#window").parent().find(".k-window-title").text($("#title").val());
    }

    $(document).on("click", "#imgBtPrevious", function (e) {
        if (index > 0) {
            bindData(list[--index], dataOperation.EDIT_VIEW, e);
        }
    });

    $(document).on("click", "#imgBtNext", function (e) {
        if (index < list.length - 1) {
            bindData(list[++index], dataOperation.EDIT_VIEW, e);
        }
    });

    $(document).on("click", "#imgBtNew", function (e) {
        initialiseWindow(e);
        bindData(-1, dataOperation.CREATE, e);
    });

    $(document).on("click", "#imgBtCopy", function (e) {
        var grid = $(e.target).parents("form").find("div[data-role='grid']");
        list = grid.prop("selectedRows").split(",");

        initialiseWindow(e);
        bindData(list[0], dataOperation.CREATE, e);
    });

    $(document).on("click", "#imgBtEdit,#imgBtSingleEdit", function (e) {
        var grid = $(e.target).parents("form").find("div[data-role='grid']");
        list = grid.prop("selectedRows").split(",");

        initialiseWindow(e);
        bindData(list[0], dataOperation.EDIT_VIEW, e);
    });

    $(document).on("click", "#imgBtSave", function (e) {
        var controllerName = $(e.target).parents("form").attr("action");
        var form = $("#window form");

        var action = $("#hidId").val() == undefined ? "Insert" : "Update";

        form.validate().resetForm();
        $.validator.unobtrusive.parse(form);

        if (form.valid()) {
            $.ajax({
                url: controllerName + "/" + action,
                data: new FormData(form[0]),
                processData: false,
                contentType: false,
                type: "POST",
                success: function (data) {
                    $("#window #error-display, #error-display").html(data);

                    if ($("#hidId").val() == undefined) {
                        popup.close();
                    }
                }
            });
        }
    });

    function bindData(id, dataOperation, e) {
        var controllerName = $(e.target).parents("form").attr("action");

        $.post(controllerName + "/BindData", { selectedId: id, dataOperation: dataOperation }, function (data) {
            $("#window").html(data);

            var form = $("#window form");

            form.unbind();
            form.data("validator", null);
            $.validator.unobtrusive.parse(document);

            setPagerData();
        });
    }
})