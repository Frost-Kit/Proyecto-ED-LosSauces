namespace CafeteriaAromas.Interfaces
{
    public interface ICola<T>
    {
        bool IsEmpty();
        bool IsFull();
        void Enqueue(T elemento);
        T Dequeue();
        T Peek();
        int Size();
    }
}
