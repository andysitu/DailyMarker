/**
 * Overall class that controls the frontend table displaying 
 * tasks. The actual tasks have separate class in Task, which
 * TaskTable contains in tasks dictionary.
 */
class TaskTable {
    constructor() {
        this.tasks = {}; // Contains Tasks class objects

        this.year;
        this.month;
        this.num_days;
        this.tasks_tbody; // Points to HTML element for easier ref.

        this.set_date();
    }
    // Load function after other elements are in place
    load_table() {
        this.tasks_tbody = document.getElementById("tasks_tbody");
        this.add_addtask_handler();
        this.add_chgMonBtn_handler();

        this.load_tasks();
    }
    // Replaces the header dates & reloads the tasks
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
    // and calls show_date to display it in the page
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
    // Changes the year & month on the webpage
    // by id "datetext-container".
    // Might need changing later to have reference rather than hardcoded id
    show_date(dateObj) {
        const month = dateObj.toLocaleString('default', { month: 'long' }),
            year = dateObj.getFullYear();
        var date_div = document.getElementById("datetext-container");
        date_div.innerText = month + " " + year;
    }
    // Removes the old headers & reloads it
    // Header contains "Tasks" th & dates
    // Uses element_maker.create_header_row to get tr element
    create_header() {
        // Removes children of thead element
        var thead = document.getElementById("table_thead");
        while (thead.firstChild)
            thead.removeChild(thead.firstChild);

        // Creates array with date integers
        var headerArr = ["Task"];
        for (var i = 1; i <= this.num_days; i++) {
            if (i < 10)
                headerArr.push("0" + i.toString());
            else
                headerArr.push(i);
        }
        var tr = element_maker.create_header_row(headerArr);
        thead.append(tr);
    }
    // Runs TController.get_tasks & uses add_tasks as callback
    get_tasks() {
        TController.get_tasks(this.add_tasks.bind(this));
    }
    // Creates Task Class Object
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
        var t, taskId,
            tasks = this.tasks;
        for (taskId in tasks) {
            this.remove_task(taskId);
        }
        t = this.tasks_tbody;
        while (t.firstChild) {
            t.removeChild(t.firstChild);
        }
    }

    add_task(id, name, selected_dates_arr) {
        this.create_Task(id, name); // Creates Task Class Object
        var tr = this.tasks[id].create_task_tr();
        this.tasks_tbody.append(tr);
        // Selected/Clicked dates for the task
        // Added here because TR needs to be added first
        if (selected_dates_arr.length > 0) { 
            this.tasks[id].add_dates(selected_dates_arr);
        }
            
    }
    // Acts as callback in TController.get_tasks
    // Runs add_task for each tasks
    add_tasks(tasks_dict) {
        var id, taskObj, name;

        for (id in tasks_dict) {
            taskObj = tasks_dict[id];

            name = taskObj.name;

            this.add_task(id, name, taskObj.dates);
        }
    }
    // Adds Click Handler to Add a task
    add_addtask_handler() {
        var btn = document.getElementById("add-btn");
        btn.addEventListener("click", function (e) {
            var name = window.prompt("Task Name:");

            if (name != null && name.length > 0) {
                TController.create_task(name, this.add_task);
            }
        }.bind(this));
    }
    // Add click handlers to change to prev/next month
    add_chgMonBtn_handler() {
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