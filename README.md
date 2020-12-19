# WFsoft.AsyncCommand

> 这是一个WPF MVVM 异步命名帮助类
* 实现异步命令绑定，包括暂停、取消、以及取消上一次命名的重载
* 也包含RelayCommand 和 RelayCommand<T> 的常规命令和 NotifyTaskCompletion<TResult>
* AsyncCommand 包含12个重载
```
AsyncCommand<object> Create(Func<Task> command)
AsyncCommand<TResult> Create<TResult>(Func<Task<TResult>> command)
AsyncCommand<object> Create(Func<CancellationToken, Task> command, bool isCancelUpTask = false)
AsyncCommand<TResult> Create<TResult>(Func<CancellationToken, Task<TResult>> command, bool isCancelUpTask = false)
AsyncCommand<object> Create(Func<CancellationToken, ManualResetEvent, Task> command, bool isCancelUpTask = false)
AsyncCommand<TResult> Create<TResult>(Func<CancellationToken, ManualResetEvent, Task<TResult>> command, bool isCancelUpTask = false)
AsyncCommand<T, object> Create<T>(Func<T, Task> command)
AsyncCommand<T, TResult> Create<T, TResult>(Func<T, Task<TResult>> command)
AsyncCommand<T, object> Create<T>(Func<T, CancellationToken, Task> command, bool isCancelUpTask = false)
AsyncCommand<T, TResult> Create<T, TResult>(Func<T, CancellationToken, Task<TResult>> command, bool isCancelUpTask = false)
AsyncCommand<T, object> Create<T>(Func<T, CancellationToken, ManualResetEvent, Task> command, bool isCancelUpTask = false)
AsyncCommand<T, TResult> Create<T, TResult>(Func<T, CancellationToken, ManualResetEvent, Task<TResult>> command, bool isCancelUpTask = false)
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
  manual.WaitOne();
  if (token.IsCancellationRequested) return;
  await Task.CompletedTask;
});
```
* 或者不使用async
```
private IAsyncCommand test1;
public IAsyncCommand Test1 => test1 ??= AsyncCommand.Create(() => {
  return Task.CompletedTask;
});
```
* 执行前取消上一次命令
```
private IAsyncCommand test4;
public IAsyncCommand Test4 => test4 ??= AsyncCommand.Create(async token => {
  if (token.IsCancellationRequested) return;
  await Task.CompletedTask;
}, true);
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


