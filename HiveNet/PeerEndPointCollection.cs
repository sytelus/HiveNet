using System;

namespace Astrila.HiveNet
{
	/// <summary>
	/// Summary description for PeerEndPointCollection.
	/// </summary>
	public class PeerEndPointCollection : System.Collections.CollectionBase 
	{
		public void Add(PeerEndPoint endPoint)
		{
			base.List.Add(endPoint);
		}
		
		public void Remove(int index)
		{
			base.List.RemoveAt(index);
		}

		public PeerEndPoint this[int index]
		{
			get
			{
				return (PeerEndPoint) base.List[index];
			}
		}
	}
}
