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
          Input : InputTheme
          Window : WindowTheme
          Combo : ComboTheme
          Arrow : ArrowTheme }

        member this.FormatFontString
            with get () = sprintf this.FontString

        member this.ButtonOffsetY
            with get () = (this.Element.Height - this.Button.Height) / 2.

        member this.CheckboxOffsetX
            with get () = (this.Element.Height - this.Checkbox.Height) / 2.

        member this.CheckboxOffsetY
            with get () = this.CheckboxOffsetX

        member this.FontSmallOffsetY
            with get () = this.Element.Height / 2.

        member this.ArrowOffsetY
            with get () = (this.Element.Height - this.Arrow.Height) / 2.

        member this.ArrowOffsetX
            with get () = (this.Element.Height - this.Arrow.Width) / 2.

    type ArrowTheme =
        { Height : float
          Width : float
          Color : string }

    type ButtonTheme =
        { Width : float
          Height : float
          CornerRadius : float
          Background : ButtonBackground }

    type InputTheme =
        { TextColor : string
          Border : InputColorByState
          Background : InputColorByState
          SelectionColor : string }

    type InputColorByState =
        { Default : string
          Active : string }

    type ButtonBackground =
        { Pressed : string
          Hover : string
          Default : string }

    type ElementTheme =
        { Height : float
          Width : float
          SeparatorSize : float
          CornerRadius : float }

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
          OffsetY : float
          Color : string }

    type WindowTheme =
        { Background : string
          Header : WindowHeader }

    type WindowHeader =
        { Color : string
          Height : float
          SymbolOffsetX : float
          SymbolOffsetY : float
          OverSymbolColor : string }

    type ComboTheme =
        { Background : ComboBackground
          CornerRadius : float
          Box : ComboBox }

    type ComboBackground =
        { Default : string
          Hover : string
          Pressed : string }

    type ComboBox =
        { Default : ComboBoxColors
          Hover : ComboBoxColors
          Selected : ComboBoxColors }

    type ComboBoxColors =
        { Background : string
          Text : string }

    let darkTheme : Theme =
        { FontSize = 16.
          FontSmallSize = 16.
          FontString = "%fpx \"Lucia Console\", Monaco, monospace"
          Arrow =
            { Width = 15.
              Height = 11.
              Color = "#fff" }
          Text =
            { OffsetX = 8.
              OffsetY = 2.
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
              ActiveColor = Color.rgb 26 188 156
              Color = Color.rgb 189 195 199
              TickColor = "#fff" }
          Element =
            { Height = 34.
              Width = 100.
              SeparatorSize = 2.
              CornerRadius = 4. }
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
            { Background = "#34495e"
              Header =
                { Color = "#1abc9c"
                  Height = 24.
                  SymbolOffsetX = 6.
                  SymbolOffsetY = 1.
                  OverSymbolColor = "#e74c3c" } }
          Combo =
            { Background =
                { Pressed = Color.rgb 22 160 133
                  Hover = Color.rgb 72 201 176
                  Default = Color.rgb 26 188 156 }
              CornerRadius = 4.
              Box =
                { Default =
                    { Background = "#ecf0f1"
                      Text = "#34495e" }
                  Hover =
                    { Background = "#bdc3c7"
                      Text = "#34495e" }
                  Selected =
                    { Background = "#1abc9c"
                      Text = "#fff" } } }
          Input =
            { TextColor = "#34495e"
              Border =
                { Default = "#bdc3c7"
                  Active = "#1abc9c" }
              Background =
                { Default = "#ecf0f1"
                  Active = "#ecf0f1" }
              SelectionColor = "#3498db" } }
