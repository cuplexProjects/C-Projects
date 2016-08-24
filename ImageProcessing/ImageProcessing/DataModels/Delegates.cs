using System;

namespace ImageProcessing.DataModels
{
    public class ApplyFilterEventArgs : EventArgs
    {
        public int PercentComplete { get; set; }
        public string ProgressMessage { get; set; }
    }

    public class ApplyFilterProcessorEventArgs : EventArgs
    {
        public int ThreadIndex { get; set; }
        public int CurrentIndex { get; set; }
        public int MinIndex { get; set; }
        public int MaxIndex { get; set; }
        public int ProcessedPixelCount { get; set; }
    }

    public delegate void ImageFilterEventHandler(object sender, ApplyFilterEventArgs e);

    public delegate void ApplyFilterProcessorEventHandler(object sender, ApplyFilterProcessorEventArgs e);
}