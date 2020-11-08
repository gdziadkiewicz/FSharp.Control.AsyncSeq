(*** condition: prepare ***)
#nowarn "211"
#I "../src/FSharp.Control.AsyncSeq/bin/Release/netstandard2.1"
#r "FSharp.Control.AsyncSeq.dll"
#r "../packages/docscompilation/Microsoft.WindowsDesktop.App.Ref/ref/netcoreapp3.1/System.Windows.Forms.dll"
(*** condition: fsx ***)
#if FSX
#r "nuget: FSharp.Control.AsyncSeq,{{package-version}}"
#r "nuget: Microsoft.WindowsDesktop.App.Ref,3.1.0"
#endif // FSX
(*** condition: ipynb ***)
#if IPYNB
#r "nuget: FSharp.Control.AsyncSeq,{{package-version}}"
#r "nuget: Microsoft.WindowsDesktop.App.Ref,3.1.0"
#endif // IPYNB

(**
[![Binder](https://mybinder.org/badge_logo.svg)](https://mybinder.org/v2/gh/fsprojects/FSharp.Control.AsyncSeq/gh-pages?filepath=AsyncSeqObservable.ipynb)

# IObservable<'T> to AsyncSeq<'T> conversion

*)

// ----------------------------------------------------------------------------
// F# async extensions (AsyncSeqObservable.fsx)
// (c) Tomas Petricek, 2011, Available under Apache 2.0 license.
// ----------------------------------------------------------------------------

#r "../../../bin/FSharp.Control.AsyncSeq.dll"
open FSharp.Control
open System.Windows.Forms
open System.Threading

// Create simple winforms user interface with a button and multiline text box
let frm = new Form(Visible=true, TopMost=true, Width=440)
let btn = new Button(Left=10, Top=10, Width=150, Text="Async Operation")
let out = new TextBox(Left=10, Top=40, Width=400, Height=200, Multiline=true)
frm.Controls.Add(btn)
frm.Controls.Add(out)

// Prints message to the displayed text box
let wprint fmt =
  Printf.kprintf (fun s -> out.Text <- out.Text + s) fmt

// When using 'AsyncSeq.ofObservableBuffered', the values emitted by the
// observable while the asynchronous sequence is blocked are stored in a
// buffer (and will be returned as next elements).
let buffering =
  async {
    for click in btn.Click |> AsyncSeq.ofObservableBuffered do
      wprint "Sleeping (and buffering clicks)...\r\n"
      do! Async.Sleep(1000)
      wprint "Done (ready for next value)\r\n" }

let ctsb = new CancellationTokenSource()
Async.Start(buffering, ctsb.Token)
ctsb.Cancel()
