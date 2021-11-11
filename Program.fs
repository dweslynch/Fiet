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
            | [| "parse"; filename |] ->
                if File.Exists filename then
                    printfn "Scanning %s" filename
                    let image = Image.Load<Rgba32> filename
                    let lexed = Ptlex.scan image
                    printfn "Scanned %s" filename
                    printfn "Parsing %s" filename
                    let blocks = Ptparse.build_blocks lexed
                    printfn "Parsed %s" filename
                    for y = 0 to Array2D.length2 lexed - 1 do
                        for x = 0 to Array2D.length1 lexed - 1 do
                            printf "%3i " (Ptparse.block_at x y blocks)
                        Console.Write Environment.NewLine
                else
                    printfn "File %s does not exist" filename

            | _ ->
                printfn "Usage: fiet [command] [filename]"
                printfn "  help:  display this message"
                printfn "  lex [filename]:  perform lexical scanning on a file"
        0 // return an integer exit code
