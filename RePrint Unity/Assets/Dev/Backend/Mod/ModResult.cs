

using System.Collections.Generic;

public class ModResult
{
    public StatChangeAmounts statChangeAmounts;

    public Mod mod;

    public List<AbilitySelection> retriggerAbilities;

    public ModResult(Mod _mod, Character player, Team enemyTeam)
    {
        mod = _mod;
        statChangeAmounts = new StatChangeAmounts(player, enemyTeam);
        retriggerAbilities = new List<AbilitySelection>();
    }


}