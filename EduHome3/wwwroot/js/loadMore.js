let count = 6;
let coursesCount = $("#loadMore").next().val()
$(document).on("click", "#loadMore", function () {
    $.ajax({
        url: "/Courses/LoadMore",
        type: "GET",
        data: {
            "skip": count
            },
        success: function (res) {
            count += 6;
            if (coursesCount <= count)
            {
                $("#loadMore").remove()
            }
            $("#myCourses").append(res)
        }
    });
});