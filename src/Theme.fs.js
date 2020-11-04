import { Record } from "../showcase/.fable/fable-library.3.0.0-nagareyama-beta-005/Types.js";
import { record_type, class_type, unit_type, lambda_type, string_type, float64_type } from "../showcase/.fable/fable-library.3.0.0-nagareyama-beta-005/Reflection.js";
import { printf, toText } from "../showcase/.fable/fable-library.3.0.0-nagareyama-beta-005/String.js";
import { Color_rgb } from "./Helpers.fs.js";

export class Theme extends Record {
    constructor(FontSize, FontSmallSize, FontString, Text$, Button, Slider, Checkbox, Element$, Label, Switch, Input, Window$, Combo, Arrow) {
        super();
        this.FontSize = FontSize;
        this.FontSmallSize = FontSmallSize;
        this.FontString = FontString;
        this.Text = Text$;
        this.Button = Button;
        this.Slider = Slider;
        this.Checkbox = Checkbox;
        this.Element = Element$;
        this.Label = Label;
        this.Switch = Switch;
        this.Input = Input;
        this.Window = Window$;
        this.Combo = Combo;
        this.Arrow = Arrow;
    }
}

export function Theme$reflection() {
    return record_type("Hink.Theme.Theme", [], Theme, () => [["FontSize", float64_type], ["FontSmallSize", float64_type], ["FontString", class_type("Microsoft.FSharp.Core.PrintfFormat`4", [lambda_type(float64_type, string_type), unit_type, string_type, string_type])], ["Text", TextTheme$reflection()], ["Button", ButtonTheme$reflection()], ["Slider", SliderTheme$reflection()], ["Checkbox", CheckboxTheme$reflection()], ["Element", ElementTheme$reflection()], ["Label", LabelTheme$reflection()], ["Switch", SwitchTheme$reflection()], ["Input", InputTheme$reflection()], ["Window", WindowTheme$reflection()], ["Combo", ComboTheme$reflection()], ["Arrow", ArrowTheme$reflection()]]);
}

export class Padding extends Record {
    constructor(Horizontal, Vertical) {
        super();
        this.Horizontal = Horizontal;
        this.Vertical = Vertical;
    }
}

export function Padding$reflection() {
    return record_type("Hink.Theme.Padding", [], Padding, () => [["Horizontal", float64_type], ["Vertical", float64_type]]);
}

export class ArrowTheme extends Record {
    constructor(Height, Width, Color) {
        super();
        this.Height = Height;
        this.Width = Width;
        this.Color = Color;
    }
}

export function ArrowTheme$reflection() {
    return record_type("Hink.Theme.ArrowTheme", [], ArrowTheme, () => [["Height", float64_type], ["Width", float64_type], ["Color", string_type]]);
}

export class ButtonTheme extends Record {
    constructor(Width, Height, CornerRadius, Background) {
        super();
        this.Width = Width;
        this.Height = Height;
        this.CornerRadius = CornerRadius;
        this.Background = Background;
    }
}

export function ButtonTheme$reflection() {
    return record_type("Hink.Theme.ButtonTheme", [], ButtonTheme, () => [["Width", float64_type], ["Height", float64_type], ["CornerRadius", float64_type], ["Background", ButtonBackground$reflection()]]);
}

export class InputTheme extends Record {
    constructor(TextColor, Border, Background, SelectionColor) {
        super();
        this.TextColor = TextColor;
        this.Border = Border;
        this.Background = Background;
        this.SelectionColor = SelectionColor;
    }
}

export function InputTheme$reflection() {
    return record_type("Hink.Theme.InputTheme", [], InputTheme, () => [["TextColor", string_type], ["Border", InputColorByState$reflection()], ["Background", InputColorByState$reflection()], ["SelectionColor", string_type]]);
}

export class InputColorByState extends Record {
    constructor(Default, Active) {
        super();
        this.Default = Default;
        this.Active = Active;
    }
}

export function InputColorByState$reflection() {
    return record_type("Hink.Theme.InputColorByState", [], InputColorByState, () => [["Default", string_type], ["Active", string_type]]);
}

export class ButtonBackground extends Record {
    constructor(Pressed, Hover, Default) {
        super();
        this.Pressed = Pressed;
        this.Hover = Hover;
        this.Default = Default;
    }
}

export function ButtonBackground$reflection() {
    return record_type("Hink.Theme.ButtonBackground", [], ButtonBackground, () => [["Pressed", string_type], ["Hover", string_type], ["Default", string_type]]);
}

export class ElementTheme extends Record {
    constructor(Height, Width, SeparatorSize, CornerRadius) {
        super();
        this.Height = Height;
        this.Width = Width;
        this.SeparatorSize = SeparatorSize;
        this.CornerRadius = CornerRadius;
    }
}

