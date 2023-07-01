using System.ComponentModel;
namespace PaintedControls
{
    public class BorderedControl : Control
    {
        protected int _borderWidth = 2;
        protected Control _control = new();

        [Category("Colors"), Description("Back colors"),
           Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public ColorPressContainer BackColors { get; set; }

        [Category("Colors"), Description("Text and border colors"),
           Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public ColorPressContainer ForeColors { get; set; }

        [Category("CustomProperty"), Description("Size of border")]
        public int BorderWidth
        {
            get => _borderWidth;
            set
            {
                _borderWidth = value;
                UpdateControlPosition();
                FullRepaint();
            }
        }
        [Category("CustomProperty"), Description("Width without border")]
        public int ClientWidth
        {
            get => Width - 2 * BorderWidth;
            set => Width = value + 2 * BorderWidth;
        }

        [Category("CustomProperty"), Description("Height without border")]
        public int ClientHeight
        {
            get => Height - 2 * BorderWidth;
            set => Height = value + 2 * BorderWidth;
        }

        public int CurrentState
        {
            get => BackColors.CurrentState;
            set
            {
                BackColors.CurrentState = value;
                ForeColors.CurrentState = value;
                FullRepaint();
            }
        }
        public BorderedControl()
        {
            BackColors = new();
            ForeColors = new();
            Resize += OnResize;
            BackColors.ColorChanged += (c) => FullRepaint();
            ForeColors.ColorChanged += (c) => FullRepaint();
        }

        protected virtual void OnResize(object? sender, EventArgs e)
        {
            if (_control == null) return;
            _control.Size = new Size(ClientWidth, ClientHeight);
        }

        protected virtual void UpdateControlPosition()
        {
            _control.Location = new(BorderWidth, BorderWidth);
            _control.Size = new Size(ClientWidth, ClientHeight);
        }
        public virtual void FullRepaint()
        {
            RepaintBorder();
        }
        protected void RepaintBorder()
        {
            var borderColor = ForeColors.CurrentColor;
            using var e = CreateGraphics();
            ControlPaint.DrawBorder(e, ClientRectangle,
                borderColor, BorderWidth, ButtonBorderStyle.Solid,
                borderColor, BorderWidth, ButtonBorderStyle.Solid,
                borderColor, BorderWidth, ButtonBorderStyle.Solid,
                borderColor, BorderWidth, ButtonBorderStyle.Solid
                );
        }

    }
}
