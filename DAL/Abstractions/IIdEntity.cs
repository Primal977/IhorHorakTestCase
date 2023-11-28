namespace DAL.Abstractions
{
    public interface IIdEntity<T> where T : notnull
    {
        public T Id { get; set; }
    }
}
