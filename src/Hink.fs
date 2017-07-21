namespace Hink

open Fable.Core
open Fable.Core.JsInterop
open Fable.Import
open Hink.Inputs
open Hink.Theme
open System

module Gui =

    type ID = string

    type Layout =
        | Horizontal
        | Vertical

    type Align =
        | Left
        | Center
        | Right

    let oneSecond = TimeSpan.FromSeconds 1.

    type Cursor =
        { mutable X : float
          mutable Y : float
          mutable Width : float
          mutable Height : float }

    type DragInfo =
        { OriginX : float
          OriginY : float }

    type WindowInfo =
        { mutable X : float
          mutable Y : float
          mutable Width : float
          mutable Height : float
          mutable Layout : Layout
          mutable Draggable : bool
          mutable Closable : bool
          mutable Closed : bool
          mutable Title : string option
          mutable DragXOrigin : float
          mutable DragYOrigin : float
          mutable IsDragging : bool }

        static member Default = // TODO: Use theme ?
            { X = 0.
              Y = 0.
              Width = 100.
              Height = 200.
              Layout = Vertical
              Draggable = false
              Closable = false
              Closed = false
              Title = None
              DragXOrigin = 0.
              DragYOrigin = 0.
              IsDragging = false }

        member this.RealPositionX
            with get () =
                if this.IsDragging then
                    this.X + (Mouse.Manager.X - this.DragXOrigin)
                else
                    this.X

        member this.RealPositionY
            with get () =
                if this.IsDragging then
                    this.Y + (Mouse.Manager.Y - this.DragYOrigin)
                else
                    this.Y

    type Row =
        { mutable Ratios : float []
          mutable CurrentRatio : int
          mutable SplitX : float
          mutable SplitWidth : float }

        member this.ActiveRatio
            with get () = this.Ratios.[this.CurrentRatio]

    type Hink =
        { Canvas : Browser.HTMLCanvasElement
          Context : Browser.CanvasRenderingContext2D
          Mouse : Mouse.Record
          Keyboard : Keyboard.Record
          Theme : Theme
          mutable RowInfo : Row option
          mutable Cursor : Cursor
          /// Store if the cursor has been styled in the last loop. Ex: Hover
          mutable IsCursorStyled : bool
          /// Store if the window should be closed
          mutable ShouldCloseWindow : bool
          mutable CurrentWindow : WindowInfo option
          mutable LastReferenceTick : DateTime
          mutable Delta : TimeSpan }

        static member Create (canvas : Browser.HTMLCanvasElement, ?fontSize, ?theme) =
            // Allow canvas interaction
            canvas.setAttribute ("tabindex", "1")
            // Init the context
            let context = canvas.getContext_2d()
            context.textBaseline <- "top"
            canvas.focus()

            // Init input manager
            Mouse.init canvas
            Keyboard.init canvas true

            { Canvas = canvas
              Context = context
              Mouse = Mouse.Manager
              Keyboard = Keyboard.Manager
              IsCursorStyled = false
              Theme = defaultArg theme darkTheme
              Cursor =
                { X = 0.
                  Y = 0.
                  Width = 0.
                  Height = 0. }
              CurrentWindow = None
              ShouldCloseWindow = false
              RowInfo = None
              LastReferenceTick = DateTime.Now
              Delta = TimeSpan.Zero }

        /// Reset the state of Hot Item
        /// Need to be called before drawing UI
        member this.Prepare () =
            let now = DateTime.Now
            this.Delta <- now - this.LastReferenceTick

            this.Context.globalCompositeOperation <- "source-over"

            if this.Delta > oneSecond then
                this.LastReferenceTick <- now
                this.Delta <- this.Delta.Subtract(oneSecond)

        /// Reset Active item
        /// Need to be called after drawing UI
        member this.Finish () =
            // TODO: Don't reset every time
            this.Mouse.ResetReleased ()
            this.Mouse.ResetDragInfo ()
            // Reset the cursor style if not styled
            if not this.IsCursorStyled then
                this.Mouse.ResetCursor ()

            // Set value to false
            // Help detecting if the mouse cursor need to be reset on the next loop
            this.IsCursorStyled <- false

            // Close current window
            if this.CurrentWindow.IsSome then
                this.EndWindow()
            this.CurrentWindow <- None
            this.RowInfo <- None

            this.Keyboard.ClearLastKey()

        member this.SetCursor cursor =
            this.Mouse.SetCursor cursor
            this.IsCursorStyled <- true

        member this.IsVisibile(elementH) =
            match this.CurrentWindow with
            | None -> true
            | Some { Closed = true } ->
                false
            | Some window ->
                this.Cursor.Y + elementH > 0. && this.Cursor.Y < window.Y + this.Theme.Window.Header.Height + window.Height - elementH

        member this.IsHover(?elementH) =
            let elementH = defaultArg elementH this.Theme.Element.Height
            this.Mouse.X >= this.Cursor.X && this.Mouse.X < (this.Cursor.X + this.Cursor.Width) &&
            this.Mouse.Y >= this.Cursor.Y && this.Mouse.Y < (this.Cursor.Y + elementH)

        member this.IsPressed(?elementH) =
            let elementH = defaultArg elementH this.Theme.Element.Height
            this.Mouse.Left && this.IsHover(elementH)

        member this.IsReleased(?elementH) =
            let elementH = defaultArg elementH this.Theme.Element.Height
            this.Mouse.JustReleased && this.IsHover(elementH)

        member this.EndElement(?elementHeight) =
            match this.CurrentWindow with
            | None ->
                this.Cursor.Y <- this.Cursor.Y + this.Theme.Element.Height + this.Theme.Element.SeparatorSize

            | Some { Layout = Vertical } ->
                let elementHeight = defaultArg elementHeight (this.Theme.Element.Height + this.Theme.Element.SeparatorSize)
                match this.RowInfo with
                | None ->
                    this.Cursor.Y <- this.Cursor.Y + elementHeight
                | Some rowInfo ->
                    // Detect new line
                    if rowInfo.CurrentRatio = (rowInfo.Ratios.Length - 1) || rowInfo.CurrentRatio = -1 then
                        this.Cursor.Y <- this.Cursor.Y + elementHeight

                        // Detect if it's the last element of the row
                        if rowInfo.CurrentRatio = (rowInfo.Ratios.Length - 1) then
                            this.RowInfo <- None
                            this.Cursor.X <- rowInfo.SplitX
                            this.Cursor.Width <- rowInfo.SplitWidth

                    else // Row
                        rowInfo.CurrentRatio <- rowInfo.CurrentRatio + 1
                        this.Cursor.X <- this.Cursor.X + this.Cursor.Width
                        this.Cursor.Width <- rowInfo.SplitWidth * rowInfo.ActiveRatio

            | Some { Layout = Horizontal } ->
                this.Cursor.X <- this.Cursor.Width + this.Theme.Element.SeparatorSize

        member this.EndWindow() =
            if not this.CurrentWindow.Value.Closed then
                // Set if the window has been closed during this loop
                this.CurrentWindow.Value.Closed <- this.ShouldCloseWindow
                this.ShouldCloseWindow <- false
            // Remove the window to end it
            this.CurrentWindow <- None
            // TODO: Handle scroll + save window position

        member this.Window(windowInfo : WindowInfo, ?backgroundColor, ?headerColor) =
            if this.CurrentWindow.IsSome then
                this.EndWindow()

            this.CurrentWindow <- Some windowInfo

            // Browser.console.log this.CurrentWindow.Value.DragX

            // if this.CurrentWindow.Value.IsDragging then
            //     this.CurrentWindow.Value.X <- this.CurrentWindow.Value.X + this.CurrentWindow.Value.DragX
            //     this.CurrentWindow.Value.Y <- this.CurrentWindow.Value.Y + this.CurrentWindow.Value.DragY

            // Browser.console.log(this.CurrentWindow.Value.DragX)

            // If the Window is not closed draw it
            if not this.CurrentWindow.Value.Closed then

                this.Cursor.X <- this.CurrentWindow.Value.RealPositionX
                this.Cursor.Y <- this.CurrentWindow.Value.RealPositionY + this.Theme.Window.Header.Height  // TODO: handle scroll
                this.Cursor.Width <- windowInfo.Width
                this.Cursor.Height <- windowInfo.Height

                // Draw Window header
                this.Context.fillStyle <- !^(defaultArg headerColor this.Theme.Window.Header.Color)
                this.Context.fillRect(
                    this.Cursor.X,
                    this.Cursor.Y - this.Theme.Window.Header.Height,
                    this.Cursor.Width,
                    this.Theme.Window.Header.Height
                )

                // Set the text font for the header
                this.Context.font <- this.Theme.FormatFontString this.Theme.FontSmallSize
                // Common Y position for the title and the symbol
                let headerTextY = this.Cursor.Y - this.Theme.Window.Header.Height + this.Theme.Window.Header.SymbolOffset

                match this.CurrentWindow.Value.Title with
                | None -> () // Nothing todo
                | Some title ->
                    let textSize = this.Context.measureText(title)
                    this.Context.fillStyle <- !^this.Theme.Text.Color
                    this.Context.fillText(
                        title,
                        this.Cursor.X + this.Theme.Window.Header.SymbolOffset,
                        headerTextY
                    )

                if this.CurrentWindow.Value.Draggable then
                    let headerOriginY = this.Cursor.Y - this.Theme.Window.Header.Height
                    let hoverHeader = this.Mouse.X >= this.Cursor.X && this.Mouse.X < (this.Cursor.X + this.Cursor.Width) && this.Mouse.Y >= headerOriginY && this.Mouse.Y < (headerOriginY + this.Theme.Window.Header.Height)

                    // If hover the header and left mouse button is pressed
                    if hoverHeader then
                        if this.Mouse.Left then
                            if not this.CurrentWindow.Value.IsDragging then
                                this.CurrentWindow.Value.DragXOrigin <- this.Mouse.X
                                this.CurrentWindow.Value.DragYOrigin <- this.Mouse.Y
                            // Memorise that we started to drag
                            this.CurrentWindow.Value.IsDragging <- true

                    if this.Mouse.JustReleased && this.CurrentWindow.Value.IsDragging then
                        // Store new X, Y position
                        this.CurrentWindow.Value.X <- this.CurrentWindow.Value.RealPositionX
                        this.CurrentWindow.Value.Y <- this.CurrentWindow.Value.RealPositionY
                        // Reset drag info
                        this.CurrentWindow.Value.IsDragging <- false
                        this.CurrentWindow.Value.DragXOrigin <- 0.
                        this.CurrentWindow.Value.DragYOrigin <- 0.

                if this.CurrentWindow.Value.Closable then
                    let textSize = this.Context.measureText("\u2715")
                    let textX = this.Cursor.X + this.Cursor.Width - textSize.width - this.Theme.Window.Header.SymbolOffset

                    // Custom IsHover check has we don't follow auto layout management for the header symbol
                    let hoverSymbol = this.Mouse.X >= textX && this.Mouse.X < (textX + textSize.width) &&
                                      this.Mouse.Y >= headerTextY && this.Mouse.Y < (headerTextY + this.Theme.FontSize)

                    // Draw symbol background
                    if hoverSymbol then
                        this.Context.fillStyle <- !^this.Theme.Window.Header.OverSymbolColor
                        this.Context.fillRect(
                            textX - this.Theme.Window.Header.SymbolOffset,
                            this.Cursor.Y - this.Theme.Window.Header.Height,
                            textSize.width + this.Theme.Window.Header.SymbolOffset * 2.,
                            headerTextY + this.Theme.FontSize
                        )
                        if this.Mouse.JustReleased then
                            this.ShouldCloseWindow <- true

                    // Draw symbol
                    this.Context.fillStyle <- !^this.Theme.Text.Color
                    this.Context.fillText("\u2715",textX, headerTextY)



                // Draw Window background
                this.Context.fillStyle <- !^(defaultArg backgroundColor this.Theme.Window.Background)

                this.Context.fillRect(
                    this.Cursor.X,
                    this.Cursor.Y, // TODO: handle scroll
                    this.Cursor.Width, // TODO: lastMaxX (auto size calculation)
                    this.Cursor.Height // TODO: lastMaxX (auto size calculation)
                )

        member this.Label (text, ?align : Align, ?backgroundColor : string) =
            if not (this.IsVisibile(this.Theme.Element.Height)) then
                this.EndElement()
            else
                let align = defaultArg align Left

                if backgroundColor.IsSome then
                    this.Context.fillStyle <- !^backgroundColor.Value
                    this.Context.fillRect(
                        this.Cursor.X + this.Theme.ButtonOffsetY,
                        this.Cursor.Y + this.Theme.ButtonOffsetY,
                        this.Cursor.Width - this.Theme.ButtonOffsetY * 2.,
                        this.Theme.Button.Height
                    )

                this.FillSmallString(text, align = align)
                this.EndElement()

        member this.Empty() =
            this.EndElement()

        member this.Button (text, ?align : Align, ?pressedColor, ?hoverColor, ?defaultColor) =
            if not (this.IsVisibile(this.Theme.Element.Height)) then
                this.EndElement()
                false
            else

                let align = defaultArg align Center
                // Check visibility
                let hover = this.IsHover()
                let pressed = this.IsPressed()
                let released = this.IsReleased()

                this.Context.fillStyle <-
                    if pressed then
                        !^(defaultArg pressedColor this.Theme.Button.Background.Pressed)
                    elif hover then
                        !^(defaultArg hoverColor this.Theme.Button.Background.Hover)
                    else
                        !^(defaultArg defaultColor this.Theme.Button.Background.Default)

                if hover then
                    this.SetCursor Mouse.Cursor.Pointer

                this.Context.RoundedRect(
                    this.Cursor.X + this.Theme.ButtonOffsetY,
                    this.Cursor.Y + this.Theme.ButtonOffsetY,
                    this.Cursor.Width - this.Theme.ButtonOffsetY * 2.,
                    this.Theme.Button.Height,
                    this.Theme.Button.CornerRadius
                )

                this.Context.fillStyle <- !^this.Theme.Text.Color
                this.FillSmallString(text, align = align)

                this.EndElement()

                released

        member this.Row (ratios) =
            this.RowInfo <- Some { Ratios = ratios
                                   CurrentRatio = 0
                                   SplitX = this.Cursor.X
                                   SplitWidth = this.Cursor.Width }

            this.Cursor.Width <- this.Cursor.Width * this.RowInfo.Value.ActiveRatio

        member this.FillSmallString (text, ?offsetX, ?offsetY, ?align : Align) =
            let offsetY = defaultArg offsetY 0.
            let align = defaultArg align Left

            let offsetX =
                let textSize = this.Context.measureText(text)
                match align with
                | Left ->
                    defaultArg offsetX this.Theme.Text.OffsetX
                | Center ->
                    (this.Cursor.Width / 2.) - (textSize.width / 2.)
                | Right ->
                    this.Cursor.Width - textSize.width - this.Theme.Text.OffsetX

            // TODO: Check max chars
            this.Context.fillStyle <- !^this.Theme.Text.Color
            this.Context.font <- this.Theme.FormatFontString this.Theme.FontSmallSize
            this.Context.fillText(
                text,
                this.Cursor.X + offsetX,
                this.Cursor.Y + this.Theme.FontSmallOffsetY + offsetY
            )

        // member this.Checkbox(id, value: bool ref, x, y, ?theme) =
        //     let theme = defaultArg theme this.Theme.Checkbox

        //     if this.Mouse.HitRegion(x, y, theme.Width, theme.Height) then
        //         this.HotItem <- id
        //         this.Mouse.SetCursor Mouse.Cursor.Pointer
        //         if this.HotItem = id && this.Mouse.Left then
        //             this.ActiveItem <- id

        //     if !value then
        //         this.Context.fillStyle <- !^theme.Color
        //     else
        //         this.Context.fillStyle <- !^theme.ActiveColor

        //     this.Context.RoundedRect(x, y, theme.Width, theme.Height, theme.CornerRadius)

        //     /// Draw the tick if the value is true
        //     /// Or if the value is not true and widget is the hot one
        //     if !value || not !value && this.HotItem = id then
        //         this.Context.fillStyle <- !^theme.TickColor
        //         this.Context.font <- this.Theme.FormatFontString ()
        //         this.Context.fillText("\u2713", x + 5., y + 4.)

        //     let clicked = (not this.Mouse.Left) && this.HotItem = id && this.ActiveItem = id

        //     if clicked then
        //         value := not !value

        //     clicked

        // member this.Switch (id, value: bool ref, x, y, ?theme) =
        //     let theme = defaultArg theme this.Theme.Switch

        //     let renderSwitch (rectX, rectY, rectW, rectH, rectStyle) (circleX, circleY, circleRadius, circleStyle) (textValue, textStyle, offset) =
        //         this.Context.fillStyle <- rectStyle
        //         this.Context.RoundedRect(rectX, rectY, rectW, rectH, rectH / 2.)
        //         this.Context.closePath()

        //         this.Context.fillStyle <- textStyle
        //         this.Context.font <- this.Theme.FormatFontString()

        //         let resizeWidth = theme.Width - (2. * (theme.SpacingCircle + theme.Radius))
        //         let textSize = this.Context.measureText(textValue)
        //         let textX =
        //             if offset then
        //                 rectX + (resizeWidth / 2.) - (textSize.width / 2.) + theme.SpacingCircle + theme.Radius * 2.
        //             else
        //                 rectX + (resizeWidth / 2.) - (textSize.width / 2.) + theme.SpacingCircle
        //         let textY = rectY + (rectH / 2.) - (this.Theme.FontSize / 2.)

        //         this.Context.fillText(
        //             textValue,
        //             textX, textY
        //         )

        //         this.Context.beginPath()
        //         this.Context.fillStyle <- circleStyle
        //         this.Context.arc(circleX, circleY, circleRadius, 0., 2. * Math.PI)
        //         this.Context.fill()

        //     if this.Mouse.HitRegion(x, y, theme.Width, theme.Height) then
        //         this.HotItem <- id
        //         this.Mouse.SetCursor Mouse.Cursor.Pointer
        //         if this.HotItem = id && this.Mouse.Left then
        //             this.ActiveItem <- id

        //     let circleY = y + theme.HalfHeight

        //     if !value then
        //         let circleX = x + theme.Width - theme.HalfHeight
        //         renderSwitch
        //             (x, y, theme.Width, theme.Height, !^theme.Color.Background.Default)
        //             (circleX, circleY, theme.Radius, !^theme.Color.Circle.Default)
        //             ("ON", !^theme.Color.Text.Default, false)
        //     else
        //         let circleX = x + theme.HalfHeight
        //         renderSwitch
        //             (x, y, theme.Width, theme.Height, !^theme.Color.Background.Active)
        //             (circleX, circleY, theme.Radius, !^theme.Color.Circle.Active)
        //             ("OFF", !^theme.Color.Text.Active, true)

        //     let clicked = (not this.Mouse.Left) && this.HotItem = id && this.ActiveItem = id

        //     if clicked then
        //         value := not !value

        //     clicked
