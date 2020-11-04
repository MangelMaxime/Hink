import { Row__get_ActiveRatio, Row } from "../Hink.fs.js";

export function Hink_Gui_Hink__Hink_Row_Z33A93963(this$, ratios) {
    this$.RowInfo = (new Row(ratios, 0, this$.Cursor.X, this$.Cursor.Width));
    this$.Cursor.Width = (this$.Cursor.Width * Row__get_ActiveRatio(this$.RowInfo));
}

