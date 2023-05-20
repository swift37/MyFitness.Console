using Librarian.DAL.Entities.Base;

namespace MyFitness.BL.Services.Interfaces
{
    public interface IDataIOService
    {
        void SaveData<T>(IEnumerable<T>? entity) where T : Entity;

        IEnumerable<T>? LoadData<T>() where T : Entity;

        void Remove<T>(int id) where T : Entity, new();
    }
}
