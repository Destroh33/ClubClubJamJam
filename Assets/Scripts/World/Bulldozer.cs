public class Bulldozer : Robot
{
    public override void UseAbility()
    {
        var spike = board.FindNear<Spike>(pos);
        if (spike == null)
            return;

        spike.alive = false;
        spike.Init();
    }
}
