using System.ComponentModel;

namespace PaintedControls
{
    public class CustomButton: BorderedControl
    {
        public override string Text
        {
            get => _control.Text;
            set
            {
                _control.Text = value;
                FullRepaint();
            }
        }
        void InitEvents()
        {
            Paint += OnPaint;
            _control.MouseDown += OnMouseDown;
            _control.MouseEnter += OnMouseEnter;
            _control.MouseLeave += OnMouseLeave;
            _control.MouseUp += OnMouseUp;
            FontChanged += (o, e) => _control.Font = Font;
        }
        public CustomButton()
        {
            _control = new Label()
            {
                Parent = this,
                AutoSize = false,
                BackColor = Color.Transparent,
                TextAlign = ContentAlignment.MiddleCenter,
            };
            InitEvents();
            UpdateControlPosition();
        }

        private void OnPaint(object? sender, PaintEventArgs e)
        {
            RepaintBorder();
        }

        public override void FullRepaint()
        {
            _control.ForeColor = ForeColors.CurrentColor;
            _control.BackColor = BackColors.CurrentColor;
            Invalidate();          
        }

        protected virtual void OnMouseUp(object? sender, MouseEventArgs e)
        {
            CurrentState = ColorStates.Hovered;
            if(ClientRectangle.Contains(e.Location))
                InvokeOnClick(this, e);
        }

        protected virtual void OnMouseLeave(object? sender, EventArgs e)
        {
            CurrentState = ColorStates.Default;
        }

        protected virtual void OnMouseEnter(object? sender, EventArgs e)
        {
            CurrentState = ColorStates.Hovered;
        }

        protected virtual void OnMouseDown(object? sender, MouseEventArgs e)
        {
            CurrentState = ColorStates.Pressed;
        }
    }
}
