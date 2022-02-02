﻿using Odap.Models;
using OdapApi.Models.Device;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OdapApi.IRepositories
{
    public interface IDeviceDataRepository
    {
        //Coordinates
        Task AddOdapDevice(DeviceData deviceMessage);
        IEnumerable<DeviceData> GetOdapDevice();
        IEnumerable<DeviceData> GetRecentMessages();
        DeviceData GetOdapDeviceById(int id);
        Task DeleteOdapDeviceById(int id);
        Task UpdateOdapDeviceById(DeviceData deviceMessage);

        //RAngle
        Task AddRAngleValue(RAngle rAngle);
        IEnumerable<DeviceData> GetRecentRAngleMessage();
    }
}
