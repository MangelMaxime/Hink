import { Align, Hink__FillSmallString_2C2B7D77, ComboState, ComboHandler, Hink__get_CurrentContext, Hink__IsPressed_77EFFD2E, Hink__IsReleased_77EFFD2E, Hink__IsHover_77EFFD2E, Hink__EndElement_77EFFD2E, Hink__IsVisibile_5E38073B } from "../Hink.fs.js";
import { equalsSafe } from "../../showcase/.fable/fable-library.3.0.0-nagareyama-beta-005/Util.js";
import { Browser_Types_CanvasRenderingContext2D__CanvasRenderingContext2D_Triangle_76A78260, Browser_Types_CanvasRenderingContext2D__CanvasRenderingContext2D_RoundedRect_49C497E8 } from "../Helpers.fs.js";
import { Theme__get_ArrowOffsetY, Theme__get_ArrowOffsetX, Theme__get_ButtonOffsetY } from "../Theme.fs.js";
import { defaultArg } from "../../showcase/.fable/fable-library.3.0.0-nagareyama-beta-005/Option.js";
import { item } from "../../showcase/.fable/fable-library.3.0.0-nagareyama-beta-005/List.js";

export function Hink_Gui_Hink__Hink_Combo_3C30ED05(this$, comboInfo, texts, label, labelAlign) {
    if (!Hink__IsVisibile_5E38073B(this$, this$.Theme.Element.Height)) {
        Hink__EndElement_77EFFD2E(this$);
        return false;
    }
    else {
        const hover = Hink__IsHover_77EFFD2E(this$);
        const released = Hink__IsReleased_77EFFD2E(this$);
        const pressed = Hink__IsPressed_77EFFD2E(this$);
        Hink__get_CurrentContext(this$).fillStyle = (pressed ? this$.Theme.Combo.Background.Pressed : (hover ? this$.Theme.Combo.Background.Hover : this$.Theme.Combo.Background.Default));
        const storeCombo = () => (new ComboHandler(comboInfo, this$.Cursor.X, (this$.Cursor.Y + this$.Theme.Element.Height) + this$.Theme.Element.SeparatorSize, this$.Cursor.Width, this$.Cursor.Height, texts, comboInfo));
        if (pressed) {
            if (equalsSafe(comboInfo.State, new ComboState(2))) {
                const arg0 = storeCombo();
                this$.CurrentCombo = arg0;
                comboInfo.State = (new ComboState(1));
            }
        }
        if (released) {
            const matchValue = comboInfo.State;
            switch (matchValue.tag) {
                case 0: {
                    comboInfo.State = (new ComboState(2));
                    break;
                }
                case 2: {
                    break;
                }
                default: {
                    comboInfo.State = (new ComboState(0));
                }
            }
        }
        const matchValue_1 = comboInfo.State;
        switch (matchValue_1.tag) {
            case 1: {
                const arg0_1 = storeCombo();
                this$.CurrentCombo = arg0_1;
                break;
            }
            case 2: {
                break;
            }
            default: {
                const arg0_1 = storeCombo();
                this$.CurrentCombo = arg0_1;
            }
        }
        Browser_Types_CanvasRenderingContext2D__CanvasRenderingContext2D_RoundedRect_49C497E8(Hink__get_CurrentContext(this$), this$.Cursor.X + Theme__get_ButtonOffsetY(this$.Theme), this$.Cursor.Y + Theme__get_ButtonOffsetY(this$.Theme), this$.Cursor.Width - (Theme__get_ButtonOffsetY(this$.Theme) * 2), this$.Theme.Element.Height, this$.Theme.Combo.CornerRadius);
        const offsetX = (this$.Cursor.X + this$.Cursor.Width) - (this$.Theme.Arrow.Width + (Theme__get_ArrowOffsetX(this$.Theme) * 2));
        const offsetY = this$.Cursor.Y + Theme__get_ArrowOffsetY(this$.Theme);
        Hink__get_CurrentContext(this$).fillStyle = this$.Theme.Text.Color;
        const matchValue_2 = comboInfo.SelectedIndex;
        if (matchValue_2 == null) {
            if (label == null) {
            }
            else {
                const text = label;
                Hink__FillSmallString_2C2B7D77(this$, text, void 0, void 0, void 0, defaultArg(labelAlign, new Align(0)));
            }
        }
        else {
            const index = matchValue_2 | 0;
            Hink__FillSmallString_2C2B7D77(this$, item(index, texts), void 0, void 0, void 0, defaultArg(labelAlign, new Align(0)));
        }
        Hink__get_CurrentContext(this$).fillStyle = this$.Theme.Arrow.Color;
        const matchValue_3 = comboInfo.State;
        switch (matchValue_3.tag) {
            case 0:
            case 1: {
                Browser_Types_CanvasRenderingContext2D__CanvasRenderingContext2D_Triangle_76A78260(Hink__get_CurrentContext(this$), offsetX + (this$.Theme.Arrow.Width / 2), offsetY, offsetX + this$.Theme.Arrow.Width, offsetY + this$.Theme.Arrow.Height, offsetX, offsetY + this$.Theme.Arrow.Height);
                break;
            }
            default: {
                Browser_Types_CanvasRenderingContext2D__CanvasRenderingContext2D_Triangle_76A78260(Hink__get_CurrentContext(this$), offsetX, offsetY, offsetX + this$.Theme.Arrow.Width, offsetY, offsetX + (this$.Theme.Arrow.Width / 2), offsetY + this$.Theme.Arrow.Height);
            }
        }
        Hink__EndElement_77EFFD2E(this$);
        return true;
    }
}

