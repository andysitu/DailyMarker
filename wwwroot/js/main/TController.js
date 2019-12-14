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
            callback(JSON.parse(response));
        });
    }
};