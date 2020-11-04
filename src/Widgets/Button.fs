namespace Hink.Widgets

open Hink.Gui
open Fable.Core
open Fable.Core.JsInterop
open Hink.Inputs
open Hink.Helpers

[<AutoOpen>]
module Button =

    type Hink with
        member this.Button (text, ?align : Align, ?pressedColor, ?hoverColor, ?defaultColor, ?textPressed, ?textHover, ?textDefault) =
            if not (this.IsVisible(this.Theme.Element.Height)) then
                this.EndElement()
                false
            else

                let align = defaultArg align Center
                // Check visibility
                let hover = this.IsHover()
                let pressed = this.IsPressed()
                let released = this.IsReleased()

                this.CurrentContext.fillStyle <-
                    if pressed then
                        !^(defaultArg pressedColor this.Theme.Button.Background.Pressed)
                    elif hover then
                        !^(defaultArg hoverColor this.Theme.Button.Background.Hover)
                    else
                        !^(defaultArg defaultColor this.Theme.Button.Background.Default)

                if hover then
                    this.SetCursor Mouse.Cursor.Pointer

                this.CurrentContext.RoundedRect(
                    this.Cursor.X + this.Theme.ButtonOffsetY,
                    this.Cursor.Y + this.Theme.ButtonOffsetY,
                    this.Cursor.Width - this.Theme.ButtonOffsetY * 2.,
                    this.Theme.Button.Height,
                    this.Theme.Button.CornerRadius
                )

                this.CurrentContext.fillStyle <-
                    if pressed then
                        !^(defaultArg textPressed this.Theme.Text.Color)
                    elif hover then
                        !^(defaultArg textHover this.Theme.Text.Color)
                    else
                        !^(defaultArg textDefault this.Theme.Text.Color)

                this.FillSmallString(text, align = align)

                this.EndElement()
                released
