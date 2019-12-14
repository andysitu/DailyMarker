class TaskTable {
    constructor() {
        this.tasks = {};

        this.year;
        this.month;
        this.num_days;
        this.tasks_tbody;
        
        this.load();
        this.get_tasks();
    }
    load() {
        this. tasks_tbody = document.getElementById("tasks_tbody")
        this.set_date();
        this.create_header();
    }
    // Sets year, month, & num_days
    set_date() {
        var d = new Date(),
            month = d.getMonth(),
            year = d.getFullYear();

        this.month = month;
        this.year = year;

        var d1 = new Date(year, month + 1, 0);
        this.num_days = d1.getDate();
    }

    create_header() {
        var thead = document.getElementById("table_thead");
        
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

    add_tasks(tasks) {
        var id,
            taskObj, name, tr, dates, i, date_string;

        for (id in tasks) {
            taskObj = tasks[id];

            name = taskObj.name;

            this.create_Task(id, name);

            tr = this.tasks[id].create_tasks_tr();
            this.tasks_tbody.append(tr);

            this.tasks[id].add_dates(taskObj.dates);
        }
    }
}