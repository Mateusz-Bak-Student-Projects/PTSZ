using System.Globalization;
using System.Threading;

namespace Persistence
{
    public class PersistenceProperties
    {
        public string InstanceDirectory { get; set; }
        public string SolutionDirectory { get; set; }

        public static void SetCultureInfo()
        {
            var baseCultureInfo = Thread.CurrentThread.CurrentCulture.LCID;
            var culture = new CultureInfo(baseCultureInfo);
            if (culture.NumberFormat.NumberDecimalSeparator != ".")
            {
                culture.NumberFormat.NumberDecimalSeparator = ".";
                Thread.CurrentThread.CurrentCulture = culture;
            }
        }
    }
}
