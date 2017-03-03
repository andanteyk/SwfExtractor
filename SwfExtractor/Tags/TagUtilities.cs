using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwfExtractor.Tags {

	internal static class TagUtilities {

		// undone: optimize

		/// <summary>
		/// バイト配列から符号なし整数を切り出します。
		/// </summary>
		/// <param name="data">バイト配列。</param>
		/// <param name="offsetByte">先頭からのオフセット(バイト単位)。</param>
		/// <param name="offsetBit">オフセット(ビット単位)。</param>
		/// <param name="lengthBit">切り出す長さ(ビット単位)。[1-32]</param>
		/// <param name="isLittleEndian">リトルエンディアンであれば true 、ビッグエンディアンであれば false 。既定値は false です。</param>
		/// <returns>切り出した値。</returns>
		public static uint PickBits( byte[] data, int offsetByte, int offsetBit, int lengthBit, bool isLittleEndian = true ) {
			if ( lengthBit > 32 )
				throw new ArgumentOutOfRangeException();

			uint ret = 0;
			int len = (int)Math.Ceiling( ( offsetBit + lengthBit ) / 8.0 );


			for ( int i = 0; i < lengthBit; i++ ) {
				int index;
				if ( isLittleEndian )
					index = offsetByte + len - 1 - ( offsetBit + i ) / 8;
				else
					index = offsetByte + ( offsetBit + i ) / 8;

				if ( ( data[index] & ( 1 << ( 7 - ( i + offsetBit ) % 8 ) ) ) != 0 ) {
					ret |= 1u << ( 31 - i );
				}
			}

			return ret >> ( 32 - lengthBit );
		}

		public static uint PickBytes( byte[] data, int offsetByte, int lengthByte, bool isLittleEndian = true ) {
			if ( lengthByte > 4 )
				throw new ArgumentOutOfRangeException();

			uint ret = 0;
			for ( int i = 0; i < lengthByte; i++ ) {
				uint value = data[offsetByte + i];

				if ( isLittleEndian )
					value <<= i * 8;
				else
					value <<= ( lengthByte - 1 - i ) * 8;

				ret |= value;
			}

			return ret;
		}

		public static ushort PickBytes16( byte[] data, int offsetByte, bool isLittleEndian = true ) {
			return (ushort)PickBytes( data, offsetByte, 2, isLittleEndian );
		}

		public static uint PickBytes32( byte[] data, int offsetByte, bool isLittleEndian = true ) {
			return PickBytes( data, offsetByte, 4, isLittleEndian );
		}


		public static int PickSignedBits( byte[] data, int offsetByte, int offsetBit, int lengthBit, bool isLittleEndian = true ) {
			int ret = (int)PickBits( data, offsetByte, offsetBit, lengthBit, isLittleEndian );
			if ( ( ret & ( 1 << ( lengthBit - 1 ) ) ) != 0 )		// msb == 1; minus;
				ret |= ~0 << ( lengthBit - 1 );						// fill by 1
			return ret;
		}


		/// <summary>
		/// 4バイト境界に合わせた幅を返します。
		/// </summary>
		public static int GetStride( int width ) {
			return ( width + 3 ) & -4;
		}



		public static byte[] DecompressDeflate( byte[] data, int index, int length, int outSize ) {

			using ( var stream = new MemoryStream( data, index + 2, length - 2, false ) ) {
				using ( var deflate = new DeflateStream( stream, CompressionMode.Decompress ) ) {

					if ( outSize > 0 ) {
						byte[] ret = new byte[outSize];

						if ( deflate.Read( ret, 0, ret.Length ) < ret.Length )
							throw new ArgumentException();

						return ret;

					} else {

						List<byte> ret = new List<byte>();
						byte[] buffer = new byte[1024];

						while ( true ) {
							int read = deflate.Read( buffer, 0, buffer.Length );
							if ( read < buffer.Length ) {
								ret.AddRange( buffer.Take( read ) );
								break;
							}
							ret.AddRange( buffer );
						}

						return ret.ToArray();
					}

				}
			}

		}

	}
}
