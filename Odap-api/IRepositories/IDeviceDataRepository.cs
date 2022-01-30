using OdapApi.Models.Device;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OdapApi.IRepositories
{
    public interface IDeviceDataRepository
    {
        Task AddOdapDevice(DeviceData deviceMessage);
        IEnumerable<DeviceData> GetOdapDevice();
        IEnumerable<DeviceData> GetRecentMessages();
        DeviceData GetOdapDeviceById(int id);
        Task DeleteOdapDeviceById(int id);
        Task UpdateOdapDeviceById(DeviceData deviceMessage);
    }
}
