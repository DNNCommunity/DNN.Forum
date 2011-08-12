if (typeof (dnnforums) == 'undefined') {
	dnnforums = {};
};
if (typeof (dnnforums.admin) == 'undefined') {
	dnnforums.admin = {};
};
if (typeof (dnnforums.history) == 'undefined') {
	dnnforums.history = {};
};
dnnforums.admin = {
    Init: function () {
        if (window.location.toString().indexOf('#') >= 0) {

            var sHash = window.location.hash.substring(1) + '&';
            var params = sHash.split('&');
            var view = params[0].split("=")[1];
            var param = params[1];
            if (param.indexOf('!') >= 0) {
                param = param.replace('params=', '');
            } else {
                param = param.split("=")[1];
            };
            dnnforums.admin.LoadView(view, param);
        };
    },
    LoadView: function (view, param, data, callback) {
        jQuery('.dnnForumActionArea').empty();
        jQuery('.dnnForumNav li').removeClass('selected');
        jQuery('#nav-' + view).addClass('selected');
        if (typeof (param) == 'undefined') {
            param = '';
        };

        dnnforums.history.Add('cpview=' + view + '&param=' + param, 'Forums - ' + view);
        jQuery('.dnnForumArea').css('display', 'none');
        jQuery('#' + view).show('fast');
        

    },
    ShowMsg: function (text, cls) {
        var w = jQuery('#dnnForumMsgArea').parent().css('width');
        jQuery('#dnnForumMsgArea div:first')
                .removeClass()
                .addClass(cls)
                .text(text);
        jQuery('#dnnForumMsgArea').css('width', w).show('slow');
    },
    HideMsg: function (callback) {
        jQuery("#dnnForumMsgArea").hide('fast', function () {
            jQuery('#dnnForumMsgArea div:first')
                .removeClass()
                .text();
            if (typeof (callback) === 'function') {
                callback.call(this);
            };
        });
    }
}
dnnforums.history = {
	Change: function (newLocation, historyData) {
		var historyMsg = (typeof historyData == "object" && historyData != null
			? historyStorage.toJSON(historyData)
			: historyData
			);
		if (newLocation.indexOf("cpview") >= 0) {
			var tmp = newLocation.split("&");
			var view = tmp[0].split("=")[1];
			var param = tmp[1];
			if (param.indexOf('!') >= 0) {
				param = param.replace('params=', '');
			} else {
				param = param.split("=")[1];
			};

			dnnforums.admin.LoadView(view, param);
		};
	},
	Add: function (loc, data) {
		window.dhtmlHistory.add(loc, data);
	},
	Init: function () {
		try {
			window.dhtmlHistory.initialize();
			window.dhtmlHistory.addListener(dnn_comm.history.Change);
		} catch (e) {
		}

	}
}
jQuery.fn.serializeObject = function () {
    var o = {};
    var a = this.serializeArray();
    $.each(a, function () {
        if (o[this.name] !== undefined) {
            if (!o[this.name].push) {
                o[this.name] = [o[this.name]];
            }
            o[this.name].push(this.value || '');
        } else {
            o[this.name] = this.value || '';
        }
    });
    return o;
};