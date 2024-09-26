let f = x: if x == 0 then 0 else if x <= 2 then 1 else f (x - 1) + f (x - 2);
in f 20
