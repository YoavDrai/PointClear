namespace PointClear.Skills
{
    /// <summary>
    /// Category of a skill. Only Active is used in Sprint 2.4; Passive and
    /// Ultimate exist so the progression core already treats every skill
    /// uniformly and future types slot in without changing the allocation
    /// system (see PC-008 / Sprint 2.4 design review).
    /// </summary>
    public enum SkillType
    {
        Active,
        Passive,
        Ultimate
    }
}
