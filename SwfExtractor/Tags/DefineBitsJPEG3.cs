using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SwfExtractor.Tags {

	public class DefineBitsJPEG3 : ImageTag {

		private static readonly byte[] PNGHeader = new byte[] { 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A };
		private static readonly byte[] GIFHeader = new byte[] { 0x47, 0x49, 0x46, 0x38, 0x39, 0x61 };

		public int ImageDataSize { get; private set; }

		public DefineBitsJPEG3( byte[] data, int offset )
			: base( data, offset ) {

			ImageDataSize = (int)TagUtilities.PickBytes32( data, DataOffset );
			DataOffset += 4;
		}


		public override System.Drawing.Bitmap ExtractImage() {
			byte[] imageData = new byte[ImageDataSize];
			Array.Copy( RawData, DataOffset, imageData, 0, imageData.Length );

			if ( imageData.Select( ( v, i ) => ( v == PNGHeader[i] ) ).All( b => b ) ||
				imageData.Select( ( v, i ) => ( v == GIFHeader[i] ) ).All( b => b ) ) {
				// png or gif

				return BitmapFromBytes( imageData, 0, imageData.Length );


			} else {
				// jpeg with alpha

				using ( var imageStream = new MemoryStream( imageData ) ) {
					using ( var rawjpeg = new Bitmap( imageStream ) ) {

						BitmapData rawjpegdata = rawjpeg.LockBits( new Rectangle( 0, 0, rawjpeg.Width, rawjpeg.Height ), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb );
						byte[] rawjpegCanvas = new byte[rawjpegdata.Stride * rawjpeg.Height];
						Marshal.Copy( rawjpegdata.Scan0, rawjpegCanvas, 0, rawjpegCanvas.Length );

						byte[] alphaCanvas = TagUtilities.DecompressDeflate( RawData, DataOffset + ImageDataSize, DataLength - ImageDataSize, rawjpeg.Width * rawjpeg.Height );

						var result = new Bitmap( rawjpeg.Width, rawjpeg.Height, PixelFormat.Format32bppPArgb );
						BitmapData resultData = result.LockBits( new Rectangle( 0, 0, result.Width, result.Height ), ImageLockMode.WriteOnly, PixelFormat.Format32bppPArgb );
						byte[] resultCanvas = new byte[resultData.Stride * resultData.Height];

						int width = rawjpegdata.Width;
						int width_in = rawjpegdata.Stride;
						int width_out = resultData.Stride;
						int height = rawjpegdata.Height;

						for ( int y = 0; y < height; y++ ) {
							for ( int x = 0; x < width; x++ ) {
								int i = y * width_out + x * 4;
								int j = y * width_in + x * 3;

								resultCanvas[i + 3] = alphaCanvas[y * width + x];
								resultCanvas[i + 2] = rawjpegCanvas[j + 2];
								resultCanvas[i + 1] = rawjpegCanvas[j + 1];
								resultCanvas[i + 0] = rawjpegCanvas[j + 0];

							}
						}

						Marshal.Copy( resultCanvas, 0, resultData.Scan0, resultCanvas.Length );
						result.UnlockBits( resultData );

						return result;

					}

				}
			}
		}
	}
}
