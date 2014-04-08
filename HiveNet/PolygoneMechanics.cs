using System;
using System.Collections;


namespace Astrila.HiveNet
{
	public enum PolygoneEndType
	{
		N=0, S=1, NE=2, SE=3, NW=4, SW=5, Uninitialized=6
	}

	[Flags()]
	public enum RoutingModeType
	{
		RouteOnlyIfExactMatch = 1,
		RouteOnRandomEdge = 2,
		RouteAny = 3,
		RouteNone = 0
	}

	public class PolygoneEdge
	{
		public const int UNCONSTRUCTED_POLYGONE_ID = 0;

		public PolygoneEndType EndType = PolygoneEndType.Uninitialized;

		public PolygoneNode ConnectedNode = null;

		public int LeftPolygoneID = UNCONSTRUCTED_POLYGONE_ID;
		public int RightPolygoneID = UNCONSTRUCTED_POLYGONE_ID;
		public int OppositePolygoneID = UNCONSTRUCTED_POLYGONE_ID;

		public PolygoneEdge LeftEdge = null;
		public PolygoneEdge RightEdge = null;


		public bool IsConnected
		{
			get
			{
				return ConnectedNode != null;	
			}
		}

		private bool IsLeftPolygoneSymmytric(PolygoneEdge anotherEdge)
		{
			return (LeftPolygoneID == anotherEdge.RightPolygoneID) || (LeftPolygoneID == 0) || (anotherEdge.RightPolygoneID == 0);
		}
		private bool IsRightPolygoneSymmytric(PolygoneEdge anotherEdge)
		{
			return (RightPolygoneID == anotherEdge.LeftPolygoneID) || (RightPolygoneID == 0) || (anotherEdge.LeftPolygoneID == 0);
		}
		private bool IsSymmystricToEdge(PolygoneEdge anotherEdge)
		{
			return IsLeftPolygoneSymmytric(anotherEdge) && IsRightPolygoneSymmytric(anotherEdge);
		}
		private bool IsOppositeToEdge(PolygoneEdge anotherEdge)
		{
			return (OppositePolygoneID != anotherEdge.OppositePolygoneID) || (anotherEdge.OppositePolygoneID == 0) || (OppositePolygoneID == 0);
		}

		private bool IsEndTypeCompatible(PolygoneEdge anotherEdge)
		{
			return (EndType == GeometricUtils.GetOppositeEndType(anotherEdge.EndType)) || (EndType == PolygoneEndType.Uninitialized)  || (anotherEdge.EndType == PolygoneEndType.Uninitialized);
		}
		public bool IsCompatibleToEdge(PolygoneEdge anotherEdge)
		{
			return IsSymmystricToEdge(anotherEdge) && IsOppositeToEdge(anotherEdge) && IsEndTypeCompatible(anotherEdge);
		}

		public bool IsRoutableToEdge(PolygoneEdge anotherEdge)
		{
			return IsLeftPolygonesExactlyEqual(anotherEdge) || IsRightPolygonesExactlyEqual(anotherEdge);
		}

		
		private void FuseEndTypes(PolygoneEdge otherNodeEdge, PolygoneEndType defaultEndType)
		{
			if (EndType == PolygoneEndType.Uninitialized)
			{
				if (otherNodeEdge.EndType != PolygoneEndType.Uninitialized)
					EndType = GeometricUtils.GetOppositeEndType(otherNodeEdge.EndType);
				else
					EndType = defaultEndType;
			}
			//else other edge should pick up my type

			LeftEdge.EndType = GeometricUtils.GetLeftEndType(EndType);
			RightEdge.EndType = GeometricUtils.GetRightEndType(EndType);
		}

