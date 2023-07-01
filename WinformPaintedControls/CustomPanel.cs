using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace PaintedControls
{

    [Designer("System.Windows.Forms.Design.ParentControlDesigner," +
        " System.Design", typeof(IDesigner))]
    public class CustomPanel :BorderedControl
    {
        public new ControlCollection Controls => _control.Controls;
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public Panel Panel => (Panel)_control;
        public CustomPanel()
        {
            
            _control = new Panel()
            {
                Parent = this,
                BorderStyle = BorderStyle.None,
                Location = new (BorderWidth, BorderWidth)
            };
            InitFlags();
            InitEvents();

            
        }

        void InitEvents()
        {
            _control.Paint += OnPaint;
            _control.MouseEnter += OnMouseEnter;
            _control.MouseLeave += OnMouseLeave;
        }

        void InitFlags()
        {
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            SetStyle(ControlStyles.Opaque, true);
        }
        public override void FullRepaint()
        {
            _control.BackColor = BackColors.CurrentColor;
            RepaintBorder();
        }

        private void OnPaint(object? sender, PaintEventArgs e)
        {
            FullRepaint();
            foreach(Control control in Controls)
            {
                control.BringToFront();
            }
        }
        private void OnMouseLeave(object? sender, EventArgs e)
        {
            CurrentState = ColorStates.Default;
        }

        private void OnMouseEnter(object? sender, EventArgs e)
        {
            CurrentState = ColorStates.Hovered;
        }
        
    }

}
