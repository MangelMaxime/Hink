namespace Hink.Widgets

open Hink.Gui
open Fable.Core
open Fable.Core.JsInterop
open Hink.Inputs
open Hink.Helpers

[<AutoOpen>]
module Label =

    type Hink with
        member this.Label (text, ?align : Align, ?backgroundColor : string) =
            if not (this.IsVisible(this.Theme.Element.Height)) then
                this.EndElement()
            else
                let align = defaultArg align Left

                if backgroundColor.IsSome then
                    this.CurrentContext.fillStyle <- !^backgroundColor.Value
                    this.CurrentContext.fillRect(
                        this.Cursor.X + this.Theme.ButtonOffsetY,
                        this.Cursor.Y + this.Theme.ButtonOffsetY,
                        this.Cursor.Width - this.Theme.ButtonOffsetY * 2.,
                        this.Theme.Button.Height
                    )

                this.CurrentContext.fillStyle <- !^this.Theme.Text.Color
                this.FillSmallString(text, align = align)
                this.EndElement()