		private void FusePolygones(PolygoneEdge otherNodeEdge, int defaultLeftPolygoneID, int defaultRightPolygoneID)
		{
			if (LeftPolygoneID == UNCONSTRUCTED_POLYGONE_ID)
			{
				if (otherNodeEdge.RightPolygoneID != UNCONSTRUCTED_POLYGONE_ID)
					LeftPolygoneID = otherNodeEdge.RightPolygoneID;
				else
					LeftPolygoneID = defaultLeftPolygoneID;
			}
			LeftEdge.RightPolygoneID = LeftPolygoneID;
			RightEdge.OppositePolygoneID = LeftPolygoneID;

			if (RightPolygoneID == UNCONSTRUCTED_POLYGONE_ID)
			{
				if (otherNodeEdge.LeftPolygoneID != UNCONSTRUCTED_POLYGONE_ID)
					RightPolygoneID = otherNodeEdge.LeftPolygoneID;
				else
					RightPolygoneID = defaultRightPolygoneID;
			}
			RightEdge.LeftPolygoneID = RightPolygoneID;
			LeftEdge.OppositePolygoneID = RightPolygoneID;
		}

		public void FuseEdges(PolygoneEdge otherNodeEdge, PolygoneNode otherNode, int defaultLeftPolygoneID, int defaultRightPolygoneID, PolygoneEndType defaultEdgeType)
		{
			FuseEndTypes(otherNodeEdge, defaultEdgeType);
			FusePolygones(otherNodeEdge, defaultLeftPolygoneID, defaultRightPolygoneID);
			ConnectedNode = otherNode;
		}

		public bool IsLeftPolygonesExactlyEqual(PolygoneEdge anotherEdge)
		{
			return (LeftPolygoneID == anotherEdge.RightPolygoneID) && (LeftPolygoneID != UNCONSTRUCTED_POLYGONE_ID) && (anotherEdge.RightPolygoneID != UNCONSTRUCTED_POLYGONE_ID);
		}
		public bool IsRightPolygonesExactlyEqual(PolygoneEdge anotherEdge)
		{
			return (RightPolygoneID == anotherEdge.LeftPolygoneID) && (RightPolygoneID != UNCONSTRUCTED_POLYGONE_ID) && (anotherEdge.LeftPolygoneID != UNCONSTRUCTED_POLYGONE_ID);
		}
		public bool IsEndTypeExactlyOppositeTo(PolygoneEdge anotherEdge)
		{
			return (EndType == GeometricUtils.GetOppositeEndType(anotherEdge.EndType)) && (EndType != PolygoneEndType.Uninitialized) && (anotherEdge.EndType != PolygoneEndType.Uninitialized);
		}

		public bool IsSameTypeAs(PolygoneEdge anotherEdge)
		{
			if (anotherEdge != null)
				return IsEndTypeExactlyOppositeTo(anotherEdge) || (EndType == anotherEdge.EndType);
			else
				return false;
		}
	}

	public class PolygoneEdgePair
	{
		public PolygoneEdge Edge1 = null;
		public PolygoneEdge Edge2 = null;
		public PolygoneEdgePair(PolygoneEdge paramEdge1, PolygoneEdge paramEdge2)
		{
			Edge1 = paramEdge1;
			Edge2 = paramEdge2;
		}
	}

	public class PolygoneNode
	{

		public PolygoneEdge[] Edges = null;

		public PolygoneNode()
		{
			ResetStateToUnconnectedNode();
		}

		public void ResetStateToUnconnectedNode()
		{
			Edges = new PolygoneEdge[] {new PolygoneEdge() , new PolygoneEdge() , new PolygoneEdge()};
			Edges[0].LeftEdge = Edges[1];
			Edges[0].RightEdge = Edges[2];
			Edges[1].LeftEdge = Edges[2];
			Edges[1].RightEdge = Edges[0];
			Edges[2].LeftEdge = Edges[0];
			Edges[2].RightEdge = Edges[1];
		}

		public PolygoneEdgePair[] GetConnectableEdgePairs(PolygoneNode anotherNode)
		{
			ArrayList connectableEdgesList = new ArrayList(3);
			for (int myEdgeIndex = 0; myEdgeIndex < 3; myEdgeIndex++)
			{
				for (int anotherNodeEdgeIndex = 0; anotherNodeEdgeIndex < 3; anotherNodeEdgeIndex++)
				{
					if (!Edges[myEdgeIndex].IsConnected && !anotherNode.Edges[anotherNodeEdgeIndex].IsConnected && Edges[myEdgeIndex].IsCompatibleToEdge(anotherNode.Edges[anotherNodeEdgeIndex]))
					{
						connectableEdgesList.Add(new PolygoneEdgePair(Edges[myEdgeIndex], anotherNode.Edges[anotherNodeEdgeIndex]));
					}
				}
			}

			return (PolygoneEdgePair[]) connectableEdgesList.ToArray(typeof(PolygoneEdgePair));
		}


