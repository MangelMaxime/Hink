namespace Hink

open System
open Browser.Types

[<AutoOpen>]
module Helpers =

    module Color =

        // Taken from http://fable.io/samples/mario/index.html
        // Format RGB color as "rgb(r,g,b)"
        let ($) s n = s + n.ToString()
        let rgb r g b = "rgb(" $ r $ "," $ g $ "," $ b $ ")"

    /// Extends Math type with Clamp function
    type System.Math with
        static member Clamp (value, (min: float), (max: float)) =
            let mutable res = value
            if value < min then
                res <- min
            if value > max then
                res <- max
            res

    type RenderingType =
        | Stroke
        | Fill
        | StrokeAndFill

    type CanvasRenderingContext2D with
        member this.RoundedRect (x, y, width, height, radius, ?action) =
            let action = defaultArg action Fill
            this.save()
            this.beginPath()

            // Draw top side and top right corners
            this.moveTo(
                x + radius,
                y
            )
            this.arcTo(
                x + width,
                y,
                x + width,
                y + radius,
                radius
            )

            // Draw Right side and bottom right corner
            this.arcTo(
                x + width,
                y + height,
                x + width - radius,
                y + height,
                radius
            )

            // Draw bottom and bottom left corner
            this.arcTo(
                x,
                y + height,
                x,
                y + height - radius,
                radius
            )

            // Draw left and top left corner
            this.arcTo(
                x,
                y,
                x + radius,
                y,
                radius
            )

            match action with
            | Stroke ->
                this.stroke()
            | Fill ->
                this.fill()
            | StrokeAndFill ->
                this.fill()
                this.stroke()

            this.restore()

        member this.Triangle(x, y, x1, y1, x2, y2) =
            this.save()
            this.beginPath()
            this.moveTo(x, y)
            this.lineTo(x1, y1)
            this.lineTo(x2, y2)
            this.fill()
            this.restore()

    type String with
        member self.FindOccurence(c) =
            let mutable indexes = []
            for i = 0 to self.Length do
                if self.[i] = c then
                    indexes <-  i :: indexes
            indexes

    let NextIndexBackward(value: string, c, reference) =
        value.FindOccurence(c)
        |> List.tryFind(fun x ->
            x < reference
        )
        |> (fun x ->
            match x with
            | Some index -> index
            | None -> 0
        )

    let NextIndexForward(value: string, c, reference) =
        value.FindOccurence(c)
        |> List.sort
        |> List.tryFind(fun x ->
            x > reference
        )
        |> (fun x ->
            match x with
            | Some index -> index
            | None -> value.Length
        )
