namespace AnkleBreaker.Utils.Weighting
{
    /// <summary>
    /// use this interface for an object that need a weight
    /// </summary>
    public interface IWeightObject
    {
        public float Weight { get; }
    }
}