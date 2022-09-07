using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            //SynchronizationContext.Current.
            //Thread.CurrentThread.
            //SynchronizationContext.Current.Post
        }
    }

    //public interface IDispatcherFacade
    //{
    //    void AddToDispatcherQueue(Delegate workItem);
    //}

    //public class DispatcherFacade : IDispatcherFacade
    //{
    //    readonly Dispatcher dispatcher;
    //    readonly ConcurrentDictionary<DispatcherOperation, object> operations;
    //    long operationsQueueCount;

    //    public int DispatcherQueueSize { get; set; } = 250;
    //    public int DispatcherMonitorRate { get; set; } = 5000;

    //    public DispatcherFacade(Dispatcher dispatcher)
    //    {
    //        this.dispatcher = dispatcher;
    //        operations = new ConcurrentDictionary<DispatcherOperation, object>();
    //        Observable.Interval(TimeSpan.FromMilliseconds(DispatcherMonitorRate)).Subscribe(MonitorDispatcherQueue);
    //    }

    //    private void MonitorDispatcherQueue(long l)
    //    {
    //        if (operationsQueueCount > DispatcherQueueSize)
    //        {
    //            DoEvents(Application.Current);
    //            operations.Clear();
    //            Interlocked.Exchange(ref operationsQueueCount, 0);
    //        }
    //    }

    //    public void AddToDispatcherQueue(Delegate workItem)
    //    {
    //        Interlocked.Increment(ref operationsQueueCount);
    //        var operation = dispatcher.BeginInvoke(DispatcherPriority.Background, workItem);
    //        operations.TryAdd(operation, null);
    //        operation.Completed += (s, e) =>
    //        {
    //            Interlocked.Decrement(ref operationsQueueCount);
    //            operations.TryRemove((DispatcherOperation)s, out object t);
    //        };
    //    }

    //    public void DoEvents(Application application)
    //    {
    //        DispatcherFrame frame = new DispatcherFrame();
    //        Dispatcher.CurrentDispatcher.BeginInvoke(DispatcherPriority.Background, new ExitFrameHandler(frm => frm.Continue = false), frame);
    //        Dispatcher.PushFrame(frame);
    //    }

    //    private delegate void ExitFrameHandler(DispatcherFrame frame);
    //}

}
