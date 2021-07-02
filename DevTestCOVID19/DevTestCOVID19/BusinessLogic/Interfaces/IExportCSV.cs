using System.Threading.Tasks;

namespace DevTestCOVID19.BusinessLogic.Interfaces
{
    public interface IExportByRegion
    {
        Task<byte[]> GetArrayByteToExportCSV();

        Task<byte[]> GetArrayByteToExportXML();
    }

    public interface IExportByProvince
    {
        Task<byte[]> GetArrayByteToExportCSV(string ISO);

        Task<byte[]> GetArrayByteToExportXML(string ISO);

    }
}
