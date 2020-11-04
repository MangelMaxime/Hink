import { Record, Union } from "../showcase/.fable/fable-library.3.0.0-nagareyama-beta-005/Types.js";
import { lambda_type, array_type, list_type, class_type, option_type, string_type, bool_type, int32_type, record_type, float64_type, union_type } from "../showcase/.fable/fable-library.3.0.0-nagareyama-beta-005/Reflection.js";
import { fromSeconds } from "../showcase/.fable/fable-library.3.0.0-nagareyama-beta-005/TimeSpan.js";
import { Record__SetCursor_Z721C83C5, init, Record$reflection as Record$reflection_1, Manager } from "./Inputs/Mouse.fs.js";
import { newGuid } from "../showcase/.fable/fable-library.3.0.0-nagareyama-beta-005/Guid.js";
import { Manager as Manager_1, init as init_1, Record$reflection } from "./Inputs/Keyboard.fs.js";
import { uncurry } from "../showcase/.fable/fable-library.3.0.0-nagareyama-beta-005/Util.js";
import { Theme__get_FontSmallOffsetY, Theme__get_FormatFontString, Theme__get_ArrowOffsetY, Theme__get_ArrowOffsetX, darkTheme, Theme$reflection } from "./Theme.fs.js";
import { defaultArg } from "../showcase/.fable/fable-library.3.0.0-nagareyama-beta-005/Option.js";
import { now } from "../showcase/.fable/fable-library.3.0.0-nagareyama-beta-005/Date.js";
import { Browser_Types_CanvasRenderingContext2D__CanvasRenderingContext2D_Triangle_76A78260 } from "./Helpers.fs.js";
import { substring } from "../showcase/.fable/fable-library.3.0.0-nagareyama-beta-005/String.js";

export class LayoutOrientation extends Union {
    constructor(tag, ...fields) {
        super();
        this.tag = (tag | 0);
        this.fields = fields;
    }
    cases() {
        return ["Horizontal", "Vertical"];
    }
}

export function LayoutOrientation$reflection() {
    return union_type("Hink.Gui.LayoutOrientation", [], LayoutOrientation, () => [[], []]);
}

export class Align extends Union {
    constructor(tag, ...fields) {
        super();
        this.tag = (tag | 0);
        this.fields = fields;
    }
    cases() {
        return ["Left", "Center", "Right"];
    }
}

export function Align$reflection() {
    return union_type("Hink.Gui.Align", [], Align, () => [[], [], []]);
}

export const oneSecond = fromSeconds(1);

export class Cursor extends Record {
    constructor(X, Y, Width, Height) {
        super();
        this.X = X;
        this.Y = Y;
        this.Width = Width;
        this.Height = Height;
    }
}

export function Cursor$reflection() {
    return record_type("Hink.Gui.Cursor", [], Cursor, () => [["X", float64_type], ["Y", float64_type], ["Width", float64_type], ["Height", float64_type]]);
}

export class SelectionArea extends Record {
    constructor(Start, End) {
        super();
        this.Start = (Start | 0);
        this.End = (End | 0);
    }
}

export function SelectionArea$reflection() {
    return record_type("Hink.Gui.SelectionArea", [], SelectionArea, () => [["Start", int32_type], ["End", int32_type]]);
}

export function SelectionArea__get_Length(this$) {
    return this$.End - this$.Start;
}

export function SelectionArea__Edging_Z524259A4(this$, size) {
    if ((this$.Start === 0) ? (this$.End === size) : false) {
        return this$.Start !== this$.End;
    }
    else {
        return false;
    }
}

export function SelectionArea_Create_Z37302880(start, end) {
    return new SelectionArea(start, end);
}

export class DragInfo extends Record {
    constructor(OriginX, OriginY) {
        super();
        this.OriginX = OriginX;
        this.OriginY = OriginY;
    }
}

export function DragInfo$reflection() {
    return record_type("Hink.Gui.DragInfo", [], DragInfo, () => [["OriginX", float64_type], ["OriginY", float64_type]]);
}

