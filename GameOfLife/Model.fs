﻿module Model

type State = Alive | Dead
type Cell = { X:int; Y:int; State:State }

type Response = | Die
                | Survive
                | Resurect
    
let isNeighbor cell1 cell2 =

    let isAbsNeighbor v1 v2 =
        match abs (v1 - v2) with
        | 0 | 1 -> true
        | _     -> false

    let isValueNeighbor v1 v2 =
        match v1 >= 0
          &&  v2 >= 0 with
        | true  -> isAbsNeighbor v1 v2
        | _     -> isAbsNeighbor v2 v1

    match cell1.X <> cell2.X
      ||  cell1.Y <> cell2.Y with
    | true ->   isValueNeighbor cell1.X cell2.X
             && isValueNeighbor cell1.Y cell2.Y
    | _    -> false

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

let createTimer timerInterval eventHandler =

    let timer = new System.Timers.Timer(float timerInterval)
    timer.AutoReset <- true
    timer.Elapsed.Add eventHandler

    async { timer.Start()
            do! Async.Sleep 500
            timer.Stop() }

let setReaction coordinate grid:Map<(int * int), Cell> = 

    let x,y = coordinate
    let count = coordinate |> getNeighbors
                           |> List.filter (fun coordinate -> grid |> getStatus coordinate = Alive)
                           |> List.length
    match count with
    | count when count < 2 
             ||  count > 3 -> grid |> setCell { X=x; Y=y; State=Dead }
    | 3                    -> match grid.TryFind coordinate with
                              | Some cell -> match cell.State with
                                             | Dead -> grid |> setCell { cell with State=Alive }
                                             | _    -> grid
                              | None      -> failwith "Cell doesn't exists"
    | _ -> grid

let cycleThroughCells (grid:Map<(int * int), Cell>) =

    grid |> Map.toSeq
         |> Seq.map snd
         |> Seq.fold (fun grid c -> grid |> setReaction (c.X, c.Y)) grid