namespace Client

open UILogic
open UILogic.DataBinding
open UILogic.Interaction
open Model
open System
open System.Windows.Input
open System.Collections.ObjectModel

type ViewModel() as self =
    inherit ViewModelBase()

    let rowCount = 6
    let grid = rowCount |> createGrid
                        |> setCell { X=3; Y=1; State=Alive }
                        |> setCell { X=3; Y=0; State=Alive }
                        |> setCell { X=4; Y=1; State=Alive }

    let mutable _cells = ObservableCollection<Cell>( grid |> Map.toSeq
                                                          |> Seq.map snd
                                                          |> Seq.toList )
    let cycleHandler _ = 

        self.Cells <- ObservableCollection<Cell>( grid |> cycleThroughCells
                                                       |> Map.toSeq
                                                       |> Seq.map snd
                                                       |> Seq.toList )
    let change _ = 

        async { while true do
                    do! Async.Sleep 500
                    cycleHandler() } |> Async.Start

    member self.Play = DelegateCommand (change, fun _ -> true) :> ICommand
    member self.N = rowCount

    member self.Cells
        with get() = _cells 
        and set(value) =
            _cells <- value

            base.NotifyPropertyChanged(<@ self.Cells @>)

//type ViewModel() as self = 
//    inherit ViewModelBase()   
//
//    let rowCount = 6
//    let mutable grid = rowCount |> createGrid
//                                |> setCell { Y=2; X=1; State=Alive }
//                                |> setCell { Y=2; X=2; State=Alive }
//                                |> setCell { Y=3; X=2; State=Alive }
//                                |> setCell { Y=3; X=3; State=Alive }
//
//    let mutable _cells = grid |> Map.toSeq
//                              |> Seq.map snd
//                              |> Seq.toList
//
//    let mutable count = 0
//    let mutable isEnabled = true
//
//    let cycleHandler _ = 
//        _cells <- grid |> cycleThroughCells
//                       |> Map.toSeq
//                       |> Seq.map snd
//                       |> Seq.toList
//        count <- count + 1
//        self.NotifyPropertyChanged(<@ self.Cells @>)
//        self.NotifyPropertyChanged(<@ self.Count @>)
//
//    let change _ = 
//        isEnabled <- false
//        self.NotifyPropertyChanged(<@ self.IsEnabled @>)
//
//        async
//            {
//                while true do
//                    do! Async.Sleep 2000
//                    cycleHandler()
//            }
//        |> Async.Start
//
//    member __.Play = DelegateCommand (change, fun _ -> isEnabled) :> ICommand
//    member __.N = rowCount
//    member __.Cells = _cells
//    member __.Count = count
//    member __.IsEnabled = isEnabled