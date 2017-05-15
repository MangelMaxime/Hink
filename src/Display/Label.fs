namespace Hink.Widgets.Display

open Fable.Core.JsInterop
open Fable.Import
open Fable.Import.Browser
open Fable.Import.PIXI
open Hink.Core

[<AutoOpen>]
module Label =

  type Label(?x, ?y, ?str) as self =
    inherit Container()

    let internalText = Text(defaultArg str "", Hink.Theme.Default.TextStyle)

    do
      // Position
      self.x <- defaultArg x 0.
      self.y <- defaultArg y 0.
      self.addChild(internalText) |> ignore

    member self.text
      with get () = internalText.text
      and set(value) =
        internalText.text <- value
