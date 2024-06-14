using System;
using System.Collections.Generic;
using System.Linq;
using RSG.Exceptions;
using RSG.Promises;

// ReSharper disable once CheckNamespace
namespace RSG
{
    /// <summary>
    /// Implements a non-generic C# promise, this is a promise that simply resolves without delivering a value.
    /// https://developer.mozilla.org/en/docs/Web/JavaScript/Reference/Global_Objects/Promise
    /// </summary>
    public interface IPromise : ICancellablePromise
    {
        /// <summary>
        /// ID of the promise, useful for debugging.
        /// </summary>
        int Id { get; }

        /// <summary>
        /// Set the name of the promise, useful for debugging.
        /// </summary>
        IPromise WithName(string name);

        /// <summary>
        /// Completes the promise. 
        /// onResolved is called on successful completion.
        /// onRejected is called on error.
        /// </summary>
        void Done(Action onResolved, Action<Exception> onRejected);

        /// <summary>
        /// Completes the promise. 
        /// onResolved is called on successful completion.
        /// Adds a default error handler.
        /// </summary>
        void Done(Action onResolved);

        /// <summary>
        /// Complete the promise. Adds a default error handler.
        /// </summary>
        void Done();

        /// <summary>
        /// Handle errors for the promise. Rejects the promise after the Catch.
        /// </summary>
        IPromise Catch(Action<Exception> onRejected);

        /// <summary>
        /// Handle errors for the promise. Resolves the promise afterwards.
        /// </summary>
        IPromise CatchAsResolved(Action<Exception> onRejected);

        /// <summary>
        /// Add a resolved callback that chains a value promise (optionally converting to a different value type).
        /// </summary>
        IPromise<TConvertedT> Then<TConvertedT>(Func<IPromise<TConvertedT>> onResolved);

        /// <summary>
        /// Add a resolved callback that chains a non-value promise.
        /// </summary>
        IPromise Then(Func<IPromise> onResolved);

        /// <summary>
        /// Add a resolved callback.
        /// </summary>
        IPromise Then(Action onResolved);

        /// <summary>
        /// Add a resolved callback and a rejected callback.
        /// The resolved callback chains a value promise (optionally converting to a different value type).
        /// </summary>
        IPromise<TConvertedT> Then<TConvertedT>(Func<IPromise<TConvertedT>> onResolved, Func<Exception, IPromise<TConvertedT>> onRejected);

        /// <summary>
        /// Add a resolved callback and a rejected callback.
        /// The resolved callback chains a non-value promise.
        /// </summary>
        IPromise Then(Func<IPromise> onResolved, Action<Exception> onRejected);

        /// <summary>
        /// Add a resolved callback and a rejected callback.
        /// </summary>
        IPromise Then(Action onResolved, Action<Exception> onRejected);

        /// <summary>
        /// Add a resolved callback, a rejected callback and a progress callback.
        /// The resolved callback chains a value promise (optionally converting to a different value type).
        /// </summary>
        IPromise<TConvertedT> Then<TConvertedT>(Func<IPromise<TConvertedT>> onResolved, Func<Exception, IPromise<TConvertedT>> onRejected, Action<float> onProgress);

        /// <summary>
        /// Add a resolved callback, a rejected callback and a progress callback.
        /// The resolved callback chains a non-value promise.
        /// </summary>
        IPromise Then(Func<IPromise> onResolved, Action<Exception> onRejected, Action<float> onProgress);

        /// <summary>
        /// Add a resolved callback, a rejected callback and a progress callback.
        /// </summary>
        IPromise Then(Action onResolved, Action<Exception> onRejected, Action<float> onProgress);

        /// <summary>
        /// Chain an enumerable of promises, all of which must resolve.
        /// The resulting promise is resolved when all of the promises have resolved.
        /// It is rejected as soon as any of the promises have been rejected.
        /// </summary>
        IPromise ThenAll(Func<IEnumerable<IPromise>> chain);

        /// <summary>
        /// Chain an enumerable of promises, all of which must resolve.
        /// Converts to a non-value promise.
        /// The resulting promise is resolved when all of the promises have resolved.
        /// It is rejected as soon as any of the promises have been rejected.
        /// </summary>
        IPromise<IEnumerable<TConvertedT>> ThenAll<TConvertedT>(Func<IEnumerable<IPromise<TConvertedT>>> chain);

        /// <summary>
        /// Chain a sequence of operations using promises.
        /// Reutrn a collection of functions each of which starts an async operation and yields a promise.
        /// Each function will be called and each promise resolved in turn.
        /// The resulting promise is resolved after each promise is resolved in sequence.
        /// </summary>
        IPromise ThenSequence(Func<IEnumerable<Func<IPromise>>> chain);

        /// <summary>
        /// Takes a function that yields an enumerable of promises.
        /// Returns a promise that resolves when the first of the promises has resolved.
        /// </summary>
        IPromise ThenRace(Func<IEnumerable<IPromise>> chain);

        /// <summary>
        /// Takes a function that yields an enumerable of promises.
        /// Converts to a value promise.
        /// Returns a promise that resolves when the first of the promises has resolved.
        /// </summary>
        IPromise<TConvertedT> ThenRace<TConvertedT>(Func<IEnumerable<IPromise<TConvertedT>>> chain);

        /// <summary> 
        /// Add a finally callback. 
        /// Finally callbacks will always be called, even if any preceding promise is cancelled, rejected, or encounters an error.
        /// The returned promise will be resolved or rejected, as per the preceding promise.
        /// </summary> 
        void Finally(Action onComplete);

