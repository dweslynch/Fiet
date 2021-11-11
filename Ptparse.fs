namespace Fiet

open System
open System.Collections.Generic

open Fiet.Ptdata

module Ptparse =

    // Assemble a color block around a pixel
    let find_block pixel scan =
        let width = Array2D.length1 scan
        let height = Array2D.length2 scan
        let color = scan.[fst pixel, snd pixel]
        let block = new List<int * int>()
        block.Add pixel
        let queue = new Data.Queue<int * int> ()
        queue.Enqueue pixel

        while queue.Size > 0 do
            let item = queue.Dequeue()
            block.Add item

            let x, y = item
            let candidates =
                // Possible adjacent values
                [(x - 1, y); (x + 1, y); (x, y - 1); (x, y + 1)]
                // Within bounds
                |> List.filter (fun (_x, _y) -> _x >= 0 && _x < width && _y >= 0 && _y < height)
                // Of the same color
                |> List.filter (fun (_x, _y) -> scan.[_x, _y] = color)
                // That haven't already been processed
                |> List.filter (fun codel -> not (block.Contains codel))
            
            block.AddRange candidates
            List.iter (fun codel -> queue.Enqueue codel) candidates
        Array.ofSeq block

    // Assemble a dictionary of color blocks
    let build_blocks scan =
        let blocks = Dictionary<int, ColorBlock> ()
        let width = Array2D.length1 scan
        let height = Array2D.length2 scan

        // List of all possible pixel coordinates
        let pixels = List<int * int> [for x = 0 to width - 1 do yield! [for y = 0 to height - 1 do yield x, y]]
        let mutable blockidx = 0

        while pixels.Count > 0 do
            //printfn "  %i codels remaining" pixels.Count
            let x, y = pixels.[0]
            let color = scan.[x, y]
            let codels = find_block pixels.[0] scan
            
            for codel in codels do
                pixels.Remove codel |> ignore

            blocks.Add (blockidx, { Color = color; Codels = codels; })
            blockidx <- blockidx + 1
        blocks

    // Determine the ordinal block occupied by a given codel
    let block_at x y (blocks : Dictionary<int, ColorBlock>) =
        let rec search i =
            if i >= blocks.Count then
                -1
            elif blocks.[i].Codels |> Array.contains (x, y) then
                i
            else
                i + 1 |> search
        search 0