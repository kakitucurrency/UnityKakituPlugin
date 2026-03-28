# KakituPlugin
Plugin for integrating the Kakitu cryptocurrency into Unity.  

Available on the [Unity asset store](https://assetstore.unity.com/packages/tools/integration/kakitu)

Tested on Windows/Linux/MacOS/Android/iOS/HTML with Unity 2020.3.2f1

Features include: functions for processing blocks, creating seeds + reading/saving them to disk in password-protected encrypted files. Generating qr codes, listening for payments to single accounts, private key payouts and many more.

### Quickly testing
The easiest way to get started is to open up the scene in Scenes/SampleScene.unity. This provides a demo level to explore all the available functionality and connects to a publically available nodejs server.

## Screenshots

_Screenshots coming soon._

#### A description of the various files
`KakituUtils.cs` contains various generic functions such as creating seeds, encrypting/decrypting them using AES with a password, converting to accounts, converting between Raw and KSHS and various other things.  
`KakituWebsocket.cs` maintains the websocket connection to the proxies.  
`KakituManager.cs` is where all other functionality is located  
`KakituAmount.cs` is a helper class for storing and manipulating the raw units in Kakitu (KSHS)
`RPC.cs` connects to a server forward proxy for the Kakitu node

`KakituDemo.cs` contains content for the sample scene which illustrates all the functionality available.  
`TestUtils.cs` contains test for various things.  

To set up using the Kakitu plugin copy the Kakitu folder across to your project. The Scripts/Scenes folders are not required for deployment. A simple example of setting up the necessary functions with the public servers is:

```
using KakituPlugin;

public class KakituDemo : MonoBehaviour
{
  void Start()
  {
    // Initialize the KakituManager & KakituWebsocket
    kakituManager = gameObject.AddComponent<KakituManager>();
    kakituManager.rpcURL = "http://95.216.164.23:28103"; // Modify url to RPC server host:port
    kakituManager.defaultRep = "kshs_387tj8fjeo6r35ry5tjppympp8dct4d1ogpis7uaxsw8ywsrgp6shfge7two";

    kakituWebsocket = gameObject.AddComponent<KakituWebSocket>();
    kakituWebsocket.url = "ws://95.216.164.23:28104"; // Modify url to websocket server host:port
    kakituManager.Websocket = kakituWebsocket;
  }

  private KakituWebSocket websocket;
  private KakituManager kakituManager;
};
```

## Various other functions available

### Generate a private key
`byte[] privateKey = KakituUtils.GeneratePrivateKey();`

### Convert a byte[] to hex string
`string hexPrivateKey = KakituUtils.ByteArrayToHexString(privateKey);`

### Convert a hex string to byte[]
`byte[] privateKey = KakituUtils.HexStringToByteArray(hexPrivateKey);`

### Get the kshs_ address from the seed
`string address = KakituUtils.PrivateKeyToAddress(privateKey);`

### Get the public key hex string from the seed
`string publicKey = KakituUtils.PrivateKeyToPublicKeyHexString(privateKey);`

### Convert a kshs_ address and public key hex string
`string publicKey = KakituUtils.AddressToPublicKeyHexString(address);`

### Convert a public key hex string to kshs_ address
`string address = KakituUtils.PublicKeyToAddress(publicKey);`

## Individual node functions
These are low level building block functions which are not always necessary, it is recommened to use the utility functions mentioned below which encapsulates most of this.
### Get account information
```
// First we get the frontier
yield return kakituManager.AccountInfo(address, (accountInfo) =>
{
  var previous = accountInfo.frontier;
  var rep = accountInfo.representative;
  if (previous != null)
    // account exists
});
```
### Get pending blocks
```
List<PendingBlock> pendingBlocks = null;
yield return kakituManager.PendingBlocks(address, (responsePendingBlocks) =>
{
  pendingBlocks = responsePendingBlocks;
});
if (pendingBlocks != null && pendingBlocks.Count > 0)
....
```

### Create & sign a block
`var block = kakituManager.CreateBlock(address, KakituUtils.HexStringToByteArray(privateKey), newBalance, link, previous, rep, work);`
### Process a block to the network
```
kakituManager.Process(block, BlockType.send, (hash) =>
{
  if (hash != null)
    // Block is processed
}
```
### Create QR Code
```
var texture2D = KakituUtils.GenerateQRCodeTextureWithAmount(250, address, numRawPayToPlay, 50);
var sprite = Sprite.Create(texture2D, new Rect(0.0f, 0.0f, texture2D.width, texture2D.height), new Vector2(0.5f, 0.5f), 100.0f);
```
## Utility node functions
### Send Kakitu, waiting for confirmation from the network
```
IEnumerator SendWaitConfHandler()
{
  yield return kakituManager.SendWaitConf(toAddress, amount, privateKey, (error, hash) =>
  {
    if (!error)
    {
      Debug.Log("Send wait confirmed!!");
    }
    else
    {
      Debug.Log("Error with SendWaitConf");
    }
  });
}
```
### Send Kakitu, without waiting for confirmation from the network
```
private IEnumerator SendHandler()
{
  yield return kakituManager.Send(toAddress, amount, privateKey, (error, hash) =>
  {
    if (!error)
    {
      Debug.Log("Send confirmed!!");
    }
    else
    {
      Debug.Log("Error with Send");
    }
  });
}
```
### Receive Kakitu, waiting for confirmation from the network
`yield return kakituManager.ReceiveWaitConf(address, pendingBlock, privateKey, (error, hash) => { }`

### Receive Kakitu, without waiting for confirmation from the network
`yield return kakituManager.Receive(address, pendingBlock, privateKey, (error, hash) => { }`

### Automatically pocket Kakitu
```
kakituManager.AutomatePocketing(address, privateKey, (block) =>
  {
    // block is the block received for the 
  });
```
### Listen to all confirmations (useful for visualisers)
```
// Register a confirmation listener, can have multiple of these
kakituManager.AddConfirmationListener((websocketConfirmationResponse) =>
{
  var block = websocketConfirmationResponse.message.block;
  // Do something, show a ball etc
});

// Set up the pipelining for the above confirmation listener
kakituManager.ListenAllConfirmations();
```
Note: Make sure that `config.js` on the server has `listen_all = true`

Recommendation setups for:
#### Arcade machines
Listen to payment - Have 1 seed per arcade machine, start at first index then increment each time a payment is needed. This only checks pending blocks, don't have anything else pocketing these funds automatically. Every time a new payment is needed, move to the next index. Only 1 payment can be listening at any 1 time!  
Create a QR code for the account/amount required. Then listen for the payment. For payouts do a similar process with showing a QR Code (use the variant taking a private key), and listen for payout.  

#### Single player  
Create seed for player and store it encrypted with password (also check for local seed files if they want to open them)  
Loop through seed files, save seed & send and wait for confirmation.  

#### Multiplayer  
Process seed (as above), create & sign block locally and hand off to server.  
Server does validation (checks block is valid) then does appropriate action and send response to client.  

### To run your own test servers (recommended for production)
Requires running a Kakitu node, as well as `npm` and `nodejs` being installed.
1. Run the `kshs_node` binary after enabling rpc and websocket in `config-node.toml` file.
`kshs_node --daemon`
2. `cd TestServer`
3. Modify the `config.js` settings to be applicable for your system.
4. `npm install`
5. `node server.js`

A Kakitu node is required to be running which the websocket & http server (for RPC requests) will communicate with. Websocket & RPC should be enabled in the `node-config.toml` Kakitu file.   
A http server (for RPC requests) is definitely needed for communicating with the Kakitu node via JSON-RPC, a test nodejs script is supplied for this called `server.js`. A websocket server to receive notifications from the Kakitu network is highly recommended to make the most use of the plugin functionality. Another test server called `websocket_node.js` listening for confirmations for blocks in real-time on the Kakitu network. `server_work_callback.js` communicate with dpow (distributed POW) for work generation, a `work_peer` in `node-config.toml` should be set up for this, all these scripts are found in the ./TestServer subdirectory. Running `server.js` will also run `websocket_node.js` & `server_work_callback,js`. The websocket script makes 2 connections to the node, 1 is filtered output and 1 gets all websocket events (usual for visualisers). If you only need filtered output (recommended!) then disable `allow_listen_all=false` in `config.js`, this is the default.  

Limitations
- The test servers should not be used in production due to lack of security/ddos protection. Likely canditates for a future version are the KakituRPCProxy.

Any donation contributions are welcome to the Kakitu Team: kshs_15qry5fsh4mjbsgsqj148o8378cx5ext4dpnh5ex99gfck686nxkiuk9uouy
