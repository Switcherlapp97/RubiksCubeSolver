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
			if (layer == CubeFlag.LeftSlice || layer == CubeFlag.BottomLayer || layer == CubeFlag.BackSlice)
				direction = !direction;

			//Get the direction of the rotation depending on the layer
			RotationType rotation;
			if (layer == CubeFlag.RightSlice || layer == CubeFlag.MiddleSliceSides || layer == CubeFlag.LeftSlice)
				rotation = RotationType.X;
			else if (layer == CubeFlag.TopLayer || layer == CubeFlag.MiddleLayer || layer == CubeFlag.BottomLayer)
				rotation = RotationType.Y;
			else
				rotation = RotationType.Z;

			Cube oldCube = DeepClone();

			#region X-Rotation
			if (rotation == RotationType.X)
			{
				if (this.IsCorner)
				{
					//Set new position of corner
					Position.Y = ((direction ^ oldCube.Position.Z == CubeFlag.BackSlice)) ? CubeFlag.TopLayer : CubeFlag.BottomLayer;
					Position.Z = ((direction ^ oldCube.Position.Y == CubeFlag.TopLayer)) ? CubeFlag.FrontSlice : CubeFlag.BackSlice;
				}
				if (IsEdge || (IsCenter && layer == CubeFlag.MiddleSliceSides))
				{
					//Set new position of edge or center (if necessary)
					if (oldCube.Position.Y == CubeFlag.TopLayer || oldCube.Position.Y == CubeFlag.BottomLayer)
						this.Position.Y = CubeFlag.MiddleLayer;
					if (oldCube.Position.Z == CubeFlag.FrontSlice)
						this.Position.Y = (direction) ? CubeFlag.TopLayer : CubeFlag.BottomLayer;
					if (oldCube.Position.Z == CubeFlag.BackSlice)
						this.Position.Y = (direction) ? CubeFlag.BottomLayer : CubeFlag.TopLayer;

					if (oldCube.Position.Z == CubeFlag.FrontSlice || oldCube.Position.Z == CubeFlag.BackSlice)
						this.Position.Z = CubeFlag.MiddleSlice;
					if (oldCube.Position.Y == CubeFlag.TopLayer)
						this.Position.Z = (direction) ? CubeFlag.BackSlice : CubeFlag.FrontSlice;
					if (oldCube.Position.Y == CubeFlag.BottomLayer)
						this.Position.Z = (direction) ? CubeFlag.FrontSlice : CubeFlag.BackSlice;
				}
			}
			#endregion

			#region Y-Rotation
			if (rotation == RotationType.Y)
			{
				if (this.IsCorner)
				{
					//Set new position
					this.Position.X = (!(direction ^ oldCube.Position.Z == CubeFlag.BackSlice)) ? CubeFlag.RightSlice : CubeFlag.LeftSlice;
					this.Position.Z = (!(direction ^ oldCube.Position.X == CubeFlag.RightSlice)) ? CubeFlag.FrontSlice : CubeFlag.BackSlice;
				}
				if (this.IsEdge || (this.IsCenter && layer == CubeFlag.MiddleLayer))
				{
					//Set new position
					if (oldCube.Position.X == CubeFlag.RightSlice || oldCube.Position.X == CubeFlag.LeftSlice)
						this.Position.X = CubeFlag.MiddleSliceSides;
					if (oldCube.Position.Z == CubeFlag.FrontSlice)
						this.Position.X = (direction) ? CubeFlag.LeftSlice : CubeFlag.RightSlice;
					if (oldCube.Position.Z == CubeFlag.BackSlice)
						this.Position.X = (direction) ? CubeFlag.RightSlice : CubeFlag.LeftSlice;

					if (oldCube.Position.Z == CubeFlag.FrontSlice || oldCube.Position.Z == CubeFlag.BackSlice)
						this.Position.Z = CubeFlag.MiddleSlice;
					if (oldCube.Position.X == CubeFlag.RightSlice)
						this.Position.Z = (direction) ? CubeFlag.FrontSlice : CubeFlag.BackSlice;
					if (oldCube.Position.X == CubeFlag.LeftSlice)
						this.Position.Z = (direction) ? CubeFlag.BackSlice : CubeFlag.FrontSlice;
				}
			}
			#endregion

			#region Z-Rotation
			if (rotation == RotationType.Z)
			{
				if (this.IsCorner)
				{
					this.Position.X = (!(direction ^ oldCube.Position.Y == CubeFlag.TopLayer)) ? CubeFlag.RightSlice : CubeFlag.LeftSlice;
					this.Position.Y = (!(direction ^ oldCube.Position.X == CubeFlag.LeftSlice)) ? CubeFlag.TopLayer : CubeFlag.BottomLayer;
				}
				if (this.IsEdge || (IsCenter && layer == CubeFlag.MiddleSlice))
				{
					if (oldCube.Position.X == CubeFlag.RightSlice || oldCube.Position.X == CubeFlag.LeftSlice)
						this.Position.X = CubeFlag.MiddleSliceSides;
					if (oldCube.Position.Y == CubeFlag.TopLayer)
						this.Position.X = (direction) ? CubeFlag.RightSlice : CubeFlag.LeftSlice;
					if (oldCube.Position.Y == CubeFlag.BottomLayer)
						this.Position.X = (direction) ? CubeFlag.LeftSlice : CubeFlag.RightSlice;

					if (oldCube.Position.Y == CubeFlag.TopLayer || oldCube.Position.Y == CubeFlag.BottomLayer)
						this.Position.Y = CubeFlag.MiddleLayer;
					if (oldCube.Position.X == CubeFlag.RightSlice)
						this.Position.Y = (direction) ? CubeFlag.BottomLayer : CubeFlag.TopLayer;
					if (oldCube.Position.X == CubeFlag.LeftSlice)
						this.Position.Y = (direction) ? CubeFlag.TopLayer : CubeFlag.BottomLayer;
				}
			}
			#endregion

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
