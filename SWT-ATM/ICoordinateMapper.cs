namespace SWT_ATM
{
    public interface ICoordinateMapper
    {
        void MapTrack(string rawData);

        void Attach(IObserver<Data> observer);

        void Deattach(IObserver<Data> observer);

        void Notify(Data subject);
    }
}