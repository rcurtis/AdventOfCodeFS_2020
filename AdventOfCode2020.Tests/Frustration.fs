module AdventOfCode2020.Tests.Frustration

open AdventOfCode2020
open Xunit
open Util

[<Fact>]
let ``Map contains all keys``() =
    let map = Map.empty.Add("A", "AA").Add("B", "BB").Add("C", "CC")
    let keys = ["A"; "B"; "C"]
    let containsAll = Map.containsAllKeys keys map
    Assert.True(containsAll)
    
[<Fact>]
let ``Map missing key``() =
    let map = Map.empty.Add("A", "AA").Add("B", "BB").Add("D", "CC")
    let keys = ["A"; "B"; "C"]
    let containsAll = Map.containsAllKeys keys map
    Assert.False(containsAll)