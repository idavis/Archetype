#r @"packages/FAKE/tools/FakeLib.dll"
open Fake
open Fake.FileHelper
open Fake.Testing.NUnit3

RestorePackages()

let buildDir = "build"
let nunit3 = "./packages/NUnit.ConsoleRunner/tools/nunit3-console.exe"

CreateDir buildDir

Target "Clean" (fun _ ->
    CleanDir "build"
)

Target "Compile" (fun _ ->
    let buildMode = getBuildParamOrDefault "buildMode" "Release"
    let setParams defaults =
        { defaults with
            Verbosity = Some(Quiet)
            Targets = ["Build"]
            Properties =
                [
                    "Optimize", "True"
                    "DebugSymbols", "True"
                    "Configuration", buildMode
                ]
         }
    build setParams "./Archetype.sln"
      |> DoNothing
)

Target "Test" (fun _ ->
    !! ("src/**/bin/Release/*.Tests.dll")
        |> NUnit3 (fun p ->
            {p with
                ShadowCopy  = false
                WorkingDir = "./build"
                ToolPath = nunit3 })
)
 
"Clean"
   ==> "Compile"
   ==> "Test"

 
RunTargetOrDefault "Test"
