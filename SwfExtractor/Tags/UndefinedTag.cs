using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwfExtractor.Tags {

	public class UndefinedTag : SwfTag {

		public UndefinedTag( byte[] data, int offset )
			: base( data, offset ) {
		}
	}
}
