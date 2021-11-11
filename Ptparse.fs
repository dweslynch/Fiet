namespace Fiet

open System
open System.Collections.Generic

open Fiet.Ptdata

module Ptparse =

    // Determines whether two coordinates are adjacent
    let private adjacent (pixel : int * int) (other : int * int) =
        let x, y = pixel
        let w, z = other

        if x = w && Math.Abs (y - z) = 1 then
            true
        elif y = z && Math.Abs (x - w) = 1 then
            true
        else false

    let private find_block pixel width height color (scan : ColorRecord[,]) =
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


    (*
    let private find_block pixel pixels color scan =
        let rec find pixel (pixels : List<int * int>) color (scan : ColorRecord[,]) codels =
            let adj =
                pixels |> Seq.filter (adjacent pixel)
                |> Seq.filter (fun (x, y) -> scan.[x, y] = color)
                |> Seq.filter (fun codel -> not (List.contains codel codels))
            List.ofSeq adj |> List.fold (fun acc codel -> List.append acc (find codel pixels color scan acc)) codels
        pixel::(find pixel pixels color scan [])
    *)
            

    (*
    let rec private find_block pixel (pixels : List<int * int>) color (scan : ColorRecord[,]) =
        let block = new List<int * int> ()
        for codel in pixels |> Seq.filter (adjacent pixel) do
            let x, y = codel
            if scan.[x, y] = color then
                pixels.Remove codel |> ignore
                block.Add codel
                for item in find_block codel pixels color scan do
                    pixels.Remove item |> ignore
                    block.Add item
        block
    *)

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
            let codels = find_block pixels.[0] width height color scan
            
            for codel in codels do
                pixels.Remove codel |> ignore

            blocks.Add (blockidx, { Color = color; Codels = codels; })
            blockidx <- blockidx + 1
        blocks

    let block_at x y (blocks : Dictionary<int, ColorBlock>) =
        let rec search i =
            if i >= blocks.Count then
                -1
            elif blocks.[i].Codels |> Array.contains (x, y) then
                i
            else
                i + 1 |> search
        search 0