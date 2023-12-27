using System;
using Com.Enchanters.Warfare.Runtime.Scripts.Abilities;

namespace BehaviourTree.Decorators
{
    /// <summary>
    /// Returns cooldown status of an Ability.
    /// </summary>
    /// <typeparam name="TContext"></typeparam>
    public sealed class AbilityCooldown<TContext> : DecoratorBehaviour<TContext> where TContext : IClock
    {
        private Ability _managedAbility;
        public bool OnCooldown => _managedAbility.IsOnCooldown();

        public AbilityCooldown(IBehaviour<TContext> child, Ability ability) : this("Cooldown", child, ability)
        {
        }

        public AbilityCooldown(string name, IBehaviour<TContext> child, Ability ability) : base(name, child)
        {
            _managedAbility = ability;
        }

        protected override BehaviourStatus Update(TContext context)
        {
            return OnCooldown ? BehaviourStatus.Failed : Child.Tick(context);
        }
    }
}
