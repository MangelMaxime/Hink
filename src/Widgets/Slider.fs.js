import { Hink__get_CursorPosX, Hink__IsActive_244AC511, Hink__get_CurrentContext, Hink__SetActiveWidget_244AC511, Hink__SetCursor_Z721C83C5, Hink__IsPressed_77EFFD2E, Hink__IsHover_77EFFD2E, Hink__EndElement_77EFFD2E, Hink__IsVisibile_5E38073B } from "../Hink.fs.js";
import { defaultArg } from "../../showcase/.fable/fable-library.3.0.0-nagareyama-beta-005/Option.js";
import { Cursor_Pointer } from "../Inputs/Mouse.fs.js";
import { Browser_Types_CanvasRenderingContext2D__CanvasRenderingContext2D_RoundedRect_49C497E8, Color_rgb } from "../Helpers.fs.js";
import { SliderTheme__get_HalfHeight } from "../Theme.fs.js";
import { min as min_2, comparePrimitives, max as max_2 } from "../../showcase/.fable/fable-library.3.0.0-nagareyama-beta-005/Util.js";

export function Hink_Gui_Hink__Hink_Slider_Z704370D1(this$, handler, min, max, step) {
    if (!Hink__IsVisibile_5E38073B(this$, this$.Theme.Element.Height)) {
        Hink__EndElement_77EFFD2E(this$);
        return false;
    }
    else {
        const slider = this$.Theme.Slider;
        const min_1 = defaultArg(min, 0);
        const max_1 = defaultArg(max, 100);
        const step_1 = defaultArg(step, 10);
        const hover = Hink__IsHover_77EFFD2E(this$);
        const pressed = Hink__IsPressed_77EFFD2E(this$);
        const x = this$.Cursor.X + slider.Padding.Horizontal;
        const y = this$.Cursor.Y + (this$.Theme.Element.Height / 2);
        const w = this$.Cursor.Width - (2 * slider.Padding.Horizontal);
        const pos = (w * handler.Value) / max_1;
        if (hover) {
            Hink__SetCursor_Z721C83C5(this$, Cursor_Pointer);
        }
        if (pressed) {
            Hink__SetActiveWidget_244AC511(this$, handler.Guid);
        }
        Hink__get_CurrentContext(this$).fillStyle = Color_rgb(235, 237, 239);
        Browser_Types_CanvasRenderingContext2D__CanvasRenderingContext2D_RoundedRect_49C497E8(Hink__get_CurrentContext(this$), x, y, w, slider.Height, SliderTheme__get_HalfHeight(slider));
        Hink__get_CurrentContext(this$).beginPath();
        Hink__get_CurrentContext(this$).fillStyle = (Hink__IsActive_244AC511(this$, handler.Guid) ? Color_rgb(22, 160, 133) : (hover ? Color_rgb(72, 201, 176) : Color_rgb(26, 188, 156)));
        Hink__get_CurrentContext(this$).arc(x + pos, y + SliderTheme__get_HalfHeight(slider), slider.Radius, 0, 2 * 3.141592653589793, false);
        Hink__get_CurrentContext(this$).fill();
        Hink__get_CurrentContext(this$).closePath();
        let res = false;
        const x_1 = Hink__get_CursorPosX(this$) + slider.Padding.Horizontal;
        if ((hover ? pressed : false) ? Hink__IsActive_244AC511(this$, handler.Guid) : false) {
            let v;
            const mousePos = null;
            v = (Math.ceil(((mousePos * max_1) / w) / step_1) * step_1);
            handler.Value = v;
            res = true;
        }
        if (Hink__IsActive_244AC511(this$, handler.Guid)) {
            const matchValue = this$.Keyboard.LastKey;
            switch (matchValue.tag) {
                case 7: {
                    this$.ActiveWidget = (void 0);
                    break;
                }
                case 11: {
                    handler.Value = max_2(comparePrimitives, handler.Value - step_1, min_1);
                    res = true;
                    break;
                }
                case 13: {
                    handler.Value = min_2(comparePrimitives, handler.Value + step_1, max_1);
                    res = true;
                    break;
                }
                default: {
                }
            }
        }
        Hink__EndElement_77EFFD2E(this$);
        return res;
    }
}

