namespace Fiet

open System

module Data =

    type Queue<'T>() =

        member val private list : 'T list = [] with get, set
        
        member this.Size with get() = List.length this.list
        member this.Enqueue item = this.list <- item::this.list
        member this.Dequeue () =
            match this.list with
                | [] -> raise (InvalidOperationException "Cannot perform dequeue operation on an empty queue")
                | hd::tl ->
                    this.list <- tl
                    hd
        member this.Peek () =
            match this.list with
                | [] -> raise (InvalidOperationException "Cannot perform peek operation on an empty queue")
                | hd::tl -> hd