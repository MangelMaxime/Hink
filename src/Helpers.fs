namespace Hink

open System

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

    type Fable.Import.Browser.CanvasRenderingContext2D with
        member self.RoundedRect (x, y, width, height, radius, ?action) =
            let action = defaultArg action Fill
            self.save()
            self.beginPath()

            // Draw top side and top right corners
            self.moveTo(
                x + radius,
                y
            )
            self.arcTo(
                x + width,
                y,
                x + width,
                y + radius,
                radius
            )

            // Draw Right side and bottom right corner
            self.arcTo(
                x + width,
                y + height,
                x + width - radius,
                y + height,
                radius
            )

            // Draw bottom and bottom left corner
            self.arcTo(
                x,
                y + height,
                x,
                y + height - radius,
                radius
            )

            // Draw left and top left corner
            self.arcTo(
                x,
                y,
                x + radius,
                y,
                radius
            )

            match action with
            | Stroke ->
                self.stroke()
            | Fill ->
                self.fill()
            | StrokeAndFill ->
                self.fill()
                self.stroke()

            self.restore()

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
