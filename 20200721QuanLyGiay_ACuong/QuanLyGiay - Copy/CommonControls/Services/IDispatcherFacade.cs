using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace CommonControls
{
    public interface IDispatcherFacade
    {
        void AddToDispatcherQueue(Action workItem);
    }

    public class DispatcherFacade : IDispatcherFacade
    {
        readonly Dispatcher dispatcher;
        readonly ConcurrentDictionary<DispatcherOperation, object> operations;
        long operationsQueueCount;

        public int DispatcherQueueSize { get; set; } = 250;
        public int DispatcherMonitorRate { get; set; } = 5000;

        public DispatcherFacade(Dispatcher dispatcher)
        {
            this.dispatcher = Application.Current.MainWindow.Dispatcher;
            operations = new ConcurrentDictionary<DispatcherOperation, object>();
        }

        private void MonitorDispatcherQueue(long l)
        {
            if (operationsQueueCount > DispatcherQueueSize)
            {
                DoEvents(Application.Current);
                operations.Clear();
                Interlocked.Exchange(ref operationsQueueCount, 0);
            }
        }

        public void AddToDispatcherQueue(Action workItem)
        {
            //Interlocked.Increment(ref operationsQueueCount);
            //var operation = dispatcher.BeginInvoke(DispatcherPriority.Background, workItem);
            //operations.TryAdd(operation, null);
            //operation.Completed += (s, e) =>
            //{
            //    Interlocked.Decrement(ref operationsQueueCount);
            //    operations.TryRemove((DispatcherOperation)s, out object t);
            //};
            dispatcher.Invoke(workItem);
        }

        public void DoEvents(Application application)
        {
            DispatcherFrame frame = new DispatcherFrame();
            Dispatcher.CurrentDispatcher.BeginInvoke(DispatcherPriority.Background, new ExitFrameHandler(frm => frm.Continue = false), frame);
            Dispatcher.PushFrame(frame);
        }

        private delegate void ExitFrameHandler(DispatcherFrame frame);
    }

}
