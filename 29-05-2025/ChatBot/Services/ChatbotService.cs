using ChatBot.Models;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace ChatBot.Services
{
    public class ChatBotService
    {
        private List<FaqItem> _faqItems;
        private Dictionary<string, double[]> _tfidfVectors;
        private List<string> _vocabulary;
        private double[] _idfVector;

        public ChatBotService(string filePath)
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException("FAQ file not found.", filePath);

            var json = File.ReadAllText(filePath);
            Console.WriteLine("JSON length: " + json.Length);
            _faqItems = JsonSerializer.Deserialize<List<FaqItem>>(json)?
                .Where(f => !string.IsNullOrWhiteSpace(f.Question) && !string.IsNullOrWhiteSpace(f.Answer))
                .ToList() ?? new List<FaqItem>();

            if (_faqItems.Count == 0)
                throw new InvalidOperationException("FAQ file contains no valid entries.");

            BuildTfIdfVectors();
        }

        private void BuildTfIdfVectors()
        {
            var docs = _faqItems
                .Select(f => Tokenize(f.Question))
                .ToList();

            // Build consistent vocabulary
            _vocabulary = docs.SelectMany(d => d).Distinct().ToList();

            // Compute IDF once for all docs
            _idfVector = InverseDocumentFrequency(docs);

            _tfidfVectors = new Dictionary<string, double[]>();

            for (int i = 0; i < _faqItems.Count; i++)
            {
                var tf = TermFrequency(docs[i]);
                var tfidf = tf.Zip(_idfVector, (a, b) => a * b).ToArray();
                _tfidfVectors[_faqItems[i].Question] = tfidf;
            }
        }

        public string GetBestAnswer(string userInput)
        {
            if (string.IsNullOrWhiteSpace(userInput))
                return "Please enter a valid question.";

            var inputTokens = Tokenize(userInput);
            var tf = TermFrequency(inputTokens);
            var inputVector = tf.Zip(_idfVector, (a, b) => a * b).ToArray();

            double bestScore = 0;
            string bestQuestion = null;

            foreach (var kvp in _tfidfVectors)
            {
                var sim = CosineSimilarity(inputVector, kvp.Value);
                if (sim > bestScore)
                {
                    bestScore = sim;
                    bestQuestion = kvp.Key;
                }
            }

            return _faqItems.FirstOrDefault(f => f.Question == bestQuestion)?.Answer
                   ?? "I'm not sure how to answer that.";
        }

        private List<string> Tokenize(string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return new List<string>();
            return Regex.Split(text.ToLower(), @"\W+").Where(t => !string.IsNullOrWhiteSpace(t)).ToList();
        }

        private double[] TermFrequency(List<string> doc)
        {
            var vector = new double[_vocabulary.Count];
            for (int i = 0; i < _vocabulary.Count; i++)
            {
                string word = _vocabulary[i];
                int count = doc.Count(w => w == word);
                vector[i] = doc.Count == 0 ? 0 : (double)count / doc.Count;
            }
            return vector;
        }

        private double[] InverseDocumentFrequency(List<List<string>> docs)
        {
            var vector = new double[_vocabulary.Count];
            int totalDocs = docs.Count;

            for (int i = 0; i < _vocabulary.Count; i++)
            {
                string word = _vocabulary[i];
                int containingDocs = docs.Count(d => d.Contains(word));
                vector[i] = Math.Log((double)totalDocs / (1 + containingDocs));
            }

            return vector;
        }

        private double CosineSimilarity(double[] v1, double[] v2)
        {
            double dot = 0.0, mag1 = 0.0, mag2 = 0.0;
            for (int i = 0; i < v1.Length; i++)
            {
                dot += v1[i] * v2[i];
                mag1 += v1[i] * v1[i];
                mag2 += v2[i] * v2[i];
            }

            return (mag1 == 0 || mag2 == 0) ? 0 : dot / (Math.Sqrt(mag1) * Math.Sqrt(mag2));
        }
    }
}
