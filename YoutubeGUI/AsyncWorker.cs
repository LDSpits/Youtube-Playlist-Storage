using System;
using System.ComponentModel;

namespace YoutubeGUIWPF
{
    public class AsyncWorker
    {
        private static BackgroundWorker worker;

        public static void DoWork(DoWorkEventHandler work, RunWorkerCompletedEventHandler onComplete)
        {
            worker = new BackgroundWorker();

            worker.DoWork += work;
            worker.RunWorkerCompleted += onComplete;

            worker.RunWorkerAsync();
        }

        public static void DoWork(DoWorkEventHandler work, RunWorkerCompletedEventHandler onComplete,object arguments)
        {
            worker = new BackgroundWorker();

            worker.DoWork += work;
            worker.RunWorkerCompleted += onComplete;

            worker.RunWorkerAsync(arguments);
        }

        public static void DoWork(DoWorkEventHandler work, RunWorkerCompletedEventHandler onComplete,ProgressChangedEventHandler onProgress, object arguments)
        {
            worker = new BackgroundWorker();

            worker.WorkerReportsProgress = true;

            worker.DoWork += work;
            worker.RunWorkerCompleted += onComplete;
            worker.ProgressChanged += onProgress;

            worker.RunWorkerAsync(arguments);
        }


    }
}
