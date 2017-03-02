using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwfExtractor.Tags {
	
	public class DefineBits : ImageTag {

		public DefineBits( byte[] data, int offset )
			: base( data, offset ) {
		}

		public override System.Drawing.Bitmap ExtractImage() {
			return BitmapFromBytes( RawData, DataOffset, DataLength );
		}
	}
}
