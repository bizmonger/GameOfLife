namespace Client
open System.Windows.Data
open Functions
open System.Windows.Media
open Types
 
type StateToBrushConverter() =
    interface IValueConverter with
        member x.Convert(value, targetType, parameter, culture) = 
            let cell = value :?> Cell
            match cell.State with 
            | Alive -> SolidColorBrush(Colors.LightGreen) :> obj
            | Dead  -> SolidColorBrush(Colors.Black)      :> obj
 
        member x.ConvertBack(value, targetType, parameter, culture) = failwith "Not implemented yet"