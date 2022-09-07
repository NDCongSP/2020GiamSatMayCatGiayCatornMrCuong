using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace BangLed
{
    class MyRadioButton:RadioButton
    {
        public MyRadioButton()
        {
            Appearance = System.Windows.Forms.Appearance.Button;
            FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            TextAlign = ContentAlignment.MiddleCenter;
            FlatAppearance.BorderSize = 2;
        }
        public override bool AutoSize
        {
            get { return base.AutoSize; }
            set { base.AutoSize = false; }
        }
        protected override void OnPaint(PaintEventArgs e)
        {


            e.Graphics.Clear(BackColor);
            int h = this.ClientSize.Height;
            //using (SolidBrush brush = new SolidBrush(ForeColor))
            //    e.Graphics.DrawString(Text, Font, brush, 27, 4);

            Point pt = new Point(0, 1);
            Rectangle rect = new Rectangle(pt, new Size(h - 2, h - 2));

            e.Graphics.FillRectangle(Brushes.Beige, rect);

            if (Checked)
            {
                using (SolidBrush brush = new SolidBrush(ForeColor))
                using (Font wing = new Font("Wingdings", h))
                    e.Graphics.DrawString("ü", wing, brush, (-30 * h) / 100, (-10 * h) / 100);
            }
            e.Graphics.DrawRectangle(Pens.DarkSlateBlue, rect);

            //Rectangle fRect = ClientRectangle;

            //if (Focused)
            //{
            //    fRect.Inflate(-1, -1);
            //    using (Pen pen = new Pen(Brushes.Gray) { DashStyle = DashStyle.Dot })
            //        e.Graphics.DrawRectangle(pen, fRect);
            //}

        }
    }
}
