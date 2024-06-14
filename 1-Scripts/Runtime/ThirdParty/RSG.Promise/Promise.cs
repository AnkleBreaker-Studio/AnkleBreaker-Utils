using System;
using System.Collections.Generic;
using System.Linq;
using RSG.Exceptions;
using RSG.Promises;

// ReSharper disable once CheckNamespace
namespace RSG
{
    /// <summary>
    /// Implements a C# promise.
    /// https://developer.mozilla.org/en/docs/Web/JavaScript/Reference/Global_Objects/Promise
    /// </summary>
    public interface IPromise<out TPromisedT> : ICancellablePromise
    {
        /// <summary>
        /// Gets the id of the promise, useful for referencing the promise during runtime.
        /// </summary>
        int Id { get; }

        /// <summary>
        /// Set the name of the promise, useful for debugging.
        /// </summary>
        IPromise<TPromisedT> WithName(string name);
        
        /// <summary>
        /// Completes the promise. 
        /// onResolved is called on successful completion.
        /// onRejected is called on error.
        /// </summary>
        void Done(Action<TPromisedT> onResolved, Action<Exception> onRejected);

        /// <summary>
        /// Completes the promise. 
        /// onResolved is called on successful completion.
        /// Adds a default error handler.
        /// </summary>
        void Done(Action<TPromisedT> onResolved);

        /// <summary>
        /// Complete the promise. Adds a default error handler.
        /// </summary>
        void Done();

        /// <summary>
        /// Handle errors for the promise. Resolves the promise afterwards.
        /// </summary>
        IPromise CatchAsResolved(Action<Exception> onRejected);
        
        /// <summary>
        /// Handle errors for the promise. Rejects the promise after the Catch.
        /// </summary>
        IPromise<TPromisedT> Catch(Action<Exception> onRejected);
        
        /// <summary>
        /// Add a resolved callback that chains a value promise (optionally converting to a different value type).
        /// </summary>
        IPromise<TConvertedT> Then<TConvertedT>(Func<TPromisedT, IPromise<TConvertedT>> onResolved);

        /// <summary>
        /// Add a resolved callback that chains a non-value promise.
        /// </summary>
        IPromise Then(Func<TPromisedT, IPromise> onResolved);

        /// <summary>
        /// Add a resolved callback.
        /// </summary>
        IPromise ThenAsNonGeneric(Action<TPromisedT> onResolved);
        
        /// <summary>
        /// Add a resolved callback.
        /// </summary>
        IPromise<TPromisedT> Then(Action<TPromisedT> onResolved);

        /// <summary>
        /// Add a resolved callback and a rejected callback.
        /// The resolved callback chains a value promise (optionally converting to a different value type).
        /// </summary>
        IPromise<TConvertedT> Then<TConvertedT>(
            Func<TPromisedT, IPromise<TConvertedT>> onResolved, 
            Func<Exception, IPromise<TConvertedT>> onRejected
        );

        /// <summary>
        /// Add a resolved callback and a rejected callback.
        /// The resolved callback chains a non-value promise.
        /// </summary>
        IPromise Then(Func<TPromisedT, IPromise> onResolved, Action<Exception> onRejected);

        /// <summary>
        /// Add a resolved callback and a rejected callback.
        /// </summary>
        IPromise Then(Action<TPromisedT> onResolved, Action<Exception> onRejected);

        /// <summary>
        /// Add a resolved callback, a rejected callback and a progress callback.
        /// The resolved callback chains a value promise (optionally converting to a different value type).
        /// </summary>
        IPromise<TConvertedT> Then<TConvertedT>(
            Func<TPromisedT, IPromise<TConvertedT>> onResolved,
            Func<Exception, IPromise<TConvertedT>> onRejected,
            Action<float> onProgress
        );

        /// <summary>
        /// Add a resolved callback, a rejected callback and a progress callback.
        /// The resolved callback chains a non-value promise.
        /// </summary>
        IPromise Then(Func<TPromisedT, IPromise> onResolved, Action<Exception> onRejected, Action<float> onProgress);

