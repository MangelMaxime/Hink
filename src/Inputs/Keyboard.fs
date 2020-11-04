namespace Hink.Inputs

open Browser.Types

[<RequireQualifiedAccess>]
module Keyboard =
    // We use module + type + AutoOpen to not polluate the Keyboard module with all keys definitions
    [<AutoOpen>]
    module KeyMod =
        type KeyMod =
            | Alt
            | Control
            | Shift
            | NoMod

    [<AutoOpen>]
    module Keys =
        type Keys =
            | Unkown of string
            | Dead
            | Ampersand
            | Backspace
            | Enter
            | Control
            | Escape
            | Tab
            | Space
            | End
            | Home
            | ArrowLeft
            | ArrowUp
            | ArrowRight
            | ArrowDown
            | Delete
            | OS
            | F1
            | F2
            | F3
            | F4
            | F5
            | F6
            | F7
            | F8
            | F9
            | F10
            | F11
            | F12
            | A
            | B
            | C
            | D
            | E
            | F
            | G
            | H
            | I
            | J
            | K
            | L
            | M
            | N
            | O
            | P
            | Q
            | R
            | S
            | T
            | U
            | V
            | W
            | X
            | Y
            | Z
            | SingleQuote
            | Slash

    let resolveKeyFromKey (key: string) =
        match key.ToLower() with
        | "&" -> Keys.Ampersand
        | "backspace" -> Keys.Backspace
        | "tab" -> Keys.Tab
        | "enter" -> Keys.Enter
        | "control" -> Keys.Control
        | "escape" -> Keys.Escape
        | "space" -> Keys.Space
        | "end" -> Keys.End
        | "home" -> Keys.Home
        | "arrowleft" -> Keys.ArrowLeft
        | "arrowup" -> Keys.ArrowUp
        | "arrowright" -> Keys.ArrowRight
        | "arrowdown" -> Keys.ArrowDown
        | "delete" -> Keys.Delete
        | "os" -> Keys.OS
        | "f1" -> Keys.F1
        | "f2" -> Keys.F2
        | "f3" -> Keys.F3
        | "f4" -> Keys.F4
        | "f5" -> Keys.F5
        | "f6" -> Keys.F6
        | "f7" -> Keys.F7
        | "f8" -> Keys.F8
        | "f9" -> Keys.F9
        | "f10" -> Keys.F10
        | "f11" -> Keys.F11
        | "f12" -> Keys.F12
        | "/" -> Keys.Slash
        | "'" -> Keys.SingleQuote
        | "a" -> Keys.A
        | "b" -> Keys.B
        | "c" -> Keys.C
        | "d" -> Keys.D
        | "e" -> Keys.E
        | "f" -> Keys.F
        | "g" -> Keys.G
        | "h" -> Keys.H
        | "i" -> Keys.I
        | "j" -> Keys.J
        | "k" -> Keys.K
        | "l" -> Keys.L
        | "m" -> Keys.M
        | "n" -> Keys.N
        | "o" -> Keys.O
        | "p" -> Keys.P
        | "q" -> Keys.Q
        | "r" -> Keys.R
        | "s" -> Keys.S
        | "t" -> Keys.T
        | "u" -> Keys.U
        | "v" -> Keys.V
        | "w" -> Keys.W
        | "x" -> Keys.X
        | "y" -> Keys.Y
        | "z" -> Keys.Z
        | _ -> Keys.Unkown key

    type Modifiers =
        {   mutable Shift : bool
            mutable Control : bool
            mutable CommandLeft : bool
            mutable CommandRight : bool
            mutable Alt : bool }
        static member Initial =
            {   Shift = false
                Control = false
                CommandLeft = false
                CommandRight = false
                Alt = false }

    type Record =
        {   mutable KeysPressed : Set<Keys>
            mutable LastKeyValue : string
            mutable LastKeyIsPrintable : bool
            mutable LastKey : Keys
            Modifiers : Modifiers }

        member this.HasNewKeyStroke () =
            this.LastKeyValue <> ""

        static member Initial =
            {   KeysPressed = Set.empty
                LastKeyValue = ""
                LastKeyIsPrintable = false
                LastKey = Keys.Unkown ""
                Modifiers = Modifiers.Initial }

        member self.IsPress key = self.KeysPressed.Contains(key)
        member self.ClearLastKey() =
            self.LastKeyValue <- ""
            self.LastKeyIsPrintable <- false
            self.LastKey <- Keys.Unkown ""

    let Manager = Record.Initial

    type PreventHandler = (KeyboardEvent -> bool)

    let init (element : HTMLElement) preventDefault (userPreventHandler : PreventHandler option) =
        let updateModifiers (e : KeyboardEvent) =
            Manager.Modifiers.Alt <- e.altKey
            Manager.Modifiers.Shift <- e.shiftKey
            Manager.Modifiers.Control <- e.ctrlKey
            Manager.Modifiers.CommandLeft <- e.keyCode = 224.
            Manager.Modifiers.CommandRight <- e.keyCode = 224.
        element.addEventListener( "keydown",
            fun e ->
                let e = e :?> KeyboardEvent
                let key = resolveKeyFromKey e.key
                Manager.LastKeyValue <- e.key
                // Here we try to determine if the key is printable or not
                // Should not be "Dead". Exemple first press on '^' is Dead
                // And the value should be of size [1,2] because we can add:
                // * One character at a time. Example: 'a', '!', '§'
                // * Two characters at a time. Example '^^', '^p'
                // Second case occured when pressing some keys in sequence.
                // Example:
                // * '^^' = '^' + '^'
                // * '^p' = '^' + 'p'
                // We also have to make sure the key is not F1..F12 so we exclude keycode range: [112,123]
                let isPrintableKey =
                    match key with
                    | Keys.F1 | Keys.F2 | Keys.F3 | Keys.F4 | Keys.F5 | Keys.F6
                    | Keys.F7 | Keys.F8 | Keys.F9 | Keys.F10 | Keys.F11 | Keys.F12
                    | Keys.OS -> false
                    | _ -> true

                Manager.LastKeyIsPrintable <- 1 <= e.key.Length && e.key.Length <= 2 && isPrintableKey
                Manager.LastKey <- key
                Manager.KeysPressed <- Set.add key Manager.KeysPressed
                // Update the Modifiers state
                updateModifiers e
            )
        element.addEventListener("keyup",
            fun e ->
                let e = e :?> KeyboardEvent
                let code = int e.keyCode
                Manager.KeysPressed <- Set.remove (resolveKeyFromKey e.key) Manager.KeysPressed
                // Update the Modifiers state
                updateModifiers e
                ()
            )
        // If the user ask to prevent tab unloosing focus
        if preventDefault then
            element.addEventListener("keydown", fun e ->
                let e = e :?> KeyboardEvent
                let shouldPreventFromCtrl =
                    if e.ctrlKey then
                        match resolveKeyFromKey e.key with
                        | Keys.A -> true
                        | _ -> false
                    else false

                let shouldPreventFromShift =
                    if e.shiftKey then
                        match resolveKeyFromKey e.key with
                        | Keys.Slash -> true
                        | _ -> false
                    else false

                let shouldPreventNoModifier =
                    match resolveKeyFromKey e.key with
                    | Keys.Tab -> true
                    | Keys.Backspace -> true
                    | Keys.SingleQuote -> true
                    | _ -> false

                if shouldPreventNoModifier || shouldPreventFromCtrl || shouldPreventFromShift then
                    e.preventDefault()
                    unbox false
                else ())

        match userPreventHandler with
        | Some handler ->
            element.addEventListener("keydown",
                fun e ->
                    let e = e :?> KeyboardEvent
                    if handler e then
                        e.preventDefault()
                        unbox false
                    else
                        () )
        | None -> () // Nothing to do
