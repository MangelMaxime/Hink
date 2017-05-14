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
        NewState = sender.state
        Event = event
      }

  and Checkbox( ?x: float, ?y: float,
                ?width: float, ?height: float,
                ?state: bool,
                ?onStateChange: CheckboxStateChange -> unit) as self =
    inherit Container()

    let onStateChangeEvent = new Event<CheckboxStateChange>()
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
      // Position
      self.x <- defaultArg x 0.
      self.y <- defaultArg y 0.
      // Size
      self.width <- defaultArg width 22.
      self.height <- defaultArg height 22.
      // Interactive
      self.interactive <- true
      self.buttonMode <- true

      drawStandardBrackground ()

      tick.anchor <- Point(0.5, 0.5)
      tick.x <- 22. / 2.
      tick.y <- 22. / 2.

      self.addChild(background, tick) |> ignore

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
        drawStandardBrackground ()
        active <- not active

        updateTickText()
        onStateChangeEvent.Trigger(CheckboxStateChange.Create(self, ev))

        self.removeAllListeners("mouseout") |> ignore
      ) |> ignore

      if onStateChange.IsSome then
        onStateChangeEvent.Publish.Add(onStateChange.Value)

    interface IStateChangeable<CheckboxStateChange> with
      member self.OnStateChange = onStateChangeEvent.Publish

    member self.state
      with get () = active
      and set (value) =
        active <- value
        updateTickText()
        onStateChangeEvent.Trigger(CheckboxStateChange.Create(self))
