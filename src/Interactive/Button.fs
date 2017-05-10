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

  and Button( ?x: float, ?y: float,
              ?width: float, ?height: float,
              ?str: string) as self =
    inherit Container()

    let onClick = new Event<ButtonOnClick>()
    let drawBackground (g: Graphics) color=
      g
        .clear()
        .beginFill(float color)
        .drawRoundedRect(0., 0., 80., 34., 4.)
        .endFill()
        |> ignore

    let background = Graphics()

    let drawStandardBrackground () =
      drawBackground background 0x1ABC9C

    let drawHoverBrackground () =
      drawBackground background 0x16A085

    let internalText = Text(defaultArg str "Button", Hink.Theme.Default.TextStyle)

    do
      // Position
      self.x <- defaultArg x 0.
      self.y <- defaultArg y 0.
      // Size
      self.width <- defaultArg width 80.
      self.height <- defaultArg height 80.
      // Interactive
      self.interactive <- true
      self.buttonMode <- true

      drawStandardBrackground ()

      internalText.anchor <- Point(0.5, 0.5)
      internalText.x <- 80. / 2.
      internalText.y <- 34. / 2.
      self.addChild(background, internalText) |> ignore

      let resetBackground () =
        self.once_mouseout(fun _ ->
          drawStandardBrackground ()
        ) |> ignore

      self.on_mouseover(fun _ ->
        drawBackground background 0x48c9b0
        resetBackground()
      ) |> ignore

      self.on_mousedown(fun _ ->
        drawHoverBrackground ()
        resetBackground()
      ) |> ignore

      self.on_mouseup(fun ev ->
        onClick.Trigger(ButtonOnClick.Create(self, ev))
        drawStandardBrackground ()
        self.removeAllListeners("mouseout") |> ignore
      ) |> ignore

    interface IClickable<ButtonOnClick> with
      member this.OnClick = onClick.Publish

    member self.text
      with get () = internalText.text
      and set(value) =
        internalText.text <- value
