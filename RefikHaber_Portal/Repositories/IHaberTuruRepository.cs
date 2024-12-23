namespace RefikHaber.Repostories
{
    public interface IHaberTuruRepository : IRepository<HaberTuru>
    {
        void Guncelle(HaberTuru haberTuru);
        void Kaydet();

    }
}
