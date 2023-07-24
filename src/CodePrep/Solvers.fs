module Solvers

let rec revArray = function
  | [] -> []
  | x::rest -> (revArray rest) @ [x]
