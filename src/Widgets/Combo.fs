namespace Hink.Widgets

open Hink.Gui
open Fable.Core
open Fable.Core.JsInterop
open Hink.Inputs
open Hink.Helpers

[<AutoOpen>]
module Combo =

    type Hink with
        member this.Combo(comboInfo : ComboInfo, texts : string list, label : string option, ?labelAlign) =
            if not (this.IsVisible(this.Theme.Element.Height)) then
                this.EndElement()
                false
            else
                let hover = this.IsHover()
                let released = this.IsReleased()
                let pressed = this.IsPressed()

                this.CurrentContext.fillStyle <-
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

                this.CurrentContext.RoundedRect(
                    this.Cursor.X + this.Theme.ButtonOffsetY,
                    this.Cursor.Y + this.Theme.ButtonOffsetY,
                    this.Cursor.Width - this.Theme.ButtonOffsetY * 2.,
                    this.Theme.Element.Height,
                    this.Theme.Combo.CornerRadius
                )

                let offsetX = this.Cursor.X + this.Cursor.Width - (this.Theme.Arrow.Width + this.Theme.ArrowOffsetX * 2.)
                let offsetY = this.Cursor.Y + this.Theme.ArrowOffsetY

                this.CurrentContext.fillStyle <- !^this.Theme.Text.Color

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

                this.CurrentContext.fillStyle <- !^this.Theme.Arrow.Color
                match comboInfo.State with
                | Closed ->
                    // Triangle down
                    this.CurrentContext.Triangle(
                        offsetX, offsetY,
                        offsetX + this.Theme.Arrow.Width, offsetY,
                        offsetX + this.Theme.Arrow.Width / 2., offsetY + this.Theme.Arrow.Height
                    )

                | Extended | JustExtended ->
                    // Triangle Up
                    this.CurrentContext.Triangle(
                        offsetX + this.Theme.Arrow.Width / 2., offsetY,
                        offsetX + this.Theme.Arrow.Width, offsetY + this.Theme.Arrow.Height,
                        offsetX, offsetY + this.Theme.Arrow.Height
                    )

                this.EndElement()
                true
