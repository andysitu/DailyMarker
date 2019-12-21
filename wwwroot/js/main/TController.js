window.addEventListener("load", function (e) {
    TController.load();
    contextMenu.load();

    window.addEventListener("click", function (e) {
        contextMenu.hide_menu(e);
    }, true);
});

var TController = {
    tasktable: null,
    load() {
        this.tasktable = new TaskTable();
        this.tasktable.load_table();
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
        var url = window.location.href + '/get_tasks_json';
        url += "?year=" + this.tasktable.year
            + "&month=" + (this.tasktable.month + 1);
        TController.get(url
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
    },
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
    edit_taskname(taskId, task_name) {
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
                
                that.tasktable.tasks[taskId].change_name(task_name);
            }
        }

        var fd = new FormData();
        fd.append("task_name", task_name);
        fd.append("task_id", taskId);
        xhr.send(fd);
    }
};

var contextMenu = {
    taskId: null,
    load() {
        this.set_click_option();
    },
    visible_status: false,
    toggleMenu(command) {
        menu.style.display = (command === "show") ? "block" : "none";
        this.visible_status = (command === "show") ? true : false;
    },
    hide_menu(e) {
        if (this.visible_status)
            this.toggleMenu("hide");
    },
    setPosition({ top, left }) {
        menu.style.left = `${left}px`;
        menu.style.top = `${top}px`;
        this.toggleMenu("show");
    },
    show_menu(e) {
        this.taskId = parseInt(e.target.getAttribute('value'));
        e.preventDefault();
        const pos = {
            left: e.pageX,
            top: e.pageY
        };
        this.setPosition(pos);
    },
    set_click_option() {
        var options = document.querySelectorAll('.menu-option'),
            o;
        for (var i = 0, len = options.length; i < len; i++) {
            o = options[i];
            o.addEventListener("click", function (e) {
                TController.select_menuOption(
                    this.taskId, e.target.getAttribute('value'));
            }.bind(this));
        }
    }
};