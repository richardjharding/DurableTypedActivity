namespace Sample

open Microsoft.Extensions.DependencyInjection
open Microsoft.Azure.Functions.Extensions.DependencyInjection


type MyDependency() = 
    member __.Add fst snd = 
        fst + snd


type Startup() =
    inherit FunctionsStartup()


    override __.Configure( builder:IFunctionsHostBuilder) = 

        builder.Services.AddScoped<MyDependency>(fun c -> MyDependency()) |> ignore

        ()

[<assembly: FunctionsStartup(typeof<Startup>)>]
do()