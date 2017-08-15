#r "../_bin/Fornax.Core.dll"
#load "../siteModel.fsx"
#load "default.fsx"

open Html
open SiteModel

type Model = {
    title : string
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
                     Href "/css/mermaid_default.css" ]
              script [ Type "text/javascript"
                       Src "https://cdn.rawgit.com/knsv/mermaid/7.0.0/dist/mermaid.min.js" ] [ ]
              script [ Type "text/javascript" ]
                     [ !! mermaidInit ] ]
    
    let pageContent =
        div [ ] 
            [ !!content
              mermaid ]

    Default.defaultPage siteModel mdl.title pageContent
