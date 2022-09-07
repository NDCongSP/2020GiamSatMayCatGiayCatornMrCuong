namespace HDSDK_Demo_CSharp
{
    partial class FormSDK
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.buttonText = new System.Windows.Forms.Button();
            this.buttonImage = new System.Windows.Forms.Button();
            this.buttonTime = new System.Windows.Forms.Button();
            this.buttonRealTimeArea = new System.Windows.Forms.Button();
            this.buttonCmd = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // buttonText
            // 
            this.buttonText.Location = new System.Drawing.Point(33, 35);
            this.buttonText.Name = "buttonText";
            this.buttonText.Size = new System.Drawing.Size(86, 23);
            this.buttonText.TabIndex = 0;
            this.buttonText.Text = "Text";
            this.buttonText.UseVisualStyleBackColor = true;
            this.buttonText.Click += new System.EventHandler(this.buttonText_Click);
            // 
            // buttonImage
            // 
            this.buttonImage.Location = new System.Drawing.Point(193, 35);
            this.buttonImage.Name = "buttonImage";
            this.buttonImage.Size = new System.Drawing.Size(87, 23);
            this.buttonImage.TabIndex = 0;
            this.buttonImage.Text = "Image";
            this.buttonImage.UseVisualStyleBackColor = true;
            this.buttonImage.Click += new System.EventHandler(this.buttonImage_Click);
            // 
            // buttonTime
            // 
            this.buttonTime.Location = new System.Drawing.Point(33, 121);
            this.buttonTime.Name = "buttonTime";
            this.buttonTime.Size = new System.Drawing.Size(86, 23);
            this.buttonTime.TabIndex = 0;
            this.buttonTime.Text = "Time";
            this.buttonTime.UseVisualStyleBackColor = true;
            this.buttonTime.Click += new System.EventHandler(this.buttonTime_Click);
            // 
            // buttonRealTimeArea
            // 
            this.buttonRealTimeArea.Location = new System.Drawing.Point(193, 121);
            this.buttonRealTimeArea.Name = "buttonRealTimeArea";
            this.buttonRealTimeArea.Size = new System.Drawing.Size(87, 23);
            this.buttonRealTimeArea.TabIndex = 0;
            this.buttonRealTimeArea.Text = "RealTimeArea";
            this.buttonRealTimeArea.UseVisualStyleBackColor = true;
            this.buttonRealTimeArea.Click += new System.EventHandler(this.buttonRealTimeArea_Click);
            // 
            // buttonCmd
            // 
            this.buttonCmd.Location = new System.Drawing.Point(33, 197);
            this.buttonCmd.Name = "buttonCmd";
            this.buttonCmd.Size = new System.Drawing.Size(86, 23);
            this.buttonCmd.TabIndex = 0;
            this.buttonCmd.Text = "Command";
            this.buttonCmd.UseVisualStyleBackColor = true;
            this.buttonCmd.Click += new System.EventHandler(this.buttonCmd_Click);
            // 
            // FormSDK
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(306, 294);
            this.Controls.Add(this.buttonRealTimeArea);
            this.Controls.Add(this.buttonCmd);
            this.Controls.Add(this.buttonTime);
            this.Controls.Add(this.buttonImage);
            this.Controls.Add(this.buttonText);
            this.Name = "FormSDK";
            this.Text = "SDKDemo";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonText;
        private System.Windows.Forms.Button buttonImage;
        private System.Windows.Forms.Button buttonTime;
        private System.Windows.Forms.Button buttonRealTimeArea;
        private System.Windows.Forms.Button buttonCmd;
    }
}

