// connect to Moralis server
Moralis.initialize("TiOBxZ3bskl416o5TiQnK33mxEJMEamFp8qUhp0t");
Moralis.serverURL = "https://wzsvcprcwot5.moralis.io:2053/server";
// add from here down
async function login() {
    let user = Moralis.User.current();
    if (!user) {
        user = await Moralis.Web3.authenticate();
    }
    console.log("logged in user:", user);
}

async function logOut() {
    await Moralis.User.logOut();
    console.log("logged out");
}

document.getElementById("btn-login").onclick = login;
document.getElementById("btn-logout").onclick = logOut;
document.getElementById("btn-get-stats").onclick = getStats;

function getStats() {
  const user = Moralis.User.current();
  if (user) {
    getUserTransactions(user);
  }
}

async function getUserTransactions(user) {
  // create query
  const query = new Moralis.Query("EthTransactions");
  query.equalTo("from_address", user.get("ethAddress"));

  // run query
  const results = await query.find();
  console.log("user transactions:", results);
}

// get stats on page load
getStats();