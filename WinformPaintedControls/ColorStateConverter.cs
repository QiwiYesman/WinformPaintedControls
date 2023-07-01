using System.ComponentModel;
namespace PaintedControls
{
    public class ColorContainerConverter : TypeConverter
    {
       public override object ConvertTo(ITypeDescriptorContext context,
            System.Globalization.CultureInfo culture, object value, Type destinationType)
        {//This method is used to shown information in the PropertyGrid.
            if (destinationType == typeof(string))
            {
                return "Colors changed with states";
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }
        public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes)
        {
            return TypeDescriptor.GetProperties(typeof(ColorPressContainer),
                attributes);
        }

        public override bool GetPropertiesSupported(ITypeDescriptorContext context) => true;

    }

    public class ColorCheckedContainerConverter : TypeConverter
    {
        public override object ConvertTo(ITypeDescriptorContext context,
             System.Globalization.CultureInfo culture, object value, Type destinationType)
        {//This method is used to shown information in the PropertyGrid.
            if (destinationType == typeof(string))
            {
                return "Colors changed with states";
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }
        public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes)
        {
            return TypeDescriptor.GetProperties(typeof(ColorCheckContainer),
                attributes);
        }

        public override bool GetPropertiesSupported(ITypeDescriptorContext context) => true;
    }
}
