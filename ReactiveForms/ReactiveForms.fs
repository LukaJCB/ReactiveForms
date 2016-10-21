namespace ReactiveForms

open Xamarin.Forms
open System
open System.Reactive.Linq
open FSharp.Collections


module RxForms =
    let fromSwitch (s: Switch) = Observable.Create(fun (o: IObserver<bool>) ->
        s.Toggled.Subscribe(fun t -> o.OnNext(t.Value)))

    let fromButton (b: Button) = Observable.Create(fun (o: IObserver<unit>) -> 
        b.Clicked.Subscribe(fun _ -> o.OnNext( () )))

    let fromEntry (e: Entry) = Observable.Create(fun (o: IObserver<_>) -> 
        e.TextChanged.Subscribe(fun s -> o.OnNext(s.NewTextValue)))

    let fromEntryCompleted (e: Entry) = Observable.Create(fun (o: IObserver<_>) -> 
        e.Completed.Subscribe(fun s -> o.OnNext(e.Text)))



    let bindLabel (l: Label) (o: IObservable<string>) = 
        o |> Observable.subscribe(fun s -> l.Text <- s) 

    let bindListView (list: ListView) (o: IObservable<List<_>>) =
        o |> Observable.subscribe(fun ts -> list.ItemsSource <- ts) 

    let bindEntry (e: Entry) (o: IObservable<string>) =
        o |> Observable.subscribe(fun s -> e.Text <- s) 



type App() =
    inherit Application()

    let stack = StackLayout(Padding = Thickness(0.0, 40.0, 0.0, 0.0))
    let editText = Entry(Placeholder = "What needs to be done?", Margin = Thickness(10.0, 0.0))
    let listView = ListView(Margin = Thickness(-5.0, 0.0, 10.0, 0.0))

    let submittedTodos = RxForms.fromEntryCompleted editText

    let todoLists = Observable.scan (fun acc cur -> acc |> List.append [cur]) [] submittedTodos
    let subscription = todoLists |> RxForms.bindListView listView
    let subscription2 = todoLists |> Observable.map (fun _ -> "") |> RxForms.bindEntry editText
        
    do stack.Children.Add(editText)
    do stack.Children.Add(listView)
    do base.MainPage <- ContentPage(Content = stack)

    let switch = Switch()
    let label = Label()

    let button = Button(VerticalOptions = LayoutOptions.CenterAndExpand, Font = Font.SystemFontOfSize(NamedSize.Large), Text = "Click me!")
