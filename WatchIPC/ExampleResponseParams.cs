/// <summary>
/// Example response parameters.
/// </summary>
using System.Collections.Generic;

namespace WatchIPC
{
	public class ResponseEntity
	{
		public int ID;
	}

    public class ExampleResponseParams : IPCParams
    {
		public List<ResponseEntity> SomeList;
		// add more stuff here

		public ExampleResponseParams()
		{
			SomeList = new List<ResponseEntity>();
		}
    }
}

