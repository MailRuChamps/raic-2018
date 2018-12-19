namespace FSharpCgdk.Model

type Arena = {
    width : float
    height : float
    depth : float
    bottom_radius : float
    top_radius : float
    corner_radius : float
    goal_top_radius : float
    goal_width : float
    goal_height : float
    goal_depth : float
    goal_side_radius : float
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