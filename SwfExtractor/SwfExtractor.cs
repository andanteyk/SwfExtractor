using SwfExtractor.Structures;
using SwfExtractor.Tags;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwfExtractor {
	public class SwfExtractor {

		public SwfHeader Header { get; private set; }
		public List<SwfTag> Tags { get; private set; }

		public byte[] RawData { get; private set; }

		public SwfExtractor() {
		}


		public void Load( byte[] data ) {

			{
				SwfHeader header;
				RawData = SwfHeader.GetHeader( data, out header );
				Header = header;
			}


			int offset = Header.HeaderLength;

			Tags = new List<SwfTag>();
			while ( true ) {

				SwfTag tag = GetTag( RawData, offset );

				if ( tag == null )
					break;

				Tags.Add( tag );
				offset += tag.Length;
			}
		}


		public static SwfTag GetTag( byte[] data, int offset ) {

			switch ( SwfTag.GetTagCode( data, offset ) ) {

				case TagType.DefineBits:
					return new DefineBits( data, offset );

				case TagType.DefineBitsJPEG2:
					return new DefineBitsJPEG2( data, offset );

				case TagType.DefineBitsJPEG3:
					return new DefineBitsJPEG3( data, offset );

				case TagType.DefineBitsLossless:
					return new DefineBitsLossless( data, offset );

				case TagType.DefineBitsLossless2:
					return new DefineBitsLossless2( data, offset );


				case TagType.PlaceObject2:
					return new PlaceObject2( data, offset );


				case TagType.DefineSprite:
					return new DefineSprite( data, offset );


				case TagType.End:
					return null;

				default:
					return new UndefinedTag( data, offset );
			}

		}

	}
}
