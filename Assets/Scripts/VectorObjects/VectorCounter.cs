
namespace Assets.Scripts
{
    internal struct VectorCounter<T> where T : IVector 
    {
        public T[] Vectors { get; set; }

        public int Indexer { get; set; }
    }
}
