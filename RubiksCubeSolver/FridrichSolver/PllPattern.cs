using RubiksCubeLib;
using RubiksCubeLib.Solver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FridrichSolver
{
  public class PllPattern: PatternTable
  {
    private Dictionary<Pattern, Algorithm> patterns = new Dictionary<Pattern, Algorithm>()
      {
        // Permutations of edges or corners only
        {new Pattern(new string[] {"MFU,MBU", "MBU,RSU", "RSU,MFU"}, 1.0 / 18.0), new Algorithm("R2 U F B' R2 F' B U R2")}, // PLL Ub
        {new Pattern(new string[] {"MFU,RSU", "MBU,MFU", "RSU,MBU"}, 1.0 / 18.0), new Algorithm("R2 U' F B' R2 F' B U' R2")}, // PLL Ua
        {new Pattern(new string[] {"MFU,LSU", "MBU,RSU", "RSU,MBU", "LSU,MFU"}, 1.0 / 36.0), new Algorithm("x' F R U' R' U D R' D U' R' U R D2 x")}, // PLL Z
        {new Pattern(new string[] {"MFU,MBU", "MBU,MFU", "RSU,LSU", "LSU,RSU"}, 1.0 / 72.0), new Algorithm("M2 U' M2 U2 M2 U' M2")}, // PLL H
        {new Pattern(new string[] {"LBU,RBU", "RBU,RFU", "RFU,LBU"}, 1.0 / 18.0), new Algorithm("x R' U R' D2 R U' R' D2 R2 x'")}, // PLL Aa
        {new Pattern(new string[] {"LBU,RFU", "RBU,LBU", "RFU,RBU"}, 1.0 / 18.0), new Algorithm("x R2 D2 R U R' D2 R U' R x'")}, // PLL Ab
        {new Pattern(new string[] {"LBU,LFU", "LFU,LBU", "RFU,RBU", "RBU,RFU"}, 1.0 / 36.0), new Algorithm("x' R U' R' D R U R' D' R U R' D R U' R' D' x")}, // PLL E

        // Swap one set of adjacent corners
        {new Pattern(new string[] {"MBU,RSU", "RSU,MBU", "RFU,LFU", "LFU,RFU"}, 1.0 / 18.0), new Algorithm("R U2 R' U2 R B' R' U' R U R B R2 U")}, // PLL Ra
        {new Pattern(new string[] {"MFU,RSU", "RSU,MFU", "RBU,LBU", "LBU,RBU"}, 1.0 / 18.0), new Algorithm("R' U2 R U2 R' F R U R' U' R' F' R2 U'")}, // PLL Rb
        {new Pattern(new string[] {"MFU,LSU", "LSU,MFU", "LBU,LFU", "LFU,LBU"}, 1.0 / 18.0), new Algorithm("R U' L' U R' U2 L U' L' U2 L")}, // PLL Ja
        {new Pattern(new string[] {"MFU,RSU", "RSU,MFU", "RBU,RFU", "RFU,RBU"}, 1.0 / 18.0), new Algorithm("L' U R U' L U2 R' U R U2 R'")}, // PLL Jb
        {new Pattern(new string[] {"LSU,RSU", "RSU,LSU", "RBU,RFU", "RFU,RBU"}, 1.0 / 18.0), new Algorithm("R U R' U' R' F R2 U' R' U' R U R' F'")}, // PLL T
        {new Pattern(new string[] {"LSU,RSU", "RSU,LSU", "RBU,LBU", "LBU,RBU"}, 1.0 / 18.0), new Algorithm("R' U R U' R2 y' R' U' R U y x R U R' U' R2 B' x'")}, // PLL F

        // Swap one set of corners diagonally
        {new Pattern(new string[] {"MBU,RSU", "RSU,MBU", "LBU,RFU", "RFU,LBU"}, 1.0 / 18.0), new Algorithm("R' U R' U' y R' F' R2 U' R' U R' F R F")}, // PLL V
        {new Pattern(new string[] {"MBU,LSU", "LSU,MBU", "LBU,RFU", "RFU,LBU"}, 1.0 / 18.0), new Algorithm("F R U' R' U' R U R' F' R U R' U' R' F R F'")}, // PLL Y
        {new Pattern(new string[] {"LSU,RSU", "RSU,LSU", "LFU,RBU", "RBU,LFU"}, 1.0 / 72.0), new Algorithm("L U' R U2 L' U R' L U' R U2 L' U R' U'")}, // PLL Na
        {new Pattern(new string[] {"LSU,RSU", "RSU,LSU", "RFU,LBU", "LBU,RFU"}, 1.0 / 72.0), new Algorithm("R' U L' U2 R U' L R' U L' U2 R U' L U")}, // PLL Nb

        // Double spins
        {new Pattern(new string[] {"LSU,RSU", "RSU,MBU", "MBU,LSU", "LFU,LBU", "LBU,RBU", "RBU,LFU"}, 1.0 / 18.0), new Algorithm("R2 u R' U R' U' R u' R2 y' R' U R")}, // PLL Ga
        {new Pattern(new string[] {"LSU,MBU", "RSU,LSU", "MBU,RSU", "RFU,RBU", "LBU,RFU", "RBU,LBU"}, 1.0 / 18.0), new Algorithm("L2 u' L U' L U L' u L2 y' R U' R'")}, // PLL Gc
        {new Pattern(new string[] {"LSU,MFU", "MFU,MBU", "MBU,LSU", "LFU,LBU", "LBU,RBU", "RBU,LFU"}, 1.0 / 18.0), new Algorithm("R U R' y' R2 u' R U' R' U R' u R2")}, // PLL Gd
        {new Pattern(new string[] {"MBU,RSU", "RSU,MFU", "MFU,MBU", "RFU,RBU", "LBU,RFU", "RBU,LBU"}, 1.0 / 18.0), new Algorithm("L' U' L y L2 u L' U L U' L u' L2")}, // PLL Gb
      };

    public override Dictionary<Pattern, Algorithm> Patterns { get { return patterns; } }
  }
}
