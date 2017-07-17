namespace Hink

module rec Theme =

    type Theme =
        { FontSize : float
          FontString : Printf.StringFormat<float -> string>
          Button : ButtonTheme
          Label : LabelTheme }

        member this.FormatFontString _ =
            sprintf this.FontString this.FontSize

    type ButtonTheme =
        { Width : float
          Height : float
          CornerRadius : float
          ActiveColor : string
          HotColor : string
          Color : string
          TextColor : string }

    type LabelTheme =
        { TextColor : string }

    let darkTheme : Theme =
        { FontSize = 16.
          FontString = "%fpx\"Lucia Console\", Monaco, monospace"
          Button =
            { Width = 80.
              Height = 34.
              CornerRadius = 4.
              ActiveColor = Color.rgb 22 160 133
              HotColor = Color.rgb 72 201 176
              Color = Color.rgb 26 188 156
              TextColor = "#fff" }
          Label =
            { TextColor = Color.rgb 52 73 94 } }
