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
        NewState = sender.State
        Event = event
      }

  and Switch(?x, ?y, ?width, ?height, ?theme) as self =
    inherit Widget()
    // Themes
    let mutable theme = defaultArg theme Hink.Theme.Default.Switch
    // Properties
    let mutable x = defaultArg x 0.
    let mutable y = defaultArg y 0.
    let mutable active = false
    // Events
    let onStateChange = new Event<SwitchStateChange>()

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

      // Build container
      let container = Container()
      container.x <- x
      container.y <- y
      // Build text
      text.anchor <- Point(0., 0.5)
      text.x <- theme.CirclePadding
      text.y <- theme.Height / 2.
      // Build Circle
      // Attach all components togethers
      container.addChild(background, text, circle) |> ignore
      container.interactive <- true
      self.UI <- container

      // Set display
      updateUI()

      self.UI.on_mouseup(fun ev ->
        active <- not active
        updateUI()
        onStateChange.Trigger(SwitchStateChange.Create(self, ev))
      ) |> ignore

    member self.OnStateChanged = onStateChange.Publish

    member self.State
      with get () = active
      and set (value) =
        active <- value
        updateUI()
        onStateChange.Trigger(SwitchStateChange.Create(self))
