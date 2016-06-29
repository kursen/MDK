/**
PoliceNumber editable input.
Internally value stored as {bk1: "BA", bk2: "9999", bk3: "QQQ"}

@class policenumber
@extends abstractinput
@final
@example
<a href="#" id="policenumber" data-type="policenumber" data-pk="1">awesome</a>
<script>
$(function(){
$('#policenumber').editable({
url: '/post',
title: 'Masukkan nomor polisi #',
value: {
bk1: "BA", 
bk2: "9999", 
bk3: "QQQ"
}
});
});
</script>
**/
(function ($) {
    "use strict";

    var PoliceNumber = function (options) {
        this.init('policenumber', options, PoliceNumber.defaults);
    };

    //inherit from Abstract input
    $.fn.editableutils.inherit(PoliceNumber, $.fn.editabletypes.abstractinput);

    $.extend(PoliceNumber.prototype, {
        /**
        Renders input from tpl

        @method render() 
        **/
        render: function () {
            this.$input = this.$tpl.find('input');


        },

        /**
        Default method to show value in element. Can be overwritten by display option.
        
        @method value2html(value, element) 
        **/
        value2html: function (value, element) {


            if (!value) {
                $(element).empty();
                return;
            }
            var html = value.bk1.toUpperCase() + ' ' + value.bk2 + ' ' + value.bk3.toUpperCase();
            $(element).html(html);
        },

        /**
        Gets value from element's html
        
        @method html2value(html) 
        **/
        html2value: function (html) {
            return this.str2value(html);
        },

        /**
        Converts value to string. 
        It is used in internal comparing (not for sending to server).
        
        @method value2str(value)  
        **/
        value2str: function (value) {
            var str = '';
            if (value) {
                for (var k in value) {
                    str = str + k + ':' + value[k] + ';';
                }
            }
            return str;
        },

        /*
        Converts string to value. Used for reading value from 'data-value' attribute.
        
        @method str2value(str)  
        */
        str2value: function (str) {
            if (typeof str == 'object') {
                return str;
            }
            var str = new String(str);
            str = str.replace(/ +?/g, '');
            var re1 = '((?:[a-z][a-z]+))'; // Word 1
            var re3 = '(\\d+)'; // Integer Number 1
            var re5 = '((?:[a-z][a-z]+))'; // Word 2

            var p = new RegExp(re1 + re3 + re5, ["i"]);
            var m = p.exec(str);
            if (m != null) {
                var word1 = m[1];
                var int1 = m[2];
                var word2 = m[3];

            }

            return { bk1: word1, bk2: int1, bk3: word2 };
        },

        /**
        Sets value of input.
        
        @method value2input(value) 
        @param {mixed} value
        **/
        value2input: function (value) {

            if (!value) {
                return;
            }
            this.$input.filter('[name="bk1"]').val(value.bk1);
            this.$input.filter('[name="bk2"]').val(value.bk2);
            this.$input.filter('[name="bk3"]').val(value.bk3);
        },

        /**
        Returns value of input.
        
        @method input2value() 
        **/
        input2value: function () {
            return {
                bk1: this.$input.filter('[name="bk1"]').val(),
                bk2: this.$input.filter('[name="bk2"]').val(),
                bk3: this.$input.filter('[name="bk3"]').val()
            };
        },

        /**
        Activates input: sets focus on the first field.
        
        @method activate() 
        **/
        activate: function () {
            this.$input.filter('[name="bk1"]').focus();
        },

        /**
        Attaches handler to submit form in case of 'showbuttons=false' mode
        
        @method autosubmit() 
        **/
        autosubmit: function () {
            this.$input.keydown(function (e) {
                if (e.which === 13) {
                    $(this).closest('form').submit();
                }
            });
        }
    });

    PoliceNumber.defaults = $.extend({}, $.fn.editabletypes.abstractinput.defaults, {
        tpl: '<div class="editable-policenumber"><div class="input-group">' +
                    '<input type="text" name="bk1" class="form-control text-center" style="width:60px; text-transform: uppercase;" maxlength="2">' +
                    '<span class="input-group-btn" style="width:0px;"></span>' +
             '<input type="text" name="bk2" class="form-control text-center"  style="width:80px;;" maxlength="4">' +
             ' <span class="input-group-btn" style="width:0px;"></span>' +
             '<input type="text" name="bk3" class="form-control text-center" style="width:60px; text-transform: uppercase;" maxlength="3"></div></div>',

        inputclass: ''
    });

    $.fn.editabletypes.policenumber = PoliceNumber;

} (window.jQuery));