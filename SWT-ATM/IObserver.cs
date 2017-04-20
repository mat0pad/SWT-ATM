namespace SWT_ATM
{
    public interface IObserver<Subject>
    {
        void Update(Subject subject);
    }
}