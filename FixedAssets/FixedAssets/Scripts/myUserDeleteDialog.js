var userid = '';
var username = '';

$(function () {
    $("#dialog-confirm").dialog({
        resizable: false,
        autoOpen: false,
        height: "auto",
        width: "auto",
        modal: true,
        buttons: {
            "Usuń": function () {
                $(this).dialog("close");
                event.preventDefault();
                document.location.href = "/Admin/Delete/" + userid;
            },
            "Anuluj": function () {
                $(this).dialog("close");
            }
        }
    });
});

function actionDelete(id, user) {
    userid = id;
    username = user;
    $("#confirmchapter").text('Czy na pewno usunąć użytkownika ' + username + '?');
    $("#dialog-confirm").dialog("open")
};



