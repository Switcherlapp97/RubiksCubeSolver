using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace TwoPhaseAlgorithmSolver
{
  public partial class TwoPhaseAlgorithm
  {
    private void InitMoveTables()
    {
      InitMoves();
      InitTwistMoveTable();
      InitFlipMoveTable();
      InitParityMove();
      InitFRtoBR_MoveTable();
      InitURFtoDLF_MoveTable();
      InitURtoUL_MoveTable();
      InitUBtoDF_MoveTable();
      InitURtoDF_MoveTable();
      InitMergeURtoULandUBtoDF();
    }

    private CoordCube[] moves = new CoordCube[N_MOVE];

    private short[,] twistMove = new short[N_TWIST, N_MOVE];
    private short[,] flipMove = new short[N_FLIP, N_MOVE];
    private short[,] parityMove;
    private short[,] FRtoBR_Move = new short[N_FRtoBR, N_MOVE];
    private short[,] URFtoDLF_Move = new short[N_URFtoDLF, N_MOVE];
    private short[,] URtoUL_Move = new short[N_URtoUL, N_MOVE];
    private short[,] UBtoDF_Move = new short[N_UBtoDF, N_MOVE];
    private short[,] URtoDF_Move = new short[N_URtoDF, N_MOVE];
    private short[,] mergeURtoULandUBtoDF = new short[336, 336];

    private void InitMoves()
    {
      moves[0] = new CoordCube(CoordCube.FromInversions(new byte[N_CORNER] { 0, 1, 1, 1, 0, 0, 0, 0 }),
        CoordCube.FromInversions(new byte[N_EDGE] { 0, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0 }),
        new byte[N_CORNER] { 0, 0, 0, 0, 0, 0, 0, 0 },
        new byte[N_EDGE] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 });

      moves[3] = new CoordCube(CoordCube.FromInversions(new byte[N_CORNER] { 0, 1, 1, 3, 0, 1, 1, 4 }),
        CoordCube.FromInversions(new byte[N_EDGE] { 0, 1, 1, 1, 0, 2, 2, 2, 5, 1, 1, 11 }),
        new byte[N_CORNER] { 2, 0, 0, 1, 1, 0, 0, 2 },
        new byte[N_EDGE] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 });

      moves[6] = new CoordCube(CoordCube.FromInversions(new byte[N_CORNER] { 0, 0, 1, 1, 4, 1, 0, 0 }),
        CoordCube.FromInversions(new byte[N_EDGE] { 0, 0, 1, 1, 1, 1, 2, 2, 7, 4, 0, 0 }),
        new byte[N_CORNER] { 1, 2, 0, 0, 2, 1, 0, 0 },
        new byte[N_EDGE] { 0, 1, 0, 0, 0, 1, 0, 0, 1, 1, 0, 0 });

      moves[9] = new CoordCube(CoordCube.FromInversions(new byte[N_CORNER] { 0, 0, 0, 0, 0, 0, 0, 3 }),
        CoordCube.FromInversions(new byte[N_EDGE] { 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0 }),
        new byte[N_CORNER] { 0, 0, 0, 0, 0, 0, 0, 0 },
        new byte[N_EDGE] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 });

      moves[12] = new CoordCube(CoordCube.FromInversions(new byte[N_CORNER] { 0, 0, 0, 1, 1, 4, 1, 0 }),
        CoordCube.FromInversions(new byte[N_EDGE] { 0, 0, 0, 1, 1, 1, 1, 2, 2, 7, 4, 0 }),
        new byte[N_CORNER] { 0, 1, 2, 0, 0, 2, 1, 0 },
        new byte[N_EDGE] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 });

      moves[15] = new CoordCube(CoordCube.FromInversions(new byte[N_CORNER] { 0, 0, 0, 0, 1, 1, 4, 1 }),
        CoordCube.FromInversions(new byte[N_EDGE] { 0, 0, 0, 0, 1, 1, 1, 1, 2, 2, 7, 4 }),
        new byte[N_CORNER] { 0, 0, 1, 2, 0, 0, 2, 1 },
        new byte[N_EDGE] { 0, 0, 0, 1, 0, 0, 0, 1, 0, 0, 1, 1 });

      for (int i = 0; i < N_MOVE; i += 3)
      {
        CoordCube move = moves[i].DeepClone();
        for (int j = 1; j < 3; j++)
        {
          move.Multiply(moves[i]);
          moves[i + j] = move.DeepClone();
        }
      }
    }

    private void InitTwistMoveTable()
    {
      if (!this.LoadMoveTableSuccessful(Path.Combine(this.TablePath,"twist_move.file"), N_MOVE, N_TWIST, out twistMove))
      {
        CoordCube a = new CoordCube();
        for (short i = 0; i < N_TWIST; i++)
        {
          for (int j = 0; j < N_MOVE; j++)
          {
            a.Twist = i;
            a.Multiply(moves[j]);
            twistMove[i, j] = a.Twist;
          }
        }
        SaveMoveTable(Path.Combine(this.TablePath,"twist_move.file"), twistMove);
      }
    }

    private void InitFlipMoveTable()
    {
      if (!this.LoadMoveTableSuccessful(Path.Combine(this.TablePath,"flip_move.file"), N_MOVE, N_FLIP, out flipMove))
      {
        CoordCube a = new CoordCube();
        for (short i = 0; i < N_FLIP; i++)
        {
          for (int j = 0; j < N_MOVE; j++)
          {
            a.Flip = i;
            a.Multiply(moves[j]);
            flipMove[i, j] = a.Flip;
          }
        }
        SaveMoveTable(Path.Combine(this.TablePath,"flip_move.file"), twistMove);
      }
    }

    private void InitParityMove()
    {
      parityMove = new short[,] { { 1, 0, 1, 1, 0, 1, 1, 0, 1, 1, 0, 1, 1, 0, 1, 1, 0, 1 }, { 0, 1, 0, 0, 1, 0, 0, 1, 0, 0, 1, 0, 0, 1, 0, 0, 1, 0 } };
    }

    private void InitFRtoBR_MoveTable()
    {
      if (!this.LoadMoveTableSuccessful(Path.Combine(this.TablePath,"fr_to_br_move.file"), N_MOVE, N_FRtoBR, out FRtoBR_Move))
      {
        CoordCube a = new CoordCube();
        for (short i = 0; i < N_FRtoBR; i++)
        {
          for (int j = 0; j < N_MOVE; j++)
          {
            a.FRtoBR = i;
            a.Multiply(moves[j]);
            FRtoBR_Move[i, j] = a.FRtoBR;
          }
        }
        SaveMoveTable(Path.Combine(this.TablePath,"fr_to_br_move.file"), FRtoBR_Move);
      }
    }

    private void InitURFtoDLF_MoveTable()
    {
      if (!this.LoadMoveTableSuccessful(Path.Combine(this.TablePath,"urf_to_dlf_move.file"), N_MOVE, N_URFtoDLF, out URFtoDLF_Move))
      {
        CoordCube a = new CoordCube();
        for (short i = 0; i < N_URFtoDLF; i++)
        {
          for (int j = 0; j < N_MOVE; j++)
          {
            a.URFtoDLF = i;
            a.Multiply(moves[j]);
            URFtoDLF_Move[i, j] = a.URFtoDLF;
          }
        }
        SaveMoveTable(Path.Combine(this.TablePath,"urf_to_dlf_move.file"), URFtoDLF_Move);
      }
    }

    private void InitURtoUL_MoveTable()
    {
      if (!this.LoadMoveTableSuccessful(Path.Combine(this.TablePath,"ur_to_ul_move.file"), N_MOVE, N_URtoUL, out URtoUL_Move))
      {
        CoordCube a = new CoordCube();
        for (short i = 0; i < N_URtoUL; i++)
        {
          for (int j = 0; j < N_MOVE; j++)
          {
            a.URtoUL = i;
            a.Multiply(moves[j]);
            URtoUL_Move[i, j] = a.URtoUL;
          }
        }
        SaveMoveTable(Path.Combine(this.TablePath,"ur_to_ul_move.file"), URtoUL_Move);
      }
    }

    private void InitUBtoDF_MoveTable()
    {
      if (!this.LoadMoveTableSuccessful(Path.Combine(this.TablePath,"ub_to_df_move.file"), N_MOVE, N_UBtoDF, out UBtoDF_Move))
      {
        CoordCube a = new CoordCube();
        for (short i = 0; i < N_UBtoDF; i++)
        {
          for (int j = 0; j < N_MOVE; j++)
          {
            a.UBtoDF = i;
            a.Multiply(moves[j]);
            UBtoDF_Move[i, j] = a.UBtoDF;
          }
        }
        SaveMoveTable(Path.Combine(this.TablePath,"ub_to_df_move.file"), UBtoDF_Move);
      }
    }

    private void InitURtoDF_MoveTable()
    {
      if (!this.LoadMoveTableSuccessful(Path.Combine(this.TablePath,"ur_to_df_move.file"), N_MOVE, N_URtoDF, out URtoDF_Move))
      {
        CoordCube a = new CoordCube();
        for (short i = 0; i < N_URtoDF; i++)
        {
          for (int j = 0; j < N_MOVE; j++)
          {
            a.URtoDF = i;
            a.Multiply(moves[j]);
            URtoDF_Move[i, j] = (short)a.URtoDF;
          }
        }
        SaveMoveTable(Path.Combine(this.TablePath,"ur_to_df_move.file"), URtoDF_Move);
      }
    }

    private void InitMergeURtoULandUBtoDF()
    {
      if (!this.LoadMoveTableSuccessful(Path.Combine(this.TablePath,"merge_move.file"), 336, 336, out mergeURtoULandUBtoDF))
      {
        for (short uRtoUL = 0; uRtoUL < 336; uRtoUL++)
          for (short uBtoDF = 0; uBtoDF < 336; uBtoDF++)
            mergeURtoULandUBtoDF[uRtoUL, uBtoDF] = (short)CoordCube.GetURtoDF(uRtoUL, uBtoDF);
        SaveMoveTable(Path.Combine(this.TablePath,"merge_move.file"), mergeURtoULandUBtoDF);
      }
    }

    #region Save and load move tables from files
    private short[,] LoadMoveTable(string filename, int lengthX, int lengthY)
    {
      if (!File.Exists(filename)) throw new Exception("File does not exist!");
      short[,] newTable = new short[lengthY, lengthX];
      using (StreamReader sr = new StreamReader(filename))
      {
        int rowIndex = 0;
        while (!sr.EndOfStream)
        {
          string line = sr.ReadLine();
          string[] entries = line.Split(';');
          if (entries.Length != lengthX)
            throw new Exception("Invalid input file!");
          for (int columnIndex = 0; columnIndex < lengthX; columnIndex++)
          {
            short entry = 0;
            if (short.TryParse(entries[columnIndex], out entry))
              newTable[rowIndex, columnIndex] = short.Parse(entries[columnIndex]);
            else
              throw new Exception("Invalid input file!");
          }
          rowIndex++;
        }
        if (rowIndex != lengthY)
          throw new Exception("Invalid input file!");
      }

      return newTable;
    }

    private bool LoadMoveTableSuccessful(string filename, int lengthX, int lengthY, out short[,] newTable)
    {
      newTable = new short[lengthY, lengthX];
      try
      {
        newTable = LoadMoveTable(filename, lengthX, lengthY);
        return true;
      }
      catch
      {
        return false;
      }
    }

    private void SaveMoveTable(string filename, short[,] table)
    {
      using (StreamWriter sw = new StreamWriter(filename))
      {
        for (int rowIndex = 0; rowIndex < table.GetLength(0); rowIndex++)
        {
          string line = table[rowIndex, 0].ToString();
          for (int columnIndex = 1; columnIndex < table.GetLength(1); columnIndex++)
            line += string.Format(";{0}", table[rowIndex, columnIndex]);
          sw.WriteLine(line);
        }
      }
    }
    #endregion
  }
}
