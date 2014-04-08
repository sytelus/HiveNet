using System;

namespace Astrila.HiveNet
{
	/// <summary>
	/// Summary description for PeerMessageHandlerCollection.
	/// </summary>
	public class PeerMessageHandlerCollection : System.Collections.CollectionBase 
	{
		public void Add(IPeerMessageHandler handler)
		{
			base.List.Add(handler);
		}
	}
}
