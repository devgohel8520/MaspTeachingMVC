function validateForm(btnSubmit) {
    var $form = $(btnSubmit).parents('form');
    var validator = $form.validate();

    var anyError = false;

    $form.find("input").each(function () {
        if (!validator.element(this)) {
            anyError = true;
        }
    });

    if (anyError)
        return false;
    else
        return true;

}

function submitUniversal(btnSubmit, target) {

    if (!validateForm(btnSubmit))
        return false;

    var btnSubmitvar = $(btnSubmit);

    var $form = btnSubmitvar.parents('form');

    btnSubmitvar.attr("disabled", "disabled");

    var errorDiv = $("#errorD");
    errorDiv.show();
    errorDiv.text("wait...");

    $.ajax({
        type: "POST",
        url: $form.attr('action'),
        data: $form.serialize(),
        error: function (xhr, status, error) {
            errorDiv.text(error);
            btnSubmitvar.removeAttr("disabled");
        },
        success: function (data) {
            btnSubmitvar.removeAttr('disabled');
            if (data == "") {
                location.href = target;
            }
            else {
                errorDiv.text(data);
                errorDiv.show();
            }
        }
    });
}