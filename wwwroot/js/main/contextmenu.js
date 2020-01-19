var contextMenu = {
    taskId: null,
    load() {
        this.set_click_option();
    },
    visible_status: false,
    // Toggling showing/hiding context menu
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