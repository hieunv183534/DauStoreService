using DauStore.Core.Interfaces.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DauStore.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController<TEntity> : ControllerBase
    {
        #region Delare

        protected IBaseService<TEntity> _baseService;

        #endregion

        #region Consstructor

        public BaseController(IBaseService<TEntity> baseService)
        {
            _baseService = baseService;
        }

        #endregion

    }
}
