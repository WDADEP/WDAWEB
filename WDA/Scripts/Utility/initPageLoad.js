
function showMessage() {
    var HiddenMessage = $('#MainContent_HiddenMessage');
    if (HiddenMessage.val() != null) {
        if (HiddenMessage.val().length > 0) {
            var divMessageBase = $('#divMessageBoxBase');
            var btnMessageClose = $('#btnMessageBoxClose');
            if (divMessageBase.length) {
                var tdMessage = $('#tdMessageBoxBody');
                if (tdMessage.length) tdMessage.html(HiddenMessage.val());
                if ($('#divMessageBoxBase').dialog.length) {
                    $("#divMessageBoxBase").dialog({
                        position: ["center", "center"],
                        autoOpen: true,
                        width: 400,
                        buttons: [
                            {
                                text: "關閉",
                                click: function () {
                                    $(this).dialog("close");
                                }
                            }
                        ]
                    });
                    HiddenMessage.val('');
                };
            };
        };
    };
};


function LoginShowMessage() {
    var HiddenMessage = $('#HiddenMessage');
    if (HiddenMessage.val() != null) {
        if (HiddenMessage.val().length > 0) {
            var divMessageBase = $('#divMessageBoxBase');
            var btnMessageClose = $('#btnMessageBoxClose');
            if (divMessageBase.length) {
                var tdMessage = $('#tdMessageBoxBody');
                if (tdMessage.length) tdMessage.html(HiddenMessage.val());
                if ($('#divMessageBoxBase').dialog.length) {
                    $("#divMessageBoxBase").dialog({
                        autoOpen: true,
                        width: 400,
                        buttons: [
                            {
                                text: "關閉",
                                click: function () {
                                    $(this).dialog("close");
                                }
                            }
                        ]
                    });
                    HiddenMessage.val('');
                };
            };
        };
    };
};

