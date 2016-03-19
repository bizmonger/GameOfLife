module UILogic.Interaction

open System
open System.Windows
open System.Windows.Input
open System.ComponentModel

type DelegateCommand (action:(obj -> unit), canExecute:(obj -> bool)) =
    let event = new DelegateEvent<EventHandler>()
    interface ICommand with
        [<CLIEvent>]
        member this.CanExecuteChanged = event.Publish
        member this.CanExecute arg = canExecute(arg)
        member this.Execute arg = action(arg)