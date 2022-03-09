using Microsoft.Extensions.Logging;
using Odap.Models;
using OdapApi.Data;
using OdapApi.IRepositories;
using OdapApi.Models.Device;
using OdapApi.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace OdapApi.Repositories
{
    public class DeviceDataRepository : IDeviceDataRepository
    {
        private protected HttpClient _httpClient;
        private readonly OdapDbContext _OdapDbContext;
        private readonly ILogger<DeviceDataRepository> _logger;
        public DeviceDataRepository(OdapDbContext OdapDbContext, ILogger<DeviceDataRepository> logger)
        {
            _OdapDbContext = OdapDbContext;
            _logger = logger;
        }

        //Odap Device
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


        // RAngle
        public async Task<RAngle> AddRAngleValue(RAngle rAngle)
        {
            rAngle.DateReceived = DateTime.Now;
            _OdapDbContext.Add(rAngle);
            await _OdapDbContext.SaveChangesAsync();
            RAngle rAngleSaved= _OdapDbContext.RAngles.OrderByDescending(message => message.Id).FirstOrDefault();
            EmailServices.SendEmailForNetworkNotification(rAngleSaved);
            return rAngleSaved;
        }
        public IEnumerable<RAngle> GetRecentRAngleMessage()
        {
            return _OdapDbContext.RAngles.OrderByDescending(message => message.Id).Take(5);
        }
        public async Task<RAngle> ApproveRAngle(int rAngleId)
        {
            // Confirm by Time so that this happens for a very short period of time
            var latestRAngle = _OdapDbContext.RAngles.Where(rAngle=>rAngle.Id==rAngleId).FirstOrDefault();
            latestRAngle.IsApproved = true;
            _OdapDbContext.Update(latestRAngle);
            await _OdapDbContext.SaveChangesAsync();
            return latestRAngle;
        }
        public async Task<RAngle> InvalidateRAngle(int rAngleId)
        {
            // Confirm by Time so that this happens for a very short period of time
            var latestRAngle = _OdapDbContext.RAngles.Where(rAngle => rAngle.Id == rAngleId).FirstOrDefault();
            latestRAngle.IsApproved = false;
            _OdapDbContext.Update(latestRAngle);
            await _OdapDbContext.SaveChangesAsync();
            return latestRAngle;
        }
    }
}
