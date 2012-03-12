var App = (function () {
    var $modal = null;

    return {
        wireControls: function (containerElementOrSelector) {
            $(containerElementOrSelector)
                .find('[placeholder]').placeholder().end()
                .find('[rel=tooltip]').tooltip().end();
        },
        resetForm: function (containerElementOrSelector) {
            var $form = $(containerElementOrSelector).filter('form');
            if ($form.size() === 0) return;

            // reset the form itself using plain old javascript
            $form.each(function () {
                this.reset();
            });

            // reset jQuery Validate's internals
            $form.validate().resetForm();

            // reset unobtrusive validation summary, if it exists
            $form.find("[data-valmsg-summary=true]")
                .removeClass("validation-summary-errors")
                .addClass("validation-summary-valid")
                .find("ul")
                .empty();

            // reset unobtrusive field level, if it exists
            $form.find("[data-valmsg-replace]")
                .removeClass("field-validation-error")
                .addClass("field-validation-valid")
                .empty();

            // reset bootstrap error support
            $form.find('.error').removeClass('error');
        },
        showModalForm: function (url, viewModel, serverErrorsObservableArray, onSuccess) {
            var self = this;

            // clear server errors in view model
            serverErrorsObservableArray([]);

            // fetch the form and inject it into a modal window
            $.get(url, function (response) {
                self.showModalContent(response.html, viewModel, onSuccess);
            });
        },
        showModalContent: function (html, viewModel, onSuccess) {
            var self = this;

            // hide existing modal
            self.hideModalContent();

            // show new modal
            $modal = $(html)
                .on('shown', function () {
                    $.validator.unobtrusive.parse('#modal');
                    self.wireControls('#modal');
                    if (viewModel) ko.applyBindings(viewModel, document.getElementById('modal'));
                    if (onSuccess) onSuccess();
                })
                .on('hidden', function () {
                    $(this).remove();
                })
                .modal();
        },
        hideModalContent: function () {
            if ($modal) $modal.modal('hide');
        },
        disableSubmitButton: function (form) {
            $('form :submit').attr({ disabled: 'disabled' });
            $(form).find(':submit').addClass('loading');
        },
        reenableSubmitButton: function (form) {
            $('form :submit').removeAttr('disabled');
            $(form).find(':submit').removeClass('loading');
        },
        submitModalForm: function (form, serverErrorsObservableArray, onSuccess, onFailure) {
            var self = this;

            // run validators first
            var $form = $(form);
            if (!$form.valid()) return;

            // clear prior server errors disable submit button to prevent multiple POSTs
            serverErrorsObservableArray([]);
            self.disableSubmitButton(form);

            $.ajax({
                type: 'POST',
                url: form.action,
                data: $(form).serialize(),
                success: function (response) {
                    if (response.redirectUrl) {
                        // if redirecting, then do so
                        window.location.href = response.redirectUrl;

                    } else if (response.success) {
                        // reenable submits, execute the callback, 
                        // and hide the modal window unless canceled (by returning false from callback)
                        self.reenableSubmitButton(form);
                        if (!onSuccess || (onSuccess && onSuccess(response)))
                            self.hideModalContent();

                    } else if (response.errors) {
                        // show server errors and re-enable the submit button and execute the callback
                        serverErrorsObservableArray(response.errors);
                        self.reenableSubmitButton(form);
                        if (onFailure) onFailure(response);
                    }
                },
                error: function () {
                    self.reenableSubmitButton(form);
                    alert('We\'re sorry, an error occurred on the server.');
                }
            });
        }
    };
})();

$(function () {
    App.wireControls(document);
});