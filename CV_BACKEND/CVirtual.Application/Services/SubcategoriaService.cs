using AutoMapper;
using CVirtual.Application.IServices;
using CVirtual.DataAccess.SQLServer.IQueries;
using CVirtual.DataAccess.SQLServer.Queries;
using CVirtual.Domain.Entities.Categoria;
using CVirtual.Domain.Entities.Subcategoria;
using CVirtual.Dto.Base;
using CVirtual.Dto.Categoria;
using CVirtual.Dto.Subcategoria;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVirtual.Application.Services
{
    public class SubcategoriaService : ISubcategoriaService
    {

        private readonly ISubcategoriaQuery _ISubcategoriaQuery;
        private readonly IMapper _IMapper;

        public SubcategoriaService(ISubcategoriaQuery iSubcategoriaQuery, IMapper iMapper)
        {
            _ISubcategoriaQuery = iSubcategoriaQuery;
            _IMapper = iMapper;
        }


        public async Task<BaseResponse<SubcategoriaResponse>> CrearSubcategoria(SubcategoriaRequest _Request)
        {
            var _BaseResponse = new BaseResponse<SubcategoriaResponse>
            {
                Code = "200",
                Success = true,
                Data = null,
                Validations = new List<MessageResponse>()
            };

            try
            {
                var _Categoria = await _ISubcategoriaQuery.CrearSubcategoria(_Request);
                _BaseResponse.Data = _IMapper.Map<SubcategoriaEntity, SubcategoriaResponse>(_Categoria);

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

        public async Task<BaseResponse<ICollection<SubcategoriaResponse>>> ObtenerByIdCategoria(int _IdCartaVirtual)
        {
            var _BaseResponse = new BaseResponse<ICollection<SubcategoriaResponse>>
            {
                Code = "200",
                Success = true,
                Data = null,
                Validations = new List<MessageResponse>()
            };

            try
            {
                var _SubcategoriasEntity = await _ISubcategoriaQuery.ObtenerByIdCategoria(_IdCartaVirtual);
                var _CategoriasResponse = _IMapper.Map<ICollection<SubcategoriaEntity>, ICollection<SubcategoriaResponse>>(_SubcategoriasEntity);

                _BaseResponse.Data = _CategoriasResponse;
            }
            catch (Exception ex)
            {
                _BaseResponse.Code = "400";
                _BaseResponse.Success = false;
                _BaseResponse.Message = "Error: " + ex.Message;
            }

            return _BaseResponse;
        }

    }
}
