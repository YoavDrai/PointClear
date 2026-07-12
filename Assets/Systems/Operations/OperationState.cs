namespace PointClear.Operations
{
    /// <summary>
    /// Sprint 2.6: lifecycle states of a single Operation run.
    /// Ready (neutral, no run active) → InProgress → a terminal state
    /// (Succeeded | Failed) → back to Ready. Once terminal, the run is over
    /// until the player returns to Ready.
    /// </summary>
    public enum OperationState
    {
        Ready,
        InProgress,
        Succeeded,
        Failed
    }
}
