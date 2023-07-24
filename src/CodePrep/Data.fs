module Data

open System.Net
open FsHttp
open Thoth.Json.Net

type ChallengeData<'data, 'result> =
  { fetch: string -> AsyncResult<'data, string>
    submit: string -> 'result -> AsyncResult<unit, string> }

let fetch<'t> challengeUrl accessToken =
  async {
    let! response = Request.sendAsync (http {
      GET challengeUrl
      
      header "Authorization" $"Bearer {accessToken}"
    })
    
    match response.statusCode with
    | HttpStatusCode.OK ->
      let! json = Response.toJsonAsync response
      let text = json.GetRawText()
      let! res =
        text
        |> Decode.Auto.fromString<'t>
        |> AsyncResult.ofResult
        
      return res
    | HttpStatusCode.Unauthorized -> return Error "Unauthorized"
    | HttpStatusCode.NotFound -> return Error $"Not Found {challengeUrl}"
    | _ -> return Error "Unknown Error"
  }

let submit<'result> challengeUrl accessToken (result: 'result) =
  async {
    let! response =
      http {
        POST challengeUrl
        
        header "Authorization" $"Bearer {accessToken}"
        
        body
        jsonSerialize result
      } |> Request.sendAsync
      
    match response.statusCode with
    | HttpStatusCode.NoContent -> return Ok ()
    | HttpStatusCode.Unauthorized -> return Error "Unauthorized"
    | HttpStatusCode.NotFound -> return Error $"Not Found {challengeUrl}"
    | _ -> return Error "Unknown Error"
  }

let mkChallengeData challengeUrl =
  { fetch = fetch challengeUrl
    submit = submit challengeUrl }
