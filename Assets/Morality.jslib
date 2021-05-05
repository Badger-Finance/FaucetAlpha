var moralityGlobal =
{
    // a communicator object inside the global object, the dollar sign must be before the name.
    $contextObject:
    {
        // Notifies C# that the login was successful.
        NotifyCSharp_GetOutcome : function(requestID, outcome, callback)
        {
            // Calls a C# function using its pointer 'callback',
            // 'v' means it is a void function, the count of 'i's if the count of 
            // arguments this function accepts in our case 3 arguments, request id, 
            // the link and an error.
            console.log("[JS] Request #" + requestID + " Outcome:" + outcome);
            Runtime.dynCall('vii', callback, [requestID, outcome]);
        },
    },

    Mlty_Initialize: function (server, project) {
        // connect to Moralis server
        Moralis.initialize(Pointer_stringify(project));
        Moralis.serverURL = Pointer_stringify(server);
        console.log('[JS] Morality Initialized');
    },

    Mlty_Authenticate: function (requestID, callback) {
        Moralis.Web3.authenticate().then(function (user) {
            console.log("[JS] logged in user:", user.get('ethAddress'));
            contextObject.NotifyCSharp_GetOutcome(requestID, 1, callback);
        }).catch(function(error)
        {
            console.log(error);
            contextObject.NotifyCSharp_GetOutcome(requestID, 0, callback);
        });
    },

    Mlty_GetUserAttributeString: function (requestID, callback, attribute) {
        console.log('[JS] Input attribute: ' + Pointer_stringify(attribute));
        var returnStr = Moralis.User.current().get(Pointer_stringify(attribute));
        console.log('[JS] Obtained from Moralis: ' + returnStr);
        var bufferSize = lengthBytesUTF8(returnStr) + 1;
        var buffer = _malloc(bufferSize);
        stringToUTF8(returnStr, buffer, bufferSize);

        console.log('[JS] Returning arrtribute:' + buffer);
        contextObject.NotifyCSharp_GetOutcome(requestID, buffer, callback);
    },

    Mlty_EvalString: function (requestID, callback, javascript) {
        console.log('[JS] Input instructions: \n' + Pointer_stringify(javascript));
        var returnVal = eval(Pointer_stringify(javascript));
        
        console.log('[JS] Obtained from eval(): ' + returnVal);
        console.log('[JS] Type: ' + typeof(returnVal));

        var returnStr = returnVal.toString();
        console.log('[JS] Stringified Type: ' + typeof(returnStr));
        console.log('[JS] Stringified Value: ' + returnStr);

        var bufferSize = lengthBytesUTF8(returnStr) + 1;
        console.log('[JS] bufferSize: ' + bufferSize);

        var buffer = _malloc(bufferSize);
        stringToUTF8(returnStr, buffer, bufferSize);


        console.log('[JS] Returning eval buffer:' + buffer);
        contextObject.NotifyCSharp_GetOutcome(requestID, buffer, callback);
    },
};

autoAddDeps(moralityGlobal, '$contextObject'); // tell emscripten about this dependency, using the file name and communicator object name as parameters.
mergeInto(LibraryManager.library, moralityGlobal); // normal unity's merge into to merge this file into the build's javascript.
