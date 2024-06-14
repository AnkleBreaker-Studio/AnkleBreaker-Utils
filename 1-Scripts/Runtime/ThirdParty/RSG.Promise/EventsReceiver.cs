using System;
using RSG.Exceptions;

// ReSharper disable once CheckNamespace
namespace RSG
{
    public static class EventsReceiver
    {
        private static IEventsReceiver Receiver { get; set; } = new DefaultEventsReceiver();

        public static void SetLogger(IEventsReceiver receiver)
        {
            Receiver = receiver;
        }

        public static void OnVerbose(string message)
        {
            Receiver?.OnVerbose(message);
        }

        public static void OnWarningMinor(string message)
        {
            Receiver?.OnWarningMinor(message);
        }

        public static void OnWarning(string message)
        {
            Receiver?.OnWarning(message);
        }

        public static void OnStateException(PromiseStateException exception)
        {
            Receiver?.OnStateException(exception);
        }

        public static void OnInternalException(Exception exception)
        {
            Receiver?.OnInternalException(exception);
        }
        
        public static void OnRejectException(Exception exception)
        {
            Receiver?.OnRejectException(exception);
        }
        
        public static void OnRejectSilentException(Exception exception)
        {
            Receiver?.OnRejectSilentException(exception);
        }
        
        public static void OnHandlerException(Exception exception)
        {
            Receiver?.OnHandlerException(exception);
        }
    }

    public interface IEventsReceiver
    {
        void OnVerbose(string message);
        void OnWarningMinor(string message);
        void OnWarning(string message);
        void OnStateException(PromiseStateException exception);
        void OnInternalException(Exception exception);
        void OnRejectException(Exception exception);
        void OnRejectSilentException(Exception exception);
        void OnHandlerException(Exception exception);
    }

    public class DefaultEventsReceiver : IEventsReceiver
    {
        public void OnVerbose(string message)
        {
            Console.WriteLine(message);
        }

        public void OnWarningMinor(string message)
        {
            Console.WriteLine(message);
        }

        public void OnWarning(string message)
        {
            Console.WriteLine(message);
        }

        public void OnStateException(PromiseStateException exception)
        {
            throw exception;
        }

        public void OnInternalException(Exception exception)
        {
            Console.WriteLine(exception);
        }

        public void OnRejectException(Exception exception)
        {
            
        }

        public void OnRejectSilentException(Exception exception)
        {
            
        }

        public void OnHandlerException(Exception exception)
        {
            
        }
    }
}