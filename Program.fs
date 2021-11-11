// Learn more about F# at http://docs.microsoft.com/dotnet/fsharp
namespace Fiet

open System
open System.IO
open SixLabors.ImageSharp
open SixLabors.ImageSharp.PixelFormats

module Program =

    // Define a function to construct a message to print
    let from whom =
        sprintf "from %s" whom

    [<EntryPoint>]
    let main argv =

        match argv with
            | [| "lex"; filename |] ->
                if File.Exists filename then
                    printfn "Scanning %s" filename
                    let image = Image.Load<Rgba32> filename
                    let lexed = Ptlex.scan image
                    printfn "Scanned %s" filename
                else
                    printfn "File %s does not exist" filename
            | _ ->
                printfn "Usage: fiet [command] [filename]"
                printfn "  help:  display this message"
                printfn "  lex [filename]:  perform lexical scanning on a file"
        0 // return an integer exit code
