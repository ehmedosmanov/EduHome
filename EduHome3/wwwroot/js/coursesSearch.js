$(document).on("keyup", "#searchInput", function () {
    let key = $("#searchInput").val();

    $.ajax({
        url: "/Courses/Search",
        type: "GET",
        data: {
            "key": key
        },
        success: function (res) {
            $("#myCourses").html(res);
    
        }
    });
});