using AutoMapper;
using CVirtual.Application.IServices;
using CVirtual.DataAccess.SQLServer.IQueries;
using CVirtual.Domain.Entities.Banner;
using CVirtual.Dto.Banner;
using CVirtual.Dto.Base;
using CVirtual.Dto.Categoria;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVirtual.Application.Services
{
    public class BannerService : IBannerService
    {
        private readonly IBannerQuery _IBannerQuery;
        private readonly IMapper _IMapper;

        public BannerService(IBannerQuery iBannerQuery, IMapper iMapper)
        {
            _IBannerQuery = iBannerQuery;
            _IMapper = iMapper;
        }

        public async Task<BaseResponse<BannerResponse>> AgregarBanner(BannerRequest _Request)
        {
            var _BaseResponse = new BaseResponse<BannerResponse>
            {
                Code = "200",
                Success = true,
                Data = null,
                Validations = new List<MessageResponse>()
            };

            try
            {
                var _Banner = await _IBannerQuery.AgregarBanner(_Request);
                _BaseResponse.Data = _IMapper.Map<BannerEntity, BannerResponse>(_Banner);

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

        public async Task<BaseResponse<ICollection<BannerResponse>>> EliminarBanner(int _IdBanner)
        {
            var _BaseResponse = new BaseResponse<ICollection<BannerResponse>>
            {
                Code = "200",
                Success = true,
                Data = null,
                Validations = new List<MessageResponse>()
            };

            try
            {
                var resultado = await _IBannerQuery.EliminarBanner(_IdBanner);

                if (resultado)
                {
                    _BaseResponse.Message = "Banner eliminada exitosamente.";
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

        


        public async Task<BaseResponse<ICollection<BannerResponse>>> ObtenerPorIdCVirtual(int _IdCartaVirtual)
        {
            var _BaseResponse = new BaseResponse<ICollection<BannerResponse>>
            {
                Code = "200",
                Success = true,
                Data = null,
                Validations = new List<MessageResponse>()
            };

            try
            {
                var _CategoriasEntity = await _IBannerQuery.ObtenerPorIdCVirtual(_IdCartaVirtual);
                var _CategoriasResponse = _IMapper.Map<ICollection<BannerEntity>, ICollection<BannerResponse>>(_CategoriasEntity);

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