        /// <summary>
        /// Add a callback that chains a non-value promise.
        /// ContinueWith callbacks will always be called, even if any preceding promise is rejected, or encounters an error.
        /// The state of the returning promise will be based on the new non-value promise, not the preceding (rejected or resolved) promise.
        /// </summary>
        IPromise ContinueWith(Func<IPromise> onResolved);

        /// <summary>
        /// Add a callback that chains a non-value promise.
        /// ContinueWith callbacks will always be called, even if any preceding promise is rejected, or encounters an error.
        /// The state of the returning promise will be based on the new non-value promise, not the preceding (rejected or resolved) promise.
        /// </summary>
        IPromise ContinueWithResolved(Action onResolved);

        /// <summary> 
        /// Add a callback that chains a value promise (optionally converting to a different value type).
        /// ContinueWith callbacks will always be called, even if any preceding promise is rejected, or encounters an error.
        /// The state of the returning promise will be based on the new value promise, not the preceding (rejected or resolved) promise.
        /// </summary> 
        IPromise<TConvertedT> ContinueWith<TConvertedT>(Func<IPromise<TConvertedT>> onComplete);

        /// <summary>
        /// Add a progress callback.
        /// Progress callbacks will be called whenever the promise owner reports progress towards the resolution
        /// of the promise.
        /// </summary>
        IPromise Progress(Action<float> onProgress);
        
        /// <summary>
        /// Add a cancel callback 
        /// </summary>
        void OnCancel(Action onCancel);
    }

    /// <summary>
    /// Interface for a promise that can be rejected or resolved.
    /// </summary>
    public interface IPendingPromise : IRejectable
    {
        /// <summary>
        /// ID of the promise, useful for debugging.
        /// </summary>
        int Id { get; }
        
        /// <summary>
        /// A shortcut to detect whether a promise is in Pending state.
        /// </summary>
        bool CanBeResolved { get; }

        /// <summary>
        /// Resolve the promise with a particular value.
        /// </summary>
        void Resolve();

        /// <summary>
        /// Report progress in a promise.
        /// </summary>
        void ReportProgress(float progress);
    }

    /// <summary>
    /// Used to list information of pending promises.
    /// </summary>
    public interface IPromiseInfo
    {
        /// <summary>
        /// Id of the promise.
        /// </summary>
        int Id { get; }

        /// <summary>
        /// Human-readable name for the promise.
        /// </summary>
        string Name { get; }
        
        /// <summary>
        /// Loggable name.
        /// </summary>
        /// <returns></returns>
        string GetName();
        
        /// <summary>
        /// Current promise state.
        /// </summary>
        PromiseState CurState { get; }
    }

    /// <summary>
    /// Arguments to the UnhandledError event.
    /// </summary>
    public class ExceptionEventArgs : EventArgs
    {
        internal ExceptionEventArgs(Exception exception)
        {
            Exception = exception;
        }

        public Exception Exception
        {
            get;
            private set;
        }
    }

    /// <summary>
    /// Represents a handler invoked when the promise is rejected.
    /// </summary>
    public struct RejectHandler
    {
        /// <summary>
        /// Callback fn.
        /// </summary>
        public Action<Exception> Callback;

        /// <summary>
        /// The promise that is rejected when there is an error while invoking the handler.
        /// </summary>
        public IRejectable Rejectable;
    }

    public struct ProgressHandler
    {
        /// <summary>
        /// Callback fn.
        /// </summary>
        public Action<float> Callback;

        /// <summary>
        /// The promise that is rejected when there is an error while invoking the handler.
        /// </summary>
        public IRejectable Rejectable;
    }

    /// <summary>
    /// Implements a non-generic C# promise, this is a promise that simply resolves without delivering a value.
    /// https://developer.mozilla.org/en/docs/Web/JavaScript/Reference/Global_Objects/Promise
    /// </summary>
    public class Promise : IPromise, IPendingPromise, IPromiseInfo
    {
        /// <summary>
        /// Set to true to enable tracking of promises.
        /// </summary>
        public static bool EnablePromiseTracking = false;

        /// <summary>
        /// Event raised for unhandled errors.
        /// For this to work you have to complete your promises with a call to Done().
        /// </summary>
        public static event EventHandler<ExceptionEventArgs> UnhandledException
        {
            add { _unhandlerException += value; }
            remove { _unhandlerException -= value; }
        }
        private static EventHandler<ExceptionEventArgs> _unhandlerException;

        /// <summary>
        /// Id for the next promise that is created.
        /// </summary>
        private static int _nextPromiseId;

        /// <summary>
        /// Information about pending promises.
        /// </summary>
        internal static readonly HashSet<IPromiseInfo> PendingPromises = 
            new HashSet<IPromiseInfo>();

        /// <summary>
        /// Information about pending promises, useful for debugging.
        /// This is only populated when 'EnablePromiseTracking' is set to true.
        /// </summary>
        public static IEnumerable<IPromiseInfo> GetPendingPromises()
        {
            return PendingPromises;
        }
        
        /// <summary>
        /// Information about pending promises count, useful for debugging.
        /// This is only populated when 'EnablePromiseTracking' is set to true.
        /// </summary>
        public static int GetPendingPromisesCount()
        {
            return PendingPromises.Count;
        }

        /// <summary>
        /// The exception when the promise is rejected.
        /// </summary>
        private Exception _rejectionException;

