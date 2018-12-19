namespace FSharpCgdk

open System.Net.Sockets
open System.IO
open System.Collections.Generic
open FSharp.Json


open FSharpCgdk.Model


type RemoteProcessClient = {
    Client : TcpClient
    Reader : StreamReader
    Writer : StreamWriter
}


module RemoteProcessClient = 
    let create host port = 
        let client = new TcpClient(host, port)
        client.NoDelay <- true
        let reader = new StreamReader(client.GetStream())
        let writer = new StreamWriter(client.GetStream())
        writer.WriteLine("json")
        writer.Flush()
        { Client = client; Reader = reader; Writer = writer }

    let read rpc : 'a option = 
        let line = rpc.Reader.ReadLine()
        if line = null || line.Length = 0 then
            None
        else 
            Json.deserialize line 
            |> Some

    let readGame rpc : Game option = read rpc

    let readRules rpc : Rules option = read rpc

    let write rpc actions = 
        rpc.Writer.WriteLine(Json.serialize actions)
        rpc.Writer.Flush()

    let writeToken rpc (token : string) = 
        rpc.Writer.WriteLine(token)
        rpc.Writer.Flush()