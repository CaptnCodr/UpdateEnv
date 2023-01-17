namespace UpdateEnv

open System
open System.Management.Automation

[<Cmdlet("Update", "Env")>]
type UpdateEnvironmentVariable () =
    inherit PSCmdlet ()

    [<Parameter(Position = 0, Mandatory = true)>]
    member val Var : string = "" with get, set

    override this.ProcessRecord () =
        let psVar =
            this.SessionState.InvokeProvider.Item.Get($@"Env:\{this.Var}")
            |> Seq.head
            |> fun o -> o.Properties
            |> Seq.tryFind (fun p -> p.Name = "Value")
            |> function
                | Some prop -> $"PS: {prop.Value}"
                | None -> "PS: "

        let envVar = $"Env: {Environment.GetEnvironmentVariable(this.Var, EnvironmentVariableTarget.User)}"

        this.WriteObject ($"{psVar}, {envVar}")

        ($@"Env:\{this.Var}", Environment.GetEnvironmentVariable(this.Var, EnvironmentVariableTarget.User))
        |> this.SessionState.InvokeProvider.Item.Set
        |> ignore

        this.WriteObject ($"'{this.Var}' has been updated!")
