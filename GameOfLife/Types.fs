module Types

type State = Alive | Dead

type Cell = { X:int; Y:int; State:State }

type Response = | Die
                | Survive
                | Resurect