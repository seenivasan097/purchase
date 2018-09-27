function validateFloatKeyPress(el, evt) {
    var charCode = (evt.which) ? evt.which : event.keyCode;
    var number = el.value.split('.');
    if (charCode != 46 && charCode > 31 && (charCode < 48 || charCode > 57)) {
        return false;
    }
    //just one dot
    if (number.length > 1 && charCode == 46) {
        return false;
    }
    //get the carat position
    var caratPos = getSelectionStart(el);
    var dotPos = el.value.indexOf(".");
    if (caratPos > dotPos && dotPos > -1 && (number[1].length > 1)) {
        return false;
    }
    return true;
}

//thanks: http://javascript.nwbox.com/cursor_position/
function getSelectionStart(o) {
    if (o.createTextRange) {
        var r = document.selection.createRange().duplicate()
        r.moveEnd('character', o.value.length)
        if (r.text == '') return o.value.length
        return o.value.lastIndexOf(r.text)
    } else return o.selectionStart
}

$.widget('custom.mcautocomplete', $.ui.autocomplete, {
    _create: function () {
        this._super();
        this.widget().menu("option", "items", "> :not(.ui-widget-header)");
    },
    _renderMenu: function (ul, items) {
        var self = this,
            thead;
        if (this.options.showHeader) {
            table = $('<div class="ui-widget-header" style="width:100%;background-color: #2eadd3 !important;color: #fff;height: 35px;"></div>');
            $.each(this.options.columns, function (index, item) {
                table.append('<span style="padding:4px 4px;float:left;width:' + item.width + ';">' + item.name + '</span>');
            });
            table.append('<div style="clear: both;"></div>');
            ul.append(table);
        }
        $.each(items, function (index, item) {
            self._renderItem(ul, item);
        });
    },
    _renderItem: function (ul, item) {
        var t = '',
            result = '';
        $.each(this.options.columns, function (index, column) {
            t += '<span style="padding:0px 4px;float:left;width:' + column.width + ';">' + item[column.valueField ? column.valueField : index] + '</span>'
        });
        result = $('<li></li>')
            .data('ui-autocomplete-item', item)
            .append('<div class="mcacAnchor" style="cursor:pointor;">' + t + '<div style="clear: both;"></div></div>')
            .appendTo(ul);
        return result;
    }
});

function getAppRandomNumber() {
    return Math.floor((Math.random() * 100000) + 1);
}

function checkIsNumber(num) {
    return !/[^.[0-9]]*/.test(num);
};

function checkIsZero(num) {
    return num <= 0;
};

var getQueryString = function (field, url) {
    var href = url ? url : window.location.href;
    var reg = new RegExp('[?&]' + field + '=([^&#]*)', 'i');
    var string = reg.exec(href);
    return string ? string[1] : null;
};