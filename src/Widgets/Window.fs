namespace Hink.Widgets

open Hink.Gui
open Fable.Core
open Fable.Core.JsInterop
open Hink.Inputs
open Hink.Helpers

[<AutoOpen>]
module Window =

    type Hink with
        member this.EndWindow() =
            if not this.CurrentWindow.Value.Closed then
                // Set if the window has been closed during this loop
                this.CurrentWindow.Value.Closed <- this.ShouldCloseWindow
                this.ShouldCloseWindow <- false

            if not (isNull this.CurrentContext) then
                this.ApplicationContext.drawImage(
                    !^this.CurrentContext.canvas,
                    this.CurrentWindow.Value.RealPositionX,
                    this.CurrentWindow.Value.RealPositionY
                )

            this.CurrentWindow.Value.ShouldRedraw <- false

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
                this.Cursor.X <- 0.// this.CurrentWindow.Value.RealPositionX
                this.Cursor.Y <- 0.// this.CurrentWindow.Value.RealPositionY + this.Theme.Window.Header.Height  // TODO: handle scroll
                this.Cursor.Width <- windowInfo.Width
                this.Cursor.Height <- windowInfo.Height

                if not (this.IsHover(windowInfo.Height) || windowInfo.ShouldRedraw || windowInfo.IsDragging) then
                    false // Don't need to draw the window the mouse isn't hover it
                else
                    // Make sure we have the Window context initialized
                    this.CurrentWindow.Value.EnsureContext()

                    // Offset the cursor by the header height
                    // We need to do it after checking if we need to redraw the window
                    // otherwise the header is ignored in the isHover function
                    this.Cursor.Y <- this.Theme.Window.Header.Height

                    // Clear the window context for the new frame
                    this.CurrentContext.clearRect(
                        0.,
                        0.,
                        this.CurrentContext.canvas.width,
                        this.CurrentContext.canvas.height
                    )

                    // Draw Window header
                    this.CurrentContext.fillStyle <- !^(defaultArg headerColor this.Theme.Window.Header.Color)
                    this.CurrentContext.fillRect(
                        this.Cursor.X,
                        this.Cursor.Y - this.Theme.Window.Header.Height,
                        this.Cursor.Width,
                        this.Theme.Window.Header.Height
                    )

                    // Set the text font for the header
                    this.CurrentContext.font <- this.Theme.FormatFontString this.Theme.FontSmallSize
                    // Common Y position for the title and the symbol
                    let headerTextY = this.Cursor.Y - this.Theme.Window.Header.Height / 2.

                    match this.CurrentWindow.Value.Title with
                    | None -> () // Nothing todo
                    | Some title ->
                        let textSize = this.CurrentContext.measureText(title)
                        this.CurrentContext.fillStyle <- !^this.Theme.Text.Color
                        this.CurrentContext.fillText(
                            title,
                            this.Cursor.X + this.Theme.Window.Header.SymbolOffsetX,
                            headerTextY + this.Theme.Text.OffsetY
                        )

                    if this.CurrentWindow.Value.Draggable then
                        let headerOriginY = this.CursorPosY - this.Theme.Window.Header.Height
                        let hoverHeader = this.Mouse.X >= this.CursorPosX && this.Mouse.X < (this.CursorPosX + this.Cursor.Width) && this.Mouse.Y >= headerOriginY && this.Mouse.Y < (headerOriginY + this.Theme.Window.Header.Height)

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
                        let textSize = this.CurrentContext.measureText("\u2715")
                        let textX = this.Cursor.X + this.Cursor.Width - textSize.width - this.Theme.Window.Header.SymbolOffsetX

                        // Custom IsHover check has we don't follow auto layout management for the header symbol
                        let hoverX = this.CursorPosX + this.Cursor.Width - textSize.width - this.Theme.Window.Header.SymbolOffsetX * 2.
                        let hoverSymbol = this.Mouse.X >= hoverX && this.Mouse.X < (this.CursorPosX + this.Cursor.Width) &&
                                          this.Mouse.Y >= (this.CursorPosY - this.Theme.Window.Header.Height) && this.Mouse.Y < this.CursorPosY

                        // printfn "Cursor pos: %O" this.CursorPosX
                        // printfn "Mouse pos: %O" this.Mouse.X

                        // Draw symbol background
                        if hoverSymbol then
                            this.CurrentContext.fillStyle <- !^this.Theme.Window.Header.OverSymbolColor
                            this.CurrentContext.fillRect(
                                textX - this.Theme.Window.Header.SymbolOffsetX,
                                this.Cursor.Y - this.Theme.Window.Header.Height,
                                textSize.width + this.Theme.Window.Header.SymbolOffsetX * 2.,
                                headerTextY + this.Theme.FontSize
                            )
                            if this.Mouse.JustReleased then
                                this.ShouldCloseWindow <- true

                        // Draw symbol
                        this.CurrentContext.fillStyle <- !^this.Theme.Text.Color
                        this.CurrentContext.fillText("\u2715", textX, headerTextY + this.Theme.Window.Header.SymbolOffsetY)

                    // Draw Window background
                    this.CurrentContext.fillStyle <- !^(defaultArg backgroundColor this.Theme.Window.Background)

                    this.CurrentContext.fillRect(
                        this.Cursor.X,
                        this.Cursor.Y, // TODO: handle scroll
                        this.Cursor.Width, // TODO: lastMaxX (auto size calculation)
                        this.Cursor.Height // TODO: lastMaxX (auto size calculation)
                    )
                    true
