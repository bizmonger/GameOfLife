module PropertyTests

open FsCheck
open Xunit
open Model

[<Fact>]
let ``cell can't have more than eight neighbors`` () = 
    Check.QuickThrowOnFailure <| fun xy -> xy |> getNeighbors
                                              |> List.length < 9
[<Fact>]
let ``number of cells in grid equals rowcount squared`` () =
    Check.QuickThrowOnFailure <| 
            fun rowCount -> rowCount >= 0 ==> (rowCount |> createGrid
                                                        |> Map.toList
                                                        |> List.length = rowCount * rowCount)