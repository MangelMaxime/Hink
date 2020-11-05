namespace Hink.Widgets

open Hink.Gui
open Fable.Core
open Fable.Core.JsInterop
open Hink.Inputs
open Hink.Helpers
open System

[<AutoOpen>]
module LifeCycle =

    type Hink with
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
                    this.CurrentContext.fillStyle <- !^this.Theme.Combo.Box.Default.Background
                    this.CurrentContext.fillRect(this.Cursor.X, this.Cursor.Y, this.Cursor.Width, boxHeight)

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
            this.Mouse.ResetPressed ()
            // Reset the cursor style if not styled
            if not this.IsCursorStyled then
                this.Mouse.ResetCursor ()

            // Set value to false
            // Help detecting if the mouse cursor need to be reset on the next loop
            this.IsCursorStyled <- false

            this.KeyboadHasBeenCapture <- false
            this.Keyboard.ClearLastKey()
