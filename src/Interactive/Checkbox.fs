namespace Hink.Widgets.Interactive

open Fable.Core.JsInterop
open Fable.Import
open Fable.Import.Browser
open Fable.Import.PIXI
open Hink.Core

[<AutoOpen>]
module Checkbox =
    type CheckboxStateChange =
        { Sender : Checkbox
          NewState : bool
          Event : InteractionEvent option }
        static member Create(sender, ?event) =
            { Sender = sender
              NewState = sender.state
              Event = event }

    and Checkbox(?x : float, ?y : float, ?width : float, ?height : float, ?state : bool, ?onStateChange : CheckboxStateChange -> unit) =
        inherit Container()
        let onStateChangeEvent = new Event<CheckboxStateChange>()
        let drawBackground (g : Graphics) color =
            g.beginFill(float color).drawRoundedRect(0., 0., 22., 22., 3.).endFill() |> ignore
        let background = Graphics()
        let drawStandardBrackground() = drawBackground background 0x1ABC9C
        let drawHoverBrackground() = drawBackground background 0x16A085
        let tick = PIXI.Text("", Hink.Theme.Default.TextStyle)
        let mutable active = false

        let updateTickText() =
            if active then tick.text <- "\u2713"
            else tick.text <- ""

        do
            // Position
            base.x <- defaultArg x 0.
            base.y <- defaultArg y 0.
            // Size
            base.width <- defaultArg width 22.
            base.height <- defaultArg height 22.
            // Interactive
            base.interactive <- true
            base.buttonMode <- true
            drawStandardBrackground()
            tick.anchor <- Point(0.5, 0.5)
            tick.x <- 22. / 2.
            tick.y <- 22. / 2.
            base.addChild (background, tick) |> ignore
            let resetBackground() = base.once_mouseout (fun _ -> drawStandardBrackground()) |> ignore
            base.on_mouseover (fun _ ->
                drawBackground background 0x48c9b0
                resetBackground())
            |> ignore
            base.on_mousedown (fun _ ->
                drawHoverBrackground()
                resetBackground())
            |> ignore
            base.on_mouseup (fun ev ->
                drawStandardBrackground()
                active <- not active
                updateTickText()
                onStateChangeEvent.Trigger(CheckboxStateChange.Create(base, ev))
                base.removeAllListeners ("mouseout") |> ignore)
            |> ignore
            if onStateChange.IsSome then onStateChangeEvent.Publish.Add(onStateChange.Value)

        interface IStateChangeable<CheckboxStateChange> with
            member this.OnStateChange = onStateChangeEvent.Publish

        member this.state
            with get () = active
            and set (value) =
                active <- value
                updateTickText()
                onStateChangeEvent.Trigger(CheckboxStateChange.Create(base))
