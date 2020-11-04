namespace Hink.Inputs

open Fable.Core.JsInterop

[<RequireQualifiedAccess>]
module Clipboard =

    // Code taken from
    // https://stackoverflow.com/questions/400212/how-do-i-copy-to-the-clipboard-in-javascript
    let copyToClipboard (text : string) = import "default" "./../js/copyToClipboard.js"
