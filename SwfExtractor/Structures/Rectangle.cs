using SwfExtractor.Tags;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwfExtractor.Structures {

	public class Rectangle {

		public readonly int BitLength;
		public readonly int XminTwips;
		public readonly int XmaxTwips;
		public readonly int YminTwips;
		public readonly int YmaxTwips;


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
				vals[i] = (int)TagUtilities.PickBits( data, offset, 5 + i * length, length, false );
			}

			rect = new Rectangle( length, vals[0], vals[2], ( vals[1] - vals[0] ), ( vals[3] - vals[2] ) );
			return (int)Math.Ceiling( ( 5 + length * 4 ) / 8.0 );
		}

	}

}
