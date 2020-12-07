module AdventOfCode2020.Day2

open System
open System.IO

type PassReq =
    { pass: string
      ch: char
      min: int
      max: int
      original: string }

let loadInput (): string [] = File.ReadAllLines "data/day_2_input.txt"

let part1 (input: string []): int =
    let mutable valid_count = 0
    
    let parse(line: string) : PassReq =
        let leftRight = line.Split ":"
        let pass = leftRight.[1]
        let minMax = (leftRight.[0].Split " ").[0].Split "-"
        let min = minMax.[0]
        let max = minMax.[1]
        let ch = leftRight.[0] |> Seq.last
        
        { PassReq.ch = ch
          pass = pass
          min = min |> Int32.Parse
          max = max |> Int32.Parse
          original = line }
    
    let isValid (pass: PassReq): bool =
        let firstFilled = pass.pass.[pass.min] = pass.ch
        let lastFilled = pass.pass.[pass.max] = pass.ch
        let valid = (firstFilled && lastFilled |> not) || (firstFilled |> not && lastFilled)
        valid
        
    input
        |> Array.map parse
        |> Array.map isValid
        |> Array.filter (fun x -> x = true)
        |> Array.length
