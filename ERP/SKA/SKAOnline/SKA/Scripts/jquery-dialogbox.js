var selectedSenderId;
$(document).ready(function () {
//event: dialog onclose
$("#DialogSenders").bind("dialogclose", function () {
selectedSenderId = null;
});
});
browseSenders = function () {
$("#BtnResetFilter").click();
$("#DialogSenders").dialog({
width: 500,
height: 400,
title: "Senders - loading data..",
modal: true,
buttons: {
OK: useSender,
Cancel: function () {
$("#DialogSenders").dialog("close");
}
}
});
getSendersList();
};
getSendersList = function (contactName, organizationName) {
if (typeof contactName != "undefined" || typeof organizationName != "undefined") {
$("#BtnResetFilter").show();
}
if (typeof contactName == "undefined")
contactName = "";
if (typeof organizationName == "undefined")
organizationName = "";
$.post(
CSRDomain + "Incoming/GetSendersList",
{ ContactName: contactName, OrganizationName: organizationName },
function (result) {
$("#SendersList").html(result.actionMessage);
$("#DialogSenders").dialog({ title: "Senders" });
}
);
};
doFilter = function () {
getSendersList($('#FilterContactName').val(), $('#FilterOrganizationName').val());
};
resetFilter = function () {
getSendersList();
$("#BtnResetFilter").hide();
}
selectSender = function (senderId) {
$(".t-grid tbody tr").each(function (index) {
$(this).removeClass("selected");
});
$("#r" + senderId).addClass("selected");
selectedSenderId = senderId;
};
instantSelectSender = function (senderId) {
selectSender(senderId);
useSender();
};
useSender = function () {
if (selectedSenderId) {
$("#SenderId").val(selectedSenderId);
$("#SelectedSenderDescription").val($("#r" + selectedSenderId + " td:eq(0)").text() + ", " + $("#r" + selectedSenderId + " td:eq(1)").text());
$("#DialogSenders").dialog("close");
}
else {
alert("No sender is selected!");
}
};
cancelSender = function () {
$("#SelectedSenderDescription").val("");
$("#SenderId").val("");
};
openFormNewSender = function () {
//close senders list
$("#DialogSenders").dialog("close");
//open form of new sender
$("#FormNewSender form")[0].reset();
$("#MsgNewSender").text("");
$("#FormNewSender").dialog({
width: 420,
height: 250,
title: "New Sender",
resizable: false,
draggable: false,
modal: true,
buttons: {
Save: function () {
saveNewSender();
},
Cancel: function () {
$("#FormNewSender").dialog("close");
$("#DialogSenders").dialog("open");
}
}
});
};
saveNewSender = function () {
var emptyField = 0;
//check data of each control
$("input[type=text]", "#FormNewSender").each(function () {
if ($.trim($(this).val()) == "") { //if empty
emptyField++;
}
});
if (emptyField == 0) {
$.post(
CSRDomain + "Sender/CreateSender",
{
ContactName: $("#ContactName").val(),
OrganizationName: $("#OrganizationName").val(),
Address: $("#Address").val(),
Phone: $("#Phone").val()
},
function (result) {
//display new sender on form
$("#SenderId").val(result.ActionResultInteger);
$("#SelectedSenderDescription").val($("#ContactName").val()+", "+$("#OrganizationName").val());
//close new sender form
$("#FormNewSender").dialog("close");
}
);
}
else {
$("#MsgNewSender").show().text("Data is not complete!").delay(2000).fadeOut();
}
}