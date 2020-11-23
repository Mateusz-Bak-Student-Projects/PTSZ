namespace InstanceGenerator
{
    public struct Linear
    {
        public double A { get; set; }
        public double B { get; set; }

        public double Value(double x) => A * x + B;

        public Linear(double a, double b)
        {
            A = a;
            B = b;
        }

        public static explicit operator Linear(double b) => new Linear(0.0, b);

        public override string ToString()
        {
            var result = "";

            if (A != 0.0)
            {
                result += $"{A}x";
            }
            else
            {
                return $"{B}";
            }

            if (B > 0.0)
            {
                result += $"+{B}";
            }
            else if (B < 0.0)
            {
                result += $"{B}";
            }

            return result;
        }
    }
}
