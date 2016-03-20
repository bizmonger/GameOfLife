namespace Client

open UILogic
open UILogic.DataBinding
open UILogic.Interaction
open Model
open System
open System.Windows.Input

type ViewModel() as self =
    inherit ViewModelBase()

    let rowCount = 6
    let mutable grid = rowCount |> createGrid
                                |> setCell { X=3; Y=1; State=Alive }
                                |> setCell { X=3; Y=0; State=Alive }
                                |> setCell { X=4; Y=1; State=Alive }

    let mutable _cells = grid |> Map.toSeq
                              |> Seq.map snd
                              |> Seq.toList
    let cycleHandler _ = 

        self.Items <- grid |> cycleThroughCells
                           |> Map.toSeq
                           |> Seq.map snd
                           |> Seq.toList

    member self.Play =
        DelegateCommand ((fun _ -> let timer = createTimer 500 cycleHandler
                                   do while true do
                                      do Async.RunSynchronously timer), fun _ -> true) :> ICommand
    member self.N = rowCount

    member self.Items
        with get() = _cells 
        and set(value) =
            _cells <- value

            base.NotifyPropertyChanged(<@self.Items@>)