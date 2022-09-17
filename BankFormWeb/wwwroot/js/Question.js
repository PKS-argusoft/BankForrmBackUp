
$(document).ready(function () {

    let elementselector = document.getElementsByTagName("td");
    for (let i = 0; i < elementselector.length; i++) {

        if (i % 6 == 0) {
            var checkOptionViewSetter = setterEnableFeature(elementselector[i + 2].innerText);
            console.log(checkOptionViewSetter);
            if (checkOptionViewSetter != null) {
                console.log(checkOptionViewSetter);
                elementselector[i + 3].querySelector('div').innerHTML = checkOptionViewSetter;
            }
        }


    }
    
});

function setterEnableFeature(p1) {

    var b = true;

    switch (p1) {
        case "Single Line Textbox":
            b = false;
            break;
        case "Multi line Textbox":
            b = false;
            break;
        case "Date":
            b = false;
            break;
        case "Number":
            b = false;
            break;
        case "Yes/No":
            b = false;

    }

    if (b == false) {
        return '<div class="dropdown show QuestionOptionSet"  disabled> </div>'
    }
    else { return null };


}

