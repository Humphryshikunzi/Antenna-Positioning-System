using Microsoft.Extensions.Logging;
using OdapApi.Data;
using OdapApi.IRepositories;
using OdapApi.Models.Device;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OdapApi.Repositories
{
    public class DeviceDataRepository : IDeviceDataRepository
    {
        private readonly OdapDbContext _OdapDbContext;
        private readonly ILogger<DeviceDataRepository> _logger;
        public DeviceDataRepository(OdapDbContext OdapDbContext, ILogger<DeviceDataRepository> logger)
        {
            _OdapDbContext = OdapDbContext;
            _logger = logger;
        }

        public async Task AddOdapDevice(DeviceData OdapObject)
        {             
            _logger.LogInformation($"Odap Notification:\r\n Received Payload from Device ID :{OdapObject.UniqueDeviceId}\n\t Lat : {OdapObject.Lat}, Long {OdapObject.Long}\n Time :{DateTime.Now.TimeOfDay}\n, Alt :{OdapObject.Alt}\n RSSI {OdapObject.RSSI}\n");
            _OdapDbContext.Add(OdapObject);
            await _OdapDbContext.SaveChangesAsync();
        }      

        public Task DeleteOdapDeviceById(int id)
        {
            _OdapDbContext.Remove(_OdapDbContext.DevicesData.Find(id));
            return _OdapDbContext.SaveChangesAsync();
        }

        public IEnumerable<DeviceData> GetRecentMessages()
        {
            return _OdapDbContext.DevicesData.OrderByDescending(message => message.Id).Take(20);
        }

        public IEnumerable<DeviceData> GetOdapDevice()
        {
            return _OdapDbContext.DevicesData;
        }

        public DeviceData GetOdapDeviceById(int id)
        {
            return _OdapDbContext.DevicesData.Find(id);
        }

        public Task UpdateOdapDeviceById(DeviceData deviceMessage)
        {
            _OdapDbContext.Update(deviceMessage);
            return _OdapDbContext.SaveChangesAsync();
        }
    }
}
