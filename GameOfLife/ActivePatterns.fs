module ActivePatterns

open Types

let (|IsEqual|NotEqual|) (cell1, cell2) =

    if cell1.X <> cell2.X || cell1.Y <> cell2.Y 
    then NotEqual
    else IsEqual

let (|BothPositive|NotBothPositive|) (v1, v2) =

    if v1 >= 0 && v2 >= 0 
    then BothPositive

    else NotBothPositive

let (|ShouldDie|ShouldLive|NoResponse|) count =

    match count with
    | count when count < 2 
             ||  count > 3 -> ShouldDie
    | 3                    -> ShouldLive
    | _                    -> NoResponse