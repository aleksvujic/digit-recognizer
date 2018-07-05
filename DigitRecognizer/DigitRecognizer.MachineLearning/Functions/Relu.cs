﻿using DigitRecognizer.Core.Utilities;
using DigitRecognizer.MachineLearning.Interfaces.Functions;

namespace DigitRecognizer.MachineLearning.Functions
{
    /// <summary>
    /// 
    /// </summary>
    public class Relu : IActivationFunction
    {
        public string Name => "Relu";

        public double[] Activate(double[] arr)
        {
            return MathUtilities.Relu(arr);
        }

        public double Derivative(double[] input, int current, int oneHot)
        {
            var result = input[current] >= 0 ? 1.0 : 0.0;

            return result;
        }
    }
}
