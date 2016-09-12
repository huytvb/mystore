using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyStore.Models
{
    public class UserModel
    {
        /// <summary>
        /// Id
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Tên
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Tên tài khoản
        /// </summary>
        public string Account { get; set; }

        /// <summary>
        /// Mật khẩu
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Quyền hạn
        /// </summary>
        public int Role { get; set; }

        /// <summary>
        /// Trạng thái: Kích hoạt - True, chưa kích hoạt - False
        /// </summary>
        public bool IsActive { get; set; }

    }
}