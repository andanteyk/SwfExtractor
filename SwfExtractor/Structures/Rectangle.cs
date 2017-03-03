using SwfExtractor.Tags;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwfExtractor.Structures {

	[System.Diagnostics.DebuggerDisplay( "({Xmin}, {Xmax}), {Width} x {Height}" )]
	public class Rectangle {

		public readonly int BitLength;
		public readonly int XminTwips;
		public readonly int XmaxTwips;
		public readonly int YminTwips;
		public readonly int YmaxTwips;


		public double Xmin { get { return XminTwips / 20.0f; } }
		public double Xmax { get { return XmaxTwips / 20.0f; } }
		public double Ymin { get { return YminTwips / 20.0f; } }
		public double Ymax { get { return YmaxTwips / 20.0f; } }

		public double Width { get { return Xmax - Xmin; } }
		public double Height { get { return Ymax - Ymin; } }

		public System.Drawing.PointF Location { get { return new System.Drawing.PointF( (float)Xmin, (float)Ymin ); } }
		public System.Drawing.SizeF Size { get { return new System.Drawing.SizeF( (float)Width, (float)Height ); } }


		public Rectangle( int bitLength, int xmin, int xmax, int ymin, int ymax ) {
			BitLength = bitLength;
			XminTwips = xmin;
			XmaxTwips = xmax;
			YminTwips = ymin;
			YmaxTwips = ymax;
		}

		public static int GetRectangle( byte[] data, int offset, out Rectangle rect ) {

			byte length = (byte)( data[offset] >> 3 );
			int[] vals = new int[4];

			for ( int i = 0; i < 4; i++ ) {
				vals[i] = (int)TagUtilities.PickSignedBits( data, offset, 5 + i * length, length, false );
			}

			rect = new Rectangle( length, vals[0], vals[1], vals[2], vals[3] );
			return (int)Math.Ceiling( ( 5 + length * 4 ) / 8.0 );
		}


		public System.Drawing.RectangleF ToRectangle() {
			return new System.Drawing.RectangleF( Location, Size );
		}

		public System.Drawing.RectangleF ToPositiveRectangle() {
			double x = Math.Min( Xmin, Xmax );
			double y = Math.Min( Ymin, Ymax );
			double width = Math.Abs( Xmax - Xmin );
			double height = Math.Abs( Ymax - Ymin );
			return new System.Drawing.RectangleF( (float)x, (float)y, (float)width, (float)height );
		}
	}

}
