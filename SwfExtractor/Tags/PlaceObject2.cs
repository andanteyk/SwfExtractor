using SwfExtractor.Structures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwfExtractor.Tags {

	public class PlaceObject2 : CharacterTag {

		public bool HasClipActions { get; private set; }
		public bool HasClipDepth { get; private set; }
		public bool HasName { get; private set; }
		public bool HasRatio { get; private set; }
		public bool HasColorTransform { get; private set; }
		public bool HasMatrix { get; private set; }
		public bool HasCharacter { get; private set; }
		public bool Move { get; private set; }

		public int Depth { get; private set; }

		public Matrix Matrix { get; private set; }
		public ColorTransformWithAlpha ColorTransform { get; private set; }
		public int Ratio { get; private set; }
		public string Name { get; private set; }
		public int ClipDepth { get; private set; }
		// clip actions are not implemented


		internal PlaceObject2( byte[] data, int offset )
			: base( data, offset ) {

			DataOffset -= 2;		// character id

			HasClipActions = TagUtilities.PickBits( data, DataOffset + 0, 0, 1 ) != 0;
			HasClipDepth = TagUtilities.PickBits( data, DataOffset + 0, 1, 1 ) != 0;
			HasName = TagUtilities.PickBits( data, DataOffset + 0, 2, 1 ) != 0;
			HasRatio = TagUtilities.PickBits( data, DataOffset + 0, 3, 1 ) != 0;
			HasColorTransform = TagUtilities.PickBits( data, DataOffset + 0, 4, 1 ) != 0;
			HasMatrix = TagUtilities.PickBits( data, DataOffset + 0, 5, 1 ) != 0;
			HasCharacter = TagUtilities.PickBits( data, DataOffset + 0, 6, 1 ) != 0;
			Move = TagUtilities.PickBits( data, DataOffset + 0, 7, 1 ) != 0;

			Depth = TagUtilities.PickBytes16( data, DataOffset + 1 );

			int index = DataOffset + 3;

			if ( HasCharacter ) {
				CharacterID = TagUtilities.PickBytes16( data, index );
				index += 2;
			} else {
				CharacterID = -1;
			}

			if ( HasMatrix ) {
				Matrix mat;
				index += Matrix.GetMatrix( data, index, out mat );
				Matrix = mat;
			}
			if ( HasColorTransform ) {
				ColorTransformWithAlpha trans;
				index += ColorTransformWithAlpha.GetColorTransformWithAlpha( data, index, out trans );
				ColorTransform = trans;
			}
			if ( HasRatio ) {
				Ratio = TagUtilities.PickBytes16( data, index );
				index += 2;
			}
			if ( HasName ) {
				int n;
				for ( n = index; data[n] != 0; n++ ) ;

				var encoding = Encoding.GetEncoding( 65001 );
				Name = encoding.GetString( data, index, n - index );
				index += n - index + 1;
			}
			if ( HasClipDepth ) {
				ClipDepth = TagUtilities.PickBytes16( data, index );
				index += 2;
			}
			// clip actions are not implemented
		}


	}

}
