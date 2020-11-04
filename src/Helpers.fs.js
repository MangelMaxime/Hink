import { Union, toString } from "../showcase/.fable/fable-library.3.0.0-nagareyama-beta-005/Types.js";
import { union_type } from "../showcase/.fable/fable-library.3.0.0-nagareyama-beta-005/Reflection.js";
import { defaultArg } from "../showcase/.fable/fable-library.3.0.0-nagareyama-beta-005/Option.js";
import { sort, tryFind, cons, empty } from "../showcase/.fable/fable-library.3.0.0-nagareyama-beta-005/List.js";
import { comparePrimitives } from "../showcase/.fable/fable-library.3.0.0-nagareyama-beta-005/Util.js";

export function Color_op_Dollar(s, n) {
    let copyOfStruct;
    return s + (copyOfStruct = n, toString(copyOfStruct));
}

export function Color_rgb(r, g, b) {
    return Color_op_Dollar(Color_op_Dollar(Color_op_Dollar(Color_op_Dollar(Color_op_Dollar(Color_op_Dollar("rgb(", r), ","), g), ","), b), ")");
}

export function System_Math__Math_Clamp_Static_Z7AD9E565(value, min, max) {
    let res = value;
    if (value < min) {
        res = min;
    }
    if (value > max) {
        res = max;
    }
    return res;
}

export class RenderingType extends Union {
    constructor(tag, ...fields) {
        super();
        this.tag = (tag | 0);
        this.fields = fields;
    }
    cases() {
        return ["Stroke", "Fill", "StrokeAndFill"];
    }
}

export function RenderingType$reflection() {
    return union_type("Hink.Helpers.RenderingType", [], RenderingType, () => [[], [], []]);
}

export function Browser_Types_CanvasRenderingContext2D__CanvasRenderingContext2D_RoundedRect_49C497E8(this$, x, y, width, height, radius, action) {
    const action_1 = defaultArg(action, new RenderingType(1));
    this$.save();
    this$.beginPath();
    this$.moveTo(x + radius, y);
    this$.arcTo(x + width, y, x + width, y + radius, radius);
    this$.arcTo(x + width, y + height, (x + width) - radius, y + height, radius);
    this$.arcTo(x, y + height, x, (y + height) - radius, radius);
    this$.arcTo(x, y, x + radius, y, radius);
    switch (action_1.tag) {
        case 1: {
            this$.fill();
            break;
        }
        case 2: {
            this$.fill();
            this$.stroke();
            break;
        }
        default: {
            this$.stroke();
        }
    }
    this$.restore();
}

export function Browser_Types_CanvasRenderingContext2D__CanvasRenderingContext2D_Triangle_76A78260(this$, x, y, x1, y1, x2, y2) {
    this$.save();
    this$.beginPath();
    this$.moveTo(x, y);
    this$.lineTo(x1, y1);
    this$.lineTo(x2, y2);
    this$.fill();
    this$.restore();
}

export function System_String__String_FindOccurence_244C7CD6(self, c) {
    let indexes = empty();
    for (let i = 0; i <= self.length; i++) {
        if (self[i] === c) {
            indexes = cons(i, indexes);
        }
    }
    return indexes;
}

export function NextIndexBackward(value, c, reference) {
    let x_1;
    const list = System_String__String_FindOccurence_244C7CD6(value, c);
    x_1 = tryFind((x) => (x < reference), list);
    if (x_1 == null) {
        return 0;
    }
    else {
        const index = x_1 | 0;
        return index | 0;
    }
}

export function NextIndexForward(value, c, reference) {
    let x_2;
    let list_1;
    const list = System_String__String_FindOccurence_244C7CD6(value, c);
    list_1 = sort(list, {
        Compare: comparePrimitives,
    });
    x_2 = tryFind((x_1) => (x_1 > reference), list_1);
    if (x_2 == null) {
        return value.length | 0;
    }
    else {
        const index = x_2 | 0;
        return index | 0;
    }
}