        /// <summary>
        /// Error handlers.
        /// </summary>
        private List<RejectHandler> _rejectHandlers;

        /// <summary>
        /// Represents a handler invoked when the promise is resolved.
        /// </summary>
        public struct ResolveHandler
        {
            /// <summary>
            /// Callback fn.
            /// </summary>
            public Action Callback;

            /// <summary>
            /// The promise that is rejected when there is an error while invoking the handler.
            /// </summary>
            public IRejectable Rejectable;
        }

        /// <summary>
        /// Completed handlers that accept no value.
        /// </summary>
        private List<ResolveHandler> _resolveHandlers;

        /// <summary>
        /// Progress handlers.
        /// </summary>
        private List<ProgressHandler> _progressHandlers;
        
        /// <summary>
        /// Cancel handlers.
        /// </summary>
        private List<ResolveHandler> _cancelHandlers;

        /// <summary>
        /// ID of the promise, useful for debugging.
        /// </summary>
        public int Id { get { return _id; } }

        private readonly int _id;

        /// <summary>
        /// Name of the promise, when set, useful for debugging.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Tracks the current state of the promise.
        /// </summary>
        public PromiseState CurState { get; private set; }

        /// <summary>
        /// Promise state shortcut.
        /// </summary>
        public bool IsPending => CurState == PromiseState.Pending;
        
        /// <summary>
        /// Promise state shortcut.
        /// </summary>
        public bool CanBeResolved => CurState == PromiseState.Pending;
        
        /// <summary>
        /// Promise state shortcut.
        /// </summary>
        public bool CanBeCanceled => CurState == PromiseState.Pending;
        
        /// <summary>
        /// A promise parent in chain.
        /// </summary>
        public ICancellablePromise Parent { get; private set; }
        
        /// <summary>
        /// Promise children in chain.
        /// </summary>
        public HashSet<ICancellablePromise> Children { get; } = new HashSet<ICancellablePromise>();
        
        /// <summary>
        /// Loggable name.
        /// </summary>
        /// <returns></returns>
        public string GetName()
        {
            return string.IsNullOrEmpty(Name) ? "Promise" : $"Promise = {Name}";
        }
        
        public Promise()
        {
            CurState = PromiseState.Pending;
            _id = NextId();
            if (EnablePromiseTracking)
            {
                PendingPromises.Add(this);
            }
        }

        public Promise(Action<Action, Action<Exception>> resolver)
        {
            CurState = PromiseState.Pending;
            _id = NextId();
            if (EnablePromiseTracking)
            {
                PendingPromises.Add(this);
            }

            try
            {
                resolver(Resolve, RejectSilent);
            }
            catch (Exception ex)
            {
                RejectSilent(ex);
            }
        }

        private Promise(PromiseState initialState)
        {
            CurState = initialState;
            _id = NextId();
        }
        
        public void AttachParent(ICancellablePromise parent)
        {
            if (parent.Parent == this)
            {
                EventsReceiver.OnWarningMinor(
                    $"Skip attempt to create cycled refs in promises parents {GetName()}");
                return;
            }

            if (Parent != null && Parent != parent)
            {
                EventsReceiver.OnWarningMinor($"Overwriting existing parent {GetName()}");
            }
            
            Parent = parent;
            parent.AttachChild(this);
        }

        public void AttachChild(ICancellablePromise child)
        {
            Children.Add(child);
        }

        /// <summary>
        /// Increments the ID counter and gives us the ID for the next promise.
        /// </summary>
        internal static int NextId()
        {
            return ++_nextPromiseId;
        }

        /// <summary>
        /// Add a rejection handler for this promise.
        /// </summary>
        private void AddRejectHandler(Action<Exception> onRejected, IRejectable rejectable)
        {
            if (_rejectHandlers == null)
            {
                _rejectHandlers = new List<RejectHandler>();
            }

            _rejectHandlers.Add(new RejectHandler
            {
                Callback = onRejected,
                Rejectable = rejectable
            });
        }

        /// <summary>
        /// Add a cancellation handler for this promise.
        /// </summary>
        /// <param name="onCanceled"></param>
        /// <param name="rejectable"></param>
        private void AddCancelHandler(Action onCanceled, IRejectable rejectable)
        {
            if (_cancelHandlers == null)
            {
                _cancelHandlers = new List<ResolveHandler>();
            }

            _cancelHandlers.Add(new ResolveHandler
            {
                Callback = onCanceled,
                Rejectable = rejectable
            });
        }

        /// <summary>
        /// Add a resolve handler for this promise.
        /// </summary>
        private void AddResolveHandler(Action onResolved, IRejectable rejectable)
        {
            if (_resolveHandlers == null)
            {
                _resolveHandlers = new List<ResolveHandler>();
            }

            _resolveHandlers.Add(new ResolveHandler
            {
                Callback = onResolved,
                Rejectable = rejectable
            });
        }

        /// <summary>
        /// Add a progress handler for this promise.
        /// </summary>
        private void AddProgressHandler(Action<float> onProgress, IRejectable rejectable)
        {
            if (_progressHandlers == null)
            {
                _progressHandlers = new List<ProgressHandler>();
            }

            _progressHandlers.Add(new ProgressHandler { Callback = onProgress, Rejectable = rejectable });
        }

