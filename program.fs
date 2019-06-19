namespace Sample

open DurableFunctions.FSharp
open Microsoft.Azure.WebJobs
open FSharp.Control.Tasks.ContextInsensitive


type MyFunctionType(mydep:MyDependency) = 

    //Question - how would I create a typed Activity definition for this member function?
    [<FunctionName("DoAdd")>]
    member __.DoAdd([<ActivityTrigger>] context:DurableActivityContext) =
        mydep.Add 2 2

    [<FunctionName("StartOrchestration")>]
    member __.StartRevokeOrchestration([<OrchestrationClient>] client:DurableOrchestrationClient,
                                        [<HttpTrigger("GET")>] startMessage:string) = 
        task {
          // TODO accept timestamp in the start message to supply to the audit query
          let! instance = client.StartNewAsync("SampleOrchestration",null) 
          ()            
        }  


type OrchestratorContainer() = 

    let myorchestration = orchestrator {

        let! result = Activity.callByName<int>("DoAdd") ()
        return result
    }

    [<FunctionName("SampleOrchestration")>]
    member x.Revoke([<OrchestrationTrigger>] context: DurableOrchestrationContext) =      
        Orchestrator.run (myorchestration, context)