export class WindowHandler extends Record {
    constructor(X, Y, Width, Height, Layout, Draggable, Closable, Closed, Title, DragXOrigin, DragYOrigin, IsDragging, ShouldRedraw, _Canvas, _Context) {
        super();
        this.X = X;
        this.Y = Y;
        this.Width = Width;
        this.Height = Height;
        this.Layout = Layout;
        this.Draggable = Draggable;
        this.Closable = Closable;
        this.Closed = Closed;
        this.Title = Title;
        this.DragXOrigin = DragXOrigin;
        this.DragYOrigin = DragYOrigin;
        this.IsDragging = IsDragging;
        this.ShouldRedraw = ShouldRedraw;
        this._Canvas = _Canvas;
        this._Context = _Context;
    }
}

export function WindowHandler$reflection() {
    return record_type("Hink.Gui.WindowHandler", [], WindowHandler, () => [["X", float64_type], ["Y", float64_type], ["Width", float64_type], ["Height", float64_type], ["Layout", LayoutOrientation$reflection()], ["Draggable", bool_type], ["Closable", bool_type], ["Closed", bool_type], ["Title", option_type(string_type)], ["DragXOrigin", float64_type], ["DragYOrigin", float64_type], ["IsDragging", bool_type], ["ShouldRedraw", bool_type], ["_Canvas", option_type(class_type("Browser.Types.HTMLCanvasElement"))], ["_Context", option_type(class_type("Browser.Types.CanvasRenderingContext2D"))]]);
}

export function WindowHandler_get_Default() {
    return new WindowHandler(0, 0, 100, 200, new LayoutOrientation(1), false, false, false, void 0, 0, 0, false, true, void 0, void 0);
}

export function WindowHandler__get_RealPositionX(this$) {
    if (this$.IsDragging) {
        return this$.X + (Manager.X - this$.DragXOrigin);
    }
    else {
        return this$.X;
    }
}

export function WindowHandler__get_RealPositionY(this$) {
    if (this$.IsDragging) {
        return this$.Y + (Manager.Y - this$.DragYOrigin);
    }
    else {
        return this$.Y;
    }
}

export function WindowHandler__EnsureContext(this$) {
    if (this$._Canvas == null) {
        const canvas = document.createElement("canvas");
        canvas.width = this$.Width;
        canvas.height = this$.Height;
        (canvas.style).width = (this$.Width.toString() + "px");
        (canvas.style).height = (this$.Height.toString() + "px");
        this$._Canvas = canvas;
        const arg0 = canvas.getContext('2d');
        this$._Context = arg0;
        this$._Context.textBaseline = "middle";
    }
}

export class CheckboxInfo extends Record {
    constructor(Value) {
        super();
        this.Value = Value;
    }
}

export function CheckboxInfo$reflection() {
    return record_type("Hink.Gui.CheckboxInfo", [], CheckboxInfo, () => [["Value", bool_type]]);
}

export function CheckboxInfo_get_Default() {
    return new CheckboxInfo(false);
}

export class ComboState extends Union {
    constructor(tag, ...fields) {
        super();
        this.tag = (tag | 0);
        this.fields = fields;
    }
    cases() {
        return ["Extended", "JustExtended", "Closed"];
    }
}

export function ComboState$reflection() {
    return union_type("Hink.Gui.ComboState", [], ComboState, () => [[], [], []]);
}

export class ComboInfo extends Record {
    constructor(SelectedIndex, State, Guid) {
        super();
        this.SelectedIndex = SelectedIndex;
        this.State = State;
        this.Guid = Guid;
    }
}

export function ComboInfo$reflection() {
    return record_type("Hink.Gui.ComboInfo", [], ComboInfo, () => [["SelectedIndex", option_type(int32_type)], ["State", ComboState$reflection()], ["Guid", class_type("System.Guid")]]);
}

export function ComboInfo_get_Default() {
    return new ComboInfo(void 0, new ComboState(2), newGuid());
}

export class ComboHandler extends Record {
    constructor(Info, X, Y, Width, Height, Values, Reference) {
        super();
        this.Info = Info;
        this.X = X;
        this.Y = Y;
        this.Width = Width;
        this.Height = Height;
        this.Values = Values;
        this.Reference = Reference;
    }
}

export function ComboHandler$reflection() {
    return record_type("Hink.Gui.ComboHandler", [], ComboHandler, () => [["Info", ComboInfo$reflection()], ["X", float64_type], ["Y", float64_type], ["Width", float64_type], ["Height", float64_type], ["Values", list_type(string_type)], ["Reference", ComboInfo$reflection()]]);
}

