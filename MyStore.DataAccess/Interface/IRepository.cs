using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MyStore.DataAccess.Interface
{
    public interface IRepository<T> where T : class
    {
        /// <summary>
        /// Tìm tất cả các phần tử phù hợp với điều kiện truyền vào
        /// </summary>
        /// <param name="isReadOnly">True: Nếu kết quả trả ra chỉ để đọc</param>
        /// <param name="spec">Điều kiện tìm kiếm. Điều kiện có thể là Id > 1, Content != null... Khi điều kiện là null thì sẽ trả ra tất cả các phần tử</param>
        /// <param name="preFilter">Bộ lọc trước: Thay đổi, lọc dữ liệu trước khi truy vấn</param>
        /// <param name="postFilter">Bộ lọc sau: Thay đổi, lọc dữ liệu sau khi truy vấn được thực hiện. </param>
        /// <returns>Danh sách các thực thể</returns>
        IEnumerable<T> Gets(bool isReadOnly,
            Expression<Func<T, bool>> spec = null,
            Func<IQueryable<T>, IQueryable<T>> preFilers = null,
            params Func<IQueryable<T>, IQueryable<T>>[] postFilers);

        /// <summary>
        /// Tìm tất cả các phần tử phù hợp với điều kiện truyền vào
        /// </summary>
        /// <param name="isReadOnly">True: Nếu kết quả trả ra chỉ để đọc</param>
        /// <param name="spec">Điều kiện tìm kiếm. Điều kiện có thể là Id > 1, Content != null... Khi điều kiện là null thì sẽ trả ra tất cả các phần tử</param>
        /// <param name="preFilter">Bộ lọc trước: Thay đổi, lọc dữ liệu trước khi truy vấn</param>
        /// <param name="postFilter">Bộ lọc sau: Thay đổi, lọc dữ liệu sau khi truy vấn được thực hiện. </param>
        /// <returns>Danh sách các thực thể</returns>
        IEnumerable<T> GetsReadOnly(
            Expression<Func<T, bool>> spec = null,
            Func<IQueryable<T>, IQueryable<T>> preFilers = null,
            params Func<IQueryable<T>, IQueryable<T>>[] postFilers);

        /// <summary>
        /// Tìm một phần tử bởi các thuộc tính nhận dạng (Id, Key)
        /// </summary>
        /// <param name="ids">Các giá trị của thuộc tính nhận dạng</param>
        /// <returns>Kiểu thực thể</returns>
        T Get(params object[] ids);

        /// <summary>
        /// Tìm một phần tử phù hợp với điều kiện kỹ thuật đã cho
        /// </summary>
        /// <param name="isReadOnly">True: Nếu kết quả trả ra chỉ để đọc</param>
        /// <param name="spec">Điều kiện</param>
        /// <returns>Kiểu thực thể</returns>
        T Get(bool isReadOnly, Expression<Func<T, bool>> spec);

        /// <summary>
        /// Tìm một phần tử phù hợp với điều kiện kỹ thuật đã cho, chỉ đọc
        /// </summary>
        /// <param name="isReadOnly">True: Nếu kết quả trả ra chỉ để đọc</param>
        /// <param name="spec">Điều kiện</param>
        /// <returns>Kiểu thực thể</returns>
        T GetReadOnly(Expression<Func<T, bool>> spec);

        /// <summary>
        /// Xác định xem có phần tử nào phù hợp với điều kiện truyền vào hay không
        /// </summary>
        /// <code>
        ///     Dt.Exist(i => i.Name == "Abc");
        /// </code>
        /// <param name="spec">Điều kiện kỹ thuật (điều kiện).</param>
        /// <returns></returns>
        bool Exist(Expression<Func<T, bool>> spec = null);

        /// <summary>
        /// Tính số lượng các phần tử phù hợp với được điểm kỹ thuật đã cho
        /// </summary>
        /// <code>
        ///     Dt.Count(i => i.Name == "Abc");
        /// </code>
        /// <param name="spec">Điều kiện kỹ thuật (điều kiện).</param>
        /// <returns></returns>
        int Count(Expression<Func<T, bool>> spec = null);

        /// <summary>
        /// Tạo mới
        /// </summary>
        /// <param name="entity"></param>
        void Create(T entity);

        /// <summary>
        /// Xóa
        /// </summary>
        /// <param name="entity"></param>
        void Delete(T entity);
    }
}
