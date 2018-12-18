namespace FSharpCgdk.Model

type Arena = {
    width : float32
    height : float32
    depth : float32
    bottom_radius : float32
    top_radius : float32
    corner_radius : float32
    goal_top_radius : float32
    goal_width : float32
    goal_height : float32
    goal_depth : float32
    goal_side_radius : float32
}


module Arena = 
    let width arena = arena.width
    let height arena = arena.height
    let depth arena = arena.depth

    let bottom_radius arena = arena.bottom_radius
    let top_radius arena = arena.top_radius
    let corner_radius arena = arena.corner_radius

    let goal_top_radius arena = arena.goal_top_radius
    let goal_width arena = arena.goal_width
    let goal_height arena = arena.goal_height
    let goal_depth arena = arena.goal_depth
    let goal_side_radius arena = arena.goal_side_radius