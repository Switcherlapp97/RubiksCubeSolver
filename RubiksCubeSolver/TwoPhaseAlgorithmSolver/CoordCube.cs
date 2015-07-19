using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TwoPhaseAlgorithmSolver
{
  public class CoordCube
  {
    public const short N_CORNER = 8;
    public const short N_EDGE = 12;
    public static byte[] Empty8x { get { return new byte[N_CORNER]; } }
    public static byte[] Empty12x { get { return new byte[N_EDGE]; } }

    public CoordCube()
      : this(FromInversions(Empty8x), FromInversions(Empty12x), Empty8x, Empty12x)
    {

    }

    public CoordCube(byte[] cp, byte[] ep, byte[] co, byte[] eo)
    {
      this.ep = ep;
      this.eo = eo;
      this.cp = cp;
      this.co = co;
    }

    #region Main data and coordinates
    // Main data
    private byte[] co;
    private byte[] eo;
    private byte[] cp;
    private byte[] ep;

    public short Twist
    {
      get { return GetTwist(); }
      set { SetTwist(value); }
    }

    public short Flip
    {
      get { return GetFlip(); }
      set { SetFlip(value); }
    }

    public short FRtoBR
    {
      get { return GetFRtoBR(); }
      set { SetFRtoBR(value); }
    }

    public short URFtoDLF
    {
      get { return GetURFtoDLF(); }
      set { SetURFtoDLF(value); }
    }

    public short Parity
    {
      get { return GetCornerParity(); }
    }

    public short URtoUL
    {
      get { return GetURtoUL(); }
      set { SetURtoUL(value); }
    }

    public short UBtoDF
    {
      get { return GetUBtoDF(); }
      set { SetUBtoDF(value); }
    }

    public int URtoDF
    {
      get { return GetURtoDF(); }
      set { SetURtoDF(value); }
    }

    public int URFtoDRB
    {
      get { return GetURFtoDLB(); }
      set { SetURFtoDLB(value); }
    }

    public int URtoBR
    {
      get { return GetURtoBR(); }
      set { SetURtoBR(value); }
    }

    public byte[] CornerPermutation { get { return cp; } set { cp = value; } }
    public byte[] EdgePermutation { get { return ep; } set { ep = value; } }
    #endregion

    public CoordCube DeepClone()
    {
      CoordCube newCubie = new CoordCube();
      newCubie.co = this.co;
      newCubie.cp = this.cp;
      newCubie.eo = this.eo;
      newCubie.ep = this.ep;
      return newCubie;
    }

    #region Coordinate conversions
    private short GetTwist()
    {
      short res = 0;
      for (int i = 0; i < N_CORNER; i++)
        res += (short)(co[i] * Math.Pow(3, N_CORNER - (i + 2)));
      return res;
    }
    private void SetTwist(short twist)
    {
      int sum = 0;
      for (int i = 0; i < N_CORNER - 1; i++)
      {
        int divisor = (int)Math.Pow(3, N_CORNER - (i + 2));
        co[i] = (byte)(twist / divisor);
        sum += twist / divisor;
        twist = (short)(twist % divisor);
      }
      co[N_CORNER - 1] = (byte)((3 - sum % 3) % 3);
    }
    private short GetFlip()
    {
      short res = 0;
      for (int i = 0; i < N_EDGE; i++)
        res += (short)(eo[i] * Math.Pow(2, N_EDGE - (i + 2)));
      return res;
    }
    private void SetFlip(short flip)
    {
      int sum = 0;
      for (int i = 0; i < N_EDGE - 1; i++)
      {
        int divisor = (int)Math.Pow(2, N_EDGE - (i + 2));
        eo[i] = (byte)(flip / divisor);
        sum += flip / divisor;
        flip = (short)(flip % divisor);
      }
      eo[N_EDGE - 1] = (byte)((2 - sum % 2) % 2);
    }
    private short GetCornerParity()
    {
      int s = 0;
      for (int i = 7; i > 0; i--)
        for (int j = i - 1; j >= 0; j--)
          if (cp[j] > cp[i])
            s++;
      return (short)(s % 2);
    }
    private void RotateLeft(byte[] arr, int l, int r)
    {
      byte temp = arr[l];
      for (int i = l; i < r; i++)
        arr[i] = arr[i + 1];
      arr[r] = temp;
    }
    private void RotateRight(byte[] arr, int l, int r)
    {
      byte temp = arr[r];
      for (int i = r; i > l; i--)
        arr[i] = arr[i - 1];
      arr[l] = temp;
    }
    private short GetFRtoBR()
    {
      int a = 0, x = 0;
      byte[] edge4 = new byte[4];
      for (int j = 11; j >= 0; j--)
        if (ep[j] >= 9)
        {
          a += Utils.BinomialCoefficient(11 - j, x + 1);
          edge4[3 - x++] = ep[j];
        }

      int b = 0;
      for (int j = 3; j > 0; j--)// compute the index b < 4! for the
      // permutation in perm
      {
        int k = 0;
        while (edge4[j] - 1 != j + 8)
        {
          RotateLeft(edge4, 0, j);
          k++;
        }
        b = (j + 1) * b + k;
      }
      return (short)(24 * a + b);
    }
    private void SetFRtoBR(short idx)
    {
      int x = 0;
      byte[] edge4 = { 9, 10, 11, 12 };
      byte[] otherEdges = { 1, 2, 3, 4, 5, 6, 7, 8 };
      int b = idx % 24;
      int a = idx / 24;
      ep = new byte[N_EDGE] { 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8 };

      for (int i = 1, k = 0; i < 4; i++)
      {
        k = b % (i + 1);
        b /= i + 1;
        while (k-- > 0)
          RotateRight(edge4, 0, i);
      }
      x = 3;
      for (int i = 0; i < 12; i++)
      {
        if (a - Utils.BinomialCoefficient(11 - i, x + 1) >= 0)
        {
          ep[i] = edge4[3 - x];
          a -= Utils.BinomialCoefficient(11 - i, x-- + 1);
        }
      }
      x = 0;
      for (int j = 0; j < 12; j++)
      {
        if (ep[j] == 8)
          ep[j] = otherEdges[x++];
      }
    }
    private short GetURFtoDLF()
    {
      int a = 0, x = 0;
      byte[] corner6 = new byte[6];
      for (int i = 0; i < N_CORNER; i++)
      {
        if (cp[i] <= 6)
        {
          a += Utils.BinomialCoefficient(i, x + 1);
          corner6[x++] = cp[i];
        }
      }

      int b = 0;
      for (int j = 5; j > 0; j--)
      {
        int k = 0;
        while ((corner6[j] - 1) != j)
        {
          RotateLeft(corner6, 0, j);
          k++;
        }
        b = (j + 1) * b + k;
      }
      return (short)(720 * a + b);
    }
    private void SetURFtoDLF(short idx)
    {
      int x = 0;
      byte[] corner6 = { 1, 2, 3, 4, 5, 6 };
      byte[] otherCorner = { 7, 8 };
      int b = idx % 720;
      int a = idx / 720;
      cp = new byte[N_CORNER] { 8, 8, 8, 8, 8, 8, 8, 8 };

      for (int i = 1, k = 0; i < 6; i++)
      {
        k = b % (i + 1);
        b /= i + 1;
        while (k-- > 0)
          RotateRight(corner6, 0, i);
      }
      x = 5;
      for (int i = 7; i >= 0; i--)
      {
        if (a - Utils.BinomialCoefficient(i, x + 1) >= 0)
        {
          cp[i] = corner6[x];
          a -= Utils.BinomialCoefficient(i, x-- + 1);
        }
      }
      x = 0;
      for (int j = 0; j < 8; j++)
      {
        if (cp[j] == 8)
          cp[j] = otherCorner[x++];
      }
    }
    private int GetURtoDF()
    {
      int a = 0, x = 0;
      byte[] edge6 = new byte[6];
      for (int i = 0; i < 12; i++)
      {
        if (ep[i] <= 6)
        {
          a += Utils.BinomialCoefficient(i, x + 1);
          edge6[x++] = ep[i];
        }
      }

      int b = 0;
      for (int i = 5; i > 0; i--)
      {
        int k = 0;
        while (edge6[i] - 1 != i)
        {
          RotateLeft(edge6, 0, i);
          k++;
        }
        b = (i + 1) * b + k;
      }
      return (720 * a + b);
    }
    public static int GetURtoDF(short idx1, short idx2)
    {
      CoordCube a = new CoordCube();
      CoordCube b = new CoordCube();
      a.URtoUL = idx1;
      b.UBtoDF = idx2;
      for (int i = 0; i < 8; i++)
        if (a.ep[i] != 12)
        {
          if (b.ep[i] != 12)
            return -1;
          else b.ep[i] = a.ep[i];
        }
      return b.URtoDF;
    }
    private void SetURtoDF(int idx)
    {
      byte[] edge6 = { 1, 2, 3, 4, 5, 6 };
      byte[] otherEdges = { 7, 8, 9, 10, 11, 12 };
      int b = idx % 720;
      int a = idx / 720;
      ep = new byte[N_EDGE] { 12, 12, 12, 12, 12, 12, 12, 12, 12, 12, 12, 12 };

      for (int i = 1, k = 0; i < 6; i++)
      {
        k = b % (i + 1);
        b /= i + 1;
        while (k-- > 0)
          RotateRight(edge6, 0, i);
      }
      int x = 5;
      for (int i = 11; i >= 0; i--)
      {
        if (a - Utils.BinomialCoefficient(i, x + 1) >= 0)
        {
          ep[i] = edge6[x];
          a -= Utils.BinomialCoefficient(i, x-- + 1);
        }
      }
      x = 0;
      for (int i = 0; i < 12; i++)
      {
        if (ep[i] == 12)
          ep[i] = otherEdges[x++];
      }
    }
    private short GetURtoUL()
    {
      int a = 0, x = 0;
      byte[] edge3 = new byte[3];
      for (int i = 0; i < 12; i++)
      {
        if (ep[i] <= 3)
        {
          a += Utils.BinomialCoefficient(i, x + 1);
          edge3[x++] = ep[i];
        }
      }

      int b = 0;
      for (int i = 2; i > 0; i--)
      {
        int k = 0;
        while (edge3[i] - 1 != i)
        {
          RotateLeft(edge3, 0, i);
          k++;
        }
        b = (i + 1) * b + k;
      }
      return (short)(6 * a + b);
    }
    private void SetURtoUL(short idx)
    {
      byte[] edge3 = { 1, 2, 3 };
      int b = idx % 6;
      int a = idx / 6;
      ep = new byte[] { 12, 12, 12, 12, 12, 12, 12, 12, 12, 12, 12, 12 };

      for (int i = 1, k = 0; i < 3; i++)
      {
        k = b % (i + 1);
        b /= i + 1;
        while (k-- > 0)
          RotateRight(edge3, 0, i);
      }
      int x = 2;
      for (int i = 11; i >= 0; i--)
      {
        if (a - Utils.BinomialCoefficient(i, x + 1) >= 0)
        {
          ep[i] = edge3[x];
          a -= Utils.BinomialCoefficient(i, x-- + 1);
        }
      }
    }
    private short GetUBtoDF()
    {
      int a = 0, x = 0;
      byte[] edge3 = new byte[3];
      for (int i = 0; i < 12; i++)
      {
        if (ep[i] >= 4 && ep[i] <= 6)
        {
          a += Utils.BinomialCoefficient(i, x + 1);
          edge3[x++] = ep[i];
        }
      }

      int b = 0;
      for (int i = 2; i > 0; i--)
      {
        int k = 0;
        while (edge3[i] - 1 != i + 3)
        {
          RotateLeft(edge3, 0, i);
          k++;
        }
        b = (i + 1) * b + k;
      }
      return (short)(6 * a + b);
    }
    private void SetUBtoDF(short idx)
    {
      byte[] edge3 = { 4, 5, 6 };
      int b = idx % 6;
      int a = idx / 6;
      ep = new byte[] { 12, 12, 12, 12, 12, 12, 12, 12, 12, 12, 12, 12 };

      for (int i = 1, k; i < 3; i++)
      {
        k = b % (i + 1);
        b /= i + 1;
        while (k-- > 0)
          RotateRight(edge3, 0, i);
      }
      int x = 2;
      for (int i = 11; i >= 0; i--)
      {
        if (a - Utils.BinomialCoefficient(i, x + 1) >= 0)
        {
          ep[i] = edge3[x];
          a -= Utils.BinomialCoefficient(i, x-- + 1);
        }
      }
    }
    private int GetURFtoDLB()
    {
      byte[] perm = new byte[N_CORNER];
      int b = 0;
      for (int i = 0; i < N_CORNER; i++)
        perm[i] = cp[i];
      for (int i = 7; i > 0; i--)
      {
        int k = 0;
        while (perm[i] - 1 != i)
        {
          RotateLeft(perm, 0, i);
          k++;
        }
        b = (i + 1) * b + k;
      }
      return b;
    }
    private void SetURFtoDLB(int idx)
    {
      byte[] perm = { 1, 2, 3, 4, 5, 6, 7, 8 };
      int k = 0;
      for (int i = 1; i < 8; i++)
      {
        k = idx % (i + 1);
        idx /= i + 1;
        while (k-- > 0)
          RotateRight(perm, 0, i);
      }

      int x = 7;
      for (int i = 7; i >= 0; i--)
      {
        cp[i] = perm[x--];
      }

    }
    private int GetURtoBR()
    {
      byte[] perm = new byte[N_EDGE];
      int b = 0;
      for (int i = 0; i < N_EDGE; i++)
        perm[i] = ep[i];
      for (int i = 11; i > 0; i--)
      {
        int k = 0;
        while (perm[i] - 1 != i)
        {
          RotateLeft(perm, 0, i);
          k++;
        }
        b = (i + 1) * b + k;
      }
      return b;
    }
    private void SetURtoBR(int idx)
    {
      byte[] perm = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };
      int k = 0;
      for (int i = 1; i < 12; i++)
      {
        k = idx % (i + 1);
        idx /= i + 1;
        while (k-- > 0)
          RotateRight(perm, 0, i);
      }
      int x = 11;
      for (int i = 11; i >= 0; i--)
        ep[i] = perm[x--];
    }
    #endregion

    #region Permutation inversion conversion: Position <-> Inversions
    public static byte[] FromInversions(byte[] perm)
    {
      byte[] cubies = new byte[perm.Length];
      byte upperBound = (byte)perm.Length;
      byte lowerBound = 1;

      bool cancelled = false;
      bool not = false;
      for (int index = perm.Length - 1; index >= 0; index--)
      {
        if (!not)
        {
          if (index == perm[index])
          {
            cubies[index] = lowerBound;
            lowerBound++;
            not = true;
          }
        }

        if (!cancelled)
        {
          if (perm[index] == 0)
          {
            cubies[index] = upperBound;
            upperBound--;
            cancelled = true;
          }
        }
      }

      while (upperBound >= lowerBound)
      {
        for (int i = perm.Length - 1; i >= 0; i--)
        {
          if (cubies[i] == 0)
          {
            int count = 0;
            for (int j = 0; j < i; j++)
            {
              if (cubies[j] > upperBound) count++;
            }
            if (count == perm[i])
            {
              cubies[i] = upperBound;
              upperBound--;
              break;
            }

          }
        }
      }
      return cubies;
    }

    public static byte[] ToInversions(byte[] perm)
    {
      byte[] inversions = new byte[perm.Length];
      for (int i = 0; i < perm.Length; i++)
      {
        int count = 0;
        for (int j = 0; j < i; j++)
        {
          if (perm[j] > perm[i]) count++;
        }
        inversions[i] = (byte)count;
      }
      return inversions;
    }
    #endregion

    public void Multiply(CoordCube b)
    {
      Tuple<byte[], byte[]> edgeMult = Multiply(ep, eo, b.ep, b.eo);
      Tuple<byte[], byte[]> cornerMult = Multiply(cp, co, b.cp, b.co);
      ep = edgeMult.Item1;
      eo = edgeMult.Item2;
      cp = cornerMult.Item1;
      co = cornerMult.Item2;
    }

    private Tuple<byte[], byte[]> Multiply(byte[] permA, byte[] orieA, byte[] permB, byte[] orieB)
    {
      byte[] AxB = new byte[permA.Length];
      byte[] AxBo = new byte[permA.Length];
      int mod = permA.Length == 8 ? 3 : 2;

      for (int i = 0; i < permA.Length; i++)
      {
        // (A*B)(x).c=A(B(x).c).c
        AxB[i] = permA[permB[i] - 1];

        // (A*B)(x).o=A(B(x).c).o+B(x).o
        AxBo[i] = (byte)((orieA[permB[i] - 1] + orieB[i]) % mod);
      }
      return new Tuple<byte[], byte[]>(AxB, AxBo);
    }
  }
}
