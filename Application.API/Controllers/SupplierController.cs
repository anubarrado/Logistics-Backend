using Logistics.BusinessCore;
using Logistics.Data.UnitofWork;
using Logistics.DTOs.Supplier;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Application.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SupplierController : ControllerBase
    {
        private IUnitOfWorkNoSql _unitOfWorkNoSql;
        private ILogger _logger;

        public SupplierController(IUnitOfWorkNoSql unitOfWorkNoSql, ILogger<SupplierController> logger)
        {
            _unitOfWorkNoSql = unitOfWorkNoSql;
            _logger = logger;
        }


        [HttpGet("List")]
        public IActionResult List()
        {
            try
            {
                var servicio = new SupplierBO(_unitOfWorkNoSql, _logger);
                return Ok(servicio.List());
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("Insert")]
        public IActionResult Insert(SupplierCreateDTO dto)
        {
            try
            {
                var bo = new SupplierBO(dto, _unitOfWorkNoSql, _logger);
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


        [HttpPost("Update")]
        public IActionResult Update(SupplierUpdateDTO dto)
        {
            try
            {
                if (_unitOfWorkNoSql.SupplierRepository.GetById(dto.IdEntidad) == null)
                    return NotFound();

                var bo = new SupplierBO(dto, _unitOfWorkNoSql, _logger);
                bo.ValidacionDePropiedades();
                if (bo.HasErrors)
                    return BadRequest(bo.Errors);
                if (bo.Update())
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

        [HttpDelete("Delete")]
        public IActionResult Delete(string idEntidad, string usuario)
        {
            try
            {
                if (_unitOfWorkNoSql.SupplierRepository.GetById(idEntidad) == null)
                    return NotFound();

                var bo = new SupplierBO(_unitOfWorkNoSql, _logger);
                if (bo.Delete(idEntidad, usuario))
                {
                    return Ok();
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex.Message);
                return BadRequest(ex.Message);
            }
        }

    }
}
