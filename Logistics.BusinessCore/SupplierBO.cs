using AutoMapper;
using Logistics.BusinessCore.Base;
using Logistics.Data.UnitofWork;
using Logistics.DTOs.Supplier;
using Logistics.Entity;
using Microsoft.Extensions.Logging;
using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logistics.BusinessCore
{
    public class SupplierBO : BaseBO
    {
        #region files
        private readonly IUnitOfWorkNoSql _unitOfWorkNoSql;
        private ILogger _logger;
        private Mapper _mapper;
        #endregion

        public SupplierBO(SupplierCreateDTO dto, IUnitOfWorkNoSql unitOfWorkNoSql, ILogger logger)
        {
            _unitOfWorkNoSql = unitOfWorkNoSql;
            _logger = logger;
            ConfigMapper();

            TrasnsformDTOtoBO(dto);
        }

        public SupplierBO(SupplierUpdateDTO dto, IUnitOfWorkNoSql unitOfWorkNoSql, ILogger logger)
        {
            _unitOfWorkNoSql = unitOfWorkNoSql;
            _logger = logger;
            ConfigMapper();

            TrasnsformDTOtoBO(dto);
        }

        public SupplierBO(IUnitOfWorkNoSql unitOfWorkNoSql, ILogger logger)
        {
            _unitOfWorkNoSql = unitOfWorkNoSql;
            _logger = logger;
            ConfigMapper();
        }

        private void ConfigMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<SupplierDTO, SupplierEntity>(MemberList.None).ReverseMap();
                cfg.CreateMap<SupplierBO, SupplierEntity>(MemberList.None).ReverseMap();
            });
            _mapper = new Mapper(config);
        }

        #region fields 
        public string _ruc { get; set; }

        #endregion

        #region Properties
        public string IdEntidad { get; set; }
        [Required]
        public string RUC
        {
            get { return _ruc; }
            set
            {
                if (value.Length == 11)
                {
                    RemoveError(nameof(this.RUC) + "format");
                }
                else
                {
                    AddError(nameof(this.RUC) + "format", "The RUC field only accepts 11 characters");
                }
                _ruc = value;
            }
        }
        [Required]
        public string FormalName { get; set; }
        [Required]
        public string Address { get; set; }
        public string ContactPerson { get; set; }
        public string PhoneNumber { get; set; }
        [EmailAddress]
        [Required]
        public string ContacEmail { get; set; }

        public bool State { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime ModificationDate { get; set; }
        #endregion

        #region Operations
        public void TrasnsformDTOtoBO(SupplierCreateDTO dto)
        {
            RUC = dto.RUC;
            FormalName = dto.FormalName;
            Address = dto.Address;
            ContactPerson = dto.ContactPerson;
            PhoneNumber = dto.PhoneNumber;
            ContacEmail = dto.ContacEmail;

            State = true;
            CreatedBy = dto.UserName;
            ModifiedBy = dto.UserName;
            CreationDate = DateTime.Now;
            ModificationDate = DateTime.Now;
        }

        public void TrasnsformDTOtoBO(SupplierUpdateDTO dto)
        {
            IdEntidad = dto.IdEntidad;
            RUC = dto.RUC;
            FormalName = dto.FormalName;
            Address = dto.Address;
            ContactPerson = dto.ContactPerson;
            PhoneNumber = dto.PhoneNumber;
            ContacEmail = dto.ContacEmail;

            State = true;
            CreatedBy = dto.CreatedBy;
            ModifiedBy = dto.UserName;
            CreationDate = dto.CreationDate;
            ModificationDate = DateTime.Now;
        }
        public SupplierDTO TrasnsformBOtoDTO()
        {
            SupplierDTO dto = new SupplierDTO()
            {
                IdEntidad = IdEntidad,
                RUC = RUC,
                FormalName = FormalName,
                Address = Address,
                ContactPerson = ContactPerson,
                PhoneNumber = PhoneNumber,
                ContacEmail = ContacEmail,

                State = State,
                CreatedBy = CreatedBy,
                ModifiedBy = ModifiedBy,
                CreationDate = CreationDate,
                ModificationDate = ModificationDate
            };
            return dto;
        }

        public List<SupplierDTO> List()
        {
            try
            {
                var list = _unitOfWorkNoSql.SupplierRepository.List();
                return _mapper.Map<List<SupplierDTO>>(list);
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public bool Insert()
        {
            try
            {
                SupplierEntity entity = _mapper.Map<SupplierEntity>(this);

                var response = _unitOfWorkNoSql.SupplierRepository.AddSync(entity);
                IdEntidad = response.IdEntity;

                return true;
            }
            catch (Exception ex )
            {
                _logger.LogCritical(ex.Message);
                return false;
            }           
        }

        public bool Update()
        {
            try
            {
                SupplierEntity entity = _mapper.Map<SupplierEntity>(this);
                entity.IdEntity = IdEntidad;

                var response = _unitOfWorkNoSql.SupplierRepository.UpdateSync(entity, IdEntidad);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex.Message);
                return false;
            }
        }

        public bool Delete(string idEntidad, string usuario)
        {
            try
            {
                var entity = _unitOfWorkNoSql.SupplierRepository.GetById(idEntidad);

                if (entity == null)
                    throw new Exception("The Supplier does not exist");

                if (entity.State == false)
                    throw new Exception("The supplier was already eliminated");

                entity.State = false;
                entity.ModifiedBy = usuario;
                entity.ModificationDate = DateTime.Now;
                entity.IdEntity = idEntidad;

                var response = _unitOfWorkNoSql.SupplierRepository.UpdateSync(entity, idEntidad);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex.Message);
                return false;
            }
        }
        #endregion

        #region Validations
        public void ValidacionDePropiedades()
        {
            ICollection<ValidationResult> results = new List<ValidationResult>();

            if (!Validation(this, out results))
            {
                foreach (var item in results)
                {
                    AddError(item.MemberNames.SingleOrDefault(), item.ErrorMessage);
                }
            }
        }
        #endregion
    }
}
