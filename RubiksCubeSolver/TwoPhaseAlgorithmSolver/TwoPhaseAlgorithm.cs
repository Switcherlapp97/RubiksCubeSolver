using RubiksCubeLib.Solver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TwoPhaseAlgorithmSolver
{
    public partial class TwoPhaseAlgorithm
    {
      // phase 1 coordinates
      public const short N_TWIST = 2187; // 3^7 possible corner orientations
      public const short N_FLIP = 2048; // 2^11 possible edge flips
      public const short N_SLICE1 = 495; // 12 choose 4 possible positions of FR, FL, BL, BR edges

      // phase 2 coordinates
      public const short N_SLICE2 = 24; // 4! permutations of FR, FL, BL, BR edges
      public const short N_PARITY = 2; // 2 possible corner parities
      public const short N_URFtoDLF = 20160; // 8! / (8 - 6)! permutation of URF, UFL, ULB, UBR, DFR, DLF corners
      public const short N_FRtoBR = 11880; // 12! / (12 - 4)! permutation of FR, FL, BL, BR edges
      public const short N_URtoUL = 1320; // 12! / (12 - 3)! permutation of UR, UF, UL edges
      public const short N_UBtoDF = 1320; // 12! / (12 - 3)! permutation of UB, DR, DF edges
      public const short N_URtoDF = 20160; // 8! / (8 - 6)! permutation of UR, UF, UL, UB, DR, DF edges

      public const int N_URFtoDLB = 40320; // 8! permutations of the corners
      public const int N_URtoBR = 479001600; //12! permutations of the edges

      public const short N_MOVE = 18;
      public const short N_CORNER = 8;
      public const short N_EDGE = 12;

      public TwoPhaseAlgorithm()
      {
        this.InitMoveTables();
        this.InitPruningTables();
      }

      #region solving logic
      private int[] ax = new int[31]; // The axis of the move
      private int[] po = new int[31]; // The power of the move

      private int[] flip = new int[31]; // phase1 coordinates
      private int[] twist = new int[31];
      private int[] slice = new int[31];

      private int[] parity = new int[31]; // phase2 coordinates
      private int[] urfdlf = new int[31];
      private int[] frbr = new int[31];
      private int[] urul = new int[31];
      private int[] ubdf = new int[31];
      private int[] urdf = new int[31];

      private int[] minDistPhase1 = new int[31]; // IDA* distance do goal estimations
      private int[] minDistPhase2 = new int[31];

      public string Solution(CoordCube cube, int maxDepth, long timeOut)
      {
        int s = 0;

        po[0] = 0;
        ax[0] = 0;
        flip[0] = cube.Flip;
        twist[0] = cube.Twist;
        parity[0] = cube.Parity;
        slice[0] = cube.FRtoBR / 24;
        urfdlf[0] = cube.URFtoDLF;
        frbr[0] = cube.FRtoBR;
        urul[0] = cube.URtoUL;
        ubdf[0] = cube.UBtoDF;

        minDistPhase1[1] = 1;
        int mv = 0, n = 0;
        bool busy = false;
        int depthPhase1 = 1;

        long tStart = DateTime.Now.Millisecond;

        do
        {
          do
          {
            if ((depthPhase1 - n > minDistPhase1[n + 1]) && !busy)
            {
              if (ax[n] == 0 || ax[n] == 3) ax[++n] = 1;
              else ax[++n] = 0;
              po[n] = 1;
            }
            else if (++po[n] > 3)
            {
              do
              {
                // increment axis
                if (++ax[n] > 5)
                {
                  if (DateTime.Now.Millisecond - tStart > timeOut << 10) return "Error 8";
                  if (n == 0)
                  {
                    if (depthPhase1 >= maxDepth) return "Error 7";
                    else
                    {
                      depthPhase1++;
                      ax[n] = 0;
                      po[n] = 1;
                      busy = false;
                      break;
                    }
                  }
                  else
                  {
                    n--;
                    busy = true;
                    break;
                  }
                }
                else
                {
                  po[n] = 1;
                  busy = false;
                }
              } while (n != 0 && (ax[n - 1] == ax[n] || ax[n - 1] - 3 == ax[n]));
            }
            else busy = false;
          } while (busy);

          mv = 3 * ax[n] + po[n] - 1;
          flip[n + 1] = flipMove[flip[n], mv];
          twist[n + 1] = twistMove[twist[n], mv];
          slice[n + 1] = FRtoBR_Move[slice[n] * 24, mv] / 24;
          minDistPhase1[n + 1] = Math.Max(GetPruning(sliceFlipPrun, N_SLICE1 * flip[n + 1]
            + slice[n + 1]), GetPruning(sliceTwistPrun, N_SLICE1 * twist[n + 1]
            + slice[n + 1]));

          if (minDistPhase1[n + 1] == 0 && n >= depthPhase1 - 5)
          {
            minDistPhase1[n + 1] = 10;// instead of 10 any value >5 is possible
            if (n == depthPhase1 - 1 && (s = TotalDepth(depthPhase1, maxDepth)) >= 0)
            {
              if (s == depthPhase1
                  || (ax[depthPhase1 - 1] != ax[depthPhase1] && ax[depthPhase1 - 1] != ax[depthPhase1] + 3))
                return string.Join(",", ax);
            }
          }
        } while (true);
      }

      private int TotalDepth(int depthPhase1, int maxDepth)
      {
        int mv = 0, d1 = 0, d2 = 0;
        int maxDepthPhase2 = Math.Min(10, maxDepth - depthPhase1);
        for (int i = 0; i < depthPhase1; i++)
        {
          mv = 3 * ax[i] + po[i] - 1;
          urfdlf[i + 1] = URFtoDLF_Move[urfdlf[i], mv];
          frbr[i + 1] = FRtoBR_Move[frbr[i], mv];
          parity[i + 1] = parityMove[parity[i], mv];
        }

        if ((d1 = GetPruning(sliceURFtoDLF_Prun,
          (N_SLICE2 * urfdlf[depthPhase1] + frbr[depthPhase1]) * 2 + parity[depthPhase1])) > maxDepthPhase2)
          return -1;

        for (int i = 0; i < depthPhase1; i++)
        {
          mv = 3 * ax[i] + po[i] - 1;
          urul[i + 1] = URtoUL_Move[urul[i], mv];
          ubdf[i + 1] = UBtoDF_Move[ubdf[i], mv];
        }
        urdf[depthPhase1] = mergeURtoULandUBtoDF[urul[depthPhase1], ubdf[depthPhase1]];

        if ((d2 = GetPruning(sliceURtoDF_Prun,
            (N_SLICE2 * urdf[depthPhase1] + frbr[depthPhase1]) * 2 + parity[depthPhase1])) > maxDepthPhase2)
          return -1;

        if ((minDistPhase2[depthPhase1] = Math.Max(d1, d2)) == 0)// already solved
          return depthPhase1;

        // now set up search

        int depthPhase2 = 1;
        int n = depthPhase1;
        bool busy = false;
        po[depthPhase1] = 0;
        ax[depthPhase1] = 0;
        minDistPhase2[n + 1] = 1;// else failure for depthPhase2=1, n=0
        // +++++++++++++++++++ end initialization +++++++++++++++++++++++++++++++++
        do
        {
          do
          {
            if ((depthPhase1 + depthPhase2 - n > minDistPhase2[n + 1]) && !busy)
            {

              if (ax[n] == 0 || ax[n] == 3)// Initialize next move
              {
                ax[++n] = 1;
                po[n] = 2;
              }
              else
              {
                ax[++n] = 0;
                po[n] = 1;
              }
            }
            else if ((ax[n] == 0 || ax[n] == 3) ? (++po[n] > 3) : ((po[n] = po[n] + 2) > 3))
            {
              do
              {// increment axis
                if (++ax[n] > 5)
                {
                  if (n == depthPhase1)
                  {
                    if (depthPhase2 >= maxDepthPhase2)
                      return -1;
                    else
                    {
                      depthPhase2++;
                      ax[n] = 0;
                      po[n] = 1;
                      busy = false;
                      break;
                    }
                  }
                  else
                  {
                    n--;
                    busy = true;
                    break;
                  }

                }
                else
                {
                  if (ax[n] == 0 || ax[n] == 3)
                    po[n] = 1;
                  else
                    po[n] = 2;
                  busy = false;
                }
              } while (n != depthPhase1 && (ax[n - 1] == ax[n] || ax[n - 1] - 3 == ax[n]));
            }
            else
              busy = false;
          } while (busy);
          // +++++++++++++ compute new coordinates and new minDist ++++++++++
          mv = 3 * ax[n] + po[n] - 1;

          urfdlf[n + 1] = URFtoDLF_Move[urfdlf[n], mv];
          frbr[n + 1] = FRtoBR_Move[frbr[n], mv];
          parity[n + 1] = parityMove[parity[n], mv];
          if (urdf[n] < 0 || urdf[n] >= 2016)
          urdf[n + 1] = URtoDF_Move[urdf[n], mv];
          if (urdf[n + 1] < 0 || urdf[n + 1] >= 20160) continue;

          minDistPhase2[n + 1] = Math.Max(GetPruning(sliceURtoDF_Prun, (N_SLICE2
              * urdf[n + 1] + frbr[n + 1])
              * 2 + parity[n + 1]), GetPruning(sliceURFtoDLF_Prun, (N_SLICE2
              * urfdlf[n + 1] + frbr[n + 1])
              * 2 + parity[n + 1]));
          // ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        } while (minDistPhase2[n + 1] != 0);
        return depthPhase1 + depthPhase2;
      }
      #endregion
    }
}