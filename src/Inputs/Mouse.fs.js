import { Record as Record_1 } from "../../showcase/.fable/fable-library.3.0.0-nagareyama-beta-005/Types.js";
import { record_type, class_type, bool_type, float64_type } from "../../showcase/.fable/fable-library.3.0.0-nagareyama-beta-005/Reflection.js";

export const Cursor_Alias = "alias";

export const Cursor_AllScroll = "all-scroll";

export const Cursor_Auto = "auto";

export const Cursor_Cell = "cell";

export const Cursor_ContextMenu = "context-menu";

export const Cursor_ColResize = "col-resize";

export const Cursor_Copy = "copy";

export const Cursor_Crosshair = "crosshair";

export const Cursor_Default = "default";

export const Cursor_EResize = "e-resize";

export const Cursor_EwResize = "ew-resize";

export const Cursor_Grab = "grab";

export const Cursor_Grabbing = "grabbing";

export const Cursor_Help = "help";

export const Cursor_Move = "move";

export const Cursor_NResize = "n-resize";

export const Cursor_NeResize = "ne-resize";

export const Cursor_NeswResize = "nesw-resize";

export const Cursor_NsResize = "ns-resize";

export const Cursor_NwResize = "nw-resize";

export const Cursor_NwseResize = "nwse-resize";

export const Cursor_NoDrop = "no-drop";

export const Cursor_None = "none";

export const Cursor_NotAllowed = "not-allowed";

export const Cursor_Pointer = "pointer";

export const Cursor_Progress = "progress";

export const Cursor_RowResize = "row-resize";

export const Cursor_SResize = "s-resize";

export const Cursor_SeResize = "se-resize";

export const Cursor_SwResize = "sw-resize";

export const Cursor_Text = "text";

export const Cursor_URL = "URL";

export const Cursor_VerticalText = "vertical-text";

export const Cursor_WResize = "w-resize";

export const Cursor_Wait = "wait";

export const Cursor_ZoomIn = "zoom-in";

export const Cursor_ZoomOut = "zoom-out";

export const Cursor_Initial = "initial";

export const Cursor_Inherit = "inherit";

export class Record extends Record_1 {
    constructor(X, Y, Left, Right, Middle, IsDragging, DragOriginX, DragOriginY, HasBeenInitiated, Element$, JustReleased) {
        super();
        this.X = X;
        this.Y = Y;
        this.Left = Left;
        this.Right = Right;
        this.Middle = Middle;
        this.IsDragging = IsDragging;
        this.DragOriginX = DragOriginX;
        this.DragOriginY = DragOriginY;
        this.HasBeenInitiated = HasBeenInitiated;
        this.Element = Element$;
        this.JustReleased = JustReleased;
    }
}

export function Record$reflection() {
    return record_type("Hink.Inputs.Mouse.Record", [], Record, () => [["X", float64_type], ["Y", float64_type], ["Left", bool_type], ["Right", bool_type], ["Middle", bool_type], ["IsDragging", bool_type], ["DragOriginX", float64_type], ["DragOriginY", float64_type], ["HasBeenInitiated", bool_type], ["Element", class_type("Browser.Types.HTMLElement")], ["JustReleased", bool_type]]);
}

export function Record_get_Initial() {
    return new Record(0, 0, false, false, false, false, 0, 0, false, window.document.body, false);
}

export function Record__HitRegion_77D16AC0(this$, x, y, w, h) {
    if (((this$.X > x) ? (this$.X <= (x + w)) : false) ? (this$.Y > y) : false) {
        return this$.Y < (y + h);
    }
    else {
        return false;
    }
}

export function Record__SetCursor_Z721C83C5(this$, cursor) {
    (this$.Element.style).cursor = cursor;
}

export function Record__ResetCursor(this$) {
    (this$.Element.style).cursor = Cursor_Auto;
}

export function Record__ResetReleased(this$) {
    this$.JustReleased = false;
}

export function Record__ResetDragInfo(this$) {
    this$.DragOriginX = this$.X;
    this$.DragOriginY = this$.Y;
}

export function Record__get_DragDeltaX(this$) {
    if (this$.IsDragging) {
        return this$.X - this$.DragOriginX;
    }
    else {
        return 0;
    }
}

export function Record__get_DragDeltaY(this$) {
    if (this$.IsDragging) {
        return this$.Y - this$.DragOriginY;
    }
    else {
        return 0;
    }
}

export const Manager = Record_get_Initial();

export function init(element) {
    if (!Manager.HasBeenInitiated) {
        Manager.Element = element;
        element.addEventListener("mousedown", (ev) => {
            const ev_1 = ev;
            const matchValue = ev_1.button;
            switch (matchValue) {
                case 0: {
                    Manager.Left = true;
                    break;
                }
                case 1: {
                    Manager.Right = true;
                    break;
                }
                case 2: {
                    Manager.Middle = true;
                    break;
                }
                default: {
                    throw (new Error("Not supported ButtonStatetton"));
                }
            }
        });
        element.addEventListener("mouseup", (ev_2) => {
            const ev_3 = ev_2;
            const matchValue_1 = ev_3.button;
            switch (matchValue_1) {
                case 0: {
                    Manager.Left = false;
                    Manager.IsDragging = false;
                    Manager.DragOriginX = 0;
                    Manager.DragOriginY = 0;
                    Manager.JustReleased = true;
                    break;
                }
                case 1: {
                    Manager.Middle = false;
                    break;
                }
                case 2: {
                    Manager.Right = false;
                    break;
                }
                default: {
                    throw (new Error("Not supported button"));
                }
            }
        });
        element.addEventListener("mousemove", (ev_4) => {
            const ev_5 = ev_4;
            Manager.X = ev_5.offsetX;
            Manager.Y = ev_5.offsetY;
            if (Manager.Left) {
                if (!Manager.IsDragging) {
                    Manager.DragOriginX = Manager.X;
                    Manager.DragOriginY = Manager.Y;
                }
                Manager.IsDragging = true;
            }
        });
        Manager.HasBeenInitiated = true;
    }
}

