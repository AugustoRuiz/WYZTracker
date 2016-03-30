using System;
using System.Collections.Generic;
using System.Text;
using OpenTK.Audio;
using OpenTK.Audio.OpenAL;
using System.Threading;
using System.Diagnostics;

namespace WYZTracker
{
    public class PlaybackStreamer : IDisposable
    {
        internal const int NUM_BUFFERS = 2;
        private const int NUM_CHANNELS = 2;
        private const int SOUND_FREQUENCY = 44100;

        private object _bufferInitLockObj = new object();

        private Thread _workerThread;

        private static AudioContext _audioContext;

        private bool _threadActive = false;
        private int[] _buffers;
        private int _audioSource;

        private int _bufferLengthInMs;
        private double[] _dataBuffer;

        public double Volume { get; set; }

        public event EventHandler<FillBufferEventArgs> FillBuffer;

        public PlaybackStreamer()
        {
            _audioSource = AL.GenSource();
            //Console.WriteLine("AL.GenSource: {0}", AL.GetErrorString(AL.GetError()));
            BufferLengthInMs = 20;
            initWorkerThread();
            Volume = 1.0;
        }

        public static bool IsAudioAvailable()
        {
            bool result = false;
            try
            {
                IList<string> devices = OpenTK.Audio.AudioContext.AvailableDevices;
                result = true;
            }
            catch (Exception e)
            {
                Logger.Log(e.ToString());
            }
            return result;
        }

        public static bool InitializeAudio()
        {
            bool initialized = false;
            try
            {
                _audioContext = new AudioContext();
                initialized = true;
            }
            catch (Exception ex)
            {
                Logger.Log(ex.ToString());
            }
            return initialized;
        }

        public static void StopAudio()
        {
            if (_audioContext != null)
            {
                _audioContext.Dispose();
                _audioContext = null;
            }
        }

        /// <summary>
        /// Buffer length in Ms.
        /// </summary>
        public int BufferLengthInMs
        {
            get
            {
                return _bufferLengthInMs;
            }
            set
            {
                if (value != _bufferLengthInMs)
                {
                    _bufferLengthInMs = value;
                    initBuffer();
                }
            }
        }

        private void initBuffer()
        {
            long bufLength = (long)((long)SOUND_FREQUENCY * (long)NUM_CHANNELS * ((double)_bufferLengthInMs / 1000));

            lock (_bufferInitLockObj)
            {
                _dataBuffer = new double[bufLength];

                if (_buffers != null)
                {
                    // Ensure all buffers are played at least once
                    if (AL.GetSourceState(_audioSource) != ALSourceState.Playing)
                    {
                        AL.SourcePlay(_audioSource);
                        //Console.WriteLine("AL.SourcePlay: {0}", AL.GetErrorString(AL.GetError()));
                    }

                    int processedBuffers;
                    int queuedBuffers;
                    do
                    {
                        AL.GetSource(_audioSource, ALGetSourcei.BuffersProcessed, out processedBuffers);
                        //Console.WriteLine("AL.GetSource(BuffersProcessed): {0}", AL.GetErrorString(AL.GetError()));
                        if (processedBuffers > 0)
                        {
                            AL.SourceUnqueueBuffer(_audioSource);
                        }
                        AL.GetSource(_audioSource, ALGetSourcei.BuffersQueued, out queuedBuffers);
                        //Console.WriteLine("AL.GetSource(BuffersQueued): {0}", AL.GetErrorString(AL.GetError()));
                    } while (queuedBuffers> 0);

                    AL.SourceStop(_audioSource);
                    //Console.WriteLine("AL.SourceStop: {0}", AL.GetErrorString(AL.GetError()));

                    AL.DeleteBuffers(_buffers);
                    //Console.WriteLine("AL.DeleteBuffers: {0}", AL.GetErrorString(AL.GetError()));

                    _buffers = null;
                }

                _buffers = AL.GenBuffers(NUM_BUFFERS);
                //Console.WriteLine("AL.GenBuffers: {0}", AL.GetErrorString(AL.GetError()));


                // Initial feed...
                for (int i = 0; i < NUM_BUFFERS; i++)
                {
                    OnFillBuffer(_dataBuffer);

                    UInt16[] buf = new UInt16[_dataBuffer.Length];
                    for (int bufPos = 0; bufPos < buf.Length; ++bufPos)
                    {
                        buf[bufPos] = (UInt16)(_dataBuffer[bufPos] * this.Volume * Int16.MaxValue);
                    }

                    AL.BufferData(_buffers[i], ALFormat.Stereo16, buf, _dataBuffer.Length, SOUND_FREQUENCY);
                    //Console.WriteLine("AL.BufferData: {0}", AL.GetErrorString(AL.GetError()));

                    AL.SourceQueueBuffer(_audioSource, _buffers[i]);
                    //Console.WriteLine("AL.SourceQueueBuffer: {0}", AL.GetErrorString(AL.GetError()));
                }
            }
        }

