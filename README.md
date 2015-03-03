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

Example : -

Create request on WatchExtension

			IPCMessage<ExampleResponseParams> responseMessage = null;
			Console.WriteLine ("Example IPC 1");

			IPCMessage<ExampleRequestParams> requestParams = new IPCMessage<ExampleRequestParams>();
			requestParams.Params.SomeMessage = "Send this to app";

			try {
				responseMessage = await MessageHandler.RequestMessage<ExampleResponseParams, ExampleRequestParams> (IPCMessageType.ExampleIPCMessage1, requestParams);
				Console.WriteLine ("Got response from phone app so displaying the response details");
				foreach(var entity in responseMessage.Params.SomeList)
				{
					Console.WriteLine(entity.ID);
				}
			} catch (Exception ex) {
				Console.WriteLine (ex);
			}
			
Trap and respond on App Side

			NSNumber messageId = userInfo.ObjectForKey(new NSString(IPCConstants.TypeTag)) as NSNumber;
			IPCMessageType watchMessageType = (IPCMessageType)messageId.Int32Value;
			Console.WriteLine("IPC Message " + watchMessageType);

			switch (watchMessageType)
			{
				case IPCMessageType.ExampleIPCMessage1:
					{
						// decode message params
						IPCMessage<ExampleRequestParams> message = new IPCMessage<ExampleRequestParams>();
						message.DecodeParams(userInfo);

						Console.WriteLine("Contents of request " + message.Params.SomeMessage);

						// create response message
						IPCMessage<ExampleResponseParams> responseMessage = new IPCMessage<ExampleResponseParams>();

						// populate repsonse message which is a list of ints
						responseMessage.ErrorCode = ErrorCode.Success;
						responseMessage.Params.SomeList.Add(new ResponseEntity(){ ID=1});
						responseMessage.Params.SomeList.Add(new ResponseEntity(){ ID=2});
						responseMessage.Params.SomeList.Add(new ResponseEntity(){ ID=4});

						// reply to message
						reply(responseMessage.EncodeParams());
						break;
					}
			}
