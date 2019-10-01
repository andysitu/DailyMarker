/**********************************************************
 * Class that handles the table creation on the pages.
 * Table will display tha tasks and dates.
 **********************************************************/
class WTable {
    constructor(div_id, month, year ) {
        this.month = month;
        this.year = year;
        this.tableContainerId = div_id;

        this.createTable();
    }

    createTable() {
        var div = document.getElementById(this.tableContainerId);
        var table = document.createElement("table");
        table.classList.add("table");

        // Get # of days on month
        var d = new Date(this.year, this.month + 1, 0),
            numDays = d.getDate();

        var thead = document.createElement("thead"),
            tr = document.createElement("tr"),
            th = document.createElement("th");

        th.setAttribute("scope", "col");
        th.innerText = "Task";
        tr.append(th);

        for (let i = 1; i <= numDays; i++) {
            th = document.createElement("th");
            th.setAttribute("scope", "col");
            th.innerText = i;
            tr.append(th);
        }

        thead.append(tr);
        table.append(thead);

        div.append(table);
    }

    deleteTable() {

    }
}