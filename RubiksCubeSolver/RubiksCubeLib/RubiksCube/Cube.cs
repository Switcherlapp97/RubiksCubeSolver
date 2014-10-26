using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace RubiksCubeLib.RubiksCube
{
	/// <summary>
	/// Represents a single cube of the Rubik
	/// </summary>
	[Serializable]
	public class Cube
	{
		// *** CONSTRUCTORS ***

		/// <summary>
		///	Empty constructor
		/// </summary>
		public Cube() { }

		/// <summary>
		/// Constructor with the position (the faces will be generated)
		/// </summary>
		/// <param name="position">Defines the position of the cube</param>
		public Cube(CubeFlag position) : this(UniCube.GenFaces(), position) { }

		/// <summary>
		/// Constructor with faces and position
		/// </summary>
		/// <param name="faces">Defines the faces where the cube belongs to</param>
		/// <param name="position">Defines the position of the cube</param>
		public Cube(IEnumerable<Face> faces, CubeFlag position)
		{
			this.Faces = faces;
			this.Position = new CubePosition(position);
			this.Colors = new List<Color>();
			this.Colors.Clear();
			this.Faces.ToList().ForEach(f => Colors.Add(f.Color));
		}

		/// <summary>
		/// Constructor with faces and position
		/// </summary>
		/// <param name="faces">Defines the faces where the cube belongs to</param>
		/// <param name="position">Defines the position of the cube</param>
		public Cube(IEnumerable<Face> faces, CubePosition position)
		{
			this.Faces = faces;
			this.Position = position;
		}




		// *** Properties ***

		/// <summary>
		/// The faces where the cube belongs to
		/// </summary>
		public IEnumerable<Face> Faces { get; set; }

		/// <summary>
		/// The colors the cube has
		/// </summary>
		public List<Color> Colors { get; set; }

		/// <summary>
		/// The position in the Rubik
		/// </summary>
		public CubePosition Position { get; set; }

		/// <summary>
		/// Returns true if this cube is placed at the corner
		/// </summary>
		public bool IsCorner { get { return CubePosition.IsCorner(Position.Flags); } }

		/// <summary>
		/// Returns true if this cube is placed at the edge
		/// </summary>
		public bool IsEdge { get { return CubePosition.IsEdge(Position.Flags); } }

		/// <summary>
		/// Returns true if this cube is placed at the center
		/// </summary>
		public bool IsCenter { get { return CubePosition.IsCenter(Position.Flags); } }






		// *** METHODS ***
		
		/// <summary>
		/// Returns a clone of this cube (same properties but different instance)
		/// </summary>
		/// <returns></returns>
		public Cube DeepClone()
		{
			using (var ms = new MemoryStream())
			{
				var formatter = new BinaryFormatter();
				formatter.Serialize(ms, this);
				ms.Position = 0;

				return (Cube)formatter.Deserialize(ms);
			}
		}

		/// <summary>
		/// Changes the color of the given face of this cube
		/// </summary>
		/// <param name="face">Defines the face to be changed</param>
		/// <param name="color">Defines the color to be set</param>
		public void SetFaceColor(FacePosition face, Color color)
		{
			this.Faces.Where(f => f.Position == face).ToList().ForEach(f => f.Color = color);
			this.Colors.Clear();
			this.Faces.ToList().ForEach(f => Colors.Add(f.Color));
		}

		/// <summary>
		/// Returns the color of the given face
		/// </summary>
		/// <param name="face">Defines the face to be analyzed</param>
		/// <returns></returns>
		public Color GetFaceColor(FacePosition face)
		{
			return this.Faces.First(f => f.Position == face).Color;
		}

		/// <summary>
		/// Set the color of all faces back to black
		/// </summary>
		public void ResetColors()
		{
			this.Faces.ToList().ForEach(f => f.Color = Color.Black);
			this.Colors.Clear();
			this.Faces.ToList().ForEach(f => Colors.Add(f.Color));
		}

		/// <summary>
		/// Change the position of the cube by rotating it on the given layer and the given direction
		/// </summary>
		/// <param name="layer">Defines the layer the cube is to rotate on</param>
		/// <param name="direction">Defines the direction of the rotation (true == clockwise)</param>
		public void NextPos(CubeFlag layer, bool direction)
		{
      Cube oldCube = DeepClone();
      Position.NextFlag(layer, direction);
			#region Colors
			if (this.IsCorner)
			{
				//Set colors
				Face layerFace = this.Faces.First(f => f.Position == CubeFlagService.ToFacePosition(layer));
				Color layerColor = layerFace.Color;

				CubeFlag newFlag = CubeFlagService.FirstNotInvalidFlag(this.Position.Flags, oldCube.Position.Flags);
				CubeFlag commonFlag = CubeFlagService.FirstNotInvalidFlag(this.Position.Flags, newFlag | layer);
				CubeFlag oldFlag = CubeFlagService.FirstNotInvalidFlag(oldCube.Position.Flags, commonFlag | layer);

				Color colorNewPos = Faces.First(f => f.Position == CubeFlagService.ToFacePosition(commonFlag)).Color;
				Color colorCommonPos = Faces.First(f => f.Position == CubeFlagService.ToFacePosition(oldFlag)).Color;

				ResetColors();
				SetFaceColor(layerFace.Position, layerColor);
				SetFaceColor(CubeFlagService.ToFacePosition(newFlag), colorNewPos);
				SetFaceColor(CubeFlagService.ToFacePosition(commonFlag), colorCommonPos);
			}

			if (this.IsCenter)
			{
				CubeFlag oldFlag = CubeFlagService.FirstNotInvalidFlag(oldCube.Position.Flags, CubeFlag.MiddleLayer | CubeFlag.MiddleSlice | CubeFlag.MiddleSliceSides);
				Color centerColor = this.Faces.First(f => f.Position == CubeFlagService.ToFacePosition(oldFlag)).Color;
				CubeFlag newPos = CubeFlagService.FirstNotInvalidFlag(this.Position.Flags, CubeFlag.MiddleSliceSides | CubeFlag.MiddleSlice | CubeFlag.MiddleLayer);

				ResetColors();
				SetFaceColor(CubeFlagService.ToFacePosition(newPos), centerColor);
			}

			if (this.IsEdge)
			{
				CubeFlag newFlag = CubeFlagService.FirstNotInvalidFlag(this.Position.Flags, oldCube.Position.Flags | CubeFlag.MiddleSlice | CubeFlag.MiddleSliceSides | CubeFlag.MiddleLayer);
				CubeFlag commonFlag = CubeFlagService.FirstNotInvalidFlag(this.Position.Flags, newFlag | CubeFlag.MiddleSlice | CubeFlag.MiddleSliceSides | CubeFlag.MiddleLayer);
				CubeFlag oldFlag = CubeFlagService.FirstNotInvalidFlag(oldCube.Position.Flags, commonFlag | CubeFlag.MiddleSlice | CubeFlag.MiddleSliceSides | CubeFlag.MiddleLayer);

				Color colorNewPos = this.Faces.First(f => f.Position == CubeFlagService.ToFacePosition(commonFlag)).Color;
				Color colorCommonPos = this.Faces.First(f => f.Position == CubeFlagService.ToFacePosition(oldFlag)).Color;


				ResetColors();
				if (layer == CubeFlag.MiddleLayer || layer == CubeFlag.MiddleSlice || layer == CubeFlag.MiddleSliceSides)
				{
					SetFaceColor(CubeFlagService.ToFacePosition(newFlag), colorNewPos);
					SetFaceColor(CubeFlagService.ToFacePosition(commonFlag), colorCommonPos);
				}
				else
				{
					SetFaceColor(CubeFlagService.ToFacePosition(commonFlag), colorNewPos);
					SetFaceColor(CubeFlagService.ToFacePosition(newFlag), colorCommonPos);
				}
			}
			#endregion
		}
	}
}
