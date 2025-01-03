﻿using System.Text.RegularExpressions;

namespace AdventOfCode;
internal class Day5
{
    private static readonly bool real = true;
    private static readonly Dictionary<Tuple<long, long>, long> seedToSoil = GetSeedToSoilMap();
    private static readonly Dictionary<Tuple<long, long>, long> soilToFertilizer = GetSoilToFertilizerMap();
    private static readonly Dictionary<Tuple<long, long>, long> fertilizerToWater = GetFertilizerToWaterMap();
    private static readonly Dictionary<Tuple<long, long>, long> waterToLight = GetWaterToLightMap();
    private static readonly Dictionary<Tuple<long, long>, long> lightToTemp = GetLightToTemperatureMap();
    private static readonly Dictionary<Tuple<long, long>, long> tempToHumidity = GetTemperatureToHumidityMap();
    private static readonly Dictionary<Tuple<long, long>, long> humidityToLocation = GetHumidityToLocationMap();
    internal static long PuzzleDay5a(string input)
    {
        int sum = 0;
        return input.Split(" ")
            .Select(x => long.Parse(x))
            .Select(x => new
            {
                Seed = x,
                Location = MapHumidityToLocation(MapTempToHumidity(MapLightToTemp(MapWaterToLight(MapFertilizerToWater(MapSoilToFertilizer(MapSeedToSoil(x)))))))
            })
            .OrderBy(x => x.Location)
            .First().Location;
    }

    internal static long PuzzleDay5a2(string input)
    {
        int sum = 0;
        return input.Split(" ")
            .Select(x => long.Parse(x))
            .Select(x => MapHumidityToLocation(MapTempToHumidity(MapLightToTemp(MapWaterToLight(MapFertilizerToWater(MapSoilToFertilizer(MapSeedToSoil(x))))))))
            .Min();
    }

    internal static long PuzzleDay5b(string input)
    {
        var seeds = Regex.Matches(input, "(\\d+) (\\d+)")
            .Cast<Match>()
            .Select(x => x.Value.Split(" "))
            .Select(x => new LongRange(long.Parse(x[0]), long.Parse(x[0])+long.Parse(x[1])-1))
            .OrderBy(x => x.Start)
            .ToList();

        var seedToLocation = Compose(seedToSoil, soilToFertilizer);
        seedToLocation = Compose(seedToLocation, fertilizerToWater);
        seedToLocation = Compose(seedToLocation, waterToLight);
        seedToLocation = Compose(seedToLocation, lightToTemp);
        seedToLocation = Compose(seedToLocation, tempToHumidity);
        seedToLocation = Compose(seedToLocation, humidityToLocation);
        seedToLocation = seedToLocation.OrderBy(x => x.Key.Item2).ToDictionary();

        var filteredMap = seedToLocation.Where(x => x.Key.Item1 + x.Value - 1 >= seeds.First().Start)
            .OrderBy(x => x.Key.Item2).ToDictionary();
        foreach (var map in filteredMap)
        {
            var seed = seeds.Where(x => map.Key.Item1 <= x.End && map.Key.Item1 + map.Value - 1 >= x.Start)
                .FirstOrDefault();
            if (seed != null)
            {
                if (map.Key.Item1 > seed.Start)
                {
                    return MapXToY(filteredMap, map.Key.Item1);
                } 
                else
                {
                    return MapXToY(filteredMap, seed.Start);
                }
            }
        }
        int sum = 0;
        return input.Split(" ")
            .Select(x => long.Parse(x))
            .Select(x => new
            {
                Seed = x,
                Location = MapHumidityToLocation(MapTempToHumidity(MapLightToTemp(MapWaterToLight(MapFertilizerToWater(MapSoilToFertilizer(MapSeedToSoil(x)))))))
            })
            .OrderBy(x => x.Location)
            .First().Location;
    }

    private static long MapSeedToSoil(long source)
    {
        return MapXToY(seedToSoil, source);
    }
    private static long MapSoilToFertilizer(long source)
    {
        return MapXToY(soilToFertilizer, source);
    }
    private static long MapFertilizerToWater(long source)
    {
        return MapXToY(fertilizerToWater, source);
    }
    private static long MapWaterToLight(long source)
    {
        return MapXToY(waterToLight, source);
    }
    private static long MapLightToTemp(long source)
    {
        return MapXToY(lightToTemp, source);
    }
    private static long MapTempToHumidity(long source)
    {
        return MapXToY(tempToHumidity, source);
    }
    private static long MapHumidityToLocation(long source)
    {
        return MapXToY(humidityToLocation, source);
    }

    private static long MapXToY(Dictionary<Tuple<long, long>, long> map, long source)
    {
        var entry = map.Where(x => x.Key.Item1 <= source)
           .OrderByDescending(x => x.Key.Item1)
           .FirstOrDefault();
        if (entry.Key == null)
        {
            return source;
        }
        long diff = source - entry.Key.Item1;
        if (diff > entry.Value)
        {
            return source;
        }
        return diff + entry.Key.Item2;
    }

