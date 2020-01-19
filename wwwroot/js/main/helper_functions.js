var element_maker = {
    create_header_row(headerArr) {
        var tr = document.createElement("tr"),
            th;
        for (var i = 0; i < headerArr.length; i++) {
            th = document.createElement("th");
            th.innerText = headerArr[i];
            th.setAttribute("scope", "col");
            tr.append(th);
        }
        return tr;
    }
};

// Helper function object
var helper = {
    num_days(year, month) {
        var first_date = new Date(1, month, year),
            last_date = new Date(0, month, year);

        return last_date
    },

    // year, month, day, num_days [int]
    // Return date object
    get_dates: function (start_year, start_month, start_day, num_days) {

        var date_arr = [];
        var i = 0;

        for (i = 0; i < num_days; i++) {
            date_arr.push(new Date(start_year, start_month, start_day + i))
        }
        return date_arr;
    },

    get_num_days: function (start_date, end_date) {
        var start_utc = Date.UTC(start_date.getFullYear(), start_date.getMonth(), start_date.getDate()),
            end_utc = Date.UTC(end_date.getFullYear(), end_date.getMonth(), end_date.getDate());

        return Math.floor((end_date - start_date) / (24 * 60 * 60 * 1000) + 1);
    },

    date_verName: "dateVer",
    date_version: "0.0.4",
    get_date: function () {

        var stor = window.localStorage;

        var dStor = stor.getItem("checkTasks_jdate"),
            check_ver = stor.getItem(this.date_verName);

        if (dStor == undefined || dStor == null || check_ver != this.date_version) {
            var d = new Date();
            this.set_date(d.getFullYear(), d.getMonth(), d.getDate());
            return this.get_date();
        } else {
            return JSON.parse(dStor);
        }
    },
    set_date: function (year, month, date) {
        var stor = window.localStorage;

        stor.setItem("checkTasks_jdate", JSON.stringify({ "date": date, "year": year, "month": month, }));
        stor.setItem(this.date_verName, this.date_version);
    },
    setTodayDate: function () {
        var t = new Date();
        this.set_date(t.getFullYear(), t.getMonth(), t.getDate());

    },
    get_date_string: function (year, month, date) {
        return year + "_" + month + "_" + date;
    },
};
