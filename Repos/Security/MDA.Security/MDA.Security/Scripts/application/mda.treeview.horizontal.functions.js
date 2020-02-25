$(function () {
    var treeMenu = $("#treeMenu").kendoMenu({
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
});