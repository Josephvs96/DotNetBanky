using System.ComponentModel;

namespace DotNetBanky.Core.Extentions
{
    public static class EnumExtenstions
    {
        public static string GetValueAsString(this Enum enumObject)
        {
            var field = enumObject.GetType().GetField(enumObject.ToString());
            var customAttributes = field.GetCustomAttributes(typeof(DescriptionAttribute), false);
            if (customAttributes.Length > 0)
                return (customAttributes[0] as DescriptionAttribute).Description;
            else
                return enumObject.ToString();
        }
    }
}
