module Day4

open System
open System.Text.RegularExpressions
open Xunit
open Xunit.Abstractions

type Event =
    | Start
    | Sleep
    | Wake

type Record =
    { Timestamp: DateTime
      Event: Event
      GuardId: int }

type Tests(output: ITestOutputHelper) =
    let input =
//         """[1518-11-01 00:00] Guard #10 begins shift
// [1518-11-01 00:05] falls asleep
// [1518-11-01 00:25] wakes up
// [1518-11-01 00:30] falls asleep
// [1518-11-01 00:55] wakes up
// [1518-11-01 23:58] Guard #99 begins shift
// [1518-11-02 00:40] falls asleep
// [1518-11-02 00:50] wakes up
// [1518-11-03 00:05] Guard #10 begins shift
// [1518-11-03 00:24] falls asleep
// [1518-11-03 00:29] wakes up
// [1518-11-04 00:02] Guard #99 begins shift
// [1518-11-04 00:36] falls asleep
// [1518-11-04 00:46] wakes up
// [1518-11-05 00:03] Guard #99 begins shift
// [1518-11-05 00:45] falls asleep
// [1518-11-05 00:55] wakes up""".Split("\n")
        System.IO.File.ReadAllLines("../../../4.txt")
        |> Seq.sort
        |> Seq.fold
            (fun (currentGuardId, records) input ->
                let m =
                    Regex.Match(input, @"\[(?<date>[^\]]+)\] (?<instruction>.+)")

                let instruction =
                    m.Groups[ "instruction" ].Value.Split(" ")

                let timestamp =
                    DateTime.Parse(m.Groups["date"].Value)

                match instruction[0] with
                | "Guard" -> (Int32.Parse(instruction[1].AsSpan(1)),
                             Seq.append records [ {
                                 Timestamp = timestamp
                                 Event = Start
                                 GuardId = (Int32.Parse(instruction[1].AsSpan(1)))
                             } ])
                | "wakes" -> (currentGuardId,
                              Seq.append records [ {
                                  Timestamp = timestamp
                                  Event = Wake
                                  GuardId = currentGuardId
                              } ])
                | "falls" -> (currentGuardId,
                              Seq.append records [ {
                                  Timestamp = timestamp
                                  Event = Sleep
                                  GuardId = currentGuardId
                              } ])

            ) (0, Seq.empty<Record>)
            |> snd

    [<Fact>]
    let ``Part 1`` () =
        let score =
            input
            |> Seq.groupBy (fun r -> r.GuardId)
            |> Seq.maxBy (fun (guardId, records) ->
                records
                |> Seq.fold (fun (asleepTime, sleepTime) r ->
                    match r.Event with
                    | Sleep -> (r.Timestamp, sleepTime)
                    | Wake -> (asleepTime, sleepTime + int ((r.Timestamp - asleepTime).TotalMinutes))
                    | _ -> (asleepTime, sleepTime)
                ) (DateTime.MinValue, 0)
                |> snd
            )
            |> (fun (guardId, records) ->
                records
                |> Seq.fold (fun (asleepTime, sleepMinutes) r ->
                    match r.Event with
                    | Sleep -> (r.Timestamp, sleepMinutes)
                    | Wake -> (asleepTime, Seq.append sleepMinutes (seq {
                        let minutes = r.Timestamp - asleepTime
                        for t in 0 .. (int minutes.TotalMinutes - 1) do
                            asleepTime.AddMinutes(t).ToString("mm")
                    }))
                    | _ -> (asleepTime, sleepMinutes)
                ) (DateTime.MinValue, Seq.empty<string>)
                |> snd
                |> Seq.groupBy (fun s -> s)
                |> Seq.maxBy (fun (minute, minutes) -> Seq.length minutes)
                |> fun (minute, _) -> guardId * (int minute)
            )
        
        output.WriteLine(string score)

    [<Fact>]
    let ``Part 2`` () =
        let score =
            input
            |> Seq.groupBy (fun r -> r.GuardId)
            |> Seq.map (fun (guardId, records) ->
                records
                |> Seq.fold (fun (asleepTime, sleepMinutes) r ->
                    match r.Event with
                    | Sleep -> (r.Timestamp, sleepMinutes)
                    | Wake -> (asleepTime, Seq.append sleepMinutes (seq {
                        let minutes = r.Timestamp - asleepTime
                        for t in 0 .. (int minutes.TotalMinutes - 1) do
                            asleepTime.AddMinutes(t).ToString("mm")
                    }))
                    | _ -> (asleepTime, sleepMinutes)
                ) (DateTime.MinValue, Seq.empty<string>)
                |> snd
                |> Seq.groupBy (fun s -> s)
                |> Seq.map (fun (m, ms) -> (m, Seq.length ms))
                |> dict
                |> fun d -> (guardId, d)
            )
            |> Seq.maxBy (fun (_, mcounts) -> mcounts.Values |> Seq.append [0] |> Seq.max)
            |> fun (guardId, mcounts) -> guardId * (mcounts |> Seq.maxBy (fun kvp -> kvp.Value) |> fun kvp -> int kvp.Key)
        
        output.WriteLine(string score)
