namespace FINN
{
	partial class Ribbon : Microsoft.Office.Tools.Ribbon.RibbonBase
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		public Ribbon()
			: base(Globals.Factory.GetRibbonFactory())
		{
			InitializeComponent();
		}

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Component Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.tab1 = this.Factory.CreateRibbonTab();
			this.FINN = this.Factory.CreateRibbonGroup();
			this.btnStation = this.Factory.CreateRibbonButton();
			this.btnGroup = this.Factory.CreateRibbonButton();
			this.btnLine = this.Factory.CreateRibbonButton();
			this.tab1.SuspendLayout();
			this.FINN.SuspendLayout();
			this.SuspendLayout();
			// 
			// tab1
			// 
			this.tab1.ControlId.ControlIdType = Microsoft.Office.Tools.Ribbon.RibbonControlIdType.Office;
			this.tab1.Groups.Add(this.FINN);
			this.tab1.Label = "FINN";
			this.tab1.Name = "tab1";
			// 
			// FINN
			// 
			this.FINN.Items.Add(this.btnStation);
			this.FINN.Items.Add(this.btnGroup);
			this.FINN.Items.Add(this.btnLine);
			this.FINN.Label = "group1";
			this.FINN.Name = "FINN";
			// 
			// btnStation
			// 
			this.btnStation.Label = "Station";
			this.btnStation.Name = "btnStation";
			this.btnStation.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.btnStation_Click);
			// 
			// btnGroup
			// 
			this.btnGroup.Label = "Group";
			this.btnGroup.Name = "btnGroup";
			this.btnGroup.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.btnGroup_Click);
			// 
			// btnLine
			// 
			this.btnLine.Label = "Line";
			this.btnLine.Name = "btnLine";
			this.btnLine.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.btnLine_Click);
			// 
			// Ribbon
			// 
			this.Name = "Ribbon";
			this.RibbonType = "Microsoft.Excel.Workbook";
			this.Tabs.Add(this.tab1);
			this.Load += new Microsoft.Office.Tools.Ribbon.RibbonUIEventHandler(this.Ribbon_Load);
			this.tab1.ResumeLayout(false);
			this.tab1.PerformLayout();
			this.FINN.ResumeLayout(false);
			this.FINN.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		internal Microsoft.Office.Tools.Ribbon.RibbonTab tab1;
		internal Microsoft.Office.Tools.Ribbon.RibbonGroup FINN;
		internal Microsoft.Office.Tools.Ribbon.RibbonButton btnLine;
		internal Microsoft.Office.Tools.Ribbon.RibbonButton btnStation;
		internal Microsoft.Office.Tools.Ribbon.RibbonButton btnGroup;
	}

	partial class ThisRibbonCollection
	{
		internal Ribbon Ribbon
		{
			get { return this.GetRibbon<Ribbon>(); }
		}
	}
}
