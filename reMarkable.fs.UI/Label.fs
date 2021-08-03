namespace reMarkable.fs.UI.Controls

open SixLabors.Fonts
open SixLabors.ImageSharp
open SixLabors.ImageSharp.PixelFormats
open reMarkable.fs.UI

type Label(text: string, font: Font) as this =
    inherit Control()
    
    do
        this.Font <- font
        this.Text <- text
        this.Size <-
            let r = this.GetMinimumRedrawRect()
            SizeF(r.Width, r.Height)

    override this.Draw(buffer: Image<Rgb24>) =
        this.DrawString(buffer, this.Text, this.Bounds, this.TextAlign)

    override this.GetMinimumRedrawRect(): RectangleF =
        this.MeasureString(this.Text, this.Bounds, this.TextAlign)
