namespace Hink

open Fable.Core
open Fable.Core.JsInterop
open Hink.Inputs
open Hink.Theme
open System
open Browser.Types
open Browser.Dom
open Browser
open Browser.Css

module Gui =

    type ID = string

    type LayoutOrientation =
        | Horizontal
        | Vertical

    type Align =
        | Left
        | Center
        | Right

    let oneSecond = TimeSpan.FromSeconds 1.

    type Cursor =
        {   mutable X : float
            mutable Y : float
            mutable Width : float
            mutable Height : float }

    type SelectionArea =
        {   Start: int
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
            {   Start = start
                End = ``end`` }

    type DragInfo =
        {   OriginX : float
            OriginY : float }

    type WindowHandler =
        {   mutable X : float
            mutable Y : float
            mutable Width : float
            mutable Height : float
            mutable Layout : LayoutOrientation
            mutable Draggable : bool
            mutable Closable : bool
            mutable Closed : bool
            mutable Title : string option
            mutable DragXOrigin : float
            mutable DragYOrigin : float
            mutable IsDragging : bool
            mutable ShouldRedraw : bool
            mutable _Canvas : HTMLCanvasElement option
            mutable _Context : CanvasRenderingContext2D option }

        static member Default =
            {   X = 0.
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
                IsDragging = false
                ShouldRedraw = true // Set to true to draw the first frame even if the mouse is not hover the window
                _Canvas = None
                _Context = None }

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

        member this.EnsureContext () =
            if this._Canvas.IsNone then
                let canvas = document.createElement("canvas") :?> HTMLCanvasElement
                canvas.width <- this.Width
                canvas.height <- this.Height
                canvas.style.width <- string this.Width + "px"
                canvas.style.height <- string this.Height + "px"

                this._Canvas <- Some canvas
                this._Context <- canvas.getContext_2d() |> Some
                this._Context.Value.textBaseline <- "middle"

    type CheckboxInfo =
        { mutable Value : bool }

        static member Default =
            { Value = false }

    type ComboState =
        | Extended
        | JustExtended
        | Closed

    type ComboInfo =
        {   mutable SelectedIndex : int option
            mutable State : ComboState
            Guid : Guid }

        static member Default
            with get () =
                {   SelectedIndex = None
                    State = Closed
                    Guid = Guid.NewGuid() }

    /// Type used to stored combo information for drawing when Finish the loop
    type ComboHandler =
        {   Info : ComboInfo
            X : float
            Y : float
            Width : float
            Height : float
            Values : string list
            Reference : ComboInfo }

    type Row =
        {   mutable Ratios : float []
            mutable CurrentRatio : int
            mutable SplitX : float
            mutable SplitWidth : float }

        member this.ActiveRatio
            with get () = this.Ratios.[this.CurrentRatio]

    type InputHandler =
        {   mutable Value : string
            mutable Selection : SelectionArea option
            mutable KeyboardCaptureHandler : (InputHandler -> Keyboard.Record -> bool) option
            // Positive offset of the cursor.
            // Offset of 0 = start of the input
            // Offset of 2 = cursor place after the second char of the input
            mutable CursorOffset : int
            // Start origin of the text to display
            // 0 = Start of the text
            // 2 = Start of the text after the 2 first chars
            mutable TextStartOrigin : int
            mutable CursorDraggingOriginOffset : int option
            Guid : Guid }

            member this.ClearSelection () =
                this.Selection <- None

            member this.SetSelection (value) =
                this.Selection <- Some value

            member this.Empty() =
                this.Value <- ""
                this.Selection <- None
                this.CursorOffset <- 0
                this.TextStartOrigin <- 0

            static member Default
                with get () = { Value = ""
                                KeyboardCaptureHandler = None
                                Selection = None
                                CursorOffset = 0
                                TextStartOrigin = 0
                                Guid = Guid.NewGuid()
                                CursorDraggingOriginOffset = None }

        type SliderOrientation =
            | Vertical
            | Horizontal

    type SliderHandler =
        {   Guid : Guid
            mutable Value : float }
        //   Max : float
        //   Min : float
        //   Value : float
        //   Step : float
        //   Orientation : SliderOrientation }

        static member Default
            with get () = { Guid = Guid.NewGuid()
                            Value = 0. }
                            // Max = 100.
                            // Min = 0.
                            // Value = 0.
                            // Step = 10.
                            // Orientation = SliderOrientation.Horizontal }

    type Hink =
        {   Canvas : HTMLCanvasElement
            ApplicationContext : CanvasRenderingContext2D
            Mouse : Mouse.Record
            Keyboard : Keyboard.Record
            KeyboardCaptureHandler : (Keyboard.Record -> bool) option
            mutable ActiveWidget : Guid option
            mutable KeyboadHasBeenCapture : bool
            Theme : Theme
            mutable RowInfo : Row option
            mutable Cursor : Cursor
            /// Store if the cursor has been styled in the last loop. Ex: Hover
            mutable IsCursorStyled : bool
            /// Store if the window should be closed
            mutable ShouldCloseWindow : bool
            mutable CurrentWindow : WindowHandler option
            mutable CurrentCombo : ComboHandler option
            mutable LastReferenceTick : DateTime
            mutable Delta : TimeSpan }

            static member Create (canvas : HTMLCanvasElement, ?fontSize, ?theme, ?keyboardPreventHandler, ?keyboardCaptureHandler) =
                // Allow canvas interaction
                canvas.setAttribute ("tabindex", "1")
                // Init the context
                let context = canvas.getContext_2d()
                context.textBaseline <- "middle"
                canvas.focus()

                // Init input manager
                Mouse.init canvas
                Keyboard.init canvas true keyboardPreventHandler
                Clipboard.init canvas

                {   Canvas = canvas
                    ApplicationContext = context
                    Mouse = Mouse.Manager
                    Keyboard = Keyboard.Manager
                    ActiveWidget = None
                    KeyboardCaptureHandler = keyboardCaptureHandler
                    KeyboadHasBeenCapture = false
                    IsCursorStyled = false
                    Theme = defaultArg theme darkTheme
                    Cursor =
                        {   X = 0.
                            Y = 0.
                            Width = 0.
                            Height = 0. }
                    CurrentWindow = None
                    ShouldCloseWindow = false
                    RowInfo = None
                    LastReferenceTick = DateTime.Now
                    Delta = TimeSpan.Zero
                    CurrentCombo = None }

        member this.CursorPosX
            with get () =
                match this.CurrentWindow with
                | Some current -> this.Cursor.X + current.RealPositionX
                | None -> this.Cursor.X

        member this.CursorPosY
            with get () =
                match this.CurrentWindow with
                | Some current -> this.Cursor.Y + current.RealPositionY
                | None -> this.Cursor.Y

        member this.CurrentContext
            with get () : CanvasRenderingContext2D =
                if this.CurrentWindow.IsSome then
                    this.CurrentWindow.Value._Context.Value
                else
                    // failwith "Widgets need to be draw inside a Window for now"
                    //this.ApplicationContext
                    this.ApplicationContext

        member this.SetActiveWidget guid =
            this.ActiveWidget <- Some guid

        member this.IsActive guid =
            this.ActiveWidget.IsSome && this.ActiveWidget.Value = guid

        member this.SetCursor cursor =
            this.Mouse.SetCursor cursor
            this.IsCursorStyled <- true

        member this.IsVisible(elementH) =
            match this.CurrentWindow with
            | None -> true
            | Some { Closed = true } ->
                false
            | Some window ->
                this.Cursor.Y + elementH > 0. && this.Cursor.Y < window.RealPositionY + this.Theme.Window.Header.Height + window.Height - elementH

        member this.IsHover(?elementH) =
            let elementH = defaultArg elementH this.Theme.Element.Height
            this.Mouse.X >= this.CursorPosX && this.Mouse.X < (this.CursorPosX + this.Cursor.Width) &&
            this.Mouse.Y >= this.CursorPosY && this.Mouse.Y < (this.CursorPosY + elementH)

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

            | Some { Layout = LayoutOrientation.Vertical } ->
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

            | Some { Layout = LayoutOrientation.Horizontal } ->
                this.Cursor.X <- this.Cursor.Width + this.Theme.Element.SeparatorSize

        member this.Empty() =
            this.EndElement()

        member this.DrawArrow(selected, hover) =
            let x = this.Cursor.X + this.Theme.ArrowOffsetX
            let y = this.Cursor.Y + this.Theme.ArrowOffsetY
            this.CurrentContext.fillStyle <- !^this.Theme.Arrow.Color
            match selected with
            | true ->
                this.CurrentContext.Triangle(
                    x, y,
                    x + this.Theme.Arrow.Width, y,
                    x + this.Theme.Arrow.Width / 2., y + this.Theme.Arrow.Height)
            | false ->
                this.CurrentContext.Triangle(
                    x, y,
                    x, y + this.Theme.Arrow.Height,
                    x + this.Theme.Arrow.Width, y + this.Theme.Arrow.Height / 2.)

        member this.FillSmallString (text, ?offsetX, ?offsetY, ?width, ?align : Align) =
            let offsetY = defaultArg offsetY this.Theme.Text.OffsetY
            let align = defaultArg align Left
            let textSize = this.CurrentContext.measureText(text)
            let width = defaultArg width this.Cursor.Width

            let offsetX =
                match align with
                | Left ->
                    defaultArg offsetX this.Theme.Text.OffsetX
                | Center ->
                    (width / 2.) - (textSize.width / 2.)
                | Right ->
                    width - textSize.width - this.Theme.Text.OffsetX

            this.CurrentContext.font <- this.Theme.FormatFontString this.Theme.FontSmallSize

            let text =
                if textSize.width > width - this.Theme.Text.OffsetX then
                    let charSize = this.CurrentContext.measureText(" ") // We assume to use a monospace font
                    let maxChar = (width - this.Theme.Text.OffsetX) / charSize.width |> int
                    text.Substring(0, maxChar - 2) + ".."
                else
                    text

            this.CurrentContext.fillText(
                text,
                this.Cursor.X + offsetX,
                this.Cursor.Y + this.Theme.FontSmallOffsetY + offsetY
            )

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
