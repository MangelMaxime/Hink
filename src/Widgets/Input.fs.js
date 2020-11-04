import { SelectionArea__Edging_Z524259A4, InputHandler__ClearSelection, Hink__FillSmallString_2C2B7D77, SelectionArea__get_Length, Hink__IsActive_244AC511, Hink__get_CurrentContext, Hink__SetActiveWidget_244AC511, Hink__SetCursor_Z721C83C5, Hink__IsPressed_77EFFD2E, Hink__IsHover_77EFFD2E, Hink__EndElement_77EFFD2E, Hink__IsVisibile_5E38073B, InputHandler__SetSelection_75B7AC0D, SelectionArea, SelectionArea_Create_Z37302880 } from "../Hink.fs.js";
import { Cursor_Text } from "../Inputs/Mouse.fs.js";
import { NextIndexForward, NextIndexBackward, RenderingType, Browser_Types_CanvasRenderingContext2D__CanvasRenderingContext2D_RoundedRect_49C497E8 } from "../Helpers.fs.js";
import { Theme__get_FontSmallOffsetY, Theme__get_FormatFontString, Theme__get_ButtonOffsetY } from "../Theme.fs.js";
import { insert, remove, substring } from "../../showcase/.fable/fable-library.3.0.0-nagareyama-beta-005/String.js";
import { max, curry, comparePrimitives, min } from "../../showcase/.fable/fable-library.3.0.0-nagareyama-beta-005/Util.js";
import { Record__HasNewKeyStroke } from "../Inputs/Keyboard.fs.js";

export function handleBackwardSelection(info, newCursorOffset) {
    if (info.Value.length > 0) {
        const oldCursorOffset = info.CursorOffset | 0;
        info.CursorOffset = newCursorOffset;
        let arg00;
        const matchValue = info.Selection;
        if (matchValue == null) {
            arg00 = SelectionArea_Create_Z37302880(info.CursorOffset, oldCursorOffset);
        }
        else {
            const selection = matchValue;
            arg00 = ((oldCursorOffset === selection.Start) ? (new SelectionArea(info.CursorOffset, selection.End)) : (new SelectionArea(selection.Start, newCursorOffset)));
        }
        InputHandler__SetSelection_75B7AC0D(info, arg00);
    }
}

export function handleForwardSelection(info, newCursorOffset) {
    if (info.Value.length > 0) {
        const oldCursorOffset = info.CursorOffset | 0;
        info.CursorOffset = newCursorOffset;
        let arg00;
        const matchValue = info.Selection;
        if (matchValue == null) {
            arg00 = SelectionArea_Create_Z37302880(oldCursorOffset, info.CursorOffset);
        }
        else {
            const selection = matchValue;
            if (selection.End === oldCursorOffset) {
                const End = info.CursorOffset | 0;
                arg00 = (new SelectionArea(selection.Start, End));
            }
            else {
                arg00 = (new SelectionArea(newCursorOffset, selection.End));
            }
        }
        InputHandler__SetSelection_75B7AC0D(info, arg00);
    }
}

