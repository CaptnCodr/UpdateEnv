namespace UpdateEnv

open System
open System.Management.Automation

[<Cmdlet("Update", "Env")>]
type UpdateEnvironmentVariable () =
    inherit PSCmdlet ()

    [<Parameter(Position = 0, Mandatory = true)>]
    member val Var : string = "" with get, set

    override this.ProcessRecord () =
        ($@"Env:\{this.Var}", Environment.GetEnvironmentVariable(this.Var, EnvironmentVariableTarget.User))
        |> this.SessionState.InvokeProvider.Item.Set
        |> ignore
