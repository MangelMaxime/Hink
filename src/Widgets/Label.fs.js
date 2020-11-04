import { Hink__FillSmallString_2C2B7D77, Hink__get_CurrentContext, Align, Hink__EndElement_77EFFD2E, Hink__IsVisibile_5E38073B } from "../Hink.fs.js";
import { defaultArg } from "../../showcase/.fable/fable-library.3.0.0-nagareyama-beta-005/Option.js";
import { Theme__get_ButtonOffsetY } from "../Theme.fs.js";

export function Hink_Gui_Hink__Hink_Label_7E23EBB7(this$, text, align, backgroundColor) {
    if (!Hink__IsVisibile_5E38073B(this$, this$.Theme.Element.Height)) {
        Hink__EndElement_77EFFD2E(this$);
    }
    else {
        const align_1 = defaultArg(align, new Align(0));
        if (backgroundColor != null) {
            Hink__get_CurrentContext(this$).fillStyle = backgroundColor;
            Hink__get_CurrentContext(this$).fillRect(this$.Cursor.X + Theme__get_ButtonOffsetY(this$.Theme), this$.Cursor.Y + Theme__get_ButtonOffsetY(this$.Theme), this$.Cursor.Width - (Theme__get_ButtonOffsetY(this$.Theme) * 2), this$.Theme.Button.Height);
        }
        Hink__get_CurrentContext(this$).fillStyle = this$.Theme.Text.Color;
        Hink__FillSmallString_2C2B7D77(this$, text, void 0, void 0, void 0, align_1);
        Hink__EndElement_77EFFD2E(this$);
    }
}

