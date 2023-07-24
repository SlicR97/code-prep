namespace global

[<RequireQualifiedAccess>]
module Async =
  let map f xA =
    async {
      let! x = xA
      return f x
    }

  let retn x =
    async.Return x
