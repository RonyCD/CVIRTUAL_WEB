using AutoMapper;
using CVirtual.Application.IServices.Modulo;
using CVirtual.DataAccess.SQLServer.IQueries.Modulo;
using CVirtual.Domain.Entities.Modulo;
using CVirtual.Dto.Base;
using CVirtual.Dto.Modulo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVirtual.Application.Services.Modulo
{
    public class ModuloService : IModuloService
    {

        private readonly IModuloQuery _IModuloQuery;
        private readonly IMapper _IMapper;

        public ModuloService(IModuloQuery iModuloQuery, IMapper iMapper)
        {
            _IModuloQuery = iModuloQuery;
            _IMapper = iMapper;
        }

        
        public BaseResponse<ICollection<ModuloResponse>> ListarModulos()
        {
            var _BaseResponse = new BaseResponse<ICollection<ModuloResponse>>()
            {
                Code = "200",
                Success = true,
                Data = null,
                Validations = new List<MessageResponse>()
            };

            try
            {
                var _ResultModulos = _IModuloQuery.ListarModulos();
                var listaModulos = _IMapper.Map<ICollection<ModuloEntity>, ICollection<ModuloResponse>>(_ResultModulos);

                _BaseResponse.Data = listaModulos;
            }
            catch (Exception ex)
            {
                _BaseResponse.Code = "400";
                _BaseResponse.Success = false;
                _BaseResponse.Validations = null;
                _BaseResponse.Message = "Error: " + ex.Message;                
            }

            return _BaseResponse;

        }


    }
}
