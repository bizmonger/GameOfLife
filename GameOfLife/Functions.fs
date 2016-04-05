module Functions

open Types
open ActivePatterns
    
let isNeighbor cell1 cell2 =

    let isAbsNeighbor v1 v2 =
        match abs (v1 - v2) with
        | 0 | 1 -> true
        | _     -> false

    let isValueNeighbor v1 v2 =
        match v1, v2 with
        | BothPositive    -> isAbsNeighbor v1 v2
        | NotBothPositive -> isAbsNeighbor v2 v1

    match (cell1,cell2) with
    | NotEqual -> isValueNeighbor cell1.X cell2.X
                 && isValueNeighbor cell1.Y cell2.Y
    | IsEqual    -> false

let createGrid rowCount = 

    [for x in 0..rowCount-1 do
        for y in 0..rowCount-1 do
            yield { X=x; Y=y; State=Dead } 
    ]|> List.map (fun c -> (c.X, c.Y), { X=c.X; Y=c.Y; State=Dead })
     |> Map.ofList

let setCell cell (grid:Map<(int * int), Cell>) =

    grid |> Map.map (fun k v -> match k with
                                | c when c = (cell.X, cell.Y) -> { v with State=cell.State }
                                | _ -> v)

let getStatus coordinate (grid:Map<(int * int), Cell>) =

    match grid.TryFind coordinate with
    | Some cell -> cell.State
    | None      -> Dead

let getNeighbors (coordinate:int*int) =
        
    let x,y = coordinate
    let west = x-1, y
    let northWest = x-1, y+1
    let north = x, y+1
    let northEast = x+1, y+1
    let east = x+1, y
    let southEast = x+1, y-1
    let south = x, y-1
    let southWest = x-1, y-1

    [west; northWest; north; northEast; east; southEast; south; southWest]

let setLive coordinate (grid:Map<(int * int), Cell>) =

    match grid.TryFind coordinate with
                        | Some cell -> match cell.State with
                                        | Dead  -> grid |> setCell { cell with State=Alive }
                                        | Alive -> grid
                        | None      -> failwith "Cell doesn't exists"

let setReaction coordinate grid:Map<(int * int), Cell> = 

    let x,y = coordinate
    let count = coordinate |> getNeighbors
                           |> List.filter (fun coordinate -> grid |> getStatus coordinate = Alive)
                           |> List.length
    match count with
    | ShouldDie  -> grid |> setCell { X=x; Y=y; State=Dead }
    | ShouldLive -> grid |> setLive coordinate
    | NoResponse -> grid

let getCells grid = grid |> Map.toSeq 
                         |> Seq.map snd 
                         |> Seq.toList

let updateCells (grid:Map<(int * int), Cell>) =

    grid |> getCells
         |> Seq.fold (fun grid c -> grid |> setReaction (c.X, c.Y)) grid