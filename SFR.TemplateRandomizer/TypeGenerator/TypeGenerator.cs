namespace SFR.TemplateRandomizer.TypeGenerator
{
    public abstract class TypeGenerator : ITypeGenerator
    {
        protected readonly Random random;

        protected TypeGenerator(Random random)
        {
            this.random = random;
        }

        public abstract object Execute();
    }
}
