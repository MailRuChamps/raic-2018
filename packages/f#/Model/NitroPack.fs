namespace FSharpCgdk.Model

type NitroPack = {
    id : int
    x : float32
    y : float32
    z : float32
    radius : float32
    nitro_amount : float32
    respawn_ticks : int option
}

open System.Numerics

module NitroPack = 
    let id nitro = nitro.id

    let x nitro = nitro.x
    let y nitro = nitro.y
    let z nitro = nitro.z
    
    let position nitro = Vector3(nitro.x, nitro.y, nitro.z)
    let position2 nitro = Vector2(nitro.x, nitro.z)

    let radius nitro = nitro.radius
    let nitro_amount nitro = nitro.nitro_amount
    let respawn_ticks nitro = nitro.respawn_ticks