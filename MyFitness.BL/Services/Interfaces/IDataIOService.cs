namespace MyFitness.BL.Services.Interfaces
{
    public interface IDataIOService
    {
        void SaveData<T>(string filePath, IEnumerable<T> entities);

        IEnumerable<T>? LoadData<T>(string filePath);
    }
}
