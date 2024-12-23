using haberPortali1.Models;
using System.Linq.Expressions;

namespace RefikHaber.Repostories
{
    public interface IHaberRepository : IRepository<Haber>
    {
        void Guncelle(Haber haber);
        void Kaydet();
    }

}
