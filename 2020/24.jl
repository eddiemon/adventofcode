cups = "389125467"
for i in 1:100
    currCup = i % length(cups)
    cupsToMove = cups[currCup+1:currCup+4]
    cups = replace(cups, cupsToMove=>"")
    
    offset = 0
    dest = cups[currCup - offset]
    while true
        if dest ∉ cupsToMove
            break
        end
        offset++
        if offset > 9
            offset = 1
        end
        dest = cups[currCup - offset]
    end
    cups = replace(cups, dest => dest * cupsToMove)
end

println(cups)