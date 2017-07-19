namespace Fable.Import
open System
open System.Text.RegularExpressions
open Fable.Core
open Fable.Import.JS
open Fable.Import.Browser

type [<AllowNullLiteral>] [<Import("*","stats.js")>] Stats() =
    member __.REVISION with get(): float = jsNative and set(v: float): unit = jsNative
    member __.dom with get(): HTMLDivElement = jsNative and set(v: HTMLDivElement): unit = jsNative
    member __.showPanel(value: float): unit = jsNative
    member __.``begin``(): unit = jsNative
    member __.``end``(): float = jsNative
    member __.update(): unit = jsNative
