module CodePrep

open Auth
open Data

[<EntryPoint>]
let main _ =
  let result =
    asyncResult {
      let credentials: UserCredentials =
        { Email = "test@test.com"
          Password = "abcd1234" }
        
      let authUrl = "https://localhost:5001/security/createToken"
      let challengeUrl = "https://localhost:5001/challenge/01"

      let solver = Solvers.revArray
      
      let auth = mkAuth authUrl credentials
      let challenge = mkChallengeData challengeUrl
    
      do! SolveChallenge.solveChallenge<int list, int list> auth challenge solver
    }
    |> Async.RunSynchronously

  match result with
  | Ok _ -> printfn "Success!"
  | Error err -> printfn $"Error: {err}"
  
  0
