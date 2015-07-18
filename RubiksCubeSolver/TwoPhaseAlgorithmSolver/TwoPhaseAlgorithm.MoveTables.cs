using System;
using System.Collections.Generic;
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

      for (int i = 0; i < N_MOVE ; i+=3)
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
    }

    private void InitFlipMoveTable()
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
    }

    private void InitParityMove()
    {
      parityMove = new short[,] { { 1, 0, 1, 1, 0, 1, 1, 0, 1, 1, 0, 1, 1, 0, 1, 1, 0, 1 }, { 0, 1, 0, 0, 1, 0, 0, 1, 0, 0, 1, 0, 0, 1, 0, 0, 1, 0 } };
    }

    private void InitFRtoBR_MoveTable()
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
    }

    private void InitURFtoDLF_MoveTable()
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
    }

    private void InitURtoUL_MoveTable()
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
    }

    private void InitUBtoDF_MoveTable()
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
    }

    private void InitURtoDF_MoveTable()
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
      int max = URtoDF_Move.Cast<short>().Max();
    }

    private void InitMergeURtoULandUBtoDF()
    {
      for (short uRtoUL = 0; uRtoUL < 336; uRtoUL++)
        for (short uBtoDF = 0; uBtoDF < 336; uBtoDF++)
          mergeURtoULandUBtoDF[uRtoUL, uBtoDF] = (short)CoordCube.GetURtoDF(uRtoUL, uBtoDF);
    }
  }
}
