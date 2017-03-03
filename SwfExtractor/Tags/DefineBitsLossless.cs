using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SwfExtractor.Tags {

	public class DefineBitsLossless : ImageTag {

		public BitsLosslessBitmapFormat BitmapFormat { get; private set; }
		public Size BitmapSize { get; private set; }
		public int ColorTableSize { get; private set; }


		internal DefineBitsLossless( byte[] data, int offset )
			: base( data, offset ) {

			BitmapFormat = (BitsLosslessBitmapFormat)TagUtilities.PickBytes( data, DataOffset + 0, 1 );
			BitmapSize = new Size( (int)TagUtilities.PickBytes16( data, DataOffset + 1 ), (int)TagUtilities.PickBytes16( data, DataOffset + 3 ) );

			DataOffset += 5;

			if ( BitmapFormat == BitsLosslessBitmapFormat.Format8bppIndexed ) {
				ColorTableSize = (int)TagUtilities.PickBytes( RawData, DataOffset + 0, 1 ) + 1;
				DataOffset += 1;
			}
		}


		public override System.Drawing.Bitmap ExtractImage() {

			switch ( BitmapFormat ) {

				case BitsLosslessBitmapFormat.Format8bppIndexed: {

						byte[] decompressed = TagUtilities.DecompressDeflate( RawData, DataOffset, DataLength,
							ColorTableSize * 3 + TagUtilities.GetStride( BitmapSize.Width ) * BitmapSize.Height );

						var result = new Bitmap( BitmapSize.Width, BitmapSize.Height, PixelFormat.Format8bppIndexed );
						var palette = result.Palette;
						for ( int i = 0; i < ColorTableSize; i++ ) {
							palette.Entries[i] = Color.FromArgb( decompressed[i * 3 + 0], decompressed[i * 3 + 1], decompressed[i * 3 + 2] );
						}
						result.Palette = palette;

						BitmapData resultData = result.LockBits( new Rectangle( 0, 0, result.Width, result.Height ), ImageLockMode.WriteOnly, PixelFormat.Format8bppIndexed );
						Marshal.Copy( decompressed, ColorTableSize * 3, resultData.Scan0, resultData.Stride * resultData.Height );
						result.UnlockBits( resultData );

						return result;
					}

				case BitsLosslessBitmapFormat.Format16bppRgb: {
						// x1 r5 g5 b5
						// not tested

						byte[] decompressed = TagUtilities.DecompressDeflate( RawData, DataOffset, DataLength, BitmapSize.Width * BitmapSize.Height * 2 );

						var result = new Bitmap( BitmapSize.Width, BitmapSize.Height, PixelFormat.Format16bppRgb555 );
						var resultData = result.LockBits( new Rectangle( 0, 0, result.Width, result.Height ), ImageLockMode.WriteOnly, PixelFormat.Format16bppRgb555 );

						byte[] resultCanvas = new byte[resultData.Stride * resultData.Height];
						for ( int i = 0; i < resultData.Stride * resultData.Height; i += 2 ) {
							resultCanvas[i + 1] = decompressed[i + 0];
							resultCanvas[i + 0] = decompressed[i + 1];
						}

						Marshal.Copy( resultCanvas, 0, resultData.Scan0, resultCanvas.Length );
						result.UnlockBits( resultData );

						return result;
					}

				case BitsLosslessBitmapFormat.Format32bppRgb: {
						// x8 r8 g8 b8

						byte[] decompressed = TagUtilities.DecompressDeflate( RawData, DataOffset, DataLength, BitmapSize.Width * BitmapSize.Height * 4 );

						var result = new Bitmap( BitmapSize.Width, BitmapSize.Height, PixelFormat.Format32bppRgb );
						var resultData = result.LockBits( new Rectangle( 0, 0, result.Width, result.Height ), ImageLockMode.WriteOnly, PixelFormat.Format32bppRgb );

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


	public enum BitsLosslessBitmapFormat {
		Format8bppIndexed = 3,
		Format16bppRgb = 4,
		Format32bppRgb = 5,
	}
}
