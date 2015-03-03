# Xamarin.iOS.WatchKitIPC
JSON based IPC for iOS WatchKit apps built using Xamarin

WatchIPC is designed to help developers pass and recieve params from thier main iOS app back into the watch extension.

If your watchapp need lots of messages with lots of data going back and forth this is very handy. iOS SDK just gives you a 
NSDictionary object default, which is fine if you only have one or two messages, but if you require lots of lots of data
its a pain adding all the params to the nsdictionary and getting them out again on the extension side.

So these classes encodes all your params into a single json string object and automatically endcodes and decodes them 
when required. Supports both directions so you can pass params to App via a IPC request and then pass differnt parameters back to the 
WatchExtension as a respose.

Full sample code enclosed but its the WatchIPC folder that has the clever stuff
