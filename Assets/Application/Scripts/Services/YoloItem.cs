using System.Collections.Generic;
using System.Linq;
using Unity.Barracuda;
using UnityEngine;

namespace YoloHolo.Services
{
    public class YoloItem
    {
        public Vector2 Center { get; }

        public Vector2 Size { get; }

        public Vector2 TopLeft { get; }

        public Vector2 BottomRight { get; }

        public float Confidence { get; }

        public string MostLikelyObject { get; }

        public YoloItem(Tensor tensorData, int boxIndex, IYoloClassTranslator translator)
        {
            Center = new Vector2(tensorData[0, 0, boxIndex, 0], tensorData[0, 0, boxIndex, 1]);
            Size = new Vector2(tensorData[0, 0, boxIndex, 2], tensorData[0, 0, boxIndex, 3]);
            TopLeft = Center - Size / 2;
            BottomRight = Center + Size / 2;
            Confidence = tensorData[0, 0, boxIndex, 4];

            // Extracting class probabilities for all classes
            var classProbabilities = new List<float>();
            for (var i = 5; i < tensorData.channels; i++)
            {
                classProbabilities.Add(tensorData[0, 0, boxIndex, i]);
            }

            // Find the index of the class with the maximum probability
            var maxIndex = classProbabilities.IndexOf(classProbabilities.Max());

            // Get the name of the class using the translator
            MostLikelyObject = translator.GetName(maxIndex);
        }
    }
}
