namespace Fiet

open System

module Ptdata =

    type Lightness =
        | Light
        | Normal
        | Dark

    type Hue =
        | Red
        | Yellow
        | Green
        | Cyan
        | Blue
        | Magenta
        | White
        | Black

    type ColorRecord =
        {
            Hue : Hue;
            Lightness : Lightness;
        }
        override this.ToString() =
            match this with
                | { Hue = Black; Lightness = _ } -> "Black"
                | { Hue = White; Lightness = _ } -> "White"
                | { Hue = hue; Lightness = Normal } -> sprintf "%A" hue
                | { Hue = hue; Lightness = lightness } -> sprintf "%A %A" lightness hue

    type ColorBlock =
        {
            Color : ColorRecord;
            Codels : (int * int) array;
        }