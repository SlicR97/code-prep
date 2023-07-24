namespace global

type AsyncResult<'Success, 'Failure> =
  Async<Result<'Success, 'Failure>>

[<RequireQualifiedAccess>]
module AsyncResult =
  let map f (x: AsyncResult<_, _>): AsyncResult<_, _> =
    Async.map (Result.map f) x

  let mapError f (x: AsyncResult<_, _>): AsyncResult<_, _> =
    Async.map (Result.mapError f) x
  
  let retn x: AsyncResult<_, _> =
    x |> Result.Ok |> Async.retn
    
  let bind (f: 'a -> AsyncResult<'b, 'c>) (xAsyncResult: AsyncResult<_, _>): AsyncResult<_, _> = async {
    let! xResult = xAsyncResult
    match xResult with
    | Ok x -> return! f x
    | Error err -> return (Error err)
  }
  
  let ofSuccess x : AsyncResult<_,_> =
    x |> Result.Ok |> Async.retn

  let ofError x : AsyncResult<_,_> =
    x |> Result.Error |> Async.retn
    
  let ofResult x : AsyncResult<_,_> =
    x |> Async.retn

  let ofAsync x : AsyncResult<_,_> =
    x |> Async.map Result.Ok

[<AutoOpen>]
module AsyncResultComputationExpression =
  type AsyncResultBuilder() =
    member _.Return(result) = AsyncResult.retn result
    member _.Bind(asyncResult, f) = AsyncResult.bind f asyncResult
    member _.ReturnFrom(asyncResult) = asyncResult
    
    member _.Zero () : AsyncResult<unit, 'TError> =
      Ok () |> async.Return

  let asyncResult = AsyncResultBuilder()