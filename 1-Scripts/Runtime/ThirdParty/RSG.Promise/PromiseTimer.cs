using System;
using System.Collections.Generic;

namespace RSG
{

    public class PromiseCancelledException : Exception
    {
        /// <summary>
        /// Just create the exception
        /// </summary>
        public PromiseCancelledException()
        {

        }

        /// <summary>
        /// Create the exception with description
        /// </summary>
        /// <param name="message">Exception description</param>
        public PromiseCancelledException(String message) : base(message)
        {

        }
    }

    /// <summary>
    /// A class that wraps a pending promise with it's predicate and time data
    /// </summary>
    internal class PredicateWait
    {
        /// <summary>
        /// Predicate for resolving the promise
        /// </summary>
        public Func<TimeData, bool> Predicate;

        /// <summary>
        /// The time the promise was started
        /// </summary>
        public float TimeStarted;

        /// <summary>
        /// The pending promise which is an interface for a promise that can be rejected or resolved.
        /// </summary>
        public IPendingPromise PendingPromise;

        /// <summary>
        /// The time data specific to this pending promise. Includes elapsed time and delta time.
        /// </summary>
        public TimeData TimeData;

        /// <summary>
        /// The frame the promise was started
        /// </summary>
        public int FrameStarted;
    }

    /// <summary>
    /// Time data specific to a particular pending promise.
    /// </summary>
    public struct TimeData
    {
        /// <summary>
        /// The amount of time that has elapsed since the pending promise started running
        /// </summary>
        public float ElapsedTime;

        /// <summary>
        /// The amount of time since the last time the pending promise was updated.
        /// </summary>
        public float DeltaTime;

        /// <summary>
        /// The amount of times that update has been called since the pending promise started running
        /// </summary>
        public int ElapsedUpdates;
    }

    public interface IPromiseTimer
    {
        /// <summary>
        /// Resolve the returned promise once the time has elapsed
        /// </summary>
        IPromise WaitFor(float seconds);

        /// <summary>
        /// Resolve the returned promise once the predicate evaluates to true
        /// </summary>
        IPromise WaitUntil(Func<TimeData, bool> predicate);

        /// <summary>
        /// Resolve the returned promise once the predicate evaluates to false
        /// </summary>
        IPromise WaitWhile(Func<TimeData, bool> predicate);

        /// <summary>
        /// Update all pending promises. Must be called for the promises to progress and resolve at all.
        /// </summary>
        void Update(float deltaTime);

        /// <summary>
        /// Cancel a waiting promise and reject it immediately.
        /// </summary>
        bool Cancel(IPromise promise);
    }

    public class PromiseTimer : IPromiseTimer
    {
        /// <summary>
        /// The current running total for time that this PromiseTimer has run for
        /// </summary>
        private float _curTime;

        /// <summary>
        /// The current running total for the amount of frames the PromiseTimer has run for
        /// </summary>
        private int _curFrame;

        /// <summary>
        /// Currently pending promises
        /// </summary>
        private readonly LinkedList<PredicateWait> _waiting = new LinkedList<PredicateWait>();

        /// <summary>
        /// Resolve the returned promise once the time has elapsed
        /// </summary>
        public IPromise WaitFor(float seconds)
        {
            return WaitUntil(t => t.ElapsedTime >= seconds);
        }

        /// <summary>
        /// Resolve the returned promise once the predicate evaluates to false
        /// </summary>
        public IPromise WaitWhile(Func<TimeData, bool> predicate)
        {
            return WaitUntil(t => !predicate(t));
        }

        /// <summary>
        /// Resolve the returned promise once the predicate evalutes to true
        /// </summary>
        public IPromise WaitUntil(Func<TimeData, bool> predicate)
        {
            var promise = new Promise();

            var wait = new PredicateWait
            {
                TimeStarted = _curTime,
                PendingPromise = promise,
                TimeData = new TimeData(),
                Predicate = predicate,
                FrameStarted = _curFrame
            };

            _waiting.AddLast(wait);

            return promise;
        }

        public bool Cancel(IPromise promise)
        {
            var node = FindInWaiting(promise);

            if (node == null)
            {
                return false;
            }

            node.Value.PendingPromise.RejectSilent(new PromiseCancelledException("Promise was cancelled by user."));
            _waiting.Remove(node);

            return true;
        }

        LinkedListNode<PredicateWait> FindInWaiting(IPromise promise)
        {
            for (var node = _waiting.First; node != null; node = node.Next)
            {
                if (node.Value.PendingPromise.Id.Equals(promise.Id))
                {
                    return node;
                }
            }

            return null;
        }

        /// <summary>
        /// Update all pending promises. Must be called for the promises to progress and resolve at all.
        /// </summary>
        public void Update(float deltaTime)
        {
            _curTime += deltaTime;
            _curFrame += 1;

            var node = _waiting.First;
            while (node != null)
            {
                var wait = node.Value;

                var newElapsedTime = _curTime - wait.TimeStarted;
                wait.TimeData.DeltaTime = newElapsedTime - wait.TimeData.ElapsedTime;
                wait.TimeData.ElapsedTime = newElapsedTime;
                var newElapsedUpdates = _curFrame - wait.FrameStarted;
                wait.TimeData.ElapsedUpdates = newElapsedUpdates;

                bool result;
                try
                {
                    result = wait.Predicate(wait.TimeData);
                }
                catch (Exception ex)
                {
                    wait.PendingPromise.Reject(ex);

                    node = RemoveNode(node);
                    continue;
                }

                if (result)
                {
                    wait.PendingPromise.Resolve();

                    node = RemoveNode(node);
                }
                else
                {
                    node = node.Next;
                }
            }
        }

        /// <summary>
        /// Removes the provided node and returns the next node in the list.
        /// </summary>
        private LinkedListNode<PredicateWait> RemoveNode(LinkedListNode<PredicateWait> node)
        {
            var currentNode = node;
            node = node.Next;

            _waiting.Remove(currentNode);

            return node;
        }
    }
}

