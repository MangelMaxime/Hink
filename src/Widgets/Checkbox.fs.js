import { Hink__FillSmallString_2C2B7D77, Hink__get_CurrentContext, Hink__IsReleased_77EFFD2E, Hink__IsHover_77EFFD2E, Hink__EndElement_77EFFD2E, Hink__IsVisibile_5E38073B } from "../Hink.fs.js";
import { Browser_Types_CanvasRenderingContext2D__CanvasRenderingContext2D_RoundedRect_49C497E8 } from "../Helpers.fs.js";
import { Theme__get_FormatFontString, Theme__get_CheckboxOffsetY, Theme__get_CheckboxOffsetX } from "../Theme.fs.js";

export function Hink_Gui_Hink__Hink_Checkbox_ZE132457(this$, checkboxInfo, label) {
    if (!Hink__IsVisibile_5E38073B(this$, this$.Theme.Element.Height)) {
        Hink__EndElement_77EFFD2E(this$);
        return false;
    }
    else {
        const hover = Hink__IsHover_77EFFD2E(this$);
        const released = Hink__IsReleased_77EFFD2E(this$);
        Hink__get_CurrentContext(this$).fillStyle = (checkboxInfo.Value ? this$.Theme.Checkbox.ActiveColor : this$.Theme.Checkbox.Color);
        Browser_Types_CanvasRenderingContext2D__CanvasRenderingContext2D_RoundedRect_49C497E8(Hink__get_CurrentContext(this$), this$.Cursor.X + Theme__get_CheckboxOffsetX(this$.Theme), this$.Cursor.Y + Theme__get_CheckboxOffsetY(this$.Theme), this$.Theme.Checkbox.Width, this$.Theme.Checkbox.Height, this$.Theme.Checkbox.CornerRadius);
        if ((hover ? (!checkboxInfo.Value) : false) ? true : checkboxInfo.Value) {
            Hink__get_CurrentContext(this$).fillStyle = this$.Theme.Checkbox.TickColor;
            Hink__get_CurrentContext(this$).font = Theme__get_FormatFontString(this$.Theme)(20);
            Hink__get_CurrentContext(this$).fillText("✓", this$.Cursor.X + (this$.Theme.Checkbox.Width / 2), this$.Cursor.Y + (this$.Theme.Element.Height / 2));
        }
        if (label == null) {
        }
        else {
            const label_1 = label;
            Hink__get_CurrentContext(this$).fillStyle = this$.Theme.Text.Color;
            Hink__FillSmallString_2C2B7D77(this$, label_1, this$.Theme.Checkbox.Width + (Theme__get_CheckboxOffsetX(this$.Theme) * 4));
        }
        if (released) {
            checkboxInfo.Value = (!checkboxInfo.Value);
        }
        Hink__EndElement_77EFFD2E(this$);
        return true;
    }
}

