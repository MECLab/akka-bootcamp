open System
open Akka.FSharp

type Message = string
type SystemMessage =
    | ContinueProcessing


[<EntryPoint>]
let main argv = 
    let myActorSystem =  System.create "my-actor-system" <| Configuration.load()

    let writerActor = 
        spawn myActorSystem "Writer-Actor" 
        <| fun mailbox -> 
                let rec loop() = actor {
                    let! message = mailbox.Receive()
                    // do something with the message.

                    return! loop()
                }
                loop()

    let reader = spawn myActorSystem "Reader-Actor" <| fun mailbox ->
        let rec loop() = actor {
            let! message = mailbox.Receive()
            // do something with the message.

            writerActor.Tell message
            return! loop()
        }
        loop()
    
    reader <! Console.ReadLine()

    0 // return an integer exit code 
