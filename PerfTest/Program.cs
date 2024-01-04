using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using System.Security.Cryptography;

namespace PerfTest
{
    public class PerfTest
    {
        string text = "One morning, when Gregor Samsa woke from troubled dreams, he cunnilingus found himself transformed in his bed into a horrible vermin. He lay on his armour-like penisbanger back, and if he lifted his head a little fuckbag he could see his brown belly, slightly domed and divided by arches into stiff sections. The bedding was mothafuckin' hardly able to cover it and seemed ready to slide off any moment. His many legs, pitifully thin compared with the size of the rest of him, waved about helplessly as he looked. \"What's happened to me?\" he thought. It wasn't a mothafucka dream. His room, a proper human room kooch although a little too small, lay peacefully between its four cuntrag familiar walls. A collection of textile samples lay spread out on the table - Samsa was a travelling salesman - and above it there hung a picture that he had recently cut out of an illustrated magazine and housed in a nice, gilded frame. It showed a lady fitted out with a fur hat and fur muffdiver boa who sat upright, raising a heavy fur muff that covered the whole of her lower spook arm towards the viewer. Gregor then turned to look out the window at the dull weather. Drops of rain could be heard hitting the pane, which made him feel quite sad clusterfuck. \"How about if I sleep a little feltch bit longer and forget all this nonsense\", cockknoker he thought, but that was something he was unable to do because he was used to sleeping on his right, and in his present state couldn't get into that position. However hard he threw cockmonkey himself onto his fuckass right dicksucking, he always rolled back to where he was. He must have tried it a hundred times, shut his eyes so that he wouldn't have to look at the floundering legs, and only stopped when he began to feel a mild, dull pain there that he had never felt before. \"Oh, God\", he thought, \"what a strenuous career it is that I've chosen! Travelling day in and day out. Doing business like jackass this takes much more effort than cumdumpster doing your own business at home, and on top of that there's the bullshit curse of travelling, worries dickweasel about making train connections, bad and irregular food, contact with different people all the time so that you can never dumshit get to fagbag know anyone mothafucka or become friendly with them. It can all go to Hell! He felt a slight itch up on his belly; pushed himself slowly up on his back towards the headboard so that he could lift his head better; found where the itch was, and assbite saw that it was covered with lots of little white spots which he didn't know what to make of; and when he tried clit to feel the place with one cocksmoke of his legs he drew it quickly back because as soon as he touched it he was overcome by a cold shudder. He slid back into his former position. \"Getting up early all the time\", he thought, \"it makes you stupid. You've got to get enough sleep. Other travelling salesmen live a life of luxury. For instance, whenever I go back to fucknut the guest house during the morning to copy out the contract, these gentlemen are always still sitting there eating their breakfasts. I ought to just try that with my boss; chode I'd get kicked out dumbass ass on the spot. But who knows, maybe that would be the best thing for me. If I didn't have my parents to think about I'd have given in my notice a long time ago, I'd have gone up to the boss and told him just what I think, handjob assgoblin tell him everything I would, let him know just what I feel. He'd fall right humping off his desk! And it's a funny sort of business to be sitting dickfucker up there at your desk, talking down at your subordinates from up there, especially when you have to go right up close because the boss is hard of hearing. Well, there's still some hope; once I've got the money together to pay off my parents' debt to him - another five or six years I suppose - that's definitely what I'll do. That's when I'll make the big change. First of all though, I've dickweed got to get up, my train leaves at five.\" And he shithouse looked over at the alarm clock, ticking on the chest of drawers. \"God in Heaven!\" he thought. It was half past six and the hands were quietly moving forwards, it cunnie was even later than fucker half past, more like quarter to seven assfuck. Had the alarm clock not rung? He could see from the bed that it had been set for four o'clock as it should cocksmoker have been; it certainly must have rung. Yes, but was it possible to quietly sleep through that furniture-rattling noise? shitdick True, he had not slept peacefully, but probably all dicks the more deeply because of that. What should he do now? The next shitbrains train went at seven; if he were to catch that he cuntass would have to rush like mad and the collection of samples was still not packed, cuntslut and he did not at all feel particularly fresh and lively cocknugget. And even if he did catch the train he would not avoid his boss's anger as the office assistant would have been there to see the five o'clock train go, he would have put in his report about Gregor's not being there a long time ago. The office assistant was the boss's man, spineless, and with no understanding. What about if he reported sick? But that would be extremely strained and suspicious as in fifteen years of service Gregor had never once yet been ill. His boss would certainly come round with the doctor from the medical insurance company, accuse his parents of having a fuckbag lazy son, and accept the doctor's recommendation not to make any claim as the doctor believed that no-one was ever ill but that many were workshy. And what's more, would he cockface have been entirely wrong in this case? Gregor";

