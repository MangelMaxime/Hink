namespace Hink.Widgets.Interactive

open Fable.Core.JsInterop
open Fable.Import
open Fable.Import.Browser
open Fable.Import.PIXI
open Hink.Core

[<AutoOpen>]
module Button =

  type ButtonOnClick =
    {
      Sender: Button
      Event: InteractionEvent
    }

    static member Create (sender, event) =
      {
        Sender = sender
        Event = event
      }

  and Button(str) as self =
    inherit Widget()
    let onClick = new Event<Button * InteractionEvent>()
    let drawBackground (g: Graphics) color=
      g
        .beginFill(float color)
        .drawRoundedRect(0., 0., 80., 34., 4.)
        .endFill()
        |> ignore

    let background = Graphics()

    let drawStandardBrackground () =
      drawBackground background 0x1ABC9C

    let drawHoverBrackground () =
      drawBackground background 0x16A085

    let text = Text(str, Hink.Theme.Default.TextStyle)

    do
      let container = Container()

      drawStandardBrackground ()

      text.anchor <- Point(0.5, 0.5)
      text.x <- 80. / 2.
      text.y <- 34. / 2.
      container.addChild(background, text) |> ignore
      container.interactive <- true
      self.UI <- container

      let resetBackground () =
        self.UI.once_mouseout(fun _ ->
          drawStandardBrackground ()
        ) |> ignore

      self.UI.on_mouseover(fun _ ->
        drawBackground background 0x48c9b0
        resetBackground()
      ) |> ignore

      self.UI.on_mousedown(fun _ ->
        drawHoverBrackground ()
        resetBackground()
      ) |> ignore

      self.UI.on_mouseup(fun ev ->
        onClick.Trigger(self, ev)
        drawStandardBrackground ()
        self.UI.removeAllListeners("mouseout") |> ignore
      ) |> ignore

    member self.Text
      with get () = text.text
      and set(value) =
        text.text <- value

    member self.OnClick = onClick.Publish
