// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

function acceptAuction(id)
{
    var verificationToken = $("input[name='__RequestVerificationToken']").val ( )
    
    $.ajax ({  
        type: "POST", 
        url: "/Administrator/AcceptAuction", 
        data: { 
            "id": id,
            "__RequestVerificationToken" : verificationToken 
        },
        dataType: "text",
        success: function ( response ) {
            location.reload();
        },
        error: function ( response ) {
            location.reload(); 
        }
    })
}

function declineAuction(id)
{
    var verificationToken = $("input[name='__RequestVerificationToken']").val ( )
    
    $.ajax ({  
        type: "POST", 
        url: "/Administrator/DeclineAuction", 
        data: { 
            "id": id,
            "__RequestVerificationToken" : verificationToken 
        },
        dataType: "text",
        success: function ( response ) {
            location.reload();
        },
        error: function ( response ) {
            location.reload(); 
        }
    })
}


function banUser(username)
{
    var verificationToken = $("input[name='__RequestVerificationToken']").val ( )
    
    $.ajax ({  
        type: "POST", 
        url: "/Administrator/Ban", 
        data: { 
            "username": username,
            "__RequestVerificationToken" : verificationToken 
        },
        dataType: "text",
        success: function ( response ) {
            $("#"+username).html(response) 
        },
        error: function ( response ) {
            alert ( response ) 
        }
    })
}

function unbanUser(username)
{
    var verificationToken = $("input[name='__RequestVerificationToken']").val ( )
    
    $.ajax ({  
        type: "POST", 
        url: "/Administrator/Unban", 
        data: { 
            "username": username,
            "__RequestVerificationToken" : verificationToken 
        },
        dataType: "text",
        success: function ( response ) {
            $("#"+username).html(response) 
        },
        error: function ( response ) {
            alert ( response ) 
        }
    })
}

function buyTokens ( ) {
    var verificationToken = $("input[name='__RequestVerificationToken']").val ( ) 

    $.ajax ({ 
        type: "POST", 
        url: "/Payment/Payment",
        data: { 
            "bagName": $("input[name='tokenbag']:checked").val(), //to je onaj parametar sto on trazi bagName
            "__RequestVerificationToken" : verificationToken 
        },
        success: function ( response ) {
            location.reload();
        },
        error: function ( response ) {
            alert ( response )
        }
    })
}


$( document ).ready(function() {
    paypal.Buttons ( {
        createOrder: function ( data, actions ) {
            return actions.order.create ( {
                purchase_units: [{
                    amount: {
                        value: $("input[name='tokenbag']:checked").val()
                    }
                }]
            } )
        },
        onApprove: function ( data, actions ) {
            return actions.order.capture ( ).then (
                function ( details ) {
                    alert ( "SUCCESS " + details.payer.name.given_name)
                    buyTokens();

                }
            )
        }
    } ).render ( "#paypal" )
    
});

function nextPage(nextPage){
    var search = $("#search").val();
    var minPrice = $("#minPrice").val();
    var maxPrice = $("#maxPrice").val();
    var state = $("#state").children("option:selected").val();

    if(search == "" && minPrice == "" && maxPrice == "" && state == "OPEN"){
        $.ajax ({  
            type: "POST", 
            url: "/Home/NextPage", 
            data: { 
                "page":nextPage,
            },
            dataType: "text",
            success: function ( response ) {
                $("#auction").html(response);
            },
            error: function ( response ) {
                alert ( response ) 
            }
        });
    }else{
        $.ajax ({  
            type: "POST", 
            url: "/Home/FilteredPage", 
            data: { 
                "page":nextPage,
                "search":search,
                "minimumPrice":minPrice,
                "maximumPrice":maxPrice,
                "state":state
            },
            dataType: "text",
            success: function ( response ) {
                $("#auction").html(response);
            },
            error: function ( response ) {
                alert ( response ) 
            }
        });
    }

    
}

function clickHandler(delay){
    if(search != "" && minPrice != "" && maxPrice != "" && state != "OPEN"){
    setTimeout( function() { nextPage('1'); }, delay );
    }
}
function close(auctionId){
    var verificationToken = $("input[name='__RequestVerificationToken']").val( );
    $.ajax ({  
        type: "POST", 
        url: "/User/CloseBid",
        data: {
            "auctionId" : auctionId,
            "__RequestVerificationToken" : verificationToken
        },
        dataType: "json",
        success: function ( response ) {
            connection.invoke("CloseAuction", auctionId.toString()).catch(function (err) {
                return console.error(err.toString());
            });
           
        },
        error: function ( response ) {

        }
    });   

}
function timer(){
    var i;
    for(i=1; i<=12; i++){
        var string = $("#closeTime"+i).val();
        if(string==null)
            break;
         
        var array = string.split(",");
    
        var now = new Date();

        var eventDate = new Date(array[0], array[1] - 1, array[2], array[3], array[4], array[5], 0);

        var currentTime = now.getTime();
        var eventTime = eventDate.getTime();
 
        var remTime = eventTime - currentTime; 
        
        if(parseInt(remTime) < 0){
            var auctionId = $("#auctionId"+i).val();
            close(auctionId);
            continue;
        }
 
        var s = Math.floor(remTime / 1000);
        var m = Math.floor(s / 60);
        var h = Math.floor(m / 60);
        var d = Math.floor(h / 24);
 
        h %= 24; m %= 60; s %= 60; d %= 30;
 
        h = h - 1 < 0 ? 0 : h; 
 
        h = (h<10) ? "0" + h : h;
        m = (m<10) ? "0" + m : m;
        s = (s<10) ? "0" + s : s;
        d = (d < 10) ? "0" + d : d;
 
        $("#time"+i).text( d + ":" + h + ":" + m + ":" + s );
 
    }
 
    
}
setInterval(timer,1000);