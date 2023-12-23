window.getValue = function (element) {
    if (element.tagName === "INPUT") {
        if (element.type === "checkbox") {
            return element.checked;
        } else {
            return element.value;
        }
    } else if (element.tagName === "SELECT") {
        return element.options[element.selectedIndex].value;
    } else if (element.tagName == "TEXTAREA") {
        return element.value;
    } else {
        return element.innerText;
    }
    return null;
}