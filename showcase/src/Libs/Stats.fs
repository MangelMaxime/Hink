namespace Fable.Import

open Fable.Core
open Browser.Types

type [<AllowNullLiteral>] [<ImportAll("../stats.js")>] Stats() =
    member __.REVISION with get(): float = jsNative and set(v: float): unit = jsNative
    member __.dom with get(): HTMLDivElement = jsNative and set(v: HTMLDivElement): unit = jsNative
    member __.showPanel(value: float): unit = jsNative
    member __.``begin``(): unit = jsNative
    member __.``end``(): float = jsNative
    member __.update(): unit = jsNative
