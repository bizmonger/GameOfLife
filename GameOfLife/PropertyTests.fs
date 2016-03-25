module PropertyTests

open FsCheck
open FsCheck.Xunit
open Model

[<Xunit.Fact>]
let ``cell can't have more than eight neighbors`` () = 
    Check.QuickThrowOnFailure <| fun xy -> xy |> getNeighbors
                                              |> List.length < 9
[<Property(QuietOnSuccess = true)>]
let ``number of cells in grid equals rowcount squared`` () =
    let values = Arb.generate<int> |> Gen.map (fun v -> v > 0) 
                                   |> Arb.fromGen

    Prop.forAll values <| fun number ->
        
        // Setup
        let rowCount = 3

        // Test
        let actual = rowCount |> createGrid
                              |> Seq.length

        // Verify
        let expected = rowCount * rowCount
        actual = expected