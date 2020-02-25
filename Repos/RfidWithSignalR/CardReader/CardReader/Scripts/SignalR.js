//alert("From signalR.js")
(function () {
    var myHub = $.connection.myHub
    $.connection.hub.start()
        .done(function () {
            console.log("SignalR connected");
            writeToPage("IT Worked");
            myHub.server.announce("Connected");
            
            myHub.server.getSerialPortOutput()
                .done(function (data) {
                    writeToPage(data)
                    
                })
                .fail(function (e) {
                    writeToPage(e)
                });
        })
        .fail(function () {
            alert("SignalR Connection failed");
            writeToPage("Error Connection");
        })

    myHub.client.announce = function (messageOne) {
        writeToPage(messageOne);
    }

    var writeToPage = function (messageTwo) {
        $("#welcome-message").append(messageTwo + "<br />");
    }
})()
