namespace Hink

module rec Theme =

    type Theme =
        { FontSize : float
          FontSmallSize : float
          FontString : Printf.StringFormat<float -> string>
          Text : TextTheme
          Button : ButtonTheme
          Checkbox : CheckboxTheme
          Element : ElementTheme
          Label : LabelTheme
          Switch : SwitchTheme
          Window : WindowTheme }

        member this.FormatFontString =
            sprintf this.FontString

        member this.ButtonOffsetY =
            (this.Element.Height - this.Button.Height) / 2.

        member this.FontSmallOffsetY =
            (this.Element.Height - this.FontSize) / 2.

    type ButtonTheme =
        { Width : float
          Height : float
          CornerRadius : float
          Background : ButtonBackground }

    type ButtonBackground =
        { Pressed : string
          Hover : string
          Default : string }

    type ElementTheme =
        { Height : float
          Width : float
          SeparatorSize : float }

    type CheckboxTheme =
        { Width : float
          Height : float
          CornerRadius : float
          ActiveColor : string
          Color : string
          TickColor : string }

    type LabelTheme =
        { TextColor : string }

    type SwitchTheme =
        { Width : float
          Height : float
          SpacingCircle : float
          Color : SwitchColor }

        member this.HalfHeight = this.Height / 2.
        member this.Radius = this.HalfHeight - this.SpacingCircle

    type SwitchStateColor =
        { Active : string
          Default : string }

    type SwitchColor =
        { Circle : SwitchStateColor
          Background : SwitchStateColor
          Text : SwitchStateColor }

    type TextTheme =
        { OffsetX : float
          Color : string }

    type WindowTheme =
        { Background : string
          Header : WindowHeader }

    type WindowHeader =
        { Color : string
          Height : float
          SymbolOffset : float
          OverSymbolColor : string }

    let darkTheme : Theme =
        { FontSize = 16.
          FontSmallSize = 16.
          FontString = "%fpx \"Lucia Console\", Monaco, monospace"
          Text =
            { OffsetX = 8.
              Color = "#fff" }
          Button =
            { Width = 80.
              Height = 34.
              CornerRadius = 4.
              Background =
                { Pressed = Color.rgb 22 160 133
                  Hover = Color.rgb 72 201 176
                  Default = Color.rgb 26 188 156 } }
          Checkbox =
            { Width = 22.
              Height = 22.
              CornerRadius = 3.
              ActiveColor = Color.rgb 189 195 199
              Color = Color.rgb 26 188 156
              TickColor = "#fff" }
          Element =
            { Height = 34.
              Width = 100.
              SeparatorSize = 2. }
          Label =
            { TextColor = Color.rgb 52 73 94 }
          Switch =
            { Width = 70.
              Height = 28.
              SpacingCircle = 4.
              Color =
                { Circle =
                    { Active = Color.rgb 127 140 154
                      Default = Color.rgb 26 188 156 }
                  Background =
                    { Active = Color.rgb 189 195 199
                      Default = Color.rgb 52 73 94 }
                  Text =
                    { Active = Color.rgb 255 255 255
                      Default = Color.rgb 26 188 156 } } }
          Window =
            { Background = "#bdc3c7"
              Header =
                { Color = "#34495e"
                  Height = 24.
                  SymbolOffset = 6.
                  OverSymbolColor = "#e74c3c" } } }
