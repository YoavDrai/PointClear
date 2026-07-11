namespace PointClear.Progression
{
    /// <summary>
    /// PROTOTYPE-ONLY placeholder curve: XP required to go from level L to
    /// L+1 is baseXpPerLevel * L — just enough to prove the
    /// progressively-increasing-requirement architecture works. This is
    /// explicitly not a balance decision; exact curve shape is deferred to
    /// future playtesting. Replace by writing a new class and pointing
    /// PlayerLevel.Awake() at it — not by editing PlayerLevel's own logic.
    /// No interface exists for this yet; introduce one only when a second
    /// real curve implementation is actually needed alongside this one.
    /// </summary>
    public class PlaceholderXpCurve
    {
        private readonly float baseXpPerLevel;

        public PlaceholderXpCurve(float baseXpPerLevel)
        {
            this.baseXpPerLevel = baseXpPerLevel;
        }

        public float XPRequiredForLevel(int level)
        {
            return baseXpPerLevel * level;
        }
    }
}