		private PolygoneEdge[] GetRoutableEdges(PolygoneNode anotherNode, PolygoneEdge excludedEdge)
		{
			ArrayList routableEdgesList = new ArrayList(3);
			for (int myEdgeIndex = 0; myEdgeIndex < 3; myEdgeIndex++)
			{
				if (!Edges[myEdgeIndex].IsSameTypeAs(excludedEdge))
				{
					for (int anotherNodeEdgeIndex = 0; anotherNodeEdgeIndex < 3; anotherNodeEdgeIndex++)
					{
						if (Edges[myEdgeIndex].IsConnected && !anotherNode.Edges[anotherNodeEdgeIndex].IsConnected && Edges[myEdgeIndex].IsRoutableToEdge(anotherNode.Edges[anotherNodeEdgeIndex]))
						{
							routableEdgesList.Add(Edges[myEdgeIndex]);
							break;
						}
					}
				}
			}

			return (PolygoneEdge[]) routableEdgesList.ToArray(typeof(PolygoneEdge));
		}

		public PolygoneEdge[] GetAllConnectedEdges(PolygoneEdge excludeExactOppositeEdgeType)
		{
			ArrayList connectedEdgesList = new ArrayList(3);
			for (int myEdgeIndex = 0; myEdgeIndex < 3; myEdgeIndex++)
			{
				if (Edges[myEdgeIndex].IsConnected 
					&& ((excludeExactOppositeEdgeType == null) || !Edges[myEdgeIndex].IsEndTypeExactlyOppositeTo(excludeExactOppositeEdgeType) ))
				{
					connectedEdgesList.Add(Edges[myEdgeIndex]);
				}
			}

			return (PolygoneEdge[]) connectedEdgesList.ToArray(typeof(PolygoneEdge));			
		}

		public PolygoneEdge[] GetAllConnectedEdges(PolygoneEdge excludedEdgeType, PolygoneEdge onlyIncludeEdgeType)
		{
			ArrayList connectedEdgesList = new ArrayList(3);
			for (int myEdgeIndex = 0; myEdgeIndex < 3; myEdgeIndex++)
			{
				if (Edges[myEdgeIndex].IsConnected 
					&& ((excludedEdgeType == null) || !Edges[myEdgeIndex].IsSameTypeAs(excludedEdgeType))
					&& ((onlyIncludeEdgeType == null) || Edges[myEdgeIndex].IsSameTypeAs(onlyIncludeEdgeType)))
				{
					connectedEdgesList.Add(Edges[myEdgeIndex]);
				}
			}

			return (PolygoneEdge[]) connectedEdgesList.ToArray(typeof(PolygoneEdge));			
		}

		public virtual void OnConnectionCompleted(PolygoneNode otherNode, PolygoneEdge otherNodeEdge, PolygoneEdge myEdge)
		{}

		public virtual void OnBeforeConnectToIncomingNode(PolygoneNode incomingNode, PolygoneEdge requestStartedOnEdge, PolygoneEdge requestLastArrivedOnEdge, RoutingModeType routingMode, bool acknowledgeIfNotRoutable)
		{}

		public virtual void OnAfterConnectToIncomingNode(PolygoneNode incomingNode, PolygoneEdge requestStartedOnEdge, PolygoneEdge requestLastArrivedOnEdge, RoutingModeType routingMode, bool acknowledgeIfNotRoutable)
		{}


		public void NotifyConnectionSucceded(PolygoneNode otherNode, PolygoneEdge otherNodeEdge, PolygoneEdge myEdge)
		{
			OnConnectionCompleted(otherNode, otherNodeEdge, myEdge);			
		}