        /// <summary>
        /// Invoke a single error handler.
        /// </summary>
        private void InvokeRejectHandler(Action<Exception> callback, IRejectable rejectable, Exception value)
        {
            try
            {
                callback(value);
            }
            catch (Exception ex)
            {
                EventsReceiver.OnHandlerException(ex);
                rejectable.RejectSilent(ex);
            }
        }

        /// <summary>
        /// Invoke a single resolve handler.
        /// </summary>
        private void InvokeResolveHandler(Action callback, IRejectable rejectable)
        {
            try
            {
                callback();
            }
            catch (Exception ex)
            {
                EventsReceiver.OnHandlerException(ex);
                rejectable.RejectSilent(ex);
            }
        }

        /// <summary>
        /// Invoke a single cancel handler.
        /// </summary>
        /// <param name="callback"></param>
        /// <param name="rejectable"></param>
        private void InvokeCancelHandler(Action callback, IRejectable rejectable)
        {
            try
            {
                callback();
            }
            catch (Exception ex)
            {
                EventsReceiver.OnHandlerException(ex);
                rejectable.RejectSilent(ex);
            }
        }

        /// <summary>
        /// Invoke a single progress handler.
        /// </summary>
        private void InvokeProgressHandler(Action<float> callback, IRejectable rejectable, float progress)
        {
            try
            {
                callback(progress);
            }
            catch (Exception ex)
            {
                EventsReceiver.OnHandlerException(ex);
                rejectable.RejectSilent(ex);
            }
        }

        /// <summary>
        /// Helper function clear out all handlers after resolution or rejection.
        /// </summary>
        private void ClearHandlers()
        {
            _rejectHandlers = null;
            _resolveHandlers = null;
            _progressHandlers = null;
            _cancelHandlers = null;
        }

        /// <summary>
        /// Invoke all reject handlers.
        /// </summary>
        private void InvokeRejectHandlers(Exception ex)
        {
            if (_rejectHandlers != null)
            {
                for (int i = 0, maxI = _rejectHandlers.Count; i < maxI; ++i)
                    InvokeRejectHandler(_rejectHandlers[i].Callback, _rejectHandlers[i].Rejectable, ex);
            }

            ClearHandlers();
        }

        /// <summary>
        /// Invoke all resolve handlers.
        /// </summary>
        private void InvokeResolveHandlers()
        {
            if (_resolveHandlers != null)
            {
                for (int i = 0, maxI = _resolveHandlers.Count; i < maxI; i++)
                    InvokeResolveHandler(_resolveHandlers[i].Callback, _resolveHandlers[i].Rejectable);
            }

            ClearHandlers();
        }
        
                
        /// <summary>
        /// Invoke all cancel handlers.
        /// </summary>
        private void InvokeCancelHandlers()
        {
            _cancelHandlers?.Each(handler => InvokeCancelHandler(handler.Callback, handler.Rejectable));

            ClearHandlers();
        }

        /// <summary>
        /// Invoke all progress handlers.
        /// </summary>
        private void InvokeProgressHandlers(float progress)
        {
            if (_progressHandlers != null)
            {
                for (int i = 0, maxI = _progressHandlers.Count; i < maxI; i++)
                    InvokeProgressHandler(_progressHandlers[i].Callback, _progressHandlers[i].Rejectable, progress);
            }
        }
        
        /// <summary>
        /// Reject the promise with an exception. Calls OnError.
        /// </summary>
        public void Reject(Exception ex)
        {
            if (CurState != PromiseState.Pending)
            {
                var message =
                    "Attempt to reject a promise that is already in state: " 
                    + CurState + ", a promise can only be rejected when it is still in state: " + PromiseState.Pending;
                EventsReceiver.OnStateException(new PromiseStateException(message));
                return;
            }
            
            var rejectException = ex ?? new Exception("Promise rejected");
            EventsReceiver.OnRejectException(rejectException);
            RejectInternal(rejectException);
        }

        /// <summary>
        /// Reject the promise with an exception. Doesn't call LogError.
        /// </summary>
        public void RejectSilent(Exception ex)
        {
            if (CurState != PromiseState.Pending)
            {
                var message =
                    "Attempt to reject a promise that is already in state: " 
                    + CurState + ", a promise can only be rejected when it is still in state: " + PromiseState.Pending;
                EventsReceiver.OnStateException(new PromiseStateException(message));
                return;
            }
            
            var rejectException = ex ?? new Exception("Promise rejected");
            EventsReceiver.OnRejectSilentException(rejectException);
            RejectInternal(rejectException);
        }

        private void RejectInternal(Exception ex)
        {
            _rejectionException = ex;
            CurState = PromiseState.Rejected;

            if (EnablePromiseTracking)
            {
                PendingPromises.Remove(this);
            }

            InvokeRejectHandlers(ex);            
        }
        
        /// <summary>
        /// Cancels the whole chain where this promise exists.
        /// </summary>
        public void Cancel()
        {
            var parent = this.FindLastPendingParent();
            if (parent == null || parent == this)
            {
                CancelInternal();
            }
            else
            {
                parent.Cancel();
            }
        }
        
        private void CancelInternal()
        {
            if (CurState != PromiseState.Pending)
            {
                return;
            }

            CurState = PromiseState.Cancelled;
            if (EnablePromiseTracking)
            {
                PendingPromises.Remove(this);
            }
            InvokeCancelHandlers();
            ClearHandlers();
        }

        public bool TryResolve()
        {
            if (CurState != PromiseState.Pending)
            {
                return false;
            }
            
            Resolve();
            return true;
        }

