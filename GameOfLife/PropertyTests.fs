module PropertyTests

open FsCheck
open Xunit
open Model

[<Fact>]
let ``cell can't have more than eight neighbors`` () = 
    Check.QuickThrowOnFailure <| fun xy -> xy |> getNeighbors
                                              |> List.length < 9