export function ElementTheme$reflection() {
    return record_type("Hink.Theme.ElementTheme", [], ElementTheme, () => [["Height", float64_type], ["Width", float64_type], ["SeparatorSize", float64_type], ["CornerRadius", float64_type]]);
}

export class CheckboxTheme extends Record {
    constructor(Width, Height, CornerRadius, ActiveColor, Color, TickColor) {
        super();
        this.Width = Width;
        this.Height = Height;
        this.CornerRadius = CornerRadius;
        this.ActiveColor = ActiveColor;
        this.Color = Color;
        this.TickColor = TickColor;
    }
}

export function CheckboxTheme$reflection() {
    return record_type("Hink.Theme.CheckboxTheme", [], CheckboxTheme, () => [["Width", float64_type], ["Height", float64_type], ["CornerRadius", float64_type], ["ActiveColor", string_type], ["Color", string_type], ["TickColor", string_type]]);
}

export class LabelTheme extends Record {
    constructor(TextColor) {
        super();
        this.TextColor = TextColor;
    }
}

export function LabelTheme$reflection() {
    return record_type("Hink.Theme.LabelTheme", [], LabelTheme, () => [["TextColor", string_type]]);
}

export class SwitchTheme extends Record {
    constructor(Width, Height, SpacingCircle, Color) {
        super();
        this.Width = Width;
        this.Height = Height;
        this.SpacingCircle = SpacingCircle;
        this.Color = Color;
    }
}

export function SwitchTheme$reflection() {
    return record_type("Hink.Theme.SwitchTheme", [], SwitchTheme, () => [["Width", float64_type], ["Height", float64_type], ["SpacingCircle", float64_type], ["Color", SwitchColor$reflection()]]);
}

export class SwitchStateColor extends Record {
    constructor(Active, Default) {
        super();
        this.Active = Active;
        this.Default = Default;
    }
}

export function SwitchStateColor$reflection() {
    return record_type("Hink.Theme.SwitchStateColor", [], SwitchStateColor, () => [["Active", string_type], ["Default", string_type]]);
}

export class SwitchColor extends Record {
    constructor(Circle, Background, Text$) {
        super();
        this.Circle = Circle;
        this.Background = Background;
        this.Text = Text$;
    }
}

export function SwitchColor$reflection() {
    return record_type("Hink.Theme.SwitchColor", [], SwitchColor, () => [["Circle", SwitchStateColor$reflection()], ["Background", SwitchStateColor$reflection()], ["Text", SwitchStateColor$reflection()]]);
}

export class TextTheme extends Record {
    constructor(OffsetX, OffsetY, Color) {
        super();
        this.OffsetX = OffsetX;
        this.OffsetY = OffsetY;
        this.Color = Color;
    }
}

export function TextTheme$reflection() {
    return record_type("Hink.Theme.TextTheme", [], TextTheme, () => [["OffsetX", float64_type], ["OffsetY", float64_type], ["Color", string_type]]);
}

export class WindowTheme extends Record {
    constructor(Background, Header) {
        super();
        this.Background = Background;
        this.Header = Header;
    }
}

export function WindowTheme$reflection() {
    return record_type("Hink.Theme.WindowTheme", [], WindowTheme, () => [["Background", string_type], ["Header", WindowHeader$reflection()]]);
}

export class WindowHeader extends Record {
    constructor(Color, Height, SymbolOffsetX, SymbolOffsetY, OverSymbolColor) {
        super();
        this.Color = Color;
        this.Height = Height;
        this.SymbolOffsetX = SymbolOffsetX;
        this.SymbolOffsetY = SymbolOffsetY;
        this.OverSymbolColor = OverSymbolColor;
    }
}

export function WindowHeader$reflection() {
    return record_type("Hink.Theme.WindowHeader", [], WindowHeader, () => [["Color", string_type], ["Height", float64_type], ["SymbolOffsetX", float64_type], ["SymbolOffsetY", float64_type], ["OverSymbolColor", string_type]]);
}

export class ComboTheme extends Record {
    constructor(Background, CornerRadius, Box) {
        super();
        this.Background = Background;
        this.CornerRadius = CornerRadius;
        this.Box = Box;
    }
}

export function ComboTheme$reflection() {
    return record_type("Hink.Theme.ComboTheme", [], ComboTheme, () => [["Background", ComboBackground$reflection()], ["CornerRadius", float64_type], ["Box", ComboBox$reflection()]]);
}

export class ComboBackground extends Record {
    constructor(Default, Hover, Pressed) {
        super();
        this.Default = Default;
        this.Hover = Hover;
        this.Pressed = Pressed;
    }
}

