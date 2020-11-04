import { op_Subtraction, now as now_1 } from "../../showcase/.fable/fable-library.3.0.0-nagareyama-beta-005/Date.js";
import { ComboState, Hink__get_CurrentContext, oneSecond } from "../Hink.fs.js";
import { subtract } from "../../showcase/.fable/fable-library.3.0.0-nagareyama-beta-005/TimeSpan.js";
import { Hink_Gui_Hink__Hink_EndWindow } from "./Window.fs.js";
import { item, length } from "../../showcase/.fable/fable-library.3.0.0-nagareyama-beta-005/List.js";
import { Hink_Gui_Hink__Hink_Button_Z123163C7 } from "./Button.fs.js";
import { Record__ResetCursor, Record__ResetDragInfo, Record__ResetReleased } from "../Inputs/Mouse.fs.js";
import { Record__ClearLastKey } from "../Inputs/Keyboard.fs.js";

export function Hink_Gui_Hink__Hink_Prepare(this$) {
    const now = now_1();
    this$.Delta = op_Subtraction(now, this$.LastReferenceTick);
    const matchValue = this$.KeyboardCaptureHandler;
    if (matchValue == null) {
    }
    else {
        const handler = matchValue;
        this$.KeyboadHasBeenCapture = handler(this$.Keyboard);
    }
    if (this$.Delta > oneSecond) {
        this$.LastReferenceTick = now;
        this$.Delta = subtract(this$.Delta, oneSecond);
    }
}

export function Hink_Gui_Hink__Hink_Finish(this$) {
    if (this$.CurrentWindow != null) {
        Hink_Gui_Hink__Hink_EndWindow(this$);
    }
    this$.CurrentWindow = (void 0);
    this$.RowInfo = (void 0);
    const matchValue = this$.CurrentCombo;
    if (matchValue == null) {
    }
    else {
        const comboInfo = matchValue;
        const matchValue_1 = comboInfo.Reference.State;
        switch (matchValue_1.tag) {
            case 0:
            case 1: {
                this$.Cursor.X = comboInfo.X;
                this$.Cursor.Y = comboInfo.Y;
                this$.Cursor.Width = comboInfo.Width;
                const elementSize = this$.Theme.Element.Height + this$.Theme.Element.SeparatorSize;
                const boxHeight = length(comboInfo.Values) * elementSize;
                Hink__get_CurrentContext(this$).fillStyle = this$.Theme.Combo.Box.Default.Background;
                Hink__get_CurrentContext(this$).fillRect(this$.Cursor.X, this$.Cursor.Y, this$.Cursor.Width, boxHeight);
                for (let index = 0; index <= (length(comboInfo.Values) - 1); index++) {
                    if ((comboInfo.Reference.SelectedIndex != null) ? (index === comboInfo.Reference.SelectedIndex) : false) {
                        if (Hink_Gui_Hink__Hink_Button_Z123163C7(this$, item(index, comboInfo.Values), void 0, this$.Theme.Combo.Box.Selected.Background, this$.Theme.Combo.Box.Hover.Background, this$.Theme.Combo.Box.Selected.Background, this$.Theme.Combo.Box.Selected.Text, this$.Theme.Combo.Box.Hover.Text, this$.Theme.Combo.Box.Selected.Text)) {
                            comboInfo.Reference.SelectedIndex = index;
                            comboInfo.Reference.State = (new ComboState(2));
                            this$.CurrentCombo = (void 0);
                        }
                    }
                    else if (Hink_Gui_Hink__Hink_Button_Z123163C7(this$, item(index, comboInfo.Values), void 0, this$.Theme.Combo.Box.Default.Background, this$.Theme.Combo.Box.Hover.Background, this$.Theme.Combo.Box.Default.Background, this$.Theme.Combo.Box.Default.Text, this$.Theme.Combo.Box.Hover.Text, this$.Theme.Combo.Box.Default.Text)) {
                        comboInfo.Reference.SelectedIndex = index;
                        comboInfo.Reference.State = (new ComboState(2));
                        this$.CurrentCombo = (void 0);
                    }
                }
                break;
            }
            default: {
            }
        }
    }
    Record__ResetReleased(this$.Mouse);
    Record__ResetDragInfo(this$.Mouse);
    if (!this$.IsCursorStyled) {
        Record__ResetCursor(this$.Mouse);
    }
    this$.IsCursorStyled = false;
    this$.KeyboadHasBeenCapture = false;
    Record__ClearLastKey(this$.Keyboard);
}