    private static Dictionary<Tuple<long, long>, long> Compose(Dictionary<Tuple<long, long>, long> aMap, Dictionary<Tuple<long, long>, long> bMap)
    {
        var cMap = new Dictionary<Tuple<long, long>, long>();

        foreach (var next in aMap)
        {
            Queue<KeyValuePair<Tuple<long, long>, long>> temp = new();
            temp.Enqueue(next);

            while (temp.Count > 0)
            {
                var a = temp.Dequeue();
                var b = bMap.Where(x => x.Key.Item1 <= a.Key.Item2)
                   .OrderByDescending(x => x.Key.Item1)
                   .FirstOrDefault();
                if (b.Key == null || a.Key.Item2 >= b.Key.Item1 + b.Value)
                {
                    cMap.Add(a.Key, a.Value);
                }
                else
                {
                    var dst = a.Key.Item2 - b.Key.Item1 + b.Key.Item2;
                    var src = a.Key.Item1;
                    var bSpace = b.Value - a.Key.Item2 + b.Key.Item1;
                    var rng = Math.Min(a.Value, bSpace);
                    cMap.Add(new(src, dst), rng);
                    if (a.Value > bSpace)
                    {
                        temp.Enqueue(new(new(a.Key.Item1 + rng, a.Key.Item2 + rng), a.Value - rng));
                    }
                }
            }
        }


        return cMap;
    }

    private static Dictionary<Tuple<long, long>, long> CreateMap(string input)
    {
        Dictionary<Tuple<long, long>, long> map = [];
        string[] inputs = input.Split("\r\n");
        foreach (var s in inputs)
        {
            var match = Regex.Match(s, "(\\d+) (\\d+) (\\d+)");
            long src = long.Parse(match.Groups[2].Value);
            long dst = long.Parse(match.Groups[1].Value);
            long num = long.Parse(match.Groups[3].Value);
            map.Add(new(src, dst), num);
        }

        var first = map.OrderBy(x => x.Key.Item1).First();
        if (first.Key.Item1 > 0)
        {
            map.Add(new(0, 0), first.Key.Item1);
        }

        return map.OrderBy(x => x.Key.Item2).ToDictionary();
    }

    private static Dictionary<Tuple<long, long>, long> GetSeedToSoilMap()
    {
        string input = real?
            @"3333452986 2926455387 455063168
            3222292973 1807198589 111160013
            4073195028 1120843626 221772268
            3215232741 2255546991 7060232
            1658311530 2727928910 32644400
            2680271553 1918358602 337188389
            1690955930 3973557555 28589896
            2081345351 4046183137 248784159
            2374165196 3613106716 306106357
            1553535599 2504868379 49003335
            4018850546 3919213073 54344482
            2050713919 2287201502 30631432
            3183342019 1775307867 31890722
            3975551599 2553871714 43298947
            1120843626 1342615894 432691973
            2330129510 4002147451 44035686
            1719545826 2628348978 99579932
            1819125758 3381518555 231588161
            1627133213 2597170661 31178317
            3017459942 2760573310 165882077
            3788516154 2317832934 187035445
            1602538934 2262607223 24594279"

            : @"50 98 2
52 50 48";

        return CreateMap(input);
    }

    private static Dictionary<Tuple<long, long>, long> GetSoilToFertilizerMap()
    {
        string input = real ?
            @"2037529808 755544791 28492175
786265521 51055407 490038659
4209265112 3740304131 26706989
1490754905 2631438697 34586718
1525341623 2263058012 73974507
1774661286 1921164897 262868522
3007460847 4099468601 72419286
3538443051 3849232192 250236409
1276304180 541094066 214450725
3913003454 3767011120 59428684
1599316130 2497793999 133644698
3079880133 3189897565 426082572
716780020 784036966 69485501
1732960828 0 41700458
2066021983 41700458 9354949
0 853522467 536962937
3972432138 2920584245 236832974
536962937 2337032519 100792490
4272174908 3826439804 22792388
3788679460 3615980137 124323994
2135345922 1446730958 474433939
637755427 2184033419 79024593
2884381438 4171887887 123079409
2075376932 2437825009 59968990
4235972101 2884381438 36202807
2609779861 1390485404 56245554
3505962705 3157417219 32480346" : @"0 15 37
37 52 2
39 0 15";

        return CreateMap(input);
    }

    private static Dictionary<Tuple<long, long>, long> GetFertilizerToWaterMap()
    {
        string input = real ?
            @"2573961911 2642757339 200829754
422488483 458127884 156893128
2774791665 2843587093 21928567
579381611 0 138939
1694098304 2191562282 184724096
3947192080 3245704277 276559964
972559115 1385376838 44883044
3491468609 3875486597 235886348
1207641311 1359799603 25577235
2301984556 4185647450 96253759
3943275635 3001826942 3916445
901810658 387379427 70748457
285678041 1226326727 103026012
0 775143786 192216167
3304976053 3522264241 95407569
2086394179 1904419031 20334668
2797747442 1694098304 113068417
2398238315 1924753699 175723596
4223752044 2376286378 71215252
192216167 1132864853 93461874
392041619 1329352739 30446864
736305758 967359953 44331476
3740421044 2865515660 136311282
388704053 771806220 3337566
2796720232 4111372945 1027210
3727354957 4281901209 13066087
1017442159 197180275 190199152
1878822400 4112400155 6703986
3400383622 2100477295 91084987
780637234 1011691429 121173424
1885526386 3674618804 200867793
2910815859 3617671810 56946994
2967762853 1807166721 97252310
3065015163 3005743387 239960890
1233218546 138939 197041336
2106728847 2447501630 195255709
579520550 615021012 156785208
3876732326 4119104141 66543309" : @"49 53 8
0 11 42
42 0 7
57 7 4";

        return CreateMap(input);
    }

