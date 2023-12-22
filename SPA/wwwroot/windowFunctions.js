
function openNewWindow(url) {
    let count = 0;
    if (!window["opened_windows"])
        window["opened_windows"] = [];

    count = window["opened_windows"].length;
    window["opened_windows"].push({
        "url": url,
        "window": window.open(url, "_blank", "width=400,height=400")
    });
}

function printWindow(url) {
    var entry = window["opened_windows"].find((e, i) => {
        return e.url === url;
    });
    if (entry) {
        entry.window.print();
    }
        //delete window["opened_windows"][url];
}