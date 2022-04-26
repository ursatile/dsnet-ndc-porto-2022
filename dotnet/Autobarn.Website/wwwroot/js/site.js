$(document).ready(connectToSignalR);
function connectToSignalR() {
    console.log("Connecting to SignalR...");
    window.notificationDivs = new Array();
    var conn = new signalR.HubConnectionBuilder().withUrl("/hub").build();
    conn.on("DisplayAutobarnNotification", displayNotification);
    conn.start().then(function () {
        console.log("SignalR has started.");
    }).catch(function (err) {
        console.log(err);
    });
}
function sanitize(html) {
    return html.replace(/>/g, "&gt;").replace(/</g, "&lt;");    
}
function displayNotification(user, message) {
    console.log(message);
    let data = JSON.parse(message);
    let $target = $("#signalr-notifications");
    let $div = $(`<div>
        New car notification! ${sanitize(data.manufacturer)} 
${sanitize(data.model)}
(${data.year}, ${sanitize(data.color)}, cost ${data.price} ${sanitize(data.currencyCode)}<br />
<br />
<a href="/vehicles/details/${data.registration}">Click here for more!</a>
</div>`);
    $target.prepend($div);
    window.setTimeout(function () {
        $div.fadeOut(2000, function () {
            $div.remove();
        });
    }, 5000);
}