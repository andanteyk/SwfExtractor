using SwfExtractor.Structures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwfExtractor.Tags {

	public class DefineShape : CharacterTag {

		public Rectangle Bounds { get; private set; }

		// other params are not implemented

		internal DefineShape( byte[] data, int offset )
			: base( data, offset ) {

			{
				Rectangle rect;
				DataOffset += Rectangle.GetRectangle( data, DataOffset, out rect );
				Bounds = rect;
			}

		}


	}
}
