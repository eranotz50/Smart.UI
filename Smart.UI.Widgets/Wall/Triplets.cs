namespace Smart.UI.Widgets
{
    public class Triplet<T> : Triplet<T, T, T>
    {
        public Triplet()
        {
        }

        public Triplet(T before, T content, T after)
            : base(before, content, after)
        {
        }
    }

    public class Triplet<TBefore, TContent, TAfter>
    {
        public TAfter After;
        public TBefore Before;
        public TContent Content;

        public Triplet()
        {
        }

        public Triplet(TBefore before, TContent content, TAfter after)
        {
            Before = before;
            Content = content;
            After = after;
        }
    }
}