        /// <summary>
        /// Resolve the promise with a particular value.
        /// </summary>
        public void Resolve()
        {
            if (CurState != PromiseState.Pending)
            {
                EventsReceiver.OnStateException(new PromiseStateException(
                    "Attempt to resolve a promise that is already in state: " + CurState 
                    + ", a promise can only be resolved when it is still in state: " 
                    + PromiseState.Pending
                ));
                return;
            }

            CurState = PromiseState.Resolved;

            if (EnablePromiseTracking)
            {
                PendingPromises.Remove(this);
            }

            InvokeResolveHandlers();
        }

        /// <summary>
        /// Report progress on the promise.
        /// </summary>
        public void ReportProgress(float progress)
        {
            if (CurState != PromiseState.Pending)
            {
                EventsReceiver.OnStateException(new PromiseStateException(
                    "Attempt to report progress on a promise that is already in state: " 
                    + CurState + ", a promise can only report progress when it is still in state: " 
                    + PromiseState.Pending
                ));
                return;
            }

            InvokeProgressHandlers(progress);
        }


        /// <summary>
        /// Completes the promise. 
        /// onResolved is called on successful completion.
        /// onRejected is called on error.
        /// </summary>
        public void Done(Action onResolved, Action<Exception> onRejected)
        {
            Then(onResolved, onRejected)
                .CatchAsResolved(ex =>
                    PropagateUnhandledException(this, ex)
                );
        }

        /// <summary>
        /// Completes the promise. 
        /// onResolved is called on successful completion.
        /// Adds a default error handler.
        /// </summary>
        public void Done(Action onResolved)
        {
            Then(onResolved)
                .CatchAsResolved(ex => 
                    PropagateUnhandledException(this, ex)
                );
        }

        /// <summary>
        /// Complete the promise. Adds a defualt error handler.
        /// </summary>
        public void Done()
        {
            if (CurState == PromiseState.Resolved)
                return;

            CatchAsResolved(ex => PropagateUnhandledException(this, ex));
        }

        /// <summary>
        /// Set the name of the promise, useful for debugging.
        /// </summary>
        public IPromise WithName(string name)
        {
            Name = name;
            return this;
        }
        
        /// <summary>
        /// Handle errors for the promise. Resolves the promise afterwards.
        /// </summary>
        public IPromise CatchAsResolved(Action<Exception> onRejected)
        {
            if (CurState == PromiseState.Resolved)
            {
                return this;
            }

            var resultPromise = new Promise();
            resultPromise.WithName(Name);
            resultPromise.AttachParent(this);

            Action resolveHandler = () => resultPromise.Resolve();

            Action<Exception> rejectHandler = ex =>
            {
                try
                {
                    onRejected(ex);
                    resultPromise.Resolve();
                }
                catch (Exception callbackException)
                {
                    EventsReceiver.OnHandlerException(callbackException);
                    resultPromise.RejectSilent(callbackException);
                }
            };

            Action cancelHandler = () => resultPromise.Cancel();

            ActionHandlers(resultPromise, resolveHandler, rejectHandler, cancelHandler);
            ProgressHandlers(resultPromise, v => resultPromise.ReportProgress(v));

            return resultPromise;
        }
        
        /// <summary>
        /// Handle errors for the promise. Rejects the promise after the Catch.
        /// </summary>
        public IPromise Catch(Action<Exception> onRejected)
        {
            if (CurState == PromiseState.Resolved)
            {
                return this;
            }

            var resultPromise = new Promise();
            resultPromise.WithName(Name);
            resultPromise.AttachParent(this);

            Action resolveHandler = () => resultPromise.Resolve();

            Action<Exception> rejectHandler = ex =>
            {
                try
                {
                    onRejected(ex);
                    resultPromise.RejectSilent(ex);
                }
                catch (Exception callbackException)
                {
                    EventsReceiver.OnHandlerException(callbackException);
                    resultPromise.RejectSilent(callbackException);
                }
            };

            Action cancelHandler = () => resultPromise.Cancel();

            ActionHandlers(resultPromise, resolveHandler, rejectHandler, cancelHandler);
            ProgressHandlers(resultPromise, v => resultPromise.ReportProgress(v));

            return resultPromise;
        }

        /// <summary>
        /// Add a cancel callback
        /// </summary>
        /// <param name="onCancel"></param>
        public void OnCancel(Action onCancel)
        {
            ActionHandlers(this, () => { }, ex => { }, onCancel);
        }

        /// <summary>
        /// Add a resolved callback that chains a value promise (optionally converting to a different value type).
        /// </summary>
        public IPromise<TConvertedT> Then<TConvertedT>(Func<IPromise<TConvertedT>> onResolved)
        {
            return Then(onResolved, null, null);
        }

        /// <summary>
        /// Add a resolved callback that chains a non-value promise.
        /// </summary>
        public IPromise Then(Func<IPromise> onResolved)
        {
            return Then(onResolved, null, null);
        }

        /// <summary>
        /// Add a resolved callback.
        /// </summary>
        public IPromise Then(Action onResolved)
        {
            return Then(onResolved, null, null);
        }

        /// <summary>
        /// Add a resolved callback and a rejected callback.
        /// The resolved callback chains a value promise (optionally converting to a different value type).
        /// </summary>
        public IPromise<TConvertedT> Then<TConvertedT>(Func<IPromise<TConvertedT>> onResolved, Func<Exception, IPromise<TConvertedT>> onRejected)
        {
            return Then(onResolved, onRejected, null);
        }

