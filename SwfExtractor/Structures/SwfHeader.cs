using SwfExtractor.Tags;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwfExtractor.Structures {

	public class SwfHeader {


		public SwfCompression Compression { get; private set; }
		public byte Version { get; private set; }
		public int FileLength { get; private set; }

		public Rectangle FrameSize { get; private set; }
		public ushort FrameRate { get; private set; }
		public ushort FrameCount { get; private set; }

		public int HeaderLength { get; private set; }



		public SwfHeader() {
		}


		private byte[] Decompress( byte[] rawData ) {

			switch ( Compression ) {

				case SwfCompression.Uncompressed:
					return rawData.ToArray();

				case SwfCompression.Deflate: {

					using ( var bytes = new MemoryStream( rawData, 8 + 2, rawData.Length - 10, false ) ) {
							using ( var decomp = new DeflateStream( bytes, CompressionMode.Decompress ) ) {

								byte[] buffer = new byte[FileLength];
								Array.Copy( rawData, buffer, 8 );

								decomp.Read( buffer, 8, FileLength - 8 );

								return buffer;
							}
						}
					}

				case SwfCompression.LZMA: 
					{
						// see https://helpx.adobe.com/flash-player/kb/exception-thrown-you-decompress-lzma-compressed.html
						byte[] properties = new byte[5];
						Array.Copy( rawData, 12, properties, 0, 5 );
						var decoder = new SevenZip.Compression.LZMA.Decoder();
						decoder.SetDecoderProperties( properties );

						byte[] result = new byte[FileLength];
						Array.Copy( rawData, result, 8 );

						using ( var instream = new MemoryStream( rawData, 17, rawData.Length - 17 ) ) {
							using ( var outstream = new MemoryStream( result, 8, result.Length - 8 ) ) {
								decoder.Code( instream, outstream, instream.Length, outstream.Length, null );
							}
						}

						return result;
					}

				default:
					throw new ArgumentException( "Unknown Compression mode" );
			}

		}

		
		public static byte[] GetHeader( byte[] rawData, out SwfHeader header ) {

			int offset = 0;
			header = new SwfHeader();

			if ( rawData.Length - offset < 8 )
				throw new ArgumentException( "Data Length is less than 8 bytes" );

			header.Compression = (SwfCompression)rawData[offset + 0];

			if ( !( Enum.GetValues( typeof( SwfCompression ) ).Cast<SwfCompression>().Contains( header.Compression ) &&
				rawData[offset + 1] == 0x57 && rawData[offset + 2] == 0x53 ) )
				throw new ArgumentException( "This Data is not SWF" );

			header.Version = rawData[offset + 3];
			header.FileLength = (int)TagUtilities.PickBytes( rawData, offset + 4, 4 );


			rawData = header.Decompress( rawData );
			offset += 8;

			Rectangle rect;
			offset += Rectangle.GetRectangle( rawData, offset, out rect );
			header.FrameSize = rect;

			header.FrameRate = TagUtilities.PickBytes16( rawData, offset );
			offset += 2;
			header.FrameCount = TagUtilities.PickBytes16( rawData, offset );
			offset += 2;

			header.HeaderLength = offset;

			return rawData;
		}
	}


	public enum SwfCompression {
		Uncompressed = 0x46,
		Deflate = 0x43,
		LZMA = 0x5a,
	}
}
