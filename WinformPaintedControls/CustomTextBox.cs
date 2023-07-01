using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Windows.Forms;

namespace PaintedControls
{
    public partial class CustomTextBox : BorderedControl
    {
        float _fontSizeRatio;
        public override string Text
        {
            get => _control.Text;
            set
            {
                _control.Text = value;
                FullRepaint();
            }
        }

        public TextBox TextBox => (TextBox)_control;

        private void RecalculateFontSizeRatio(Font f)
        {
            int emHeight = f.FontFamily.GetEmHeight(f.Style);
            int cellAscent = f.FontFamily.GetCellAscent(f.Style);
            int cellDescent = f.FontFamily.GetCellDescent(f.Style);
            _fontSizeRatio = (cellDescent *1.5f + 0.5f *cellAscent) / emHeight;
        }

        [Category("CustomProperty"), Description("Font height in pixels")]
        public float FontSize
        {
            get => _control.Font.Height;
        }
        [Category("CustomProperty"), Description("Font of textbox (height is automatic"),
            Browsable(true),
            Editor("System.Drawing.Design.FontNameEditor, System.Drawing.Design",
                    "System.Drawing.Design.UITypeEditor, System.Drawing"),
            TypeConverter(typeof(FontConverter.FontNameConverter))]
        public string FontName
        {
            get => _control.Font.Name;
            set
            {
                _control.Font = new Font(value, 10);
                RecalculateFontSizeRatio(_control.Font);
                UpdateTextBoxFont();
            }
        }

        public CustomTextBox():base()
        {
            
            MinimumSize = new(BorderWidth+5, BorderWidth+5);
            _control = new TextBox
            {
                Font = new Font("Consolas", 10),
                BorderStyle = BorderStyle.None,
                AutoSize = false,
                Parent = this,
                Location = new Point(BorderWidth, BorderWidth)
            };
            FontName = "Consolas";
            InitEvents();
        }

        void InitEvents()
        {

            Paint += OnPaint;
            _control.MouseEnter += OnMouseEnter;
            _control.MouseLeave += OnMouseLeave;
            _control.GotFocus += OnFocused;
            _control.LostFocus += OnDefocused;
        }

        private void OnPaint(object? sender, PaintEventArgs e)
        {
            RepaintBorder();
        }

        public float GetUpdatedFontSize(float height)
        {
            float size = height * _fontSizeRatio;
            return MathF.Floor(size);
        }

        public void UpdateTextBoxFont()
        {
            float fSize;
            _control.Font = new(_control.Font.FontFamily,
                (fSize =GetUpdatedFontSize(ClientHeight))>0?fSize:5,
                GraphicsUnit.Pixel);
        }
        protected override void OnResize(object? sender, EventArgs e)
        {
            if (_control == null) return;
            base.OnResize(sender, e);
            UpdateTextBoxFont();       
            
        }

        public override void FullRepaint()
        {
            _control.BackColor = BackColors.CurrentColor;
            _control.ForeColor = ForeColors.CurrentColor;
            Invalidate();
        }


        private void OnMouseEnter(object? sender, EventArgs e)
        {
            if (CurrentState == ColorStates.Pressed) return;
            CurrentState = ColorStates.Hovered;
        }

        private void OnMouseLeave(object? sender, EventArgs e)
        {
            if (CurrentState == ColorStates.Pressed) return;
            CurrentState = ColorStates.Default;
        }

        private void OnFocused(object? sender, EventArgs e)
        {
            CurrentState = ColorStates.Pressed;
        }

        private void OnDefocused(object? sender, EventArgs e)
        {
            CurrentState = ColorStates.Default;
        }

    }
}
