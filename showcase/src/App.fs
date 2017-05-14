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

  let switchText state = sprintf "Checkbox value: %b" state
  let switchLabel = Label(switchText switch.state)
  switchLabel.UI.x <- 100.
  switchLabel.UI.y <- 155.

  switch
  |> withOnStateChange(
    fun event ->
        switchLabel.Text <- switchText event.NewState
    )
  |> ignore

  let window = Window()
  window.UI.x <- 20.
  window.UI.y <- 200.

  let mutable counter = 0
  let counterButton =
    Button(
      x = 20.,
      y = 20.,
      str = "Click me !",
      onClick = (
        fun event ->
          counter <- counter + 1
          event.Sender.text <- sprintf "Clicked: %i" counter
      )
    )

  let checkbox =
    Checkbox(
      x = 20.,
      y = 100.,
      onStateChange = (fun event -> Browser.console.log event.NewState)
    )

  app.AddWidget(switchLabel)
  //app.AddWidget(window)

  app
    .RootContainer
    .addChild(counterButton, checkbox, switch)
    |> ignore

  // Start app
  app.Start()

  Fable.Import.Browser.console.log Hink.Theme.Default.TextStyle
