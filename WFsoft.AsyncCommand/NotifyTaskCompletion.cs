﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WFsoft.AsyncCommand
{
    /// <summary>
    /// 此为命名返回结果，Command.xxxx可以调用下面的属性
    /// <TextBlock Text="{Binding Mycommand7.Execution.Result}"/>
    /// <Button Command = "{Binding Mycommand7}" CommandParameter="111222">测试</Button>
    /// <Button Command = "{Binding Mycommand7.CancelCommand}" > 取消 </ Button >
    /// <Button Command="{Binding Mycommand7.ManualResetAsyncCommand}">暂停</Button>
    /// <TextBlock Text = "{Binding Mycommand7.IsSuspend}" />
    /// </summary>
    public sealed class NotifyTaskCompletion<TResult> : INotifyPropertyChanged
    {
        public NotifyTaskCompletion(Task<TResult> task)
        {
            Task = task;
            TaskCompletion = WatchTaskAsync(task);
        }
        private async Task WatchTaskAsync(Task task)
        {
            try
            {
                await task;
            }
            catch
            {
            }
            var propertyChanged = PropertyChanged;
            if (propertyChanged == null)
                return;
            propertyChanged(this, new PropertyChangedEventArgs("Status"));
            propertyChanged(this, new PropertyChangedEventArgs("IsCompleted"));
            propertyChanged(this, new PropertyChangedEventArgs("IsNotCompleted"));
            if (task.IsCanceled)
            {
                propertyChanged(this, new PropertyChangedEventArgs("IsCanceled"));
            }
            else if (task.IsFaulted)
            {
                propertyChanged(this, new PropertyChangedEventArgs("IsFaulted"));
                propertyChanged(this, new PropertyChangedEventArgs("Exception"));
                propertyChanged(this, new PropertyChangedEventArgs("InnerException"));
                propertyChanged(this, new PropertyChangedEventArgs("ErrorMessage"));
            }
            else
            {
                propertyChanged(this, new PropertyChangedEventArgs("IsSuccessfullyCompleted"));
                propertyChanged(this, new PropertyChangedEventArgs("Result"));
            }
        }
        public Task<TResult> Task { get; private set; }
        public Task TaskCompletion { get; private set; }
        public TResult Result => (Task.Status == TaskStatus.RanToCompletion) ? Task.Result : default;
        public TaskStatus Status => Task.Status;
        public bool IsCompleted => Task.IsCompleted;
        public bool IsNotCompleted => !Task.IsCompleted;
        public bool IsSuccessfullyCompleted => Task.Status == TaskStatus.RanToCompletion;
        public bool IsCanceled => Task.IsCanceled;
        public bool IsFaulted => Task.IsFaulted;
        public AggregateException Exception => Task.Exception;
        public Exception InnerException => Exception?.InnerException;
        public string ErrorMessage => InnerException?.Message;
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
