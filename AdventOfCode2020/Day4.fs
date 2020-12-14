module AdventOfCode2020.Day4

open System
open System.IO
open Xunit
open Util

type Height =
    | HeightIn of int
    | HeightCm of int

type Passport ={
    BirthYear : int
    IssueYear: int
    ExpirationYear: int
    Height: Height
    HairColor: string
    EyeColor: string
    PassportId: string
    }

let parseInt str = str |> Int32.Parse

let splitToKv (input: string) : (string * string) =
    let split = input.Split ":"
    split.[0], split.[1]

let loadInput (): string [] =
    let contents = File.ReadAllText "data/day_4_input.txt"
    let chunks = contents.Split("\r\n\r\n")
    let chunks = chunks |> Array.map (fun chunk -> chunk.Replace("\r\n", " "))
    chunks

let chunkIsValid(chunk:string) : bool =
    let mutable map = Map.empty
    let tokens = chunk.Replace("\r\n", " ").Split(" ")
    for token in tokens do
        let (key,value) = splitToKv token
        map <- map.Add (key, value)
    map |> Map.containsAllKeys ["byr"; "iyr"; "eyr"; "hgt"; "hcl"; "ecl"; "pid"]

let chunkIsValid2(chunk: string) : bool =
    let validYear min max year =
        year <= max && year >= min
    
    let validHeight (height:string) : bool =
        let heightStr = height |> String.filter Char.IsDigit
        let heightInt = Int32.Parse heightStr
        let unitOfMeasure = height |> String.filter Char.IsLetter
        match unitOfMeasure with
        | "cm" -> heightInt >= 150 && heightInt <= 193
        | "in" -> heightInt >= 59 && heightInt <= 76
        | _ -> false
    
    let validHairColor (color: string) : bool =
        if color.[0] <> '#' then false else
        if color.Length <> 7 then false else
        let validChars = ['0'..'9'] @ ['a'..'f']
        let filtered =
            color.[1..]
            |> String.filter (fun x -> List.contains x validChars)
        filtered.Length = 6
    
    let validEyeColor color =
        match color with
        | "amb" | "blu" | "brn" | "gry" | "grn" | "hzl" | "oth" -> true
        | _ -> false
    
    let validPassportId (code: string ) : bool =
        let onlyDigits = code |> String.filter Char.IsDigit
        onlyDigits.Length = 9

    let mutable map = Map.empty
    let tokens = chunk.Replace("\r\n", " ").Split(" ")
    for token in tokens do
        let (key,value) = splitToKv token
        map <- map.Add (key, value)
    if map |> Map.containsAllKeys ["byr"; "iyr"; "eyr"; "hgt"; "hcl"; "ecl"; "pid"] |> not then false else
    
    let validByr = validYear 1920 2002 (map.["byr"] |> parseInt)
    let validIyr = validYear 2010 2020 (map.["iyr"] |> parseInt)
    let validEyr = validYear 2020 2030 (map.["eyr"] |> parseInt)
    let validHgt = validHeight map.["hgt"]
    let validHcl = validHairColor map.["hcl"]
    let validEcl = validEyeColor map.["ecl"]
    let validPid = validPassportId map.["pid"]
    validByr && validIyr && validEyr && validHgt && validHcl && validEcl && validPid

let tryGetPassport (chunk: string) : Passport option =
    let validYear min max year =
        match year <= max && year >= min with
        | true -> Some year
        | _ -> None
    
    let validHeight (height:string) : Height option =
        let heightStr = height |> String.filter Char.IsDigit
        let heightInt = Int32.Parse heightStr
        let unitOfMeasure = height |> String.filter Char.IsLetter
        match unitOfMeasure with
        | "cm" when heightInt >= 150 && heightInt <= 193-> (HeightCm heightInt) |> Some
        | "in" when heightInt >= 59 && heightInt <= 76 -> (HeightIn heightInt) |> Some
        | _ -> None
    
    let validHairColor (color: string) : string option =
        if color.[0] <> '#' then None else
        if color.Length <> 7 then None else
        let validChars = ['0'..'9'] @ ['a'..'f']
        let filtered =
            color.[1..]
            |> String.filter (fun x -> List.contains x validChars)
        if filtered.Length = 6 then Some color else None
    
    let validEyeColor color : string option =
        match color with
        | "amb" | "blu" | "brn" | "gry" | "grn" | "hzl" | "oth" -> Some color
        | _ -> None
    
    let validPassportId (code: string ) : string option =
        let onlyDigits = code |> String.filter Char.IsDigit
        if onlyDigits.Length = 9 then Some code else None
    
    let mutable map = Map.empty
    let tokens = chunk.Replace("\r\n", " ").Split(" ")
    for token in tokens do
        let (key,value) = splitToKv token
        map <- map.Add (key, value)
    if map |> Map.containsAllKeys ["byr"; "iyr"; "eyr"; "hgt"; "hcl"; "ecl"; "pid"] |> not then None else
    
    let passport =
        Option.opt {
            let! validByr = validYear 1920 2002 (map.["byr"] |> parseInt)
            let! validIyr = validYear 2010 2020 (map.["iyr"] |> parseInt)
            let! validEyr = validYear 2020 2030 (map.["eyr"] |> parseInt)
            let! validHgt = validHeight map.["hgt"]
            let! validHcl = validHairColor map.["hcl"]
            let! validEcl = validEyeColor map.["ecl"]
            let! validPid = validPassportId map.["pid"]
            return { Passport.Height = validHgt
                     BirthYear = validByr
                     IssueYear = validIyr
                     ExpirationYear = validEyr
                     HairColor = validHcl
                     EyeColor = validEcl
                     PassportId = validPid }
        }
    
    passport

