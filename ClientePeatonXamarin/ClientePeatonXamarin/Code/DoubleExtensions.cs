using System;

namespace ClientePeatonXamarin.Code
{
    public static class DoubleExtensions
    {

        public static double Clamp(this double self, double min, double max)
        {

            return Math.Min(max, Math.Max(self, min));
        }
    }

    public static class Infrastructure
    {
        public static void Init()
        {

        }
    }

}