        /// <summary>
        /// Add a resolved callback, a rejected callback and a progress callback.
        /// </summary>
        IPromise Then(Action<TPromisedT> onResolved, Action<Exception> onRejected, Action<float> onProgress);

        /// <summary>
        /// Return a new promise with a different value.
        /// May also change the type of the value.
        /// </summary>
        IPromise<TConvertedT> Then<TConvertedT>(Func<TPromisedT, TConvertedT> transform);

        /// <summary>
        /// Chain an enumerable of promises, all of which must resolve.
        /// Returns a promise for a collection of the resolved results.
        /// The resulting promise is resolved when all of the promises have resolved.
        /// It is rejected as soon as any of the promises have been rejected.
        /// </summary>
        IPromise<IEnumerable<TConvertedT>> ThenAll<TConvertedT>(Func<TPromisedT, IEnumerable<IPromise<TConvertedT>>> chain);

        /// <summary>
        /// Chain an enumerable of promises, all of which must resolve.
        /// Converts to a non-value promise.
        /// The resulting promise is resolved when all of the promises have resolved.
        /// It is rejected as soon as any of the promises have been rejected.
        /// </summary>
        IPromise ThenAll(Func<TPromisedT, IEnumerable<IPromise>> chain);

        /// <summary>
        /// Takes a function that yields an enumerable of promises.
        /// Returns a promise that resolves when the first of the promises has resolved.
        /// Yields the value from the first promise that has resolved.
        /// </summary>
        IPromise<TConvertedT> ThenRace<TConvertedT>(Func<TPromisedT, IEnumerable<IPromise<TConvertedT>>> chain);

        /// <summary>
        /// Takes a function that yields an enumerable of promises.
        /// Converts to a non-value promise.
        /// Returns a promise that resolves when the first of the promises has resolved.
        /// Yields the value from the first promise that has resolved.
        /// </summary>
        IPromise ThenRace(Func<TPromisedT, IEnumerable<IPromise>> chain);

        /// <summary> 
        /// Add a finally callback. 
        /// Finally callbacks will always be called, even if any preceding promise is cancelled, rejected, or encounters an error.
        /// The returned promise will be resolved, rejected or cancelled as per the preceding promise.
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
        IPromise<TPromisedT> Progress(Action<float> onProgress);
        
        /// <summary>
        /// Add a cancel callback.
        /// </summary>
        /// <param name="onCancel"></param>
        void OnCancel(Action onCancel);
    }

    /// <summary>
    /// Interface for a promise that can be rejected.
    /// </summary>
    public interface IRejectable
    {
        /// <summary>
        /// Reject the promise with an exception. Doesn't use LogError.
        /// </summary>
        void RejectSilent(Exception ex);
        
        /// <summary>
        /// Reject the promise with an exception and LogError log.
        /// </summary>
        void Reject(Exception ex);
    }

    /// <summary>
    /// Interface for a promise that can be rejected or resolved.
    /// </summary>
    public interface IPendingPromise<TPromisedT> : IRejectable
    {
        /// <summary>
        /// ID of the promise, useful for debugging.
        /// </summary>
        int Id { get; }
        
        /// <summary>
        /// Just a shortcut to check whether the promise is in Pending state.
        /// </summary>
        bool CanBeResolved { get; }

        /// <summary>
        /// Resolve the promise with a particular value.
        /// </summary>
        void Resolve(TPromisedT value);

        /// <summary>
        /// Report progress in a promise.
        /// </summary>
        void ReportProgress(float progress);
    }

    /// <summary>
    /// Specifies the state of a promise.
    /// </summary>
    public enum PromiseState
    {
        Pending,    // The promise is in-flight.
        Rejected,   // The promise has been rejected.
        Resolved,    // The promise has been resolved.
        Cancelled   // The promise has been cancelled.
    }

    /// <summary>
    /// Implements a C# promise.
    /// https://developer.mozilla.org/en/docs/Web/JavaScript/Reference/Global_Objects/Promise
    /// </summary>
    public class Promise<TPromisedT> : IPromise<TPromisedT>, IPendingPromise<TPromisedT>, IPromiseInfo
    {
        /// <summary>
        /// The exception when the promise is rejected.
        /// </summary>
        private Exception _rejectionException;

