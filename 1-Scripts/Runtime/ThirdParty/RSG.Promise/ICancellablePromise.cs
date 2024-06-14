using System.Collections.Generic;

// ReSharper disable once CheckNamespace
namespace RSG
{
    public interface ICancellablePromise
    {
        /// <summary>
        /// Current state of promise
        /// </summary>
        PromiseState CurState { get; }
        
        /// <summary>
        /// Shortcut to check whether a promise is in pending state.
        /// </summary>
        bool CanBeCanceled { get; }
        
        /// <summary>
        /// A promise that was a source for creation the new one.
        /// </summary>
        ICancellablePromise Parent { get; }
        
        /// <summary>
        /// Promises which were created by applying callbacks to this one.
        /// </summary>
        HashSet<ICancellablePromise> Children { get; }
        
        /// <summary>
        /// Register parent. There can only be one parent.
        /// </summary>
        /// <param name="parent"></param>
        void AttachParent(ICancellablePromise parent);
        
        /// <summary>
        /// Registers a new child.
        /// </summary>
        /// <param name="child"></param>
        void AttachChild(ICancellablePromise child);
        
        /// <summary>
        /// Cancels the whole chain where this promise exists.
        /// </summary>
        void Cancel();
    }
}