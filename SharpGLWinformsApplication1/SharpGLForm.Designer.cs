namespace BIMI.PointCloud
{
    partial class SharpGLForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.openGLControl = new SharpGL.OpenGLControl();
            this.trkbrJoint1 = new System.Windows.Forms.TrackBar();
            this.trkbrJoint6 = new System.Windows.Forms.TrackBar();
            this.trkbrJoint5 = new System.Windows.Forms.TrackBar();
            this.trkbrJoint4 = new System.Windows.Forms.TrackBar();
            this.trkbrJoint3 = new System.Windows.Forms.TrackBar();
            this.trkbrJoint2 = new System.Windows.Forms.TrackBar();
            this.lblJoint1 = new System.Windows.Forms.Label();
            this.lblJoint2 = new System.Windows.Forms.Label();
            this.lblJoint3 = new System.Windows.Forms.Label();
            this.lblJoint4 = new System.Windows.Forms.Label();
            this.lblJoint5 = new System.Windows.Forms.Label();
            this.lblJoint6 = new System.Windows.Forms.Label();
            this.grpbxCtr = new System.Windows.Forms.GroupBox();
            this.txtJoint6Value = new System.Windows.Forms.TextBox();
            this.txtJoint5Value = new System.Windows.Forms.TextBox();
            this.txtJoint4Value = new System.Windows.Forms.TextBox();
            this.txtJoint3Value = new System.Windows.Forms.TextBox();
            this.txtJoint2Value = new System.Windows.Forms.TextBox();
            this.txtJoint1Value = new System.Windows.Forms.TextBox();
            this.btnResetJointPos = new System.Windows.Forms.Button();
            this.btnResetScene = new System.Windows.Forms.Button();
            this.grpbxAOV = new System.Windows.Forms.GroupBox();
            this.btnBehindScene = new System.Windows.Forms.Button();
            this.btnTopScene = new System.Windows.Forms.Button();
            this.btnRightScene = new System.Windows.Forms.Button();
            this.btnLeftScene = new System.Windows.Forms.Button();
            this.btnIK = new System.Windows.Forms.Button();
            this.timSimu = new System.Windows.Forms.Timer(this.components);
            this.grpbxIK = new System.Windows.Forms.GroupBox();
            this.lblr32 = new System.Windows.Forms.Label();
            this.txtr32 = new System.Windows.Forms.TextBox();
            this.lblr22 = new System.Windows.Forms.Label();
            this.txtr22 = new System.Windows.Forms.TextBox();
            this.lblr12 = new System.Windows.Forms.Label();
            this.txtr12 = new System.Windows.Forms.TextBox();
            this.lblr33 = new System.Windows.Forms.Label();
            this.lblr23 = new System.Windows.Forms.Label();
            this.lblr13 = new System.Windows.Forms.Label();
            this.txtr33 = new System.Windows.Forms.TextBox();
            this.txtr23 = new System.Windows.Forms.TextBox();
            this.txtr13 = new System.Windows.Forms.TextBox();
            this.lblr31 = new System.Windows.Forms.Label();
            this.lblr21 = new System.Windows.Forms.Label();
            this.lblr11 = new System.Windows.Forms.Label();
            this.txtr31 = new System.Windows.Forms.TextBox();
            this.txtr21 = new System.Windows.Forms.TextBox();
            this.txtr11 = new System.Windows.Forms.TextBox();
            this.lblZ = new System.Windows.Forms.Label();
            this.lblY = new System.Windows.Forms.Label();
            this.lblX = new System.Windows.Forms.Label();
            this.txtZ = new System.Windows.Forms.TextBox();
            this.txtY = new System.Windows.Forms.TextBox();
            this.txtX = new System.Windows.Forms.TextBox();
            this.grpbxSimu = new System.Windows.Forms.GroupBox();
            this.lblAccuracy = new System.Windows.Forms.Label();
            this.trkbrAccuracy = new System.Windows.Forms.TrackBar();
            this.btnPauseStart = new System.Windows.Forms.Button();
            this.lblScaleRatio = new System.Windows.Forms.Label();
            this.trkbrScaleRatio = new System.Windows.Forms.TrackBar();
            this.btnSimu = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.btnOpenFile = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.openGLControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trkbrJoint1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trkbrJoint6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trkbrJoint5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trkbrJoint4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trkbrJoint3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trkbrJoint2)).BeginInit();
            this.grpbxCtr.SuspendLayout();
            this.grpbxAOV.SuspendLayout();
            this.grpbxIK.SuspendLayout();
            this.grpbxSimu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trkbrAccuracy)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trkbrScaleRatio)).BeginInit();
            this.SuspendLayout();
            // 
            // openGLControl
            // 
            this.openGLControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.openGLControl.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.openGLControl.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.openGLControl.DrawFPS = true;
            this.openGLControl.ForeColor = System.Drawing.SystemColors.ControlText;
            this.openGLControl.FrameRate = 1000;
            this.openGLControl.Location = new System.Drawing.Point(16, 15);
            this.openGLControl.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.openGLControl.Name = "openGLControl";
            this.openGLControl.OpenGLVersion = SharpGL.Version.OpenGLVersion.OpenGL2_1;
            this.openGLControl.RenderContextType = SharpGL.RenderContextType.FBO;
            this.openGLControl.RenderTrigger = SharpGL.RenderTrigger.TimerBased;
            this.openGLControl.Size = new System.Drawing.Size(1489, 1003);
            this.openGLControl.TabIndex = 0;
            this.openGLControl.OpenGLInitialized += new System.EventHandler(this.openGLControl_OpenGLInitialized);
            this.openGLControl.OpenGLDraw += new SharpGL.RenderEventHandler(this.openGLControl_OpenGLDraw);
            this.openGLControl.Resized += new System.EventHandler(this.openGLControl_Resized);
            this.openGLControl.KeyDown += new System.Windows.Forms.KeyEventHandler(this.openGLControl_KeyDown);
            this.openGLControl.MouseDown += new System.Windows.Forms.MouseEventHandler(this.openGLControl_MouseDown);
            this.openGLControl.MouseMove += new System.Windows.Forms.MouseEventHandler(this.openGLControl_MouseMove);
            this.openGLControl.MouseUp += new System.Windows.Forms.MouseEventHandler(this.openGLControl_MouseUp);
            // 
            // trkbrJoint1
            // 
            this.trkbrJoint1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.trkbrJoint1.BackColor = System.Drawing.Color.White;
            this.trkbrJoint1.LargeChange = 2;
            this.trkbrJoint1.Location = new System.Drawing.Point(0, 108);
            this.trkbrJoint1.Margin = new System.Windows.Forms.Padding(4);
            this.trkbrJoint1.Name = "trkbrJoint1";
            this.trkbrJoint1.Size = new System.Drawing.Size(235, 56);
            this.trkbrJoint1.TabIndex = 1;
            this.trkbrJoint1.ValueChanged += new System.EventHandler(this.trkbrJoint1_ValueChanged);
            // 
            // trkbrJoint6
            // 
            this.trkbrJoint6.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.trkbrJoint6.BackColor = System.Drawing.Color.White;
            this.trkbrJoint6.LargeChange = 2;
            this.trkbrJoint6.Location = new System.Drawing.Point(-4, 505);
            this.trkbrJoint6.Margin = new System.Windows.Forms.Padding(4);
            this.trkbrJoint6.Name = "trkbrJoint6";
            this.trkbrJoint6.Size = new System.Drawing.Size(239, 56);
            this.trkbrJoint6.TabIndex = 6;
            this.trkbrJoint6.ValueChanged += new System.EventHandler(this.trkbrJoint6_ValueChanged);
            // 
            // trkbrJoint5
            // 
            this.trkbrJoint5.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.trkbrJoint5.BackColor = System.Drawing.Color.White;
            this.trkbrJoint5.LargeChange = 2;
            this.trkbrJoint5.Location = new System.Drawing.Point(0, 426);
            this.trkbrJoint5.Margin = new System.Windows.Forms.Padding(4);
            this.trkbrJoint5.Name = "trkbrJoint5";
            this.trkbrJoint5.Size = new System.Drawing.Size(235, 56);
            this.trkbrJoint5.TabIndex = 5;
            this.trkbrJoint5.ValueChanged += new System.EventHandler(this.trkbrJoint5_ValueChanged);
            // 
            // trkbrJoint4
            // 
            this.trkbrJoint4.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.trkbrJoint4.BackColor = System.Drawing.Color.White;
            this.trkbrJoint4.LargeChange = 2;
            this.trkbrJoint4.Location = new System.Drawing.Point(0, 348);
            this.trkbrJoint4.Margin = new System.Windows.Forms.Padding(4);
            this.trkbrJoint4.Name = "trkbrJoint4";
            this.trkbrJoint4.Size = new System.Drawing.Size(235, 56);
            this.trkbrJoint4.TabIndex = 4;
            this.trkbrJoint4.ValueChanged += new System.EventHandler(this.trkbrJoint4_ValueChanged);
            // 
            // trkbrJoint3
            // 
            this.trkbrJoint3.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.trkbrJoint3.BackColor = System.Drawing.Color.White;
            this.trkbrJoint3.LargeChange = 2;
            this.trkbrJoint3.Location = new System.Drawing.Point(0, 269);
            this.trkbrJoint3.Margin = new System.Windows.Forms.Padding(4);
            this.trkbrJoint3.Name = "trkbrJoint3";
            this.trkbrJoint3.Size = new System.Drawing.Size(235, 56);
            this.trkbrJoint3.TabIndex = 3;
            this.trkbrJoint3.ValueChanged += new System.EventHandler(this.trkbrJoint3_ValueChanged);
            // 
            // trkbrJoint2
            // 
            this.trkbrJoint2.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.trkbrJoint2.BackColor = System.Drawing.Color.White;
            this.trkbrJoint2.LargeChange = 2;
            this.trkbrJoint2.Location = new System.Drawing.Point(0, 190);
            this.trkbrJoint2.Margin = new System.Windows.Forms.Padding(4);
            this.trkbrJoint2.Name = "trkbrJoint2";
            this.trkbrJoint2.Size = new System.Drawing.Size(235, 56);
            this.trkbrJoint2.TabIndex = 2;
            this.trkbrJoint2.ValueChanged += new System.EventHandler(this.trkbrJoint2_ValueChanged);
            // 
            // lblJoint1
            // 
            this.lblJoint1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lblJoint1.AutoSize = true;
            this.lblJoint1.Location = new System.Drawing.Point(8, 89);
            this.lblJoint1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblJoint1.Name = "lblJoint1";
            this.lblJoint1.Size = new System.Drawing.Size(63, 15);
            this.lblJoint1.TabIndex = 7;
            this.lblJoint1.Text = "Joint_1";
            // 
            // lblJoint2
            // 
            this.lblJoint2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lblJoint2.AutoSize = true;
            this.lblJoint2.Location = new System.Drawing.Point(8, 168);
            this.lblJoint2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblJoint2.Name = "lblJoint2";
            this.lblJoint2.Size = new System.Drawing.Size(63, 15);
            this.lblJoint2.TabIndex = 8;
            this.lblJoint2.Text = "Joint_2";
            // 
            // lblJoint3
            // 
            this.lblJoint3.AutoSize = true;
            this.lblJoint3.Location = new System.Drawing.Point(8, 252);
            this.lblJoint3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblJoint3.Name = "lblJoint3";
            this.lblJoint3.Size = new System.Drawing.Size(63, 15);
            this.lblJoint3.TabIndex = 9;
            this.lblJoint3.Text = "Joint_3";
            // 
            // lblJoint4
            // 
            this.lblJoint4.AutoSize = true;
            this.lblJoint4.Location = new System.Drawing.Point(8, 331);
            this.lblJoint4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblJoint4.Name = "lblJoint4";
            this.lblJoint4.Size = new System.Drawing.Size(63, 15);
            this.lblJoint4.TabIndex = 10;
            this.lblJoint4.Text = "Joint_4";
            // 
            // lblJoint5
            // 
            this.lblJoint5.AutoSize = true;
            this.lblJoint5.Location = new System.Drawing.Point(8, 410);
            this.lblJoint5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblJoint5.Name = "lblJoint5";
            this.lblJoint5.Size = new System.Drawing.Size(63, 15);
            this.lblJoint5.TabIndex = 11;
            this.lblJoint5.Text = "Joint_5";
            // 
            // lblJoint6
            // 
            this.lblJoint6.AutoSize = true;
            this.lblJoint6.Location = new System.Drawing.Point(8, 489);
            this.lblJoint6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblJoint6.Name = "lblJoint6";
            this.lblJoint6.Size = new System.Drawing.Size(63, 15);
            this.lblJoint6.TabIndex = 12;
            this.lblJoint6.Text = "Joint_6";
            // 
            // grpbxCtr
            // 
            this.grpbxCtr.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.grpbxCtr.Controls.Add(this.txtJoint6Value);
            this.grpbxCtr.Controls.Add(this.txtJoint5Value);
            this.grpbxCtr.Controls.Add(this.txtJoint4Value);
            this.grpbxCtr.Controls.Add(this.txtJoint3Value);
            this.grpbxCtr.Controls.Add(this.txtJoint2Value);
            this.grpbxCtr.Controls.Add(this.txtJoint1Value);
            this.grpbxCtr.Controls.Add(this.btnResetJointPos);
            this.grpbxCtr.Controls.Add(this.trkbrJoint2);
            this.grpbxCtr.Controls.Add(this.lblJoint1);
            this.grpbxCtr.Controls.Add(this.lblJoint2);
            this.grpbxCtr.Controls.Add(this.trkbrJoint1);
            this.grpbxCtr.Controls.Add(this.trkbrJoint6);
            this.grpbxCtr.Controls.Add(this.trkbrJoint5);
            this.grpbxCtr.Controls.Add(this.lblJoint3);
            this.grpbxCtr.Controls.Add(this.lblJoint4);
            this.grpbxCtr.Controls.Add(this.lblJoint5);
            this.grpbxCtr.Controls.Add(this.lblJoint6);
            this.grpbxCtr.Controls.Add(this.trkbrJoint3);
            this.grpbxCtr.Controls.Add(this.trkbrJoint4);
            this.grpbxCtr.Location = new System.Drawing.Point(1652, 15);
            this.grpbxCtr.Margin = new System.Windows.Forms.Padding(4);
            this.grpbxCtr.Name = "grpbxCtr";
            this.grpbxCtr.Padding = new System.Windows.Forms.Padding(4);
            this.grpbxCtr.Size = new System.Drawing.Size(235, 565);
            this.grpbxCtr.TabIndex = 34;
            this.grpbxCtr.TabStop = false;
            this.grpbxCtr.Text = "Ctr";
            // 
            // txtJoint6Value
            // 
            this.txtJoint6Value.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtJoint6Value.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtJoint6Value.Location = new System.Drawing.Point(65, 536);
            this.txtJoint6Value.Name = "txtJoint6Value";
            this.txtJoint6Value.Size = new System.Drawing.Size(100, 23);
            this.txtJoint6Value.TabIndex = 33;
            this.txtJoint6Value.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtJoint5Value
            // 
            this.txtJoint5Value.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtJoint5Value.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtJoint5Value.Location = new System.Drawing.Point(65, 457);
            this.txtJoint5Value.Name = "txtJoint5Value";
            this.txtJoint5Value.Size = new System.Drawing.Size(100, 23);
            this.txtJoint5Value.TabIndex = 32;
            this.txtJoint5Value.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtJoint4Value
            // 
            this.txtJoint4Value.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtJoint4Value.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtJoint4Value.Location = new System.Drawing.Point(65, 379);
            this.txtJoint4Value.Name = "txtJoint4Value";
            this.txtJoint4Value.Size = new System.Drawing.Size(100, 23);
            this.txtJoint4Value.TabIndex = 31;
            this.txtJoint4Value.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtJoint3Value
            // 
            this.txtJoint3Value.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtJoint3Value.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtJoint3Value.Location = new System.Drawing.Point(65, 300);
            this.txtJoint3Value.Name = "txtJoint3Value";
            this.txtJoint3Value.Size = new System.Drawing.Size(100, 23);
            this.txtJoint3Value.TabIndex = 30;
            this.txtJoint3Value.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtJoint2Value
            // 
            this.txtJoint2Value.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtJoint2Value.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtJoint2Value.Location = new System.Drawing.Point(65, 221);
            this.txtJoint2Value.Name = "txtJoint2Value";
            this.txtJoint2Value.Size = new System.Drawing.Size(100, 23);
            this.txtJoint2Value.TabIndex = 29;
            this.txtJoint2Value.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtJoint1Value
            // 
            this.txtJoint1Value.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtJoint1Value.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtJoint1Value.Location = new System.Drawing.Point(65, 140);
            this.txtJoint1Value.Name = "txtJoint1Value";
            this.txtJoint1Value.Size = new System.Drawing.Size(100, 23);
            this.txtJoint1Value.TabIndex = 28;
            this.txtJoint1Value.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // btnResetJointPos
            // 
            this.btnResetJointPos.Location = new System.Drawing.Point(51, 25);
            this.btnResetJointPos.Margin = new System.Windows.Forms.Padding(4);
            this.btnResetJointPos.Name = "btnResetJointPos";
            this.btnResetJointPos.Size = new System.Drawing.Size(141, 48);
            this.btnResetJointPos.TabIndex = 7;
            this.btnResetJointPos.Text = "Reset_JointPos";
            this.btnResetJointPos.UseVisualStyleBackColor = true;
            this.btnResetJointPos.Click += new System.EventHandler(this.btnResetJointPos_Click);
            // 
            // btnResetScene
            // 
            this.btnResetScene.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnResetScene.Location = new System.Drawing.Point(13, 33);
            this.btnResetScene.Margin = new System.Windows.Forms.Padding(4);
            this.btnResetScene.Name = "btnResetScene";
            this.btnResetScene.Size = new System.Drawing.Size(110, 48);
            this.btnResetScene.TabIndex = 8;
            this.btnResetScene.Text = "Reset_Scene";
            this.btnResetScene.UseVisualStyleBackColor = true;
            this.btnResetScene.Click += new System.EventHandler(this.btnResetScene_Click);
            // 
            // grpbxAOV
            // 
            this.grpbxAOV.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.grpbxAOV.Controls.Add(this.btnBehindScene);
            this.grpbxAOV.Controls.Add(this.btnTopScene);
            this.grpbxAOV.Controls.Add(this.btnRightScene);
            this.grpbxAOV.Controls.Add(this.btnLeftScene);
            this.grpbxAOV.Controls.Add(this.btnResetScene);
            this.grpbxAOV.Location = new System.Drawing.Point(1513, 600);
            this.grpbxAOV.Margin = new System.Windows.Forms.Padding(4);
            this.grpbxAOV.Name = "grpbxAOV";
            this.grpbxAOV.Padding = new System.Windows.Forms.Padding(4);
            this.grpbxAOV.Size = new System.Drawing.Size(132, 420);
            this.grpbxAOV.TabIndex = 35;
            this.grpbxAOV.TabStop = false;
            this.grpbxAOV.Text = "AOV";
            // 
            // btnBehindScene
            // 
            this.btnBehindScene.Location = new System.Drawing.Point(13, 293);
            this.btnBehindScene.Margin = new System.Windows.Forms.Padding(4);
            this.btnBehindScene.Name = "btnBehindScene";
            this.btnBehindScene.Size = new System.Drawing.Size(110, 48);
            this.btnBehindScene.TabIndex = 12;
            this.btnBehindScene.Text = "Behind";
            this.btnBehindScene.UseVisualStyleBackColor = true;
            this.btnBehindScene.Click += new System.EventHandler(this.btnBehindScene_Click);
            // 
            // btnTopScene
            // 
            this.btnTopScene.Location = new System.Drawing.Point(13, 228);
            this.btnTopScene.Margin = new System.Windows.Forms.Padding(4);
            this.btnTopScene.Name = "btnTopScene";
            this.btnTopScene.Size = new System.Drawing.Size(110, 48);
            this.btnTopScene.TabIndex = 11;
            this.btnTopScene.Text = "Top";
            this.btnTopScene.UseVisualStyleBackColor = true;
            this.btnTopScene.Click += new System.EventHandler(this.btnTopScene_Click);
            // 
            // btnRightScene
            // 
            this.btnRightScene.Location = new System.Drawing.Point(13, 163);
            this.btnRightScene.Margin = new System.Windows.Forms.Padding(4);
            this.btnRightScene.Name = "btnRightScene";
            this.btnRightScene.Size = new System.Drawing.Size(110, 48);
            this.btnRightScene.TabIndex = 10;
            this.btnRightScene.Text = "Right";
            this.btnRightScene.UseVisualStyleBackColor = true;
            this.btnRightScene.Click += new System.EventHandler(this.btnRightScene_Click);
            // 
            // btnLeftScene
            // 
            this.btnLeftScene.Location = new System.Drawing.Point(13, 98);
            this.btnLeftScene.Margin = new System.Windows.Forms.Padding(4);
            this.btnLeftScene.Name = "btnLeftScene";
            this.btnLeftScene.Size = new System.Drawing.Size(110, 48);
            this.btnLeftScene.TabIndex = 9;
            this.btnLeftScene.Text = "Left";
            this.btnLeftScene.UseVisualStyleBackColor = true;
            this.btnLeftScene.Click += new System.EventHandler(this.btnLeftScene_Click);
            // 
            // btnIK
            // 
            this.btnIK.Location = new System.Drawing.Point(16, 25);
            this.btnIK.Name = "btnIK";
            this.btnIK.Size = new System.Drawing.Size(100, 48);
            this.btnIK.TabIndex = 24;
            this.btnIK.Text = "IK_Solve";
            this.btnIK.UseVisualStyleBackColor = true;
            this.btnIK.Click += new System.EventHandler(this.btnIK_Click);
            // 
            // timSimu
            // 
            this.timSimu.Interval = 10;
            this.timSimu.Tick += new System.EventHandler(this.timSimu_Tick);
            // 
            // grpbxIK
            // 
            this.grpbxIK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.grpbxIK.Controls.Add(this.lblr32);
            this.grpbxIK.Controls.Add(this.txtr32);
            this.grpbxIK.Controls.Add(this.lblr22);
            this.grpbxIK.Controls.Add(this.txtr22);
            this.grpbxIK.Controls.Add(this.lblr12);
            this.grpbxIK.Controls.Add(this.txtr12);
            this.grpbxIK.Controls.Add(this.lblr33);
            this.grpbxIK.Controls.Add(this.lblr23);
            this.grpbxIK.Controls.Add(this.lblr13);
            this.grpbxIK.Controls.Add(this.txtr33);
            this.grpbxIK.Controls.Add(this.txtr23);
            this.grpbxIK.Controls.Add(this.txtr13);
            this.grpbxIK.Controls.Add(this.lblr31);
            this.grpbxIK.Controls.Add(this.lblr21);
            this.grpbxIK.Controls.Add(this.lblr11);
            this.grpbxIK.Controls.Add(this.txtr31);
            this.grpbxIK.Controls.Add(this.txtr21);
            this.grpbxIK.Controls.Add(this.txtr11);
            this.grpbxIK.Controls.Add(this.lblZ);
            this.grpbxIK.Controls.Add(this.lblY);
            this.grpbxIK.Controls.Add(this.lblX);
            this.grpbxIK.Controls.Add(this.txtZ);
            this.grpbxIK.Controls.Add(this.txtY);
            this.grpbxIK.Controls.Add(this.txtX);
            this.grpbxIK.Controls.Add(this.btnIK);
            this.grpbxIK.Location = new System.Drawing.Point(1513, 15);
            this.grpbxIK.Name = "grpbxIK";
            this.grpbxIK.Size = new System.Drawing.Size(132, 567);
            this.grpbxIK.TabIndex = 36;
            this.grpbxIK.TabStop = false;
            this.grpbxIK.Text = "IK";
            // 
            // lblr32
            // 
            this.lblr32.AutoSize = true;
            this.lblr32.Location = new System.Drawing.Point(99, 412);
            this.lblr32.Name = "lblr32";
            this.lblr32.Size = new System.Drawing.Size(31, 15);
            this.lblr32.TabIndex = 48;
            this.lblr32.Text = "r32";
            // 
            // txtr32
            // 
            this.txtr32.Location = new System.Drawing.Point(21, 409);
            this.txtr32.Name = "txtr32";
            this.txtr32.Size = new System.Drawing.Size(72, 25);
            this.txtr32.TabIndex = 20;
            // 
            // lblr22
            // 
            this.lblr22.AutoSize = true;
            this.lblr22.Location = new System.Drawing.Point(99, 372);
            this.lblr22.Name = "lblr22";
            this.lblr22.Size = new System.Drawing.Size(31, 15);
            this.lblr22.TabIndex = 46;
            this.lblr22.Text = "r22";
            // 
            // txtr22
            // 
            this.txtr22.Location = new System.Drawing.Point(21, 369);
            this.txtr22.Name = "txtr22";
            this.txtr22.Size = new System.Drawing.Size(72, 25);
            this.txtr22.TabIndex = 19;
            // 
            // lblr12
            // 
            this.lblr12.AutoSize = true;
            this.lblr12.Location = new System.Drawing.Point(99, 332);
            this.lblr12.Name = "lblr12";
            this.lblr12.Size = new System.Drawing.Size(31, 15);
            this.lblr12.TabIndex = 44;
            this.lblr12.Text = "r12";
            // 
            // txtr12
            // 
            this.txtr12.Location = new System.Drawing.Point(21, 329);
            this.txtr12.Name = "txtr12";
            this.txtr12.Size = new System.Drawing.Size(72, 25);
            this.txtr12.TabIndex = 18;
            // 
            // lblr33
            // 
            this.lblr33.AutoSize = true;
            this.lblr33.Location = new System.Drawing.Point(99, 532);
            this.lblr33.Name = "lblr33";
            this.lblr33.Size = new System.Drawing.Size(31, 15);
            this.lblr33.TabIndex = 42;
            this.lblr33.Text = "r33";
            // 
            // lblr23
            // 
            this.lblr23.AutoSize = true;
            this.lblr23.Location = new System.Drawing.Point(99, 492);
            this.lblr23.Name = "lblr23";
            this.lblr23.Size = new System.Drawing.Size(31, 15);
            this.lblr23.TabIndex = 41;
            this.lblr23.Text = "r23";
            // 
            // lblr13
            // 
            this.lblr13.AutoSize = true;
            this.lblr13.Location = new System.Drawing.Point(99, 452);
            this.lblr13.Name = "lblr13";
            this.lblr13.Size = new System.Drawing.Size(31, 15);
            this.lblr13.TabIndex = 40;
            this.lblr13.Text = "r13";
            // 
            // txtr33
            // 
            this.txtr33.Location = new System.Drawing.Point(21, 529);
            this.txtr33.Name = "txtr33";
            this.txtr33.Size = new System.Drawing.Size(72, 25);
            this.txtr33.TabIndex = 23;
            // 
            // txtr23
            // 
            this.txtr23.Location = new System.Drawing.Point(21, 489);
            this.txtr23.Name = "txtr23";
            this.txtr23.Size = new System.Drawing.Size(72, 25);
            this.txtr23.TabIndex = 22;
            // 
            // txtr13
            // 
            this.txtr13.Location = new System.Drawing.Point(21, 449);
            this.txtr13.Name = "txtr13";
            this.txtr13.Size = new System.Drawing.Size(72, 25);
            this.txtr13.TabIndex = 21;
            // 
            // lblr31
            // 
            this.lblr31.AutoSize = true;
            this.lblr31.Location = new System.Drawing.Point(99, 292);
            this.lblr31.Name = "lblr31";
            this.lblr31.Size = new System.Drawing.Size(31, 15);
            this.lblr31.TabIndex = 36;
            this.lblr31.Text = "r31";
            // 
            // lblr21
            // 
            this.lblr21.AutoSize = true;
            this.lblr21.Location = new System.Drawing.Point(98, 252);
            this.lblr21.Name = "lblr21";
            this.lblr21.Size = new System.Drawing.Size(31, 15);
            this.lblr21.TabIndex = 35;
            this.lblr21.Text = "r21";
            // 
            // lblr11
            // 
            this.lblr11.AutoSize = true;
            this.lblr11.Location = new System.Drawing.Point(98, 212);
            this.lblr11.Name = "lblr11";
            this.lblr11.Size = new System.Drawing.Size(31, 15);
            this.lblr11.TabIndex = 34;
            this.lblr11.Text = "r11";
            // 
            // txtr31
            // 
            this.txtr31.Location = new System.Drawing.Point(21, 289);
            this.txtr31.Name = "txtr31";
            this.txtr31.Size = new System.Drawing.Size(72, 25);
            this.txtr31.TabIndex = 17;
            // 
            // txtr21
            // 
            this.txtr21.Location = new System.Drawing.Point(21, 249);
            this.txtr21.Name = "txtr21";
            this.txtr21.Size = new System.Drawing.Size(72, 25);
            this.txtr21.TabIndex = 16;
            // 
            // txtr11
            // 
            this.txtr11.Location = new System.Drawing.Point(21, 209);
            this.txtr11.Name = "txtr11";
            this.txtr11.Size = new System.Drawing.Size(72, 25);
            this.txtr11.TabIndex = 15;
            // 
            // lblZ
            // 
            this.lblZ.AutoSize = true;
            this.lblZ.Location = new System.Drawing.Point(99, 172);
            this.lblZ.Name = "lblZ";
            this.lblZ.Size = new System.Drawing.Size(15, 15);
            this.lblZ.TabIndex = 30;
            this.lblZ.Text = "Z";
            // 
            // lblY
            // 
            this.lblY.AutoSize = true;
            this.lblY.Location = new System.Drawing.Point(98, 132);
            this.lblY.Name = "lblY";
            this.lblY.Size = new System.Drawing.Size(15, 15);
            this.lblY.TabIndex = 29;
            this.lblY.Text = "Y";
            // 
            // lblX
            // 
            this.lblX.AutoSize = true;
            this.lblX.Location = new System.Drawing.Point(98, 92);
            this.lblX.Name = "lblX";
            this.lblX.Size = new System.Drawing.Size(15, 15);
            this.lblX.TabIndex = 28;
            this.lblX.Text = "X";
            // 
            // txtZ
            // 
            this.txtZ.Location = new System.Drawing.Point(21, 169);
            this.txtZ.Name = "txtZ";
            this.txtZ.Size = new System.Drawing.Size(72, 25);
            this.txtZ.TabIndex = 14;
            // 
            // txtY
            // 
            this.txtY.Location = new System.Drawing.Point(21, 129);
            this.txtY.Name = "txtY";
            this.txtY.Size = new System.Drawing.Size(72, 25);
            this.txtY.TabIndex = 13;
            // 
            // txtX
            // 
            this.txtX.Location = new System.Drawing.Point(21, 89);
            this.txtX.Name = "txtX";
            this.txtX.Size = new System.Drawing.Size(72, 25);
            this.txtX.TabIndex = 12;
            // 
            // grpbxSimu
            // 
            this.grpbxSimu.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.grpbxSimu.Controls.Add(this.lblAccuracy);
            this.grpbxSimu.Controls.Add(this.trkbrAccuracy);
            this.grpbxSimu.Controls.Add(this.btnPauseStart);
            this.grpbxSimu.Controls.Add(this.lblScaleRatio);
            this.grpbxSimu.Controls.Add(this.trkbrScaleRatio);
            this.grpbxSimu.Controls.Add(this.btnSimu);
            this.grpbxSimu.Controls.Add(this.btnClear);
            this.grpbxSimu.Controls.Add(this.btnOpenFile);
            this.grpbxSimu.Location = new System.Drawing.Point(1648, 600);
            this.grpbxSimu.Name = "grpbxSimu";
            this.grpbxSimu.Size = new System.Drawing.Size(239, 420);
            this.grpbxSimu.TabIndex = 37;
            this.grpbxSimu.TabStop = false;
            this.grpbxSimu.Text = "Simu";
            // 
            // lblAccuracy
            // 
            this.lblAccuracy.AutoSize = true;
            this.lblAccuracy.BackColor = System.Drawing.SystemColors.Control;
            this.lblAccuracy.Location = new System.Drawing.Point(65, 130);
            this.lblAccuracy.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblAccuracy.Name = "lblAccuracy";
            this.lblAccuracy.Size = new System.Drawing.Size(79, 15);
            this.lblAccuracy.TabIndex = 31;
            this.lblAccuracy.Text = "Accuracy:";
            // 
            // trkbrAccuracy
            // 
            this.trkbrAccuracy.BackColor = System.Drawing.SystemColors.Control;
            this.trkbrAccuracy.Location = new System.Drawing.Point(51, 96);
            this.trkbrAccuracy.Name = "trkbrAccuracy";
            this.trkbrAccuracy.Size = new System.Drawing.Size(141, 56);
            this.trkbrAccuracy.TabIndex = 26;
            this.trkbrAccuracy.ValueChanged += new System.EventHandler(this.trkbrAccuracy_ValueChanged);
            // 
            // btnPauseStart
            // 
            this.btnPauseStart.Location = new System.Drawing.Point(51, 228);
            this.btnPauseStart.Name = "btnPauseStart";
            this.btnPauseStart.Size = new System.Drawing.Size(141, 48);
            this.btnPauseStart.TabIndex = 28;
            this.btnPauseStart.Text = "Pause";
            this.btnPauseStart.UseVisualStyleBackColor = true;
            this.btnPauseStart.Click += new System.EventHandler(this.btnPauseStart_Click);
            // 
            // lblScaleRatio
            // 
            this.lblScaleRatio.AutoSize = true;
            this.lblScaleRatio.BackColor = System.Drawing.SystemColors.Control;
            this.lblScaleRatio.Location = new System.Drawing.Point(64, 191);
            this.lblScaleRatio.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblScaleRatio.Name = "lblScaleRatio";
            this.lblScaleRatio.Size = new System.Drawing.Size(95, 15);
            this.lblScaleRatio.TabIndex = 30;
            this.lblScaleRatio.Text = "ScaleRatio:";
            // 
            // trkbrScaleRatio
            // 
            this.trkbrScaleRatio.BackColor = System.Drawing.SystemColors.Control;
            this.trkbrScaleRatio.Location = new System.Drawing.Point(51, 157);
            this.trkbrScaleRatio.Name = "trkbrScaleRatio";
            this.trkbrScaleRatio.Size = new System.Drawing.Size(141, 56);
            this.trkbrScaleRatio.TabIndex = 27;
            this.trkbrScaleRatio.ValueChanged += new System.EventHandler(this.trkbrScaleRatio_ValueChanged);
            // 
            // btnSimu
            // 
            this.btnSimu.Location = new System.Drawing.Point(51, 228);
            this.btnSimu.Name = "btnSimu";
            this.btnSimu.Size = new System.Drawing.Size(141, 48);
            this.btnSimu.TabIndex = 27;
            this.btnSimu.Text = "Simulate";
            this.btnSimu.UseVisualStyleBackColor = true;
            this.btnSimu.Click += new System.EventHandler(this.btnSimu_Click);
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(51, 293);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(141, 48);
            this.btnClear.TabIndex = 29;
            this.btnClear.Text = "Clear";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // btnOpenFile
            // 
            this.btnOpenFile.Location = new System.Drawing.Point(51, 33);
            this.btnOpenFile.Name = "btnOpenFile";
            this.btnOpenFile.Size = new System.Drawing.Size(141, 48);
            this.btnOpenFile.TabIndex = 25;
            this.btnOpenFile.Text = "Open File";
            this.btnOpenFile.UseVisualStyleBackColor = true;
            this.btnOpenFile.Click += new System.EventHandler(this.btnOpenFile_Click);
            // 
            // SharpGLForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1902, 1033);
            this.Controls.Add(this.grpbxSimu);
            this.Controls.Add(this.grpbxIK);
            this.Controls.Add(this.grpbxAOV);
            this.Controls.Add(this.grpbxCtr);
            this.Controls.Add(this.openGLControl);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "SharpGLForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "KUKA KR5";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            ((System.ComponentModel.ISupportInitialize)(this.openGLControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trkbrJoint1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trkbrJoint6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trkbrJoint5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trkbrJoint4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trkbrJoint3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trkbrJoint2)).EndInit();
            this.grpbxCtr.ResumeLayout(false);
            this.grpbxCtr.PerformLayout();
            this.grpbxAOV.ResumeLayout(false);
            this.grpbxIK.ResumeLayout(false);
            this.grpbxIK.PerformLayout();
            this.grpbxSimu.ResumeLayout(false);
            this.grpbxSimu.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trkbrAccuracy)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trkbrScaleRatio)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private SharpGL.OpenGLControl openGLControl;
        private System.Windows.Forms.TrackBar trkbrJoint1;
        private System.Windows.Forms.TrackBar trkbrJoint6;
        private System.Windows.Forms.TrackBar trkbrJoint5;
        private System.Windows.Forms.TrackBar trkbrJoint4;
        private System.Windows.Forms.TrackBar trkbrJoint3;
        private System.Windows.Forms.TrackBar trkbrJoint2;
        private System.Windows.Forms.Label lblJoint1;
        private System.Windows.Forms.Label lblJoint2;
        private System.Windows.Forms.Label lblJoint3;
        private System.Windows.Forms.Label lblJoint4;
        private System.Windows.Forms.Label lblJoint5;
        private System.Windows.Forms.Label lblJoint6;
        private System.Windows.Forms.GroupBox grpbxCtr;
        private System.Windows.Forms.Button btnResetJointPos;
        private System.Windows.Forms.Button btnResetScene;
        private System.Windows.Forms.GroupBox grpbxAOV;
        private System.Windows.Forms.Button btnTopScene;
        private System.Windows.Forms.Button btnRightScene;
        private System.Windows.Forms.Button btnLeftScene;
        private System.Windows.Forms.Button btnIK;
        private System.Windows.Forms.Timer timSimu;
        private System.Windows.Forms.GroupBox grpbxIK;
        private System.Windows.Forms.Label lblr31;
        private System.Windows.Forms.Label lblr21;
        private System.Windows.Forms.Label lblr11;
        private System.Windows.Forms.TextBox txtr31;
        private System.Windows.Forms.TextBox txtr21;
        private System.Windows.Forms.TextBox txtr11;
        private System.Windows.Forms.Label lblZ;
        private System.Windows.Forms.Label lblY;
        private System.Windows.Forms.Label lblX;
        private System.Windows.Forms.TextBox txtZ;
        private System.Windows.Forms.TextBox txtY;
        private System.Windows.Forms.TextBox txtX;
        private System.Windows.Forms.Label lblr33;
        private System.Windows.Forms.Label lblr23;
        private System.Windows.Forms.Label lblr13;
        private System.Windows.Forms.TextBox txtr33;
        private System.Windows.Forms.TextBox txtr23;
        private System.Windows.Forms.TextBox txtr13;
        private System.Windows.Forms.GroupBox grpbxSimu;
        private System.Windows.Forms.Button btnSimu;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Button btnOpenFile;
        private System.Windows.Forms.TextBox txtJoint6Value;
        private System.Windows.Forms.TextBox txtJoint5Value;
        private System.Windows.Forms.TextBox txtJoint4Value;
        private System.Windows.Forms.TextBox txtJoint3Value;
        private System.Windows.Forms.TextBox txtJoint2Value;
        private System.Windows.Forms.TextBox txtJoint1Value;
        private System.Windows.Forms.Label lblr12;
        private System.Windows.Forms.TextBox txtr12;
        private System.Windows.Forms.Label lblr32;
        private System.Windows.Forms.TextBox txtr32;
        private System.Windows.Forms.Label lblr22;
        private System.Windows.Forms.TextBox txtr22;
        private System.Windows.Forms.TrackBar trkbrScaleRatio;
        private System.Windows.Forms.Label lblScaleRatio;
        private System.Windows.Forms.Button btnPauseStart;
        private System.Windows.Forms.Button btnBehindScene;
        private System.Windows.Forms.Label lblAccuracy;
        private System.Windows.Forms.TrackBar trkbrAccuracy;
    }
}

