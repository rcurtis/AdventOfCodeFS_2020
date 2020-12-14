module AdventOfCode2020.Day5

open System
open System.IO
open Xunit

type Seat = { Row: int; Col: int }

type SeatWithId = { Row: int; Col: int; Id: int }

let COL_COUNT = 8
let ROW_COUNT = 128

let loadInput (): string [] = File.ReadAllLines "data/day_5_input.txt"

let find_row (input: string): int =
    let mutable (low, high) = (0, ROW_COUNT - 1)

    input
    |> String.iter (fun ch ->
        let step = ((float) high - (float) low) / 2.0

        match ch with
        | 'F' -> high <- low + (Math.Floor(step) |> int)
        | 'B' -> low <- low + (Math.Ceiling(step) |> int)
        | _ -> failwith "Doh!")

    low

let find_col (input: string): int =
    let mutable (left, right) = (0, COL_COUNT - 1)

    input
    |> String.iter (fun ch ->
        let step = ((float) right - (float) left) / 2.0

        match ch with
        | 'L' -> right <- left + (Math.Floor(step) |> int)
        | 'R' -> left <- left + (Math.Ceiling(step) |> int)
        | _ -> failwith "Doh!")

    left

let find_seat (input: string): Seat =
    { Seat.Row = find_row input.[0..6]
      Seat.Col = find_col input.[7..] }

let getSeatId (seat: Seat) = seat.Row * 8 + seat.Col

let getSeatWithId (seat: Seat) =
    { SeatWithId.Col = seat.Col
      Row = seat.Row
      Id = getSeatId seat }

module Test =
    [<Fact>]
    let ``Find seat`` () =
        let seat = find_seat "FBFBBFFRLR"
        let expected = { Seat.Row = 44; Col = 5 }
        Assert.Equal(expected, seat)

    [<Fact>]
    let ``Find seatId from seat`` () =
        let id = find_seat "FBFBBFFRLR" |> getSeatId
        Assert.Equal(357, id)
    
    [<Fact>]
    let ``getSeatWithId `` () =
        let seatWithId = find_seat "FBFBBFFRLR" |> getSeatWithId
        Assert.Equal({Row=44; Col=5; Id=357}, seatWithId)
    
    [<Theory>]
    [<InlineData("BFFFBBFRRR", 70, 7, 567)>]
    [<InlineData("FFFBBBFRRR", 14, 7, 119)>]
    [<InlineData("BBFFBBFRLL", 102, 4, 820)>]
    let ``Valid data`` input row col id =
        let seatWithId = find_seat input |> getSeatWithId
        Assert.Equal({Row=row; Col=col; Id=id}, seatWithId)
