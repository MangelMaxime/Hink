namespace Hink.Widgets.Interactive

open Fable.Core.JsInterop
open Fable.Import
open Fable.Import.Browser
open Fable.Import.PIXI
open Hink.Core

[<AutoOpen>]
module Button =
    type ButtonOnClick =
        { Sender : Button
          Event : InteractionEvent }
        static member Create(sender, event) =
            { Sender = sender
              Event = event }

    and Button(?x : float, ?y : float, ?width : float, ?height : float, ?str : string, ?onClick : ButtonOnClick -> unit) =
        inherit Container()
        let onClickEvent = new Event<ButtonOnClick>()
        let drawBackground (g : Graphics) color =
            g.clear().beginFill(float color).drawRoundedRect(0., 0., 80., 34., 4.).endFill() |> ignore
        let background = Graphics()
        let drawStandardBrackground() = drawBackground background 0x1ABC9C
        let drawHoverBrackground() = drawBackground background 0x16A085
        let internalText = Text(defaultArg str "Button", Hink.Theme.Default.TextStyle)

        do
            // Position
            base.x <- defaultArg x 0.
            base.y <- defaultArg y 0.
            // Size
            base.width <- defaultArg width 80.
            base.height <- defaultArg height 80.
            // Interactive
            base.interactive <- true
            base.buttonMode <- true
            drawStandardBrackground()
            internalText.anchor <- Point(0.5, 0.5)
            internalText.x <- 80. / 2.
            internalText.y <- 34. / 2.
            base.addChild (background, internalText) |> ignore
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
                onClickEvent.Trigger(ButtonOnClick.Create(base, ev))
                drawStandardBrackground()
                base.removeAllListeners ("mouseout") |> ignore)
            |> ignore
            // Register onClick callback if one given in the constructor
            if onClick.IsSome then onClickEvent.Publish.Add(onClick.Value)

        interface IClickable<ButtonOnClick> with
            member this.OnClick = onClickEvent.Publish

        member this.text
            with get () = internalText.text
            and set (value) = internalText.text <- value
