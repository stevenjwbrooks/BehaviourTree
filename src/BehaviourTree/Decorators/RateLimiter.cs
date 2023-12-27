namespace BehaviourTree.Decorators
{
    public sealed class RateLimiter<TContext> : DecoratorBehaviour<TContext> where TContext : IClock
    {
        private ulong? _previousTimestamp;
        private BehaviourStatus _previousChildStatus;
        public readonly ulong IntervalInMilliseconds;

        public RateLimiter(IBehaviour<TContext> child, uint intervalInMilliseconds) : this("RateLimiter", child, intervalInMilliseconds)
        {
        }

        public RateLimiter(string name, IBehaviour<TContext> child, uint intervalInMilliseconds) : base(name, child)
        {
            IntervalInMilliseconds = intervalInMilliseconds;
        }

        protected override BehaviourStatus Update(TContext context)
        {
            var currentTimeStamp = context.GetTimeStampInMilliseconds();

            var elapsedMilliseconds = currentTimeStamp - _previousTimestamp;

            if (_previousTimestamp == null || elapsedMilliseconds >= IntervalInMilliseconds)
            {
                _previousChildStatus = Child.Tick(context);

                if (_previousChildStatus != BehaviourStatus.Running)
                {
                    _previousTimestamp = currentTimeStamp;
                }
            }

            return _previousChildStatus;
        }
    }
}
