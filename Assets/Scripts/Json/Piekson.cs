public class Piekson
{   
    public static string ToJson(object obj)
    {
        System.Globalization.CultureInfo customCulture =
            (System.Globalization.CultureInfo) System.Threading.Thread.CurrentThread.CurrentCulture.Clone();
        customCulture.NumberFormat.NumberDecimalSeparator = ".";

        System.Threading.Thread.CurrentThread.CurrentCulture = customCulture;
        return PiekaJsonSerializer.ToJson(obj);
    }

    public static T FromJson<T>(string json)
    {
        System.Globalization.CultureInfo customCulture =
            (System.Globalization.CultureInfo) System.Threading.Thread.CurrentThread.CurrentCulture.Clone();
        customCulture.NumberFormat.NumberDecimalSeparator = ".";

        System.Threading.Thread.CurrentThread.CurrentCulture = customCulture;
        return PiekaJsonDeserializer.FromJson<T>(json);
    }
}