        /// <summary>
        /// Initialize the background worker thread.
        /// </summary>
        private void initWorkerThread()
        {
            _workerThread = new Thread(new ThreadStart(this.ThreadRun));
            _workerThread.Name = "PlaybackStreamer worker";
            _workerThread.IsBackground = true;

            _threadActive = true;
            _workerThread.Start();
        }

        /// <summary>
        /// Request an audio fill
        /// </summary>
        /// <param name="buffer"></param>
        protected void OnFillBuffer(double[] buffer)
        {
            EventHandler<FillBufferEventArgs> tmpCopy = this.FillBuffer;
            FillBufferEventArgs e = new FillBufferEventArgs(buffer);
            if (tmpCopy != null)
            {
                tmpCopy(this, e);
            }
        }

        /// <summary>
        /// Worker thread method.
        /// </summary>
        private void ThreadRun()
        {
            while (_threadActive)
            {
                Stopwatch w = Stopwatch.StartNew();

                lock (_bufferInitLockObj)
                {
                    int processedBuffersCount;
                    int bufferRef;

                    AL.GetSource(_audioSource, ALGetSourcei.BuffersProcessed, out processedBuffersCount);
                    //Console.WriteLine("AL.GetSource(BuffersProcessed): {0}", AL.GetErrorString(AL.GetError()));

                    while (processedBuffersCount > 0)
                    {
                        bufferRef = AL.SourceUnqueueBuffer(_audioSource);
                        //Console.WriteLine("AL.SourceUnqueueBuffer: {0}", AL.GetErrorString(AL.GetError()));

                        this.OnFillBuffer(_dataBuffer);

                        UInt16[] buf = new UInt16[_dataBuffer.Length];
                        for (int bufPos = 0; bufPos < buf.Length; ++bufPos)
                        {
                            buf[bufPos] = (UInt16)(_dataBuffer[bufPos] * this.Volume * Int16.MaxValue);
                        }

                        AL.BufferData(bufferRef, ALFormat.Stereo16, buf, buf.Length * 2, SOUND_FREQUENCY);
                        //Console.WriteLine("AL.BufferData: {0}", AL.GetErrorString(AL.GetError()));

                        AL.SourceQueueBuffer(_audioSource, bufferRef);
                        //Console.WriteLine("AL.SourceQueueBuffer: {0}", AL.GetErrorString(AL.GetError()));

                        AL.GetSource(_audioSource, ALGetSourcei.BuffersProcessed, out processedBuffersCount);
                    }

                    if (AL.GetSourceState(_audioSource) != ALSourceState.Playing)
                    {
                        AL.SourcePlay(_audioSource);
                        //Console.WriteLine("AL.SourcePlay: {0}", AL.GetErrorString(AL.GetError()));
                    }
                }
                w.Stop();
                if(w.ElapsedMilliseconds < BufferLengthInMs)
                {
                    Thread.Sleep((BufferLengthInMs - (int)w.ElapsedMilliseconds)/2);
                }
            }
        }

        #region IDisposable

        /// <summary>
        /// Releases resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Destructor.
        /// </summary>
        ~PlaybackStreamer()
        {
            Dispose(false);
        }

        /// <summary>
        /// Dispose. Release worker thread, semaphore and audio context.
        /// </summary>
        /// <param name="disposing">Indicates if the user called the dispose method or the finalizer did it.</param>
        protected virtual void Dispose(bool disposing)
        {
            // Release the worker thread and semaphore if needed.
            if (_threadActive)
            {
                _threadActive = false;
                _workerThread.Join(2000);
                _workerThread = null;
            }

            if (disposing)
            {
                if (_audioSource != 0)
                {
                    AL.SourceStop(_audioSource);
                    //Console.WriteLine("AL.SourceStop: {0}", AL.GetErrorString(AL.GetError()));

                    AL.DeleteSource(_audioSource);
                    //Console.WriteLine("AL.DeleteSource: {0}", AL.GetErrorString(AL.GetError()));

                    _audioSource = 0;
                }

                if (_buffers != null)
                {
                    AL.DeleteBuffers(_buffers);
                    //Console.WriteLine("AL.DeleteBuffers: {0}", AL.GetErrorString(AL.GetError()));

                    _buffers = null;
                }
            }
        }

        #endregion
    }
}
