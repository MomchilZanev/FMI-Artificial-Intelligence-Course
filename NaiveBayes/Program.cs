namespace NaiveBayes
{
    public class Program
    {
        static void Main(string[] args)
        {
            string dataFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Data", "house-votes-84.data");

            List<List<bool>> votingRecords = new List<List<bool>>();
            foreach (string vote in File.ReadAllText(dataFilePath).Split('\n', StringSplitOptions.RemoveEmptyEntries))
            {
                votingRecords.Add(translateVote(vote));
            }

            double totalAccuracy = 0;
            for (int i = 0; i < 10; ++i)
            {
                var dataSets = splitSetInTwo(votingRecords, 0.1);
                List<List<bool>> testingSet = dataSets.Key;
                List<List<bool>> trainingSet = dataSets.Value;

                double accuracy = getAccuracy(testingSet, trainingSet);

                Console.WriteLine(string.Format("Test {0}, accuracy: {1:P2};", i + 1, accuracy));
                totalAccuracy += accuracy;
            }

            Console.WriteLine(string.Format("Average model accuracy: {0:P2};", totalAccuracy / 10.0));
        }

        // return true = republican, false = democrat
        static bool predictParty(List<bool> vote, List<List<int>> republicanVotes, List<List<int>> democratVotes)
        {
            int republicansCount = republicanVotes[0][0] + republicanVotes[1][0];
            int democratsCount = democratVotes[0][0] + democratVotes[1][0];

            // Laplace smoothing + logarithmic probability
            int lambda = 1;
            int A = 2; // Two options for each vote: yea/nay
            double republicanProbability = Math.Log((double)republicansCount + lambda / (republicansCount + democratsCount + (A * lambda)));
            double democratProbability = Math.Log((double)democratsCount + lambda / (republicansCount + democratsCount + (A * lambda)));
            for (int i = 1; i < vote.Count; i++)
            {
                int index = vote[i] ? 0 : 1; // yea or nay
                republicanProbability += Math.Log((double)(republicanVotes[index][i - 1] + lambda) / (republicansCount + (A * lambda)));
                democratProbability += Math.Log((double)(democratVotes[index][i - 1] + lambda) / (democratsCount + (A * lambda)));
            }

            return republicanProbability >= democratProbability;
        }

        static double getAccuracy(List<List<bool>> testingSet, List<List<bool>> trainingSet)
        {
            // [ [ yea ], [ nay ] ]
            List<List<int>> republicanVotes = new List<List<int>>() { Enumerable.Repeat(0, 16).ToList(), Enumerable.Repeat(0, 16).ToList() };
            List<List<int>> democratVotes = new List<List<int>>() { Enumerable.Repeat(0, 16).ToList(), Enumerable.Repeat(0, 16).ToList() };
            foreach (List<bool> vote in trainingSet)
            {
                for (int i = 1; i < vote.Count; ++i)
                {
                    int index = vote[i] ? 0 : 1; // yea or nay
                    if (vote[0]) // is republican
                        republicanVotes[index][i - 1]++;
                    else
                        democratVotes[index][i - 1]++;
                }
            }

            int correctPredictions = 0;
            foreach (List<bool> vote in testingSet)
            {
                bool party = vote[0];
                bool predictedParty = predictParty(vote, republicanVotes, democratVotes);

                if (party == predictedParty)
                    correctPredictions++;
            }

            return (double)correctPredictions / testingSet.Count;
        }

        static KeyValuePair<List<List<bool>>, List<List<bool>>> splitSetInTwo(List<List<bool>> orignalSet, double proportion)
        {
            List<List<bool>> secondarySet = new List<List<bool>>();
            foreach (List<bool> originalVote in orignalSet)
            {
                List<bool> copiedVote = new List<bool>();
                foreach (bool value in originalVote)
                {
                    copiedVote.Add(value);
                }
                secondarySet.Add(copiedVote);
            }

            Random random = new Random();
            int primarySetSize = (int)(orignalSet.Count * proportion);
            List<List<bool>> primarySet = new List<List<bool>>();
            for (int i = 0; i < primarySetSize; ++i)
            {
                int randomIndex = random.Next(secondarySet.Count);
                List<bool> copiedVote = new List<bool>();
                foreach (bool value in secondarySet[randomIndex])
                {
                    copiedVote.Add(value);
                }
                primarySet.Add(copiedVote);
                secondarySet.RemoveAt(randomIndex);
            }

            return new KeyValuePair<List<List<bool>>, List<List<bool>>>(primarySet, secondarySet);
        }

        // Element at index 0 is the class. true => republican, false => democrat.
        // Elements 1 through 16 are features/voting positions.
        // Unknown positions '?' are translated to a random position.
        static List<bool> translateVote(string vote)
        {
            List<string> tokens = vote.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList();

            Random random = new Random();
            List<bool> result = new List<bool>();
            foreach (string token in tokens)
            {
                if (token == "republican" || token == "y")
                    result.Add(true);
                else if (token == "?")
                    result.Add(random.Next() > (Int32.MaxValue / 2));
                else
                    result.Add(false);
            }

            return result;
        }
    }
}