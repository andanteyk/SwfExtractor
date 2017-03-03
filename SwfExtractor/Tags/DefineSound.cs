using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwfExtractor.Tags {

	public class DefineSound : CharacterTag {

		public SoundFormat SoundFormat { get; private set; }
		public int SoundRate { get; private set; }
		public int SoundSize { get; private set; }
		public bool IsStereo { get; private set; }
		public int SampleCount { get; private set; }

		internal DefineSound( byte[] data, int offset )
			: base( data, offset ) {

			SoundFormat = (SoundFormat)TagUtilities.PickBits( data, DataOffset, 0, 4 );
			SoundRate = (int)TagUtilities.PickBits( data, DataOffset, 4, 2 );
			SoundSize = (int)TagUtilities.PickBits( data, DataOffset, 6, 1 );
			IsStereo = TagUtilities.PickBits( data, DataOffset, 7, 1 ) != 0;
			SampleCount = (int)TagUtilities.PickBytes32( data, DataOffset + 1 );

			DataOffset += 5;
		}


		public byte[] ExtractSound() {
			var ret = new byte[Length - ( DataOffset - Offset )];
			Array.Copy( RawData, DataOffset, ret, 0, ret.Length );
			return ret;
		}

		public string GetFileExtension() {
			switch ( SoundFormat ) {
				case Tags.SoundFormat.UncompressedLittleEndian:
				case Tags.SoundFormat.UncompressedNativeEndian:
				case Tags.SoundFormat.ADPCM:
					return ".wav";

				case Tags.SoundFormat.MP3:
					return ".mp3";

				default:
					return ".dat";
			}
		}
	}


	public enum SoundFormat {
		UncompressedNativeEndian = 0,
		ADPCM = 1,
		MP3 = 2,
		UncompressedLittleEndian = 3,
		Nellymoser16k = 4,
		Nellymoser8k = 5,
		Nellymoser = 6,
		Speex = 11,
	}

}