        /// <summary>
        /// Add a resolved callback and a rejected callback.
        /// The resolved callback chains a non-value promise.
        /// </summary>
        public IPromise Then(Func<IPromise> onResolved, Action<Exception> onRejected)
        {
            return Then(onResolved, onRejected, null);
        }

        /// <summary>
        /// Add a resolved callback and a rejected callback.
        /// </summary>
        public IPromise Then(Action onResolved, Action<Exception> onRejected)
        {
            return Then(onResolved, onRejected, null);
        }

        /// <summary>
        /// Add a resolved callback, a rejected callback and a progress callback.
        /// The resolved callback chains a value promise (optionally converting to a different value type).
        /// </summary>
        public IPromise<TConvertedT> Then<TConvertedT>(
            Func<IPromise<TConvertedT>> onResolved,
            Func<Exception, IPromise<TConvertedT>> onRejected,
            Action<float> onProgress)
        {
            if (CurState == PromiseState.Resolved)
            {
                try
                {
                    return onResolved();
                } 
                catch (Exception ex)
                {
                    EventsReceiver.OnHandlerException(ex);
                    return Promise<TConvertedT>.Rejected(ex);
                }
            }

            // This version of the function must supply an onResolved.
            // Otherwise there is now way to get the converted value to pass to the resulting promise.

            var resultPromise = new Promise<TConvertedT>();
            resultPromise.WithName(Name);
            resultPromise.AttachParent(this);

            Action resolveHandler = () =>
            {
                onResolved()
                    .Progress(progress => resultPromise.ReportProgress(progress))
                    .Then(
                        // Should not be necessary to specify the arg type on the next line, but Unity (mono) has an internal compiler error otherwise.
                        chainedValue => resultPromise.Resolve(chainedValue),
                        ex => resultPromise.RejectSilent(ex)
                    )
                    .OnCancel(() => resultPromise.Cancel());
            };

            Action<Exception> rejectHandler = ex =>
            {
                if (onRejected == null)
                {
                    resultPromise.RejectSilent(ex);
                    return;
                }

                try
                {
                    onRejected(ex)
                        .Then(
                            chainedValue => resultPromise.Resolve(chainedValue),
                            callbackEx => resultPromise.RejectSilent(callbackEx)
                        );
                }
                catch (Exception callbackEx)
                {
                    EventsReceiver.OnHandlerException(callbackEx);
                    resultPromise.RejectSilent(callbackEx);
                }
            };
            
            Action cancelHandler = () => resultPromise.Cancel();

            ActionHandlers(resultPromise, resolveHandler, rejectHandler, cancelHandler);
            if (onProgress != null)
            {
                ProgressHandlers(this, onProgress);
            }

            return resultPromise;
        }

        /// <summary>
        /// Add a resolved callback, a rejected callback and a progress callback.
        /// The resolved callback chains a non-value promise.
        /// </summary>
        public IPromise Then(Func<IPromise> onResolved, Action<Exception> onRejected, Action<float> onProgress)
        {
            if (CurState == PromiseState.Resolved)
            {
                try
                {
                    return onResolved();
                }
                catch (Exception ex)
                {
                    EventsReceiver.OnHandlerException(ex);
                    return Rejected(ex);
                }
            }

            var resultPromise = new Promise();
            resultPromise.WithName(Name);
            resultPromise.AttachParent(this);

            Action resolveHandler;
            if (onResolved != null)
            {
                resolveHandler = () =>
                {
                    onResolved()
                        .Progress(progress => resultPromise.ReportProgress(progress))
                        .Then(
                            () => resultPromise.Resolve(),
                            ex => resultPromise.RejectSilent(ex)
                        )
                        .OnCancel(() => resultPromise.Cancel());
                };
            }
            else
            {
                resolveHandler = resultPromise.Resolve;
            }

            Action<Exception> rejectHandler;
            if (onRejected != null)
            {
                rejectHandler = ex =>
                {
                    onRejected(ex);
                    resultPromise.RejectSilent(ex);
                };
            }
            else
            {
                rejectHandler = resultPromise.RejectSilent;
            }
            
            Action cancelHandler = () => resultPromise.Cancel();

            ActionHandlers(resultPromise, resolveHandler, rejectHandler, cancelHandler);
            if (onProgress != null)
            {
                ProgressHandlers(this, onProgress);
            }

            return resultPromise;
        }

        /// <summary>
        /// Add a resolved callback, a rejected callback and a progress callback.
        /// </summary>
        public IPromise Then(Action onResolved, Action<Exception> onRejected, Action<float> onProgress)
        {
            if (CurState == PromiseState.Resolved)
            {
                try
                {
                    onResolved();
                    return this;
                }
                catch (Exception ex)
                {
                    EventsReceiver.OnHandlerException(ex);
                    return Rejected(ex);
                }
            }

            var resultPromise = new Promise();
            resultPromise.WithName(Name);
            resultPromise.AttachParent(this);

            Action resolveHandler;
            if (onResolved != null)
            {
                resolveHandler = () =>
                {
                    onResolved();
                    resultPromise.Resolve();
                };
            }
            else
            {
                resolveHandler = resultPromise.Resolve;
            }

            Action<Exception> rejectHandler;
            if (onRejected != null)
            {
                rejectHandler = ex =>
                {
                    if (onRejected != null)
                    {
                        onRejected(ex);
                        resultPromise.Resolve();
                        return;
                    }

                    resultPromise.RejectSilent(ex);
                };
            }
            else
            {
                rejectHandler = resultPromise.RejectSilent;
            }
            
            Action cancelHandler = () => resultPromise.Cancel();

            ActionHandlers(resultPromise, resolveHandler, rejectHandler, cancelHandler);
            if (onProgress != null)
            {
                ProgressHandlers(this, onProgress);
            }

            return resultPromise;
        }

