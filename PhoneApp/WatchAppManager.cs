using System;
using Foundation;
using WatchIPC;

namespace PhoneApp
{
	static public class WatchAppManager
	{
		static public void ProcessMessage(NSDictionary userInfo, Action<NSDictionary> reply)
		{
			Console.WriteLine("ProcessMessage");
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
				case IPCMessageType.ExampleIPCMessage2:
					{
						// decode message params
						IPCMessage<IPCParams> message = new IPCMessage<IPCParams>();
						message.DecodeParams(userInfo);

						// create response message
						IPCMessage<IPCParams> responseMessage = new IPCMessage<IPCParams>();

						// populate repsonse message
						responseMessage.ErrorCode = ErrorCode.SomeError;

						// reply to message
						reply(responseMessage.EncodeParams());
						break;
					}
			}
		}
	}
}