export function ComboBackground$reflection() {
    return record_type("Hink.Theme.ComboBackground", [], ComboBackground, () => [["Default", string_type], ["Hover", string_type], ["Pressed", string_type]]);
}

export class ComboBox extends Record {
    constructor(Default, Hover, Selected) {
        super();
        this.Default = Default;
        this.Hover = Hover;
        this.Selected = Selected;
    }
}

export function ComboBox$reflection() {
    return record_type("Hink.Theme.ComboBox", [], ComboBox, () => [["Default", ComboBoxColors$reflection()], ["Hover", ComboBoxColors$reflection()], ["Selected", ComboBoxColors$reflection()]]);
}

export class ComboBoxColors extends Record {
    constructor(Background, Text$) {
        super();
        this.Background = Background;
        this.Text = Text$;
    }
}

export function ComboBoxColors$reflection() {
    return record_type("Hink.Theme.ComboBoxColors", [], ComboBoxColors, () => [["Background", string_type], ["Text", string_type]]);
}

export class SliderTheme extends Record {
    constructor(Height, Radius, Padding) {
        super();
        this.Height = Height;
        this.Radius = Radius;
        this.Padding = Padding;
    }
}

export function SliderTheme$reflection() {
    return record_type("Hink.Theme.SliderTheme", [], SliderTheme, () => [["Height", float64_type], ["Radius", float64_type], ["Padding", Padding$reflection()]]);
}

export function Theme__get_FormatFontString(this$) {
    return toText(this$.FontString);
}

export function Theme__get_ButtonOffsetY(this$) {
    return (this$.Element.Height - this$.Button.Height) / 2;
}

export function Theme__get_CheckboxOffsetX(this$) {
    return (this$.Element.Height - this$.Checkbox.Height) / 2;
}

export function Theme__get_CheckboxOffsetY(this$) {
    return Theme__get_CheckboxOffsetX(this$);
}

export function Theme__get_FontSmallOffsetY(this$) {
    return this$.Element.Height / 2;
}

export function Theme__get_ArrowOffsetY(this$) {
    return (this$.Element.Height - this$.Arrow.Height) / 2;
}

export function Theme__get_ArrowOffsetX(this$) {
    return (this$.Element.Height - this$.Arrow.Width) / 2;
}

export function SwitchTheme__get_HalfHeight(this$) {
    return this$.Height / 2;
}

export function SwitchTheme__get_Radius(this$) {
    return SwitchTheme__get_HalfHeight(this$) - this$.SpacingCircle;
}

export function SliderTheme__get_HalfHeight(this$) {
    return this$.Height / 2;
}

export const darkTheme = (() => {
    let Pressed, Hover;
    const FontString = printf("%fpx \"Lucia Console\", Monaco, monospace");
    const Arrow = new ArrowTheme(11, 15, "#fff");
    const Text$ = new TextTheme(8, 2, "#fff");
    const Button = new ButtonTheme(80, 34, 4, new ButtonBackground(Color_rgb(22, 160, 133), Color_rgb(72, 201, 176), Color_rgb(26, 188, 156)));
    const Checkbox = new CheckboxTheme(22, 22, 3, Color_rgb(26, 188, 156), Color_rgb(189, 195, 199), "#fff");
    const Element$ = new ElementTheme(34, 100, 2, 4);
    const Label = new LabelTheme(Color_rgb(52, 73, 94));
    const Switch = new SwitchTheme(70, 28, 4, new SwitchColor(new SwitchStateColor(Color_rgb(127, 140, 154), Color_rgb(26, 188, 156)), new SwitchStateColor(Color_rgb(189, 195, 199), Color_rgb(52, 73, 94)), new SwitchStateColor(Color_rgb(255, 255, 255), Color_rgb(26, 188, 156))));
    const Window$ = new WindowTheme("#34495e", new WindowHeader("#1abc9c", 24, 6, 1, "#e74c3c"));
    const Combo = new ComboTheme((Pressed = Color_rgb(22, 160, 133), (Hover = Color_rgb(72, 201, 176), new ComboBackground(Color_rgb(26, 188, 156), Hover, Pressed))), 4, new ComboBox(new ComboBoxColors("#ecf0f1", "#34495e"), new ComboBoxColors("#bdc3c7", "#34495e"), new ComboBoxColors("#1abc9c", "#fff")));
    const Input = new InputTheme("#34495e", new InputColorByState("#bdc3c7", "#1abc9c"), new InputColorByState("#ecf0f1", "#ecf0f1"), "#3498db");
    return new Theme(16, 16, FontString, Text$, Button, new SliderTheme(12, 9, new Padding(10, 0)), Checkbox, Element$, Label, Switch, Input, Window$, Combo, Arrow);
})();

