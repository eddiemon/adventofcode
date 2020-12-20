function solve1(input)
    for i in 1:length(input)-1
        for j in i:length(input)
            if (input[i] + input[j] == 2020)
                println(input[i] * input[j])
            end
        end
    end
end

function solve2(input) do
    for i in 1:length(input)-2
        for j in i:length(input)-1
            for k in j:length(input)
                if (input[i] + input[j] + input[k] == 2020)
                    println(input[i] * input[j] * input[k])
                end
            end
        end
    end
end

input = parse.(Int64, readlines("1.in"))

solve1(input)
solve2(input)