#r "../_bin/Fornax.Core.dll"
#load "../siteModel.fsx"
#load "default.fsx"

open Html
open SiteModel

type Model = {
    title : string
    published : System.DateTime
}

let mermaidInit =
    """
mermaid.initialize({startOnLoad:true});
    """

let generate (siteModel : SiteModel) (mdl : Model) (posts : Post list) (content : string) =
    let mermaid =
        div [ ]
            [ link [ Rel "stylesheet"
                     Type "text/css"
                     Href "https://cdn.rawgit.com/knsv/mermaid/7.0.0/dist/mermaid.css" ] [ ]
              script [ Type "text/javascript"
                       Src "https://cdn.rawgit.com/knsv/mermaid/7.0.0/dist/mermaid.min.js" ] [ ]
              script [ Type "text/javascript" ]
                     [ !! mermaidInit ] ]

    Default.defaultPage siteModel mdl.title [mermaid; !! content]
