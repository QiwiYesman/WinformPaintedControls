using System.ComponentModel;
namespace PaintedControls
{
  
    public class ColorHoverContainer
    {
        
        public event Action<Color>? ColorChanged;
        protected Color[] _colors;
        public Color GetColor(int colorState) => _colors[colorState];

        [Browsable(false)]
        public int CurrentState { get; set; }
        [Browsable(false)]
        public Color CurrentColor { get => _colors[CurrentState]; }

        [
      TypeConverter(typeof(ColorConverter))]
        public Color DefaultColor
        {
            get => _colors[ColorStates.Default];
            set
            {
                _colors[ColorStates.Default] = value;
                OnColorChanged(DefaultColor);
            }
        }
        [
      TypeConverter(typeof(ColorConverter))]
        public Color HoverColor
        {
            get => _colors[ColorStates.Hovered];
            set
            {
                _colors[ColorStates.Hovered] = value;
                OnColorChanged(HoverColor);
            }
        }

        public ColorHoverContainer(int size=2)
        {
            _colors = new Color[size];
            DefaultColor = Color.Black;
            HoverColor = Color.BlueViolet;
        }

        protected void OnColorChanged(Color color)
        {
            ColorChanged?.Invoke(color);
        }
    }
    [TypeConverter(typeof(ColorContainerConverter))]
    public class ColorPressContainer : ColorHoverContainer
    {
        [TypeConverter(typeof(ColorConverter))]
        public Color PressColor
        {
            get => _colors[ColorStates.Pressed];
            set
            {
                _colors[ColorStates.Pressed] = value;
                OnColorChanged(PressColor);
            }
        }
        public ColorPressContainer(int size = 3) : base(size)
        {
            PressColor = Color.Yellow;
        }
    }

    [TypeConverter(typeof(ColorCheckedContainerConverter))]
    public class ColorCheckContainer: ColorPressContainer
    {
        [TypeConverter(typeof(ColorConverter))]
        public Color CheckColor
        {
            get => _colors[ColorStates.Checked];
            set
            {
                _colors[ColorStates.Checked] = value;
                OnColorChanged(CheckColor);
            }
        }

        public ColorCheckContainer(int size = 4) : base(size)
        {
            CheckColor = Color.Cyan;
        }
    }
}