let part1 input =
    input
    |> Array.filter chunkIsValid
    |> Array.length

let part2 input =
    input
    |> Array.filter chunkIsValid2
    |> Array.length

let part2_option input =
    input
    |> Array.choose tryGetPassport
    |> Array.length

module Tests =
    [<Fact>]
    let ``Valid with all fields present``() =
        let result = chunkIsValid "ecl:gry pid:860033327 eyr:2020 hcl:#fffffd byr:1937 iyr:2017 cid:147 hgt:183cm"
        Assert.True(result)
    
    [<Fact>]
    let ``Invalid because missing hgt field``() =
        let result = chunkIsValid "iyr:2013 ecl:amb cid:350 eyr:2023 pid:028048884 hcl:#cfa07d byr:1929"
        Assert.False(result)
        
    [<Fact>]
    let ``Valid because only missing cid``() =
        let result = chunkIsValid "hcl:#ae17e1 iyr:2013 eyr:2024 ecl:brn pid:760753108 byr:1931 hgt:179cm"
        Assert.True(result)
        
    [<Fact>]
    let ``Invalid because missing 2 required fields: cid and byr``() =
        let result = chunkIsValid "hcl:#cfa07d eyr:2025 pid:166559648 iyr:2011 ecl:brn hgt:59in"
        Assert.False(result)        
        
    [<Theory>]
    [<InlineData("eyr:1972 cid:100 hcl:#18171d ecl:amb hgt:170 pid:186cm iyr:2018 byr:1926")>]
    [<InlineData("iyr:2019 hcl:#602927 eyr:1967 hgt:170cm ecl:grn pid:012533040 byr:1946")>]
    [<InlineData("hcl:dab227 iyr:2012 ecl:brn hgt:182cm pid:021572410 eyr:2020 byr:1992 cid:277")>]
    [<InlineData("hgt:59cm ecl:zzz eyr:2038 hcl:74454a iyr:2023 pid:3556412378 byr:2007")>]
    let ``Part 2 invalid``(input: string) =
        let result = chunkIsValid2 input
        Assert.False(result)
        
    [<Theory>]
    [<InlineData("pid:087499704 hgt:74in ecl:grn iyr:2012 eyr:2030 byr:1980 hcl:#623a2f")>]
    [<InlineData("eyr:2029 ecl:blu cid:129 byr:1989 iyr:2014 pid:896056539 hcl:#a97842 hgt:165cm")>]
    [<InlineData("hcl:#888785 hgt:164cm byr:2001 iyr:2015 cid:88 pid:545766238 ecl:hzl eyr:2022")>]
    [<InlineData("iyr:2010 hgt:158cm hcl:#b6652a ecl:blu byr:1944 eyr:2021 pid:093154719")>]
    let ``Part 2 valid``(input: string) =
        let result = chunkIsValid2 input
        Assert.True(result)
    
    [<Theory>]
    [<InlineData("eyr:1972 cid:100 hcl:#18171d ecl:amb hgt:170 pid:186cm iyr:2018 byr:1926")>]
    [<InlineData("iyr:2019 hcl:#602927 eyr:1967 hgt:170cm ecl:grn pid:012533040 byr:1946")>]
    [<InlineData("hcl:dab227 iyr:2012 ecl:brn hgt:182cm pid:021572410 eyr:2020 byr:1992 cid:277")>]
    [<InlineData("hgt:59cm ecl:zzz eyr:2038 hcl:74454a iyr:2023 pid:3556412378 byr:2007")>]
    let ``Part 2 invalid option``(input: string) =
        let result = tryGetPassport input
        Assert.True(result.IsNone)
        
    [<Theory>]
    [<InlineData("pid:087499704 hgt:74in ecl:grn iyr:2012 eyr:2030 byr:1980 hcl:#623a2f")>]
    [<InlineData("eyr:2029 ecl:blu cid:129 byr:1989 iyr:2014 pid:896056539 hcl:#a97842 hgt:165cm")>]
    [<InlineData("hcl:#888785 hgt:164cm byr:2001 iyr:2015 cid:88 pid:545766238 ecl:hzl eyr:2022")>]
    [<InlineData("iyr:2010 hgt:158cm hcl:#b6652a ecl:blu byr:1944 eyr:2021 pid:093154719")>]
    let ``Part 2 valid option``(input: string) =
        let result = tryGetPassport input
        Assert.True(result.IsSome)