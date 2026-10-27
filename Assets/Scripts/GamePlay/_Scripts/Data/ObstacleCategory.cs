public enum ObstacleCategory
{
    SolidCover,      // Crates, metal/wood barricades, trees - blocks movement & bullets
    LowCover,        // Sandbags - blocks movement, allows bullets to pass through
    ExplosiveHazard, // Red barrels - explodes on bullet damage / tank collision
    ToxicHazard      // Green barrels - leaves toxic slow puddle on shatter / collision
}
