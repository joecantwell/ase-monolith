

// IIFE
(function ($) {
    "use strict";

    var carValue = $('#CarValue'),
        carReg = $('#regNo'),
        resultsDiv = $('#carFinderResults'),
        vehicleStore = $('#JsonVehicle'),
        spinner = $("#spinner");

    $(function () {
        carValue.maskMoney();
    });

    $('#check-reg').bind('submit', function (e) {
     
        e.preventDefault(); //STOP default action
        // shouldn't need this but the submit is firing twice for no obvious reason
        e.stopImmediatePropagation();

        // clean up from previous search
        resultsDiv.empty();

        // validate form entry       
        if (false === $(this).parsley().validate())
            return;      
        
        try {
            spinner.show();
            // trigger an ajax get
            var formUrl = $(this).attr("action");
            
            HttpRequest.get(formUrl, {regno : carReg.val()}, false, function(result) {
                
                if (result) {
                    console.log(result);
                    // populate the car results div!
                    
                    resultsDiv.append('<div class="alert alert-success">' + result.ModelDesc + '</div>');
                    vehicleStore.val(JSON.stringify(result));
                    resultsDiv.show();

                }
                spinner.delay(500).fadeOut();
            });
            
        } catch (exception) {
            console.log(exception);
        }
     
    });

} (window.jQuery) ); // global object is passed in as a param