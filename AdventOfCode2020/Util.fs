module AdventOfCode2020.Util

module Map =
    let containsAllKeys (keys: string list) (map: Map<string, _>): bool =
        keys
        |> List.exists (fun key -> map.ContainsKey key |> not)
        |> not
