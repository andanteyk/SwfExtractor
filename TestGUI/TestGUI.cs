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

			string filename = ( (string[])e.Data.GetData( DataFormats.FileDrop ) )[0];

			SwfExtractor.SwfExtractor swf = new SwfExtractor.SwfExtractor();

			using ( var reader = new FileStream( filename, FileMode.Open, FileAccess.Read, FileShare.Read ) ) {
				var bin = new byte[reader.Length];
				reader.Read( bin, 0, (int)reader.Length );
				swf.Load( bin );

				{
					var img = swf.Tags.OfType<SwfExtractor.Tags.DefineBitsJPEG3>().FirstOrDefault();
					if ( img != null ) {
						var image = img.ExtractImage();
						pictureBox1.Image = image;
					}
				}

				foreach ( var img in swf.Tags.OfType<SwfExtractor.Tags.ImageTag>() ) {
					img.ExtractImage().Save( img.CharacterID + ".png", System.Drawing.Imaging.ImageFormat.Png );
				}
			}
		}
	}
}
