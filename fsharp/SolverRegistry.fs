namespace AdventOfCode

open System

module SolverRegistry =

    let private solvers : Map<(int * int), IAdventDay> =
        AppDomain.CurrentDomain.GetAssemblies()
        |> Seq.collect (fun asm -> asm.GetTypes())
        |> Seq.filter (fun t ->
            not t.IsAbstract &&
            typeof<IAdventDay>.IsAssignableFrom t)
        |> Seq.choose (fun t ->
            try
                Some (Activator.CreateInstance(t) :?> IAdventDay)
            with _ ->
                None)
        |> Seq.map (fun s -> ((s.Year, s.Day), s))
        |> Map.ofSeq

    let get year day =
        match solvers |> Map.tryFind (year, day) with
        | Some solver -> solver
        | None ->
            invalidOp $"No solver registered for {year} day {day}"
