namespace Hink.Widgets.Display

open Fable.Core.JsInterop
open Fable.Import
open Fable.Import.Browser
open Fable.Import.PIXI
open Hink.Core

[<AutoOpen>]
module Label =

  type Label(str) as self =
    inherit Widget()

    let text = Text(str, Hink.Theme.Default.TextStyle)

    do
      let container = Container()

      container.addChild(text) |> ignore
      self.UI <- container

    member self.Text
      with get () = text.text
      and set(value) =
        text.text <- value
