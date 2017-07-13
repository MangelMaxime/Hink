namespace Hink.Showcase

open Hink.Core
open Hink.Widgets.Container
open Hink.Widgets.Display
open Hink.Widgets.Interactive
open Fable.Import

module Main =

  // Application code
  let app = Application()
  app.Init()

  // Build UI
  let switch =
    Switch(
      x = 20.,
      y = 150.
    )

  let switchText state = sprintf "Switch value: %b" state
  let switchLabel =
    Label(
      x = 100.,
      y= 155.,
      str = switchText switch.state
    )

  switch
  |> withOnStateChange(
    fun event ->
        switchLabel.text <- switchText event.NewState
    )
  |> ignore

  let window =
    Window(
      x = 20.,
      y = 200.
    )
//
  let mutable counter = 0
  let counterButton =
    Button(
      x = 20.,
      y = 40.,
      str = "Click me !",
      onClick = (
        fun event ->
          counter <- counter + 1
          event.Sender.text <- sprintf "Clicked: %i" counter
      )
    )

  let checbkoxLabel =
    Label (
      x = 50.,
      y = 105.,
      str = "Checkbox value: false"
    )

  let checkbox =
    Checkbox(
      x = 20.,
      y = 100.,
      onStateChange =
        ( fun event ->
          checbkoxLabel.text <- sprintf "Checkbox value: %b" event.NewState )
    )

  app
    .RootContainer
    .addChild(counterButton, checkbox, checbkoxLabel, switch, switchLabel, window)
    |> ignore

  // Start app
  app.Start()

  Fable.Import.Browser.console.log Hink.Theme.Default.TextStyle