        /// <summary>
        /// Helper function to invoke or register resolve/reject handlers.
        /// </summary>
        private void ActionHandlers(IRejectable resultPromise, Action resolveHandler, 
            Action<Exception> rejectHandler, Action cancelHandler)
        {
            if (CurState == PromiseState.Resolved)
            {
                InvokeResolveHandler(resolveHandler, resultPromise);
            }
            else if (CurState == PromiseState.Rejected)
            {
                InvokeRejectHandler(rejectHandler, resultPromise, _rejectionException);
            }
            else if (CurState == PromiseState.Cancelled)
            {
                InvokeCancelHandler(cancelHandler, resultPromise);
            }
            else
            {
                AddResolveHandler(resolveHandler, resultPromise);
                AddRejectHandler(rejectHandler, resultPromise);
                AddCancelHandler(cancelHandler, resultPromise);
            }
        }

        /// <summary>
        /// Helper function to invoke or register progress handlers.
        /// </summary>
        private void ProgressHandlers(IRejectable resultPromise, Action<float> progressHandler)
        {
            if (CurState == PromiseState.Pending)
            {
                AddProgressHandler(progressHandler, resultPromise);
            }
        }

        /// <summary>
        /// Chain an enumerable of promises, all of which must resolve.
        /// The resulting promise is resolved when all of the promises have resolved.
        /// It is rejected as soon as any of the promises have been rejected.
        /// </summary>
        public IPromise ThenAll(Func<IEnumerable<IPromise>> chain)
        {
            return Then(() => All(chain()));
        }

        /// <summary>
        /// Chain an enumerable of promises, all of which must resolve.
        /// Converts to a non-value promise.
        /// The resulting promise is resolved when all of the promises have resolved.
        /// It is rejected as soon as any of the promises have been rejected.
        /// </summary>
        public IPromise<IEnumerable<TConvertedT>> ThenAll<TConvertedT>(Func<IEnumerable<IPromise<TConvertedT>>> chain)
        {
            return Then(() => Promise<TConvertedT>.All(chain()));
        }

        /// <summary>
        /// Returns a promise that resolves when all of the promises in the enumerable argument have resolved.
        /// Returns a promise of a collection of the resolved results.
        /// </summary>
        public static IPromise All(params IPromise[] promises)
        {
            return All((IEnumerable<IPromise>)promises); // Cast is required to force use of the other All function.
        }

        /// <summary>
        /// Returns a promise that resolves when all of the promises in the enumerable argument have resolved.
        /// Returns a promise of a collection of the resolved results.
        /// </summary>
        public static IPromise All(IEnumerable<IPromise> promises)
        {
            var promisesArray = promises.ToArray();
            if (promisesArray.Length == 0)
            {
                return Resolved();
            }

            var remainingCount = promisesArray.Length;
            var resultPromise = new Promise();
            resultPromise.WithName("All");
            var progress = new float[remainingCount];

            promisesArray.Each((promise, index) =>
            {
                resultPromise.OnCancel(promise.Cancel);
                promise.OnCancel(resultPromise.Cancel);
                
                promise
                    .Progress(v =>
                    {
                        progress[index] = v;
                        if (resultPromise.CurState == PromiseState.Pending)
                        {
                            resultPromise.ReportProgress(progress.Average());
                        }
                    })
                    .Then(() =>
                    {
                        progress[index] = 1f;

                        --remainingCount;
                        if (remainingCount <= 0 && resultPromise.CurState == PromiseState.Pending)
                        {
                            // This will never happen if any of the promises errored.
                            resultPromise.Resolve();
                        }
                    })
                    .CatchAsResolved(ex =>
                    {
                        if (resultPromise.CurState == PromiseState.Pending)
                        {
                            // If a promise errored and the result promise is still pending, reject it.
                            resultPromise.RejectSilent(ex);
                        }
                    })
                    .Done();
            });

            return resultPromise;
        }

        /// <summary>
        /// Chain a sequence of operations using promises.
        /// Return a collection of functions each of which starts an async operation and yields a promise.
        /// Each function will be called and each promise resolved in turn.
        /// The resulting promise is resolved after each promise is resolved in sequence.
        /// </summary>
        public IPromise ThenSequence(Func<IEnumerable<Func<IPromise>>> chain)
        {
            return Then(() => Sequence(chain()));
        }

        /// <summary>
        /// Chain a number of operations using promises.
        /// Takes a number of functions each of which starts an async operation and yields a promise.
        /// </summary>
        public static IPromise Sequence(params Func<IPromise>[] fns)
        {
            return Sequence((IEnumerable<Func<IPromise>>)fns);
        }

        /// <summary>
        /// Chain a sequence of operations using promises.
        /// Takes a collection of functions each of which starts an async operation and yields a promise.
        /// </summary>
        public static IPromise Sequence(IEnumerable<Func<IPromise>> fns)
        {
            var promise = new Promise();

            int count = 0;

            fns.Aggregate(
                Resolved(),
                (prevPromise, fn) =>
                {
                    int itemSequence = count;
                    ++count;
                    
                    return prevPromise
                            .Then(() =>
                            {
                                var sliceLength = 1f / count;
                                promise.ReportProgress(sliceLength * itemSequence);
                                return fn();
                            })
                            .Progress(v =>
                            {
                                var sliceLength = 1f / count;
                                promise.ReportProgress(sliceLength * (v + itemSequence));
                            });
                }
            )
            .Then(promise.Resolve)
            .Catch(promise.Reject);

            return promise;
        }

