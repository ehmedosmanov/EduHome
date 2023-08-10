$(document).on("keyup", "#searchInput", function () {
    let key = $("#searchInput").val();

    $.ajax({
        url: "/Blog/Search",
        type: "GET",
        data: {
            "key": key
        },
        success: function (res) {
            $("#myBlogs").html(res);
    
        }
    });
});