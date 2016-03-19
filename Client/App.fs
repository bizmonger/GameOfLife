module MainApp

open System
type App = FsXaml.XAML<"App.xaml">
[<STAThread;EntryPoint>]
let main _ = App().Root.Run()