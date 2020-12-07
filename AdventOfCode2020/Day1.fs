module AdventOfCode2020.Day1

open System
open System.IO

let loadInput() : int list =
    let contents = File.ReadAllLines "data/day_1_input.txt"
    contents
    |> Array.map (fun line -> Int32.Parse line)
    |> List.ofArray
    

let part1 (input: int list) : (int * int) =
    let mutable found = (0,0)
    for x in input do
        for y in input do
            if x + y = 2020 then found <- (x, y)
    found

let part2 (input: int list) : (int*int*int) =
    let mutable found = (0,0,0)
    for x in input do
        for y in input do
            for z in input do 
                if x + y + z = 2020 then found <- (x, y, z)
    found
