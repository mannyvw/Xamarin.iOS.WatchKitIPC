/// <summary>
/// IPC message type.
/// </summary>
/// 
using Newtonsoft.Json;
using System;
using Foundation;

namespace WatchIPC
{
    /// <summary>
    /// Watch message type.
    /// </summary>
    public enum IPCMessageType
    {
		// add ipc messages here
        ExampleIPCMessage1 = 1000,
		ExampleIPCMessage2 = 1001
    }

    /// <summary>
    /// Error code from IPC messages
    /// </summary>
    public enum ErrorCode
    {
        Success,
		// add any errors you need here
		SomeError
    }

	/// <summary>
	/// IPC constants.
	/// </summary>
    public class IPCConstants
    {
        /// <summary>
        /// The json tag use to encode params.
        /// </summary>
        public const string JsonTag = "json";
        /// <summary>
        /// The json tag use to encode params.
        /// </summary>
        public const string ErrorCodeTag = "errorcode";
        /// <summary>
        /// The ipc message type tag.
        /// </summary>
        public const string TypeTag = "ipcmessagetype";
    }

    /// <summary>
    /// IPC message.
    /// </summary>
    public class IPCMessage<T> where T : IPCParams, new()
    {
        /// <summary>
        /// Gets or sets the error code.
        /// </summary>
        /// <value>The error code.</value>
        public ErrorCode ErrorCode { get; set; }

        /// <summary>
        /// Gets or sets the parameters.
        /// </summary>
        /// <value>The parameters.</value>
        public T Params { get; set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="WatchIPC.IPCMessage`1"/> class.
		/// </summary>
        public IPCMessage()
        {
            Params = new T();
            ErrorCode = ErrorCode.Success; // default to success
        }

        /// <summary>
        /// Encodes the parameters.
        /// </summary>
        /// <returns>The parameters.</returns>
        public NSMutableDictionary EncodeParams()
        {
            NSMutableDictionary param = new NSMutableDictionary();
            string json = Serialise();
			Console.WriteLine("EncodeParams " + json);
            param.SetValueForKey(new NSString(json), new NSString(IPCConstants.JsonTag));
            param.SetValueForKey(new NSNumber((int)ErrorCode), new NSString(IPCConstants.ErrorCodeTag));

            return param;
        }

        /// <summary>
        /// Decodes the parameters.
        /// </summary>
        /// <param name="userInfo">User info.</param>
        public void DecodeParams(NSDictionary userInfo)
        {
            try
            {
                NSString json = userInfo.ValueForKey(new NSString(IPCConstants.JsonTag)) as NSString;
				Console.WriteLine ("DecodeParams " + json);
                NSNumber errorCode = userInfo.ValueForKey(new NSString(IPCConstants.ErrorCodeTag)) as NSNumber;
                ErrorCode = (ErrorCode)errorCode.Int32Value;
                Deserialise(json);
            }
            catch(Exception exception)
            {
                Console.WriteLine("Failed to decode " + exception.Message);
            }
        }

        /// <summary>
        /// Serialise this instance.
        /// </summary>
        public string Serialise()
        {
            return JsonConvert.SerializeObject(Params);
        }

        /// <summary>
        /// Deserialise the specified json.
        /// </summary>
        /// <param name="json">Json.</param>
        public void Deserialise(string json)
        {
            Params = JsonConvert.DeserializeObject<T>(json);
        }
    }
}

