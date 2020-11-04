import { Record as Record_1, Union } from "../../showcase/.fable/fable-library.3.0.0-nagareyama-beta-005/Types.js";
import { class_type, record_type, bool_type, string_type, union_type } from "../../showcase/.fable/fable-library.3.0.0-nagareyama-beta-005/Reflection.js";
import { remove, add, FSharpSet__Contains, empty } from "../../showcase/.fable/fable-library.3.0.0-nagareyama-beta-005/Set.js";
import { compareSafe } from "../../showcase/.fable/fable-library.3.0.0-nagareyama-beta-005/Util.js";

export class KeyMod_KeyMod extends Union {
    constructor(tag, ...fields) {
        super();
        this.tag = (tag | 0);
        this.fields = fields;
    }
    cases() {
        return ["Alt", "Control", "Shift", "NoMod"];
    }
}

export function KeyMod_KeyMod$reflection() {
    return union_type("Hink.Inputs.Keyboard.KeyMod.KeyMod", [], KeyMod_KeyMod, () => [[], [], [], []]);
}

export class Keys_Keys extends Union {
    constructor(tag, ...fields) {
        super();
        this.tag = (tag | 0);
        this.fields = fields;
    }
    cases() {
        return ["Unkown", "Dead", "Ampersand", "Backspace", "Enter", "Control", "Escape", "Tab", "Space", "End", "Home", "ArrowLeft", "ArrowUp", "ArrowRight", "ArrowDown", "Delete", "OS", "F1", "F2", "F3", "F4", "F5", "F6", "F7", "F8", "F9", "F10", "F11", "F12", "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z", "SingleQuote", "Slash"];
    }
}

export function Keys_Keys$reflection() {
    return union_type("Hink.Inputs.Keyboard.Keys.Keys", [], Keys_Keys, () => [[["Item", string_type]], [], [], [], [], [], [], [], [], [], [], [], [], [], [], [], [], [], [], [], [], [], [], [], [], [], [], [], [], [], [], [], [], [], [], [], [], [], [], [], [], [], [], [], [], [], [], [], [], [], [], [], [], [], [], [], []]);
}

export function resolveKeyFromKey(key) {
    const matchValue = key.toLocaleLowerCase();
    switch (matchValue) {
        case "\u0026": {
            return new Keys_Keys(2);
        }
        case "backspace": {
            return new Keys_Keys(3);
        }
        case "tab": {
            return new Keys_Keys(7);
        }
        case "enter": {
            return new Keys_Keys(4);
        }
        case "control": {
            return new Keys_Keys(5);
        }
        case "escape": {
            return new Keys_Keys(6);
        }
        case "space": {
            return new Keys_Keys(8);
        }
        case "end": {
            return new Keys_Keys(9);
        }
        case "home": {
            return new Keys_Keys(10);
        }
        case "arrowleft": {
            return new Keys_Keys(11);
        }
        case "arrowup": {
            return new Keys_Keys(12);
        }
        case "arrowright": {
            return new Keys_Keys(13);
        }
        case "arrowdown": {
            return new Keys_Keys(14);
        }
        case "delete": {
            return new Keys_Keys(15);
        }
        case "os": {
            return new Keys_Keys(16);
        }
        case "f1": {
            return new Keys_Keys(17);
        }
        case "f2": {
            return new Keys_Keys(18);
        }
        case "f3": {
            return new Keys_Keys(19);
        }
        case "f4": {
            return new Keys_Keys(20);
        }
        case "f5": {
            return new Keys_Keys(21);
        }
        case "f6": {
            return new Keys_Keys(22);
        }
        case "f7": {
            return new Keys_Keys(23);
        }
        case "f8": {
            return new Keys_Keys(24);
        }
        case "f9": {
            return new Keys_Keys(25);
        }
        case "f10": {
            return new Keys_Keys(26);
        }
        case "f11": {
            return new Keys_Keys(27);
        }
        case "f12": {
            return new Keys_Keys(28);
        }
        case "/": {
            return new Keys_Keys(56);
        }
        case "\u0027": {
            return new Keys_Keys(55);
        }
        case "a": {
            return new Keys_Keys(29);
        }
        case "b": {
            return new Keys_Keys(30);
        }
        case "c": {
            return new Keys_Keys(31);
        }
        case "d": {
            return new Keys_Keys(32);
        }
        case "e": {
            return new Keys_Keys(33);
        }
        case "f": {
            return new Keys_Keys(34);
        }
        case "g": {
            return new Keys_Keys(35);
        }
        case "h": {
            return new Keys_Keys(36);
        }
        case "i": {
            return new Keys_Keys(37);
        }
        case "j": {
            return new Keys_Keys(38);
        }
        case "k": {
            return new Keys_Keys(39);
        }
        case "l": {
            return new Keys_Keys(40);
        }
        case "m": {
            return new Keys_Keys(41);
        }
        case "n": {
            return new Keys_Keys(42);
        }
        case "o": {
            return new Keys_Keys(43);
        }
        case "p": {
            return new Keys_Keys(44);
        }
        case "q": {
            return new Keys_Keys(45);
        }
        case "r": {
            return new Keys_Keys(46);
        }
        case "s": {
            return new Keys_Keys(47);
        }
        case "t": {
            return new Keys_Keys(48);
        }
        case "u": {
            return new Keys_Keys(49);
        }
        case "v": {
            return new Keys_Keys(50);
        }
        case "w": {
            return new Keys_Keys(51);
        }
        case "x": {
            return new Keys_Keys(52);
        }
        case "y": {
            return new Keys_Keys(53);
        }
        case "z": {
            return new Keys_Keys(54);
        }
        default: {
            return new Keys_Keys(0, key);
        }
    }
}

