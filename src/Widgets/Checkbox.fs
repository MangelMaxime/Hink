namespace Hink.Widgets

open Hink.Gui
open Fable.Core
open Fable.Core.JsInterop
open Hink.Inputs
open Hink.Helpers

[<AutoOpen>]
module Checkbox =

    type Hink with
        member this.Checkbox(checkboxInfo : CheckboxInfo, ?label) =
            if not (this.IsVisibile(this.Theme.Element.Height)) then
                this.EndElement()
                false
            else
                let hover = this.IsHover()
                let released = this.IsReleased()

                this.CurrentContext.fillStyle <-
                    if checkboxInfo.Value then
                        !^this.Theme.Checkbox.ActiveColor
                    else
                        !^this.Theme.Checkbox.Color

                this.CurrentContext.RoundedRect(
                    this.Cursor.X + this.Theme.CheckboxOffsetX,
                    this.Cursor.Y + this.Theme.CheckboxOffsetY,
                    this.Theme.Checkbox.Width,
                    this.Theme.Checkbox.Height,
                    this.Theme.Checkbox.CornerRadius
                )

                if hover && (not checkboxInfo.Value) || checkboxInfo.Value then
                    this.CurrentContext.fillStyle <- !^this.Theme.Checkbox.TickColor
                    this.CurrentContext.font <- this.Theme.FormatFontString 20.
                    this.CurrentContext.fillText(
                        "\u2713",
                        this.Cursor.X + this.Theme.Checkbox.Width / 2.,
                        this.Cursor.Y + this.Theme.Element.Height / 2.
                    )

                match label with
                | Some label ->
                    // Label section
                    this.CurrentContext.fillStyle <- !^this.Theme.Text.Color
                    this.FillSmallString(
                        label,
                        this.Theme.Checkbox.Width + this.Theme.CheckboxOffsetX * 4.
                    )
                | None -> ()

                if released then
                    checkboxInfo.Value <- not checkboxInfo.Value

                this.EndElement()
                checkboxInfo.Value
