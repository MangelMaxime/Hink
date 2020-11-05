namespace Hink.Inputs

open Fable.Core.JsInterop

open Browser.Types
open Browser.Dom

[<RequireQualifiedAccess>]
module Clipboard =

    module Interop =
        // Code taken from
        // https://stackoverflow.com/questions/400212/how-do-i-copy-to-the-clipboard-in-javascript
        let  copyToClipboard (text : string) = import "default" "./../js/copyToClipboard.js"

    type Record =
        {
            mutable HasBeenInitiated : bool
            mutable Element : HTMLElement
            mutable Content : string
        }

        static member Initial =
            {
                HasBeenInitiated = false
                Element = window.document.body
                Content = ""
            }

    let Manager = Record.Initial

    let init (element : HTMLElement) =

        if not Manager.HasBeenInitiated then
            Manager.Element <- element
            // Attach handler to paste event
            // Don't attach the listener to the provided element because canvas can't have paste event
            document.body.addEventListener("paste",
                fun (ev) ->
                    // TODO: Clean up this code
                    let ev = ev :?> ClipboardEvent
                    if not (isNullOrUndefined ev.clipboardData) then
                        Manager.Content <- ev.clipboardData.getData("text")
                    else if not (isNullOrUndefined window?clipboardData) then
                        // Missing type in the binding
                        Manager.Content <- window?clipboardData?getData$("text")
            )
            // Tag that event listener has been set
            Manager.HasBeenInitiated <- true

    let copyToClipboard (text : string) =
        Interop.copyToClipboard text
        Manager.Element.focus() // Re focus the canvas
