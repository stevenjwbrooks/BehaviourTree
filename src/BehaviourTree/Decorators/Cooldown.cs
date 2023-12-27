using System;

namespace BehaviourTree.Decorators
{
    public sealed class Cooldown<TContext> : DecoratorBehaviour<TContext> where TContext : IClock
    {
        public readonly ulong CooldownTimeInMilliseconds;
        private ulong _cooldownStartedTimestamp;

        public bool OnCooldown { get; private set; }

        public Cooldown(IBehaviour<TContext> child, uint cooldownTimeInMilliseconds) : this("Cooldown", child, cooldownTimeInMilliseconds)
        {
        }

        public Cooldown(string name, IBehaviour<TContext> child, uint cooldownTimeInMilliseconds) : base(name, child)
        {
            CooldownTimeInMilliseconds = cooldownTimeInMilliseconds;
        }

        protected override BehaviourStatus Update(TContext context)
        {
            return OnCooldown ? CooldownBehaviour(context) : RegularBehaviour(context);
        }

        private BehaviourStatus RegularBehaviour(TContext context)
        {
            Console.WriteLine("Cooldown Check");
            var childStatus = Child.Tick(context);

            if (childStatus == BehaviourStatus.Succeeded)
            {
                EnterCooldown(context);
            }

            return childStatus;
        }

        private BehaviourStatus CooldownBehaviour(TContext context)
        {
            var currentTimeStamp = context.GetTimeStampInMilliseconds();

            var elapsedMilliseconds = currentTimeStamp - _cooldownStartedTimestamp;

            if (elapsedMilliseconds >= CooldownTimeInMilliseconds)
            {
                ExitCooldown();

                return RegularBehaviour(context);
            }

            return BehaviourStatus.Failed;
        }

        private void ExitCooldown()
        {
            Console.WriteLine("Exit Cooldown"); 
            
            OnCooldown = false;
            _cooldownStartedTimestamp = 0;
        }

        private void EnterCooldown(TContext context)
        {
            Console.WriteLine("Enter Cooldown");
            
            OnCooldown = true;
            _cooldownStartedTimestamp = context.GetTimeStampInMilliseconds();
        }
    }
}
