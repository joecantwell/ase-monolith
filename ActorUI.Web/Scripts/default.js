
/**
 * @method HttpRequest provides get & post methods and returns the result in a callback
 * @param url - request endpoint
 * @param data - data to send to the server
 * @param cache - can be set to true or false
 * @param callback - function that receives the result
 */
var HttpRequest = {
    get: function (url, data, cache, callback) {
        $.ajax({
            url: url,
            data: data,
            type: "GET",
            dataType: "json",
            cache: cache,
            contentType: "application/json; charset=utf-8",
            timeout: 10000, // timeout set to 10 seconds
            success: function (result) {
                callback(result);
            },
            error: function (result) {
                console.log(result);
            }          
        });
    },

    postJson: function (url, data, callback) {
        $.ajax(
       {
           url: url,
           type: "POST",
           contentType: "application/json",
           processData: false,
           data: JSON.stringify(data),
           success: function (result) {
               callback(result);
           },
           error: function (result) {
               console.log(result);
           }
       });
    },

    postForm: function (url, data, callback) {
    $.ajax(
   {
       url: url,
       type: "POST",
       contentType: false,
       processData: false,
       data: data,
       success: function (result) {
           callback(result);
       },
       error: function (result, status, xhr) {
           console.log(result.responseText);              
           return false;
       }
   });
}
}