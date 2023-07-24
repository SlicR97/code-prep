module SolveChallenge

open Auth
open Data

let solveChallenge<'data, 'result> (auth: Auth) (challenge: ChallengeData<'data, 'result>) solver =
  asyncResult {
    let! token = auth.login ()

    let! challengeData = challenge.fetch token

    let result = solver challengeData

    do! challenge.submit token result
  }
