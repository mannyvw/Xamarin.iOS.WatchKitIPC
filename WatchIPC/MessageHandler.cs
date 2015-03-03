using System;
using System.Threading.Tasks;
using WatchKit;
using Foundation;

namespace WatchIPC
{
	static public class MessageHandler
	{
		/// <summary>
		/// Requests the message.
		/// </summary>
		/// <returns>The message.</returns>
		/// <param name="type">Type.</param>
		/// <param name="requestMessage">Request message.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		/// <typeparam name="U">The 2nd type parameter.</typeparam>
		static public Task<IPCMessage<T>> RequestMessage<T, U> (IPCMessageType type,IPCMessage<U> requestMessage) where U : IPCParams, new() where T : IPCParams, new()
		{
			return RequestMessage<T,U> (type, new IPCMessage<T>(), requestMessage);
		}

		/// <summary>
		/// Requests the message.
		/// </summary>
		/// <returns>The message.</returns>
		/// <param name="type">Type.</param>
		/// <param name="message">Message.</param>
		/// <param name="requestMessage">Request message.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		/// <typeparam name="U">The 2nd type parameter.</typeparam>
		static public Task<IPCMessage<T>> RequestMessage<T, U> (IPCMessageType type, IPCMessage<T> responseMessage, IPCMessage<U> requestMessage) where U : IPCParams, new() where T : IPCParams, new()
		{
			var tcs = new TaskCompletionSource<IPCMessage<T>> ();

			WKInterfaceController.OpenParentApplication(PrepareMessage<U>(type, requestMessage),(NSDictionary userInfo,NSError error )=>{
				if(error != null)
				{
					tcs.TrySetException(new Exception(error.ToString()));
					return;
				}
				responseMessage.DecodeParams(userInfo);
				tcs.SetResult(responseMessage);
			});
			return tcs.Task;
		}	


		/// <summary>
		/// Prepares the message for sending over IPC from the watch extension.
		/// </summary>
		/// <returns>The message.</returns>
		/// <param name="messageType">Message type.</param>
		/// <param name="message">Message.</param>
		static public NSDictionary PrepareMessage<T> (IPCMessageType messageType, IPCMessage<T> requestMessage=null) where T : IPCParams, new()
		{
			Console.WriteLine("PrepareMessage " + messageType);
			NSMutableDictionary dict = null;

			if (requestMessage == null)
				dict = new  NSMutableDictionary();
			else
				dict = requestMessage.EncodeParams();
			dict.SetValueForKey(new NSNumber((int)messageType), new NSString(IPCConstants.TypeTag));

			return dict;
		}
	
	}
}

