using System;
using System.Collections.Generic;
using System.Drawing;

namespace VirtualRubik
{
	public static class UniCube
	{
		public static IEnumerable<Face3D> genFaces(Cube3D.RubikPosition masterPosition)
		{
			return new Face3D[] {
		        new Face3D(new Point3D[] { new Point3D(-1, 1, -1), new Point3D(1, 1, -1), new Point3D(1, -1, -1), new Point3D(-1, -1, -1) }, Color.Black, Face3D.FacePosition.Front, masterPosition, Face3D.SelectionMode.None),   
                new Face3D(new Point3D[] { new Point3D(-1, 1, 1), new Point3D(1, 1, 1), new Point3D(1, -1, 1), new Point3D(-1, -1, 1) }, Color.Black, Face3D.FacePosition.Back, masterPosition, Face3D.SelectionMode.None),     
                new Face3D(new Point3D[] { new Point3D(-1, -1, -1), new Point3D(1, -1, -1), new Point3D(1, -1, 1), new Point3D(-1, -1, 1) }, Color.Black, Face3D.FacePosition.Top, masterPosition, Face3D.SelectionMode.None),
                new Face3D(new Point3D[] { new Point3D(-1, 1, -1), new Point3D(1, 1, -1), new Point3D(1, 1, 1), new Point3D(-1, 1, 1) }, Color.Black, Face3D.FacePosition.Bottom, masterPosition, Face3D.SelectionMode.None),      
                new Face3D(new Point3D[] { new Point3D(1, 1, 1), new Point3D(1, 1, -1), new Point3D(1, -1, -1), new Point3D(1, -1, 1) }, Color.Black, Face3D.FacePosition.Right, masterPosition, Face3D.SelectionMode.None),     
                new Face3D(new Point3D[] { new Point3D(-1, 1, 1), new Point3D(-1, 1, -1), new Point3D(-1, -1, -1), new Point3D(-1, -1, 1) }, Color.Black, Face3D.FacePosition.Left, masterPosition, Face3D.SelectionMode.None)
		    };
		}
	}
}
