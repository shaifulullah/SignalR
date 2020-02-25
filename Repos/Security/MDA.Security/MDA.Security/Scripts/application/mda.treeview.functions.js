$(function () {
    var treeMenu = $("#treeMenu").kendoTreeView({
        select: function () {
            var cookies = document.cookie.split(";");

            $.each(cookies, function () {
                var cookieName = decodeURIComponent(this.split("=")[0]).trim();

                if (cookieName.indexOf("FILTER-") == 0 || cookieName.indexOf("SORT-") == 0 ||
                    cookieName.indexOf("PAGE-") == 0 || cookieName.indexOf("SELECTED-") == 0) {
                    $.removeCookie(cookieName, { path: "/" });
                }
            });
        }
    });

    kendoTreeView = treeMenu.data().kendoTreeView;

    // On Click of Parent Node Expand Node
    treeMenu.on("click", ".k-in", function (e) {
        kendoTreeView.toggle($(e.target).closest(".k-item"));
    });

    // Expand Nodes
    var $selectedNode = $("#" + $.cookie("SelectedNode"));

    $selectedNode.parents("li").add(this).each(function () {
        kendoTreeView.expand($(this));
    });

    kendoTreeView.select($selectedNode);
});