export function Hink_Gui_Hink__Hink_Input_Z1A61478C(this$, info) {
    if (!Hink__IsVisibile_5E38073B(this$, this$.Theme.Element.Height)) {
        Hink__EndElement_77EFFD2E(this$);
        return false;
    }
    else {
        const hover = Hink__IsHover_77EFFD2E(this$);
        const pressed = Hink__IsPressed_77EFFD2E(this$);
        if (hover) {
            Hink__SetCursor_Z721C83C5(this$, Cursor_Text);
        }
        if (pressed) {
            Hink__SetActiveWidget_244AC511(this$, info.Guid);
        }
        Hink__get_CurrentContext(this$).strokeStyle = (Hink__IsActive_244AC511(this$, info.Guid) ? this$.Theme.Input.Border.Active : this$.Theme.Input.Border.Default);
        Hink__get_CurrentContext(this$).fillStyle = (Hink__IsActive_244AC511(this$, info.Guid) ? this$.Theme.Input.Background.Active : this$.Theme.Input.Background.Default);
        Hink__get_CurrentContext(this$).lineWidth = 2;
        Browser_Types_CanvasRenderingContext2D__CanvasRenderingContext2D_RoundedRect_49C497E8(Hink__get_CurrentContext(this$), this$.Cursor.X + Theme__get_ButtonOffsetY(this$.Theme), this$.Cursor.Y + Theme__get_ButtonOffsetY(this$.Theme), this$.Cursor.Width - (Theme__get_ButtonOffsetY(this$.Theme) * 2), this$.Theme.Element.Height, this$.Theme.Element.CornerRadius, new RenderingType(2));
        Hink__get_CurrentContext(this$).font = Theme__get_FormatFontString(this$.Theme)(this$.Theme.FontSmallSize);
        const textSize = Hink__get_CurrentContext(this$).measureText(info.Value);
        const charSize = Hink__get_CurrentContext(this$).measureText(" ");
        let maxChar;
        const value = (this$.Cursor.Width - (this$.Theme.Text.OffsetX * 2)) / charSize.width;
        maxChar = (~(~value));
        const text = (textSize.width > (this$.Cursor.Width - (this$.Theme.Text.OffsetX * 2))) ? substring(info.Value, info.TextStartOrigin, maxChar) : info.Value;
        const matchValue = info.Selection;
        if (matchValue == null) {
        }
        else {
            const selection = matchValue;
            let selectionSize;
            const arg00 = substring(info.Value, selection.Start, SelectionArea__get_Length(selection));
            const objectArg = Hink__get_CurrentContext(this$);
            selectionSize = objectArg.measureText(arg00);
            let startX;
            if (selection.Start === 0) {
                startX = this$.Cursor.X;
            }
            else {
                let leftSize;
                const arg00_1 = substring(info.Value, 0, selection.Start);
                const objectArg_1 = Hink__get_CurrentContext(this$);
                leftSize = objectArg_1.measureText(arg00_1);
                startX = (this$.Cursor.X + leftSize.width);
            }
            const selectionHeight = this$.Theme.FontSmallSize * 1.5;
            Hink__get_CurrentContext(this$).fillStyle = this$.Theme.Input.SelectionColor;
            Hink__get_CurrentContext(this$).fillRect(startX + this$.Theme.Text.OffsetX, this$.Cursor.Y + ((this$.Theme.Element.Height - selectionHeight) / 2), min(comparePrimitives, selectionSize.width, charSize.width * maxChar), selectionHeight);
        }
        Hink__get_CurrentContext(this$).fillStyle = this$.Theme.Input.TextColor;
        Hink__get_CurrentContext(this$).fillText(text, this$.Cursor.X + this$.Theme.Text.OffsetX, (this$.Cursor.Y + Theme__get_FontSmallOffsetY(this$.Theme)) + this$.Theme.Text.OffsetY);
        if ((Hink__IsActive_244AC511(this$, info.Guid) ? (this$.Delta < 500) : false) ? (info.Selection == null) : false) {
            Hink__get_CurrentContext(this$).fillStyle = "#000";
            const cursorMetrics = Hink__get_CurrentContext(this$).measureText("|");
            const textMetrics = Hink__get_CurrentContext(this$).measureText(info.Value);
            let cursorOffsetMetrics;
            const arg00_2 = substring(info.Value, min(comparePrimitives, info.CursorOffset, maxChar));
            const objectArg_2 = Hink__get_CurrentContext(this$);
            cursorOffsetMetrics = objectArg_2.measureText(arg00_2);
            Hink__FillSmallString_2C2B7D77(this$, "|", ((textMetrics.width - (cursorMetrics.width / 2)) - cursorOffsetMetrics.width) + this$.Theme.Text.OffsetX);
        }
        if (Hink__IsActive_244AC511(this$, info.Guid)) {
            if (Record__HasNewKeyStroke(this$.Keyboard)) {
                let isCapture;
                let capturedByUser;
                const matchValue_1 = info.KeyboardCaptureHandler;
                if (curry(2, matchValue_1) == null) {
                    capturedByUser = false;
                }
                else {
                    const handler = curry(2, matchValue_1);
                    capturedByUser = handler(info)(this$.Keyboard);
                }
                let res = true;
                if (!capturedByUser) {
                    const matchValue_2 = this$.Keyboard.Modifiers;
                    if (matchValue_2.Shift) {
                        if (matchValue_2.Control) {
                            const matchValue_3 = this$.Keyboard.LastKey;
                            switch (matchValue_3.tag) {
                                case 11: {
                                    const newCursorOffset = NextIndexBackward(info.Value, " ", info.CursorOffset + info.TextStartOrigin) | 0;
                                    handleBackwardSelection(info, newCursorOffset);
                                    break;
                                }
                                case 13: {
                                    const newCursorOffset_1 = NextIndexForward(info.Value, " ", info.CursorOffset + info.TextStartOrigin) | 0;
                                    handleForwardSelection(info, newCursorOffset_1);
                                    break;
                                }
                                default: {
                                    res = false;
                                }
                            }
                        }
                        else {
                            const matchValue_5 = this$.Keyboard.LastKey;
                            switch (matchValue_5.tag) {
                                case 11: {
                                    const newCursorOffset_2 = max(comparePrimitives, 0, info.CursorOffset - 1) | 0;
                                    handleBackwardSelection(info, newCursorOffset_2);
                                    break;
                                }
                                case 13: {
                                    const newCursorOffset_3 = min(comparePrimitives, info.CursorOffset + 1, info.Value.length) | 0;
                                    handleForwardSelection(info, newCursorOffset_3);
                                    break;
                                }
                                default: {
                                    res = false;
                                }
                            }
                        }
                    }
                    else if (matchValue_2.Control) {
                        InputHandler__ClearSelection(info);
                        const matchValue_4 = this$.Keyboard.LastKey;
                        switch (matchValue_4.tag) {
                            case 11: {
                                if (info.Value.length > 0) {
                                    const oldCursorOffset = info.CursorOffset | 0;
                                    const index = NextIndexBackward(info.Value, " ", info.CursorOffset + info.TextStartOrigin) | 0;
                                    if ((index - info.TextStartOrigin) < 0) {
                                        info.CursorOffset = 0;
                                        info.TextStartOrigin = index;
                                    }
                                    else {
                                        info.CursorOffset = (index - info.TextStartOrigin);
                                    }
                                }
                                break;
                            }
                            case 13: {
                                if (info.Value.length > 0) {
                                    info.CursorOffset = NextIndexForward(info.Value, " ", info.CursorOffset + info.TextStartOrigin);
                                }
                                break;
                            }
                            case 3: {
                                if (info.Value.length > 0) {
                                    const index_1 = NextIndexBackward(info.Value, " ", info.CursorOffset) | 0;
                                    const delta = (info.CursorOffset - index_1) | 0;
                                    info.Value = remove(info.Value, index_1, delta);
                                    info.CursorOffset = (info.CursorOffset - delta);
                                }
                                break;
                            }
                            case 15: {
                                if (info.Value.length > 0) {
                                    const index_2 = NextIndexForward(info.Value, " ", info.CursorOffset) | 0;
                                    info.Value = remove(info.Value, info.CursorOffset, index_2 - info.CursorOffset);
                                }
                                break;
                            }
                            case 29: {
                                if (info.Value.length > 0) {
                                    info.Selection = SelectionArea_Create_Z37302880(0, info.Value.length);
                                    info.CursorOffset = info.Value.length;
                                }
                                break;
                            }
                            default: {
                                res = false;
                            }
                        }
                    }
                    else {
                        const oldSelection = (info.Selection != null) ? [true, info.Selection] : [false, SelectionArea_Create_Z37302880(0, 0)];
                        const matchValue_6 = this$.Keyboard.LastKey;
                        switch (matchValue_6.tag) {
                            case 3: {
                                InputHandler__ClearSelection(info);
                                if (info.Value.length > 0) {
                                    if (oldSelection[0]) {
                                        const selection_1 = oldSelection[1];
                                        info.Value = remove(info.Value, selection_1.Start, SelectionArea__get_Length(selection_1));
                                        if (info.CursorOffset === selection_1.End) {
                                            info.CursorOffset = max(comparePrimitives, info.CursorOffset - SelectionArea__get_Length(selection_1), 0);
                                        }
                                    }
                                    else if (info.CursorOffset > 0) {
                                        if (info.TextStartOrigin > 0) {
                                            info.TextStartOrigin = (info.TextStartOrigin - 1);
                                            info.Value = remove(info.Value, info.TextStartOrigin + info.CursorOffset, 1);
                                        }
                                        else {
                                            info.CursorOffset = (info.CursorOffset - 1);
                                            info.Value = remove(info.Value, info.CursorOffset, 1);
                                        }
                                    }
                                }
                                break;
                            }
                            case 15: {
                                InputHandler__ClearSelection(info);
                                if (info.Value.length > 0) {
                                    if (oldSelection[0]) {
                                        const selection_2 = oldSelection[1];
                                        info.Value = remove(info.Value, selection_2.Start, SelectionArea__get_Length(selection_2));
                                        if (info.CursorOffset === selection_2.End) {
                                            info.CursorOffset = max(comparePrimitives, info.CursorOffset - SelectionArea__get_Length(selection_2), 0);
                                        }
                                    }
                                    else if (info.CursorOffset < info.Value.length) {
                                        info.Value = remove(info.Value, info.CursorOffset, 1);
                                    }
                                    if (info.Value.length === 0) {
                                        info.TextStartOrigin = 0;
                                    }
                                }
                                break;
                            }
                            case 11: {
                                InputHandler__ClearSelection(info);
                                if (oldSelection[0]) {
                                    if (SelectionArea__Edging_Z524259A4(oldSelection[1], info.Value.length)) {
                                        info.CursorOffset = 0;
                                        info.TextStartOrigin = 0;
                                    }
                                    else {
                                        info.CursorOffset = max(comparePrimitives, 0, info.CursorOffset - 1);
                                    }
                                }
                                else {
                                    info.CursorOffset = max(comparePrimitives, 0, info.CursorOffset - 1);
                                }
                                if (info.CursorOffset === 0) {
                                    if (info.TextStartOrigin >= 1) {
                                        info.TextStartOrigin = (info.TextStartOrigin - 1);
                                    }
                                    else {
                                        info.TextStartOrigin = 0;
                                    }
                                }
                                break;
                            }
                            case 13: {
                                InputHandler__ClearSelection(info);
                                if (oldSelection[0]) {
                                    if (SelectionArea__Edging_Z524259A4(oldSelection[1], info.Value.length)) {
                                        info.CursorOffset = info.Value.length;
                                    }
                                    else {
                                        info.CursorOffset = min(comparePrimitives, info.CursorOffset + 1, min(comparePrimitives, maxChar, info.Value.length));
                                    }
                                }
                                else {
                                    info.CursorOffset = min(comparePrimitives, info.CursorOffset + 1, min(comparePrimitives, maxChar, info.Value.length));
                                }
                                if (info.CursorOffset >= maxChar) {
                                    info.TextStartOrigin = min(comparePrimitives, info.Value.length - maxChar, info.TextStartOrigin + 1);
                                    info.CursorOffset = maxChar;
                                }
                                break;
                            }
                            case 10: {
                                InputHandler__ClearSelection(info);
                                info.CursorOffset = 0;
                                info.TextStartOrigin = 0;
                                break;
                            }
                            case 9: {
                                InputHandler__ClearSelection(info);
                                if (info.Value.length > maxChar) {
                                    info.CursorOffset = maxChar;
                                    info.TextStartOrigin = (info.Value.length - maxChar);
                                }
                                else {
                                    info.CursorOffset = info.Value.length;
                                }
                                break;
                            }
                            case 6: {
                                InputHandler__ClearSelection(info);
                                break;
                            }
                            default: {
                                res = false;
                            }
                        }
                    }
                }
                const matchValue_7 = info.Selection;
                if (matchValue_7 == null) {
                }
                else {
                    const selection_5 = matchValue_7;
                    if (selection_5.Start === selection_5.End) {
                        info.Selection = (void 0);
                    }
                    else if (selection_5.Start > selection_5.End) {
                        info.Selection = SelectionArea_Create_Z37302880(selection_5.End, selection_5.Start);
                    }
                }
                if (!res) {
                    res = this$.KeyboadHasBeenCapture;
                }
                isCapture = res;
                if ((!isCapture) ? this$.Keyboard.LastKeyIsPrintable : false) {
                    const matchValue_8 = info.Selection;
                    if (matchValue_8 == null) {
                    }
                    else {
                        const selection_6 = matchValue_8;
                        info.Value = remove(info.Value, selection_6.Start, SelectionArea__get_Length(selection_6));
                        if (info.CursorOffset === selection_6.End) {
                            info.CursorOffset = max(comparePrimitives, info.CursorOffset - SelectionArea__get_Length(selection_6), 0);
                            info.TextStartOrigin = 0;
                        }
                    }
                    info.Value = insert(info.Value, info.CursorOffset + info.TextStartOrigin, this$.Keyboard.LastKeyValue);
                    info.CursorOffset = (info.CursorOffset + 1);
                    if (info.CursorOffset > maxChar) {
                        info.CursorOffset = maxChar;
                        info.TextStartOrigin = (info.TextStartOrigin + 1);
                    }
                    InputHandler__ClearSelection(info);
                }
            }
        }
        Hink__EndElement_77EFFD2E(this$);
        return true;
    }
}

