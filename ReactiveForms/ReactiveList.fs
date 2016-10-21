namespace ReactiveForms

open Xamarin.Forms
open System
open System.Reactive.Linq
open FSharp.Control.Reactive
open FSharp.Control.Reactive.Observable

module ReactiveList =
    type Person = { name: string; birthday: DateTime; favoriteColor: Color }

    let header = Label(HorizontalOptions = LayoutOptions.Center, Text = "ListView")

    let listView = ListView(ItemsSource = [])

