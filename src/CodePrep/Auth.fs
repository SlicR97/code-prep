module Auth

open FsHttp
open Thoth.Json.Net

type UserCredentials =
  { Email: string
    Password: string }

type Auth =
  { login: unit -> AsyncResult<string, string> }

let login url (credentials: UserCredentials) () =
  async {
    let! response =
      http {
        POST url

        body
        jsonSerialize credentials
      } |> Request.sendAsync

    let! json = Response.toStringAsync None response
    return Decode.Auto.fromString<string>(json)
  }

let mkAuth url credentials =
  { login = login url credentials }
