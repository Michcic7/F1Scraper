namespace ScraperApp.Interfaces;

internal interface IDriversSerializer
{
    void SerializeDrivers(int year, bool includeIndexing);
}
