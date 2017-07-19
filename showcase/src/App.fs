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
    let isChecked = ref false
    let switchValue = ref false

    #if DEBUG
    let stats = Stats()
    stats.showPanel(0.)

    Browser.document.body.appendChild stats.dom |> ignore
    #endif

    let rec render _ =
        ui.Context.clearRect(0., 0., ui.Canvas.width, ui.Canvas.height)
        ui.Context.fillStyle <- !^"#fff"

        #if DEBUG
        stats.``begin``()
        #endif

        ui.Prepare()


        ui.Window(10., 10., 400., 285.)
        if ui.Button("Click me") then
            buttonCounter <- buttonCounter + 1
        ui.Label(sprintf "Clicked: %i" buttonCounter)
        ui.Label(sprintf "Clicked: %i" buttonCounter, Center)
        ui.Label(sprintf "Clicked: %i" buttonCounter, Right)

        ui.Label("Row demo", Center, backgroundColor = "#34495e")

        ui.Row([|1./2.; 1./4.; 1./4.|])
        ui.Label("1/2", backgroundColor = "#f39c12")
        ui.Label("1/4", backgroundColor = "#27ae60")
        ui.Label("1/4", backgroundColor = "#8e44ad")

        ui.Label("We filled all the row, so new line here", Center, backgroundColor = "#34495e" )

        ui.Window(500., 50., 400., 285.)
        ui.Label("Here we have a new window")
        ui.Label("Here we have a new window")
        ui.Label("Here we have a new window")
        ui.Row([|1./2.; 1./4.; 1./4.|])
        ui.Label("1/2", backgroundColor = "#f39c12")
        ui.Label("1/4", backgroundColor = "#27ae60")
        ui.Label("1/4", backgroundColor = "#8e44ad")

        // if ui.Button("plp", "Click me !", 20., 20., 120.) then
        //     buttonCounter <- buttonCounter + 1

        // ui.Label(sprintf "You clicked: %i" buttonCounter, 150., 30.)

        // ui.Checkbox("checkbox", isChecked, 20., 80.) |> ignore
        // ui.Label(
        //     sprintf "Clicked: %b" isChecked.Value,
        //     60., 83.)

        // ui.Switch("switch", switchValue, 20., 130.) |> ignore
        // ui.Label(
        //     sprintf "Switch: %b" switchValue.Value,
        //     100., 135.
        // )

        ui.Finish()

        #if DEBUG
        stats.``end``() |> ignore
        #endif

        Browser.window.requestAnimationFrame(unbox render) |> ignore

    render 0.