        /// <summary>
        /// The value when the promises is resolved.
        /// </summary>
        private TPromisedT _resolveValue;

        /// <summary>
        /// Error handler.
        /// </summary>
        private List<RejectHandler> _rejectHandlers;

        /// <summary>
        /// Cancel handler.
        /// </summary>
        private List<Promise.ResolveHandler> _cancelHandlers;

        /// <summary>
        /// Progress handlers.
        /// </summary>
        private List<ProgressHandler> _progressHandlers;

        /// <summary>
        /// Completed handlers that accept a value.
        /// </summary>
        private List<Action<TPromisedT>> _resolveCallbacks;

        private List<IRejectable> _resolveRejectables;

        /// <summary>
        /// ID of the promise, useful for debugging.
        /// </summary>
        public int Id => _id;

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
        /// Shortcut property for checking if promise is pending.
        /// </summary>
        public bool CanBeResolved => CurState == PromiseState.Pending;

        /// <summary>
        /// Shortcut property for checking if promise is pending.
        /// </summary>
        public bool CanBeCanceled => CurState == PromiseState.Pending;

        /// <summary>
        /// Promise parent in chain.
        /// </summary>
        public ICancellablePromise Parent { get; private set; }

        /// <summary>
        /// Promise children in chain.
        /// </summary>
        public HashSet<ICancellablePromise> Children { get; } = new HashSet<ICancellablePromise>();

        /// <summary>
        /// Get loggable name.
        /// </summary>
        /// <returns></returns>
        public string GetName()
        {
            return string.IsNullOrEmpty(Name) ? "Promise" : $"Promise = {Name}";
        }

        public Promise(string name = null)
        {
            CurState = PromiseState.Pending;
            _id = Promise.NextId();
            Name = name;

            if (Promise.EnablePromiseTracking)
            {
                Promise.PendingPromises.Add(this);
            }
        }

        public Promise(Action<Action<TPromisedT>, Action<Exception>> resolver)
        {
            CurState = PromiseState.Pending;
            _id = Promise.NextId();

            if (Promise.EnablePromiseTracking)
            {
                Promise.PendingPromises.Add(this);
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
            _id = Promise.NextId();
        }

        /// <summary>
        /// Attach a parent in chain.
        /// </summary>
        /// <param name="parent"></param>
        public void AttachParent(ICancellablePromise parent)
        {
            if (parent.Parent == this)
            {
                EventsReceiver.OnWarningMinor(
                    $"Skip attempt to create cycled refs in promises parents {GetName()}");
                return;
            }

            if (Parent != null)
            {
                EventsReceiver.OnWarningMinor($"Overwriting existing parent {GetName()}");
            }

            Parent = parent;
            parent.AttachChild(this);
        }

        /// <summary>
        /// Add a child in chain.
        /// </summary>
        /// <param name="child"></param>
        public void AttachChild(ICancellablePromise child)
        {
            Children.Add(child);
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

            _rejectHandlers.Add(new RejectHandler { Callback = onRejected, Rejectable = rejectable });
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
                _cancelHandlers = new List<Promise.ResolveHandler>();
            }

            _cancelHandlers.Add(new Promise.ResolveHandler
            {
                Callback = onCanceled,
                Rejectable = rejectable
            });
        }

