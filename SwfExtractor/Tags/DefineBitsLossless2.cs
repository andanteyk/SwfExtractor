using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SwfExtractor.Tags {

	public class DefineBitsLossless2 : ImageTag {

		public BitsLossless2BitmapFormat BitmapFormat { get; private set; }
		public Size BitmapSize { get; private set; }
		public int ColorTableSize { get; private set; }


		public DefineBitsLossless2( byte[] data, int offset )
			: base( data, offset ) {

			BitmapFormat = (BitsLossless2BitmapFormat)TagUtilities.PickBytes( data, DataOffset + 0, 1 );
			BitmapSize = new Size( (int)TagUtilities.PickBytes16( data, DataOffset + 1 ), (int)TagUtilities.PickBytes16( data, DataOffset + 3 ) );

			DataOffset += 5;

			if ( BitmapFormat == BitsLossless2BitmapFormat.Format8bppIndexed ) {
				ColorTableSize = (int)TagUtilities.PickBytes( data, DataOffset, 1 ) + 1;
				DataOffset += 1;
			}
		}


		public override System.Drawing.Bitmap ExtractImage() {

			switch ( BitmapFormat ) {

				case BitsLossless2BitmapFormat.Format8bppIndexed: {

						byte[] decompressed = TagUtilities.DecompressDeflate( RawData, DataOffset, DataLength,
							ColorTableSize * 4 + TagUtilities.GetStride( BitmapSize.Width ) * BitmapSize.Height );

						var result = new Bitmap( BitmapSize.Width, BitmapSize.Height, PixelFormat.Format8bppIndexed );
						var palette = result.Palette;
						for ( int i = 0; i < ColorTableSize; i++ ) {
							// 乗算済みカラーを通常色に戻す
							palette.Entries[i] = Color.FromArgb( decompressed[i * 4 + 3],
								decompressed[i * 4 + 3] == 0 ? 0 : decompressed[i * 4 + 0] * 255 / decompressed[i * 4 + 3],
								decompressed[i * 4 + 3] == 0 ? 0 : decompressed[i * 4 + 1] * 255 / decompressed[i * 4 + 3],
								decompressed[i * 4 + 3] == 0 ? 0 : decompressed[i * 4 + 2] * 255 / decompressed[i * 4 + 3] );
						}
						result.Palette = palette;

						BitmapData resultData = result.LockBits( new Rectangle( 0, 0, result.Width, result.Height ), ImageLockMode.WriteOnly, PixelFormat.Format8bppIndexed );
						Marshal.Copy( decompressed, ColorTableSize * 4, resultData.Scan0, resultData.Stride * resultData.Height );
						result.UnlockBits( resultData );

						return result;
					}

				case BitsLossless2BitmapFormat.Format32bppArgb: {

						byte[] decompressed = TagUtilities.DecompressDeflate( RawData, DataOffset, DataLength, BitmapSize.Width * BitmapSize.Height * 4 );

						var result = new Bitmap( BitmapSize.Width, BitmapSize.Height, PixelFormat.Format32bppPArgb );
						var resultData = result.LockBits( new Rectangle( 0, 0, result.Width, result.Height ), ImageLockMode.WriteOnly, PixelFormat.Format32bppPArgb );

						byte[] resultCanvas = new byte[resultData.Stride * resultData.Height];
						for ( int i = 0; i < resultData.Stride * resultData.Height; i += 4 ) {
							resultCanvas[i + 3] = decompressed[i + 0];
							resultCanvas[i + 2] = decompressed[i + 1];
							resultCanvas[i + 1] = decompressed[i + 2];
							resultCanvas[i + 0] = decompressed[i + 3];
						}

						Marshal.Copy( resultCanvas, 0, resultData.Scan0, resultCanvas.Length );
						result.UnlockBits( resultData );

						return result;
					}

				default:
					throw new NotSupportedException();
			}
		}
	}

	public enum BitsLossless2BitmapFormat {
		Format8bppIndexed = 3,
		Format32bppArgb = 5,
	}
}