    private static Dictionary<Tuple<long, long>, long> GetWaterToLightMap()
    {
        string input = real ?
            @"1713604757 2608139445 8097487
416889953 2083343492 58961768
1343227622 1417788674 170490633
2121243443 2549534549 51843959
3366419885 3580448237 174948163
2173087402 0 443149530
3030450490 4139087067 155880229
3186330719 3883823748 51661818
1513718255 1282808215 134980459
1075515973 1815631843 267711649
967716956 443149530 107799017
3237992537 3755396400 128427348
3825109759 3262058246 318389991
879862906 954959732 80165960
6760937 1035125692 247682523
2953403197 3185010953 77047293
960028866 2541846459 7688090
4143499750 3033543407 151467546
2749801696 3935485566 203601501
1648698714 1750725800 64906043
254443460 1588279307 162446493
475851721 550948547 404011185
3541368048 2749801696 283741711
1721702244 2142305260 399541199
0 2601378508 6760937" : @"88 18 7
18 25 70";

        return CreateMap(input);
    }

    private static Dictionary<Tuple<long, long>, long> GetLightToTemperatureMap()
    {
        string input = real ?
            @"72585995 0 6987206
3613700480 3337307014 262222114
3107305066 2641039316 68521519
1346057837 1130104209 332811266
1789578875 2709560835 110508022
1678869103 3335693412 1613602
4221249401 2413595122 73717895
1283830508 2273268756 62227329
2654986608 3645211688 211881713
1900086897 3599529128 45682560
866613618 2487313017 153726299
1945769457 1462915475 224589729
2170359186 2335496085 78099037
1020339917 866613618 263490591
3175826585 3857093401 437873895
1680482705 3226597242 109096170
2866868321 1687505204 240436745
3875922594 1927941949 345326807
2248458223 2820068857 406528385
0 6987206 72585995" : @"45 77 23
81 45 19
68 64 13";

        return CreateMap(input);
    }

    private static Dictionary<Tuple<long, long>, long> GetTemperatureToHumidityMap()
    {
        string input = real ?
            @"688557414 35571309 205276783
1344556852 4212560627 61250968
3617960922 3570339727 35153299
1798596843 4122755682 89804945
968601622 2840504203 289938389
3674269922 3395595703 19024769
2596309702 1911252584 80861179
2438263727 2065549417 13406592
1405807820 1518463561 392789023
617995054 866387965 70562360
1258540011 2078956009 86016841
3395085595 2301904832 149439673
4212818712 3130442592 82148584
1888401788 968601622 549861939
3005925897 2451344505 389159698
3544525268 1992113763 73435654
929405506 858843146 7544819
3868591817 3605493026 188507640
3653114221 4273811595 21155701
893834197 0 35571309
2588602301 3387888302 7707401
2677170881 3794000666 328755016
4057099457 3414620472 155719255
0 240848092 617995054
3693294691 3212591176 175297126
2451670319 2164972850 136931982": @"0 69 1
1 0 69";

        return CreateMap(input);
    }

    private static Dictionary<Tuple<long, long>, long> GetHumidityToLocationMap()
    {
        string input = real ?
            @"3586928302 2065932610 149219519
            709155282 323064563 19167863
            1359021687 3937987878 39697029
            4009761511 2966667063 138486244
            300370292 619673957 122108798
            1230327417 992326977 128694270
            1524755905 3624589590 105592616
            3736147821 2637585432 174385627
            1068077767 3730182206 162249650
            3120252652 1674438017 80200651
            434861301 1581164273 93273744
            3489613286 225749547 97315016
            2210476726 741782755 111642249
            1630348521 1811987842 123678973
            1456067890 2488658901 68688015
            2890604013 917374342 74952635
            2638888586 853425004 63949338
            528135045 2557346916 80238516
            2440002559 2215152129 198886027
            608373561 3105153307 100781721
            3910533448 342232426 99228063
            4148247755 3892431856 45556022
            2702837924 4107201207 187766089
            1754027494 1121021247 298601872
            2322118975 1948049026 117883584
            889864299 441460489 178213468
            3200453303 3559871086 36387444
            225749547 2414038156 74620745
            422479090 1935666815 12382211
            728323145 1419623119 161541154
            1398718716 1754638668 57349174
            2052629366 3977684907 129516300
            4193803777 3458707567 101163519
            2182145666 3596258530 28331060
            2965556648 2811971059 154696004
            3236840747 3205935028 252772539" : @"60 56 37
56 93 4";

        return CreateMap(input);
    }

    private record LongRange(long Start, long End) 
    {

    }
}
