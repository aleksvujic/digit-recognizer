﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using DigitRecognizer.Core.Data;
using DigitRecognizer.Core.Extensions;
using DigitRecognizer.Core.Utilities;
using DigitRecognizer.MachineLearning.Infrastructure.Models;
using DigitRecognizer.MachineLearning.Providers;
using DigitRecognizer.Presentation.Infrastructure;
using DigitRecognizer.Presentation.Services;
using DigitRecognizer.Presentation.Views.Interfaces;

namespace DigitRecognizer.Presentation.Presenters
{
    public class BenchmarkPresenter
    {
        #region MyRegion

        private readonly IBenchmarkView _benchmarkView;
        private readonly IMessageService _messageService;
        private readonly ILoggingService _loggingService;
        private CancellationTokenSource _cancellationTokenSource;

        #endregion

        #region Ctor

        public BenchmarkPresenter(IBenchmarkView benchmarkView, IMessageService messageService, ILoggingService loggingService)
        {
            _messageService = messageService;
            _loggingService = loggingService;
            _benchmarkView = benchmarkView;

            _benchmarkView.RunBenchmark += OnRunBenchmark;

            _benchmarkView.CancelBenchmark += OnCancelBenchmark;

            _benchmarkView.IsBenchmarkRunning = false;
        }

        #endregion

        #region Methods

        private async void OnRunBenchmark(object sender, EventArgs e)
        {
            // TODO: Refactor this code to use a globally available model

            var dlg = new OpenFileDialog
            {
                Multiselect = true
            };

            dlg.ShowDialog();

            IPredictionModel model = ClusterPredictionModel.FromFiles(dlg.FileNames);

            _cancellationTokenSource = new CancellationTokenSource();

            CancellationToken token = _cancellationTokenSource.Token;

            token.Register(() => { _benchmarkView.IsBenchmarkRunning = false; });

            try
            {
                await Task.Run(() => {
                    RunBenchmark(model);
                }, token);
            }
            catch (Exception exception)
            {

                _loggingService.Log(exception);

                _messageService.ShowMessage("");
            }
        }

        private void RunBenchmark(IPredictionModel model)
        {
            _benchmarkView.IsBenchmarkRunning = true;

            var provider = new BatchDataProvider(DirectoryHelper.TestLabelsPath, DirectoryHelper.TestImagesPath, 100);

            var acc = 0;

            for (var i = 0; i < 100; i++)
            {
                if (!_benchmarkView.IsBenchmarkRunning)
                {
                    break;
                }

                MnistImageBatch data = provider.GetData();

                List<double[]> predictions = data.Pixels.Select(model.Predict).ToList();

                acc += data.Labels.Where((t, j) => t == predictions[j].ArgMax()).Count();

                _benchmarkView.PerformProgressStep();
            }

            _benchmarkView.SetAccuracy(acc);

            _benchmarkView.IsBenchmarkRunning = false;
        }

        private void OnCancelBenchmark(object sender, EventArgs e)
        {
            _cancellationTokenSource.Cancel();
        }
        
        #endregion
    }
}