        /// <summary>
        /// Takes a function that yields an enumerable of promises.
        /// Returns a promise that resolves when the first of the promises has resolved.
        /// </summary>
        public IPromise ThenRace(Func<IEnumerable<IPromise>> chain)
        {
            return Then(() => Race(chain()));
        }

        /// <summary>
        /// Takes a function that yields an enumerable of promises.
        /// Converts to a value promise.
        /// Returns a promise that resolves when the first of the promises has resolved.
        /// </summary>
        public IPromise<TConvertedT> ThenRace<TConvertedT>(Func<IEnumerable<IPromise<TConvertedT>>> chain)
        {
            return Then(() => Promise<TConvertedT>.Race(chain()));
        }

        /// <summary>
        /// Returns a promise that resolves when the first of the promises in the enumerable argument have resolved.
        /// Returns the value from the first promise that has resolved.
        /// </summary>
        public static IPromise Race(params IPromise[] promises)
        {
            return Race((IEnumerable<IPromise>)promises); // Cast is required to force use of the other function.
        }

        /// <summary>
        /// Returns a promise that resolves when the first of the promises in the enumerable argument have resolved.
        /// Returns the value from the first promise that has resolved.
        /// </summary>
        public static IPromise Race(IEnumerable<IPromise> promises)
        {
            var promisesArray = promises.ToArray();
            if (promisesArray.Length == 0)
            {
                var ex = new InvalidOperationException("At least 1 input promise must be provided for Race");
                EventsReceiver.OnInternalException(ex);
                return Rejected(ex);
            }

            var resultPromise = new Promise();
            resultPromise.WithName("Race");

            var progress = new float[promisesArray.Length];

            promisesArray.Each((promise, index) =>
            {
                resultPromise.OnCancel(promise.Cancel);
                
                promise
                    .Progress(v =>
                    {
                        progress[index] = v;
                        resultPromise.ReportProgress(progress.Max());
                    })
                    .CatchAsResolved(ex =>
                    {
                        if (resultPromise.CurState == PromiseState.Pending)
                        {
                            // If a promise errored and the result promise is still pending, reject it.
                            resultPromise.Reject(ex);
                        }
                    })
                    .Then(() =>
                    {
                        if (resultPromise.CurState == PromiseState.Pending)
                        {
                            resultPromise.Resolve();
                        }
                    })
                    .Done();
            });

            return resultPromise;
        }

        /// <summary>
        /// Convert a simple value directly into a resolved promise.
        /// </summary>
        private static readonly IPromise ResolvedPromise = new Promise(PromiseState.Resolved);
        
        public static IPromise Resolved()
        {
            return ResolvedPromise;
        }

        /// <summary>
        /// Convert an exception directly into a rejected promise.
        /// </summary>
        public static IPromise Rejected(Exception ex)
        {
            var promise = new Promise(PromiseState.Rejected);
            promise._rejectionException = ex;
            return promise;
        }
        
        /// <summary>
        /// Convert a simple value directly into a canceled promise.
        /// </summary>
        public static IPromise Canceled()
        {
            return new Promise(PromiseState.Cancelled);
        }

        public void Finally(Action onComplete)
        {
            Promise promise = new Promise();
            promise.WithName(Name);
            promise.AttachParent(this);

            Then(() => { promise.Resolve(); });
            Catch(e => { promise.Resolve(); });
            OnCancel(() => promise.Resolve());

            promise.Then(onComplete);
        }

        public IPromise ContinueWith(Func<IPromise> onComplete)
        {
            var promise = new Promise();
            promise.WithName(Name);
            promise.AttachParent(this);

            Then(promise.Resolve);
            Catch(e => promise.Resolve());

            return promise.Then(onComplete);
        }

        public IPromise ContinueWithResolved(Action onResolved)
        {
            var promise = new Promise();
            promise.WithName(Name);
            promise.AttachParent(this);

            Then(promise.Resolve);
            Catch(e => promise.Resolve());

            return promise.Then(() =>
            {
                try
                {
                    onResolved();
                }
                catch (Exception e)
                {
                    EventsReceiver.OnHandlerException(e);
                    return Rejected(e);
                }
                
                return Resolved();
            });
        }

        public IPromise<TConvertedT> ContinueWith<TConvertedT>(Func<IPromise<TConvertedT>> onComplete)
        {
            var promise = new Promise();
            promise.WithName(Name);
            promise.AttachParent(this);

            Then(promise.Resolve);
            Catch(e => promise.Resolve());

            return promise.Then(onComplete);
        }

        public IPromise Progress(Action<float> onProgress)
        {
            if (CurState == PromiseState.Pending && onProgress != null)
            {
                ProgressHandlers(this, onProgress);
            }
            return this;
        }

        /// <summary>
        /// Raises the UnhandledException event.
        /// </summary>
        internal static void PropagateUnhandledException(object sender, Exception ex)
        {
            if (_unhandlerException != null)
            {
                _unhandlerException(sender, new ExceptionEventArgs(ex));
            }
        }
    }
}
