using System.Collections.Generic;

namespace RPG.Stats
{
    public interface IModifierProvider
    {
        IEnumerable<float> GetAdditiveModidfier(Stat stat);
        IEnumerable<float> GetPercentageModifier(Stat stat);
    }
}