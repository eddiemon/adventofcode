m = (readlines("2.in") .|>
    l->match(r"(?<min>\d+)-(?<max>\d+) (?<char>.): (?<pw>.+)", l)) |>
    m->map(m->(min=parse(Int,m["min"]), max=parse(Int,m["max"]), count=count(m["char"], m["pw"])), m) |>
    m->filter(x-> x.min <= x.count <= x.max, m)
show(length(m))