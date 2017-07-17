namespace Hink.Showcase

open Fable.Core
open Fable.Core.JsInterop
open Fable.Import
open Hink.Gui


module Main =
    // Application code
    let canvas = Browser.document.getElementById "application" :?> Browser.HTMLCanvasElement
    let ui = Hink.Create(canvas)

    let mutable buttonCounter = 0

    let rec render _ =
        ui.Context.clearRect(0., 0., ui.Canvas.width, ui.Canvas.height)
        ui.Context.fillStyle <- !^"#fff"

        ui.Prepare()

        if ui.Button("plp", "Click me !", 20., 20., 120.) then
            buttonCounter <- buttonCounter + 1

        ui.Label(sprintf "You clicked: %i" buttonCounter, 150., 30.)

        ui.Finish()
        Browser.window.requestAnimationFrame(unbox render) |> ignore

    render 0.
