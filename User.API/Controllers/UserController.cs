using AutoMapper;
using Logistics.BusinessCore;
using Logistics.BusinessCore.Services;
using Logistics.Data.UnitofWork;
using Logistics.DTOs.Auth;
using Logistics.DTOs.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace User.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private IUnitOfWorkNoSql _unitOfWorkNoSql;
        private ILogger _logger;
        private readonly TokenGeneratorService _tokenGeneradorService;


        public UserController(IUnitOfWorkNoSql unitOfWorkNoSql, ILogger<UserController> logger, TokenGeneratorService tokenGeneradorService)
        {
            _unitOfWorkNoSql = unitOfWorkNoSql;
            _logger = logger;
            _tokenGeneradorService = tokenGeneradorService;
        }

        [AllowAnonymous]
        [HttpPost("SingUp")]
        public IActionResult SingUp(UserCreateDTO dto)
        {
            try
            {
                var bo = new UserBO(dto, _unitOfWorkNoSql, _logger);
                bo.ValidacionDePropiedades();
                if (bo.HasErrors)
                    return BadRequest(bo.Errors);
                if (bo.Insert())
                {
                    return Ok(bo.TrasnsformBOtoDTO());
                }
                else
                {
                    return BadRequest(bo.Errors);
                }
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public IActionResult Login([FromBody] LoginRequestDTO req)
        {
            try
            {
                UserBO bo = new UserBO(_unitOfWorkNoSql, _logger);

                var usuarioModelo = _unitOfWorkNoSql.UserRepository.GetByUserName(req.UserName);
                if (usuarioModelo == null)
                    return BadRequest("The User does not exist");

                if (!bo.ValidateHashPassword(req.UserName, req.Passsword))
                    return BadRequest("Credenciales incorrectas");

                ClaimsUserDTO claims = new ClaimsUserDTO()
                {
                    UserId = usuarioModelo.IdEntity,
                    UserName = req.UserName
                };
                string accessToken = _tokenGeneradorService.GenerateToken(claims);
                return Ok(accessToken);
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("Test")]
        public IActionResult Test()
        {
            try
            {
                return Ok("Ok");
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex.Message);
                return BadRequest(ex.Message);
            }
        }
    }
}
