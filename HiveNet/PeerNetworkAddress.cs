using System;

namespace Astrila.HiveNet
{

	public enum PeerNetworkAddressType
	{
		IPv4Direct,
		IPv4Quadruple
	}

	public enum PeerNetworkAddressComponentType
	{
		IPv4DirectAddress,
		IPv4DirectPort,
		IPv4QuadruplePublicAddress,
		IPv4QuadruplePublicPort,
		IPv4QuadruplePrivateAddress,
		IPv4QuadruplePrivatePort
	}

	/// <summary>
	/// Summary description for PeerNetworkAddress.
	/// </summary>
	public class PeerNetworkAddress
	{
		public PeerNetworkAddressType AddressType
		{
			get
			{
				throw new NotImplementedException();
			}

			set
			{
				throw new NotImplementedException();
			}
		}

		public string GetAddressComponent(PeerNetworkAddressComponentType componentType)
		{
			throw new NotImplementedException();
		}

		public string SetAddressComponent(PeerNetworkAddressComponentType componentType, string componentValue)
		{
			throw new NotImplementedException();
		}

		
	}
}
