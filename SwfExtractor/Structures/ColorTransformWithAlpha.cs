using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwfExtractor.Structures {

	public class ColorTransformWithAlpha {

		public readonly bool HasAddTerms;
		public readonly bool HasMultTerms;
		public readonly int FieldBits;

		public readonly double RedMultTerm;
		public readonly double GreenMultTerm;
		public readonly double BlueMultTerm;
		public readonly double AlphaMultTerm;

		public readonly int RedAddTerm;
		public readonly int GreenAddTerm;
		public readonly int BlueAddTerm;
		public readonly int AlphaAddTerm;


		public ColorTransformWithAlpha( bool hasAddTerms, bool hasMultTerms, int fieldBits,
			double redMultTerm, double greenMultTerm, double blueMultTerm, double alphaMultTerm,
			int redAddTerm, int greenAddTerm, int blueAddTerm, int alphaAddTerm ) {

			HasMultTerms = hasMultTerms;
			HasAddTerms = hasAddTerms;
			FieldBits = fieldBits;

			RedMultTerm = redMultTerm;
			GreenMultTerm = greenMultTerm;
			BlueMultTerm = blueMultTerm;
			AlphaMultTerm = alphaMultTerm;

			RedAddTerm = redAddTerm;
			GreenAddTerm = greenAddTerm;
			BlueAddTerm = blueAddTerm;
			AlphaAddTerm = alphaAddTerm;

		}

		public static int GetColorTransformWithAlpha( byte[] data, int offset, out ColorTransformWithAlpha transform ) {
			int offsetBit = 0;

			bool hasAddTerms = Tags.TagUtilities.PickBits( data, offset, offsetBit, 1, false ) != 0;
			offsetBit += 1;

			bool hasMultTerms = Tags.TagUtilities.PickBits( data, offset, offsetBit, 1, false ) != 0;
			offsetBit += 1;

			int fieldBits = (int)Tags.TagUtilities.PickBits( data, offset, offsetBit, 4, false );
			offsetBit += 4;

			int redMultTerm = 0;
			int greenMultTerm = 0;
			int blueMultTerm = 0;
			int alphaMultTerm = 0;

			if ( hasMultTerms ) {
				redMultTerm = Tags.TagUtilities.PickSignedBits( data, offset, offsetBit, fieldBits, false );
				offsetBit += fieldBits;
				greenMultTerm = Tags.TagUtilities.PickSignedBits( data, offset, offsetBit, fieldBits, false );
				offsetBit += fieldBits;
				blueMultTerm = Tags.TagUtilities.PickSignedBits( data, offset, offsetBit, fieldBits, false );
				offsetBit += fieldBits;
				alphaMultTerm = Tags.TagUtilities.PickSignedBits( data, offset, offsetBit, fieldBits, false );
				offsetBit += fieldBits;
			}

			int redAddTerm = 0;
			int greenAddTerm = 0;
			int blueAddTerm = 0;
			int alphaAddTerm = 0;

			if ( hasAddTerms ) {
				redAddTerm = Tags.TagUtilities.PickSignedBits( data, offset, offsetBit, fieldBits, false );
				offsetBit += fieldBits;
				greenAddTerm = Tags.TagUtilities.PickSignedBits( data, offset, offsetBit, fieldBits, false );
				offsetBit += fieldBits;
				blueAddTerm = Tags.TagUtilities.PickSignedBits( data, offset, offsetBit, fieldBits, false );
				offsetBit += fieldBits;
				alphaAddTerm = Tags.TagUtilities.PickSignedBits( data, offset, offsetBit, fieldBits, false );
				offsetBit += fieldBits;
			}

			transform = new ColorTransformWithAlpha( hasAddTerms, hasMultTerms, fieldBits,
				redMultTerm / 256.0, greenMultTerm / 256.0, blueMultTerm / 256.0, alphaMultTerm / 256.0,
				redAddTerm, greenAddTerm, blueAddTerm, alphaAddTerm );

			return (int)Math.Ceiling( offsetBit / 8.0 );
		}
	}
}
