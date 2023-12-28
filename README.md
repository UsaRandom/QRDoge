# QRDoge

A simple .NET Maui 8 android app for bridging the gap between a [Dogecoin Core Node](https://github.com/dogecoin/dogecoin) and [DogecoinTerminal](https://github.com/UsaRandom/DogecoinTerminal)

![image](https://github.com/UsaRandom/QRDoge/assets/2897796/fda7698d-bdd2-4b89-b9b0-62e5818f947d)


## Setup

QRDoge uses the `importaddress`, `listunspent`, `sendrawtransaction` rpc methods to fulfill DogecoinTerminal requests. 

You'll need to enable rpc in a secure fastion and be sure to add your phone to the ip whitelist.


## IMPORTANT

QRDoge is currently limited to max qrcode size of ~3k and limited even more so on an average phone's ability to actually read a qr code. It breaks if you try to exceed these limits. 



## App Flow

* Ensure all `Node Connection Settings` are filled out.

* When DogecoinTerminal asks for you to `Scan with QRDoge (Action Description)`, scan it with QRDoge.
* This happens when:
* * When a `new wallet` is created. 
  * When the `Refresh Balance` button is pressed.
  * Towards the end of the `Send` button's flow.
* `When QRDoge displays a QR Code itself`, press 'next' on DogecoinTerminal and provide the qr code to DogecoinTerminal.


