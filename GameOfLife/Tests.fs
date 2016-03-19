module Tests

open FsUnit
open NUnit.Framework
open Model

[<Test>]
let ``cells sharing x-coordinate are neighbors``() =
   // Setup
   let cell1 = { X=0; Y=0; State=Dead }
   let cell2 = { X=0; Y=1 ; State=Dead }

   // Verify
   cell1 |> isNeighbor cell2 
         |> should equal true

[<Test>]
let ``cells sharing y-coordinate are neighbors``() =
   // Setup
   let cell1 = { X=0; Y=1; State=Dead }
   let cell2 = { X=1; Y=1; State=Dead }

   cell1 |> isNeighbor cell2 
         |> should equal true

[<Test>]
let ``cell that's right 1 and down 1 is neighbor``() =
   // Setup
   let cell1 = { X=0; Y=0; State=Dead }
   let cell2 = { X=1; Y=(-1); State=Dead }

   // Verify
   cell1 |> isNeighbor cell2 
         |> should equal true

[<Test>]
let ``cell that's right 1 and down-0 is neighbor``() =
   // Setup
   let cell1 = { X=0; Y=0; State=Dead }
   let cell2 = { X=1; Y=0; State=Dead }

   // Verify
   cell1 |> isNeighbor cell2 
         |> should equal true

[<Test>]
let ``cell that's right 1 and up 1 is neighbor``() =
   // Setup
   let cell1 = { X=0; Y=0; State=Dead }
   let cell2 = { X=1; Y=1 ; State=Dead }

   // Verify
   cell1 |> isNeighbor cell2 
         |> should equal true

[<Test>]
let ``cell that's up 1 is neighbor``() =
   // Setup
   let cell1 = { X=0; Y=0; State=Dead }
   let cell2 = { X=0; Y=1 ; State=Dead }

   // Verify
   cell1 |> isNeighbor cell2 
         |> should equal true

[<Test>]
let ``cell that's down 1 is neighbor``() =
   // Setup
   let cell1 = { X=0; Y=0;    State=Dead }
   let cell2 = { X=0; Y=(-1); State=Dead }

   // Verify
   cell1 |> isNeighbor cell2 
         |> should equal true

[<Test>]
let ``cell that's left 1 and up 1 is neighbor``() =
   // Setup
   let cell1 = { X=0; Y=0; State=Dead }
   let cell2 = { X=(-1); Y=1 ; State=Dead }

   // Verify
   cell1 |> isNeighbor cell2 
         |> should equal true

[<Test>]
let ``cell that's left 1 and down 1 is neighbor``() =
   // Setup
   let cell1 = { X=0;    Y=0;    State=Dead }
   let cell2 = { X=(-1); Y=(-1); State=Dead }

   // Verify
   cell1 |> isNeighbor cell2 
         |> should equal true

[<Test>]
let ``far away x-coordinates are not neighbors``() =
   // Setup
   let cell1 = { X=(-1); Y=0; State=Dead }
   let cell2 = { X=(+1); Y=0; State=Dead }

   // Verify
   cell1 |> isNeighbor cell2 
         |> should equal false

[<Test>]
let ``far away y-coordinates are not neighbors``() =
   // Setup
   let cell1 = { X=0; Y=(+1); State=Dead }
   let cell2 = { X=0; Y=(-1); State=Dead }

   // Verify
   cell1 |> isNeighbor cell2 
         |> should equal false

[<Test>]
let ``far away x,y-coordinates are not neighbors``() =
   // Setup
   let cell1 = { X=(+1); Y=(+1); State=Dead }
   let cell2 = { X=(+1); Y=(-1); State=Dead }

   // Verify
   cell1 |> isNeighbor cell2 
         |> should equal false

[<Test>]
let ``cells with same coordinates cannot be neighbors``() =
   // Setup
   let cell1 = { X=0; Y=0; State=Dead }
   let cell2 = { X=0; Y=0; State=Dead }

   // Verify
   cell1 |> isNeighbor cell2 
         |> should equal false

[<Test>]
let ``create grid``() =
    // Test
    let rowCount = 3
    let grid = rowCount |> createGrid
    
    // Verify
    grid.Count |> should equal 9

[<Test>]
let ``find center``() =
    // Setup
    let rowCount = 3
    let grid = rowCount |> createGrid

    let getCoordinate coordinate =
        match grid.TryFind coordinate with
        | Some coordinate -> true
        | None            -> false

    // Test
    let found = getCoordinate (1,1)

    // Verify
    found |> should equal true

[<Test>]
let ``get status``() =
    // Setup
    let rowCount = 3
    let grid = rowCount |> createGrid

    // Test
    let center = grid |> getStatus (1,1)

    // Verify
    center |> should equal Dead

[<Test>]
let ``set cell to alive``() =
    // Setup
    let rowCount = 3
    let target = { X=1; Y=1; State=Alive }

    let grid = rowCount |> createGrid
                        |> setCell target
    // Test
    let result = grid |> getStatus (1,1)

    // Verify
    result |> should equal Alive

[<Test>]
let ``get neighbors``() =
    // Setup
    let rowCount = 3
    let grid = rowCount |> createGrid

    let center = 2,2
    let count = center |> getNeighbors
                       |> List.length
    // Verify
    count |> should equal 8

[<Test>]
let ``Any live cell with fewer than two live neighbors dies, as if caused by under-population``() =
    // Setup
    let rowCount = 3
    let grid = rowCount |> createGrid
                        |> setCell { X=1; Y=1; State=Alive }
                        |> setReaction (1,1)
    // Verify
    grid |> getStatus (1,1) |> should equal Dead

[<Test>]
let ``Any live cell with two or three live neighbours lives on to the next generation``() =
    // Setup
    let rowCount = 3
    let grid = rowCount |> createGrid
                        |> setCell { X=0; Y=0; State=Alive }
                        |> setCell { X=1; Y=1; State=Alive }
                        |> setCell { X=2; Y=1; State=Alive }
                        |> setReaction (1,1)
    // Verify
    grid |> getStatus (1,1) |> should equal Alive

[<Test>]
let ``Any live cell with more than three live neighbours dies, as if by over-population``() =
    // Setup
    let rowCount = 3
    let grid = rowCount |> createGrid
                        |> setCell { X=0; Y=0; State=Alive }
                        |> setCell { X=1; Y=1; State=Alive }
                        |> setCell { X=0; Y=1; State=Alive }
                        |> setCell { X=1; Y=0; State=Alive }
                        |> setCell { X=2; Y=1; State=Alive }
                        |> setReaction (1,1)
    // Verify
    grid |> getStatus (1,1) |> should equal Dead

[<Test>]
let ``Any dead cell with exactly three live neighbours becomes a live cell, as if by reproduction``() =
    // Setup
    let rowCount = 3
    let grid = rowCount |> createGrid
                        |> setCell { X=0; Y=1; State=Alive }
                        |> setCell { X=1; Y=1; State=Dead }
                        |> setCell { X=0; Y=2; State=Alive }
                        |> setCell { X=1; Y=2; State=Alive }
                        |> setReaction (1,1)
    // Verify
    grid |> getStatus (1,1) |> should equal Alive