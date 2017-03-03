using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwfExtractor.Tags {

	public abstract class ImageTag : CharacterTag {

		internal ImageTag( byte[] data, int offset )
			: base( data, offset ) {
		}

		public abstract Bitmap ExtractImage();


		protected Bitmap BitmapFromBytes( byte[] bytes, int index, int length ) {
			byte[] data = new byte[length];
			Array.Copy( bytes, index, data, 0, length );
			return System.ComponentModel.TypeDescriptor.GetConverter( typeof( Bitmap ) ).ConvertFrom( data ) as Bitmap;
		}

	}
}
