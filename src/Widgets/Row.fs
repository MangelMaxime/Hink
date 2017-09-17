namespace Hink.Widgets

open Hink.Gui
open Fable.Core
open Fable.Core.JsInterop
open Hink.Inputs
open Hink.Helpers

[<AutoOpen>]
module Row =

    type Hink with
        member this.Row (ratios) =
            this.RowInfo <- Some { Ratios = ratios
                                   CurrentRatio = 0
                                   SplitX = this.Cursor.X
                                   SplitWidth = this.Cursor.Width }

            this.Cursor.Width <- this.Cursor.Width * this.RowInfo.Value.ActiveRatio
