using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwfExtractor.Tags {

		[System.Diagnostics.DebuggerDisplay( "[{TagCode}] Offset: {Offset}, Length: {Length}, CharacterID: {CharacterID}" )]
	public abstract class CharacterTag : SwfTag {

		public int CharacterID { get; protected set; }

		public CharacterTag( byte[] data, int offset )
			: base( data, offset ) {

			CharacterID = TagUtilities.PickBytes16( data, DataOffset );
			DataOffset += 2;
		}

	}
}
