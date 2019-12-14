window.addEventListener("load", function (e) {
    TController.load();
});

var TController = {
    table: null,
    load() {
        table = new TaskTable();
    },
    get(url) {
        return new Promise(function (resolve, reject) {
            var req = new XMLHttpRequest();
            req.open("GET", url);

            req.onload = function () {
                if (req.status == 200) {
                    resolve(req.response);
                }
                else {
                    reject(Error(req.statusText));
                }
            }

            req.onerror = function () {
                reject(Error("Network Error"));
            }

            req.send();
        });
    },
    get_tasks(callback) {
        TController.get(window.location.href + '/get_tasks_json'
        ).then(function (response) {
            var tasks = {},
                result = JSON.parse(response),
                t, r, id, dates;
            for (id in result) {
                r = result[id]
                t = tasks[id] = {};
                t.name = r.name;

                dates = r.date_string.split("_");

                if (dates[dates.length - 1] == "") {
                    dates.pop();
                }
                t.dates = dates;
            }

            callback(tasks);
        });
    }
};