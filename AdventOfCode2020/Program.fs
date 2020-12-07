﻿// Learn more about F# at http://fsharp.org

open System
open AdventOfCode2020

let day1Part1() =
    let input = Day1.loadInput()
    let (x,y) = Day1.part1 input
    printfn "Found %d and %d that add up to 2020, multiplied = %d" x y (x*y)
    
let day1Part2() =
    let input = Day1.loadInput()
    let (x,y, z) = Day1.part2 input
    printfn "Found %d, %d and %d that add up to 2020, multiplied = %d" x y z (x*y*z)
    
let day2Part1() =
    let input = Day2.loadInput()
    let validCount = Day2.part1 input
    printfn "Valid password count: %d" validCount

[<EntryPoint>]
let main argv =
    day1Part1()
    day1Part2()
    day2Part1()
    0 // return an integer exit code
