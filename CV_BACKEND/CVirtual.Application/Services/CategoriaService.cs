using AutoMapper;
using Azure.Core;
using CVirtual.Application.IServices;
using CVirtual.DataAccess.SQLServer.IQueries;
using CVirtual.Domain.Entities.Categoria;
using CVirtual.Dto.Base;
using CVirtual.Dto.Categoria;
using Microsoft.AspNetCore.Http.HttpResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVirtual.Application.Services
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

        public async Task<BaseResponse<bool>> EditarCategoria(CategoriaEditarRequest _Request)
        {
            var _BaseResponse = new BaseResponse<bool>
            {
                Code = "200",
                Success = true,
                Data = false,
                Validations = new List<MessageResponse>()
            };

            try
            {
                var categoriaEntity = _IMapper.Map<CategoriaEditarRequest, CategoriaEditarEntity>(_Request);

                if (_Request.IdCategoria <= 0)
                {
                    throw new Exception("IdCategoria debe ser mayor que cero.");
                }

                var result = await _ICategoriaQuery.EditarCategoria(categoriaEntity);

                _BaseResponse.Data = result;
                _BaseResponse.Message = result ? "Categoría actualizada con éxito" : "No se pudo actualizar la categoría";
            }
            catch (Exception ex)
            {
                _BaseResponse.Code = "400";
                _BaseResponse.Success = false;
                _BaseResponse.Message = "Error: " + ex.Message;
            }

            return _BaseResponse;
        }


        public async Task<BaseResponse<ICollection<CategoriaResponse>>> EliminarById(int _IdCategoria)
        {
            var _BaseResponse = new BaseResponse<ICollection<CategoriaResponse>>
            {
                Code = "200",
                Success = true,
                Data = null,
                Validations = new List<MessageResponse>()
            };

            try
            {                   
                var resultado = await _ICategoriaQuery.EliminarById(_IdCategoria);

                if (resultado)
                {
                    _BaseResponse.Message = "Categoría eliminada exitosamente.";
                }
                
            }
            catch (Exception ex)
            {
                _BaseResponse.Code = "400";
                _BaseResponse.Success = false;
                _BaseResponse.Message = "Error: " + ex.Message;
            }

            return _BaseResponse;
        }


        public async Task<BaseResponse<ICollection<CategoriaResponse>>> ObtenerPorIdCVirtual(int _IdCartaVirtual)
        {
            var _BaseResponse = new BaseResponse<ICollection<CategoriaResponse>>
            {
                Code = "200",
                Success = true,
                Data = null,
                Validations = new List<MessageResponse>()
            };

            try
            {
                var _CategoriasEntity = await _ICategoriaQuery.ObtenerPorIdCVirtual(_IdCartaVirtual);
                var _CategoriasResponse = _IMapper.Map<ICollection<CategoriaEntity>, ICollection<CategoriaResponse>>(_CategoriasEntity);

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
