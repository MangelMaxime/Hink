namespace Hink.Inputs

open Fable.Core
open Browser.Types
open Browser.Event
open Browser

[<RequireQualifiedAccess>]
module Mouse =
    /// Just a alias type
    type ButtonState = bool

    [<RequireQualifiedAccess>]
    module Cursor =
        let Alias = "alias"
        let AllScroll = "all-scroll"
        let Auto = "auto"
        let Cell = "cell"
        let ContextMenu = "context-menu"
        let ColResize = "col-resize"
        let Copy = "copy"
        let Crosshair = "crosshair"
        let Default = "default"
        let EResize = "e-resize"
        let EwResize = "ew-resize"
        let Grab = "grab"
        let Grabbing = "grabbing"
        let Help = "help"
        let Move = "move"
        let NResize = "n-resize"
        let NeResize = "ne-resize"
        let NeswResize = "nesw-resize"
        let NsResize = "ns-resize"
        let NwResize = "nw-resize"
        let NwseResize = "nwse-resize"
        let NoDrop = "no-drop"
        let None = "none"
        let NotAllowed = "not-allowed"
        let Pointer = "pointer"
        let Progress = "progress"
        let RowResize = "row-resize"
        let SResize = "s-resize"
        let SeResize = "se-resize"
        let SwResize = "sw-resize"
        let Text = "text"
        let URL = "URL"
        let VerticalText = "vertical-text"
        let WResize = "w-resize"
        let Wait = "wait"
        let ZoomIn = "zoom-in"
        let ZoomOut = "zoom-out"
        let Initial = "initial"
        let Inherit = "inherit"

    /// Record used to store Mouse state
    type Record =
        {   mutable X : float
            mutable Y : float
            mutable Left : ButtonState
            mutable Right : ButtonState
            mutable Middle : ButtonState
            mutable IsDragging : bool
            mutable DragOriginX : float
            mutable DragOriginY : float
            mutable HasBeenInitiated : bool
            mutable Element : HTMLElement
            mutable JustReleased : bool }

        /// Initial state of Mouse
        static member Initial =
            {   X = 0.
                Y = 0.
                Left = false
                Right = false
                Middle = false
                IsDragging = false
                DragOriginX = 0.
                DragOriginY = 0.
                HasBeenInitiated = false
                Element = window.document.body
                JustReleased = false }

        member this.HitRegion(x, y, w, h) = this.X > x && this.X <= x + w && this.Y > y && this.Y < y + h
        member this.SetCursor cursor = this.Element.style.cursor <- cursor
        member this.ResetCursor() = this.Element.style.cursor <- Cursor.Auto

        member this.ResetReleased () =
            this.JustReleased <- false

        member this.ResetDragInfo () =
            this.DragOriginX <- this.X
            this.DragOriginY <- this.Y

        member this.DragDeltaX
            with get () =
                if this.IsDragging then this.X - this.DragOriginX else 0.

        member this.DragDeltaY
            with get () =
                if this.IsDragging then this.Y - this.DragOriginY else 0.

    /// Variable used to access current Mouse state
    let Manager = Record.Initial

    /// Init the Mouse module to map Mouse state
    /// * element: Element used to listem the Mouse events
    let init (element : HTMLElement) =
        // If the Manager has not been Initiated
        if not Manager.HasBeenInitiated then
            Manager.Element <- element
            /// Attach handler to Mouse down event
            element.addEventListener("mousedown",
                fun ev ->
                    let ev = ev :?> MouseEvent
                    match ev.button with
                    | 0. -> Manager.Left <- true
                    | 1. -> Manager.Right <- true
                    | 2. -> Manager.Middle <- true
                    | _ -> failwith "Not supported ButtonStatetton"
            )
            /// Attach handler to Mouse up event
            element.addEventListener("mouseup",
                fun ev ->
                    let ev = ev :?> MouseEvent
                    match ev.button with
                    | 0. ->
                        Manager.Left <- false
                        Manager.IsDragging <- false
                        Manager.DragOriginX <- 0.
                        Manager.DragOriginY <- 0.
                        Manager.JustReleased <- true
                    | 1. -> Manager.Middle <- false
                    | 2. -> Manager.Right <- false
                    | _ -> failwith "Not supported button"
            )
            /// Attach handler to Mouse move event
            element.addEventListener("mousemove",
                fun ev ->
                    let ev = ev :?> MouseEvent
                    // Update position
                    Manager.X <- ev.offsetX
                    Manager.Y <- ev.offsetY
                    // Update dragging state
                    if Manager.Left then
                        // Start dragging
                        if not Manager.IsDragging then
                            Manager.DragOriginX <- Manager.X
                            Manager.DragOriginY <- Manager.Y
                        Manager.IsDragging <- true
            )
            // Tag that event listener has been set
            Manager.HasBeenInitiated <- true