export class Row extends Record {
    constructor(Ratios, CurrentRatio, SplitX, SplitWidth) {
        super();
        this.Ratios = Ratios;
        this.CurrentRatio = (CurrentRatio | 0);
        this.SplitX = SplitX;
        this.SplitWidth = SplitWidth;
    }
}

export function Row$reflection() {
    return record_type("Hink.Gui.Row", [], Row, () => [["Ratios", array_type(float64_type)], ["CurrentRatio", int32_type], ["SplitX", float64_type], ["SplitWidth", float64_type]]);
}

export function Row__get_ActiveRatio(this$) {
    return this$.Ratios[this$.CurrentRatio];
}

export class InputHandler extends Record {
    constructor(Value, Selection$, KeyboardCaptureHandler, CursorOffset, TextStartOrigin, Guid) {
        super();
        this.Value = Value;
        this.Selection = Selection$;
        this.KeyboardCaptureHandler = KeyboardCaptureHandler;
        this.CursorOffset = (CursorOffset | 0);
        this.TextStartOrigin = (TextStartOrigin | 0);
        this.Guid = Guid;
    }
}

export function InputHandler$reflection() {
    return record_type("Hink.Gui.InputHandler", [], InputHandler, () => [["Value", string_type], ["Selection", option_type(SelectionArea$reflection())], ["KeyboardCaptureHandler", option_type(lambda_type(InputHandler$reflection(), lambda_type(Record$reflection(), bool_type)))], ["CursorOffset", int32_type], ["TextStartOrigin", int32_type], ["Guid", class_type("System.Guid")]]);
}

export function InputHandler__ClearSelection(this$) {
    this$.Selection = (void 0);
}

export function InputHandler__SetSelection_75B7AC0D(this$, value) {
    this$.Selection = value;
}

export function InputHandler__Empty(this$) {
    this$.Value = "";
    this$.Selection = (void 0);
    this$.CursorOffset = 0;
    this$.TextStartOrigin = 0;
}

export function InputHandler_get_Default() {
    return new InputHandler("", void 0, uncurry(2, void 0), 0, 0, newGuid());
}

export class SliderOrientation extends Union {
    constructor(tag, ...fields) {
        super();
        this.tag = (tag | 0);
        this.fields = fields;
    }
    cases() {
        return ["Vertical", "Horizontal"];
    }
}

export function SliderOrientation$reflection() {
    return union_type("Hink.Gui.SliderOrientation", [], SliderOrientation, () => [[], []]);
}

export class SliderHandler extends Record {
    constructor(Guid, Value) {
        super();
        this.Guid = Guid;
        this.Value = Value;
    }
}

export function SliderHandler$reflection() {
    return record_type("Hink.Gui.SliderHandler", [], SliderHandler, () => [["Guid", class_type("System.Guid")], ["Value", float64_type]]);
}

export function SliderHandler_get_Default() {
    return new SliderHandler(newGuid(), 0);
}

export class Hink extends Record {
    constructor(Canvas, ApplicationContext, Mouse, Keyboard, KeyboardCaptureHandler, ActiveWidget, KeyboadHasBeenCapture, Theme, RowInfo, Cursor, IsCursorStyled, ShouldCloseWindow, CurrentWindow, CurrentCombo, LastReferenceTick, Delta) {
        super();
        this.Canvas = Canvas;
        this.ApplicationContext = ApplicationContext;
        this.Mouse = Mouse;
        this.Keyboard = Keyboard;
        this.KeyboardCaptureHandler = KeyboardCaptureHandler;
        this.ActiveWidget = ActiveWidget;
        this.KeyboadHasBeenCapture = KeyboadHasBeenCapture;
        this.Theme = Theme;
        this.RowInfo = RowInfo;
        this.Cursor = Cursor;
        this.IsCursorStyled = IsCursorStyled;
        this.ShouldCloseWindow = ShouldCloseWindow;
        this.CurrentWindow = CurrentWindow;
        this.CurrentCombo = CurrentCombo;
        this.LastReferenceTick = LastReferenceTick;
        this.Delta = Delta;
    }
}

