using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using WebapiWinservice.DB;
using WebapiWinservice.Services;
using WebapiWinserviceObjects;

namespace WebapiWinservice.Controllers
{
    [Route("api/ip2loc")]
    [ApiController]
    public class Ip2LocationController : ControllerBase
    {
        private readonly Ip2LocationService _controller;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public Ip2LocationController(Ip2LocationService controller, IHttpContextAccessor httpContextAccessor)
        {
            _controller = controller;
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// http://127.0.0.1:5006/api/ip2loc
        /// http://127.0.0.1:5006/api/ip2loc?ip=91.207.131.254
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetInfo(string ip = null)
        {
            IPAddress address = null;
            if (!string.IsNullOrWhiteSpace(ip) && !IPAddress.TryParse(ip, out  address))
            {
                return BadRequest("Invalid Ip-Address");
            }
            else
            {
                Ip2locationDb11 info = _controller.GetInfo(address == null ?_httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString() : address.ToString());
                if (info == null)
                {
                    return BadRequest("Not found");
                }
                return new JsonResult(info)
                {
                    StatusCode = StatusCodes.Status200OK
                };
            }
        }

        [HttpPost]
        public ActionResult GetInfo(Ip2LocationRequest request)
        {
            IPAddress address = null;
            if (!string.IsNullOrWhiteSpace(request.IpAddress) && !IPAddress.TryParse(request.IpAddress, out address))
            {
                return BadRequest("Invalid Ip-Address");
            }
            else
            {
                var info = _controller.GetInfo(address == null ? _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString() : address.ToString());
                if (info == null)
                {
                    return BadRequest("Not found");
                }
                return new JsonResult(info)
                {
                    StatusCode = StatusCodes.Status200OK
                };
            }
        }

        /// <summary>
        /// http://127.0.0.1:5006/api/ip2loc/city?ip=91.207.131.254
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("city")]
        public ActionResult GetCityInfo(string ip)
        {
            IPAddress address = null;
            if (!string.IsNullOrWhiteSpace(ip) && !IPAddress.TryParse(ip, out address))
            {
                return BadRequest("Invalid Ip-Address");
            }
            else
            {
                var info = _controller.GetInfo(address == null ? _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString() : address.ToString());
                if (info == null)
                {
                    return BadRequest("Not found");
                }
                return Ok(info.CityName);
            }
        }
    }
}
