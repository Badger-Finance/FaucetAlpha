async function Test () {
    // connect to Moralis server - Ropsten
    Moralis.initialize("GmCC89F9lQVTsepysE6S4rs4JBpPJacqyFYNB31M");
    Moralis.serverURL = "https://o6aadi5f8aml.moralis.io:2053/server";
    console.log('[JS] Morality Initialized');

    config = Moralis.Config.get({useMasterKey: false}).then(function(config) {
        privateParam = config.get("CooldownRateSeconds");
        console.log(privateParam);
    });
     
}

Test();