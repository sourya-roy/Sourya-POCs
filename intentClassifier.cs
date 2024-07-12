using Microsoft.ML;
using Microsoft.ML.Data;

namespace Intent_Classification
{
    class Classifier
    {
        public static void Predictor(string text)
        {
            // Create a new ML.NET environment
            string dataPath = "D:\\POCs\\Intent Classification\\Intent Classification\\Files\\intents.csv";
            var mlContext = new MLContext();

            // Load data
            var dataView = mlContext.Data.LoadFromTextFile<IntentData>(dataPath, hasHeader: true, separatorChar: ',');

            // Split data into training and test sets
            var trainTestSplit = mlContext.Data.TrainTestSplit(dataView, testFraction: 0.2);
            var trainingData = trainTestSplit.TrainSet;
            var testData = trainTestSplit.TestSet;

            // Define data preparation and model training pipeline
            var pipeline = mlContext.Transforms.Text.FeaturizeText("Features", nameof(IntentData.Text))
                .Append(mlContext.Transforms.Conversion.MapValueToKey("Label"))
                .Append(mlContext.MulticlassClassification.Trainers.SdcaMaximumEntropy("Label", "Features"))
                .Append(mlContext.Transforms.Conversion.MapKeyToValue("PredictedLabel"));

            // Train the model
            var model = pipeline.Fit(trainingData);

            // Evaluate the model
            var predictions = model.Transform(testData);
            var metrics = mlContext.MulticlassClassification.Evaluate(predictions);

            Console.WriteLine($"Log-loss: {metrics.LogLoss}");

            // Save the model
            mlContext.Model.Save(model, trainingData.Schema, "intent_classification_model.zip");

            // Load and test the model with a single prediction
            var loadedModel = mlContext.Model.Load("intent_classification_model.zip", out var modelInputSchema);
            var predictor = mlContext.Model.CreatePredictionEngine<IntentData, IntentPrediction>(loadedModel);

            var sampleData = new IntentData { Text = text };
            var prediction = predictor.Predict(sampleData);

            Console.WriteLine($"Predicted intent: {prediction.PredictedLabel}");
            Console.WriteLine($"\n\nOthers {0} \nSearch related to artifact {1} \nSearh unrelated to artifact {2} \nAction related to artifact {3} \nAction unrelated to artifact {4}");
        }

        public class IntentData
        {
            [LoadColumn(0)]
            public string Text { get; set; }

            [LoadColumn(1)]
            public uint Label { get; set; }  // Label as uint (0 to 4)
        }

        public class IntentPrediction
        {
            [ColumnName("PredictedLabel")]
            public uint PredictedLabel { get; set; }  // Predicted label as uint

            public string LabelText { get; set; }  // Corresponding label text
        }
    }
}
