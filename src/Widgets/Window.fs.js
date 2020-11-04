import { Hink__get_CursorPosY, Hink__get_CursorPosX, WindowHandler__EnsureContext, Hink__IsHover_77EFFD2E, WindowHandler__get_RealPositionY, WindowHandler__get_RealPositionX, Hink__get_CurrentContext } from "../Hink.fs.js";
import { defaultArg } from "../../showcase/.fable/fable-library.3.0.0-nagareyama-beta-005/Option.js";
import { Theme__get_FormatFontString } from "../Theme.fs.js";
import { substring } from "../../showcase/.fable/fable-library.3.0.0-nagareyama-beta-005/String.js";

export function Hink_Gui_Hink__Hink_EndWindow(this$) {
    if (!this$.CurrentWindow.Closed) {
        this$.CurrentWindow.Closed = this$.ShouldCloseWindow;
        this$.ShouldCloseWindow = false;
    }
    if (!(Hink__get_CurrentContext(this$) == null)) {
        this$.ApplicationContext.drawImage(Hink__get_CurrentContext(this$).canvas, WindowHandler__get_RealPositionX(this$.CurrentWindow), WindowHandler__get_RealPositionY(this$.CurrentWindow));
    }
    this$.CurrentWindow = (void 0);
}

export function Hink_Gui_Hink__Hink_Window_Z25586FD2(this$, windowInfo, backgroundColor, headerColor) {
    if (this$.CurrentWindow != null) {
        Hink_Gui_Hink__Hink_EndWindow(this$);
    }
    this$.CurrentWindow = windowInfo;
    if (this$.CurrentWindow.Closed) {
        if (this$.CurrentWindow._Context != null) {
            this$.CurrentWindow._Context.clearRect(0, 0, Hink__get_CurrentContext(this$).canvas.width, Hink__get_CurrentContext(this$).canvas.height);
        }
        Hink_Gui_Hink__Hink_EndWindow(this$);
        return false;
    }
    else {
        this$.Cursor.X = 0;
        this$.Cursor.Y = 0;
        this$.Cursor.Width = windowInfo.Width;
        this$.Cursor.Height = windowInfo.Height;
        if (!((Hink__IsHover_77EFFD2E(this$, windowInfo.Height) ? true : windowInfo.ShouldRedraw) ? true : windowInfo.IsDragging)) {
            return false;
        }
        else {
            WindowHandler__EnsureContext(this$.CurrentWindow);
            this$.Cursor.Y = this$.Theme.Window.Header.Height;
            Hink__get_CurrentContext(this$).clearRect(0, 0, Hink__get_CurrentContext(this$).canvas.width, Hink__get_CurrentContext(this$).canvas.height);
            Hink__get_CurrentContext(this$).fillStyle = defaultArg(headerColor, this$.Theme.Window.Header.Color);
            Hink__get_CurrentContext(this$).fillRect(this$.Cursor.X, this$.Cursor.Y - this$.Theme.Window.Header.Height, this$.Cursor.Width, this$.Theme.Window.Header.Height);
            Hink__get_CurrentContext(this$).font = Theme__get_FormatFontString(this$.Theme)(this$.Theme.FontSmallSize);
            const headerTextY = this$.Cursor.Y - (this$.Theme.Window.Header.Height / 2);
            let symbolWidth = 0;
            if (this$.CurrentWindow.Closable) {
                const textSize = Hink__get_CurrentContext(this$).measureText("✕");
                const textX = ((this$.Cursor.X + this$.Cursor.Width) - textSize.width) - this$.Theme.Window.Header.SymbolOffsetX;
                symbolWidth = (textSize.width - (this$.Theme.Window.Header.SymbolOffsetX * 2));
                const hoverX = ((Hink__get_CursorPosX(this$) + this$.Cursor.Width) - textSize.width) - (this$.Theme.Window.Header.SymbolOffsetX * 2);
                const hoverSymbol = (((this$.Mouse.X >= hoverX) ? (this$.Mouse.X < (Hink__get_CursorPosX(this$) + this$.Cursor.Width)) : false) ? (this$.Mouse.Y >= (Hink__get_CursorPosY(this$) - this$.Theme.Window.Header.Height)) : false) ? (this$.Mouse.Y < Hink__get_CursorPosY(this$)) : false;
                if (hoverSymbol) {
                    Hink__get_CurrentContext(this$).fillStyle = this$.Theme.Window.Header.OverSymbolColor;
                    Hink__get_CurrentContext(this$).fillRect(textX - this$.Theme.Window.Header.SymbolOffsetX, this$.Cursor.Y - this$.Theme.Window.Header.Height, textSize.width + (this$.Theme.Window.Header.SymbolOffsetX * 2), headerTextY + this$.Theme.FontSize);
                    if (this$.Mouse.JustReleased) {
                        this$.ShouldCloseWindow = true;
                    }
                }
                Hink__get_CurrentContext(this$).fillStyle = this$.Theme.Text.Color;
                Hink__get_CurrentContext(this$).fillText("✕", textX, headerTextY + this$.Theme.Window.Header.SymbolOffsetY);
            }
            const matchValue = this$.CurrentWindow.Title;
            if (matchValue != null) {
                const title = matchValue;
                const textSize_1 = Hink__get_CurrentContext(this$).measureText(title);
                const maxWidth = ((this$.Cursor.Width - symbolWidth) - this$.Theme.Text.OffsetX) - (this$.Theme.Window.Header.SymbolOffsetX * 2);
                let text;
                if (textSize_1.width > maxWidth) {
                    const charSize = Hink__get_CurrentContext(this$).measureText(" ");
                    let maxChar;
                    const value = (maxWidth - this$.Theme.Text.OffsetX) / charSize.width;
                    maxChar = (~(~value));
                    text = (substring(title, 0, maxChar - 2) + "..");
                }
                else {
                    text = title;
                }
                Hink__get_CurrentContext(this$).fillStyle = this$.Theme.Text.Color;
                Hink__get_CurrentContext(this$).fillText(text, this$.Cursor.X + this$.Theme.Window.Header.SymbolOffsetX, headerTextY + this$.Theme.Text.OffsetY);
            }
            if (this$.CurrentWindow.Draggable) {
                const headerOriginY = Hink__get_CursorPosY(this$) - this$.Theme.Window.Header.Height;
                const hoverHeader = (((this$.Mouse.X >= Hink__get_CursorPosX(this$)) ? (this$.Mouse.X < (Hink__get_CursorPosX(this$) + this$.Cursor.Width)) : false) ? (this$.Mouse.Y >= headerOriginY) : false) ? (this$.Mouse.Y < (headerOriginY + this$.Theme.Window.Header.Height)) : false;
                if (hoverHeader) {
                    if (this$.Mouse.Left) {
                        if (!this$.CurrentWindow.IsDragging) {
                            this$.CurrentWindow.DragXOrigin = this$.Mouse.X;
                            this$.CurrentWindow.DragYOrigin = this$.Mouse.Y;
                        }
                        this$.CurrentWindow.IsDragging = true;
                    }
                }
                if (this$.Mouse.JustReleased ? this$.CurrentWindow.IsDragging : false) {
                    this$.CurrentWindow.X = WindowHandler__get_RealPositionX(this$.CurrentWindow);
                    this$.CurrentWindow.Y = WindowHandler__get_RealPositionY(this$.CurrentWindow);
                    this$.CurrentWindow.IsDragging = false;
                    this$.CurrentWindow.DragXOrigin = 0;
                    this$.CurrentWindow.DragYOrigin = 0;
                }
            }
            Hink__get_CurrentContext(this$).fillStyle = defaultArg(backgroundColor, this$.Theme.Window.Background);
            Hink__get_CurrentContext(this$).fillRect(this$.Cursor.X, this$.Cursor.Y, this$.Cursor.Width, this$.Cursor.Height);
            return true;
        }
    }
}

