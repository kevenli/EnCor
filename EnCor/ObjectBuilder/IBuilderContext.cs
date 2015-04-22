namespace EnCor.ObjectBuilder
{
    public interface IBuilderContext
    {
        T GetExtension<T>();

        T GetExtension<T>(string name);
    }
}
