
let count = 6;
let proCount = $("#loadBtn").next().val()
$(document).on("click", "#loadBtn", function () {
    $.ajax({
        url: "/Courses/LoadMore/",  /*backendde hara müraciet ediləcək yerdi*/
        type: "get",  /*// response*/
        data: {
            "skip": count
        },
        success: function (res) {    /* //backendden gələn cavabı hansı istiqamətə yönəltməkdi*/
            $("#myCourses").append(res)
            count += 6;
            if (proCount <= count) {
                $("#loadBtn").remove()
            }

        }
    });
});