        /// <summary>
        /// Add a resolve handler for this promise.
        /// </summary>
        private void AddResolveHandler(Action<TPromisedT> onResolved, IRejectable rejectable)
        {
            if (_resolveCallbacks == null)
            {
                _resolveCallbacks = new List<Action<TPromisedT>>();
            }

            if (_resolveRejectables == null)
            {
                _resolveRejectables = new List<IRejectable>();
            }

            _resolveCallbacks.Add(onResolved);
            _resolveRejectables.Add(rejectable);
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
        /// Invoke a single handler.
        /// </summary>
        private void InvokeHandler<T>(Action<T> callback, IRejectable rejectable, T value)
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
        /// Helper function clear out all handlers after resolution or rejection.
        /// </summary>
        private void ClearHandlers()
        {
            _rejectHandlers = null;
            _resolveCallbacks = null;
            _resolveRejectables = null;
            _cancelHandlers = null;
            _progressHandlers = null;
        }

        /// <summary>
        /// Invoke all reject handlers.
        /// </summary>
        private void InvokeRejectHandlers(Exception ex)
        {
            if (_rejectHandlers != null)
            {
                for (int i = 0, maxI = _rejectHandlers.Count; i < maxI; ++i)
                    InvokeHandler(_rejectHandlers[i].Callback, _rejectHandlers[i].Rejectable, ex);
            }

            ClearHandlers();
        }

        /// <summary>
        /// Invoke all resolve handlers.
        /// </summary>
        private void InvokeResolveHandlers(TPromisedT value)
        {
            if (_resolveCallbacks != null)
            {
                for (int i = 0, maxI = _resolveCallbacks.Count; i < maxI; i++)
                {
                    InvokeHandler(_resolveCallbacks[i], _resolveRejectables[i], value);
                }
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
        /// Invoke all progress handlers.
        /// </summary>
        private void InvokeProgressHandlers(float progress)
        {
            if (_progressHandlers != null)
            {
                for (int i = 0, maxI = _progressHandlers.Count; i < maxI; ++i)
                    InvokeHandler(_progressHandlers[i].Callback, _progressHandlers[i].Rejectable, progress);
            }
        }

        /// <summary>
        /// Reject the promise with an exception. Calls OnRejectException log.
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
        /// Reject the promise with an exception. Calls OnRejectSilentException.
        /// This kind of log can be avoided in you EventReceiver implementation.
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

            if (Promise.EnablePromiseTracking)
            {
                Promise.PendingPromises.Remove(this);
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
            if (Promise.EnablePromiseTracking)
            {
                Promise.PendingPromises.Remove(this);
            }
            InvokeCancelHandlers();
            ClearHandlers();
        }

        public bool TryResolve(TPromisedT value)
        {
            if (CurState != PromiseState.Pending)
            {
                return false;
            }
            
            Resolve(value);
            return true;
        }

        /// <summary>
        /// Resolve the promise with a particular value.
        /// </summary>
        public void Resolve(TPromisedT value)
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

            _resolveValue = value;
            CurState = PromiseState.Resolved;

            if (Promise.EnablePromiseTracking)
            {
                Promise.PendingPromises.Remove(this);
            }

            InvokeResolveHandlers(value);
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
        public void Done(Action<TPromisedT> onResolved, Action<Exception> onRejected)
        {
            Then(onResolved, onRejected)
                .CatchAsResolved(ex =>
                    Promise.PropagateUnhandledException(this, ex)
                );
        }

        /// <summary>
        /// Completes the promise. 
        /// onResolved is called on successful completion.
        /// Adds a default error handler.
        /// </summary>
        public void Done(Action<TPromisedT> onResolved)
        {
            ThenAsNonGeneric(onResolved)
                .CatchAsResolved(ex =>
                    Promise.PropagateUnhandledException(this, ex)
                );
        }

        /// <summary>
        /// Complete the promise. Adds a default error handler.
        /// </summary>
        public void Done()
        {
            if (CurState == PromiseState.Resolved)
                return;

            CatchAsResolved(ex =>
                Promise.PropagateUnhandledException(this, ex)
            );
        }

        /// <summary>
        /// Set the name of the promise, useful for debugging.
        /// </summary>
        public IPromise<TPromisedT> WithName(string name)
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
                return Promise.Resolved();
            }

            var resultPromise = new Promise();
            resultPromise.WithName(Name);
            resultPromise.AttachParent(this);

            Action<TPromisedT> resolveHandler = _ => resultPromise.Resolve();

            Action<Exception> rejectHandler = ex =>
            {
                try
                {
                    onRejected(ex);
                    resultPromise.Resolve();
                }
                catch(Exception cbEx)
                {
                    EventsReceiver.OnHandlerException(cbEx);
                    resultPromise.RejectSilent(cbEx);
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
        public IPromise<TPromisedT> Catch(Action<Exception> onRejected)
        {
            if (CurState == PromiseState.Resolved)
            {
                return this;
            }

            var resultPromise = new Promise<TPromisedT>();
            resultPromise.WithName(Name);
            resultPromise.AttachParent(this);

            Action<TPromisedT> resolveHandler = v => resultPromise.Resolve(v);

            Action<Exception> rejectHandler = ex =>
            {
                try
                {
                    onRejected(ex);
                    resultPromise.RejectSilent(ex);
                }
                catch(Exception cbEx)
                {
                    EventsReceiver.OnHandlerException(cbEx);
                    resultPromise.RejectSilent(cbEx);
                }
            };
            
            Action cancelHandler = () => resultPromise.Cancel();

            ActionHandlers(resultPromise, resolveHandler, rejectHandler, cancelHandler);
            ProgressHandlers(resultPromise, v => resultPromise.ReportProgress(v));

            return resultPromise;
        }

        /// <summary>
        /// Handle cancel for the promise
        /// </summary>
        /// <param name="onCancel"></param>
        public void OnCancel(Action onCancel)
        {
            ActionHandlers(this, v => { }, ex => { }, onCancel);
        }

        /// <summary>
        /// Add a resolved callback that chains a value promise (optionally converting to a different value type).
        /// </summary>
        public IPromise<TConvertedT> Then<TConvertedT>(Func<TPromisedT, IPromise<TConvertedT>> onResolved)
        {
            return Then(onResolved, null, null);
        }

        /// <summary>
        /// Add a resolved callback that chains a non-value promise.
        /// </summary>
        public IPromise Then(Func<TPromisedT, IPromise> onResolved)
        {
            return Then(onResolved, null, null);
        }

        /// <summary>
        /// Add a resolved callback.
        /// </summary>
        public IPromise ThenAsNonGeneric(Action<TPromisedT> onResolved)
        {
            return Then(onResolved, null, null);
        }
        
        /// <summary>
        /// Add a resolved callback and a rejected callback.
        /// The resolved callback chains a value promise (optionally converting to a different value type).
        /// </summary>
        public IPromise<TConvertedT> Then<TConvertedT>(
            Func<TPromisedT, IPromise<TConvertedT>> onResolved,
            Func<Exception, IPromise<TConvertedT>> onRejected
        )
        {
            return Then(onResolved, onRejected, null);
        }

        /// <summary>
        /// Add a resolved callback and a rejected callback.
        /// The resolved callback chains a non-value promise.
        /// </summary>
        public IPromise Then(Func<TPromisedT, IPromise> onResolved, Action<Exception> onRejected)
        {
            return Then(onResolved, onRejected, null);
        }

        /// <summary>
        /// Add a resolved callback and a rejected callback.
        /// </summary>
        public IPromise Then(Action<TPromisedT> onResolved, Action<Exception> onRejected)
        {
            return Then(onResolved, onRejected, null);
        }

        /// <summary>
        /// Add a resolved callback.
        /// </summary>
        public IPromise<TPromisedT> Then(Action<TPromisedT> onResolved)
        {
            return Then(v=>
            {
                onResolved(v);
                return Resolved(v);
            }, null, null);
        }

        /// <summary>
        /// Add a resolved callback, a rejected callback and a progress callback.
        /// The resolved callback chains a value promise (optionally converting to a different value type).
        /// </summary>
        public IPromise<TConvertedT> Then<TConvertedT>(
            Func<TPromisedT, IPromise<TConvertedT>> onResolved, 
            Func<Exception, IPromise<TConvertedT>> onRejected,
            Action<float> onProgress
        )
        {
            if (CurState == PromiseState.Resolved)
            {
                try
                {
                    return onResolved(_resolveValue);
                }
                catch (Exception ex)
                {
                    EventsReceiver.OnHandlerException(ex);
                    return Promise<TConvertedT>.Rejected(ex);
                }
            }

            // This version of the function must supply an onResolved.
            // Otherwise there is no way to get the converted value to pass to the resulting promise.

            var resultPromise = new Promise<TConvertedT>();
            resultPromise.WithName(Name);
            resultPromise.AttachParent(this);

            Action<TPromisedT> resolveHandler = v =>
            {
                onResolved(v)
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
        public IPromise Then(Func<TPromisedT, IPromise> onResolved, Action<Exception> onRejected, Action<float> onProgress)
        {
            if (CurState == PromiseState.Resolved)
            {
                try
                {
                    return onResolved(_resolveValue);
                }
                catch (Exception ex)
                {
                    EventsReceiver.OnHandlerException(ex);
                    return Promise.Rejected(ex);
                }
            }

            var resultPromise = new Promise();
            resultPromise.WithName(Name);
            resultPromise.AttachParent(this);

            Action<TPromisedT> resolveHandler = v =>
            {
                if (onResolved != null)
                {
                    onResolved(v)
                        .Progress(progress => resultPromise.ReportProgress(progress))
                        .Then(
                            () => resultPromise.Resolve(),
                            ex => resultPromise.RejectSilent(ex)
                        )
                        .OnCancel(() => resultPromise.Cancel());
                }
                else
                {
                    resultPromise.Resolve();
                }
            };

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
        public IPromise Then(Action<TPromisedT> onResolved, Action<Exception> onRejected, Action<float> onProgress)
        {
            if (CurState == PromiseState.Resolved)
            {
                try
                {
                    onResolved(_resolveValue);
                    return Promise.Resolved();
                }
                catch (Exception ex)
                {
                    EventsReceiver.OnHandlerException(ex);
                    return Promise.Rejected(ex);
                }
            }

            var resultPromise = new Promise();
            resultPromise.WithName(Name);
            resultPromise.AttachParent(this);

            Action<TPromisedT> resolveHandler = v =>
            {
                if (onResolved != null)
                {
                    onResolved(v);
                }

                resultPromise.Resolve();
            };

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
        /// Return a new promise with a different value.
        /// May also change the type of the value.
        /// </summary>
        public IPromise<TConvertedT> Then<TConvertedT>(Func<TPromisedT, TConvertedT> transform)
        {
            return Then(value => Promise<TConvertedT>.Resolved(transform(value)));
        }

        /// <summary>
        /// Helper function to invoke or register resolve/reject handlers.
        /// </summary>
        private void ActionHandlers(IRejectable resultPromise, Action<TPromisedT> resolveHandler, 
            Action<Exception> rejectHandler, Action cancelHandler)
        {
            if (CurState == PromiseState.Resolved)
            {
                InvokeHandler(resolveHandler, resultPromise, _resolveValue);
            }
            else if (CurState == PromiseState.Rejected)
            {
                InvokeHandler(rejectHandler, resultPromise, _rejectionException);
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
        /// Chain a number of operations using promises.
        /// Returns the value of the first promise that resolves, or otherwise the exception thrown by the last operation.
        /// </summary>
        public static IPromise<T> First<T>(params Func<IPromise<T>>[] fns)
        {
            return First((IEnumerable<Func<IPromise<T>>>)fns);
        }

        /// <summary>
        /// Chain a number of operations using promises.
        /// Returns the value of the first promise that resolves, or otherwise the exception thrown by the last operation.
        /// </summary>
        public static IPromise<T> First<T>(IEnumerable<Func<IPromise<T>>> fns)
        {
            var promise = new Promise<T>();

            int count = 0;

            fns.Aggregate(
                Promise<T>.Rejected(null),
                (prevPromise, fn) =>
                {
                    int itemSequence = count;
                    ++count;

                    var newPromise = new Promise<T>();
                    
                    prevPromise
                        .Progress(v =>
                        {
                            var sliceLength = 1f / count;
                            promise.ReportProgress(sliceLength * (v + itemSequence));
                        })
                        .Then(newPromise.Resolve)
                        .Catch(ex =>
                        {
                            var sliceLength = 1f / count;
                            promise.ReportProgress(sliceLength * itemSequence);

                            fn()
                                .Then(value => newPromise.Resolve(value))
                                .Catch(newPromise.Reject)
                                .Done()
                            ;
                        });

                    return newPromise;
                })
            .Then(value => promise.Resolve(value))
            .Catch(ex =>
            {
                promise.ReportProgress(1f);
                promise.Reject(ex);
            });
            
            return promise;
        }

        /// <summary>
        /// Chain an enumerable of promises, all of which must resolve.
        /// Returns a promise for a collection of the resolved results.
        /// The resulting promise is resolved when all of the promises have resolved.
        /// It is rejected as soon as any of the promises have been rejected.
        /// </summary>
        public IPromise<IEnumerable<TConvertedT>> ThenAll<TConvertedT>(Func<TPromisedT, IEnumerable<IPromise<TConvertedT>>> chain)
        {
            return Then(value => Promise<TConvertedT>.All(chain(value)));
        }

        /// <summary>
        /// Chain an enumerable of promises, all of which must resolve.
        /// Converts to a non-value promise.
        /// The resulting promise is resolved when all of the promises have resolved.
        /// It is rejected as soon as any of the promises have been rejected.
        /// </summary>
        public IPromise ThenAll(Func<TPromisedT, IEnumerable<IPromise>> chain)
        {
            return Then(value => Promise.All(chain(value)));
        }

        /// <summary>
        /// Returns a promise that resolves when all of the promises in the enumerable argument have resolved.
        /// Returns a promise of a collection of the resolved results.
        /// </summary>
        public static IPromise<IEnumerable<TPromisedT>> All(params IPromise<TPromisedT>[] promises)
        {
            return All((IEnumerable<IPromise<TPromisedT>>)promises); // Cast is required to force use of the other All function.
        }

        /// <summary>
        /// Returns a promise that resolves when all of the promises in the enumerable argument have resolved.
        /// Returns a promise of a collection of the resolved results.
        /// </summary>
        public static IPromise<IEnumerable<TPromisedT>> All(IEnumerable<IPromise<TPromisedT>> promises, Action<IPromise<TPromisedT>, TPromisedT> onIntermediate = null)
        {
            var promisesArray = promises.ToArray();
            if (promisesArray.Length == 0)
            {
                return Promise<IEnumerable<TPromisedT>>.Resolved(Enumerable.Empty<TPromisedT>());
            }

            var remainingCount = promisesArray.Length;
            var results = new TPromisedT[remainingCount];
            var progress = new float[remainingCount];
            var resultPromise = new Promise<IEnumerable<TPromisedT>>();
            resultPromise.WithName("All");

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
                    .Then(result =>
                    {
                        progress[index] = 1f;
                        results[index] = result;

                        onIntermediate?.Invoke(promise, result);

                        --remainingCount;
                        if (remainingCount <= 0 && resultPromise.CurState == PromiseState.Pending)
                        {
                            // This will never happen if any of the promises errorred.
                            resultPromise.Resolve(results);
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
        /// Takes a function that yields an enumerable of promises.
        /// Returns a promise that resolves when the first of the promises has resolved.
        /// Yields the value from the first promise that has resolved.
        /// </summary>
        public IPromise<TConvertedT> ThenRace<TConvertedT>(Func<TPromisedT, IEnumerable<IPromise<TConvertedT>>> chain)
        {
            return Then(value => Promise<TConvertedT>.Race(chain(value)));
        }

        /// <summary>
        /// Takes a function that yields an enumerable of promises.
        /// Converts to a non-value promise.
        /// Returns a promise that resolves when the first of the promises has resolved.
        /// Yields the value from the first promise that has resolved.
        /// </summary>
        public IPromise ThenRace(Func<TPromisedT, IEnumerable<IPromise>> chain)
        {
            return Then(value => Promise.Race(chain(value)));
        }

        /// <summary>
        /// Returns a promise that resolves when the first of the promises in the enumerable argument have resolved.
        /// Returns the value from the first promise that has resolved.
        /// </summary>
        public static IPromise<TPromisedT> Race(params IPromise<TPromisedT>[] promises)
        {
            return Race((IEnumerable<IPromise<TPromisedT>>)promises); // Cast is required to force use of the other function.
        }

        /// <summary>
        /// Returns a promise that resolves when the first of the promises in the enumerable argument have resolved.
        /// Returns the value from the first promise that has resolved.
        /// </summary>
        public static IPromise<TPromisedT> Race(IEnumerable<IPromise<TPromisedT>> promises)
        {
            var promisesArray = promises.ToArray();
            if (promisesArray.Length == 0)
            {
                var ex = new InvalidOperationException(
                    "At least 1 input promise must be provided for Race"
                );
                EventsReceiver.OnInternalException(ex);
                return Rejected(ex);
            }

            var resultPromise = new Promise<TPromisedT>();
            resultPromise.WithName("Race");

            var progress = new float[promisesArray.Length];

            promisesArray.Each((promise, index) =>
            {
                resultPromise.OnCancel(promise.Cancel);
                
                promise
                    .Progress(v =>
                    {
                        if (resultPromise.CurState == PromiseState.Pending)
                        {
                            progress[index] = v;
                            resultPromise.ReportProgress(progress.Max());
                        }
                    })
                    .Then(result =>
                    {
                        if (resultPromise.CurState == PromiseState.Pending)
                        {
                            resultPromise.Resolve(result);
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
        /// Convert a simple value directly into a resolved promise.
        /// </summary>
        public static IPromise<TPromisedT> Resolved(TPromisedT promisedValue)
        {
            var promise = new Promise<TPromisedT>(PromiseState.Resolved);
            promise._resolveValue = promisedValue;
            return promise;
        }

        /// <summary>
        /// Convert an exception directly into a rejected promise.
        /// </summary>
        public static IPromise<TPromisedT> Rejected(Exception ex)
        {
            var promise = new Promise<TPromisedT>(PromiseState.Rejected);
            promise._rejectionException = ex;
            return promise;
        }
        
        /// <summary>
        /// Convert a simple value directly into a canceled promise.
        /// </summary>
        public static IPromise<TPromisedT> Canceled()
        {
            return new Promise<TPromisedT>(PromiseState.Cancelled);
        }

        public void Finally(Action onComplete)
        {
            Promise promise = new Promise();
            promise.WithName(Name);
            promise.AttachParent(this);

            ThenAsNonGeneric(x => { promise.Resolve(); });
            Catch(e => { promise.Resolve(); });
            OnCancel(() => promise.Resolve());

            promise.Then(onComplete);
        }

        public IPromise ContinueWith(Func<IPromise> onComplete)
        {
            var promise = new Promise();
            promise.WithName(Name);
            promise.AttachParent(this);

            ThenAsNonGeneric(x => promise.Resolve());
            Catch(e => promise.Resolve());

            return promise.Then(onComplete);
        }

        public IPromise ContinueWithResolved(Action onResolved)
        {
            var promise = new Promise();
            promise.WithName(Name);
            promise.AttachParent(this);

            ThenAsNonGeneric(x => promise.Resolve());
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
                    return Promise.Rejected(e);
                }
                
                return Promise.Resolved();
            });
        }

        public IPromise<TConvertedT> ContinueWith<TConvertedT>(Func<IPromise<TConvertedT>> onComplete)
        {
            var promise = new Promise();
            promise.WithName(Name);
            promise.AttachParent(this);

            ThenAsNonGeneric(x => promise.Resolve());
            Catch(e => promise.Resolve());

            return promise.Then(onComplete);
        }

        public IPromise<TPromisedT> Progress(Action<float> onProgress)
        {
            if (CurState == PromiseState.Pending && onProgress != null)
            {
                ProgressHandlers(this, onProgress);
            }
            return this;
        }

        public static IPromise<T> Canceled<T>()
        {
            var promise = new Promise<T>();
            promise.Cancel();
            return promise;
        }
    }
}
