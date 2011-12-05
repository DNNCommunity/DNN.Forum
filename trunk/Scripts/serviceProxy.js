/// <reference path="jquery.js" />
/*
ServiceProxy.js  
Version 0.981 - 4/5/11

(c) 2008-2011 Rick Strahl, West Wind Technologies 
www.west-wind.com

Load json2.js before this library
http://github.com/douglascrockford/JSON-js/blob/master/json2.js

Licensed under MIT License
http://en.wikipedia.org/wiki/MIT_License
*/
// set up a global filter to always parse JSON with
// date formatting applied
$.ajaxSetup({
    converters: { "text json": function (jsonString) {
        var res = JSON.parseWithDate(jsonString);
        if (res && res.hasOwnProperty("d"))
            res = res.d;
        return res;
    } 
    }
});
this.ServiceProxy = function (serviceUrl, options) {
    /// <summary>
    /// Generic Service Proxy class that can be used to
    /// call JSON Services generically using jQuery.
    /// Includes all dependencies.
    /// </summary>
    /// <param name="serviceUrl" type="string">
    /// The Url of the service ready to accept the method name
    /// should contain trailing slash (or other URL separator ?,&)
    /// </param>
    /// <example>
    /// var proxy = new ServiceProxy("JsonStockService.svc/");
    /// proxy.invoke("GetStockQuote",{symbol:"msft"},
    ///              function(quote) { alert(result.LastPrice); },onPageError);
    ///</example>
    var _I = this;
    this.serviceUrl = serviceUrl;
    this.isWcf = false;
    this.timeout = 10000;
    this.contentType = "application/json";
    $.extend(_I, typeof (serviceUrl) === "object" ? serviceUrl : options);
    this.invoke = function (method, params, callback, errorHandler) {
        /// <summary>
        /// Calls a WCF/ASMX service and returns the result.
        /// </summary>    
        /// <param name="method" type="string">The method of the service to call</param>
        /// <param name="params" type="object">An object that represents the parameters to pass {symbol:"msft",years:2}       
        /// <param name="callback" type="function">Function called on success. 
        /// Receives a single parameter of the parsed result value</parm>
        /// <param name="errorCallback" type="function">Function called on failure. 
        /// Receives a single error object with Message property</parm>        
        var jsonData = _I.isWcf ? JSON.stringifyWcf(params) : JSON.stringify(params);
        // Service endpoint URL        
        var url = _I.serviceUrl + method;
        $.ajax({
            url: url,
            data: jsonData,
            type: "POST",
            contentType: _I.contentType,
            timeout: _I.timeout,
            // use $.ajaxSetup converter above
            dataType: "json",
            success: function (result) {
                if (callback)
                    callback(result);
            },
            error: function (xhr, status) {
                var err = null;
                if (xhr.readyState == 4) {
                    var res = xhr.responseText;
                    if (res && res.charAt(0) == '{' && status != "parsererror")
                        var err = JSON.parse(res);
                    if (!err) {
                        if (xhr.status && xhr.status != 200)
                            err = new CallbackException(xhr.status + " " + xhr.statusText);
                        else {
                            if (status == "parsererror")
                                status = "Unable to parse JSON response.";
                            else if (status == "timeout")
                                status = "Request timed out.";
                            else if (status == "error")
                                status = "Unknown error";
                            err = new CallbackException("Callback Error: " + status);
                        }
                        err.detail = res;
                    }
                }
                if (!err)
                    err = new CallbackException("Callback Error: " + status);
                if (errorHandler)
                    errorHandler(err, _I, xhr);
            }
        });
    }
}
this.CallbackException = function (message, detail) {
    this.isCallbackError = true;
    if (typeof (message) == "object") {
        if (message.message)
            this.message = message.message;
        else if (message.Message)
            this.message = message.Message;
    }
    else
        this.message = message;
    if (detail)
        this.detail = detail;
    else
        this.detail = null;
}
if (this.JSON && !this.JSON.parseWithDate) {
    var reISO = /^(\d{4})-(\d{2})-(\d{2})T(\d{2}):(\d{2}):(\d{2}(?:\.\d*)?)Z$/;
    var reMsAjax = /^\/Date\((d|-|.*)\)[\/|\\]$/;
    JSON.parseWithDate = function (json) {
        /// <summary>
        /// parses a JSON string and turns ISO or MSAJAX date strings
        /// into native JS date objects
        /// </summary>    
        /// <param name="json" type="var">json with dates to parse</param>        
        /// </param>
        /// <returns type="value, array or object" />
        try {
            var res = JSON.parse(json,
            function (key, value) {
                if (typeof value === 'string') {
                    var a = reISO.exec(value);
                    if (a)
                        return new Date(Date.UTC(+a[1], +a[2] - 1, +a[3], +a[4], +a[5], +a[6]));
                    a = reMsAjax.exec(value);
                    if (a) {
                        var b = a[1].split(/[-+,.]/);
                        return new Date(b[0] ? +b[0] : 0 - +b[1]);
                    }
                }
                return value;
            });
            return res;
        } catch (e) {
            // orignal error thrown has no error message so rethrow with message
            throw new Error("JSON content could not be parsed");
            return null;
        }
    };
    JSON.stringifyWcf = function (json) {
        /// <summary>
        /// Wcf specific stringify that encodes dates in the
        /// a WCF compatible format ("/Date(9991231231)/")
        /// Note: this format works ONLY with WCF. 
        ///       ASMX can use ISO dates as of .NET 3.5 SP1
        /// </summary>
        /// <param name="key" type="var">property name</param>
        /// <param name="value" type="var">value of the property</param>         
        return JSON.stringify(json, function (key, value) {
            if (typeof value == "string") {
                var a = reISO.exec(value);
                if (a) {
                    var val = '/Date(' + new Date(Date.UTC(+a[1], +a[2] - 1, +a[3], +a[4], +a[5], +a[6])).getTime() + ')/';
                    this[key] = val;
                    return val;
                }
            }
            return value;
        })
    };
    JSON.dateStringToDate = function (dtString) {
        /// <summary>
        /// Converts a JSON ISO or MSAJAX string into a date object
        /// </summary>    
        /// <param name="" type="var">Date String</param>
        /// <returns type="date or null if invalid" /> 
        var a = reISO.exec(dtString);
        if (a)
            return new Date(Date.UTC(+a[1], +a[2] - 1, +a[3], +a[4], +a[5], +a[6]));
        a = reMsAjax.exec(dtString);
        if (a) {
            var b = a[1].split(/[-,.]/);
            return new Date(+b[0]);
        }
        return null;
    };
}