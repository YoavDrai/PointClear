namespace PointClear.Enemies
{
    /// <summary>
    /// Block 2A: marker for an enemy's behaviour driver (its movement/attack AI).
    /// The shared <see cref="EnemyDeathBeat"/> disables every IEnemyBehaviour on a
    /// dying enemy so it stops acting during its death beat. Empty by design —
    /// identity only, not a framework. A new enemy AI implements this to
    /// participate in the death beat.
    /// </summary>
    public interface IEnemyBehaviour
    {
    }
}