		public void ConnectToIncomingNode(PolygoneNode incomingNode, PolygoneEdge requestStartedOnEdge, PolygoneEdge requestLastArrivedOnEdge, RoutingModeType routingMode, bool acknowledgeIfNotRoutable)
		{
			OnBeforeConnectToIncomingNode(incomingNode, requestStartedOnEdge, requestLastArrivedOnEdge, routingMode, acknowledgeIfNotRoutable);
			if (incomingNode != this)
			{
				PolygoneEdgePair[] PolygoneEdgePairs = GetConnectableEdgePairs(incomingNode);
				if (PolygoneEdgePairs.Length > 0)	//We have connectable edges
				{
					PolygoneEdgePair choosenPolygoneEdgePair = PolygoneEdgePairs[GeometricUtils.GetRandomNumber(0,PolygoneEdgePairs.Length-1)];
					int defaultLeftPolygoneID = GeometricUtils.GetNewPolygoneID();
					int defaultRightPolygoneID = GeometricUtils.GetNewPolygoneID();
					choosenPolygoneEdgePair.Edge2.FuseEdges(choosenPolygoneEdgePair.Edge1, this, defaultRightPolygoneID, defaultLeftPolygoneID, PolygoneEndType.S);
					choosenPolygoneEdgePair.Edge1.FuseEdges(choosenPolygoneEdgePair.Edge2, incomingNode, defaultLeftPolygoneID, defaultRightPolygoneID, PolygoneEndType.N);

					incomingNode.NotifyConnectionSucceded(this,choosenPolygoneEdgePair.Edge1, choosenPolygoneEdgePair.Edge2);
					OnConnectionCompleted(incomingNode, choosenPolygoneEdgePair.Edge2, choosenPolygoneEdgePair.Edge1);			

					//move the request around
					if (choosenPolygoneEdgePair.Edge1.LeftEdge.IsConnected)
						choosenPolygoneEdgePair.Edge1.LeftEdge.ConnectedNode.ConnectToIncomingNode(incomingNode, choosenPolygoneEdgePair.Edge1.LeftEdge, choosenPolygoneEdgePair.Edge1.LeftEdge, RoutingModeType.RouteOnlyIfExactMatch, false);
					if (choosenPolygoneEdgePair.Edge1.RightEdge.IsConnected)
						choosenPolygoneEdgePair.Edge1.RightEdge.ConnectedNode.ConnectToIncomingNode(incomingNode, choosenPolygoneEdgePair.Edge1.RightEdge, choosenPolygoneEdgePair.Edge1.RightEdge, RoutingModeType.RouteOnlyIfExactMatch, false);
				}
				else
				{
					if ((routingMode & RoutingModeType.RouteOnlyIfExactMatch) != RoutingModeType.RouteNone)
					{
						PolygoneEdge[] routableEdges = GetRoutableEdges(incomingNode, requestLastArrivedOnEdge);
						
						//If no exact matches then continue on random edge
						if ((routableEdges.Length == 0) && ((routingMode & RoutingModeType.RouteOnRandomEdge) != RoutingModeType.RouteNone))
						{
							if (requestStartedOnEdge == null)
								routableEdges= GetAllConnectedEdges(null, null);	//Get all edges
							else
								routableEdges= GetAllConnectedEdges(requestStartedOnEdge);
//								if (requestLastArrivedOnEdge.IsSameTypeAs(requestStartedOnEdge))
//									routableEdges= GetAllConnectedEdges(requestLastArrivedOnEdge, null);	//Get all edges except the last one									
//								else
//									routableEdges= GetAllConnectedEdges(null, requestStartedOnEdge);	//Get only edge of same type as origin
						}
						//else do not route on random edge

						if (routableEdges.Length > 0)
						{
							PolygoneEdge choosenEdge = routableEdges[GeometricUtils.GetRandomNumber(0, routableEdges.Length-1)];
							//if (routableEdges.Length==3) choosenEdge = routableEdges[0];
							choosenEdge.ConnectedNode.ConnectToIncomingNode(incomingNode, (requestStartedOnEdge != null)?requestStartedOnEdge:choosenEdge, choosenEdge, routingMode, acknowledgeIfNotRoutable);
						}
						else
						{
							if (acknowledgeIfNotRoutable)
							{
								//We reached the edge
								incomingNode.NoRoutingAvailable(this);
							}
							//else no ack
						}
					}
					//else do not route
				}
			}
			//else ignore the self connect request
			OnAfterConnectToIncomingNode(incomingNode, requestStartedOnEdge, requestLastArrivedOnEdge, routingMode, acknowledgeIfNotRoutable);
		}

