using System;

namespace Astrila.HiveNet
{
	/// <summary>
	/// Summary description for IPeerMessageHandler.
	/// </summary>
	public interface IPeerMessageHandler
	{
		void ProcessMessage(PeerMessage message);
		void Initialize();
		void Shutdown();
	}
}
