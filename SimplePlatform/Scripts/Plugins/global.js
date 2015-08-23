var udf = "undefined";
var baseURL = "";
/* StringBuilder */
function StringBuilder(value) { this.strings = new Array(""); this.append(value); };
StringBuilder.prototype.append = function (value) { if (value) this.strings.push(value); return this; };
StringBuilder.prototype.clear = function () { this.strings.length = 1; };
StringBuilder.prototype.toString = function (value) { this.append(value); return this.strings.join(""); };
/* NullCheck */
function IsNull(obj) { return (typeof (obj) == udf || obj == null); };
function IsNullOrEmpty(obj) { return (typeof (obj) == udf || obj == null || obj === ""); };
/* Array */
//Array.prototype.contains = function (element) { for (var i = 0; i < this.length; i++) if (this[i] == element) return true; return false; };
function GetPosXY(obj) { return { "eWidth": obj.width(), "eHeight": obj.height(), "x": obj.offset().left, "y": obj.offset().top, "scrollX": obj.scrollLeft(), "scrollY": obj.scrollTop(), "windowWidth": $(window).width(), "windowHeight": $(window).height(), "eOuterWidth": obj.outerWidth(true), "eOuterHeight": obj.outerHeight(true) }; };
/* Date Object Extension */
Date.prototype.monthNames = ["January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"];
Date.prototype.shortMonthNames = ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sept", "Oct", "Nov", "Dec"];
Date.prototype.mmddyyyy = function () {
    var yyyy = this.getFullYear().toString();
    var mm = (this.getMonth() + 1).toString(); // getMonth() is zero-based
    var dd = this.getDate().toString();
    return (mm + "/" + dd + "/" + yyyy); // padding
};
Date.prototype.getMonthName = function () { return this.monthNames[this.getMonth()]; };
Date.prototype.getShortMonthName = function () { return this.getMonthName().substr(0, 3); };
Date.prototype.getYear = function () { return this.getFullYear().toString().substr(2, 2); };
Date.prototype.addDays = function (days) { var dat = new Date(this.valueOf()); dat.setDate(dat.getDate() + days); return dat; };
Date.prototype.addMonths = function (months) { var dat = new Date(this.valueOf()); dat.setMonth(dat.getMonth() + months); return dat; };
Date.prototype.getMonthWeeks = function () {
    var year = this.getFullYear();
    var month = this.getMonth();
    var currentMonth = new Date(year, month, 1);
    var nextMonth = new Date(year, month + 1, 1);
    var days = (nextMonth - currentMonth) / (1000 * 60 * 60 * 24)
    var counter = 0;
    for (var i = 0; i < days ; i++) { if (currentMonth.addDays(i).getDay() == 1) { counter++; } }
    return counter;
}
Date.prototype.getQuarter = function () {
    var q = [1, 2, 3, 4];
    return q[Math.floor(this.getMonth() / 3)];
}
Date.prototype.getFirstDateByDay = function () {
    var startDate = new Date(this.valueOf());
    startDate.setDate(1);
    var dayNr = (startDate.getDay() + 6) % 7; // Get Date By Monday
    return ((startDate.getDate() + (6 - dayNr)) + (dayNr > 0 ? 1 : 0)); // Get Date By Monday
};
Date.prototype.getWeek = function () {
    // Create a copy of this date object  
    var target = new Date(this.valueOf());
    // ISO week date weeks start on monday  
    // so correct the day number  
    var dayNr = (this.getDay() + 6) % 7;
    // ISO 8601 states that week 1 is the week  
    // with the first thursday of that year.  
    // Set the target date to the thursday in the target week  
    target.setDate((target.getDate() - dayNr) + (dayNr > 0 ? 1 : 0));
    // Store the millisecond value of the target date  
    var firstThursday = target.valueOf();
    // Set the target to the first thursday of the year  
    // First set the target to january first  
    target.setMonth(0, target.getFirstDateByDay());
    // Not a thursday? Correct the date to the next thursday  
    if (target.getDay() != 1) { target.setMonth(0, 1 + ((1 - target.getDay()) + 7) % 7); }
    // The weeknumber is the number of weeks between the   
    // first thursday of the year and the thursday in the target week  
    return 1 + Math.ceil((firstThursday - target) / 604800000); // 604800000 = 7 * 24 * 3600 * 1000  
};
Date.prototype.getFirstWeek = function () {
    var startDate = new Date(this.valueOf());
    startDate.setDate(1);
    var dayNr = (startDate.getDay() + 6) % 7; // Get Date By Monday
    startDate.setDate((startDate.getDate() + (6 - dayNr)) + (dayNr > 0 ? 1 : 0)); // Get Date By Monday
    return startDate.getWeek();
};
String.prototype.trunc = String.prototype.trunc || function (n) { return this.length > n ? this.substr(0, n - 1) + '&hellip;' : this.toString(); };
String.prototype.cleanText = String.prototype.cleanText || function () { return this.replace(new RegExp("[\&\–\.\/ ]", "g"), ""); };
function BaseURL() { if (typeof (window.baseURLValue) == udf || window.baseURLValue == "") window.baseURLValue = baseURL; return window.baseURLValue; };
function GetBrowserTime() { return (new Date()).toLocaleDateString(); };
function GetWeeks(startDate, stopDate) {
    var dateArray = new Array();
    startDate = startDate.split("|");
    endDate = stopDate.split("|");
    var startWeek = parseInt(startDate[0]);
    var startYear = parseInt(startDate[1]);
    var endWeek = parseInt(endDate[0]);
    var endYear = parseInt(endDate[1]);
    var currentYear = startYear;
    var maxWeeks = 52;
    var currentWeek = startWeek;
    while (true) {
        var obj = new Object();
        obj.Lable = currentYear + " " + (currentWeek.toString().length < 2 ? ("0" + currentWeek.toString()) : currentWeek.toString());
        obj.Value = currentYear + "" + (currentWeek.toString().length < 2 ? ("0" + currentWeek.toString()) : currentWeek.toString());
        dateArray.push(obj);
        if (currentWeek == endWeek && currentYear == endYear) { break; };
        if (currentWeek == maxWeeks) { currentWeek = 1; currentYear = (currentYear + 1); }
        else currentWeek++;
    };
    return dateArray;
};
function GetMonthsWithYear(startDate, stopDate) {
    var dateArray = new Array();
    var currentDate = startDate;
    while (currentDate <= stopDate) {
        var obj = new Object();
        obj.Lable = currentDate.getShortMonthName() + "-" + currentDate.getYear();
        obj.Value = currentDate.getShortMonthName() + " " + currentDate.getFullYear();
        dateArray.push(obj);
        currentDate = currentDate.addMonths(1);
    }; return dateArray;
};
function GetQuartersWithYear(startDate, stopDate) {
    var quarterArray = new Array();
    var currentDate = startDate;
    var previousQuarter = 0;
    while (currentDate <= stopDate) {
        var quarter = Math.ceil((currentDate.getMonth() + 1) / 3);
        if (quarter != previousQuarter) {
            previousQuarter = quarter;
            var obj = new Object();
            obj.Lable = "Q" + quarter + " " + currentDate.getYear();
            obj.Value = quarter + "Q" + currentDate.getYear();
            quarterArray.push(obj);
        };
        currentDate = currentDate.addMonths(1);
    }; return quarterArray;
};
function GetYears(startDate, stopDate) {
    var quarterArray = new Array();
    var currentDate = startDate;
    var previousYear = 0;
    while (currentDate <= stopDate) {
        var currentYear = currentDate.getFullYear();
        if (currentYear != previousYear) {
            previousYear = currentYear;
            var obj = new Object();
            obj.Lable = "Year " + currentYear;
            obj.Value = currentYear;
            quarterArray.push(obj);
        };
        currentDate = currentDate.addMonths(1);
    }; return quarterArray;
};
function GetCleanArray(value) {
    if (value.length <= 0) { return new Array(); };
    var objArray = new Array();
    for (var i = 0; i < value.length; i++) {
        if (IsNullOrEmpty(value[i])) continue;
        objArray.push(value[i]);
    };
    return objArray;
};
function GetSelectedOptionTextFromDropDown(obj) { if (IsNullOrEmpty(obj)) return new Array(); var values = new Array(); obj.find("option:selected").each(function () { if (!IsNullOrEmpty($(this).text())) values.push($(this).text()); }); return values; };
function ReplaceAll(value, findText, replaceText) { while (value.indexOf(findText) >= 0) value = value.replace(findText, replaceText); return value; };
function RoundValue(value, decimalPoint) { return (decimalPoint > 0 ? Math.round(value * Math.pow(10, decimalPoint)) / Math.pow(10, decimalPoint) : Math.round(value)); }
function ConvertValueToThousandMillionsBillions(value) {
    var tempValue = value;
    if (tempValue < 0) { tempValue *= -1; }
    var convertType = (tempValue < (1000 * 100) ? 1 : (tempValue < (1000 * (1000 * 1000)) ? 2 : (tempValue < (1000 * (1000 * 1000) * 1000) ? 3 : 4)));
    var obj = new Object();
    obj.OriginalValue = value;
    obj.Value = (convertType == 1 ? (RoundValue(value, 0)) : (convertType == 2 ? (value / (1000 * 1000)) : (convertType == 3 ? (value / (1000 * (1000 * 1000))) : (value / (1000 * (1000 * 1000) * 1000)))));
    obj.ValueIn = (convertType == 1 ? "Thousand" : (convertType == 2 ? "Million" : (convertType == 3 ? "Billion" : "Trillion")));
    obj.ValueInShort = (convertType == 1 ? "" : (convertType == 2 ? "M" : (convertType == 3 ? "B" : "T")));
    return obj;
};
function ShowDialogBox(dataModalPlaceHolder, dataModalURL, dataPayLoad, callbackFunction) {
    dataModalPlaceHolder.modal({ show: true });
    dataModalPlaceHolder.empty().load(dataModalURL, $.proxy(function () {
        dataModalPlaceHolder.off('show.bs.modal').on('show.bs.modal', $.proxy(function (event) {
            callbackFunction(event, dataModalPlaceHolder);
        }, this)).modal("show");
    }, this))
};
function ShowOkDialogBox(dataModalPlaceHolder, title, message, successCallbackFunction, returnURL) {
    var sb = new StringBuilder();
    sb.append("<div class=\"modal-dialog divUserModalDialog\">");
    sb.append("<div class=\"modal-content\">");
    sb.append("<div class=\"modal-header bg-primary\">");
    sb.append("<button type=\"button\" class=\"close\" data-dismiss=\"modal\" aria-hidden=\"true\">&times;</button>");
    sb.append("<h4 class=\"modal-title\">" + title + "</h4>");
    sb.append("</div>");
    sb.append("<div class=\"modal-body\">");
    sb.append(message);
    sb.append("</div>");
    sb.append("<div class=\"modal-footer\">");
    sb.append("<button type=\"button\" class=\"btn btn-primary\" id=\"btnOk\">Ok</button>");
    sb.append("</div>");
    sb.append("</div>");
    sb.append("</div>");
    dataModalPlaceHolder.empty().append(sb.toString());
    dataModalPlaceHolder.modal({ show: true });
    dataModalPlaceHolder.modal("show");
    dataModalPlaceHolder.find("#btnOk").off("click.dataModel").on("click.dataModel", function (event) { dataModalPlaceHolder.modal("hide"); if (!IsNullOrEmpty(successCallbackFunction)) successCallbackFunction(event, dataModalPlaceHolder); if (!IsNullOrEmpty(returnURL)) window.location.href = returnURL; });
};
function ShowOkCancelDialogBox(dataModalPlaceHolder, title, message, okCallbackFunction, cancelCallbackFunction, returnURL) {
    var sb = new StringBuilder();
    sb.append("<div class=\"modal-dialog divUserModalDialog\">");
    sb.append("<div class=\"modal-content\">");
    sb.append("<div class=\"modal-header bg-primary\">");
    sb.append("<button type=\"button\" class=\"close\" data-dismiss=\"modal\" aria-hidden=\"true\">&times;</button>");
    sb.append("<h4 class=\"modal-title\">" + title + "</h4>");
    sb.append("</div>");
    sb.append("<div class=\"modal-body\">");
    sb.append(message);
    sb.append("</div>");
    sb.append("<div class=\"modal-footer\">");
    sb.append("<button type=\"button\" class=\"btn btn-primary\" id=\"btnOk\">Ok</button>");
    sb.append("<button type=\"button\" class=\"btn btn-primary\" id=\"btnCancel\">Cancel</button>");
    sb.append("</div>");
    sb.append("</div>");
    sb.append("</div>");
    dataModalPlaceHolder.empty().append(sb.toString());
    dataModalPlaceHolder.modal({ show: true });
    dataModalPlaceHolder.modal("show");
    dataModalPlaceHolder.find("#btnOk").off("click.dataModel").on("click.dataModel", function (event) { dataModalPlaceHolder.modal("hide"); if (!IsNullOrEmpty(okCallbackFunction)) okCallbackFunction(event, dataModalPlaceHolder); if (!IsNullOrEmpty(returnURL)) window.location.href = returnURL; });
    dataModalPlaceHolder.find("#btnCancel").off("click.dataModel").on("click.dataModel", function (event) { dataModalPlaceHolder.modal("hide"); if (!IsNullOrEmpty(cancelCallbackFunction)) cancelCallbackFunction(event, dataModalPlaceHolder); });
};
function ShowUserAlert(message) {
    if ($("#divUserAlert").length > 0) $("#divUserAlert").alert('close');
    var sb = new StringBuilder();
    sb.append("<div id=\"divUserAlert\" class=\"alert alert-info alert-dismissible divUserAlert\" role=\"alert\">");
    sb.append("<button type=\"button\" class=\"close\" data-dismiss=\"alert\" aria-label=\"Close\"><span aria-hidden=\"true\">&times;</span></button>");
    sb.append("<strong>Heads up!</strong>&nbsp;" + message);
    sb.append("</div>");
    $("body").prepend($(sb.toString()));
    setTimeout(function () { $("#divUserAlert").alert('close'); }, 4000);
};