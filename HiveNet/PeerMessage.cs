using System;

namespace Astrila.HiveNet
{
	/// <summary>
	/// Summary description for PeerMessage.
	/// </summary>
	public class PeerMessage
	{

		private string _SentBy = null;
		public string SentBy
		{
			get
			{
				return _SentBy;
			}
			set
			{
				_SentBy = value;
			}
		}

		private string _SentTo = null;
		public string SentTo
		{
			get
			{
				return _SentTo;
			}
			set
			{
				_SentTo = value;
			}
		}

		private DateTime _SentOn = DateTime.MinValue;
		public DateTime SentOn
		{
			get
			{
				return _SentOn;
			}
			set
			{
				_SentOn = value;
			}
		}

		private DateTime _ExpiresOn = DateTime.MinValue;
		public DateTime ExpiresOn
		{
			get
			{
				return _ExpiresOn;
			}
			set
			{
				_ExpiresOn = value;
			}
		}

		private string _ID = null;
		public string ID
		{
			get
			{
				return _ID;
			}
			set
			{
				_ID = value;
			}
		}

		private string _Content = null;
		public string Content
		{
			get
			{
				return _Content;
			}
			set
			{
				_Content = value;
			}
		}

		private string _PreviousMessageID = null;
		public string PreviousMessageID
		{
			get
			{
				return _PreviousMessageID;
			}
			set
			{
				_PreviousMessageID = value;
			}
		}

		private string _NextMessageID = null;
		public string NextMessageID
		{
			get
			{
				return _NextMessageID;
			}
			set
			{
				_NextMessageID = value;
			}
		}

		public string Xml
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
	}
}
