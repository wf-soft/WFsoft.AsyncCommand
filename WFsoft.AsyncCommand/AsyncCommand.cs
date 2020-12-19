using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WFsoft.AsyncCommand.Common;

namespace WFsoft.AsyncCommand
{
    /// <summary>
    /// 使用方法,委托第一个为t,T为参数，否则为返回类型. 暂停检查点:manual.WaitOne() == true 是没暂停
    /// public IAsyncCommand Mycommand8 => mycommand8 ??= AsyncCommand.Create<string>(async (t, token, manual) => { await Task.Delay(100); });
    /// </summary>
    public static class AsyncCommand
    {
        public static AsyncCommand<object> Create(Func<Task> command) => new AsyncCommand<object>(async () => { await command(); return null; });
        public static AsyncCommand<TResult> Create<TResult>(Func<Task<TResult>> command) => new AsyncCommand<TResult>(() => command());
        public static AsyncCommand<object> Create(Func<CancellationToken, Task> command, bool isCancelUpTask = false) => new AsyncCommand<object>(async (token) => { await command(token); return null; }, isCancelUpTask);
        public static AsyncCommand<TResult> Create<TResult>(Func<CancellationToken, Task<TResult>> command, bool isCancelUpTask = false) => new AsyncCommand<TResult>(command, isCancelUpTask);
        public static AsyncCommand<object> Create(Func<CancellationToken, ManualResetEvent, Task> command, bool isCancelUpTask = false) => new AsyncCommand<object>(async (token, manual) => { await command(token, manual); return null; }, isCancelUpTask);
        public static AsyncCommand<TResult> Create<TResult>(Func<CancellationToken, ManualResetEvent, Task<TResult>> command, bool isCancelUpTask = false) => new AsyncCommand<TResult>(command, isCancelUpTask);
        public static AsyncCommand<T, object> Create<T>(Func<T, Task> command) => new AsyncCommand<T, object>(async (t) => { await command(t); return null; });
        public static AsyncCommand<T, TResult> Create<T, TResult>(Func<T, Task<TResult>> command) => new AsyncCommand<T, TResult>(command);
        public static AsyncCommand<T, object> Create<T>(Func<T, CancellationToken, Task> command, bool isCancelUpTask = false) => new AsyncCommand<T, object>(async (t, token) => { await command(t, token); return null; }, isCancelUpTask);
        public static AsyncCommand<T, TResult> Create<T, TResult>(Func<T, CancellationToken, Task<TResult>> command, bool isCancelUpTask = false) => new AsyncCommand<T, TResult>(command, isCancelUpTask);
        public static AsyncCommand<T, object> Create<T>(Func<T, CancellationToken, ManualResetEvent, Task> command, bool isCancelUpTask = false) => new AsyncCommand<T, object>(async (t, token, manual) => { await command(t, token, manual); return null; }, isCancelUpTask);
        public static AsyncCommand<T, TResult> Create<T, TResult>(Func<T, CancellationToken, ManualResetEvent, Task<TResult>> command, bool isCancelUpTask = false) => new AsyncCommand<T, TResult>(command, isCancelUpTask);
    }
}
