import { createAtom } from "../.fable/fable-library.3.0.0-nagareyama-beta-005/Util.js";
import { FSharpRef } from "../.fable/fable-library.3.0.0-nagareyama-beta-005/Types.js";
import { Hink__Empty, Align, Hink_Create_Z68A72FE, SliderHandler_get_Default, InputHandler, InputHandler_get_Default, CheckboxInfo_get_Default, ComboInfo_get_Default, WindowHandler, WindowHandler_get_Default } from "../../src/Hink.fs.js";
import { resolveKeyFromKey } from "../../src/Inputs/Keyboard.fs.js";
import { Hink_Gui_Hink__Hink_Finish, Hink_Gui_Hink__Hink_Prepare } from "../../src/Widgets/LifeCycle.fs.js";
import { Hink_Gui_Hink__Hink_Window_Z25586FD2 } from "../../src/Widgets/Window.fs.js";
import { Hink_Gui_Hink__Hink_Label_7E23EBB7 } from "../../src/Widgets/Label.fs.js";
import { toConsole, printf, toText } from "../.fable/fable-library.3.0.0-nagareyama-beta-005/String.js";
import { Hink_Gui_Hink__Hink_Button_Z123163C7 } from "../../src/Widgets/Button.fs.js";
import { Hink_Gui_Hink__Hink_Row_Z33A93963 } from "../../src/Widgets/Row.fs.js";
import { Hink_Gui_Hink__Hink_Slider_Z704370D1 } from "../../src/Widgets/Slider.fs.js";
import { Hink_Gui_Hink__Hink_Combo_3C30ED05 } from "../../src/Widgets/Combo.fs.js";
import { ofArray } from "../.fable/fable-library.3.0.0-nagareyama-beta-005/List.js";
import { Hink_Gui_Hink__Hink_Checkbox_ZE132457 } from "../../src/Widgets/Checkbox.fs.js";
import { Hink_Gui_Hink__Hink_Input_Z1A61478C } from "../../src/Widgets/Input.fs.js";

export const canvas = document.getElementById("application");

export const buttonCounter = createAtom(0);

export const isChecked = new FSharpRef(false);

export const switchValue = new FSharpRef(false);

export const window1 = (() => {
    const inputRecord = WindowHandler_get_Default();
    return new WindowHandler(10, 10, 400, 400, inputRecord.Layout, inputRecord.Draggable, inputRecord.Closable, inputRecord.Closed, inputRecord.Title, inputRecord.DragXOrigin, inputRecord.DragYOrigin, inputRecord.IsDragging, inputRecord.ShouldRedraw, inputRecord._Canvas, inputRecord._Context);
})();

export const window2 = (() => {
    const inputRecord = WindowHandler_get_Default();
    return new WindowHandler(100, 50, 400, 285, inputRecord.Layout, true, true, true, "You can close me", inputRecord.DragXOrigin, inputRecord.DragYOrigin, inputRecord.IsDragging, inputRecord.ShouldRedraw, inputRecord._Canvas, inputRecord._Context);
})();

export const Emerald = "#2ecc71";

export const Nephritis = "#27ae60";

export const Carrot = "#e67e22";

export const Pumpkin = "#d35400";

export const Amethyst = "#9b59b6";

export const Wisteria = "#8e44ad";

export const combo1 = ComboInfo_get_Default();

export const checkbox1 = CheckboxInfo_get_Default();

export const input1 = (() => {
    const inputRecord = InputHandler_get_Default();
    return new InputHandler("Some text here", inputRecord.Selection, inputRecord.KeyboardCaptureHandler, inputRecord.CursorOffset, inputRecord.TextStartOrigin, inputRecord.Guid);
})();

export const input2 = InputHandler_get_Default();

export const slider1 = SliderHandler_get_Default();

export function keyboardPreventHandler(e) {
    let shouldPreventFromCtrl;
    if (e.ctrlKey) {
        const matchValue = resolveKeyFromKey(e.key);
        switch (matchValue.tag) {
            case 32: {
                shouldPreventFromCtrl = true;
                break;
            }
            case 43: {
                shouldPreventFromCtrl = true;
                break;
            }
            default: {
                shouldPreventFromCtrl = false;
            }
        }
    }
    else {
        shouldPreventFromCtrl = false;
    }
    return shouldPreventFromCtrl;
}

export function keyboardCaptureHandler(keyboard) {
    if (keyboard.Modifiers.Control) {
        const matchValue_1 = keyboard.LastKey;
        if (matchValue_1.tag === 43) {
            window2.Closed = false;
            return true;
        }
        else {
            return false;
        }
    }
    else {
        const matchValue_2 = keyboard.LastKey;
        if (matchValue_2.tag === 6) {
            if (!window2.Closed) {
                window2.Closed = true;
            }
            return true;
        }
        else {
            return false;
        }
    }
}

