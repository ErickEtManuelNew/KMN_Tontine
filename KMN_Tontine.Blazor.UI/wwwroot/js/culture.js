window.blazorCulture = {
    get: function () {
        return localStorage['culture'];
    },
    set: function (value) {
        localStorage['culture'] = value;
    }
};
