namespace gs.ChefDI
{
    public interface IInstanceProvider
    {
        object SpawnInstance(IObjectResolver resolver);
    }
}
