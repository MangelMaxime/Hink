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
  let mutable counter = 0

  let label = Label("I am a label")
  label.UI.x <- 20.
  label.UI.y <- 60.

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

  let defaultButton = Button()

  let customButton =
    Button(
      x = 50.,
      y = 50.
    )

  let buttonWithCallback =
    Button(
      x = 100.,
      y = 100.,
      onClick = (fun _ -> Browser.console.log("constructor callback"))
    )

  customButton
  |> withOnClick (fun event ->
    Browser.console.log("ko")
  )
  |> ignore

  let checkbox =
    Checkbox(
      x = 20.,
      y = 100.,
      onStateChange = (fun event -> Browser.console.log event.NewState)
    )

  app.AddWidget(label)
  app.AddWidget(switchLabel)
  //app.AddWidget(window)

  app
    .RootContainer
    .addChild(defaultButton, customButton, buttonWithCallback, checkbox, switch)
    |> ignore

  // Start app
  app.Start()

  Fable.Import.Browser.console.log Hink.Theme.Default.TextStyle
