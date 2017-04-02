namespace Hink.Widgets.Interactive

open Fable.Core.JsInterop
open Fable.Import
open Fable.Import.Browser
open Fable.Import.PIXI
open Hink.Core

[<AutoOpen>]
module Checkbox =

  type CheckboxStateChange =
    {
      Sender: Checkbox
      NewState: bool
      Event: InteractionEvent option
    }

    static member Create (sender, ?event) =
      {
        Sender = sender
        NewState = sender.State
        Event = event
      }

  and Checkbox() as self =
    inherit Widget()
    let onStateChange = new Event<CheckboxStateChange>()
    let drawBackground (g: Graphics) color =
      g
        .beginFill(float color)
        .drawRoundedRect(0., 0., 22., 22., 3.)
        .endFill()
        |> ignore

    let background = Graphics()

    let drawStandardBrackground () =
      drawBackground background 0x1ABC9C

    let drawHoverBrackground () =
      drawBackground background 0x16A085

    let tick = PIXI.Text("", Hink.Theme.Default.TextStyle)
    let mutable active = false
    let updateTickText () =
        if active then
          tick.text <- "\u2713"
        else
          tick.text <- ""

    do
      let container = Container()

      drawStandardBrackground ()
      tick.anchor <- Point(0.5, 0.5)
      tick.x <- 22. / 2.
      tick.y <- 22. / 2.
      container.addChild(background, tick) |> ignore
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
        drawStandardBrackground ()
        active <- not active

        updateTickText()
        onStateChange.Trigger(CheckboxStateChange.Create(self, ev))

        self.UI.removeAllListeners("mouseout") |> ignore
      ) |> ignore

    member self.StateChange = onStateChange.Publish

    member self.State
      with get () = active
      and set (value) =
        active <- value
        updateTickText()
        onStateChange.Trigger(CheckboxStateChange.Create(self))
