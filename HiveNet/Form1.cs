using System;
using System.Diagnostics;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Threading;

namespace Astrila.HiveNet
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class Form1 : System.Windows.Forms.Form
	{
		private System.Windows.Forms.ToolBar toolBar1;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.PictureBox mainRegionPictureBox;
		private System.Windows.Forms.ToolBarButton toolBarButtonDraw;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public Form1()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.toolBar1 = new System.Windows.Forms.ToolBar();
			this.toolBarButtonDraw = new System.Windows.Forms.ToolBarButton();
			this.panel1 = new System.Windows.Forms.Panel();
			this.mainRegionPictureBox = new System.Windows.Forms.PictureBox();
			this.panel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// toolBar1
			// 
			this.toolBar1.Appearance = System.Windows.Forms.ToolBarAppearance.Flat;
			this.toolBar1.Buttons.AddRange(new System.Windows.Forms.ToolBarButton[] {
																						this.toolBarButtonDraw});
			this.toolBar1.DropDownArrows = true;
			this.toolBar1.Location = new System.Drawing.Point(0, 0);
			this.toolBar1.Name = "toolBar1";
			this.toolBar1.ShowToolTips = true;
			this.toolBar1.Size = new System.Drawing.Size(640, 42);
			this.toolBar1.TabIndex = 1;
			this.toolBar1.ButtonClick += new System.Windows.Forms.ToolBarButtonClickEventHandler(this.toolBar1_ButtonClick);
			// 
			// toolBarButtonDraw
			// 
			this.toolBarButtonDraw.Text = "Start";
			// 
			// panel1
			// 
			this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.panel1.Controls.Add(this.mainRegionPictureBox);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel1.Location = new System.Drawing.Point(0, 42);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(640, 427);
			this.panel1.TabIndex = 2;
			// 
			// mainRegionPictureBox
			// 
			this.mainRegionPictureBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.mainRegionPictureBox.Location = new System.Drawing.Point(0, 0);
			this.mainRegionPictureBox.Name = "mainRegionPictureBox";
			this.mainRegionPictureBox.Size = new System.Drawing.Size(638, 425);
			this.mainRegionPictureBox.TabIndex = 1;
			this.mainRegionPictureBox.TabStop = false;
			// 
			// Form1
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(640, 469);
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.toolBar1);
			this.Name = "Form1";
			this.Text = "Form1";
			this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
			this.Load += new System.EventHandler(this.Form1_Load);
			this.panel1.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			Application.Run(new Form1());
		}

		private void Form1_Load(object sender, System.EventArgs e)
		{
		}

		private void StartDraw()
		{
			mainRegionPictureBox.Image = new Bitmap(mainRegionPictureBox.Width, mainRegionPictureBox.Height);
			Graphics mainRegionGraphics = Graphics.FromImage(mainRegionPictureBox.Image);

			RefreshDelegate rg = new  RefreshDelegate(this.RefreshImage);

			GraphicPolygoneNode firstNode = new GraphicPolygoneNode(rg, true,mainRegionGraphics, mainRegionPictureBox.Width, mainRegionPictureBox.Height);
			for (int i = 0; i < 1000; i++)
			{
				GraphicPolygoneNode newNode = new GraphicPolygoneNode(rg, false,mainRegionGraphics, mainRegionPictureBox.Width, mainRegionPictureBox.Height);				
				firstNode.ConnectToIncomingNode(newNode, null, null, RoutingModeType.RouteAny, true);
			}
		}

		public void RefreshImage()
		{
			mainRegionPictureBox.Refresh();
		}
		

		private void toolBar1_ButtonClick(object sender, System.Windows.Forms.ToolBarButtonClickEventArgs e)
		{
			if (e.Button  == toolBarButtonDraw)
			{
				StartDraw();
			}
		}

	}

		public delegate void RefreshDelegate();
	
	public class GraphicPolygoneNode : PolygoneNode
	{
		private RefreshDelegate _RefreshGate = null;
		private PointF _MyDrawingPoint = PointF.Empty;
		private int _maxX;
		private int _maxY;
		private const int EDGE_LENGTH = 20;
		private static Pen _pointPen = new Pen(Brushes.Black, 2);
		private static Pen _onFreshConnectPen = new Pen(Brushes.Red, 2);
		private static Pen _onCoConnectPen = new Pen(Brushes.Yellow, 2);
		private static Pen _onJustConnectPen = new Pen(Brushes.RoyalBlue, 2);
		private bool noDelays = false;
		private bool completePolygoneDelays = true;

		private static Pen[] _linePens = new Pen[] {new Pen(Color.RosyBrown,1), new Pen(Color.YellowGreen,1), new Pen(Color.Turquoise,1), 
													new Pen(Color.SeaGreen,1), new Pen(Color.Teal ,1), new Pen(Color.BlueViolet,1)};

		private Graphics _gr = null;
		private const double ROOT2 = 1.4142135623730950488016887242097;
		private int _stepDelay = 1;

		public GraphicPolygoneNode(RefreshDelegate refreshGate, bool isFirstPoint, Graphics gr, int maxX, int maxY)
		{
			_gr = gr;
			_maxX = maxX;
			_maxY = maxY;
			_RefreshGate = refreshGate;
			if (isFirstPoint)
			{
				_MyDrawingPoint = new Point(_maxX/2, _maxY/2);
			}
		}

		private void Draw(Graphics gr, Pen penToUse)
		{
			Debug.Assert(!_MyDrawingPoint.Equals(PointF.Empty), "Can't draw pixel because its empty");
			gr.DrawEllipse(penToUse, _MyDrawingPoint.X,  _MyDrawingPoint.Y, 3,  3);
		}

		public override void OnBeforeConnectToIncomingNode(PolygoneNode incomingNode, PolygoneEdge requestStartedOnEdge, PolygoneEdge requestLastArrivedOnEdge, RoutingModeType routingMode, bool acknowledgeIfNotRoutable)
		{
			Draw(_gr,(routingMode==RoutingModeType.RouteAny)?_onFreshConnectPen:_onCoConnectPen);		
			if (noDelays == false)
			{
				_RefreshGate();
				System.Threading.Thread.Sleep((routingMode==RoutingModeType.RouteAny)?_stepDelay:((completePolygoneDelays)?2000:1));
			}
		}

		public override void OnAfterConnectToIncomingNode(PolygoneNode incomingNode, PolygoneEdge requestStartedOnEdge, PolygoneEdge requestLastArrivedOnEdge, RoutingModeType routingMode, bool acknowledgeIfNotRoutable)
		{
			Draw(_gr, _pointPen);
			if (noDelays == false)
			{
				_RefreshGate();
				System.Threading.Thread.Sleep(_stepDelay);		
			}
		}


		public override void OnConnectionCompleted(PolygoneNode otherNode, PolygoneEdge otherNodeEdge, PolygoneEdge myEdge)
		{
			//Calculate my point
			PointF sourcePoint = ((GraphicPolygoneNode) otherNode)._MyDrawingPoint;
			switch(otherNodeEdge.EndType)
			{
				case PolygoneEndType.N:
					_MyDrawingPoint.X = sourcePoint.X;
					_MyDrawingPoint.Y = sourcePoint.Y + EDGE_LENGTH*0.75f;
					break;
				case PolygoneEndType.S:
					_MyDrawingPoint.X = sourcePoint.X;
					_MyDrawingPoint.Y = sourcePoint.Y - EDGE_LENGTH*0.75f;
					break;
				case PolygoneEndType.NE:
					_MyDrawingPoint.X =  (float) (sourcePoint.X + EDGE_LENGTH/ROOT2);
					_MyDrawingPoint.Y = (float) (sourcePoint.Y + EDGE_LENGTH*0.75f/ROOT2);
					break;
				case PolygoneEndType.NW:
					_MyDrawingPoint.X = (float) (sourcePoint.X - EDGE_LENGTH/ROOT2);
					_MyDrawingPoint.Y = (float) (sourcePoint.Y + EDGE_LENGTH*0.75f/ROOT2);
					break;
				case PolygoneEndType.SE:
					_MyDrawingPoint.X = (float) (sourcePoint.X + EDGE_LENGTH/ROOT2);
					_MyDrawingPoint.Y = (float) (sourcePoint.Y - EDGE_LENGTH*0.75f/ROOT2);
					break;
				case PolygoneEndType.SW:
					_MyDrawingPoint.X = (float) (sourcePoint.X - EDGE_LENGTH/ROOT2);
					_MyDrawingPoint.Y = (float) (sourcePoint.Y - EDGE_LENGTH*0.75f/ROOT2);
					break;
				default:
					throw new Exception("edge End type not recognized");
			}

			Draw(_gr, _onJustConnectPen);
			_gr.DrawLine(_linePens[(int) otherNodeEdge.EndType],sourcePoint,_MyDrawingPoint);
			_RefreshGate();			
			//Application.DoEvents();
			if (noDelays == false)
			{
				System.Threading.Thread.Sleep(500);
			}
		}

	}
}
