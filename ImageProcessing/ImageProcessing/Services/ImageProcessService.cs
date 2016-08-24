using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ImageProcessing.DataModels;

namespace ImageProcessing.Services
{
    public class ImageProcessService : IDisposable
    {
        private static ImageProcessService _instance;
        private readonly object _threadLockObject = new object();
        private readonly Dictionary<int, int> _updateProgressDictionary;
        private ProcessFilter[] processFilterArray;
        private WaitHandle[] waitHandleArray;
        private Bitmap _outputBitmap;
        private Bitmap _sourceBitmap;
        private SourceDataBase _sourceDataBase;

        public static ImageProcessService Instance
        {
            get { return _instance ?? (_instance = new ImageProcessService()); }
        }

        public bool IsRunning { get; private set; }
        public bool ImageLoaded { get; private set; }

        private ImageProcessService()
        {
            _updateProgressDictionary = new Dictionary<int, int>();
        }

        public void Dispose()
        {
            _instance = null;
        }

        public event ImageFilterEventHandler OnProgressUpdate;
        public event EventHandler OnProcessComplete;

        public void LoadImage(string path)
        {
            Image sourceImage = Image.FromFile(path);
            _sourceBitmap = new Bitmap(sourceImage);
            ImageLoaded = true;
        }

        public bool SaveImage(string path)
        {
            if (_outputBitmap == null)
                return false;

            _outputBitmap.Save(path);
            return true;
        }

        public Size GetOriginalImageSize()
        {
            if (_sourceBitmap == null)
                return new Size(0, 0);

            return new Size(_sourceBitmap.Width, _sourceBitmap.Height);
        }

        public async Task ApplyFilter(IImageFilter imageFilter, EdgeHandling edgeHandling)
        {
            if (IsRunning || _sourceBitmap == null) return;
            IsRunning = true;
            await Task.Run(() => { ApplyFilterMultithreaded(imageFilter, edgeHandling); });
            IsRunning = false;
        }

        private void ApplyFilterMultithreaded(IImageFilter imageFilter, EdgeHandling edgeHandling)
        {
            int numberOfThreads = Environment.ProcessorCount;

            // ReSharper disable once InconsistentlySynchronizedField
            _updateProgressDictionary.Clear();

            try
            {
                _sourceDataBase = new SourceDataBase(_sourceBitmap, edgeHandling);

                int imgPixelSize = _sourceDataBase.NumberOfPixels;

                int workSize = Convert.ToInt32(Math.Floor(imgPixelSize / (double) numberOfThreads));
                int remaningPixels = imgPixelSize % numberOfThreads;


                //Split work into number of threads awailable for this system
                var doneEvents = new ManualResetEvent[numberOfThreads];
                processFilterArray = new ProcessFilter[numberOfThreads];
                
                int startIndex = 0;

                for (int i = 0; i < numberOfThreads; i++)
                {
                    doneEvents[i] = new ManualResetEvent(false);
                    ProcessFilter processFilter;
                    var sourceData = new SourceData(_sourceDataBase);

                    if (remaningPixels > 0 && i == numberOfThreads - 1)
                        processFilter = new ProcessFilter(sourceData, imageFilter, startIndex, startIndex + workSize + remaningPixels, doneEvents[i]);
                    else
                        processFilter = new ProcessFilter(sourceData, imageFilter, startIndex, startIndex + workSize, doneEvents[i]);

                    processFilter.OnProgressUpdate += processFilter_OnProgressUpdate;

                    startIndex += workSize;
                    processFilterArray[i] = processFilter;
                    ThreadPool.QueueUserWorkItem(processFilter.ThreadPoolCallback, i);
                }

                waitHandleArray = new WaitHandle[doneEvents.Length];
                for (int i = 0; i < doneEvents.Length; i++)
                    waitHandleArray[i] = doneEvents[i];

                // Wait for all threads in pool to complete.
                WaitHandle.WaitAll(waitHandleArray);
                _outputBitmap = _sourceDataBase.GetOutputBitmap();
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                Console.WriteLine(message);
            }

            processFilterArray = null;
            waitHandleArray = null;
            IsRunning = false;
            
            if (OnProcessComplete != null)
                OnProcessComplete.Invoke(this, new EventArgs());
        }

        private void processFilter_OnProgressUpdate(object sender, ApplyFilterProcessorEventArgs e)
        {
            if (OnProgressUpdate == null) return;

            lock (_threadLockObject)
            {
                if (_updateProgressDictionary.ContainsKey(e.ThreadIndex))
                    _updateProgressDictionary[e.ThreadIndex] = e.ProcessedPixelCount;
                else
                    _updateProgressDictionary.Add(e.ThreadIndex, e.ProcessedPixelCount);

                int pxSum = _updateProgressDictionary.Values.Sum(p => p);
                int percentComplete = Convert.ToInt32((pxSum / (double) _sourceDataBase.NumberOfPixels) * 100);
                OnProgressUpdate.Invoke(this, new ApplyFilterEventArgs {PercentComplete = percentComplete, ProgressMessage = ""});
            }
        }

        public Bitmap GetOutputBitmap()
        {
            return _outputBitmap;
        }

        public void ClearImage()
        {
            _sourceBitmap = null;
            ImageLoaded = false;
        }

        public async Task CancelApplyFilter()
        {
            if (!IsRunning && (processFilterArray == null || waitHandleArray == null)) return;
            foreach (ProcessFilter filter in processFilterArray)
                filter.Stop();

            await Task.Run(() => WaitHandle.WaitAll(waitHandleArray));
        }
    }


    public class ProcessFilter
    {
        private const int MaxProgressUpdates = 100;
        private readonly ManualResetEvent _doneEvent;
        private readonly IImageFilter _filter;
        private readonly int _fromPixel;
        private readonly SourceData _sourceData;
        private readonly int _toPixel;
        private bool _runThreadPoolCallback;

        public ProcessFilter(SourceData sourceData, IImageFilter filter, int fromPixel, int toPixel, ManualResetEvent doneEvent)
        {
            _sourceData = sourceData;
            _filter = filter;
            _fromPixel = fromPixel;
            _toPixel = toPixel;
            _doneEvent = doneEvent;
        }

        public event ApplyFilterProcessorEventHandler OnProgressUpdate;

        public void ThreadPoolCallback(object threadContext)
        {
            int threadIndex = (int) threadContext;
            _runThreadPoolCallback = true;
            ProcessPixels(threadIndex);
            _doneEvent.Set();
        }

        public void Stop()
        {
            _runThreadPoolCallback = false;
        }

        public void ProcessPixels(int threadIndex)
        {
            int loopCount = 0;
            int pixelCount = _toPixel - _fromPixel;
            int pbarModValue = pixelCount / MaxProgressUpdates;

            for (int i = _fromPixel; i < _toPixel && _runThreadPoolCallback; i++)
            {
                loopCount++;
                _sourceData.SetPixelIndex(i);
                Pixel p = _filter.Map(_sourceData);
                _sourceData.SetPixel(p, _sourceData.Offset_X, _sourceData.Offset_Y);

                // We dont want to invoke OnProgressUpdate for every pixel since that would be a major bottleneck
                if (OnProgressUpdate == null || i % pbarModValue != 0) continue;
                OnProgressUpdate.Invoke(this, new ApplyFilterProcessorEventArgs {ThreadIndex = threadIndex, CurrentIndex = i, MinIndex = _fromPixel, MaxIndex = _toPixel, ProcessedPixelCount = loopCount});
            }
        }
    }
}