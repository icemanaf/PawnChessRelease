namespace chessboard
{
    partial class frmPawnChess
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
            this.pnlChessBoard = new System.Windows.Forms.Panel();
            this.btnReset = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.updAlphaBeta = new System.Windows.Forms.NumericUpDown();
            this.chkUserIsWhite = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.updAlphaBeta)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlChessBoard
            // 
            this.pnlChessBoard.Location = new System.Drawing.Point(12, 12);
            this.pnlChessBoard.MaximumSize = new System.Drawing.Size(420, 420);
            this.pnlChessBoard.MinimumSize = new System.Drawing.Size(420, 420);
            this.pnlChessBoard.Name = "pnlChessBoard";
            this.pnlChessBoard.Size = new System.Drawing.Size(420, 420);
            this.pnlChessBoard.TabIndex = 0;
            // 
            // btnReset
            // 
            this.btnReset.Location = new System.Drawing.Point(12, 438);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(97, 34);
            this.btnReset.TabIndex = 1;
            this.btnReset.Text = "New Game";
            this.btnReset.UseVisualStyleBackColor = true;
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(267, 458);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(123, 13);
            this.label2.TabIndex = 19;
            this.label2.Text = "Level of Play (Ply Depth)";
            // 
            // updAlphaBeta
            // 
            this.updAlphaBeta.Location = new System.Drawing.Point(396, 456);
            this.updAlphaBeta.Name = "updAlphaBeta";
            this.updAlphaBeta.Size = new System.Drawing.Size(36, 20);
            this.updAlphaBeta.TabIndex = 18;
            // 
            // chkUserIsWhite
            // 
            this.chkUserIsWhite.AutoSize = true;
            this.chkUserIsWhite.Location = new System.Drawing.Point(12, 478);
            this.chkUserIsWhite.Name = "chkUserIsWhite";
            this.chkUserIsWhite.Size = new System.Drawing.Size(89, 17);
            this.chkUserIsWhite.TabIndex = 20;
            this.chkUserIsWhite.Text = "User is White";
            this.chkUserIsWhite.UseVisualStyleBackColor = true;
            // 
            // frmPawnChess
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(447, 502);
            this.Controls.Add(this.chkUserIsWhite);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.updAlphaBeta);
            this.Controls.Add(this.btnReset);
            this.Controls.Add(this.pnlChessBoard);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(600, 575);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(455, 529);
            this.Name = "frmPawnChess";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Pawn Chess v 1.00";
            this.Load += new System.EventHandler(this.frmPawnChess_Load);
            ((System.ComponentModel.ISupportInitialize)(this.updAlphaBeta)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel pnlChessBoard;
        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown updAlphaBeta;
        private System.Windows.Forms.CheckBox chkUserIsWhite;
    }
}

