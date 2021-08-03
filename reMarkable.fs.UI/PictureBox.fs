namespace reMarkable.fs.UI.Controls

type PictureBox = class end

// public class PictureBox : Control
//     private Image<Rgb24> _image;
//
//     public PictureBox() { }
//
//     public Image<Rgb24> Source
//     {
//         get => _image;
//         set {
//             _image = value;
//             RedrawWithChange(() => _image = value);
//         }
//     }
//
//     protected override RectangleF GetMinimumRedrawRect()
//         var imageRect = new RectangleF(0, 0, _image.Size().Width, _image.Size().Height);
//         imageRect.Align(Bounds, TextAlign);
//         return imageRect;
//
//     public override void Draw(Image<Rgb24> buffer)
//         var rect = GetMinimumRedrawRect();
//         buffer.Mutate(g => g.DrawImage(_image, PointExtensions.toInteger(rect.Location), 1));

