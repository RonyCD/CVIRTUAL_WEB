using AutoMapper;
using Azure.Core;
using CVirtual.Application.IServices.Categoria;
using CVirtual.DataAccess.SQLServer.IQueries.Categoria;
using CVirtual.DataAccess.SQLServer.IQueries.Modulo;
using CVirtual.Domain.Entities.Categoria;
using CVirtual.Dto.Base;
using CVirtual.Dto.Categoria;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVirtual.Application.Services.Categoria
{
    public class CategoriaService : ICategoriaService
    {

        private readonly ICategoriaQuery _ICategoriaQuery;
        private readonly IMapper _IMapper;

        public CategoriaService(ICategoriaQuery iCategoriaQuery, IMapper iMapper)
        {
            _ICategoriaQuery = iCategoriaQuery;
            _IMapper = iMapper;
        }

        public async Task<BaseResponse<CategoriaResponse>> CrearCategoria(CategoriaRequest _Request)
        {
            var _BaseResponse = new BaseResponse<CategoriaResponse>
            {
                Code = "200",
                Success = true,
                Data = null,
                Validations = new List<MessageResponse>()
            };

            try
            {

                //var _ResultCategoria = _IMapper.Map<CategoriaRequest, CategoriaEntity>(_Request);
                //var _Categoria = await _ICategoriaQuery.CrearCategoria(_ResultCategoria);

                //_BaseResponse.Data = _IMapper.Map<CategoriaEntity, CategoriaResponse>(_Categoria);

                var _Categoria = await _ICategoriaQuery.CrearCategoria(_Request);
                _BaseResponse.Data = _IMapper.Map<CategoriaEntity, CategoriaResponse>(_Categoria);
            
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
