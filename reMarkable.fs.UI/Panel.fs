namespace reMarkable.fs.UI.Controls

open System.Collections
open System.Collections.Generic
open reMarkable.fs.UI
open SixLabors.ImageSharp
open SixLabors.ImageSharp.Drawing.Processing
open SixLabors.ImageSharp.PixelFormats
open SixLabors.ImageSharp.Processing


type Panel() =
    inherit Control()
    
    let controls = List<Control>()
   

    member _.Count = controls.Count
    member _.GetEnumerator() = controls.GetEnumerator() :> IEnumerator
    
    interface IEnumerable<Control> with
        member this.GetEnumerator() = (controls :> IEnumerable<Control>).GetEnumerator()
        member this.GetEnumerator() = (controls :> IEnumerable).GetEnumerator()

    //member _.IsReadOnly: bool = controls.IsReadOnly

    interface IPanel with
        member this.Window
            with get() = base.Window
            and set(value) = base.Window <- value
            
        member this.Bounds
            with get() = base.Bounds
            and set(value) = base.Bounds <- value
        member this.Draw(buffer) = this.Draw(buffer)
        member this.LayoutControls() = this.LayoutControls()


    abstract member Add: Control -> unit
    default this.Add(item: Control): unit =
        item.Parent <- this :> Control |> Some
        item.Window <- this.Window;
        controls.Add(item)

    member _.Clear() = controls.Clear()

//    public bool Contains(Control item)
//        return _controls.Contains(item);
//
//    public void CopyTo(Control[] array, int arrayIndex)
//        foreach (var item in array)
//            if (item != null)
//                item.Parent = this;
//                item.Window = Window;
//
//        _controls.CopyTo(array, arrayIndex);
//
//    public bool Remove(Control item)
//    {
//        return _controls.Remove(item);
//    }
//
//    public int IndexOf(Control item)
//    {
//        return _controls.IndexOf(item);
//    }
//
//    public void Insert(int index, Control item)
//        if (item != null)
//            item.Parent = this;
//            item.Window = Window;
//        
//        _controls.Insert(index, item);
//
//    public void RemoveAt(int index)
//        _controls.RemoveAt(index);
//
//    public Control this[int index]
//        get => _controls[index];
//        set
//            if (value != null)
//                value.Parent = this;
//                value.Window = Window;
//            _controls[index] = value;
//
    override this.Draw(buffer: Image<Rgb24>): unit =
        if this.BackgroundColor <> Color.Transparent then
            buffer.Mutate(fun (g: IImageProcessingContext) ->
                g.Fill(this.BackgroundColor, this.Bounds)
                |> ignore
            )
        
        controls
        |> Seq.iter(fun control -> control.Draw(buffer))

    abstract member LayoutControls: unit -> unit
    default _.LayoutControls() = ()