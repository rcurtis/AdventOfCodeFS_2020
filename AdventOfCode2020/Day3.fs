module AdventOfCode2020.Day3

open System
open System.IO

let loadInput (): string [] = File.ReadAllLines "data/day_3_input.txt"

let walkTreeCount(input : string[])(x_step:int)(y_step:int) : uint64 =
    let width = input.[0].Length
    let mutable x = 0
    let mutable y = 0
    let mutable treeCount : uint64 = 0UL
    while y < input.Length do
        let row = input.[y]
        let tile = row.[x]
        
        if tile = '#' then do
            treeCount <- treeCount + 1UL
        
        x <- (x + x_step) % width
        y <- y + y_step
    treeCount
    
let part1 (input: string[]) =
    walkTreeCount input 3 1 

let part2(input: string[]) : uint64 =
    walkTreeCount input 1 1 *
        walkTreeCount input 3 1 *
        walkTreeCount input 5 1 *
        walkTreeCount input 7 1 *
        walkTreeCount input 1 2
