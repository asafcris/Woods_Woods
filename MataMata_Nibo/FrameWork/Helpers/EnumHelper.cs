using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Helpers
{
    public class EnumHelper
    {
        public static IList<KeyValuePair<string, string>> GetAll(Type enumType)
        {

            string[] names = Enum.GetNames(enumType);

            var values = Enum.GetValues(enumType);

            var lista = new List<KeyValuePair<string, string>>();

            for (int i = 0; i < names.Length; i++)
            {
                var e = (Enum)Enum.Parse(enumType, values.GetValue(i).ToString());

                lista.Add(new KeyValuePair<string, string>(((int)values.GetValue(i)).ToString(CultureInfo.InvariantCulture), GetDescription(e)));
            }

            return lista;

        }

        public static string GetDescription(Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());

            var attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

            if (attributes.Length > 0)
                return attributes[0].Description;
            else
                return value.ToString();
        }
    }
}
