class Task {
    constructor(id, name, num_days, dates_obj) {
        this.id = parseInt(id);
        this.name = name;
        this.num_days = num_days;
        // Key: [int], 
        this.dates = {};
    }
    // Dates are selected/clicked dates
    add_dates(date_string_arr) {
        var day, date_string, i;
        for (i = 0; i < date_string_arr.length; i++) {
            date_string = date_string_arr[i];
            // Get last 2 chars for date
            day = parseInt(date_string.slice(-2));
            this.set_td(day, true);
        }
    }

    change_name(new_name) {
        this.name = new_name;
        var td = document.getElementById("task-td-" + this.id);
        td.innerText = new_name;
    }

    get_td_id(day) { return `td-${this.id}-${day}`; }
    get_tr_id() { return `tr-${this.id}`; }
    // Set td for the date to either selected or unselected
    set_td(day, status = null) {
        if (day === null)
            return;
        var td = document.getElementById(
                    this.get_td_id(day));

        if (status || !this.dates[day]) {
            td.classList.add("date-selected");
            this.dates[day] = true;
        } else {
            delete this.dates[day];
            td.classList.remove("date-selected");
        }   
    }
    create_task_tr() {
        var tr = document.createElement("tr"),
            td = document.createElement("td");

        tr.setAttribute("id", this.get_tr_id());

        td.innerText = this.name;
        td.setAttribute("value", this.id);
        td.setAttribute("id", "task-td-" + this.id);
        td.classList.add("taskname-td");
        this.edit_task_contextmenu(td);
        tr.append(td);

        for (var i = 0; i < this.num_days; i++) {
            td = document.createElement("td");
            td.setAttribute("id", this.get_td_id(i + 1));
            td.setAttribute("value", i + 1);
            td.classList.add("taskdate-td")
            tr.append(td);
            td.addEventListener("click", this.task_listener.bind(this));
        }
        return tr;
    }
    task_listener(e) {
        var td = e.target;
        if (td.classList.contains("taskdate-td")) {
            var day = parseInt(td.getAttribute("value"));
            if (typeof (day) == "number") {
                TController.set_taskdate(this.id, day, this.set_td.bind(this));
            }
        }
    }

    edit_task_contextmenu(task_td) {
        task_td.addEventListener("click",
            contextMenu.show_menu.bind(contextMenu));
    }
}