export class Modifiers extends Record_1 {
    constructor(Shift, Control, CommandLeft, CommandRight, Alt) {
        super();
        this.Shift = Shift;
        this.Control = Control;
        this.CommandLeft = CommandLeft;
        this.CommandRight = CommandRight;
        this.Alt = Alt;
    }
}

export function Modifiers$reflection() {
    return record_type("Hink.Inputs.Keyboard.Modifiers", [], Modifiers, () => [["Shift", bool_type], ["Control", bool_type], ["CommandLeft", bool_type], ["CommandRight", bool_type], ["Alt", bool_type]]);
}

export function Modifiers_get_Initial() {
    return new Modifiers(false, false, false, false, false);
}

export class Record extends Record_1 {
    constructor(KeysPressed, LastKeyValue, LastKeyIsPrintable, LastKey, Modifiers) {
        super();
        this.KeysPressed = KeysPressed;
        this.LastKeyValue = LastKeyValue;
        this.LastKeyIsPrintable = LastKeyIsPrintable;
        this.LastKey = LastKey;
        this.Modifiers = Modifiers;
    }
}

export function Record$reflection() {
    return record_type("Hink.Inputs.Keyboard.Record", [], Record, () => [["KeysPressed", class_type("Microsoft.FSharp.Collections.FSharpSet`1", [Keys_Keys$reflection()])], ["LastKeyValue", string_type], ["LastKeyIsPrintable", bool_type], ["LastKey", Keys_Keys$reflection()], ["Modifiers", Modifiers$reflection()]]);
}

export function Record__HasNewKeyStroke(this$) {
    return this$.LastKeyValue !== "";
}

export function Record_get_Initial() {
    return new Record(empty({
        Compare: compareSafe,
    }), "", false, new Keys_Keys(0, ""), Modifiers_get_Initial());
}

export function Record__IsPress_7D622389(self, key) {
    return FSharpSet__Contains(self.KeysPressed, key);
}

export function Record__ClearLastKey(self) {
    self.LastKeyValue = "";
    self.LastKeyIsPrintable = false;
    self.LastKey = (new Keys_Keys(0, ""));
}

export const Manager = Record_get_Initial();

export function init(element, preventDefault, userPreventHandler) {
    const updateModifiers = (e) => {
        Manager.Modifiers.Alt = e.altKey;
        Manager.Modifiers.Shift = e.shiftKey;
        Manager.Modifiers.Control = e.ctrlKey;
        Manager.Modifiers.CommandLeft = (e.keyCode === 224);
        Manager.Modifiers.CommandRight = (e.keyCode === 224);
    };
    element.addEventListener("keydown", (e_1) => {
        const e_2 = e_1;
        const key = resolveKeyFromKey(e_2.key);
        Manager.LastKeyValue = e_2.key;
        const isPrintableKey = (key.tag === 17) ? false : ((key.tag === 18) ? false : ((key.tag === 19) ? false : ((key.tag === 20) ? false : ((key.tag === 21) ? false : ((key.tag === 22) ? false : ((key.tag === 23) ? false : ((key.tag === 24) ? false : ((key.tag === 25) ? false : ((key.tag === 26) ? false : ((key.tag === 27) ? false : ((key.tag === 28) ? false : ((key.tag === 16) ? false : true))))))))))));
        Manager.LastKeyIsPrintable = (((1 <= e_2.key.length) ? (e_2.key.length <= 2) : false) ? isPrintableKey : false);
        Manager.LastKey = key;
        Manager.KeysPressed = add(key, Manager.KeysPressed);
        updateModifiers(e_2);
    });
    element.addEventListener("keyup", (e_3) => {
        const e_4 = e_3;
        const code = (~(~e_4.keyCode)) | 0;
        Manager.KeysPressed = remove(resolveKeyFromKey(e_4.key), Manager.KeysPressed);
        updateModifiers(e_4);
    });
    if (preventDefault) {
        element.addEventListener("keydown", (e_5) => {
            const e_6 = e_5;
            let shouldPreventFromCtrl;
            if (e_6.ctrlKey) {
                const matchValue = resolveKeyFromKey(e_6.key);
                shouldPreventFromCtrl = ((matchValue.tag === 29) ? true : false);
            }
            else {
                shouldPreventFromCtrl = false;
            }
            let shouldPreventFromShift;
            if (e_6.shiftKey) {
                const matchValue_1 = resolveKeyFromKey(e_6.key);
                shouldPreventFromShift = ((matchValue_1.tag === 56) ? true : false);
            }
            else {
                shouldPreventFromShift = false;
            }
            let shouldPreventNoModifier;
            const matchValue_2 = resolveKeyFromKey(e_6.key);
            switch (matchValue_2.tag) {
                case 7: {
                    shouldPreventNoModifier = true;
                    break;
                }
                case 3: {
                    shouldPreventNoModifier = true;
                    break;
                }
                case 55: {
                    shouldPreventNoModifier = true;
                    break;
                }
                default: {
                    shouldPreventNoModifier = false;
                }
            }
            if ((shouldPreventNoModifier ? true : shouldPreventFromCtrl) ? true : shouldPreventFromShift) {
                e_6.preventDefault();
            }
        });
    }
    if (userPreventHandler == null) {
    }
    else {
        const handler = userPreventHandler;
        element.addEventListener("keydown", (e_7) => {
            const e_8 = e_7;
            if (handler(e_8)) {
                e_8.preventDefault();
            }
        });
    }
}

