window.addEventListener("load", function (e) {
    TController.load();
    contextMenu.load();
    
    window.addEventListener("click", function (e) {
        // Clicking anywhere except task will hide contextmenu for it
        contextMenu.hide_menu(e);
    }, true);
    window.addEventListener("contextmenu", function (e) {
        // Right click also disables task contextmenu
        contextMenu.hide_menu(e);
    }, false);
});

// Controller-like object for Table. Creates TaskTable which creates Tasks
// Calls the async requests to the server
var TController = {
    tasktable: null,
    load() {
        this.tasktable = new TaskTable();
        this.tasktable.load_table();
    },
    // Helper function for Promise with getting url
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
    // Gets tasks dictionary & feeds it to callback function
    get_tasks(callback) {
        var url = window.location.href + '/get_tasks_json';
        url += "?year=" + this.tasktable.year
            + "&month=" + (this.tasktable.month + 1);
        TController.get(url
        ).then(function (response) {
            var tasks = {},
                // Result converted to JSON for callback
                result = JSON.parse(response),
                task, r, id, dates;
            // Split dates_string (1 string contains all the clicked dates)
            // into array
            for (id in result) {
                r = result[id]
                task = tasks[id] = {};
                task.name = r.name;

                dates = r.date_string.split("_");

                if (dates[dates.length - 1] == "") {
                    dates.pop();
                }
                task.dates = dates;
            }

            callback(tasks);
        });
    },
    // Used by Task Class, which also provides the callback
    set_taskdate(task_id, day_value, callback) {
        var xhr = new XMLHttpRequest();
        if (!get_tasks_url)
            var get_tasks_url = window.location.href + '/set_taskdate_ajax';
        xhr.open('POST', get_tasks_url);
        xhr.setRequestHeader("RequestVerificationToken",
            document.getElementById('RequestVerificationToken').value);
        xhr.setRequestHeader("Content-Type", "application/json;charset=UTF-8");

        xhr.onload = function () {
            if (xhr.status == 200) {
                if (callback)
                    callback(day_value);
            }
        }
        var tt = this.tasktable;

        xhr.send(JSON.stringify({
            task_id, task_id,
            day: day_value,
            month: tt.month + 1,
            year: tt.year,
        }));
    },
    // Used by Task Class, which also provides the callback
    create_task(task_name, addtask_callback) {
        var that = TController;
        var xhr = new XMLHttpRequest();
        if (!add_task_url)
            var add_task_url = window.location.href + '/add_task_ajax';
        xhr.open('POST', add_task_url);
        xhr.setRequestHeader("RequestVerificationToken",
            document.getElementById('RequestVerificationToken').value);

        xhr.onload = function () {
            if (xhr.status == 200) {
                if (addtask_callback) {
                    var taskObj = JSON.parse(xhr.response);
                    var name = taskObj["name"],
                        id = taskObj["id"];
                    addtask_callback.call(that.tasktable, id, name);
                }
            }
        }

        var fd = new FormData();
        fd.append("task_name", task_name);
        xhr.send(fd);
    },
    // Options (eg delete, edit name) for Tasks. Used by contextMenu.
    select_menuOption(taskId, option) {
        if (option == "delete") {
            this.delete_task(taskId);
        } else if (option == "edit") {
            var new_name = window.prompt("Name to change to?");
            if (new_name && new_name.length > 0) {
                this.edit_taskname(taskId, new_name);
            }
        }
    },
    delete_task(taskId) {
        var that = this;
        var xhr = new XMLHttpRequest();
        if (!add_task_url)
            var add_task_url = window.location.href + '/delete_task_ajax';
        xhr.open('POST', delete_task_url);
        xhr.setRequestHeader("RequestVerificationToken",
            document.getElementById('RequestVerificationToken').value);

        xhr.onload = function () {
            if (xhr.status == 200) {
                var taskObj = JSON.parse(xhr.response);
                that.tasktable.remove_task(taskId);
            }
        }

        var fd = new FormData();
        fd.append("task_id", taskId);
        xhr.send(fd);
    },
    edit_taskname(taskId, new_task_name) {
        var that = this;
        var xhr = new XMLHttpRequest();
        if (!add_task_url)
            var add_task_url = window.location.href + '/edit_taskname_ajax';
        xhr.open('POST', edit_task_url);
        xhr.setRequestHeader("RequestVerificationToken",
            document.getElementById('RequestVerificationToken').value);

        xhr.onload = function () {
            if (xhr.status == 200) {
                var taskObj = JSON.parse(xhr.response);
                
                that.tasktable.tasks[taskId].change_name(new_task_name);
            }
        }

        var fd = new FormData();
        fd.append("new_task_name", new_task_name);
        fd.append("task_id", taskId);
        xhr.send(fd);
    }
};