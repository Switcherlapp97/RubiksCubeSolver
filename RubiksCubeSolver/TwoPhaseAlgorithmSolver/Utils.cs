using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TwoPhaseAlgorithmSolver
{
  public static class Utils
  {
    public static int Factorial(int number)
    {
      int faculty = 1;
      for (int i = 1; i <= number; i++) faculty *= i;
      return faculty;
    }

    public static int BinomialCoefficient(int n, int k)
    {
      if (n == 0 && (n - k) == -1) return 0;
      return Utils.Factorial(n) / (Utils.Factorial(k) * Utils.Factorial(n - k));
    }

    public static int Decrement(int number, int start, int end)
    {
      number--;
      while (start <= number && end >= number)
      {
        number--;
      }
      return number;
    }
  }
}
