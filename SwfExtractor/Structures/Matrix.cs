using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwfExtractor.Structures {

	public class Matrix {

		public readonly bool HasScale;
		public readonly int ScaleBits;
		public readonly double ScaleX;
		public readonly double ScaleY;

		public readonly bool HasRotate;
		public readonly int RotateBits;
		public readonly double RotateSkew0;
		public readonly double RotateSkew1;

		public readonly int TranslateBits;
		public readonly int TranslateXTwips;
		public readonly int TranslateYTwips;

		public float TranslateX { get { return TranslateXTwips / 20.0f; } }
		public float TranslateY { get { return TranslateYTwips / 20.0f; } }


		public Matrix( bool hasScale, int scaleBits, double scaleX, double scaleY,
			bool hasRotate, int rotateBits, double rotateSkew0, double rotateSkew1,
			int translateBits, int translateX, int translateY ) {

			HasScale = hasScale;
			ScaleBits = scaleBits;
			ScaleX = scaleX;
			ScaleY = scaleY;

			HasRotate = hasRotate;
			RotateBits = rotateBits;
			RotateSkew0 = rotateSkew0;
			RotateSkew1 = rotateSkew1;

			TranslateBits = translateBits;
			TranslateXTwips = translateX;
			TranslateYTwips = translateY;
		}


		public static int GetMatrix( byte[] data, int offset, out Matrix matrix ) {

			int offsetBit = 0;

			bool hasScale = Tags.TagUtilities.PickBits( data, offset, offsetBit, 1, false ) != 0;
			offsetBit += 1;

			int scaleBits = 0;
			int scaleX = 0;
			int scaleY = 0;
			if ( hasScale ) {
				scaleBits = (int)Tags.TagUtilities.PickBits( data, offset, offsetBit, 5, false );
				offsetBit += 5;
				scaleX = (int)Tags.TagUtilities.PickBits( data, offset, offsetBit, scaleBits, false );
				offsetBit += scaleBits;
				scaleY = (int)Tags.TagUtilities.PickBits( data, offset, offsetBit, scaleBits, false );
				offsetBit += scaleBits;
			}

			bool hasRotate = Tags.TagUtilities.PickBits( data, offset, offsetBit, 1, false ) != 0;
			offsetBit += 1;

			int rotateBits = 0;
			int rotateSkew0 = 0;
			int rotateSkew1 = 0;
			if ( hasRotate ) {
				rotateBits = (int)Tags.TagUtilities.PickBits( data, offset, offsetBit, 5, false );
				offsetBit += 5;
				rotateSkew0 = (int)Tags.TagUtilities.PickBits( data, offset, offsetBit, rotateBits, false );
				offsetBit += rotateBits;
				rotateSkew1 = (int)Tags.TagUtilities.PickBits( data, offset, offsetBit, rotateBits, false );
				offsetBit += rotateBits;
			}

			int translateBits = (int)Tags.TagUtilities.PickBits( data, offset, offsetBit, 5, false );
			offsetBit += 5;
			int translateX = Tags.TagUtilities.PickSignedBits( data, offset, offsetBit, translateBits, false );
			offsetBit += translateBits;
			int translateY = Tags.TagUtilities.PickSignedBits( data, offset, offsetBit, translateBits, false );
			offsetBit += translateBits;


			matrix = new Matrix( hasScale, scaleBits, scaleX / 65536.0, scaleY / 65536.0,
				hasRotate, rotateBits, rotateSkew0 / 65536.0, rotateSkew1 / 65536.0,
				translateBits, translateX, translateY );

			return (int)Math.Ceiling( offsetBit / 8.0 );
		}
	}
}
