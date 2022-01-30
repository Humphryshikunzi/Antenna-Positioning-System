using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OdapApi.IRepositories;
using OdapApi.Models.Device;
using System.Threading.Tasks;

namespace OdapApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OdapController : ControllerBase
    {
        private readonly IDeviceDataRepository _OdapRepository;
        private readonly ILogger<IDeviceDataRepository> _logger;
        public OdapController(IDeviceDataRepository OdapRepository, ILogger<IDeviceDataRepository> logger)
        {
            _OdapRepository = OdapRepository;
            _logger = logger;
        }

        [HttpPost("addOdapObject")]
        public async Task<ActionResult> AddOdapMessage(DeviceData OdapObject)
        {
            await _OdapRepository.AddOdapDevice(OdapObject);
            return Ok();
        }

        [HttpGet("all")]
        public ActionResult GetAll()
        {
            return Ok(_OdapRepository.GetOdapDevice());
        }
        [HttpGet("recentMessages")]
        public ActionResult GetRecent()
        {
            return Ok(_OdapRepository.GetRecentMessages());
        }

        [HttpGet("getbyid/{id}")]
        public ActionResult GetById(int id)
        {
            return Ok(_OdapRepository.GetOdapDeviceById(id));
        }

        [HttpPut("update")]
        public async Task<ActionResult> Update(DeviceData deviceMessage)
        {
            if (ModelState.IsValid)
            {
                await _OdapRepository.UpdateOdapDeviceById(deviceMessage);
                return Ok(deviceMessage);
            }
            return BadRequest();
        }

        [HttpDelete("delete/{id}")]
        public ActionResult Delete(int id)
        {
            var help = _OdapRepository.GetOdapDeviceById(id);
            if (help != null)
            {
                _OdapRepository.DeleteOdapDeviceById(id);
                return Ok(help);
            }
            return BadRequest();
        }
    }
}
