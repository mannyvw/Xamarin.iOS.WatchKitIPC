using System;

using WatchKit;
using Foundation;
using WatchIPC;

namespace WatchExtension
{
	public partial class InterfaceController : WKInterfaceController
	{
		public InterfaceController(IntPtr handle) : base(handle)
		{
		}

		public override void Awake(NSObject context)
		{
			base.Awake(context);

			// Configure interface objects here.
			Console.WriteLine("{0} awake with context", this);
		}
			

		public override void WillActivate()
		{
			// This method is called when the watch view controller is about to be visible to the user.
			Console.WriteLine("{0} will activate", this);
		}

		public override void DidDeactivate()
		{
			// This method is called when the watch view controller is no longer visible to the user.
			Console.WriteLine("{0} did deactivate", this);
		}

		async partial void WKInterfaceButton0_Activated(WKInterfaceButton sender)
		{
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
		}

		async partial void WKInterfaceButton1_Activated(WKInterfaceButton sender)
		{
			IPCMessage<IPCParams> responseMessage = null;
			Console.WriteLine ("Example IPC 2 - No Params in Request, No Params in response");

			try {
				responseMessage = await MessageHandler.RequestMessage<IPCParams, IPCParams> (IPCMessageType.ExampleIPCMessage2, new IPCMessage<IPCParams>());
				Console.WriteLine ("Got response from phone app so displaying the response details");
				Console.WriteLine (responseMessage.ErrorCode);
			} 
			catch (Exception ex) {
				Console.WriteLine (ex);
			}
		}
	}
}

