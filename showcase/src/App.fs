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

    let window1 = { WindowInfo.Default with X = 10.
                                            Y = 10.
                                            Width = 400.
                                            Height = 300. }

    let window2 = { WindowInfo.Default with X = 500.
                                            Y = 50.
                                            Width = 400.
                                            Height = 285.
                                            Closable = true }

    let rec render _ =
        ui.Context.clearRect(0., 0., ui.Canvas.width, ui.Canvas.height)
        ui.Context.fillStyle <- !^"#fff"

        #if DEBUG
        stats.``begin``()
        #endif

        ui.Prepare()

        ui.Window(window1)
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

        ui.Label("Should be displayed", backgroundColor = "#8e44ad")
        ui.Label("Should not be displayed", backgroundColor = "#8e44ad")

        ui.Window(window2, "#e74c3c")
        ui.Label("Here we have a new window")
        ui.Row([|1./2.; 1./4.; 1./4.|])
        ui.Label("1/2", backgroundColor = "#f39c12")
        ui.Label("1/4", backgroundColor = "#27ae60")
        ui.Label("1/4", backgroundColor = "#8e44ad")

        ui.Finish()



        // ui.Context.fillStyle <- !^"blue"
        // ui.Context.fillRect(10., 10., 100., 100.)

        // ui.Context.globalCompositeOperation <- "destination-over"

        // ui.Context.fillStyle <- !^"red"
        // ui.Context.fillRect(50., 50., 100., 100.)

        // ui.Context.globalCompositeOperation <- "source-over"

        // ui.Context.fillStyle <- !^"green"
        // ui.Context.fillRect(75., 75., 100., 100.)

        #if DEBUG
        stats.``end``() |> ignore
        #endif

        Browser.window.requestAnimationFrame(unbox render) |> ignore

    render 0.
