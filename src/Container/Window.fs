namespace Hink.Widgets.Container

open Fable.Core.JsInterop
open Fable.Import
open Fable.Import.PIXI
open Hink.Core

[<AutoOpen>]
module Window =

  type Window(?title) as self =
    inherit Widget()

    let mutable title = defaultArg title ""

    let drawBackground (g: Graphics) color=
      g
        .beginFill(float color)
        .drawRoundedRect(0., 0., 80., 34., 4.)
        .endFill()
        |> ignore

    let background = Graphics()

    let drawStandardBrackground () =
      drawBackground background 0x1ABC9C

    let drawHoverBrackground () =
      drawBackground background 0x16A085

    let titleText = Text(title, Hink.Theme.Default.TextStyle)
    let titleBar = Container()
    let titleBarBackground = Graphics()
    let content = Container()

    do
      let container = Container()

      drawStandardBrackground ()

      titleBarBackground
        .beginFill(float 0x34495E)
        .drawRect(0., 0., 200., 20.)
        .endFill()
        |> ignore

      titleText.anchor <- Point(0., 0.5)
      titleText.x <- 80. / 2.
      titleText.y <- 34. / 2.
      titleBar.addChild(titleBarBackground, titleText) |> ignore
       
      container.addChild(background, titleBar) |> ignore
      container.interactive <- true
      self.UI <- container

      // let resetBackground () =
      //   self.UI.once_mouseout(JsFunc1(fun _ ->
      //     drawStandardBrackground ()
      //   )) |> ignore

      // self.UI.on_mouseover(JsFunc1(fun _ ->
      //   drawBackground background 0x48c9b0
      //   resetBackground()
      // )) |> ignore

      // self.UI.on_mousedown(JsFunc1(fun _ ->
      //   drawHoverBrackground ()
      //   resetBackground()
      // )) |> ignore

      // self.UI.on_mouseup(JsFunc1(fun ev ->
      //   drawStandardBrackground ()
      //   self.UI.removeAllListeners("mouseout") |> ignore
      // )) |> ignore
