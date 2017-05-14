namespace Hink.Widgets.Interactive

open Fable.Core.JsInterop
open Fable.Import
open Fable.Import.Browser
open Fable.Import.PIXI
open Hink.Core

[<AutoOpen>]
module Switch =

  type SwitchStateChange =
    {
      Sender: Switch
      NewState: bool
      Event: InteractionEvent option
    }

    static member Create (sender, ?event) =
      {
        Sender = sender
        NewState = sender.state
        Event = event
      }

  and Switch( ?x, ?y, ?width, ?height, ?theme,
              ?onStateChange: SwitchStateChange -> unit) as self =
    inherit Container()
    // Themes
    let mutable theme = defaultArg theme Hink.Theme.Default.Switch
    let mutable active = false
    // Events
    let onStateChangeEvent = new Event<SwitchStateChange>()

    // Helpers
    let background = Graphics()
    let drawBackground color =
      background
        .clear()
        .beginFill(float color)
        .drawRoundedRect(
          0.,
          0.,
          theme.Width,
          theme.Height,
          theme.Height / 2.
        )
        .endFill()
        |> ignore

    let circle = Graphics()
    let drawCircle color =
      let x =
        if active then
          theme.Width - (theme.CircleRadius + theme.CirclePadding)
        else
          theme.CircleRadius + theme.CirclePadding
      circle
        .clear()
        .beginFill(float color)
        .drawCircle(
          x,
          theme.Height / 2.,
          theme.CircleRadius
        )
        .endFill()
        |> ignore

    let text = Text("")

    let updateUI () =
      if active then
        drawBackground theme.ActiveTheme.BackgroundColor
        text.x <- theme.CirclePadding * 3.
        text.text <- "ON"
        text.style <- theme.ActiveTheme.TextStyle
        drawCircle theme.ActiveTheme.CircleColor
      else
        drawBackground theme.InactiveTheme.BackgroundColor
        text.x <- theme.CirclePadding * 2. + theme.CircleRadius * 2.
        text.text <- "OFF"
        text.style <- theme.InactiveTheme.TextStyle
        drawCircle theme.InactiveTheme.CircleColor

    do

      //Position
      self.x <- defaultArg x 0.
      self.y <- defaultArg y 0.
      // Build text
      text.anchor <- Point(0., 0.5)
      text.x <- theme.CirclePadding
      text.y <- theme.Height / 2.
      // Interactive
      self.interactive <- true
      self.buttonMode <- true
      // Attach all components togethers
      self.addChild(background, text, circle) |> ignore

      // Set display
      updateUI()

      self.on_mouseup(fun ev ->
        active <- not active
        updateUI()
        onStateChangeEvent.Trigger(SwitchStateChange.Create(self, ev))
      ) |> ignore

      if onStateChange.IsSome then
        onStateChangeEvent.Publish.Add(onStateChange.Value)

    interface IStateChangeable<SwitchStateChange> with
      member self.OnStateChange = onStateChangeEvent.Publish

    member self.state
      with get () = active
      and set (value) =
        active <- value
        updateUI()
        onStateChangeEvent.Trigger(SwitchStateChange.Create(self))
