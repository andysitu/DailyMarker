class TaskTable {
    constructor() {
        this.num_days = 0;
        this.tasks_tbody = document.getElementById("tasks_tbody");
        this.create_header();
        this.get_tasks();
    }

    create_header() {
        var thead = document.getElementById("table_thead");

        var d = new Date(),
            month = d.getMonth(),
            year = d.getFullYear();
        var d1 = new Date(year, month + 1, 0);
        this.num_days = d1.getDate();

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

    add_tasks(tasks) {
        console.log(tasks);
        var id,
            task, name, tr;

        for (id in tasks) {
            task = tasks[id];

            name = task.name;
            tr = this.create_tasks_tr(id, name);
            this.tasks_tbody.append(tr);
        }
    }

    create_tasks_tr(id, name) {
        var tr = document.createElement("tr"),
            td = document.createElement("td");

        td.innerText = name;
        td.setAttribute("value", id);
        tr.append(td);

        for (var i = 0; i < this.num_days; i++) {
            td = document.createElement("td");
            tr.append(td);
        }
        return tr;
    }

}