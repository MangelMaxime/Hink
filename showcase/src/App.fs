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
                                            Height = 260. }

    let window2 = { WindowInfo.Default with X = 500.
                                            Y = 50.
                                            Width = 400.
                                            Height = 285.
                                            Closable = true
                                            Draggable = true
                                            Title = Some "You can close me" }
    let mutable window2BackgroundColor = ui.Theme.Window.Header.Color

    let Emerald = "#2ecc71"
    let Nephritis = "#27ae60"
    let Carrot = "#e67e22"
    let Pumpkin = "#d35400"
    let Amethyst = "#9b59b6"
    let Wisteria = "#8e44ad"


    let rec render (_: float) =
        ui.Context.clearRect(0., 0., ui.Canvas.width, ui.Canvas.height)
        ui.Context.fillStyle <- !^"#fff"

        #if DEBUG
        stats.``begin``()
        #endif

        ui.Prepare()

        if ui.Window(window1) then
            if ui.Button("Click me") then
                buttonCounter <- buttonCounter + 1
            ui.Label(sprintf "Clicked: %i times" buttonCounter, Center)

            ui.Label("Row layout demo", Center, backgroundColor = "#34495e")

            ui.Row([|1./2.; 1./4.; 1./4.|])
            ui.Label("1/2", Center, "#f39c12")
            ui.Label("1/4", Center, "#27ae60")
            ui.Label("1/4", Center, "#8e44ad")

            ui.Label("We filled all the row, so new line here", Center, backgroundColor = "#34495e" )

            ui.Empty()
            ui.Row([|1./4.; 1./2.; 1./4.|])

            ui.Empty()
            if ui.Button("Open second Window") then
                window2.Closed <- false
            ui.Empty()

        if ui.Window(window2, headerColor = window2BackgroundColor) then
            ui.Label("Click to change window header", Center)
            ui.Row([|1./3.; 1./3.; 1./3.|])


            if ui.Button("Emerald", defaultColor = Emerald) then
                window2BackgroundColor <- Emerald

            if ui.Button("Amethyst", defaultColor = Amethyst) then
                window2BackgroundColor <- Amethyst

            if ui.Button("Carrot", defaultColor = Carrot) then
                window2BackgroundColor <- Carrot

        ui.Finish()


        #if DEBUG
        stats.``end``() |> ignore
        #endif

        Browser.window.requestAnimationFrame(Browser.FrameRequestCallback(render))
        |> ignore

    render 0.
