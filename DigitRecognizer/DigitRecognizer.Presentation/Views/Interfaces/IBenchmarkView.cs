﻿using System;

namespace DigitRecognizer.Presentation.Views.Interfaces
{
    public interface IBenchmarkView
    {
        event EventHandler RunBenchmark;
        event EventHandler CancelBenchmark;

        bool IsBenchmarkRunning { get; set; }

        void PerformProgressStep();
        void SetAccuracy(int accuracy);
    }
}