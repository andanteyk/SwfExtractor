namespace TestGUI {
	partial class TestGUI {
		/// <summary>
		/// 必要なデザイナー変数です。
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// 使用中のリソースをすべてクリーンアップします。
		/// </summary>
		/// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
		protected override void Dispose( bool disposing ) {
			if ( disposing && ( components != null ) ) {
				components.Dispose();
			}
			base.Dispose( disposing );
		}

		#region Windows フォーム デザイナーで生成されたコード

		/// <summary>
		/// デザイナー サポートに必要なメソッドです。このメソッドの内容を
		/// コード エディターで変更しないでください。
		/// </summary>
		private void InitializeComponent() {
			this.tabControl1 = new System.Windows.Forms.TabControl();
			this.TabExtract = new System.Windows.Forms.TabPage();
			this.tabPage2 = new System.Windows.Forms.TabPage();
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.tabControl1.SuspendLayout();
			this.TabExtract.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
			this.SuspendLayout();
			// 
			// tabControl1
			// 
			this.tabControl1.Controls.Add(this.TabExtract);
			this.tabControl1.Controls.Add(this.tabPage2);
			this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabControl1.Location = new System.Drawing.Point(0, 0);
			this.tabControl1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new System.Drawing.Size(331, 326);
			this.tabControl1.TabIndex = 0;
			// 
			// TabExtract
			// 
			this.TabExtract.AllowDrop = true;
			this.TabExtract.Controls.Add(this.pictureBox1);
			this.TabExtract.Location = new System.Drawing.Point(4, 24);
			this.TabExtract.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.TabExtract.Name = "TabExtract";
			this.TabExtract.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.TabExtract.Size = new System.Drawing.Size(323, 298);
			this.TabExtract.TabIndex = 0;
			this.TabExtract.Text = "Extract";
			this.TabExtract.UseVisualStyleBackColor = true;
			this.TabExtract.DragDrop += new System.Windows.Forms.DragEventHandler(this.TabExtract_DragDrop);
			this.TabExtract.DragEnter += new System.Windows.Forms.DragEventHandler(this.TabExtract_DragEnter);
			// 
			// tabPage2
			// 
			this.tabPage2.Location = new System.Drawing.Point(4, 22);
			this.tabPage2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.tabPage2.Name = "tabPage2";
			this.tabPage2.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.tabPage2.Size = new System.Drawing.Size(225, 99);
			this.tabPage2.TabIndex = 1;
			this.tabPage2.Text = "tabPage2";
			this.tabPage2.UseVisualStyleBackColor = true;
			// 
			// pictureBox1
			// 
			this.pictureBox1.Location = new System.Drawing.Point(8, 7);
			this.pictureBox1.MinimumSize = new System.Drawing.Size(16, 16);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size(100, 50);
			this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
			this.pictureBox1.TabIndex = 0;
			this.pictureBox1.TabStop = false;
			// 
			// TestGUI
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(331, 326);
			this.Controls.Add(this.tabControl1);
			this.Font = new System.Drawing.Font("Meiryo UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.Name = "TestGUI";
			this.Text = "SWF Extractor";
			this.tabControl1.ResumeLayout(false);
			this.TabExtract.ResumeLayout(false);
			this.TabExtract.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TabControl tabControl1;
		private System.Windows.Forms.TabPage TabExtract;
		private System.Windows.Forms.TabPage tabPage2;
		private System.Windows.Forms.PictureBox pictureBox1;
	}
}

