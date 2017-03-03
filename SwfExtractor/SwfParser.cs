using SwfExtractor.Structures;
using SwfExtractor.Tags;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwfExtractor {
	public class SwfParser {

		public SwfHeader Header { get; private set; }
		public List<SwfTag> Tags { get; private set; }

		public byte[] RawData { get; private set; }

		public SwfParser() {
		}


		public void Parse( byte[] data ) {

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


		public void Parse( string path ) {
			using ( var reader = new FileStream( path, FileMode.Open, FileAccess.Read, FileShare.Read ) ) {
				var bin = new byte[reader.Length];
				reader.Read( bin, 0, (int)reader.Length );
				Parse( bin );
			}
		}


		public IEnumerable<T> FindTags<T>() where T : SwfTag {
			return FindTagsInteral<T>( Tags );
		}

		private IEnumerable<T> FindTagsInteral<T>( IEnumerable<SwfTag> tags ) where T : SwfTag {
			foreach ( var tag in tags ) {

				if ( tag is T )
					yield return tag as T;

				if ( tag is DefineSprite ) {
					foreach ( var child in FindTagsInteral<T>( ( tag as DefineSprite ).ControlTags ) )
						yield return child;
				}
			}
		}


		internal static SwfTag GetTag( byte[] data, int offset ) {

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


				case TagType.DefineSound:
					return new DefineSound( data, offset );


				case TagType.PlaceObject2:
					return new PlaceObject2( data, offset );

				case TagType.DefineShape:
					return new DefineShape( data, offset );


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
