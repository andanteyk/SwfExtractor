using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace SwfExtractor.Tags {

	public class DefineSprite : CharacterTag {

		public int FrameCount { get; private set; }

		private List<SwfTag> _controlTags { get; set; }
		public ReadOnlyCollection<SwfTag> ControlTags { get { return _controlTags.AsReadOnly(); } }

		internal DefineSprite( byte[] data, int offset )
			: base( data, offset ) {

			FrameCount = TagUtilities.PickBytes16( data, DataOffset + 0 );
			DataOffset += 2;
			

			_controlTags = new List<SwfTag>();


			int index = DataOffset;
			while ( index <= Offset + Length ) {
				var tag = SwfParser.GetTag( data, index );
				if ( tag == null )
					break;
				_controlTags.Add( tag );
				index += tag.Length;
			}
		}


		public IEnumerable<T> FindTags<T>() where T : SwfTag {
			return FindTagsInteral<T>( ControlTags );
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
	}
}