export function Hink$reflection() {
    return record_type("Hink.Gui.Hink", [], Hink, () => [["Canvas", class_type("Browser.Types.HTMLCanvasElement")], ["ApplicationContext", class_type("Browser.Types.CanvasRenderingContext2D")], ["Mouse", Record$reflection_1()], ["Keyboard", Record$reflection()], ["KeyboardCaptureHandler", option_type(lambda_type(Record$reflection(), bool_type))], ["ActiveWidget", option_type(class_type("System.Guid"))], ["KeyboadHasBeenCapture", bool_type], ["Theme", Theme$reflection()], ["RowInfo", option_type(Row$reflection())], ["Cursor", Cursor$reflection()], ["IsCursorStyled", bool_type], ["ShouldCloseWindow", bool_type], ["CurrentWindow", option_type(WindowHandler$reflection())], ["CurrentCombo", option_type(ComboHandler$reflection())], ["LastReferenceTick", class_type("System.DateTime")], ["Delta", class_type("System.TimeSpan")]]);
}

export function Hink_Create_Z68A72FE(canvas, fontSize, theme, keyboardPreventHandler, keyboardCaptureHandler) {
    canvas.setAttribute("tabindex", "1");
    const context = canvas.getContext('2d');
    context.textBaseline = "middle";
    canvas.focus();
    init(canvas);
    init_1(canvas, true, keyboardPreventHandler);
    const Theme = defaultArg(theme, darkTheme);
    const Cursor_1 = new Cursor(0, 0, 0, 0);
    const LastReferenceTick = now();
    return new Hink(canvas, context, Manager, Manager_1, keyboardCaptureHandler, void 0, false, Theme, void 0, Cursor_1, false, false, void 0, void 0, LastReferenceTick, 0);
}

export function Hink__get_CursorPosX(this$) {
    const matchValue = this$.CurrentWindow;
    if (matchValue == null) {
        return this$.Cursor.X;
    }
    else {
        const current = matchValue;
        return this$.Cursor.X + WindowHandler__get_RealPositionX(current);
    }
}

export function Hink__get_CursorPosY(this$) {
    const matchValue = this$.CurrentWindow;
    if (matchValue == null) {
        return this$.Cursor.Y;
    }
    else {
        const current = matchValue;
        return this$.Cursor.Y + WindowHandler__get_RealPositionY(current);
    }
}

export function Hink__get_CurrentContext(this$) {
    if (this$.CurrentWindow != null) {
        return this$.CurrentWindow._Context;
    }
    else {
        return this$.ApplicationContext;
    }
}

export function Hink__SetActiveWidget_244AC511(this$, guid) {
    this$.ActiveWidget = guid;
}

export function Hink__IsActive_244AC511(this$, guid) {
    if (this$.ActiveWidget != null) {
        return this$.ActiveWidget === guid;
    }
    else {
        return false;
    }
}

export function Hink__SetCursor_Z721C83C5(this$, cursor) {
    Record__SetCursor_Z721C83C5(this$.Mouse, cursor);
    this$.IsCursorStyled = true;
}

export function Hink__IsVisibile_5E38073B(this$, elementH) {
    const matchValue = this$.CurrentWindow;
    if (matchValue != null) {
        if (matchValue.Closed) {
            return false;
        }
        else {
            const window$ = matchValue;
            if ((this$.Cursor.Y + elementH) > 0) {
                return this$.Cursor.Y < (((WindowHandler__get_RealPositionY(window$) + this$.Theme.Window.Header.Height) + window$.Height) - elementH);
            }
            else {
                return false;
            }
        }
    }
    else {
        return true;
    }
}

export function Hink__IsHover_77EFFD2E(this$, elementH) {
    const elementH_1 = defaultArg(elementH, this$.Theme.Element.Height);
    if (((this$.Mouse.X >= Hink__get_CursorPosX(this$)) ? (this$.Mouse.X < (Hink__get_CursorPosX(this$) + this$.Cursor.Width)) : false) ? (this$.Mouse.Y >= Hink__get_CursorPosY(this$)) : false) {
        return this$.Mouse.Y < (Hink__get_CursorPosY(this$) + elementH_1);
    }
    else {
        return false;
    }
}

export function Hink__IsPressed_77EFFD2E(this$, elementH) {
    const elementH_1 = defaultArg(elementH, this$.Theme.Element.Height);
    if (this$.Mouse.Left) {
        return Hink__IsHover_77EFFD2E(this$, elementH_1);
    }
    else {
        return false;
    }
}

export function Hink__IsReleased_77EFFD2E(this$, elementH) {
    const elementH_1 = defaultArg(elementH, this$.Theme.Element.Height);
    if (this$.Mouse.JustReleased) {
        return Hink__IsHover_77EFFD2E(this$, elementH_1);
    }
    else {
        return false;
    }
}

