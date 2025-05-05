using System;
using System.Collections.Generic;

namespace Treks.Services
{
    public enum ToastType
    {
        Info,
        Success,
        Error
    }

    public class ToastMessage
    {
        public string Message { get; set; } = "";
        public ToastType Type { get; set; } = ToastType.Info;
        public DateTime Timestamp { get; set; } = DateTime.Now;
    }

    public class ToastService
    {
        private readonly List<ToastMessage> _toasts = new();
        public IReadOnlyList<ToastMessage> ToastMessages => _toasts.AsReadOnly();

        public event Action? OnToastsUpdated;

        public void ShowToast(string message, ToastType type = ToastType.Info, int durationInSeconds = 5)
        {
            var toast = new ToastMessage
            {
                Message = message,
                Type = type,
                Timestamp = DateTime.Now
            };

            _toasts.Add(toast);
            OnToastsUpdated?.Invoke();

            _ = Task.Run(async () =>
            {
                await Task.Delay(durationInSeconds * 1000);
                RemoveToast(toast);
            });
        }

        public void RemoveToast(ToastMessage toast)
        {
            if (_toasts.Contains(toast))
            {
                _toasts.Remove(toast);
                OnToastsUpdated?.Invoke();
            }
        }

        public void ClearToasts()
        {
            _toasts.Clear();
            OnToastsUpdated?.Invoke();
        }
    }
}