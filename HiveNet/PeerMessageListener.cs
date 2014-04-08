using System;

namespace Astrila.HiveNet
{
	/// <summary>
	/// Summary description for PeerMessageListener.
	/// </summary>
	public class PeerMessageListener
	{
		public void Start()
		{
			throw new NotImplementedException();
		}

		public void Stop()
		{
			throw new NotImplementedException();
		}

		private PeerMessageHandlerCollection _MessageHandlers = new PeerMessageHandlerCollection();
		public PeerMessageHandlerCollection MessageHandlers
		{
			get
			{
				return _MessageHandlers;				
			}
		}
	}
}
