using AutoMapper;
using Logistics.BusinessCore.Base;
using Logistics.Data.UnitofWork;
using Logistics.DTOs.User;
using Logistics.Entity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logistics.BusinessCore
{
    public class UserBO : BaseBO
    {
        #region files
        private readonly IUnitOfWorkNoSql _unitOfWorkNoSql;
        private ILogger _logger;
        private Mapper _mapper;
        #endregion

        public UserBO(UserCreateDTO dto, IUnitOfWorkNoSql unitOfWorkNoSql, ILogger logger)
        {
            _unitOfWorkNoSql = unitOfWorkNoSql;
            _logger = logger;
            ConfigMapper();

            TrasnsformDTOtoBO(dto);
        }

        public UserBO(IUnitOfWorkNoSql unitOfWorkNoSql, ILogger logger)
        {
            _unitOfWorkNoSql = unitOfWorkNoSql;
            _logger = logger;
            ConfigMapper();
        }

        private void ConfigMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<UserDTO, UserEntity>(MemberList.None).ReverseMap();
                cfg.CreateMap<UserBO, UserEntity>(MemberList.None).ReverseMap();
            });
            _mapper = new Mapper(config);
        }

        #region fields 

        #endregion

        #region Properties
        public string IdEntidad { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required] 
        public string FullName { get; set; }
        public string Passsword { get; set; }
        [Required]
        public string PassswordHash { get; set; }

        public bool State { get; set; }
        public DateTime CreationDate { get; set; }
        #endregion

        #region Operations
        public void TrasnsformDTOtoBO(UserCreateDTO dto)
        {
            UserName = dto.UserName;
            FullName = dto.FullName;
            PassswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Passsword);
            
            State = true;
            CreationDate = DateTime.Now;
        }

        public UserDTO TrasnsformBOtoDTO()
        {
            UserDTO dto = new UserDTO()
            {
                IdEntidad = IdEntidad,
                UserName = UserName,
                FullName = FullName,
                
                State = State,
                CreationDate = CreationDate,
            };
            return dto;
        }

        public List<UserDTO> List()
        {
            try
            {
                var list = _unitOfWorkNoSql.UserRepository.List();
                return _mapper.Map<List<UserDTO>>(list);
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
                UserEntity entity = _mapper.Map<UserEntity>(this);

                var userDB = _unitOfWorkNoSql.UserRepository.GetByUserName(UserName);
                if (userDB != null)
                    throw new Exception("The UserName is already in use");

                var response = _unitOfWorkNoSql.UserRepository.AddSync(entity);
                IdEntidad = response.IdEntity;

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex.Message);
                throw new Exception(ex.Message);
            }
        }
        public bool ValidateHashPassword(string userName, string passsword)
        {
            try
            {
                var userDB = _unitOfWorkNoSql.UserRepository.GetByUserName(userName);
                if (userDB == null)
                    throw new Exception("The user does not exist");
                return BCrypt.Net.BCrypt.Verify(passsword, userDB.PassswordHash);
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
