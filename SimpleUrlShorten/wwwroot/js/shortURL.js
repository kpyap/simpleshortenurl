
function requestShortUrl(originalUrl, requestUrl, resultSelector, outputSelector) {
    $(outputSelector).slideDown('slow');
    $.ajax({
        url: requestUrl,
        dataType: "json",
        async: true,
        jsonp: false,
        type: "POST",
        data: {
            originalUrl: originalUrl
        },
        beforeSend: function () {
            $(resultSelector).text('Processing...');
        },
        error: function (xhr, textStatus, errorThrown) {
            $(resultSelector).append(errorThrown);
        },
        success: function (data) {
            $(resultSelector).text("");
            $(resultSelector).removeClass("text-danger");
            if (data.status === true) {
                $(resultSelector).append('<p>Completed.</p>Your URL is: <a href="' + data.url.shortUrl + '" target="_blank">' + data.url.shortUrl + '</a>');
            } else {
                $(resultSelector).addClass("text-danger");
                $(resultSelector).append('<p>Error:</p>'+ data.message);
            }
        }
    });
}