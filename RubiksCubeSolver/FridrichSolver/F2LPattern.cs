using RubiksCubeLib;
using RubiksCubeLib.RubiksCube;
using RubiksCubeLib.Solver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FridrichSolver
{
  public class F2LPattern : PatternTable
  {
    private Dictionary<Pattern, Algorithm> patterns;

    public override Dictionary<Pattern, Algorithm> Patterns { get { return patterns; } }

    CubeFlag CornerTargetPos { get; set; }
    public CubeFlag EdgeTargetPos { get; set; }

    public F2LPattern(CubeFlag edgeTargetPos, CubeFlag cornerTargetPos, CubeFlag rightSlice, CubeFlag frontSlice)
    {
      this.CornerTargetPos = cornerTargetPos;
      this.EdgeTargetPos = edgeTargetPos;
      InitPatterns(rightSlice, frontSlice);
    }

    private void InitPatterns(CubeFlag r, CubeFlag f)
    {
      CubeFlag l = CubeFlagService.GetOppositeFlag(r);
      CubeFlag b = CubeFlagService.GetOppositeFlag(f);
      bool rIsX = CubeFlagService.IsXFlag(r);

      // edge orientation changes depending on the target slot
      Orientation correct = (r == CubeFlag.BackSlice && f == CubeFlag.RightSlice) || (f == CubeFlag.LeftSlice && r == CubeFlag.FrontSlice)
        ? Orientation.Correct : Orientation.Clockwise;
      Orientation clockwise = correct == Orientation.Correct ? Orientation.Clockwise : Orientation.Correct;

      patterns = new Dictionary<Pattern, Algorithm>()
      {
        #region Corner correct oriented at targetposition 
        {
          new Pattern(new List<PatternItem>()
            {
              new PatternItem(new CubePosition(CubeFlag.TopLayer | f | (rIsX ? CubeFlag.MiddleSliceSides : CubeFlag.MiddleSlice)), correct, EdgeTargetPos),
              new PatternItem(new CubePosition(CornerTargetPos), Orientation.Correct, CornerTargetPos)
            }),
          new Algorithm("U {0} U {0}' U' {1}' U' {1}",CubeFlagService.ToNotationString(r), CubeFlagService.ToNotationString(f))
        },
        {
          new Pattern(new List<PatternItem>()
            {
              new PatternItem(new CubePosition(CubeFlag.TopLayer | r | (!rIsX ? CubeFlag.MiddleSliceSides : CubeFlag.MiddleSlice)), clockwise, EdgeTargetPos),
              new PatternItem(new CubePosition(CornerTargetPos), Orientation.Correct, CornerTargetPos)
            }),
          new Algorithm("U' {1}' U' {1} U {0} U {0}'",CubeFlagService.ToNotationString(r), CubeFlagService.ToNotationString(f))
        },
        {
          new Pattern(new List<PatternItem>()
            {
              new PatternItem(new CubePosition(EdgeTargetPos), Orientation.Clockwise, EdgeTargetPos),
              new PatternItem(new CubePosition(CornerTargetPos), Orientation.Correct, CornerTargetPos)
            }),
          new Algorithm("{0}2 U2 {1} {0}2 {1}' U2 {0}' U {0}'",CubeFlagService.ToNotationString(r), CubeFlagService.ToNotationString(f))
        },
        #endregion

        #region Corner clockwise oriented at target position
        {
          new Pattern(new List<PatternItem>()
            {
              new PatternItem(new CubePosition(CubeFlag.TopLayer | f | (rIsX ? CubeFlag.MiddleSliceSides : CubeFlag.MiddleSlice)), correct, EdgeTargetPos),
              new PatternItem(new CubePosition(CornerTargetPos), Orientation.CounterClockwise, CornerTargetPos)
            }),
          new Algorithm("{0}' U {0} U' {0}' U {0}",CubeFlagService.ToNotationString(f))
        },
        {
          new Pattern(new List<PatternItem>()
            {
              new PatternItem(new CubePosition(CubeFlag.TopLayer | r | (!rIsX ? CubeFlag.MiddleSliceSides : CubeFlag.MiddleSlice)), clockwise, EdgeTargetPos),
              new PatternItem(new CubePosition(CornerTargetPos), Orientation.CounterClockwise, CornerTargetPos)
            }),
          new Algorithm("{0} U {0}' U' {0} U {0}'",CubeFlagService.ToNotationString(r))
        },
        {
          new Pattern(new List<PatternItem>()
            {
              new PatternItem(new CubePosition(EdgeTargetPos), Orientation.Correct, EdgeTargetPos),
              new PatternItem(new CubePosition(CornerTargetPos), Orientation.CounterClockwise, CornerTargetPos)
            }),
          new Algorithm("{0} U2 {0} U {0}' U {0} U2 {0}2",CubeFlagService.ToNotationString(r))
        },
        {
          new Pattern(new List<PatternItem>()
            {
              new PatternItem(new CubePosition(EdgeTargetPos), Orientation.Clockwise, EdgeTargetPos),
              new PatternItem(new CubePosition(CornerTargetPos), Orientation.CounterClockwise, CornerTargetPos)
            }),
          new Algorithm("{0} U {0}' U' {0} U' {0}' U2 {1}' U' {1}",CubeFlagService.ToNotationString(r), CubeFlagService.ToNotationString(f))
        },
        #endregion

        #region Corner counter-clockwise oriented at target position
        {
          new Pattern(new List<PatternItem>()
            {
              new PatternItem(new CubePosition(CubeFlag.TopLayer | f | (rIsX ? CubeFlag.MiddleSliceSides : CubeFlag.MiddleSlice)), correct, EdgeTargetPos),
              new PatternItem(new CubePosition(CornerTargetPos), Orientation.Clockwise, CornerTargetPos)
            }),
          new Algorithm("{0}' U' {0} U {0}' U' {0}",CubeFlagService.ToNotationString(f))
        },
        {
          new Pattern(new List<PatternItem>()
            {
              new PatternItem(new CubePosition(CubeFlag.TopLayer | r | (!rIsX ? CubeFlag.MiddleSliceSides : CubeFlag.MiddleSlice)), clockwise, EdgeTargetPos),
              new PatternItem(new CubePosition(CornerTargetPos), Orientation.Clockwise, CornerTargetPos)
            }),
          new Algorithm("{0} U' {0}' U {0} U' {0}'",CubeFlagService.ToNotationString(r))
        },
        {
          new Pattern(new List<PatternItem>()
            {
              new PatternItem(new CubePosition(EdgeTargetPos), Orientation.Correct, EdgeTargetPos),
              new PatternItem(new CubePosition(CornerTargetPos), Orientation.Clockwise, CornerTargetPos)
            }),
          new Algorithm("{0}' U' {0} U2 {0}' U {0} U' {0}' U' {0}",CubeFlagService.ToNotationString(f))
        },
        {
          new Pattern(new List<PatternItem>()
            {
              new PatternItem(new CubePosition(EdgeTargetPos), Orientation.Clockwise, EdgeTargetPos),
              new PatternItem(new CubePosition(CornerTargetPos), Orientation.Clockwise, CornerTargetPos)
            }),
          new Algorithm("{1}' U' {1} U {1}' U {1} U2 {0} U {0}'",CubeFlagService.ToNotationString(r), CubeFlagService.ToNotationString(f))
        },
        #endregion

        #region Corner correct oriented in top layer
        {
          new Pattern(new List<PatternItem>()
            {
              new PatternItem(new CubePosition(EdgeTargetPos), Orientation.Correct, EdgeTargetPos),
              new PatternItem(new CubePosition(CornerTargetPos &~ CubeFlag.BottomLayer | CubeFlag.TopLayer), Orientation.Correct, CornerTargetPos)
            }),
          new Algorithm("{0} U {0}' U' {0} U {0}' U' {0} U {0}'",CubeFlagService.ToNotationString(r))
        },
        {
          new Pattern(new List<PatternItem>()
            {
              new PatternItem(new CubePosition(EdgeTargetPos), Orientation.Clockwise, EdgeTargetPos),
              new PatternItem(new CubePosition(CornerTargetPos &~ CubeFlag.BottomLayer | CubeFlag.TopLayer), Orientation.Correct, CornerTargetPos)
            }),
          new Algorithm("{0} U' {0}' U {1}' U {1}",CubeFlagService.ToNotationString(r), CubeFlagService.ToNotationString(f))
        },
        {
          new Pattern(new List<PatternItem>()
            {
              new PatternItem(new CubePosition(CubeFlag.TopLayer | (rIsX ? CubeFlag.MiddleSliceSides : CubeFlag.MiddleSlice) | b), clockwise, EdgeTargetPos),
              new PatternItem(new CubePosition(CornerTargetPos &~ CubeFlag.BottomLayer | CubeFlag.TopLayer), Orientation.Correct, CornerTargetPos)
            }),
          new Algorithm("U {0} U2 {0}' U {0} U' {0}'",CubeFlagService.ToNotationString(r))
        },
        {
          new Pattern(new List<PatternItem>()
            {
              new PatternItem(new CubePosition(CubeFlag.TopLayer | (rIsX ? CubeFlag.MiddleSliceSides : CubeFlag.MiddleSlice) | b), correct, EdgeTargetPos),
              new PatternItem(new CubePosition(CornerTargetPos &~ CubeFlag.BottomLayer | CubeFlag.TopLayer), Orientation.Correct, CornerTargetPos)
            }),
          new Algorithm("{0}' U {0} U2 {0}' U' {0}",CubeFlagService.ToNotationString(f))
        },
        {
          new Pattern(new List<PatternItem>()
            {
              new PatternItem(new CubePosition(CubeFlag.TopLayer | (!rIsX ? CubeFlag.MiddleSliceSides : CubeFlag.MiddleSlice) | l), clockwise, EdgeTargetPos),
              new PatternItem(new CubePosition(CornerTargetPos &~ CubeFlag.BottomLayer | CubeFlag.TopLayer), Orientation.Correct, CornerTargetPos)
            }),
          new Algorithm("{0} U' {0}' U2 {0} U {0}'",CubeFlagService.ToNotationString(r))
        },
        {
          new Pattern(new List<PatternItem>()
            {
              new PatternItem(new CubePosition(CubeFlag.TopLayer | (!rIsX ? CubeFlag.MiddleSliceSides : CubeFlag.MiddleSlice) | l), correct, EdgeTargetPos),
              new PatternItem(new CubePosition(CornerTargetPos &~ CubeFlag.BottomLayer | CubeFlag.TopLayer), Orientation.Correct, CornerTargetPos)
            }),
          new Algorithm("U' {0}' U2 {0} U' {0}' U {0}",CubeFlagService.ToNotationString(f))
        },
        {
          new Pattern(new List<PatternItem>()
            {
              new PatternItem(new CubePosition(CubeFlag.TopLayer | (rIsX ? CubeFlag.MiddleSliceSides : CubeFlag.MiddleSlice) | f), correct, EdgeTargetPos),
              new PatternItem(new CubePosition(CornerTargetPos &~ CubeFlag.BottomLayer | CubeFlag.TopLayer), Orientation.Correct, CornerTargetPos)
            }),
          new Algorithm("{0}' U2 {0} U {0}' U' {0}",CubeFlagService.ToNotationString(f))
        },
        {
          new Pattern(new List<PatternItem>()
            {
              new PatternItem(new CubePosition(CubeFlag.TopLayer | (rIsX ? CubeFlag.MiddleSliceSides : CubeFlag.MiddleSlice) | f), clockwise, EdgeTargetPos),
              new PatternItem(new CubePosition(CornerTargetPos &~ CubeFlag.BottomLayer | CubeFlag.TopLayer), Orientation.Correct, CornerTargetPos)
            }),
          new Algorithm("{0}' U' {1}' U {1} {0} {1}' U {1}",CubeFlagService.ToNotationString(r), CubeFlagService.ToNotationString(f))
        },
        {
          new Pattern(new List<PatternItem>()
            {
              new PatternItem(new CubePosition(CubeFlag.TopLayer | (!rIsX ? CubeFlag.MiddleSliceSides : CubeFlag.MiddleSlice) | r), correct, EdgeTargetPos),
              new PatternItem(new CubePosition(CornerTargetPos &~ CubeFlag.BottomLayer | CubeFlag.TopLayer), Orientation.Correct, CornerTargetPos)
            }),
          new Algorithm("{0} U2 {0}' U' {0} U {0}'",CubeFlagService.ToNotationString(r))
        },
        {
          new Pattern(new List<PatternItem>()
            {
              new PatternItem(new CubePosition(CubeFlag.TopLayer | (!rIsX ? CubeFlag.MiddleSliceSides : CubeFlag.MiddleSlice) | r), clockwise, EdgeTargetPos),
              new PatternItem(new CubePosition(CornerTargetPos &~ CubeFlag.BottomLayer | CubeFlag.TopLayer), Orientation.Correct, CornerTargetPos)
            }),
          new Algorithm("{0} U {1} U' {1}' {0}' {1} U' {1}'",CubeFlagService.ToNotationString(f), CubeFlagService.ToNotationString(r))
        },
        #endregion

        #region Corner counter-clockwise oriented in top layer
        {
          new Pattern(new List<PatternItem>()
            {
              new PatternItem(new CubePosition(EdgeTargetPos), Orientation.Correct, EdgeTargetPos),
              new PatternItem(new CubePosition(CornerTargetPos &~ CubeFlag.BottomLayer | CubeFlag.TopLayer), Orientation.CounterClockwise, CornerTargetPos)
            }),
          new Algorithm("U' {0} U' {0}' U2 {0} U' {0}'",CubeFlagService.ToNotationString(r))
        },
        {
          new Pattern(new List<PatternItem>()
            {
              new PatternItem(new CubePosition(EdgeTargetPos), Orientation.Clockwise, EdgeTargetPos),
              new PatternItem(new CubePosition(CornerTargetPos &~ CubeFlag.BottomLayer | CubeFlag.TopLayer), Orientation.CounterClockwise, CornerTargetPos)
            }),
          new Algorithm("U' {0} U {0}' U {1}' U' {1}",CubeFlagService.ToNotationString(r), CubeFlagService.ToNotationString(f))
        },
        {
          new Pattern(new List<PatternItem>()
            {
              new PatternItem(new CubePosition(CubeFlag.TopLayer | (!rIsX ? CubeFlag.MiddleSliceSides : CubeFlag.MiddleSlice) | r), clockwise, EdgeTargetPos),
              new PatternItem(new CubePosition(CornerTargetPos &~ CubeFlag.BottomLayer | CubeFlag.TopLayer), Orientation.CounterClockwise, CornerTargetPos)
            }),
          new Algorithm("U {0} U' {0}'",CubeFlagService.ToNotationString(r))
        },
        {
          new Pattern(new List<PatternItem>()
            {
              new PatternItem(new CubePosition(CubeFlag.TopLayer | (!rIsX ? CubeFlag.MiddleSliceSides : CubeFlag.MiddleSlice) | r), correct, EdgeTargetPos),
              new PatternItem(new CubePosition(CornerTargetPos &~ CubeFlag.BottomLayer | CubeFlag.TopLayer), Orientation.CounterClockwise, CornerTargetPos)
            }),
          new Algorithm("{0} U2 {0}2 U' {0}2 U' {0}'",CubeFlagService.ToNotationString(f))
        },
        {
          new Pattern(new List<PatternItem>()
            {
              new PatternItem(new CubePosition(CubeFlag.TopLayer | (rIsX ? CubeFlag.MiddleSliceSides : CubeFlag.MiddleSlice) | b), clockwise, EdgeTargetPos),
              new PatternItem(new CubePosition(CornerTargetPos &~ CubeFlag.BottomLayer | CubeFlag.TopLayer), Orientation.CounterClockwise, CornerTargetPos)
            }),
          new Algorithm("U' {0} U {0}' U2 {0} U' {0}'",CubeFlagService.ToNotationString(r))
        },
        {
          new Pattern(new List<PatternItem>()
            {
              new PatternItem(new CubePosition(CubeFlag.TopLayer | (rIsX ? CubeFlag.MiddleSliceSides : CubeFlag.MiddleSlice) | b), correct, EdgeTargetPos),
              new PatternItem(new CubePosition(CornerTargetPos &~ CubeFlag.BottomLayer | CubeFlag.TopLayer), Orientation.CounterClockwise, CornerTargetPos)
            }),
          new Algorithm("U' {0} U' {0}' U {1}' U' {1}",CubeFlagService.ToNotationString(r), CubeFlagService.ToNotationString(f))
        },
        {
          new Pattern(new List<PatternItem>()
            {
              new PatternItem(new CubePosition(CubeFlag.TopLayer | (!rIsX ? CubeFlag.MiddleSliceSides : CubeFlag.MiddleSlice) | l), correct, EdgeTargetPos),
              new PatternItem(new CubePosition(CornerTargetPos &~ CubeFlag.BottomLayer | CubeFlag.TopLayer), Orientation.CounterClockwise, CornerTargetPos)
            }),
          new Algorithm("{0}' U' {0}",CubeFlagService.ToNotationString(f))
        },
        {
          new Pattern(new List<PatternItem>()
            {
              new PatternItem(new CubePosition(CubeFlag.TopLayer | (!rIsX ? CubeFlag.MiddleSliceSides : CubeFlag.MiddleSlice) | l), clockwise, EdgeTargetPos),
              new PatternItem(new CubePosition(CornerTargetPos &~ CubeFlag.BottomLayer | CubeFlag.TopLayer), Orientation.CounterClockwise, CornerTargetPos)
            }),
          new Algorithm("U' {0} U2 {0}' U2 {0} U' {0}'",CubeFlagService.ToNotationString(r))
        },
        {
          new Pattern(new List<PatternItem>()
            {
              new PatternItem(new CubePosition(CubeFlag.TopLayer | (rIsX ? CubeFlag.MiddleSliceSides : CubeFlag.MiddleSlice) | f), correct, EdgeTargetPos),
              new PatternItem(new CubePosition(CornerTargetPos &~ CubeFlag.BottomLayer | CubeFlag.TopLayer), Orientation.CounterClockwise, CornerTargetPos)
            }),
          new Algorithm("d {0}' U {0} U' {0}' U' {0}",CubeFlagService.ToNotationString(r))
        },
        {
          new Pattern(new List<PatternItem>()
            {
              new PatternItem(new CubePosition(CubeFlag.TopLayer | (rIsX ? CubeFlag.MiddleSliceSides : CubeFlag.MiddleSlice) | f), clockwise, EdgeTargetPos),
              new PatternItem(new CubePosition(CornerTargetPos &~ CubeFlag.BottomLayer | CubeFlag.TopLayer), Orientation.CounterClockwise, CornerTargetPos)
            }),
          new Algorithm("{0}' U {0} U2 {1} U {1}'",CubeFlagService.ToNotationString(f), CubeFlagService.ToNotationString(r))
        },
        #endregion

        #region Corner clockwise oriented in top layer
        {
          new Pattern(new List<PatternItem>()
            {
              new PatternItem(new CubePosition(EdgeTargetPos), Orientation.Correct, EdgeTargetPos),
              new PatternItem(new CubePosition(CornerTargetPos &~ CubeFlag.BottomLayer | CubeFlag.TopLayer), Orientation.Clockwise, CornerTargetPos)
            }),
          new Algorithm("U {0} U {0}' U2 {0} U {0}'",CubeFlagService.ToNotationString(r))
        },
        {
          new Pattern(new List<PatternItem>()
            {
              new PatternItem(new CubePosition(EdgeTargetPos), Orientation.Clockwise, EdgeTargetPos),
              new PatternItem(new CubePosition(CornerTargetPos &~ CubeFlag.BottomLayer | CubeFlag.TopLayer), Orientation.Clockwise, CornerTargetPos)
            }),
          new Algorithm("U2 {1}' U {1} U {0} U {0}'",CubeFlagService.ToNotationString(r), CubeFlagService.ToNotationString(f))
        },
        {
          new Pattern(new List<PatternItem>()
            {
              new PatternItem(new CubePosition(CubeFlag.TopLayer | (!rIsX ? CubeFlag.MiddleSliceSides : CubeFlag.MiddleSlice) | r), clockwise, EdgeTargetPos),
              new PatternItem(new CubePosition(CornerTargetPos &~ CubeFlag.BottomLayer | CubeFlag.TopLayer), Orientation.Clockwise, CornerTargetPos)
            }),
          new Algorithm("U' {0} U' {0}' U {0} U {0}'",CubeFlagService.ToNotationString(r))
        },
        {
          new Pattern(new List<PatternItem>()
            {
              new PatternItem(new CubePosition(CubeFlag.TopLayer | (!rIsX ? CubeFlag.MiddleSliceSides : CubeFlag.MiddleSlice) | r), correct, EdgeTargetPos),
              new PatternItem(new CubePosition(CornerTargetPos &~ CubeFlag.BottomLayer | CubeFlag.TopLayer), Orientation.Clockwise, CornerTargetPos)
            }),
          new Algorithm("{1} U' {1}' U2 {0}' U' {0}",CubeFlagService.ToNotationString(f), CubeFlagService.ToNotationString(r))
        },
        {
          new Pattern(new List<PatternItem>()
            {
              new PatternItem(new CubePosition(CubeFlag.TopLayer | (rIsX ? CubeFlag.MiddleSliceSides : CubeFlag.MiddleSlice) | b), correct, EdgeTargetPos),
              new PatternItem(new CubePosition(CornerTargetPos &~ CubeFlag.BottomLayer | CubeFlag.TopLayer), Orientation.Clockwise, CornerTargetPos)
            }),
          new Algorithm("d {0}' U2 {0} U2 {0}' U {0}",CubeFlagService.ToNotationString(r))
        },
        {
          new Pattern(new List<PatternItem>()
            {
              new PatternItem(new CubePosition(CubeFlag.TopLayer | (rIsX ? CubeFlag.MiddleSliceSides : CubeFlag.MiddleSlice) | b), clockwise, EdgeTargetPos),
              new PatternItem(new CubePosition(CornerTargetPos &~ CubeFlag.BottomLayer | CubeFlag.TopLayer), Orientation.Clockwise, CornerTargetPos)
            }),
          new Algorithm("{0} U {0}'",CubeFlagService.ToNotationString(r))
        },
        {
          new Pattern(new List<PatternItem>()
            {
              new PatternItem(new CubePosition(CubeFlag.TopLayer | (!rIsX ? CubeFlag.MiddleSliceSides : CubeFlag.MiddleSlice) | l), correct, EdgeTargetPos),
              new PatternItem(new CubePosition(CornerTargetPos &~ CubeFlag.BottomLayer | CubeFlag.TopLayer), Orientation.Clockwise, CornerTargetPos)
            }),
          new Algorithm("d {0}' U' {0} U2 {0}' U {0}",CubeFlagService.ToNotationString(r))
        },
        {
          new Pattern(new List<PatternItem>()
            {
              new PatternItem(new CubePosition(CubeFlag.TopLayer | (!rIsX ? CubeFlag.MiddleSliceSides : CubeFlag.MiddleSlice) | l), clockwise, EdgeTargetPos),
              new PatternItem(new CubePosition(CornerTargetPos &~ CubeFlag.BottomLayer | CubeFlag.TopLayer), Orientation.Clockwise, CornerTargetPos)
            }),
          new Algorithm("d {0}' U {0} d' {0} U {0}'",CubeFlagService.ToNotationString(r))
        },
        {
          new Pattern(new List<PatternItem>()
            {
              new PatternItem(new CubePosition(CubeFlag.TopLayer | (rIsX ? CubeFlag.MiddleSliceSides : CubeFlag.MiddleSlice) | f), clockwise, EdgeTargetPos),
              new PatternItem(new CubePosition(CornerTargetPos &~ CubeFlag.BottomLayer | CubeFlag.TopLayer), Orientation.Clockwise, CornerTargetPos)
            }),
          new Algorithm("{0}' U2 {0}2 U {0}2 U {0}",CubeFlagService.ToNotationString(r))
        },
        {
          new Pattern(new List<PatternItem>()
            {
              new PatternItem(new CubePosition(CubeFlag.TopLayer | (rIsX ? CubeFlag.MiddleSliceSides : CubeFlag.MiddleSlice) | f), correct, EdgeTargetPos),
              new PatternItem(new CubePosition(CornerTargetPos &~ CubeFlag.BottomLayer | CubeFlag.TopLayer), Orientation.Clockwise, CornerTargetPos)
            }),
          new Algorithm("U' {0}' U {0}",CubeFlagService.ToNotationString(f))
        }
        #endregion
      };
    }
  }
}
