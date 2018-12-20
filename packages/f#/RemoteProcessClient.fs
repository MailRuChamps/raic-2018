namespace FSharpCgdk

open System.Net.Sockets
open System.IO
open System.Collections.Generic
open FSharp.Json


open FSharpCgdk.Model


type RemoteProcessClient(host, port) = 
    let client = new TcpClient(host, port)
    do client.NoDelay <- true
    let reader = new StreamReader(client.GetStream())
    let writer = new StreamWriter(client.GetStream())
    do 
        writer.WriteLine("json")
        writer.Flush()


    member this.readGame() : Game option = 
        let line = reader.ReadLine()
        if line = null || line.Length = 0 then
            None
        else 
            Some (Json.deserialize line)


    member this.readRules() : Rules option = 
        let line = reader.ReadLine()
        if line = null || line.Length = 0 then
            None
        else 
            Some (Json.deserialize line)


    member this.write (actions : Map<string, Action>) = 
        let json = Json.serializeU actions
        writer.WriteLine json
        writer.Flush()


    member this.writeToken (token : string) = 
        writer.WriteLine token
        writer.Flush()