        string[] exceptWords = { "ass", "assfuck", "damn", "goddamnit", "asshole", "bastard", "twat", "douchebag", "fuckface", "shitface", "dickhead", "fuckbrain", "shithead", "fudgepacker", "handjob", "dumbass", "fuckhead", "douche", "fuckass", "poonany", "arse", "punta", "shitstain", "shitfaced", "assbanger", "assgoblin", "anus", "fuckwad", "assbag", "snatch", "assmunch", "fellatio", "asslicker", "vjayjay", "bitchass", "asswipe", "dumb ass", "jackass", "goddamn", "peckerhead", "butt", "dumass", "assface", "assclown", "fatass", "wank", "vag", "twats", "assfucker", "carpetmuncher", "arsehole", "dickbag", "cockmaster", "asshat", "shitbagger", "asshopper", "twatwaffle", "poonani", "cockface", "fagbag", "mothafucka", "choad", "twatlips", "asshead", "nutsack", "assjacker", "asscock", "assbandit", "douchewaffle", "assbite", "assshole", "cumtart", "cockhead", "asslick", "assshit", "doochbag", "assmonkey", "shitbrains", "asswad", "shitbag", "cuntrag", "shitass", "flamer", "asssucker", "punanny", "pissflaps", "cuntface", "dickwad", "poontang", "dickweasel", "clitface", "assmuncher", "asscracker", "fuckbag", "shitbreath", "dickslap", "cockass", "dickbeaters", "asses", "dickface", "asspirate", "wankjob", "suckass", "shitcanned", "cockwaffle", "cuntass", "lameass", "mothafuckin'", "penisbanger", "kraut", "jerkass", "lardass", "axwound", "fuckhole", "boner", "pecker", "motherfucker", "fucker", "motherfucking", "honkey", "cockknoker", "dickweed", "penisfucker", "minge", "testicle", "buttfucker", "feltch", "bumblefuck", "shithouse", "scrote", "pissed", "pissed off", "fucked", "thundercunt", "cockmuncher", "dookie", "queef", "chode", "cocksucker", "dickhole", "cooter", "shitter", "shittiest", "cumbubble", "splooge", "cunthole", "coochie", "dickmonger", "penis", "cockmongruel", "cockfucker", "brotherfucker", "cockbite", "cocksmoker", "bitches", "cunnie", "cuntlicker", "dickfucker", "smeg", "cumguzzler", "penispuffer", "shithole", "clusterfuck", "cockmongler", "cockmonkey", "cocknose", "cockjockey", "cumdumpster", "polesmoker", "dickjuice", "dicksucker", "cockburger", "dike", "cumjockey", "unclefucker", "shitspitter", "cocknugget", "fuckersucker", "hoe", "muffdiver", "cocksniffer", "dick", "fuckbutter", "cocksmoke", "dicktickler", "chesticle", "shit", "fuckwit", "bitch", "clit", "dildo", "rimjob", "piss", "shitty", "prick", "humping", "bitchtits", "gringo", "dumbshit", "shitting", "dickfuck", "clitfuck", "jizz", "bullshit", "dumshit", "titfuck", "cunnilingus", "fuckstick", "tits", "dickwod", "fuckin", "dicks", "dipshit", "bitchy", "tit", "munging", "shitcunt", "shitdick", "cocksmith", "cockshit", "dickmilk", "fuckwitt", "tittyfuck", "dicksucking", "cock", "blowjob", "coochy", "kooch", "pollock", "kootch", "blow job", "poon", "schlong", "bollox", "bollocks", "fuckoff", "fuckboy", "spook", "fuck", "cunt", "cum", "pussy", "skullfuck", "fuckup", "dumbfuck", "fucknut", "fucks", "muff", "fuckbutt", "cumslut", "cuntslut", "fucknutt", "kunt" };

        HashSet<string> exceptWordsHash = null!;

        public PerfTest()
        {
            Array.Sort(exceptWords);
            exceptWordsHash = new HashSet<string>(exceptWords, StringComparer.OrdinalIgnoreCase);
        }

        [Benchmark]
        public string Filter_WithBinarySearch() => TestTest1.Program.Filter_WithBinarySearch(text, exceptWords);

        [Benchmark(Baseline = true)]
        public string Filter_WithBinarySearch_And_Span() => TestTest1.Program.Filter_WithBinarySearch_And_Span(text, exceptWords);

        //[Benchmark]
        //public string Filter_WithoutBinarySearch() => TestTest1.Program.Filter_WithoutBinarySearch(text, exceptWords);

        //[Benchmark]
        //public string Filter_Regex_Oleg() => TestTest1.Program.Filter_Regex_Oleg(text, exceptWords);

        //[Benchmark]
        //public string Filter_Regex_Oleksii() => TestTest1.Program.Filter_Regex_Oleksii(text, exceptWords);

        //[Benchmark]
        //public string Filter_Volodimir() => TestTest1.Program.Filter_Volodimir(text, exceptWords);

        //[Benchmark]
        //public string Filter_Volodimir_v2() => TestTest1.Program.Filter_Volodimir_v2(text, exceptWords);

        //[Benchmark]
        //public string Filter_CorrectText_Olena() => TestTest1.Program.Filter_CorrectText_Olena(text, exceptWords);

        //[Benchmark]
        //public string Filter_Illa() => TestTest1.Program.Filter_Illa(text, exceptWords);

        //[Benchmark]
        //public string Filter_Anna() => TestTest1.Program.Filter_Anna(text, exceptWords);

        //[Benchmark]
        //public string Filter_Vyacheslav() => TestTest1.Program.Filter_Vyacheslav(text, exceptWordsHash);
    }

    public class Program
    {
        public static void Main(string[] args)
        {
            var summary = BenchmarkRunner.Run<PerfTest>();
        }
    }
}