export const ui = Hink_Create_Z68A72FE(canvas, void 0, void 0, keyboardPreventHandler, keyboardCaptureHandler);

export const window2BackgroundColor = createAtom(ui.Theme.Window.Header.Color);

export function render(_arg1) {
    let arg10, clo1, arg10_1, clo1_1;
    ui.ApplicationContext.clearRect(0, 0, ui.Canvas.width, ui.Canvas.height);
    ui.ApplicationContext.fillStyle = "#fff";
    Hink_Gui_Hink__Hink_Prepare(ui);
    if (Hink_Gui_Hink__Hink_Window_Z25586FD2(ui, window1)) {
        Hink_Gui_Hink__Hink_Label_7E23EBB7(ui, (arg10 = (buttonCounter() | 0), (clo1 = toText(printf("Clicked: %i times")), clo1(arg10))), new Align(1));
        if (Hink_Gui_Hink__Hink_Button_Z123163C7(ui, "Click me")) {
            toConsole(printf("Click"));
            buttonCounter(buttonCounter() + 1, true);
        }
        Hink_Gui_Hink__Hink_Label_7E23EBB7(ui, "Row layout demo", new Align(1), "#34495e");
        Hink_Gui_Hink__Hink_Row_Z33A93963(ui, new Float64Array([1 / 2, 1 / 4, 1 / 4]));
        Hink_Gui_Hink__Hink_Label_7E23EBB7(ui, "1/2", new Align(1), "#f39c12");
        Hink_Gui_Hink__Hink_Label_7E23EBB7(ui, "1/4", new Align(1), "#27ae60");
        Hink_Gui_Hink__Hink_Label_7E23EBB7(ui, "1/4", new Align(1), "#8e44ad");
        Hink_Gui_Hink__Hink_Label_7E23EBB7(ui, "We filled all the row, so new line here", new Align(1), "#34495e");
        Hink_Gui_Hink__Hink_Label_7E23EBB7(ui, "Text truncated because it\u0027s too long from here");
        Hink_Gui_Hink__Hink_Row_Z33A93963(ui, new Float64Array([1 / 4, 1 / 2, 1 / 4]));
        Hink__Empty(ui);
        if (Hink_Gui_Hink__Hink_Button_Z123163C7(ui, "Open second Window")) {
            window2.Closed = false;
        }
        Hink__Empty(ui);
        Hink_Gui_Hink__Hink_Label_7E23EBB7(ui, (arg10_1 = slider1.Value, (clo1_1 = toText(printf("Slider: %.0f")), clo1_1(arg10_1))));
        const value = Hink_Gui_Hink__Hink_Slider_Z704370D1(ui, slider1);
        void value;
        Hink_Gui_Hink__Hink_Label_7E23EBB7(ui, "Use Ctrl+O to open the second Window", new Align(1));
    }
    if (Hink_Gui_Hink__Hink_Window_Z25586FD2(ui, window2, void 0, window2BackgroundColor())) {
        Hink_Gui_Hink__Hink_Label_7E23EBB7(ui, "Click to change window header", new Align(1));
        Hink_Gui_Hink__Hink_Row_Z33A93963(ui, new Float64Array([1 / 3, 1 / 3, 1 / 3]));
        if (Hink_Gui_Hink__Hink_Button_Z123163C7(ui, "Emerald", void 0, void 0, void 0, Emerald)) {
            window2BackgroundColor(Emerald, true);
        }
        if (Hink_Gui_Hink__Hink_Button_Z123163C7(ui, "Amethyst", void 0, void 0, void 0, Amethyst)) {
            window2BackgroundColor(Amethyst, true);
        }
        if (Hink_Gui_Hink__Hink_Button_Z123163C7(ui, "Carrot", void 0, void 0, void 0, Carrot)) {
            window2BackgroundColor(Carrot, true);
        }
        const value_1 = Hink_Gui_Hink__Hink_Combo_3C30ED05(ui, combo1, ofArray(["Fable", "Elm", "Haxe"]), "Default", new Align(1));
        void value_1;
        const value_2 = Hink_Gui_Hink__Hink_Checkbox_ZE132457(ui, checkbox1, "Remember me");
        void value_2;
        const value_3 = Hink_Gui_Hink__Hink_Input_Z1A61478C(ui, input1);
        void value_3;
        const value_4 = Hink_Gui_Hink__Hink_Input_Z1A61478C(ui, input2);
        void value_4;
        Hink__Empty(ui);
        Hink_Gui_Hink__Hink_Label_7E23EBB7(ui, "Close me by pressing Espace", new Align(1));
    }
    Hink_Gui_Hink__Hink_Finish(ui);
    const value_5 = window.requestAnimationFrame((dt) => {
        render(dt);
    });
    void value_5;
}

render(0);

