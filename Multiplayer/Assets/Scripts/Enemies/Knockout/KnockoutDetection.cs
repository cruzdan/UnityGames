public class KnockoutDetection : EnemyDetection
{
    public override void Idle()
    {
        Player nearestPlayer = GetNearestPlayer();
        if (nearestPlayer != null)
        {
            enemy.PlayerTarget = nearestPlayer;
            enemy.StartChase();
        }
    }
}
