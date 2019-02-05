using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwfExtractor.Tags {
    public class DefineBinaryData : CharacterTag {

        public DefineBinaryData(byte[] data, int offset) : base(data, offset) {
            DataOffset += 4;    //32bit reserved
        }

        public byte[] ExtractData() {
            var ret = new byte[Length - (DataOffset - Offset)];
            Array.Copy(RawData, DataOffset, ret, 0, ret.Length);
            return ret;
        }
    }
}
