using System.Collections.Generic;
using System.Linq;

// ReSharper disable once CheckNamespace
namespace RSG
{
    public static class CancelableExtensions
    {
        public static ICancellablePromise FindLastPendingParent(this ICancellablePromise cancellable)
        {
            ICancellablePromise result;
            var next = cancellable;
            
            do
            {
                result = next;
                next = result.Parent;
            } while (next != null && next.CanBeCanceled);

            return result;
        }
        
        public static List<ICancellablePromise> GetCancelSequenceFromParentToThis(this ICancellablePromise cancellable)
        {
            var result = new List<ICancellablePromise>();

            var next = cancellable;
            
            do
            {
                result.Insert(0, next);
                next = next.Parent;
            } while (next != null && next.CanBeCanceled);
            
            return result;
        }
        
        public static List<ICancellablePromise> CollectSelfAndAllPendingChildren(this ICancellablePromise cancellable)
        {
            var result = new List<ICancellablePromise>();
            AddSelfAndChildren(result, cancellable);
            return result;
        }

        private static void AddSelfAndChildren(List<ICancellablePromise> result, ICancellablePromise cancellable)
        {
            if (cancellable.CanBeCanceled)
            {
                result.Add(cancellable);
            }

            foreach (var child in cancellable.Children.Where(x => x.CanBeCanceled))
            {
                AddSelfAndChildren(result, child);
            }
        }
        
        public static string GetTreeDescription(this ICancellablePromise promise)
        {
            var result = string.Empty;
            RecordSelfAndChildren(ref result, promise, 0);
            return result;
        }
        
        private static void RecordSelfAndChildren(ref string result, ICancellablePromise promise, int indent)
        {
            var indentString = "\n";

            for (var i = 0; i < indent; i++)
            {
                indentString += " ";
            }
            
            result += indentString + promise.CurState;

            foreach (var child in promise.Children)
            {
                RecordSelfAndChildren(ref result, child, indent + 1);
            }
        }

        public static List<PromiseState> GetAllTreeStates(this ICancellablePromise promise)
        {
            var result = new List<PromiseState>();
            FillSelfAndChildren(ref result, promise);
            return result;
        }
        
        private static void FillSelfAndChildren(ref List<PromiseState> result, ICancellablePromise promise)
        {
            result.Add(promise.CurState);

            foreach (var child in promise.Children)
            {
                FillSelfAndChildren(ref result, child);
            }
        }
    }
}