		public void NoRoutingAvailable(PolygoneNode lastNode)
		{
			throw new Exception("No routing available");
		}
	}

	public class GeometricUtils
	{
		public static int PolygonicIncrement(int whichValue)
		{
			if (whichValue == 6)
				return 1;
			else
				return whichValue++;
		}

		public static int PolygonicDecrement(int whichValue)
		{
			if (whichValue == 1)
				return 6;
			else
				return whichValue--;
		}

		public static int[] GetOtherTwoFromTriSet(int whichIsThis)
		{
			int[] otherTwo = new int[2];
			otherTwo[0] = GetNextOfTri(whichIsThis);
			otherTwo[1] = GetNextOfTri(otherTwo[0]);
			return otherTwo;
		}

		public static int GetTheOtherFromTriSet(int notThisOne, int notThatOne)
		{
			int typeSum = notThisOne + notThatOne;
			switch (typeSum)
			{
				case 1: return 2; 
				case 3: return 0; 
				case 2: return 1; 
				default: throw new ArgumentException("GetTheOtherFromtriSet is passed points from which other can not be determined"); 
			}
		}


		public static int GetNextOfTri(int whichIsThis)
		{
			switch (whichIsThis)
			{
				case 0: return 1; 
				case 1: return 2; 
				case 2: return 0; 
				default: throw new ArgumentException("Invalid tri type");
			}
		}

		private static int _LastPolygoneID = 1;
		public static int GetNewPolygoneID()
		{
			return _LastPolygoneID++;
		}

		private static Random randomGenerator = null;
		public static int GetRandomNumber(int min, int max)
		{
			if (min != max)
			{
				if (randomGenerator == null)
				{
					randomGenerator = new Random();
				}
				return randomGenerator.Next(min,max+1);
			}
			else
			{
				return min;
			}
		}

		public static PolygoneEndType GetOppositeEndType(PolygoneEndType forWhichEndType)
		{
			switch(forWhichEndType)
			{
				case PolygoneEndType.N: return PolygoneEndType.S;
				case PolygoneEndType.S: return PolygoneEndType.N;
				case PolygoneEndType.NE: return PolygoneEndType.SW;
				case PolygoneEndType.NW: return PolygoneEndType.SE;
				case PolygoneEndType.SW: return PolygoneEndType.NE;
				case PolygoneEndType.SE: return PolygoneEndType.NW;
				case PolygoneEndType.Uninitialized: return PolygoneEndType.Uninitialized;
				default: throw new Exception("End type note recognized");
			}
		}

		public static PolygoneEndType GetLeftEndType(PolygoneEndType forWhichEndType)
		{
			switch(forWhichEndType)
			{
				case PolygoneEndType.N: return PolygoneEndType.SW;
				case PolygoneEndType.S: return PolygoneEndType.NE;
				case PolygoneEndType.NE: return PolygoneEndType.NW;
				case PolygoneEndType.NW: return PolygoneEndType.S;
				case PolygoneEndType.SW: return PolygoneEndType.SE;
				case PolygoneEndType.SE: return PolygoneEndType.N;
				case PolygoneEndType.Uninitialized: return PolygoneEndType.Uninitialized;
				default: throw new Exception("End type note recognized");
			}
		}

		public static PolygoneEndType GetRightEndType(PolygoneEndType forWhichEndType)
		{
			switch(forWhichEndType)
			{
				case PolygoneEndType.N: return PolygoneEndType.SE;
				case PolygoneEndType.S: return PolygoneEndType.NW;
				case PolygoneEndType.NE: return PolygoneEndType.S;
				case PolygoneEndType.NW: return PolygoneEndType.NE;
				case PolygoneEndType.SW: return PolygoneEndType.N;
				case PolygoneEndType.SE: return PolygoneEndType.SW;
				case PolygoneEndType.Uninitialized: return PolygoneEndType.Uninitialized;
				default: throw new Exception("End type note recognized");
			}
		}

	}

}
