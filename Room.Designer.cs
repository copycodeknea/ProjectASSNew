namespace ProjectASS
{
    partial class Room
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.phonenumbertxt = new System.Windows.Forms.TextBox();
            this.roomnumbertxt = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.rdyes = new System.Windows.Forms.RadioButton();
            this.rdno = new System.Windows.Forms.RadioButton();
            this.label3 = new System.Windows.Forms.Label();
            this.deletebtn = new System.Windows.Forms.Button();
            this.editbtn = new System.Windows.Forms.Button();
            this.addbtn = new System.Windows.Forms.Button();
            this.searchbtn = new System.Windows.Forms.Button();
            this.searchtxt = new System.Windows.Forms.TextBox();
            this.roomdatagridview = new System.Windows.Forms.DataGridView();
            this.refreshbtn = new System.Windows.Forms.Button();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.roomdatagridview)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.DarkTurquoise;
            this.panel1.Controls.Add(this.label1);
            this.panel1.Location = new System.Drawing.Point(-19, -3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1219, 75);
            this.panel1.TabIndex = 22;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 19.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(454, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(290, 38);
            this.label1.TabIndex = 0;
            this.label1.Text = "Room Information";
            // 
            // phonenumbertxt
            // 
            this.phonenumbertxt.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.phonenumbertxt.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.phonenumbertxt.ForeColor = System.Drawing.Color.Black;
            this.phonenumbertxt.Location = new System.Drawing.Point(227, 256);
            this.phonenumbertxt.Name = "phonenumbertxt";
            this.phonenumbertxt.Size = new System.Drawing.Size(243, 34);
            this.phonenumbertxt.TabIndex = 26;
            // 
            // roomnumbertxt
            // 
            this.roomnumbertxt.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.roomnumbertxt.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.roomnumbertxt.ForeColor = System.Drawing.Color.Black;
            this.roomnumbertxt.Location = new System.Drawing.Point(227, 172);
            this.roomnumbertxt.Name = "roomnumbertxt";
            this.roomnumbertxt.Size = new System.Drawing.Size(243, 34);
            this.roomnumbertxt.TabIndex = 25;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.Black;
            this.label4.Location = new System.Drawing.Point(32, 262);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(176, 29);
            this.label4.TabIndex = 24;
            this.label4.Text = "Phone Number";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.Black;
            this.label2.Location = new System.Drawing.Point(32, 178);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(171, 29);
            this.label2.TabIndex = 23;
            this.label2.Text = "Room Number";
            // 
            // rdyes
            // 
            this.rdyes.AutoSize = true;
            this.rdyes.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rdyes.Location = new System.Drawing.Point(246, 363);
            this.rdyes.Name = "rdyes";
            this.rdyes.Size = new System.Drawing.Size(67, 29);
            this.rdyes.TabIndex = 27;
            this.rdyes.TabStop = true;
            this.rdyes.Text = "Yes";
            this.rdyes.UseVisualStyleBackColor = true;
            // 
            // rdno
            // 
            this.rdno.AutoSize = true;
            this.rdno.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rdno.Location = new System.Drawing.Point(373, 363);
            this.rdno.Name = "rdno";
            this.rdno.Size = new System.Drawing.Size(58, 29);
            this.rdno.TabIndex = 28;
            this.rdno.TabStop = true;
            this.rdno.Text = "No";
            this.rdno.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.Black;
            this.label3.Location = new System.Drawing.Point(46, 363);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(135, 29);
            this.label3.TabIndex = 29;
            this.label3.Text = "Room Free";
            // 
            // deletebtn
            // 
            this.deletebtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.deletebtn.ForeColor = System.Drawing.Color.Red;
            this.deletebtn.Location = new System.Drawing.Point(373, 446);
            this.deletebtn.Name = "deletebtn";
            this.deletebtn.Size = new System.Drawing.Size(122, 52);
            this.deletebtn.TabIndex = 32;
            this.deletebtn.Text = "DELETE";
            this.deletebtn.UseVisualStyleBackColor = true;
            // 
            // editbtn
            // 
            this.editbtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.editbtn.ForeColor = System.Drawing.Color.Thistle;
            this.editbtn.Location = new System.Drawing.Point(212, 446);
            this.editbtn.Name = "editbtn";
            this.editbtn.Size = new System.Drawing.Size(122, 52);
            this.editbtn.TabIndex = 31;
            this.editbtn.Text = "EDIT";
            this.editbtn.UseVisualStyleBackColor = true;
            // 
            // addbtn
            // 
            this.addbtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.addbtn.ForeColor = System.Drawing.Color.Thistle;
            this.addbtn.Location = new System.Drawing.Point(51, 446);
            this.addbtn.Name = "addbtn";
            this.addbtn.Size = new System.Drawing.Size(122, 52);
            this.addbtn.TabIndex = 30;
            this.addbtn.Text = "ADD";
            this.addbtn.UseVisualStyleBackColor = true;
            this.addbtn.Click += new System.EventHandler(this.addbtn_Click);
            // 
            // searchbtn
            // 
            this.searchbtn.BackColor = System.Drawing.Color.Gray;
            this.searchbtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.searchbtn.ForeColor = System.Drawing.Color.White;
            this.searchbtn.Location = new System.Drawing.Point(904, 105);
            this.searchbtn.Name = "searchbtn";
            this.searchbtn.Size = new System.Drawing.Size(109, 41);
            this.searchbtn.TabIndex = 35;
            this.searchbtn.Text = "Search";
            this.searchbtn.UseVisualStyleBackColor = false;
            // 
            // searchtxt
            // 
            this.searchtxt.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.searchtxt.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.searchtxt.ForeColor = System.Drawing.Color.Black;
            this.searchtxt.Location = new System.Drawing.Point(644, 105);
            this.searchtxt.Name = "searchtxt";
            this.searchtxt.Size = new System.Drawing.Size(243, 34);
            this.searchtxt.TabIndex = 34;
            // 
            // roomdatagridview
            // 
            this.roomdatagridview.BackgroundColor = System.Drawing.SystemColors.HighlightText;
            this.roomdatagridview.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.roomdatagridview.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2,
            this.Column3});
            this.roomdatagridview.Location = new System.Drawing.Point(528, 172);
            this.roomdatagridview.Name = "roomdatagridview";
            this.roomdatagridview.RowHeadersWidth = 51;
            this.roomdatagridview.RowTemplate.Height = 24;
            this.roomdatagridview.Size = new System.Drawing.Size(644, 443);
            this.roomdatagridview.TabIndex = 33;
            // 
            // refreshbtn
            // 
            this.refreshbtn.BackColor = System.Drawing.Color.Transparent;
            this.refreshbtn.BackgroundImage = global::ProjectASS.Properties.Resources.reloading;
            this.refreshbtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.refreshbtn.FlatAppearance.BorderSize = 0;
            this.refreshbtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.refreshbtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.refreshbtn.ForeColor = System.Drawing.Color.Transparent;
            this.refreshbtn.Location = new System.Drawing.Point(1051, 101);
            this.refreshbtn.Name = "refreshbtn";
            this.refreshbtn.Size = new System.Drawing.Size(54, 48);
            this.refreshbtn.TabIndex = 36;
            this.refreshbtn.UseVisualStyleBackColor = false;
            // 
            // Column1
            // 
            this.Column1.HeaderText = "Room ID";
            this.Column1.MinimumWidth = 6;
            this.Column1.Name = "Column1";
            this.Column1.Width = 150;
            // 
            // Column2
            // 
            this.Column2.HeaderText = "Room Phone";
            this.Column2.MinimumWidth = 6;
            this.Column2.Name = "Column2";
            this.Column2.Width = 150;
            // 
            // Column3
            // 
            this.Column3.HeaderText = "Room Availibility";
            this.Column3.MinimumWidth = 6;
            this.Column3.Name = "Column3";
            this.Column3.Width = 150;
            // 
            // Room
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1198, 650);
            this.Controls.Add(this.refreshbtn);
            this.Controls.Add(this.searchbtn);
            this.Controls.Add(this.searchtxt);
            this.Controls.Add(this.roomdatagridview);
            this.Controls.Add(this.deletebtn);
            this.Controls.Add(this.editbtn);
            this.Controls.Add(this.addbtn);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.rdno);
            this.Controls.Add(this.rdyes);
            this.Controls.Add(this.phonenumbertxt);
            this.Controls.Add(this.roomnumbertxt);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Room";
            this.Text = "Room";
            this.Load += new System.EventHandler(this.Room_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.roomdatagridview)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox phonenumbertxt;
        private System.Windows.Forms.TextBox roomnumbertxt;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.RadioButton rdyes;
        private System.Windows.Forms.RadioButton rdno;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button deletebtn;
        private System.Windows.Forms.Button editbtn;
        private System.Windows.Forms.Button addbtn;
        private System.Windows.Forms.Button refreshbtn;
        private System.Windows.Forms.Button searchbtn;
        private System.Windows.Forms.TextBox searchtxt;
        private System.Windows.Forms.DataGridView roomdatagridview;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
    }
}