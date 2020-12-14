module AdventOfCode2020.Util

module Map =
    let containsAllKeys (keys: string list) (map: Map<string, _>): bool =
        keys
        |> List.exists (fun key -> map.ContainsKey key |> not)
        |> not

module Option =
    type OptionBuilder() =
        member x.Bind(v,f) = Option.bind f v
        member x.Return v = Some v
        member x.ReturnFrom o = o
        member x.Zero () = None

    let opt = OptionBuilder()