using System;

namespace Astrila.HiveNet
{
	/// <summary>
	/// Summary description for PeerMessageHandlerBase.
	/// </summary>
	public abstract class PeerMessageHandlerBase : IPeerMessageHandler
	{
		#region IPeerMessageHandler Members

		public void ProcessMessage(PeerMessage message)
		{
			throw new NotImplementedException();
		}

		public void Initialize()
		{
			throw new NotImplementedException();
		}

		public void Shutdown()
		{
			throw new NotImplementedException();
		}

		#endregion
	}
}
