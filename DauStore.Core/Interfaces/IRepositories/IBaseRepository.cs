using System;
using System.Collections.Generic;
using System.Text;

namespace DauStore.Core.Interfaces.IRepositories
{
    public interface IBaseRepository<TEntity>
    {
        /// <summary>
        /// Lấy toàn bộ bản ghi
        /// </summary>
        /// <returns></returns>
        List<TEntity> GetAll();

        /// <summary>
        /// Lấy theo id
        /// </summary>
        /// <param name="entityId"></param>
        /// <returns></returns>
        /// Author HieuNv
        TEntity GetById(Guid entityId);

        /// <summary>
        /// Thêm
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        /// Author HieuNv
        int Add(TEntity entity);

        /// <summary>
        /// Sửa 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="entityId"></param>
        /// <returns></returns>
        /// Author HieuNv
        int Update(TEntity entity, Guid entityId);

        /// <summary>
        /// Xóa
        /// </summary>
        /// <param name="entityId"></param>
        /// <returns></returns>
        /// Author HieuNv
        int Delete(Guid entityId);

        /// <summary>
        /// Lấy theo một prop
        /// </summary>
        /// <param name="propName"></param>
        /// <param name="propValue"></param>
        /// <returns></returns>
        /// Author HieuNv
        TEntity GetByProp(string propName, object propValue);

        /// <summary>
        /// Xóa theo 1 prop
        /// </summary>
        /// <param name="propName"></param>
        /// <param name="propValue"></param>
        /// <returns></returns>
        int DeleteByProp(string propName, object propValue);

        /// <summary>
        /// lấy mã mới
        /// </summary>
        /// <returns></returns>
        string GetNewCode();

    }
}
