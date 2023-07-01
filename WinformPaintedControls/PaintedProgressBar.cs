using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using Timer = System.Windows.Forms.Timer;
namespace PaintedControls
{
    public class ValueChangerByTimer
    {
        Action ChangeValue;
        public Action<float> ChangeOuterValue;
        public Timer Timer { get; set; }
        public float OldValue { get; set; }
        public float NewValue { get; set; }
        public ValueChangerByTimer()
        {
            ChangeOuterValue = (e)=>{ };
            ChangeValue = Decrease;
            Timer = new() { Interval = 1 };
            Timer.Tick += (o, e) => ChangeValue();
        }

        void Decrease()
        {
            OldValue--;
            if(OldValue<=NewValue)
            {
                OldValue = NewValue;
                Timer.Stop();
            }
            ChangeOuterValue(OldValue);
        }

        void Increase()
        {
            OldValue++;
            if (OldValue >= NewValue)
            {
                OldValue = NewValue;
                Timer.Stop();
            }
            ChangeOuterValue(OldValue);
        }

        public void Start()
        {
            if(OldValue<NewValue)
            {
                ChangeValue = Increase;
            }
            else
            {
                ChangeValue = Decrease;
            }
            Timer.Start();
        }
    }

    public class PaintedProgressBar : Control
    {
        ValueChangerByTimer _timer;
        private float _value = 0, _percent =0;
        private Color
            _progressMainColor = Color.Red,
            _progressSubColor = Color.Orange;

        public Color ProgressMainColor
        {
            get => _progressMainColor;
            set
            {
                _progressMainColor = value;
                Invalidate();
            }
        }

        public Color ProgressSubColor
        {
            get => _progressSubColor;
            set
            {
                _progressSubColor = value;
                Invalidate();
            }
        }

        void SetValue(float value)
        {
            var oldValue = _value;
            _value = value;
            
            var newValueRect = ClientRectangle;
            var oldValueRect = ClientRectangle;

            newValueRect.Width = (int)(newValueRect.Width * _value / 100);

            oldValueRect.Width = (int)(oldValueRect.Width * oldValue / 100);

            Rectangle updateRect = new();

            if (newValueRect.Width > oldValueRect.Width)
            {
                updateRect.X = oldValueRect.Size.Width;
                updateRect.Width = newValueRect.Width - oldValueRect.Width;
            }
            else
            {
                updateRect.X = newValueRect.Size.Width;
                updateRect.Width = oldValueRect.Width - newValueRect.Width;
            }

            updateRect.Height = Height;
            Invalidate(updateRect);
        }
        public PaintedProgressBar()
        {
            BackColor = Color.DarkBlue;
            _timer = new()
            {
                ChangeOuterValue = SetValue
            };
        }


        protected override void OnResize(EventArgs e)
        {
            Invalidate();
        }


        public float Percent
        {
            get => _percent;

            set
            {
                if (value < 0)
                {
                    value = 0;
                }
                else if (value > 100)
                {
                    value = 100;
                }
                _timer.Timer.Stop();
                _timer.NewValue = value;
                _timer.OldValue = _percent;
                _timer.Start();
                _percent = value;
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Rectangle rect = ClientRectangle;
            rect.Width = (int)(rect.Width * _value / 100);
            if (rect.Width == 0) return;
            Graphics g = e.Graphics;

            var brush = new LinearGradientBrush(rect, ProgressMainColor, ProgressSubColor, LinearGradientMode.Vertical);
            g.FillRectangle(brush, rect);

            brush.Dispose();
            g.Dispose();
        }
    }
}
