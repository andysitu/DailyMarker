class TaskTable {
    constructor() {
        this.tasks = {};

        this.year;
        this.month;
        this.num_days;
        this.tasks_tbody;

        this.set_date();
    }
    // Load function after other elements are in place
    load_table() {
        this.tasks_tbody = document.getElementById("tasks_tbody");
        this.add_addtask_handler();
        this.add_monthbtn_handler();

        this.load_tasks();
    }
    load_tasks() {
        this.create_header();
        this.clear_tasks();
        this.get_tasks();
    }

    change_month(num_months) {
        var month = this.month + num_months;
        this.set_date(this.year, month);
        this.load_tasks();
    }

    // Sets year, month, & num_days
    set_date(year, month) {
        if (year == null) {
            var d = new Date(),
                month = d.getMonth(),
                year = d.getFullYear();
        }

        var d1 = new Date(year, month + 1, 0);
        this.month = d1.getMonth();
        this.year = d1.getFullYear();
        this.num_days = d1.getDate();
        this.show_date(d1);
    }

    show_date(dateObj) {
        const month = dateObj.toLocaleString('default', { month: 'long' }),
            year = dateObj.getFullYear();
        var date_div = document.getElementById("datetext-container");
        date_div.innerText = month + " " + year;
    }

    create_header() {
        var thead = document.getElementById("table_thead");
        while (thead.firstChild)
            thead.removeChild(thead.firstChild);
        
        var headerArr = ["Task"];
        for (var i = 1; i <= this.num_days; i++) {
            headerArr.push(i);
        }
        var tr = element_maker.create_header_row(headerArr);
        thead.append(tr);
    }

    get_tasks() {
        TController.get_tasks(this.add_tasks.bind(this));
    }

    create_Task(id, name) {
        this.tasks[id] = new Task(id, name, this.num_days);
    }

    remove_task(task_id) {
        var t = this.tasks[task_id];
        var tr = document.getElementById(t.get_tr_id());
        this.tasks_tbody.removeChild(tr);

        delete this.tasks[task_id];
    }

    clear_tasks() {
        var t;
        for (taskId in this.tasks) {
            this.remove_task(taskId);
        }
        t = this.tasks_tbody;
        while (t.firstChild) {
            t.removeChild(t.firstChild);
        }
    }

    add_task(id, name, dates_obj) {
        this.create_Task(id, name);

        var tr = this.tasks[id].create_task_tr();
        this.tasks_tbody.append(tr);
        if (dates_obj)
            this.tasks[id].add_dates(dates_obj);
    }

    add_tasks(tasks) {
        var id,
            taskObj, name;

        for (id in tasks) {
            taskObj = tasks[id];

            name = taskObj.name;

            this.add_task(id, name, taskObj.dates);
        }
    }

    add_addtask_handler() {
        var btn = document.getElementById("add-btn");
        btn.addEventListener("click", function (e) {
            var name = window.prompt("Task Name:");

            if (name != null && name.length > 0) {
                TController.create_task(name, this.add_task);
            }
        }.bind(this));
    }

    add_monthbtn_handler() {
        var prevbtn = document.getElementById("prev-month-btn"),
            nextbtn = document.getElementById("next-month-btn");
        prevbtn.addEventListener("click", function (e) {
            this.change_month(-1);
        }.bind(this));

        nextbtn.addEventListener("click", function (e) {
            this.change_month(1);
        }.bind(this));
    }
}