$(document).ready(connectToSignalR);
function connectToSignalR() {
    console.log("Connecting to SignalR...");
    //window.notificationDivs = new Array();
    var conn = new signalR.HubConnectionBuilder().withUrl("/hub").build();
    conn.on("DisplayAutobarnNotification", function (user, message) {
        console.log(user);
        var data = JSON.parse(message);
        console.log(data);
    });
    conn.start().then(function () {
        console.log("SignalR has started.");
    }).catch(function (err) {
        console.log(err);
    });
}
