using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolBar;

namespace PaintedControls
{
    public partial class CustomComboBox : UserControl
    {
        CustomTextBox textBox;
        CustomCheckBox checkBox;
        CustomListBox listBox;
        ToolStripDropDown popup;

        public string CurrentText
        {
            get => textBox.Text;
            set => textBox.Text = value;
        }
        public CustomComboBox()
        {
            listBox = new();
            textBox = new()
            {
                Parent = this,
                Location = new(0, 0),
                MinimumSize = new(50, 5)
            };
            checkBox = new()
            {
                Parent = this
            };
            popup = new() { AutoSize =true};
            listBox.List.IntegralHeight = true;
            SetDefaultColors();
            UpdateSizeAndPosition();
            Resize += OnResize;

            InitEvents();
            InitListPopup();
    
        }

        

        void SelectItemClosePopup()
        {
            CurrentText = listBox.List.SelectedItem?.ToString()??"";
            popup.Hide();
        }

        void SwitchPopupOpen()
        {
            if (checkBox.Checked)
            {
                popup.Show(this, textBox.Left, textBox.Bottom);
                return;
            }
            popup.Hide();
        } 
        void InitEvents()
        {
            checkBox.Button.MouseClick += (o, e) => SwitchPopupOpen();
            checkBox.Button.MouseDoubleClick += (o, e) => SwitchPopupOpen();

            textBox.TextBox.FontChanged += (_, _) =>
            {
                float size = textBox.FontSize / 2;
                if (size <= 7) size = textBox.FontSize;
                listBox.Font = new(textBox.FontName, size, GraphicsUnit.Pixel);
            };
            listBox.List.SelectedIndexChanged += (o, e) => SelectItemClosePopup();
        }
        void InitListPopup()
        {
            popup = new()
            {
                AutoSize = false,
                Size = listBox.Size,
                DropShadowEnabled = false,
                Margin = Padding.Empty,
                Padding = Padding.Empty
            };
            var host = new ToolStripControlHost(listBox)
            {
                AutoSize = false,
                Size =listBox.Size,
                Padding = Padding.Empty,
                Margin = Padding.Empty
            };
            popup.Items.Add(host);
            popup.Closed += OnPopupClosed;
        }

        private void OnPopupClosed(object? sender, ToolStripDropDownClosedEventArgs e)
        {
            if(!checkBox.ContainsFocus)
            {
                checkBox.Checked = false;
            }
        }

        void SetDefaultColors()
        {
            textBox.BackColors.HoverColor = Color.Black;
            textBox.BackColors.DefaultColor = Color.Black;
            textBox.BackColors.PressColor = Color.Black;
            textBox.ForeColors.HoverColor = Color.Yellow;
            textBox.ForeColors.DefaultColor = Color.White;
            textBox.ForeColors.PressColor = Color.Pink;

            checkBox.BackColors.HoverColor = Color.DarkBlue;
            checkBox.BackColors.DefaultColor = Color.Black;
            checkBox.BackColors.PressColor = Color.Blue;
            checkBox.BackColors.CheckColor = Color.Cyan;
            checkBox.ForeColors.HoverColor = Color.Yellow;
            checkBox.ForeColors.DefaultColor = Color.White;
            checkBox.ForeColors.PressColor = Color.Pink;
            checkBox.ForeColors.CheckColor = Color.Blue;

            listBox.BackColors.HoverColor = Color.Black;
            listBox.BackColors.DefaultColor = Color.Black;
            listBox.BackColors.PressColor = Color.Red;
            listBox.ForeColors.HoverColor = Color.Yellow;
            listBox.ForeColors.DefaultColor = Color.White;
            listBox.ForeColors.PressColor = Color.DarkBlue;

        }

        public int BorderWidth
        {
            get => textBox.BorderWidth;
            set
            {
                textBox.BorderWidth = value;
                checkBox.BorderWidth = value;
                listBox.BorderWidth = value;
            }
        }

        [Category("Colors"), Description("Back colors"), Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public ColorPressContainer BackTextColors 
        {
            get => textBox.BackColors;
            set=>  textBox.BackColors = value;
            
        }

        [Category("Colors"), Description("Back colors"), Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public ColorCheckContainer BackCheckColors
        {
            get => checkBox.BackColors;
            set => checkBox.BackColors = value;

        }

        [Category("Colors"), Description("Fore colors"), Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public ColorPressContainer ForeTextColors
        {
            get => textBox.ForeColors;
            set => textBox.ForeColors = value;

        }

        [Category("Colors"), Description("Fore colors"), Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public ColorCheckContainer ForeCheckColors
        {
            get => checkBox.ForeColors;
            set => checkBox.ForeColors = value;

        }

        [Category("Colors"), Description("Fore colors"), Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public ColorPressContainer ForeListColors
        {
            get => listBox.ForeColors;
            set => listBox.ForeColors = value;

        }

        [Category("Colors"), Description("Back colors"), Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public ColorPressContainer BackListColors
        {
            get => listBox.BackColors;
            set => listBox.BackColors = value;

        }


        void UpdateSizeAndPosition()
        {
            textBox.Height = Height;
            checkBox.Height = Height;
            checkBox.Width = Height;
            textBox.Width = Width - Height;
            checkBox.Location = new(textBox.Width, 0);

            listBox.Width = Width;
            popup.Width = Width;
            listBox.ClientHeight = listBox.List.PreferredHeight;
            popup.Height = listBox.Height;
        }

        private void OnResize(object? sender, EventArgs e)
        {
            UpdateSizeAndPosition();

        }
    }
}
