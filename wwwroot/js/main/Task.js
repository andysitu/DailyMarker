class Task {
    constructor(id, name, num_days) {
        this.id = id;
        this.name = name;
        this.num_days = num_days;
        // Key: [int], 
        this.dates = {};
    }

    add_dates(date_string_arr) {
        var day, date_string, i;
        for (i = 0; i < date_string_arr.length; i++) {
            date_string = date_string_arr[i];
            // Get last 2 chars for date
            day = parseInt(date_string.slice(-2));
            this.set_td(day, true);
        }
    }

    get_td_id(day) {
        return "td-" + this.id + "-" + day;
    }

    set_td(day, status = null) {
        var td = document.getElementById(
                    this.get_td_id(day));

        td.classList.add("date-selected");
    }

    create_tasks_tr() {
        var tr = document.createElement("tr"),
            td = document.createElement("td");

        td.innerText = this.name;
        td.setAttribute("value", this.id);
        tr.append(td);

        for (var i = 0; i < this.num_days; i++) {
            td = document.createElement("td");
            td.setAttribute("id", this.get_td_id(i+1));
            tr.append(td);
        }

        tr.addEventListener("click", this.task_listener.bind(this));
        return tr;
    }

    task_listener(e) {
        console.log(e.target);
    }
}