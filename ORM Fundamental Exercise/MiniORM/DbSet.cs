namespace MiniORM
{  
    public class DbSet<T>
    {
        // TODO: Create your DbSet class here.

        public IEnumerable<T>? Entities { get; set; }
    }
}
