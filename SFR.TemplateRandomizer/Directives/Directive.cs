namespace Drafts.Directives
{
    public abstract class Directive : IDirective
    {
        protected readonly Random random;

        protected Directive(Random random)
        {
            this.random = random;
        }

        public abstract object Execute();
    }
}
