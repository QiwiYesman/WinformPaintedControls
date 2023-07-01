using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PaintedControls
{
    public class CustomListBox: BorderedControl
    {
        int _hoverIndex = -1;
        protected int HoverIndex
        {
            get => _hoverIndex;
            set
            {
                if (_hoverIndex == value) return;
                
                if (HoverIndex >= 0 && HoverIndex < Items.Count)
                {
                    List.Invalidate(List.GetItemRectangle(HoverIndex));
                }
                _hoverIndex = value;
                if (HoverIndex>= 0)
                {
                    List.Invalidate(List.GetItemRectangle(HoverIndex));
                }
                
            }
        }
        
        private ListBox _list;
        [Category("CustomProperty"), Description("Strings-items of list")]
        [Editor("System.Windows.Forms.Design.ListControlStringCollectionEditor, "
            + "System.Drawing.Design", typeof(System.Drawing.Design.UITypeEditor))]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public ListBox.ObjectCollection Items
        {
            get => List.Items;
            
        }
        public ListBox List => _list;
        public CustomListBox()
        {
            _control = new ListBox()
            {
                Parent = this,
                Location = new(BorderWidth, BorderWidth),
                DrawMode = DrawMode.OwnerDrawFixed,
                BorderStyle = BorderStyle.None,
                BackColor = BackColors.CurrentColor,
                ForeColor = ForeColors.CurrentColor,
            };
            _list = (ListBox)_control;
            InitColors();
            InitEvents();
        }

        void InitColors()
        {
            BackColors.PressColor = Color.DarkBlue;
            ForeColors.PressColor = Color.Yellow;
            ForeColors.DefaultColor = Color.White;
        }
        void InitEvents()
        {
            List.DrawItem += OnDrawItem;
            List.MouseEnter += OnMouseEnter;
            List.MouseLeave += OnMouseLeave;
            List.MouseMove += OnMouseMove;
            Paint += OnPaint;
            FontChanged += OnFontChanged;
        }
        private void OnMouseMove(object? sender, MouseEventArgs e)
        {
            HoverIndex = List.IndexFromPoint(e.Location);
        }

        private void OnMouseLeave(object? sender, EventArgs e)
        {
            HoverIndex = -1;
            CurrentState = ColorStates.Default;
        }

        private void OnMouseEnter(object? sender, EventArgs e)
        {
            CurrentState = ColorStates.Hovered;
        }

        private void OnFontChanged(object? sender, EventArgs e)
        {
            List.ItemHeight = Font.Height;
        }

        private void OnPaint(object? sender, PaintEventArgs e)
        {
            BackColor = BackColors.CurrentColor;
            FullRepaint();
        }

        private void OnDrawItem(object? sender, DrawItemEventArgs e)
        {
            if (e.Font == null) return;
            if (e.Index < 0)
            {
                e.DrawBackground();
                return;
            }
            var text = List.Items[e.Index].ToString();
            using var bb = new SolidBrush(BackColors.PressColor);
            using var bf = new SolidBrush(ForeColors.PressColor);
            if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
            {

            }
            else if(e.Index == HoverIndex)
            {
                bb.Color = BackColors.HoverColor;
                bf.Color = ForeColors.HoverColor;
            }
            else
            {
                bb.Color = BackColors.DefaultColor;
                bf.Color = ForeColors.DefaultColor;
            }
            e.Graphics.FillRectangle(bb, e.Bounds);
            e.Graphics.DrawString(text, Font, bf, e.Bounds);
        }

    }
}