export function Hink__EndElement_77EFFD2E(this$, elementHeight) {
    const matchValue = this$.CurrentWindow;
    if (matchValue != null) {
        if (matchValue.Layout.tag === 0) {
            this$.Cursor.X = (this$.Cursor.Width + this$.Theme.Element.SeparatorSize);
        }
        else {
            const elementHeight_1 = defaultArg(elementHeight, this$.Theme.Element.Height + this$.Theme.Element.SeparatorSize);
            const matchValue_1 = this$.RowInfo;
            if (matchValue_1 != null) {
                const rowInfo = matchValue_1;
                if ((rowInfo.CurrentRatio === (rowInfo.Ratios.length - 1)) ? true : (rowInfo.CurrentRatio === -1)) {
                    this$.Cursor.Y = (this$.Cursor.Y + elementHeight_1);
                    if (rowInfo.CurrentRatio === (rowInfo.Ratios.length - 1)) {
                        this$.RowInfo = (void 0);
                        this$.Cursor.X = rowInfo.SplitX;
                        this$.Cursor.Width = rowInfo.SplitWidth;
                    }
                }
                else {
                    rowInfo.CurrentRatio = (rowInfo.CurrentRatio + 1);
                    this$.Cursor.X = (this$.Cursor.X + this$.Cursor.Width);
                    this$.Cursor.Width = (rowInfo.SplitWidth * Row__get_ActiveRatio(rowInfo));
                }
            }
            else {
                this$.Cursor.Y = (this$.Cursor.Y + elementHeight_1);
            }
        }
    }
    else {
        this$.Cursor.Y = ((this$.Cursor.Y + this$.Theme.Element.Height) + this$.Theme.Element.SeparatorSize);
    }
}

export function Hink__Empty(this$) {
    Hink__EndElement_77EFFD2E(this$);
}

export function Hink__DrawArrow_Z17567AD1(this$, selected, hover) {
    const x = this$.Cursor.X + Theme__get_ArrowOffsetX(this$.Theme);
    const y = this$.Cursor.Y + Theme__get_ArrowOffsetY(this$.Theme);
    Hink__get_CurrentContext(this$).fillStyle = this$.Theme.Arrow.Color;
    if (selected) {
        Browser_Types_CanvasRenderingContext2D__CanvasRenderingContext2D_Triangle_76A78260(Hink__get_CurrentContext(this$), x, y, x + this$.Theme.Arrow.Width, y, x + (this$.Theme.Arrow.Width / 2), y + this$.Theme.Arrow.Height);
    }
    else {
        Browser_Types_CanvasRenderingContext2D__CanvasRenderingContext2D_Triangle_76A78260(Hink__get_CurrentContext(this$), x, y, x, y + this$.Theme.Arrow.Height, x + this$.Theme.Arrow.Width, y + (this$.Theme.Arrow.Height / 2));
    }
}

export function Hink__FillSmallString_2C2B7D77(this$, text, offsetX, offsetY, width, align) {
    const offsetY_1 = defaultArg(offsetY, this$.Theme.Text.OffsetY);
    const align_1 = defaultArg(align, new Align(0));
    const textSize = Hink__get_CurrentContext(this$).measureText(text);
    const width_1 = defaultArg(width, this$.Cursor.Width);
    const offsetX_1 = (align_1.tag === 1) ? ((width_1 / 2) - (textSize.width / 2)) : ((align_1.tag === 2) ? ((width_1 - textSize.width) - this$.Theme.Text.OffsetX) : defaultArg(offsetX, this$.Theme.Text.OffsetX));
    Hink__get_CurrentContext(this$).font = Theme__get_FormatFontString(this$.Theme)(this$.Theme.FontSmallSize);
    let text_1;
    if (textSize.width > (width_1 - this$.Theme.Text.OffsetX)) {
        const charSize = Hink__get_CurrentContext(this$).measureText(" ");
        let maxChar;
        const value = (width_1 - this$.Theme.Text.OffsetX) / charSize.width;
        maxChar = (~(~value));
        text_1 = (substring(text, 0, maxChar - 2) + "..");
    }
    else {
        text_1 = text;
    }
    Hink__get_CurrentContext(this$).fillText(text_1, this$.Cursor.X + offsetX_1, (this$.Cursor.Y + Theme__get_FontSmallOffsetY(this$.Theme)) + offsetY_1);
}

