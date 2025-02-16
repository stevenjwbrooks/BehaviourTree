﻿namespace BehaviourTree.Decorators
{
    public sealed class TimeLimit<TContext> : DecoratorBehaviour<TContext> where TContext : IClock
    {
        private ulong? _initialTimestamp;
        public readonly ulong TimeLimitInMilliseconds;

        public TimeLimit(IBehaviour<TContext> child, uint timeLimitInMilliseconds) : this("TimeLimit", child, timeLimitInMilliseconds)
        {
        }

        public TimeLimit(string name, IBehaviour<TContext> child, uint timeLimitInMilliseconds) : base(name, child)
        {
            TimeLimitInMilliseconds = timeLimitInMilliseconds;
        }

        protected override BehaviourStatus Update(TContext context)
        {
            var currentTimeStamp = context.GetTimeStampInMilliseconds();

            if (_initialTimestamp == null)
            {
                _initialTimestamp = currentTimeStamp;
            }

            var elapsedMilliseconds = currentTimeStamp - _initialTimestamp;

            if (elapsedMilliseconds >= TimeLimitInMilliseconds)
            {
                return BehaviourStatus.Failed;
            }

            return Child.Tick(context);
        }

        protected override void OnTerminate(BehaviourStatus status)
        {
            _initialTimestamp = null;
        }

        protected override void DoReset(BehaviourStatus status)
        {
            _initialTimestamp = null;
            base.DoReset(status);
        }
    }
}
