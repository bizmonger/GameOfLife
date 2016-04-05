namespace Client

open UILogic
open UILogic.DataBinding
open UILogic.Interaction
open Functions
open System
open System.Windows.Input
open System.Collections.ObjectModel
open Types

type ViewModel() as self =
    inherit ViewModelBase()

    let rowCount = 6
    let grid = rowCount |> createGrid
                        |> setCell { X=3; Y=1; State=Alive }
                        |> setCell { X=3; Y=0; State=Alive }
                        |> setCell { X=4; Y=1; State=Alive }

    let mutable _cells = ObservableCollection<Cell>( grid |> getCells)

    let cycleHandler _ = 

        self.Cells <- ObservableCollection<Cell>( grid |> updateCells
                                                       |> getCells)
    let change _ = 

        async { while true do
                    do! Async.Sleep 500
                    cycleHandler() 
              } |> Async.Start

    member self.Play = DelegateCommand (change, fun _ -> true) :> ICommand
    member self.N = rowCount

    member self.Cells
        with get() = _cells 
        and set(value) =
            _cells <- value

            base.NotifyPropertyChanged(<@ self.Cells @>)