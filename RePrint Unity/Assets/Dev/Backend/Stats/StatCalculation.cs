using UnityEngine;

public class StatCalculation
{
    public static void CalculateAbility(Character activator, Character target, int overclock, Ability ability)
    {

    }

    public static int CalculatePotentialDamage(Character activator, int overclock, Ability ability)
    {
        int totalAmount = 0;

        foreach (AbilityEffect effect in ability.GetAbilityEffects(overclock))
        {
            if (effect.Type == AbilityEffectType.DoDamage)
            {
                int baseAmount = effect.ValueInput.GetValue();

                foreach (AbilityEffectModifier modifier in effect.Modifiers)
                {
                    baseAmount = modifier.CalculateSolution(baseAmount, activator.IncomingValues.GetIncomingValue(modifier.IncomingValue));
                }

                totalAmount += baseAmount;
            }
        }

        // TODO: Alter the total amount based on the activator's current mod chips

        return totalAmount;
    }

    // private static int GetIncomingValue(Character )
}

public class AbilityResults
{

}