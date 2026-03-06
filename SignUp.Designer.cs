namespace ProjectASS
{
    partial class SignupForm
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
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.Semailtxt = new System.Windows.Forms.TextBox();
            this.Spasswordtxt = new System.Windows.Forms.TextBox();
            this.Signupbtn = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.Sclosebtn = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackgroundImage = global::ProjectASS.Properties.Resources.laoclassic_kompong_phluk_village_5149340_1920;
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(446, 483);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(464, 172);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(74, 29);
            this.label2.TabIndex = 2;
            this.label2.Text = "Email";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(464, 255);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(120, 29);
            this.label3.TabIndex = 3;
            this.label3.Text = "Password";
            // 
            // Semailtxt
            // 
            this.Semailtxt.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Semailtxt.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Semailtxt.Location = new System.Drawing.Point(605, 172);
            this.Semailtxt.Name = "Semailtxt";
            this.Semailtxt.Size = new System.Drawing.Size(232, 30);
            this.Semailtxt.TabIndex = 4;
            // 
            // Spasswordtxt
            // 
            this.Spasswordtxt.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Spasswordtxt.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Spasswordtxt.Location = new System.Drawing.Point(605, 254);
            this.Spasswordtxt.Name = "Spasswordtxt";
            this.Spasswordtxt.Size = new System.Drawing.Size(232, 30);
            this.Spasswordtxt.TabIndex = 5;
            // 
            // Signupbtn
            // 
            this.Signupbtn.FlatAppearance.BorderSize = 4;
            this.Signupbtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Signupbtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Signupbtn.Location = new System.Drawing.Point(626, 354);
            this.Signupbtn.Name = "Signupbtn";
            this.Signupbtn.Size = new System.Drawing.Size(154, 50);
            this.Signupbtn.TabIndex = 6;
            this.Signupbtn.Text = "Sign Up";
            this.Signupbtn.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 22.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(642, 42);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(151, 42);
            this.label1.TabIndex = 7;
            this.label1.Text = "Sign Up";
            // 
            // Sclosebtn
            // 
            this.Sclosebtn.Location = new System.Drawing.Point(871, -1);
            this.Sclosebtn.Name = "Sclosebtn";
            this.Sclosebtn.Size = new System.Drawing.Size(30, 29);
            this.Sclosebtn.TabIndex = 8;
            this.Sclosebtn.Text = "X";
            this.Sclosebtn.UseVisualStyleBackColor = true;
            this.Sclosebtn.Click += new System.EventHandler(this.Sclosebtn_Click);
            // 
            // SignupForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(900, 450);
            this.Controls.Add(this.Sclosebtn);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.Signupbtn);
            this.Controls.Add(this.Spasswordtxt);
            this.Controls.Add(this.Semailtxt);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.pictureBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "SignupForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "SignUp";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox Semailtxt;
        private System.Windows.Forms.TextBox Spasswordtxt;
        private System.Windows.Forms.Button Signupbtn;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button Sclosebtn;
    }
}