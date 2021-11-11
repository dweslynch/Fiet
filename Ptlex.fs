namespace Fiet

open System
open SixLabors.ImageSharp
open SixLabors.ImageSharp.PixelFormats

open Fiet.Ptdata

module Ptlex =

    let private fromRGB r g b =
        match r, g, b with
            // Black
            | 0uy, 0uy, 0uy -> { Hue = Black; Lightness = Normal; }
            // Reds
            | 0xFFuy, 0xC0uy, 0xC0uy -> { Hue = Red; Lightness = Light; }
            | 0xFFuy, 0uy, 0uy -> { Hue = Red; Lightness = Normal; }
            | 0xC0uy, 0uy, 0uy -> { Hue = Red; Lightness = Dark; }
            // Yellows
            | 0xFFuy, 0xFFuy, 0xC0uy -> { Hue = Yellow; Lightness = Light; }
            | 0xFFuy, 0xFFuy, 0uy -> { Hue = Yellow; Lightness = Normal; }
            | 0xC0uy, 0xC0uy, 0uy -> { Hue = Yellow; Lightness = Dark; }
            // Greens
            | 0xC0uy, 0xFFuy, 0xC0uy -> { Hue = Green; Lightness = Light; }
            | 0uy, 0xFFuy, 0uy -> { Hue = Green; Lightness = Normal; }
            | 0uy, 0xC0uy, 0uy -> { Hue = Green; Lightness = Dark; }
            // Cyans
            | 0xC0uy, 0xFFuy, 0xFFuy -> { Hue = Cyan; Lightness = Light; }
            | 0uy, 0xFFuy, 0xFFuy -> { Hue = Cyan; Lightness = Normal; }
            | 0uy, 0xC0uy, 0xC0uy -> { Hue = Cyan; Lightness = Dark; }
            // Blues
            | 0xC0uy, 0xC0uy, 0xFFuy -> { Hue = Blue; Lightness = Light; }
            | 0uy, 0uy, 0xFFuy -> { Hue = Blue; Lightness = Normal; }
            | 0uy, 0uy, 0xC0uy -> { Hue = Blue; Lightness = Dark; }
            // Magentas
            | 0xFFuy, 0xC0uy, 0xFFuy -> { Hue = Magenta; Lightness = Light; }
            | 0xFFuy, 0uy, 0xFFuy -> { Hue = Magenta; Lightness = Normal; }
            | 0xC0uy, 0uy, 0xC0uy -> { Hue = Magenta; Lightness = Dark; }
            // Black
            | _, _, _ -> { Hue = Black; Lightness = Normal; }

    let private fromPixel (pixel : Rgba32) =
        fromRGB pixel.R pixel.G pixel.B

    let scan (image : Image<Rgba32>) =
        Array2D.init image.Width image.Height (fun x y -> fromPixel image.[x, y])