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

    let private log strAction msg =
        printfn "%6s | %6s |> %s" "RPC" strAction msg

    let create host port = 
        let client = new TcpClient(host, port)
        client.NoDelay <- true
        let reader = new StreamReader(client.GetStream())
        let writer = new StreamWriter(client.GetStream())
        writer.WriteLine("json")
        writer.Flush()
        log "create" (host + (string port))
        { Client = client; Reader = reader; Writer = writer }

    let read rpc : 'a option = 
        let line = rpc.Reader.ReadLine()
        if line = null || line.Length = 0 then
            log "read" "None"
            None
        else 
            log "read" line
            Json.deserialize line 
            |> Some

    let readGame rpc : Game option = read rpc

    let readRules rpc : Rules option = read rpc

    let write rpc actions = 
        let json = Json.serializeU actions
        log "write" json
        rpc.Writer.WriteLine()
        rpc.Writer.Flush()

    let writeToken rpc (token : string) = 
        log "writeToken" token
        rpc.Writer.WriteLine(token)
        rpc.Writer.Flush()