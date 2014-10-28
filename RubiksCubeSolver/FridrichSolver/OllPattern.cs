using RubiksCubeLib;
using RubiksCubeLib.Solver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FridrichSolver
{
  public class OllPattern : PatternTable
  {
    private Dictionary<Pattern, Algorithm> patterns = new Dictionary<Pattern, Algorithm>()
      {
        // All edges oriented correctly
        {new Pattern(new string[] {"LFU,1", "RFU,1", "RBU,1"}, 1.0 / 54.0), new Algorithm("R' U' R U' R' U2 R")}, // OLL #26
        {new Pattern(new string[] {"LFU,2", "RBU,2", "LBU,2"}, 1.0 / 54.0), new Algorithm("R' U2 R U R' U R")}, // OLL #27
        {new Pattern(new string[] {"LFU,2", "RFU,1", "RBU,2", "LBU,1"}, 1.0 / 108.0), new Algorithm("R U R' U R U' R' U R U2 R'")}, // OLL #21
        {new Pattern(new string[] {"LFU,2", "RFU,2", "RBU,1", "LBU,1"}, 1.0 / 54.0), new Algorithm("R U2 R2 U' R2 U' R2 U2 R")}, // OLL #22
        {new Pattern(new string[] {"RFU,2", "RBU,1"}, 1.0 / 54.0), new Algorithm("x R' U' L U R U' L' U x'")}, // OLL #24
        {new Pattern(new string[] {"LFU,1", "RBU,2"}, 1.0 / 54.0), new Algorithm("R' F R B' R' F' R B")}, // OLL #25
        {new Pattern(new string[] {"LFU,1", "RFU,2"}, 1.0 / 54.0), new Algorithm("R2 D R' U2 R D' R' U2 R'")}, // OLL #23

        // Corners correct, edges flipped
        {new Pattern(new string[] {"MFU,1", "RSU,1"}, 1.0 / 54.0), new Algorithm("r U R' U' M' U R U' R'")}, // OLL #28
        {new Pattern(new string[] {"MFU,1", "MBU,1"}, 1.0 / 108.0), new Algorithm("R U R' U' M U R U' r'")}, // OLL #57

        // P-shapes
        {new Pattern(new string[] {"LFU,1", "MFU,1", "LSU,1", "LBU,2"}, 1.0 / 54.0), new Algorithm("R' U' F U R U' R' F' R")}, // OLL #31
        {new Pattern(new string[] {"RFU,2", "MFU,1", "RSU,1", "RBU,1"}, 1.0 / 54.0), new Algorithm("L U F' U' L' U L F L'")}, // OLL #32
        {new Pattern(new string[] {"LFU,2", "MFU,1", "LSU,1", "LBU,1"}, 1.0 / 54.0), new Algorithm("F' U' L' U L F")}, // OLL #43
        {new Pattern(new string[] {"RFU,1", "MFU,1", "RSU,1", "RBU,2"}, 1.0 / 54.0), new Algorithm("F U R U' R' F'")}, // OLL #44

        // W-shapes
        {new Pattern(new string[] {"LFU,1", "MBU,1", "RSU,1", "RBU,2"}, 1.0 / 54.0), new Algorithm("R' U' R U' R' U R U x' R U' R' U x")}, // OLL #36
        {new Pattern(new string[] {"RFU,1", "MFU,1", "RSU,1", "LBU,2"}, 1.0 / 54.0), new Algorithm("R U R' U R U' R' U' R' F R F'")}, // OLL #38

        // Squares
        {new Pattern(new string[] {"LFU,2", "LBU,2", "RBU,2", "LSU,1", "MBU,1"}, 1.0 / 54.0), new Algorithm("r' U2 R U R' U r")}, // OLL #5
        {new Pattern(new string[] {"LFU,1", "LBU,1", "RFU,1", "LSU,1", "MFU,1"}, 1.0 / 54.0), new Algorithm("r U2 R' U' R U' r'")}, // OLL #6

        // L-shapes
        {new Pattern(new string[] {"LFU,2", "LBU,1", "RBU,1", "RFU,2", "RSU,1", "MFU,1"}, 1.0 / 54.0), new Algorithm("F R U R' U' R U R' U' F'")}, // OLL #48
        {new Pattern(new string[] {"LFU,1", "LBU,2", "RBU,2", "RFU,1", "LSU,1", "MFU,1"}, 1.0 / 54.0), new Algorithm("F' L' U' L U L' U' L U F")}, // OLL #47
        {new Pattern(new string[] {"LFU,1", "LBU,2", "RBU,2", "RFU,1", "RSU,1", "MBU,1"}, 1.0 / 54.0), new Algorithm("R B' R2 F R2 B R2 F' R")}, // OLL #49
        {new Pattern(new string[] {"LFU,1", "LBU,2", "RBU,2", "RFU,1", "RSU,1", "MFU,1"}, 1.0 / 54.0), new Algorithm("R' F R2 B' R2 F' R2 B R'")}, // OLL #50
        {new Pattern(new string[] {"LFU,1", "LBU,2", "RBU,1", "RFU,2", "RSU,1", "MBU,1"}, 1.0 / 54.0), new Algorithm("r' U2 R U R' U' R U R' U r")}, // OLL #53
        {new Pattern(new string[] {"LFU,1", "LBU,2", "RBU,1", "RFU,2", "RSU,1", "MFU,1"}, 1.0 / 54.0), new Algorithm("r U2 R' U' R U R' U' R U' r'")}, // OLL #54

        // Fish shapes
        {new Pattern(new string[] {"LFU,1", "LBU,1", "RBU,1", "MFU,1", "RSU,1"}, 1.0 / 54.0), new Algorithm("R U R' U' R' F R2 U R' U' F'")}, // OLL #9
        {new Pattern(new string[] {"LFU,2", "LBU,2", "RFU,2", "MBU,1", "RSU,1"}, 1.0 / 54.0), new Algorithm("R U R' y R' F R U' R' F' R")}, // OLL #10
        {new Pattern(new string[] {"LFU,1", "RBU,2", "LSU,1", "MBU,1"}, 1.0 / 54.0), new Algorithm("R U2 R2 F R F' R U2 R'")}, // OLL #35
        {new Pattern(new string[] {"LFU,1", "MFU,1", "RSU,1", "RBU,2"}, 1.0 / 54.0), new Algorithm("F R U' R' U' R U R' F'")}, // OLL #37

        // Awkward shapes
        {new Pattern(new string[] {"LFU,2", "MBU,1", "RSU,1", "RFU,1"}, 1.0 / 54.0), new Algorithm("M' U R U R' U' R' F R F' M")}, // OLL #29
        {new Pattern(new string[] {"LBU,1", "MFU,1", "RSU,1", "RBU,2"}, 1.0 / 54.0), new Algorithm("F R' F R2 U' R' U' R U R' F2")}, // OLL #30
        {new Pattern(new string[] {"LBU,2", "MFU,1", "RSU,1", "RBU,1"}, 1.0 / 54.0), new Algorithm("F U R U' R' F' R' U2 R U R' U R")}, // OLL #41
        {new Pattern(new string[] {"LFU,1", "MBU,1", "RSU,1", "RFU,2"}, 1.0 / 54.0), new Algorithm("R' U' R U' R' U2 R F R U R' U' F'")}, // OLL #42

        // Lightning bolts
        {new Pattern(new string[] {"RFU,2", "LBU,2", "RBU,2", "RSU,1", "MFU,1"}, 1.0 / 54.0), new Algorithm("r U R' U R U2 r'")}, // OLL #7
        {new Pattern(new string[] {"RFU,1", "RBU,1", "LFU,1", "RSU,1", "MBU,1"}, 1.0 / 54.0), new Algorithm("r' U' R U' R' U2 r")}, // OLL #8
        {new Pattern(new string[] {"RFU,2", "LBU,2", "LFU,2", "RSU,1", "MFU,1"}, 1.0 / 54.0), new Algorithm("r U R' U R' F R F' R U2 r'")}, // OLL #11
        {new Pattern(new string[] {"RBU,1", "LBU,1", "RFU,1", "RSU,1", "MBU,1"}, 1.0 / 54.0), new Algorithm("r' U' R U' x' R U' R' U x R' U2 r")}, // OLL 12
        {new Pattern(new string[] {"LBU,2", "RFU,1", "MFU,1", "MBU,1"}, 1.0 / 54.0), new Algorithm("L F' L' U' L U F U' L'")}, // OLL #39
        {new Pattern(new string[] {"LFU,2", "RBU,1", "MFU,1", "MBU,1"}, 1.0 / 54.0), new Algorithm("R' F R U R' U' F' U R")}, // OLL #40

        // T-shapes
        {new Pattern(new string[] {"LBU,2", "LFU,1", "MFU,1", "MBU,1"}, 1.0 / 54.0), new Algorithm("R U R' U' R' F R F'")}, // OLL #33
        {new Pattern(new string[] {"LBU,1", "LFU,2", "MFU,1", "MBU,1"}, 1.0 / 54.0), new Algorithm("F R U R' U' F'")}, // OLL #45

        // C-shapes
        {new Pattern(new string[] {"LBU,1", "RBU,2", "MFU,1", "MBU,1"}, 1.0 / 54.0), new Algorithm("R U R' U' B' R' F R F' B")}, // OLL #34
        {new Pattern(new string[] {"RBU,2", "RFU,1", "LSU,1", "RSU,1"}, 1.0 / 54.0), new Algorithm("R' U' R' F R F' U R")}, // OLL #46

        // I-shapes
        {new Pattern(new string[] {"LFU,2", "LBU,1", "RBU,1", "RFU,2", "MFU,1", "MBU,1"}, 1.0 / 54.0), new Algorithm("f R U R' U' R U R' U' f'")}, // OLL #51
        {new Pattern(new string[] {"LFU,2", "LBU,1", "RBU,2", "RFU,1", "MFU,1", "MBU,1"}, 1.0 / 108.0), new Algorithm("r' U' r U' R' U R U' R' U M' U r")}, // OLL #56
        {new Pattern(new string[] {"LFU,1", "LBU,2", "RBU,2", "RFU,1", "RSU,1", "LSU,1"}, 1.0 / 54.0), new Algorithm("R U R' U R U' y R U' R' F'")}, // OLL #52
        {new Pattern(new string[] {"LFU,2", "LBU,1", "RBU,2", "RFU,1", "RSU,1", "LSU,1"}, 1.0 / 108.0), new Algorithm("R U2 R2 U' R U' R' U2 F R F'")}, // OLL #55

        // Knight move shapes
        {new Pattern(new string[] {"LFU,2", "LBU,2", "RFU,2", "MBU,1", "MFU,1"}, 1.0 / 54.0), new Algorithm("x' R U' R' F' R U R' x y R' U R")}, // OLL #13
        {new Pattern(new string[] {"LFU,1", "LBU,1", "RBU,1", "MBU,1", "MFU,1"}, 1.0 / 54.0), new Algorithm("R' F R U R' F' R y' R U' R'")}, // OLL #14
        {new Pattern(new string[] {"LFU,1", "LBU,1", "RFU,1", "MBU,1", "MFU,1"}, 1.0 / 54.0), new Algorithm("r U r' R U R' U' r U' r'")}, // OLL #16
        {new Pattern(new string[] {"LFU,2", "LBU,2", "RBU,2", "MBU,1", "MFU,1"}, 1.0 / 54.0), new Algorithm("r' U' r R' U' R U r' U r")}, // OLL #15

        // No edges flipped correctly
        {new Pattern(new string[] {"LFU,2", "LBU,1", "RBU,2", "RFU,1", "RSU,1", "LSU,1", "MFU,1", "MBU,1"}, 1.0 / 108.0), new Algorithm("R U2 R2 F R F' U2 R' F R F'")}, // OLL #1
        {new Pattern(new string[] {"LFU,2", "LBU,1", "RBU,1", "RFU,2", "RSU,1", "LSU,1", "MFU,1", "MBU,1"}, 1.0 / 54.0), new Algorithm("F R U R' U' F' f R U R' U' f'")}, // OLL #2
        {new Pattern(new string[] {"LFU,2", "LBU,2", "RBU,2", "RSU,1", "LSU,1", "MFU,1", "MBU,1"}, 1.0 / 54.0), new Algorithm("f R U R' U' f' U' F R U R' U' F'")}, // OLL #3
        {new Pattern(new string[] {"LFU,1", "LBU,1", "RFU,1", "RSU,1", "LSU,1", "MFU,1", "MBU,1"}, 1.0 / 54.0), new Algorithm("f R U R' U' f' U F R U R' U' F'")}, // OLL #4
        {new Pattern(new string[] {"LFU,1", "RFU,2", "RSU,1", "LSU,1", "MFU,1", "MBU,1"}, 1.0 / 54.0), new Algorithm("r U R' U R U2 r2 U' R U' R' U2 r")}, // OLL #18
        {new Pattern(new string[] {"LFU,2", "RFU,1", "RSU,1", "LSU,1", "MFU,1", "MBU,1"}, 1.0 / 54.0), new Algorithm("M' U R U R' U' r R2 F R F'")}, // OLL #19
        {new Pattern(new string[] {"LFU,2", "RBU,1", "RSU,1", "LSU,1", "MFU,1", "MBU,1"}, 1.0 / 54.0), new Algorithm("R U R' U R' F R F' U2 R' F R F'")}, // OLL #17
        {new Pattern(new string[] {"RSU,1", "LSU,1", "MFU,1", "MBU,1"}, 1.0 / 261.0), new Algorithm("M' U R U R' U' M2 U R U' r'")}, // OLL #20
      };

    public override Dictionary<Pattern, Algorithm> Patterns { get { return patterns; } }
  }
}
