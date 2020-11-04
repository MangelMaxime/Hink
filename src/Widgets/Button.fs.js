import { Hink__FillSmallString_2C2B7D77, Hink__SetCursor_Z721C83C5, Hink__get_CurrentContext, Hink__IsReleased_77EFFD2E, Hink__IsPressed_77EFFD2E, Hink__IsHover_77EFFD2E, Align, Hink__EndElement_77EFFD2E, Hink__IsVisibile_5E38073B } from "../Hink.fs.js";
import { defaultArg } from "../../showcase/.fable/fable-library.3.0.0-nagareyama-beta-005/Option.js";
import { Cursor_Pointer } from "../Inputs/Mouse.fs.js";
import { Browser_Types_CanvasRenderingContext2D__CanvasRenderingContext2D_RoundedRect_49C497E8 } from "../Helpers.fs.js";
import { Theme__get_ButtonOffsetY } from "../Theme.fs.js";

export function Hink_Gui_Hink__Hink_Button_Z123163C7(this$, text, align, pressedColor, hoverColor, defaultColor, textPressed, textHover, textDefault) {
    if (!Hink__IsVisibile_5E38073B(this$, this$.Theme.Element.Height)) {
        Hink__EndElement_77EFFD2E(this$);
        return false;
    }
    else {
        const align_1 = defaultArg(align, new Align(1));
        const hover = Hink__IsHover_77EFFD2E(this$);
        const pressed = Hink__IsPressed_77EFFD2E(this$);
        const released = Hink__IsReleased_77EFFD2E(this$);
        Hink__get_CurrentContext(this$).fillStyle = (pressed ? defaultArg(pressedColor, this$.Theme.Button.Background.Pressed) : (hover ? defaultArg(hoverColor, this$.Theme.Button.Background.Hover) : defaultArg(defaultColor, this$.Theme.Button.Background.Default)));
        if (hover) {
            Hink__SetCursor_Z721C83C5(this$, Cursor_Pointer);
        }
        Browser_Types_CanvasRenderingContext2D__CanvasRenderingContext2D_RoundedRect_49C497E8(Hink__get_CurrentContext(this$), this$.Cursor.X + Theme__get_ButtonOffsetY(this$.Theme), this$.Cursor.Y + Theme__get_ButtonOffsetY(this$.Theme), this$.Cursor.Width - (Theme__get_ButtonOffsetY(this$.Theme) * 2), this$.Theme.Button.Height, this$.Theme.Button.CornerRadius);
        Hink__get_CurrentContext(this$).fillStyle = (pressed ? defaultArg(textPressed, this$.Theme.Text.Color) : (hover ? defaultArg(textHover, this$.Theme.Text.Color) : defaultArg(textDefault, this$.Theme.Text.Color)));
        Hink__FillSmallString_2C2B7D77(this$, text, void 0, void 0, void 0, align_1);
        Hink__EndElement_77EFFD2E(this$);
        return released;
    }
}

