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

		public DefineSprite( byte[] data, int offset )
			: base( data, offset ) {

			FrameCount = TagUtilities.PickBytes16( data, DataOffset + 0 );
			DataOffset += 2;
			

			_controlTags = new List<SwfTag>();


			int index = DataOffset;
			while ( index <= Offset + Length ) {
				var tag = SwfExtractor.GetTag( data, index );
				if ( tag == null )
					break;
				_controlTags.Add( tag );
				index += tag.Length;
			}
		}

	}
}
