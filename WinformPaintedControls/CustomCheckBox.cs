using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaintedControls
{
    
    public class CustomCheckBox: CustomButton
    {
        public event Action<CustomCheckBox, bool> CheckChanged;
        bool _isChecked = false;

        public bool Checked
        {
            get => _isChecked;
            set 
            {
                CurrentState = value ? ColorStates.Checked : ColorStates.Hovered;
                if (_isChecked != value)
                {
                    CheckChanged?.Invoke(this, _isChecked);
                    _isChecked = value;
                }
                
                
            }
        }

        public Control Button => _control;

        [Category("Colors"), Description("Back colors"), Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public new ColorCheckContainer BackColors { get; set; }

        [Category("Colors"), Description("Text and border colors"), Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public new ColorCheckContainer ForeColors { get; set; }
        public CustomCheckBox()
        {
            BackColors = new();
            ForeColors = new();
            base.BackColors = BackColors;
            base.ForeColors = ForeColors;
            InitEvents();
        }

        void InitEvents()
        {
            _control.MouseClick += OnMouseClick;
            _control.MouseDoubleClick += OnMouseClick;
            BackColors.ColorChanged += (e) => FullRepaint();
            ForeColors.ColorChanged += (e) => FullRepaint();
        }

        private void OnMouseClick(object? sender, MouseEventArgs e)
        {
            Checked = !Checked;
            FullRepaint();
            InvokeOnClick(this, e);
        }

        protected override void OnMouseUp(object? sender, MouseEventArgs e)
        {
            if (Checked && !ClientRectangle.Contains(e.Location))
            {
                CurrentState = ColorStates.Checked;
            }
        }

        protected override void OnMouseLeave(object? sender, EventArgs e)
        {
            if (!Checked) CurrentState = ColorStates.Default;
        }

        protected override void OnMouseEnter(object? sender, EventArgs e)
        {
            if (!Checked) CurrentState = ColorStates.Hovered;
        }

        protected override void OnMouseDown(object? sender, MouseEventArgs e)
        {
            CurrentState = ColorStates.Pressed;
        }


    }
}
