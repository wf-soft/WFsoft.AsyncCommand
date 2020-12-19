# WFsoft.AsyncCommand
WPF MVVM AsyncCommand

> 这是一个WPF MVVM 异步命名帮助类
* 实现异步命令绑定，包括暂停、取消、以及取消上一次命名的重载
* 也包含RelayCommand 和 RelayCommand<T> 的常规命令和 NotifyTaskCompletion<TResult>
* AsyncCommand 包含12个重载
```
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
```

> AsyncCommand使用方法
* 不带参数
```
private IAsyncCommand test1;
public IAsyncCommand Test1 => test1 ??= AsyncCommand.Create(async () => {
  await Task.CompletedTask;
});
private IAsyncCommand test2;
public IAsyncCommand Test2 => test2 ??= AsyncCommand.Create(async token => {
  await Task.CompletedTask;
});
private IAsyncCommand test3;
public IAsyncCommand Test3 => test3 ??= AsyncCommand.Create(async (token, manual) => {
  await Task.CompletedTask;
});
private IAsyncCommand test4;
public IAsyncCommand Test4 => test4 ??= AsyncCommand.Create(async (token, manual) => {
  await Task.CompletedTask;
}, true);
```
* 带CommandParameter参数
```
private IAsyncCommand test1;
public IAsyncCommand Test1 => test1 ??= AsyncCommand.Create<Data>(async data => {
  await Task.CompletedTask;
});
private IAsyncCommand test2;
public IAsyncCommand Test2 => test2 ??= AsyncCommand.Create<Data>(async (data, token) => {
  await Task.CompletedTask;
});
private IAsyncCommand test3;
public IAsyncCommand Test3 => test3 ??= AsyncCommand.Create<Data>(async (data, token, manual) => {
  await Task.CompletedTask;
});
private IAsyncCommand test4;
public IAsyncCommand Test4 => test4 ??= AsyncCommand.Create<Data>(async (data, token, manual) => {
  manual.WaitOne();
  if (token.IsCancellationRequested) return;
  await Task.CompletedTask;
}, true);
```
* 或者不使用async
```
private IAsyncCommand test1;
public IAsyncCommand Test1 => test1 ??= AsyncCommand.Create(() => {
  return Task.CompletedTask;
});
```

* XAML界面我们可以使用
```
<TextBlock Text="{Binding Test1.Execution.Result}"/>
<TextBlock Text="{Binding Test1.Execution.IsCompleted}"/>
<TextBlock Text="{Binding Test1.Execution.IsNotCompleted}"/>
<TextBlock Text="{Binding Test1.Execution.IsSuccessfullyCompleted}"/>
<TextBlock Text="{Binding Test1.Execution.IsCanceled}"/>
<TextBlock Text="{Binding Test1.Execution.IsFaulted}"/>
<TextBlock Text="{Binding Test1.Execution.ErrorMessage}"/>
<Button Command = "{Binding Test1}" CommandParameter="111222">测试</Button>
<Button Command = "{Binding Test1.CancelCommand}" > 取消 </ Button >
<Button Command="{Binding Test1.ManualResetAsyncCommand}">暂停</Button>
<TextBlock Text = "{Binding Test1.IsSuspend}" />
```


