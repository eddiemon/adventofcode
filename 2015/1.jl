function solve()
    input = readlines("1.in")[1]
    floorNo = 0
    for c in input
        floorNo += c == '(' ? 1 : -1
    end
    floorNo
end
println(solve())
