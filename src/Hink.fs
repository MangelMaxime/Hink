namespace Hink

open Fable.Core
open Fable.Core.JsInterop
open Fable.Import
open Hink.Inputs
open Hink.Theme
open System

module Gui =

    type ID = string

    let oneSecond = TimeSpan.FromSeconds 1.

    type Hink =
        { Canvas : Browser.HTMLCanvasElement
          Context : Browser.CanvasRenderingContext2D
          Mouse : Mouse.Record
          Keyboard : Keyboard.Record
          Theme : Theme
          mutable HotItem : ID
          mutable ActiveItem : ID
          mutable LastReferenceTick : DateTime
          mutable Delta : TimeSpan }

        static member Create (canvas : Browser.HTMLCanvasElement, ?fontSize, ?theme) =
            // Allow canvas interaction
            canvas.setAttribute ("tabindex", "1")
            // Init the context
            let context = canvas.getContext_2d()
            context.textBaseline <- "top"
            canvas.focus()


            // Init input manager
            Mouse.init canvas
            Keyboard.init canvas true

            { Canvas = canvas
              Context = context
              Mouse = Mouse.Manager
              Keyboard = Keyboard.Manager
              Theme = defaultArg theme darkTheme
              HotItem = ""
              ActiveItem = ""
              LastReferenceTick = DateTime.Now
              Delta = TimeSpan.Zero }

        /// Reset the state of Hot Item
        /// Need to be called before drawing UI
        member this.Prepare () =
            let now = DateTime.Now
            this.Delta <- now - this.LastReferenceTick

            if this.Delta > oneSecond then
                this.LastReferenceTick <- now
                this.Delta <- this.Delta.Subtract(oneSecond)

            this.HotItem <- ""

        /// Reset Active item
        /// Need to be called after drawing UI
        member this.Finish () =
            /// If the Mouse is down remove Active Item
            /// This help to not trigger several times a click for example
            if not this.Mouse.Left then
                this.ActiveItem <- ""

            if this.HotItem = "" then
                this.Mouse.ResetCursor ()

            // if this.Keyboard.LastKey = Keyboard.Keys.Tab then
            //     this.KeyboardItem <- ""
            //     this.CursorOffsetX <- 0

            this.Keyboard.ClearLastKey()

        member this.Button (id, text, x, y, ?width, ?height, ?theme) =
            let w = defaultArg width this.Theme.Button.Width
            let h = defaultArg height this.Theme.Button.Height
            let theme = defaultArg theme this.Theme.Button

            // Determine button state
            if this.Mouse.HitRegion(x, y, w, h) then
                this.HotItem <- id
                this.Mouse.SetCursor Mouse.Cursor.Pointer
                if this.HotItem = id && this.Mouse.Left then
                    this.ActiveItem <- id

            // Set the color
            if this.ActiveItem = id then
                this.Context.fillStyle <- !^theme.ActiveColor
            elif this.HotItem = id then
                this.Context.fillStyle <- !^theme.HotColor
            else
                this.Context.fillStyle <- !^theme.Color

            // Draw button "background"
            this.Context.RoundedRect(x, y, w, h, theme.CornerRadius)

            this.Context.fillStyle <- !^theme.TextColor
            this.Context.font <- this.Theme.FormatFontString ()

            let textSize = this.Context.measureText(text)
            let textX = x + (w / 2.) - (textSize.width / 2.)
            let textY = y + (h / 2.) - (this.Theme.FontSize  / 2.)

            this.Context.fillText(text, textX, textY)

            let mutable out = false

            out || (not this.Mouse.Left) && this.HotItem = id && this.ActiveItem = id

        member this.Label (text, x, y, ?theme) =
            let theme = defaultArg theme this.Theme.Label
            this.Context.fillStyle <- !^theme.TextColor
            this.Context.font <- this.Theme.FormatFontString ()

            this.Context.fillText(text, x, y)
