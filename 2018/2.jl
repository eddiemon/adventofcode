function solve()
    codes = chomp.(readlines())
    # println(codes)

    twos = 0
    threes = 0
    for l in codes
        cnt = Dict()
        for c in l
            ccnt = get(cnt, c, 0)
            cnt[c] = ccnt + 1
            # print(c)
        end
        # println(Set(values(cnt)))
        twos += 2 in values(cnt) ? 1 : 0
        threes += 3 in values(cnt) ? 1 : 0
    end
    println(twos * threes)
end

solve()