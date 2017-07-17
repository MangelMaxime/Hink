namespace Hink.Widgets.Container

open Fable.Core.JsInterop
open Fable.Import
open Fable.Import.PIXI
open Hink.Core

[<AutoOpen>]
module Window =
    let private makeContainer() = Container()
    let private makeGraphics() = Graphics()

    type Window(?x, ?y, ?width, ?height, ?title) as self =
        inherit Container()
        let mutable title = defaultArg title ""
        let drawBodyBackground (g : Graphics) color =
            g.beginFill(float color).drawRoundedRect(0., 0., 80., 34., 4.).endFill() |> ignore
        let body = Graphics()
        let drawBodyStandardBrackground() = drawBodyBackground body 0x1ABC9C
        let titleBarText = Text(title, Hink.Theme.Default.TextStyle)
        let titleBar = Container()
        let titleBarBackground = Graphics()
        let content = Container()
        do
            // Position
            self.x <- defaultArg x 0.
            self.y <- defaultArg y 0.
            // Size
            self.width <- defaultArg width 300.
            self.height <- defaultArg height 200.
            // Interactive
            self.interactive <- true
            self.buttonMode <- true
            drawBodyStandardBrackground()
            titleBarBackground.beginFill(float 0x34495E).drawRect(0., 0., 80., 20.).endFill() |> ignore
            titleBarText.anchor <- Point(0., 0.5)
            titleBarText.x <- 80. / 2.
            titleBarText.y <- 34. / 2.
            titleBar.addChild (titleBarBackground, titleBarText) |> ignore
            self.addChild (body, titleBar) |> ignore
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
