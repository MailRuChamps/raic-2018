namespace FSharpCgdk.Model

type NitroPack = {
    id : int
    x : float
    y : float
    z : float
    radius : float
    nitro_amount : float
    respawn_ticks : int option
}

open System.Numerics

module NitroPack = 
    let id nitro = nitro.id

    let x nitro = nitro.x
    let y nitro = nitro.y
    let z nitro = nitro.z
    
    let position nitro = Vector3(float32 nitro.x, float32 nitro.y, float32 nitro.z)
    let position2 nitro = Vector2(float32 nitro.x, float32 nitro.z)

    let radius nitro = nitro.radius
    let nitro_amount nitro = nitro.nitro_amount
    let respawn_ticks nitro = nitro.respawn_ticks