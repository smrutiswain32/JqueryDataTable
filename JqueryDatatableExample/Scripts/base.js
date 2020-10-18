
function Callreport(baseurl, userrole, datasavemsg, updatemesg) {

    this.baseurl = baseurl;
    this.userrole = userrole;
    this.datasavemsg = datasavemsg;
    this.updatemesg = updatemesg;
}
$(document).bind("ajaxSend", function (e) {
    //$("#loader").show();

}).bind("ajaxComplete", function () {
    //$("#loader").hide();

    });
var baseObj = new Callreport();


