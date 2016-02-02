$(function () {
    $('[data-date]').each(function () {
        var localDate = new Date(parseInt($(this).attr('data-date')));
        var now = new Date();
        if (localDate.getDate() == now.getDate() && localDate.getFullYear() == now.getFullYear() && now.getMonth() == localDate.getMonth()) {
            $(this).html(localDate.toLocaleTimeString());
        } else {
            $(this).html(localDate.toLocaleString());
        }
    });
});