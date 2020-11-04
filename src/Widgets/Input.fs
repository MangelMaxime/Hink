namespace Hink.Widgets

open Hink.Gui
open Fable.Core
open Fable.Core.JsInterop
open Hink.Inputs
open Hink.Helpers
open System

[<AutoOpen>]
module Input =

    let handleBackwardSelection (info: InputHandler) newCursorOffset =
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

    let handleForwardSelection (info: InputHandler) newCursorOffset =
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

    type Hink with
        member this.Input(info: InputHandler) =
            if not (this.IsVisible(this.Theme.Element.Height)) then
                this.EndElement()
                false
            else
                let hover = this.IsHover()
                let pressed = this.IsPressed()

                if hover then
                    this.SetCursor Mouse.Cursor.Text

                if pressed then
                    this.SetActiveWidget info.Guid

                this.CurrentContext.strokeStyle <-
                    if this.IsActive info.Guid then
                        !^this.Theme.Input.Border.Active
                    else
                        !^this.Theme.Input.Border.Default

                this.CurrentContext.fillStyle <-
                    if this.IsActive info.Guid then
                        !^this.Theme.Input.Background.Active
                    else
                        !^this.Theme.Input.Background.Default

                this.CurrentContext.lineWidth <- 2.

                this.CurrentContext.RoundedRect(
                    this.Cursor.X + this.Theme.ButtonOffsetY,
                    this.Cursor.Y + this.Theme.ButtonOffsetY,
                    this.Cursor.Width - this.Theme.ButtonOffsetY * 2.,
                    this.Theme.Element.Height,
                    this.Theme.Element.CornerRadius,
                    StrokeAndFill
                )

                // Custom way to draw text because we need to handle text offset when being larger than the input
                this.CurrentContext.font <- this.Theme.FormatFontString this.Theme.FontSmallSize

                let offsetY = this.Theme.Text.OffsetY
                let textSize = this.CurrentContext.measureText(info.Value)
                let offsetX = this.Theme.Text.OffsetX

                let charSize = this.CurrentContext.measureText(" ") // We assume to use a monospace font
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
                        |> this.CurrentContext.measureText

                    let startX =
                        // If the selection is from the beginning of the line. Nothing to change
                        if selection.Start = 0 then
                            this.Cursor.X
                        else
                            // If the selection is offset, calculate the offset and draw the selection from it
                            let leftSize =
                                info.Value.Substring(0, selection.Start)
                                |> this.CurrentContext.measureText
                            this.Cursor.X + leftSize.width

                    let selectionHeight =
                        this.Theme.FontSmallSize * 1.5

                    this.CurrentContext.fillStyle <- !^this.Theme.Input.SelectionColor
                    this.CurrentContext.fillRect(
                        startX + this.Theme.Text.OffsetX,
                        this.Cursor.Y + (this.Theme.Element.Height - selectionHeight) / 2.,
                        Math.Min(selectionSize.width, charSize.width * float maxChar),
                        selectionHeight
                    )
                | None -> () // Nothing to do

                // Draw text after the selection.
                this.CurrentContext.fillStyle <- !^this.Theme.Input.TextColor
                this.CurrentContext.fillText(
                    text,
                    this.Cursor.X + offsetX,
                    this.Cursor.Y + this.Theme.FontSmallOffsetY + offsetY
                )

                // Cursor
                if this.IsActive info.Guid && this.Delta < TimeSpan.FromMilliseconds(500.) && info.Selection.IsNone then

                    this.CurrentContext.fillStyle <- !^"#000"

                    let cursorMetrics = this.CurrentContext.measureText("|")
                    let textMetrics = this.CurrentContext.measureText(info.Value)
                    let cursorOffsetMetrics =
                      info.Value.Substring(Math.Min(info.CursorOffset, maxChar))
                      |> this.CurrentContext.measureText

                    this.FillSmallString(
                        "|",
                        textMetrics.width - cursorMetrics.width / 2. - cursorOffsetMetrics.width + this.Theme.Text.OffsetX
                    )

                if this.IsActive info.Guid then
                    if this.Keyboard.HasNewKeyStroke() then
                        // Memorise if we capture the keystroke
                        // Example: Ctrl, Arrows are capture. Letters are not
                        let isCapture =
                            // Let first the user capture the keystroke so he can cancel it if wanted
                            let capturedByUser =
                                match info.KeyboardCaptureHandler with
                                | Some handler ->
                                    handler info this.Keyboard
                                | None -> false // Do nothing

                            let mutable res = true

                            if not capturedByUser then
                                // First we resolve the action depending on modifiers
                                match this.Keyboard.Modifiers with
                                | { Control = true; Shift = true } ->
                                    match this.Keyboard.LastKey with
                                    | Keyboard.Keys.ArrowLeft ->
                                        NextIndexBackward(info.Value, ' ', info.CursorOffset + info.TextStartOrigin)
                                        |> handleBackwardSelection info
                                    | Keyboard.Keys.ArrowRight ->
                                        NextIndexForward(info.Value, ' ', info.CursorOffset + info.TextStartOrigin)
                                        |> handleForwardSelection info
                                    | _ -> res <- false // Not captured
                                | { Control = true } ->
                                    match this.Keyboard.LastKey with
                                    | Keyboard.Keys.ArrowLeft ->
                                        info.ClearSelection()
                                        if info.Value.Length > 0 then
                                            let oldCursorOffset = info.CursorOffset
                                            let index = NextIndexBackward(info.Value, ' ', info.CursorOffset + info.TextStartOrigin)
                                            if index - info.TextStartOrigin < 0 then
                                                info.CursorOffset <- 0
                                                info.TextStartOrigin <- index
                                            else
                                                info.CursorOffset <- index - info.TextStartOrigin
                                    | Keyboard.Keys.ArrowRight ->
                                        info.ClearSelection()
                                        if info.Value.Length > 0 then
                                            info.CursorOffset <- NextIndexForward(info.Value, ' ', info.CursorOffset + info.TextStartOrigin)
                                    | Keyboard.Keys.Backspace ->
                                        info.ClearSelection()
                                        if info.Value.Length > 0 then
                                            let index = NextIndexBackward(info.Value, ' ', info.CursorOffset)
                                            let delta = info.CursorOffset - index
                                            info.Value <- info.Value.Remove(index, delta)
                                            info.CursorOffset <- info.CursorOffset - delta
                                    | Keyboard.Keys.Delete ->
                                        info.ClearSelection()
                                        if info.Value.Length > 0 then
                                            let index = NextIndexForward(info.Value, ' ', info.CursorOffset)
                                            info.Value <- info.Value.Remove(info.CursorOffset, index - info.CursorOffset)
                                    | Keyboard.Keys.A ->
                                        info.ClearSelection()
                                        if info.Value.Length > 0 then
                                            info.Selection <- Some (SelectionArea.Create(0, info.Value.Length))
                                            info.CursorOffset <- info.Value.Length
                                    | Keyboard.Keys.C ->
                                        match info.Selection with
                                        | Some selection ->
                                            let textToCopy = info.Value.Substring(selection.Start, selection.End)
                                            Clipboard.copyToClipboard textToCopy
                                        | None ->
                                            ()
                                    | Keyboard.Keys.X ->

                                        match info.Selection with
                                        | Some selection ->
                                            let textToCopy = info.Value.Substring(selection.Start, selection.Length)
                                            Clipboard.copyToClipboard textToCopy

                                            info.ClearSelection()

                                            info.Value <- info.Value.Remove(selection.Start, selection.Length)
                                            if info.CursorOffset = selection.End then
                                                info.CursorOffset <- Math.Max(info.CursorOffset - selection.Length, 0)
                                        | None ->
                                            ()
                                    | _ -> res <- false // Not captured
                                | { Shift = true } ->
                                    match this.Keyboard.LastKey with
                                    | Keyboard.Keys.ArrowLeft ->
                                        Math.Max(0, info.CursorOffset - 1)
                                        |> handleBackwardSelection info
                                    | Keyboard.Keys.ArrowRight ->
                                        Math.Min(info.CursorOffset + 1, info.Value.Length)
                                        |> handleForwardSelection info
                                    | _ -> res <- false // Not captured
                                | _ ->
                                    let oldSelection =
                                        if info.Selection.IsSome then
                                            true, info.Selection.Value
                                        else
                                            false, SelectionArea.Create(0, 0)
                                    match this.Keyboard.LastKey with
                                    | Keyboard.Keys.Backspace ->
                                        info.ClearSelection()
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
                                        info.ClearSelection()
                                        if info.Value.Length > 0 then
                                            match oldSelection with
                                            | (true, selection) ->
                                                info.Value <- info.Value.Remove(selection.Start, selection.Length)
                                                if info.CursorOffset = selection.End then
                                                    info.CursorOffset <- Math.Max(info.CursorOffset - selection.Length, 0)
                                            | (false, _) ->
                                                if info.CursorOffset < info.Value.Length then
                                                    info.Value <- info.Value.Remove(info.CursorOffset, 1)
                                            // If the input is empty, make sure to reset the start origin
                                            // Usefull when doing Ctrl + A -> Delete
                                            // over a string longer than the input max size
                                            if info.Value.Length = 0 then
                                                info.TextStartOrigin <- 0
                                    | Keyboard.Keys.ArrowLeft ->
                                        info.ClearSelection()
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
                                        info.ClearSelection()
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
                                        info.ClearSelection()
                                        info.CursorOffset <- 0
                                        info.TextStartOrigin <- 0
                                    | Keyboard.Keys.End ->
                                        info.ClearSelection()
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
                            match info.Selection with
                            | Some selection ->
                                info.Value <- info.Value.Remove(selection.Start, selection.Length)
                                if info.CursorOffset = selection.End then
                                    info.CursorOffset <- Math.Max(info.CursorOffset - selection.Length, 0)
                                    info.TextStartOrigin <- 0
                            | None -> () // Nothing to do

                            info.Value <- info.Value.Insert(info.CursorOffset + info.TextStartOrigin, this.Keyboard.LastKeyValue)
                            info.CursorOffset <- info.CursorOffset + 1
                            if info.CursorOffset > maxChar then
                                info.CursorOffset <- maxChar
                                info.TextStartOrigin <- info.TextStartOrigin + 1
                            info.ClearSelection()

                this.EndElement()
                true
