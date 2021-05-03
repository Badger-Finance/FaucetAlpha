var moralityGlobal =
{
    // a communicator object inside the global object, the dollar sign must be before the name.
    $contextObject:
    {
         // Notifies C# that the login was successful.
         NotifyCSharp_LoginOutcome : function(requestID, outcome, callback)
         {
             // Calls a C# function using its pointer 'callback',
             // 'v' means it is a void function, the count of 'i's if the count of 
             // arguments this function accepts in our case 3 arguments, request id, 
             // the link and an error.
             Runtime.dynCall('vii', callback, [requestID, outcome]);
         },
    },

    Mlty_Initialize: function (server, project) {
        // connect to Moralis server
        Moralis.initialize(Pointer_stringify(project));
        Moralis.serverURL = Pointer_stringify(server);
        console.log('Morality Initialized');
    },

    Mlty_Authenticate: function (requestID, callback) {
        Moralis.Web3.authenticate().then(function (user) {
            console.log("logged in user:", user);
            console.log("ETH Address:", user.get('ethAddress'));
            contextObject.NotifyCSharp_LoginOutcome(requestID, 1, callback);
        }).catch(function(error)
        {
            console.log(error);
            contextObject.NotifyCSharp_LoginOutcome(requestID, 0, callback);
        });
    },

    Mlty_WalletAddress: function () {
        var returnStr = web3.currentProvider.selectedAddress;
        var bufferSize = lengthBytesUTF8(returnStr) + 1;
        var buffer = _malloc(bufferSize);
        stringToUTF8(returnStr, buffer, bufferSize);
        return buffer;
    },
};

autoAddDeps(moralityGlobal, '$contextObject'); // tell emscripten about this dependency, using the file name and communicator object name as parameters.
mergeInto(LibraryManager.library, moralityGlobal); // normal unity's merge into to merge this file into the build's javascript.
