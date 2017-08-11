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

    type SelectionArea =
        { Start: int
          End: int }

        member this.Length =
            this.End - this.Start


        ///**Description**
        /// Return true is the selection encapsulate the given size
        ///**Parameters**
        ///  * `size` - parameter of type `int` - Size to encapsulate
        ///
        ///**Output Type**
        ///  * `bool`
        ///
        member this.Edging size =
            this.Start = 0 && this.End = size && this.Start <> this.End

        static member Create (start, ``end``) =
            { Start = start
              End = ``end`` }

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

        static member Default =
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

    type CheckboxInfo =
        { mutable Value : bool }

        static member Default =
            { Value = false }

    type ComboState =
        | Extended
        | JustExtended
        | Closed

    type ComboInfo =
        { mutable SelectedIndex : int option
          mutable State : ComboState
          Guid : Guid }

        static member Default
            with get () =
                { SelectedIndex = None
                  State = Closed
                  Guid = Guid.NewGuid() }

    /// Type used to stored combo information for drawing when Finish the loop
    type ComboMemory =
        { Info : ComboInfo
          X : float
          Y : float
          Width : float
          Height : float
          Values : string list
          Reference : ComboInfo }

    type Row =
        { mutable Ratios : float []
          mutable CurrentRatio : int
          mutable SplitX : float
          mutable SplitWidth : float }

        member this.ActiveRatio
            with get () = this.Ratios.[this.CurrentRatio]

    type InputInfo =
        { mutable IsActive : bool
          mutable Value : string
          mutable Selection : SelectionArea option
          // Positive offset of the cursor.
          // Offset of 0 = start of the input
          // Offset of 2 = cursor place after the second char of the input
          mutable CursorOffset : int
          // Start origin of the text to display
          // 0 = Start of the text
          // 2 = Start of the text after the 2 first chars
          mutable TextStartOrigin : int }

        member this.ClearSelection () =
            this.Selection <- None

        member this.SetSelection (value) =
            this.Selection <- Some value

        static member Default
            with get () = { IsActive = false
                            Value = ""
                            Selection = None
                            CursorOffset = 0
                            TextStartOrigin = 0 }

    type Hink =
        { Canvas : Browser.HTMLCanvasElement
          Context : Browser.CanvasRenderingContext2D
          Mouse : Mouse.Record
          Keyboard : Keyboard.Record
          KeyboardCaptureHandler : (Keyboard.Record -> bool) option
          mutable KeyboadHasBeenCapture : bool
          Theme : Theme
          mutable RowInfo : Row option
          mutable Cursor : Cursor
          /// Store if the cursor has been styled in the last loop. Ex: Hover
          mutable IsCursorStyled : bool
          /// Store if the window should be closed
          mutable ShouldCloseWindow : bool
          mutable CurrentWindow : WindowInfo option
          mutable CurrentCombo : ComboMemory option
          mutable LastReferenceTick : DateTime
          mutable Delta : TimeSpan }

        static member Create (canvas : Browser.HTMLCanvasElement, ?fontSize, ?theme, ?keyboardPreventHandler, ?keyboardCaptureHandler) =
            // Allow canvas interaction
            canvas.setAttribute ("tabindex", "1")
            // Init the context
            let context = canvas.getContext_2d()
            context.textBaseline <- "middle"
            canvas.focus()

            // Init input manager
            Mouse.init canvas
            Keyboard.init canvas true keyboardPreventHandler

            { Canvas = canvas
              Context = context
              Mouse = Mouse.Manager
              Keyboard = Keyboard.Manager
              KeyboardCaptureHandler = keyboardCaptureHandler
              KeyboadHasBeenCapture = false
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
              Delta = TimeSpan.Zero
              CurrentCombo = None }

        /// Reset the state of Hot Item
        /// Need to be called before drawing UI
        member this.Prepare () =
            let now = DateTime.Now
            this.Delta <- now - this.LastReferenceTick

            match this.KeyboardCaptureHandler with
            | Some handler ->
                this.KeyboadHasBeenCapture <- handler this.Keyboard
            | None -> () // Do nothing

            if this.Delta > oneSecond then
                this.LastReferenceTick <- now
                this.Delta <- this.Delta.Subtract(oneSecond)

        /// Reset Active item
        /// Need to be called after drawing UI
        member this.Finish () =
            // Close current window
            if this.CurrentWindow.IsSome then
                this.EndWindow()
            this.CurrentWindow <- None
            this.RowInfo <- None

            match this.CurrentCombo with
            | Some comboInfo ->
                match comboInfo.Reference.State with
                | Closed -> () // Nothing to do
                | Extended | JustExtended ->
                    // Draw the combo
                    // Generate a pseudo layout to
                    // this.CurrentWindow <- Some { WindowInfo.Default with Width = comboInfo.Width
                    //                                                      Height = comboInfo.Height }
                    this.Cursor.X <- comboInfo.X
                    this.Cursor.Y <- comboInfo.Y
                    this.Cursor.Width <- comboInfo.Width

                    let elementSize = this.Theme.Element.Height + this.Theme.Element.SeparatorSize
                    let boxHeight = float comboInfo.Values.Length * elementSize
                    this.Context.fillStyle <- !^this.Theme.Combo.Box.Default.Background
                    this.Context.fillRect(this.Cursor.X, this.Cursor.Y, this.Cursor.Width, boxHeight)

                    for index = 0 to comboInfo.Values.Length - 1 do
                        if comboInfo.Reference.SelectedIndex.IsSome && index = comboInfo.Reference.SelectedIndex.Value then
                            if this.Button(comboInfo.Values.[index],
                                           pressedColor = this.Theme.Combo.Box.Selected.Background,
                                           hoverColor = this.Theme.Combo.Box.Hover.Background,
                                           defaultColor = this.Theme.Combo.Box.Selected.Background,
                                           textPressed = this.Theme.Combo.Box.Selected.Text,
                                           textHover = this.Theme.Combo.Box.Hover.Text,
                                           textDefault = this.Theme.Combo.Box.Selected.Text) then
                                comboInfo.Reference.SelectedIndex <- Some index
                                comboInfo.Reference.State <- Closed
                                this.CurrentCombo <- None
                        else
                            if this.Button(comboInfo.Values.[index],
                                           pressedColor = this.Theme.Combo.Box.Default.Background,
                                           hoverColor = this.Theme.Combo.Box.Hover.Background,
                                           defaultColor = this.Theme.Combo.Box.Default.Background,
                                           textPressed = this.Theme.Combo.Box.Default.Text,
                                           textHover = this.Theme.Combo.Box.Hover.Text,
                                           textDefault = this.Theme.Combo.Box.Default.Text) then
                                comboInfo.Reference.SelectedIndex <- Some index
                                comboInfo.Reference.State <- Closed
                                this.CurrentCombo <- None

                    // if this.Mouse.Left && not (this.IsHover(boxHeight)) then
                    //     comboInfo.Reference.State <- Closed
                    //     this.CurrentCombo <- None

            | None -> () // Nothing to do

            this.Mouse.ResetReleased ()
            this.Mouse.ResetDragInfo ()
            // Reset the cursor style if not styled
            if not this.IsCursorStyled then
                this.Mouse.ResetCursor ()

            // Set value to false
            // Help detecting if the mouse cursor need to be reset on the next loop
            this.IsCursorStyled <- false

            this.KeyboadHasBeenCapture <- false
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
                this.Cursor.Y + elementH > 0. && this.Cursor.Y < window.RealPositionY + this.Theme.Window.Header.Height + window.Height - elementH

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
            // TODO: Handle scroll

        member this.Window(windowInfo : WindowInfo, ?backgroundColor, ?headerColor) =
            if this.CurrentWindow.IsSome then
                this.EndWindow()

            this.CurrentWindow <- Some windowInfo

            // If the Window is closed don't draw it
            if this.CurrentWindow.Value.Closed then
                this.EndWindow()
                false
            else
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
                let headerTextY = this.Cursor.Y - this.Theme.Window.Header.Height / 2.

                match this.CurrentWindow.Value.Title with
                | None -> () // Nothing todo
                | Some title ->
                    let textSize = this.Context.measureText(title)
                    this.Context.fillStyle <- !^this.Theme.Text.Color
                    this.Context.fillText(
                        title,
                        this.Cursor.X + this.Theme.Window.Header.SymbolOffsetX,
                        headerTextY + this.Theme.Text.OffsetY
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
                    let textX = this.Cursor.X + this.Cursor.Width - textSize.width - this.Theme.Window.Header.SymbolOffsetX

                    // Custom IsHover check has we don't follow auto layout management for the header symbol
                    let hoverX = this.Cursor.X + this.Cursor.Width - textSize.width - this.Theme.Window.Header.SymbolOffsetX * 2.
                    let hoverSymbol = this.Mouse.X >= hoverX && this.Mouse.X < (this.Cursor.X + this.Cursor.Width) &&
                                      this.Mouse.Y >= (this.Cursor.Y - this.Theme.Window.Header.Height) && this.Mouse.Y < this.Cursor.Y

                    // Draw symbol background
                    if hoverSymbol then
                        this.Context.fillStyle <- !^this.Theme.Window.Header.OverSymbolColor
                        this.Context.fillRect(
                            textX - this.Theme.Window.Header.SymbolOffsetX,
                            this.Cursor.Y - this.Theme.Window.Header.Height,
                            textSize.width + this.Theme.Window.Header.SymbolOffsetX * 2.,
                            headerTextY + this.Theme.FontSize
                        )
                        if this.Mouse.JustReleased then
                            this.ShouldCloseWindow <- true

                    // Draw symbol
                    this.Context.fillStyle <- !^this.Theme.Text.Color
                    this.Context.fillText("\u2715", textX, headerTextY + this.Theme.Window.Header.SymbolOffsetY)

                // Draw Window background
                this.Context.fillStyle <- !^(defaultArg backgroundColor this.Theme.Window.Background)

                this.Context.fillRect(
                    this.Cursor.X,
                    this.Cursor.Y, // TODO: handle scroll
                    this.Cursor.Width, // TODO: lastMaxX (auto size calculation)
                    this.Cursor.Height // TODO: lastMaxX (auto size calculation)
                )
                true

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

                this.Context.fillStyle <- !^this.Theme.Text.Color
                this.FillSmallString(text, align = align)
                this.EndElement()

        member this.Empty() =
            this.EndElement()

        member this.Button (text, ?align : Align, ?pressedColor, ?hoverColor, ?defaultColor, ?textPressed, ?textHover, ?textDefault) =
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

                this.Context.fillStyle <-
                    if pressed then
                        !^(defaultArg textPressed this.Theme.Text.Color)
                    elif hover then
                        !^(defaultArg textHover this.Theme.Text.Color)
                    else
                        !^(defaultArg textDefault this.Theme.Text.Color)

                this.FillSmallString(text, align = align)

                this.EndElement()

                released

        member this.Combo(comboInfo : ComboInfo, texts : string list, label : string option, ?labelAlign) =
            if not (this.IsVisibile(this.Theme.Element.Height)) then
                this.EndElement()
                false
            else
                let hover = this.IsHover()
                let released = this.IsReleased()
                let pressed = this.IsPressed()

                this.Context.fillStyle <-
                    if pressed then
                        !^this.Theme.Combo.Background.Pressed
                    elif hover then
                        !^this.Theme.Combo.Background.Hover
                    else
                        !^this.Theme.Combo.Background.Default

                let storeCombo () =
                    { Info = comboInfo
                      Values = texts
                      X = this.Cursor.X
                      Y = this.Cursor.Y + this.Theme.Element.Height + this.Theme.Element.SeparatorSize
                      Width = this.Cursor.Width
                      Height = this.Cursor.Height
                      Reference = comboInfo }

                if pressed then
                    if comboInfo.State = Closed then
                        this.CurrentCombo <- storeCombo() |> Some
                        comboInfo.State <- JustExtended

                if released then
                    match comboInfo.State with
                    | JustExtended ->
                        comboInfo.State <- Extended
                    | Extended ->
                        comboInfo.State <- Closed
                    | Closed -> () // Nothing to do

                match comboInfo.State with
                | Extended | JustExtended -> // Update the position. Help handle window drag
                    this.CurrentCombo <- storeCombo() |> Some
                | Closed -> () // Nothing to do

                // if released && this.CurrentCombo.IsSome then//
                //     this.CurrentCombo.Value.Reference.State <- Closed

                this.Context.RoundedRect(
                    this.Cursor.X + this.Theme.ButtonOffsetY,
                    this.Cursor.Y + this.Theme.ButtonOffsetY,
                    this.Cursor.Width - this.Theme.ButtonOffsetY * 2.,
                    this.Theme.Element.Height,
                    this.Theme.Combo.CornerRadius
                )

                let offsetX = this.Cursor.X + this.Cursor.Width - (this.Theme.Arrow.Width + this.Theme.ArrowOffsetX * 2.)
                let offsetY = this.Cursor.Y + this.Theme.ArrowOffsetY

                this.Context.fillStyle <- !^this.Theme.Text.Color

                match comboInfo.SelectedIndex with
                | Some index ->
                    this.FillSmallString(
                        texts.[index],
                        align = defaultArg labelAlign Left
                    )
                | None ->
                    match label with
                    | Some text ->
                        this.FillSmallString(
                            text,
                            align = defaultArg labelAlign Left
                        )
                    | None -> ()

                this.Context.fillStyle <- !^this.Theme.Arrow.Color
                match comboInfo.State with
                | Closed ->
                    // Triangle down
                    this.Context.Triangle(
                        offsetX, offsetY,
                        offsetX + this.Theme.Arrow.Width, offsetY,
                        offsetX + this.Theme.Arrow.Width / 2., offsetY + this.Theme.Arrow.Height
                    )

                | Extended | JustExtended ->
                    // Triangle Up
                    this.Context.Triangle(
                        offsetX + this.Theme.Arrow.Width / 2., offsetY,
                        offsetX + this.Theme.Arrow.Width, offsetY + this.Theme.Arrow.Height,
                        offsetX, offsetY + this.Theme.Arrow.Height
                    )

                this.EndElement()
                true

        member this.DrawArrow(selected, hover) =
            let x = this.Cursor.X + this.Theme.ArrowOffsetX
            let y = this.Cursor.Y + this.Theme.ArrowOffsetY
            this.Context.fillStyle <- !^this.Theme.Arrow.Color
            match selected with
            | true ->
                this.Context.Triangle(
                    x, y,
                    x + this.Theme.Arrow.Width, y,
                    x + this.Theme.Arrow.Width / 2., y + this.Theme.Arrow.Height)
            | false ->
                this.Context.Triangle(
                    x, y,
                    x, y + this.Theme.Arrow.Height,
                    x + this.Theme.Arrow.Width, y + this.Theme.Arrow.Height / 2.)

        member this.Row (ratios) =
            this.RowInfo <- Some { Ratios = ratios
                                   CurrentRatio = 0
                                   SplitX = this.Cursor.X
                                   SplitWidth = this.Cursor.Width }

            this.Cursor.Width <- this.Cursor.Width * this.RowInfo.Value.ActiveRatio

        member this.FillSmallString (text, ?offsetX, ?offsetY, ?align : Align) =
            let offsetY = defaultArg offsetY this.Theme.Text.OffsetY
            let align = defaultArg align Left
            let textSize = this.Context.measureText(text)

            let offsetX =
                match align with
                | Left ->
                    defaultArg offsetX this.Theme.Text.OffsetX
                | Center ->
                    (this.Cursor.Width / 2.) - (textSize.width / 2.)
                | Right ->
                    this.Cursor.Width - textSize.width - this.Theme.Text.OffsetX

            this.Context.font <- this.Theme.FormatFontString this.Theme.FontSmallSize

            let text =
                if textSize.width > this.Cursor.Width - this.Theme.Text.OffsetX then
                    let charSize = this.Context.measureText(" ") // We assume to use a monospace font
                    let maxChar = (this.Cursor.Width - this.Theme.Text.OffsetX) / charSize.width |> int
                    text.Substring(0, maxChar - 2) + ".."
                else
                    text

            this.Context.fillText(
                text,
                this.Cursor.X + offsetX,
                this.Cursor.Y + this.Theme.FontSmallOffsetY + offsetY
            )

        member this.Checkbox(checkboxInfo : CheckboxInfo, label) =
            if not (this.IsVisibile(this.Theme.Element.Height)) then
                this.EndElement()
                false
            else
                let hover = this.IsHover()
                let released = this.IsReleased()

                this.Context.fillStyle <-
                    if checkboxInfo.Value then
                        !^this.Theme.Checkbox.ActiveColor
                    else
                        !^this.Theme.Checkbox.Color

                this.Context.RoundedRect(
                    this.Cursor.X + this.Theme.CheckboxOffsetX,
                    this.Cursor.Y + this.Theme.CheckboxOffsetY,
                    this.Theme.Checkbox.Width,
                    this.Theme.Checkbox.Height,
                    this.Theme.Checkbox.CornerRadius
                )

                if hover && (not checkboxInfo.Value) || checkboxInfo.Value then
                    this.Context.fillStyle <- !^this.Theme.Checkbox.TickColor
                    this.Context.font <- this.Theme.FormatFontString 20.
                    this.Context.fillText(
                        "\u2713",
                        this.Cursor.X + this.Theme.Checkbox.Width / 2.,
                        this.Cursor.Y + this.Theme.Element.Height / 2.
                    )

                // Label section
                this.Context.fillStyle <- !^this.Theme.Text.Color
                this.FillSmallString(
                    label,
                    this.Theme.Checkbox.Width + this.Theme.CheckboxOffsetX * 2.
                )

                if released then
                    checkboxInfo.Value <- not checkboxInfo.Value

                this.EndElement()
                checkboxInfo.Value

        member this.Input(info: InputInfo) =
            if not (this.IsVisibile(this.Theme.Element.Height)) then
                this.EndElement()
                false
            else
                let hover = this.IsHover()
                let pressed = this.IsPressed()

                if hover then
                    this.SetCursor Mouse.Cursor.Text

                if pressed then
                    info.IsActive <- true

                this.Context.strokeStyle <-
                    if info.IsActive then
                        !^this.Theme.Input.Border.Active
                    else
                        !^this.Theme.Input.Border.Default

                this.Context.fillStyle <-
                    if info.IsActive then
                        !^this.Theme.Input.Background.Active
                    else
                        !^this.Theme.Input.Background.Default

                this.Context.lineWidth <- 2.

                this.Context.RoundedRect(
                    this.Cursor.X + this.Theme.ButtonOffsetY,
                    this.Cursor.Y + this.Theme.ButtonOffsetY,
                    this.Cursor.Width - this.Theme.ButtonOffsetY * 2.,
                    this.Theme.Element.Height,
                    this.Theme.Element.CornerRadius,
                    StrokeAndFill
                )

                // Custom way to draw text because we need to handle text offset when being larger than the input
                this.Context.font <- this.Theme.FormatFontString this.Theme.FontSmallSize

                let offsetY = this.Theme.Text.OffsetY
                let textSize = this.Context.measureText(info.Value)
                let offsetX = this.Theme.Text.OffsetX

                let charSize = this.Context.measureText(" ") // We assume to use a monospace font
                let maxChar = (this.Cursor.Width - this.Theme.Text.OffsetX * 2.) / charSize.width |> int

                let text =
                    if textSize.width > this.Cursor.Width - this.Theme.Text.OffsetX * 2. then
                        info.Value.Substring(info.TextStartOrigin , maxChar)
                    else
                        info.Value

                // Handle selection case
                match info.Selection with
                | Some selection ->
                    let selectionSize =
                        info.Value.Substring(
                            selection.Start,
                            selection.Length
                        )
                        |> this.Context.measureText

                    let startX =
                        // If the selection is from the beginning of the line. Nothing to change
                        if selection.Start = 0 then
                            this.Cursor.X
                        else
                            // If the selection is offset, calculate the offset and draw the selection from it
                            let leftSize =
                                info.Value.Substring(0, selection.Start)
                                |> this.Context.measureText
                            this.Cursor.X + leftSize.width

                    let selectionHeight =
                        this.Theme.FontSmallSize * 1.5

                    this.Context.fillStyle <- !^this.Theme.Input.SelectionColor
                    this.Context.fillRect(
                        startX + this.Theme.Text.OffsetX,
                        this.Cursor.Y + (this.Theme.Element.Height - selectionHeight) / 2.,
                        Math.Min(selectionSize.width, charSize.width * float maxChar),
                        selectionHeight
                    )
                | None -> () // Nothing to do

                // Draw text after the selection.
                this.Context.fillStyle <- !^this.Theme.Input.TextColor
                this.Context.fillText(
                    text,
                    this.Cursor.X + offsetX,
                    this.Cursor.Y + this.Theme.FontSmallOffsetY + offsetY
                )

                // Cursor
                if info.IsActive && this.Delta < TimeSpan.FromMilliseconds(500.) && info.Selection.IsNone then

                    this.Context.fillStyle <- !^"#000"

                    let cursorMetrics = this.Context.measureText("|")
                    let textMetrics = this.Context.measureText(info.Value)
                    let cursorOffsetMetrics =
                      info.Value.Substring(Math.Min(info.CursorOffset, maxChar))
                      |> this.Context.measureText

                    this.FillSmallString(
                        "|",
                        textMetrics.width - cursorMetrics.width / 2. - cursorOffsetMetrics.width + this.Theme.Text.OffsetX
                    )

                let handleBackwardSelection newCursorOffset =
                    if info.Value.Length > 0 then
                        let oldCursorOffset = info.CursorOffset
                        info.CursorOffset <- newCursorOffset
                        match info.Selection with
                        | Some selection ->
                            if oldCursorOffset = selection.Start then
                                { selection with Start = info.CursorOffset }
                            else
                                { selection with End = newCursorOffset }
                        | None ->
                            SelectionArea.Create(
                                info.CursorOffset,
                                oldCursorOffset
                            )
                        |> info.SetSelection

                let handleForwardSelection newCursorOffset =
                    if info.Value.Length > 0 then
                        let oldCursorOffset = info.CursorOffset
                        info.CursorOffset <- newCursorOffset
                        match info.Selection with
                        | Some selection ->
                            if selection.End = oldCursorOffset then
                                { selection with End = info.CursorOffset }
                            else
                                { selection with Start = newCursorOffset }
                        | None ->
                            SelectionArea.Create(
                                oldCursorOffset,
                                info.CursorOffset
                            )
                        |> info.SetSelection

                if info.IsActive then
                    if this.Keyboard.HasNewKeyStroke() then
                        // Memorise if we capture the keystroke
                        // Example: Ctrl, Arrows are capture. Letters are not
                        let isCapture =
                            let mutable res = true
                            // First we resolve the action depending on modifiers
                            match this.Keyboard.Modifiers with
                            | { Control = true; Shift = true } ->
                                match this.Keyboard.LastKey with
                                | Keyboard.Keys.ArrowLeft ->
                                    NextIndexBackward(info.Value, ' ', info.CursorOffset + info.TextStartOrigin)
                                    |> handleBackwardSelection
                                | Keyboard.Keys.ArrowRight ->
                                    NextIndexForward(info.Value, ' ', info.CursorOffset + info.TextStartOrigin)
                                    |> handleForwardSelection
                                | _ -> res <- false // Not captured
                            | { Control = true } ->
                                info.ClearSelection()
                                match this.Keyboard.LastKey with
                                | Keyboard.Keys.ArrowLeft ->
                                    if info.Value.Length > 0 then
                                        let oldCursorOffset = info.CursorOffset
                                        let index = NextIndexBackward(info.Value, ' ', info.CursorOffset + info.TextStartOrigin)
                                        if index - info.TextStartOrigin < 0 then
                                            info.CursorOffset <- 0
                                            info.TextStartOrigin <- index
                                        else
                                            info.CursorOffset <- index - info.TextStartOrigin
                                | Keyboard.Keys.ArrowRight ->
                                    if info.Value.Length > 0 then
                                        info.CursorOffset <- NextIndexForward(info.Value, ' ', info.CursorOffset + info.TextStartOrigin)
                                | Keyboard.Keys.Backspace ->
                                    if info.Value.Length > 0 then
                                        let index = NextIndexBackward(info.Value, ' ', info.CursorOffset)
                                        let delta = info.CursorOffset - index
                                        info.Value <- info.Value.Remove(index, delta)
                                        info.CursorOffset <- info.CursorOffset - delta
                                | Keyboard.Keys.Delete ->
                                    if info.Value.Length > 0 then
                                        let index = NextIndexForward(info.Value, ' ', info.CursorOffset)
                                        info.Value <- info.Value.Remove(info.CursorOffset, index - info.CursorOffset)
                                | Keyboard.Keys.A ->
                                    if info.Value.Length > 0 then
                                        info.Selection <- Some (SelectionArea.Create(0, info.Value.Length))
                                        info.CursorOffset <- info.Value.Length
                                | _ -> res <- false // Not captured
                            | { Shift = true } ->
                                match this.Keyboard.LastKey with
                                | Keyboard.Keys.ArrowLeft ->
                                    Math.Max(0, info.CursorOffset - 1)
                                    |> handleBackwardSelection
                                | Keyboard.Keys.ArrowRight ->
                                    Math.Min(info.CursorOffset + 1, info.Value.Length)
                                    |> handleForwardSelection
                                | _ -> res <- false // Not captured
                            | _ ->
                                let oldSelection =
                                    if info.Selection.IsSome then
                                        true, info.Selection.Value
                                    else
                                        false, SelectionArea.Create(0, 0)
                                info.ClearSelection()
                                match this.Keyboard.LastKey with
                                | Keyboard.Keys.Backspace ->
                                    if info.Value.Length > 0 then
                                        match oldSelection with
                                        | (true, selection) ->
                                            info.Value <- info.Value.Remove(selection.Start, selection.Length)
                                            if info.CursorOffset = selection.End then
                                                info.CursorOffset <- Math.Max(info.CursorOffset - selection.Length, 0)
                                        | (false, _) ->
                                            if info.CursorOffset > 0 then
                                                if info.TextStartOrigin > 0 then
                                                    info.TextStartOrigin <- info.TextStartOrigin - 1
                                                    info.Value <- info.Value.Remove(info.TextStartOrigin + info.CursorOffset, 1)
                                                else
                                                    info.CursorOffset <- info.CursorOffset - 1
                                                    info.Value <- info.Value.Remove(info.CursorOffset, 1)
                                | Keyboard.Keys.Delete ->
                                    if info.Value.Length > 0 then
                                        match oldSelection with
                                        | (true, selection) ->
                                            info.Value <- info.Value.Remove(selection.Start, selection.Length)
                                            if info.CursorOffset = selection.End then
                                                info.CursorOffset <- Math.Max(info.CursorOffset - selection.Length, 0)
                                        | (false, _) ->
                                            if info.CursorOffset < info.Value.Length then
                                                info.Value <- info.Value.Remove(info.CursorOffset, 1)
                                | Keyboard.Keys.ArrowLeft ->
                                    match oldSelection with
                                    | (true, selection) ->
                                        if selection.Edging info.Value.Length then
                                            info.CursorOffset <- 0
                                            info.TextStartOrigin <- 0
                                        else
                                            info.CursorOffset <- Math.Max(0, info.CursorOffset - 1)
                                    | (false, _) ->
                                        info.CursorOffset <- Math.Max(0, info.CursorOffset - 1)

                                    if info.CursorOffset = 0 then
                                        if info.TextStartOrigin >= 1 then
                                            info.TextStartOrigin <- info.TextStartOrigin - 1
                                        else
                                            info.TextStartOrigin <- 0
                                | Keyboard.Keys.ArrowRight ->
                                    match oldSelection with
                                    | (true, selection) ->
                                        if selection.Edging info.Value.Length then
                                            info.CursorOffset <- info.Value.Length
                                        else
                                            info.CursorOffset <- Math.Min(info.CursorOffset + 1, Math.Min(maxChar, info.Value.Length))
                                    | (false, _) ->
                                        info.CursorOffset <- Math.Min(info.CursorOffset + 1, Math.Min(maxChar, info.Value.Length))

                                    if info.CursorOffset >= maxChar then
                                        info.TextStartOrigin <- Math.Min(info.Value.Length - maxChar, info.TextStartOrigin + 1)
                                        info.CursorOffset <- maxChar
                                | Keyboard.Keys.Home ->
                                    info.CursorOffset <- 0
                                    info.TextStartOrigin <- 0
                                | Keyboard.Keys.End ->
                                    if info.Value.Length > maxChar then
                                        info.CursorOffset <- maxChar
                                        info.TextStartOrigin <- info.Value.Length - maxChar
                                    else
                                        info.CursorOffset <- info.Value.Length
                                | Keyboard.Keys.Escape ->
                                    info.ClearSelection()
                                | _ -> res <- false // Not captured

                            // Neutralize the selection (prevent visual bugs)
                            match info.Selection with
                            | Some selection ->
                                // If the selection is "empty" then remove it
                                if selection.Start = selection.End then
                                    info.Selection <- None
                                elif selection.Start > selection.End then
                                    info.Selection <- Some (SelectionArea.Create(selection.End, selection.Start))
                            | None -> () // Nothing to do

                            if not res then
                                res <- this.KeyboadHasBeenCapture

                            res

                        if not isCapture && this.Keyboard.LastKeyIsPrintable then
                            info.Value <- info.Value.Insert(info.CursorOffset + info.TextStartOrigin, this.Keyboard.LastKeyValue)
                            info.CursorOffset <- info.CursorOffset + 1
                            if info.CursorOffset > maxChar then
                                info.CursorOffset <- maxChar
                                info.TextStartOrigin <- info.TextStartOrigin + 1
                            info.ClearSelection()

                this.EndElement()
                true

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
