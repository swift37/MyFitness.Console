using Librarian.DAL.Entities.Base;

namespace MyFitness.BL.Services.Interfaces
{
    public interface IDataIOService
    {
        void SaveData<T>(IEnumerable<T> entities) where T : Entity;

        IEnumerable<T>? LoadData<T>() where T : Entity;
    }
}
