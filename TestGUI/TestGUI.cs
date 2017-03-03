using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SwfExtractor;
using System.IO;
using SwfExtractor.Tags;

namespace TestGUI {
	public partial class TestGUI : Form {
		public TestGUI() {
			InitializeComponent();
		}

		private void TabExtract_DragEnter( object sender, DragEventArgs e ) {

			if ( e.Data.GetDataPresent( DataFormats.FileDrop ) )
				e.Effect = DragDropEffects.Copy;
			else
				e.Effect = DragDropEffects.None;

		}

		private void TabExtract_DragDrop( object sender, DragEventArgs e ) {

			bool mapAnalysis = false;


			string filename = ( (string[])e.Data.GetData( DataFormats.FileDrop ) )[0];

			try {

				SwfParser swf = new SwfParser();
				swf.Parse( filename );


				foreach ( var img in swf.FindTags<ImageTag>() ) {
					img.ExtractImage().Save( img.CharacterID + ".png", System.Drawing.Imaging.ImageFormat.Png );
				}

				foreach ( var sound in swf.FindTags<DefineSound>() ) {
					using ( var writer = new FileStream( sound.CharacterID + sound.GetFileExtension(), FileMode.Create, FileAccess.Write, FileShare.Write ) ) {
						var dat = sound.ExtractSound();
						writer.Write( dat, 0, dat.Length );
					}
				}



				if ( mapAnalysis ) {	// kancolle mapdata analysis

					var imagetags = swf.FindTags<ImageTag>();
					Bitmap map = new Bitmap( 768, 435 );
					bool loaded = false;

					using ( var g = Graphics.FromImage( map ) ) {


						foreach ( var it in imagetags ) {
							var img = it.ExtractImage();
							if ( img.Width == map.Width && img.Height == map.Height ) {
								g.DrawImage( img, 0, 0 );
								loaded = true;
							}
							img.Dispose();
						}

						if ( !loaded )
							throw new InvalidOperationException( "this data is not map" );



						var placeobjects = swf.FindTags<PlaceObject2>();

						for ( int i = 0; ; i++ ) {

							string name = "line" + i;

							var linetag = placeobjects.FirstOrDefault( t => t.HasName && t.Name == name );

							if ( linetag == null )
								break;

							var pos = new PointF( linetag.Matrix.TranslateX, linetag.Matrix.TranslateY );

							if ( pos.X < 0 ) pos.X = 0;
							else if ( pos.X >= map.Width ) pos.X = map.Width - 1;
							if ( pos.Y < 0 ) pos.Y = 0;
							else if ( pos.Y >= map.Height ) pos.Y = map.Height - 1;

							map.SetPixel( (int)pos.X, (int)pos.Y, Color.Black );
							g.DrawString( i.ToString(), Font, Brushes.White, new PointF( pos.X + 1, pos.Y + 1 ) );
							g.DrawString( i.ToString(), Font, Brushes.Black, pos );

							var parentSprite = swf.FindTags<DefineSprite>().FirstOrDefault( t => t.CharacterID == linetag.CharacterID );
							if ( parentSprite != null ) {
								var innerplace = parentSprite.FindTags<PlaceObject2>().FirstOrDefault();
								if ( innerplace != null ) {
									var shape = swf.FindTags<DefineShape>().FirstOrDefault( t => t.CharacterID == innerplace.CharacterID );
									if ( shape != null ) {
										var rect =  shape.Bounds.ToPositiveRectangle();
										g.DrawRectangle( Pens.Orange, linetag.Matrix.TranslateX + rect.X, linetag.Matrix.TranslateY + rect.Y, rect.Width, rect.Height );
									}
								}
							}
						}
					}

					if ( pictureBox1.Image != null ) {
						var img = pictureBox1.Image;
						pictureBox1.Image = null;
						img.Dispose();
					}
					pictureBox1.Image = map;

				}

			} catch ( Exception ex ) {

				MessageBox.Show( ex.Message + "\r\n" + ex.StackTrace, ex.GetType().Name, MessageBoxButtons.OK, MessageBoxIcon.Error );
			}

		}
	}
}
