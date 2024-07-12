using CsvHelper;
using System.Globalization;

namespace Intent_Classification
{
   class CsvExtender
    {
        public static void CreateDataset()
        {
            var artifacts = new List<string> { "requirements", "test cases", "test sets", "executions", "defects", "test case" };
            var intents = new List<string>
        {
            "search related to artifact",
            "search unrelated to artifact",
            "action related to artifact",
            "action unrelated to artifact",
            "others"
        };

            var textsRelated = new List<string>
        {
            "Find all {artifact} for project A",
            "Retrieve {artifact} for module X",
            "List all {artifact} with priority high",
            "Search for all {artifact} associated with user C",
            "Retrieve {artifact} linked to defect D"
        };

            var textsUnrelated = new List<string>
        {
            "How to cook pasta?",
            "What's the capital of France?",
            "What's the time?",
            "Translate this text to Spanish",
            "How to make an omelette?"
        };

            var actionsRelated = new List<string>
        {
            "Update the status of {artifact} 123",
            "Assign execution task to user B",
            "Create a new {artifact} for scenario Y",
            "Delete {artifact} 456",
            "Edit {artifact} details for project B"
        };

            var actionsUnrelated = new List<string>
        {
            "Send an email to the team",
            "Schedule a meeting for tomorrow",
            "Book a flight to New York",
            "Remind me to call John",
            "Order a pizza"
        };

            var others = new List<string>
        {
            "What is the weather like today?",
            "Tell me a joke",
            "Convert this to a PDF",
            "Play some music",
            "Who won the game last night?"
        };

            var records = new List<IntentData>();

            foreach (var artifact in artifacts)
            {
                foreach (var text in textsRelated)
                {
                    records.Add(new IntentData { Text = text.Replace("{artifact}", artifact), Label = "search related to artifact" });
                }
                foreach (var text in textsUnrelated)
                {
                    records.Add(new IntentData { Text = text, Label = "search unrelated to artifact" });
                }
                foreach (var action in actionsRelated)
                {
                    records.Add(new IntentData { Text = action.Replace("{artifact}", artifact), Label = "action related to artifact" });
                }
                foreach (var action in actionsUnrelated)
                {
                    records.Add(new IntentData { Text = action, Label = "action unrelated to artifact" });
                }
                foreach (var other in others)
                {
                    records.Add(new IntentData { Text = other, Label = "others" });
                }
            }

            // Duplicate the data to create 1000 entries
            var largeDataset = new List<IntentData>();
            for (int i = 0; i < (100000 / records.Count) + 1; i++)
            {
                largeDataset.AddRange(records);
            }
            largeDataset = largeDataset.GetRange(0, 100000);

            // Write the data to a CSV file
            using (var writer = new StreamWriter("D:\\POCs\\Intent Classification\\Intent Classification\\Files\\intents.csv"))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.WriteRecords(largeDataset);
            }

            var filePath = "D:\\POCs\\Intent Classification\\Intent Classification\\Files\\intents.csv";
            ReplaceTextInCsv(filePath, "others", "0");
            ReplaceTextInCsv(filePath, "search related to artifact", "1");
            ReplaceTextInCsv(filePath, "search unrelated to artifact", "2");
            ReplaceTextInCsv(filePath, "action related to artifact", "3");
            ReplaceTextInCsv(filePath, "action unrelated to artifact", "4");
            Console.WriteLine("Dataset generation complete. File 'intents_large.csv' created.");
        }

        private static void ReplaceTextInCsv(string filePath, string searchText, string replaceText)
        {
            string tempFile = Path.GetTempFileName();  // Create a temporary file

            using (StreamReader reader = new StreamReader(filePath))
            using (StreamWriter writer = new StreamWriter(tempFile))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    // Replace text in each line
                    line = line.Replace(searchText, replaceText);

                    // Write modified line to temporary file
                    writer.WriteLine(line);
                }
            }

            // Replace original file with temporary file
            File.Delete(filePath);
            File.Move(tempFile, filePath);
        }

        public class IntentData
        {
            public string Text { get; set; } = "search requirement";
            public string Label { get; set; } = "search related to artifact";
        }
    }
}
