using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyStore.DataAccess.Interface;

namespace MyStore.DataAccess.Interface
{
    public interface IDbContext : IDisposable
    {
        /// <summary>
        /// Lưu thay đổi
        /// </summary>
        /// <returns>Số bản ghi được thay đổi</returns>
        int SaveChanges();

        /// <summary>
        /// Lấy ra DbSet cho các kiểu thực thể
        /// </summary>
        /// <typeparam name="T">Kiểu thực thể</typeparam>
        /// <returns>DbSet của kiểu thực thể</returns>
        IDbSet<T> Set<T>() where T : class;

        /// <summary>
        /// Lấy ra các repository cho các kiểu thực thể
        /// </summary>
        /// <typeparam name="T">Kiểu thực thể</typeparam>
        /// <returns>Repository của kiểu thực thể</returns>
        IRepository<T> GetRepository<T>() where T : class;

        /// <summary>
        /// Sử dụng câu query sql để truy vấn dữ liệu
        /// </summary>
        /// <param name="queryText">Câu truy vấn.</param>
        /// <param name="parameters">Các tham số.</param>
        /// <returns>Danh sách kết quả truy vấn</returns>
        IEnumerable<dynamic> RawQuery(string queryText, params object[] parameters);

        /// <summary>
        /// Sử dụng câu Strore để truy vấn dữ liệu
        /// </summary>
        /// <param name="storeName">Câu store.</param>
        /// <param name="parameters">Các tham số.</param>
        /// <returns>Danh sách kết quả truy vấn</returns>
        IEnumerable<dynamic> RawProcedure(string storeName, params object[] parameters);

        /// <summary>
        /// Sử dụng câu query sql để thay đổi dữ liệu
        /// </summary>
        /// <param name="commandText">Câu truy vấn.</param>
        /// <param name="parameters">Các tham số.</param>
        /// <returns>Số bản ghi được thay đổi</returns>
        int RawModify(string commandText, params object[] parameters);

        /// <summary>
        /// Sử dụng câu query sql để truy vấn dữ liệu (lấy ra dữ liệu của cột đầu tiên dòng đầu tiên)
        /// </summary>
        /// <param name="commandText">Câu truy vấn.</param>
        /// <param name="parameters">Các tham số.</param>
        /// <returns>Dữ liệu của cột đầu tiên dòng đầu tiên</returns>
        object RawScalar(string commandText, params object[] parameters);

        /// <summary>
        /// Sử dụng câu sql để truy vấn dữ liệu
        /// </summary>
        /// <param name="query">Sql query</param>
        /// <param name="parameters">Sql parameter</param>
        /// <returns>Trả về kết quả dạng datatable</returns>
        DataTable RawTable(string query, params object[] parameters);

        /// <summary>
        /// Sử dụng Store để truy vấn dữ liệu
        /// </summary>
        /// <param name="commandText">Sql query</param>
        /// <param name="parameters">Sql parameter</param>
        /// <returns>Trả về kết quả dạng datatable</returns>
        DataTable RawProcedureTable(string commandText, params object[] parameters);

        /// <summary>
        /// Các bộ lọc cho từng nguồn dữ liệu
        /// </summary>
        /// <value>Các bộ lọc.</value>
        QueryFilterProvider Filters { get; }

        /// <summary>
        /// Config EF
        /// </summary>
        System.Data.Entity.Infrastructure.DbContextConfiguration Configuration { get; }
    }
}
