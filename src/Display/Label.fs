namespace Hink.Widgets.Display

open Fable.Core.JsInterop
open Fable.Import
open Fable.Import.Browser
open Fable.Import.PIXI
open Hink.Core

[<AutoOpen>]
module Label =
    type Label(?x, ?y, ?str) =
        inherit Container()
        let internalText = Text(defaultArg str "", Hink.Theme.Default.TextStyle)

        do
            // Position
            base.x <- defaultArg x 0.
            base.y <- defaultArg y 0.
            base.addChild (internalText) |> ignore

        member this.text
            with get () = internalText.text
            and set (value) = internalText.text <- value
