namespace ZodiarkLib.Database
{
    public interface IDataRecord<T>
    {
        T Id { get; set; }
    }
}