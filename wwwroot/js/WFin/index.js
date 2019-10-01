window.addEventListener("load", function () {
    ajax();

    // Get today's date
    var d = new Date();

    var wtable = new WTable("table_div", d.getMonth(), d.getFullYear());

    //$.ajax({
    //    url: "/WFin/GetData",
    //    type: "POST",
    //    contentType: 'application/json; charset=utf-8',
    //    data: JSON.stringify({ "year": "gggg", }),
    //    success: function (response) {
    //        console.log(response);
    //    },
    //});
});

function ajax() {
    var xhr = new XMLHttpRequest();

    xhr.open("POST", "./WFin/GetTasks?year=2019&month=9", true);
    xhr.setRequestHeader('Content-Type', 'application/json; charset=utf-8');

    xhr.onreadystatechange = function () {
        if (xhr.readyState == 4) {
            console.log(xhr.responseText);
        }
    };

    xhr.send(JSON.stringify({
        "year": "gggg",
    }));
}