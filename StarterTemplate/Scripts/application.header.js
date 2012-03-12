App.HeaderViewModel = function () {
    var self = this;

    self.logInFormServerErrors = ko.observableArray();
    self.signUpFormServerErrors = ko.observableArray();
    self.forgotPasswordFormServerErrors = ko.observableArray();
    self.changePasswordFormServerErrors = ko.observableArray();
    self.authFormActivePanel = ko.observable('');

    self.authFormModalHeader = ko.computed(function() {
        switch (self.authFormActivePanel()) {
        case 'log-in':
            return 'Log In';
        case 'sign-up':
            return 'Sign Up';
        case 'forgot-password':
            return 'Forgot Your Password?';
        case 'forgot-password-confirmation':
            return 'Check Your Email';
        case 'change-password':
            return 'Change Your Password';
        default:
            return '';
        }
    });
};

App.HeaderViewModel.prototype = {
    getLogInForm: function (data, evt) {
        var self = this;
        App.showModalForm(evt.target.href, self, self.logInFormServerErrors, function () {
            self.showLogInForm();
        });
    },
    showLogInForm: function () {
        this.logInFormServerErrors([]);
        App.resetForm('#logInForm');
        this.authFormActivePanel('log-in');
    },
    submitLogInForm: function (form) {
        var self = this;
        App.submitModalForm(form, this.logInFormServerErrors, function (response) {
            if (response.model && response.model.mustChangePassword)
                self.showChangePasswordForm();
            else
                window.location.reload();
        });
    },
    getSignUpForm: function (data, evt) {
        var self = this;
        App.showModalForm(evt.target.href, self, self.signUpFormServerErrors, function () {
            self.showSignUpForm();
        });
    },
    showSignUpForm: function () {
        this.signUpFormServerErrors([]);
        App.resetForm('#signUpForm');
        this.authFormActivePanel('sign-up');
    },
    submitSignUpForm: function (form) {
        App.submitModalForm(form, this.signUpFormServerErrors, function () {
            window.location.reload();
        });
    },
    showForgotPasswordForm: function () {
        this.forgotPasswordFormServerErrors([]);
        App.resetForm('#forgotPasswordForm');
        this.authFormActivePanel('forgot-password');
    },
    submitForgotPasswordForm: function (form) {
        var self = this;
        App.submitModalForm(form, this.forgotPasswordFormServerErrors, function () {
            self.authFormActivePanel('forgot-password-confirmation');
            return false;
        });
    },
    showChangePasswordForm: function () {
        this.changePasswordFormServerErrors([]);
        App.resetForm('#changePasswordForm');
        this.authFormActivePanel('change-password');
    },
    submitChangePasswordForm: function (form) {
        App.submitModalForm(form, this.changePasswordFormServerErrors, function () {
            window.location.reload();
        });
    }
};

$(function() {
    ko.applyBindings(new App.HeaderViewModel(), document.getElementById('page-header'));
});