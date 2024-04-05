using Azure;
using Azure.AI.Vision.ImageAnalysis;
using System.Text.RegularExpressions;

namespace ReadNumbers
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string AzureKey = "";
            string AzureEndpoint = "https://westeurope.api.cognitive.microsoft.com/";
            string[] images = new string[] { "Images\\img2_1.jpg", "Images\\img2_2.jpg", "Images\\img2_3.png" };
            ImageAnalysisClient client = new ImageAnalysisClient(
                 new Uri(AzureEndpoint),
                 new AzureKeyCredential(AzureKey));

            foreach (var imgPath in images)
            {
                var data = BinaryData.FromBytes(File.ReadAllBytes(imgPath));
                ImageAnalysisResult result = client.Analyze(
                    data,
                    VisualFeatures.Caption | VisualFeatures.Read,
                    new ImageAnalysisOptions { GenderNeutralCaption = true });
                List<string> outputs = new List<string>();
                var regex = new Regex(@"^\d+$");

                foreach (DetectedTextBlock block in result.Read.Blocks)
                    foreach (DetectedTextLine line in block.Lines)
                    {
                        var number = line.Text.Replace(" ", string.Empty);
                        if (regex.IsMatch(number))
                        {
                            outputs.Add(number);
                        }
                    }
                Console.WriteLine($"Numbers found in the image: {imgPath}");
                if (outputs.Any())
                {
                    foreach (var output in outputs)
                    {
                        Console.WriteLine(output);
                    }
                }
                else
                {
                    Console.WriteLine($"No numbers found in the image {imgPath}");
                }
                Console.WriteLine();
            }
            Console.ReadLine();
        }
    }
}
