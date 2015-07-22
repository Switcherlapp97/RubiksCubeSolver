using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace TwoPhaseAlgorithmSolver
{
  public partial class TwoPhaseAlgorithm
  {
    private void InitPruningTables()
    {
      InitSliceFlipPruningTable();
      InitSliceTwistPruningTable();
      InitSliceURFtoDLF_PruningTable();
      InitSliceURtoDF_PruningTable();
    }

    // All pruning tables
    private byte[] sliceFlipPrun = new byte[N_SLICE1 * N_FLIP / 2];
    private byte[] sliceTwistPrun = new byte[N_SLICE1 * N_TWIST / 2 + 1];
    private byte[] sliceURFtoDLF_Prun = new byte[N_SLICE2 * N_URFtoDLF * N_PARITY / 2];
    private byte[] sliceURtoDF_Prun = new byte[N_SLICE2 * N_URtoDF * N_PARITY / 2];

    private void InitSliceTwistPruningTable()
    {
      if (!LoadPruningTableSuccessful(Path.Combine(this.TablePath,"slice_twist_prun.file"), N_SLICE1 * N_TWIST / 2 + 1, out sliceTwistPrun))
      {
        for (int i = 0; i < N_SLICE1 * N_TWIST / 2 + 1; i++)
          sliceTwistPrun[i] = 0xFF; // = -1 for signed byte
        int depth = 0;
        SetPruning(sliceTwistPrun, 0, 0);
        int done = 1;
        while (done != N_SLICE1 * N_TWIST)
        {
          for (int i = 0; i < N_SLICE1 * N_TWIST; i++)
          {
            int twist = i / N_SLICE1;
            int slice = i % N_SLICE1;
            if (GetPruning(sliceTwistPrun, i) == depth)
            {
              for (int j = 0; j < N_MOVE; j++)
              {
                int newSlice = FRtoBR_Move[slice * 24, j] / 24;
                int newTwist = twistMove[twist, j];
                if (GetPruning(sliceTwistPrun, N_SLICE1 * newTwist + newSlice) == 0x0F)
                {
                  SetPruning(sliceTwistPrun, N_SLICE1 * newTwist + newSlice, (byte)(depth + 1));
                  done++;
                }
              }
            }
          }
          depth++;
        }
        SavePruningTable(Path.Combine(this.TablePath,"slice_twist_prun.file"), sliceTwistPrun);
      }
    }

    private void InitSliceFlipPruningTable()
    {
      if (!LoadPruningTableSuccessful(Path.Combine(this.TablePath,"slice_flip_prun.file"), N_SLICE1 * N_FLIP / 2, out sliceFlipPrun))
      {
        for (int i = 0; i < N_SLICE1 * N_FLIP / 2; i++)
          sliceFlipPrun[i] = 0xFF; // = -1 for signed byte
        int depth = 0;
        SetPruning(sliceFlipPrun, 0, 0);
        int done = 1;
        while (done != N_SLICE1 * N_FLIP)
        {
          for (int i = 0; i < N_SLICE1 * N_FLIP; i++)
          {
            int flip = i / N_SLICE1;
            int slice = i % N_SLICE1;
            if (GetPruning(sliceFlipPrun, i) == depth)
            {
              for (int j = 0; j < N_MOVE; j++)
              {
                int newSlice = FRtoBR_Move[slice * 24, j] / 24;
                int newFlip = flipMove[flip, j];
                if (GetPruning(sliceFlipPrun, N_SLICE1 * newFlip + newSlice) == 0x0F)
                {
                  SetPruning(sliceFlipPrun, N_SLICE1 * newFlip + newSlice, (byte)(depth + 1));
                  done++;
                }
              }
            }
          }
          depth++;
        }
        SavePruningTable(Path.Combine(this.TablePath,"slice_flip_prun.file"), sliceFlipPrun);
      }
    }

    private void InitSliceURFtoDLF_PruningTable()
    {
      if (!LoadPruningTableSuccessful(Path.Combine(this.TablePath,"slice_urf_to_dlf_prun.file"), N_SLICE2 * N_URFtoDLF * N_PARITY / 2, out sliceURFtoDLF_Prun))
      {
        for (int i = 0; i < N_SLICE2 * N_URFtoDLF * N_PARITY / 2; i++)
          sliceURFtoDLF_Prun[i] = 0xFF; // -1
        int depth = 0;
        SetPruning(sliceURFtoDLF_Prun, 0, 0);
        int done = 1;
        int[] forbidden = new int[] { 3, 5, 6, 8, 12, 14, 15, 17 };
        while (done < N_SLICE2 * N_URFtoDLF * N_PARITY)
        {
          for (int i = 0; i < N_SLICE2 * N_URFtoDLF * N_PARITY; i++)
          {
            int parity = i % 2;
            int URFtoDLF = (i / 2) / N_SLICE2;
            int slice = (i / 2) % N_SLICE2;
            byte prun = GetPruning(sliceURFtoDLF_Prun, i);
            if (prun == depth)
            {
              for (int j = 0; j < 18; j++)
              {
                if (!forbidden.Contains(j))
                {
                  int newSlice = FRtoBR_Move[slice, j] % 24;
                  int newURFtoDLF = URFtoDLF_Move[URFtoDLF, j];
                  int newParity = parityMove[parity, j];
                  if (GetPruning(sliceURFtoDLF_Prun, (N_SLICE2 * newURFtoDLF + newSlice) * 2 + newParity) == (byte)0x0F)
                  {
                    SetPruning(sliceURFtoDLF_Prun, (N_SLICE2 * newURFtoDLF + newSlice) * 2 + newParity, (byte)(depth + 1));
                    done++;
                  }
                }
              }
            }
          }
          depth++;
        }
        SavePruningTable(Path.Combine(this.TablePath,"slice_urf_to_dlf_prun.file"), sliceURFtoDLF_Prun);
      }
    }

    private void InitSliceURtoDF_PruningTable()
    {
      if (!LoadPruningTableSuccessful(Path.Combine(this.TablePath,"slice_ur_to_df_prun.file"), N_SLICE2 * N_URtoDF * N_PARITY / 2, out sliceURtoDF_Prun))
      {
        for (int i = 0; i < N_SLICE2 * N_URtoDF * N_PARITY / 2; i++)
          sliceURtoDF_Prun[i] = 0xFF; // = -1 for signed byte
        int depth = 0;
        SetPruning(sliceURtoDF_Prun, 0, 0);
        int done = 1;
        while (done != N_SLICE2 * N_URtoDF * N_PARITY)
        {
          int[] forbidden = new int[] { 3, 5, 6, 8, 12, 14, 15, 17 };
          for (int i = 0; i < N_SLICE2 * N_URtoDF * N_PARITY; i++)
          {
            int parity = i % 2;
            int URtoDF = (i / 2) / N_SLICE2;
            int slice = (i / 2) % N_SLICE2;
            if (GetPruning(sliceURtoDF_Prun, i) == depth)
            {
              for (int j = 0; j < 18; j++)
              {
                if (!forbidden.Contains(j))
                {
                  int newSlice = FRtoBR_Move[slice, j] % 24;
                  int newURtoDF = URtoDF_Move[URtoDF, j];
                  int newParity = parityMove[parity, j];
                  if (GetPruning(sliceURtoDF_Prun, (N_SLICE2 * newURtoDF + newSlice) * 2 + newParity) == 0x0F)
                  {
                    SetPruning(sliceURtoDF_Prun, (N_SLICE2 * newURtoDF + newSlice) * 2 + newParity, (byte)(depth + 1));
                    done++;
                  }
                }
              }
            }
          }
          depth++;
        }
        SavePruningTable(Path.Combine(this.TablePath,"slice_ur_to_df_prun.file"), sliceURtoDF_Prun);
      }
    }

    private void SetPruning(byte[] table, int index, byte value)
    {
      if ((index & 1) == 0)
        table[index / 2] &= (byte)((int)0xF0 | (int)value);
      else
        table[index / 2] &= (byte)(0x0F | (value << 4));
    }

    private byte GetPruning(byte[] table, int index)
    {
      if ((index & 1) == 0)
        return (byte)((int)table[index / 2] & (int)0x0F);
      else
        return (byte)(((int)table[index / 2] & (int)0xF0) >> 4);
    }

    private byte[] LoadPruningTable(string filename, int length)
    {
      byte[] newBytes = File.ReadAllBytes(filename);
      if (newBytes.Length != length)
        throw new Exception("Invalid input file");
      return newBytes;
    }

    private void SavePruningTable(string filename, byte[] table)
    {
      File.WriteAllBytes(filename, table);
    }

    private bool LoadPruningTableSuccessful(string filename, int length, out byte[] newTable)
    {
      newTable = new byte[length];
      try
      {
        newTable = LoadPruningTable(filename, length);
        return true;
      }
      catch
      {
        return false;
      }
    }
  }
}
