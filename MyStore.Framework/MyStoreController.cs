using MyStore.Framework.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace MyStore.Framework
{
    /// <summary>
    /// Trình: 6/5/15
    /// </summary>
    public class MyStoreController : Controller
    {
        private int _defautNotificationTime;

        public MyStoreController()
        {
            _defautNotificationTime = int.Parse(System.Configuration.ConfigurationManager.AppSettings["DefaultNotificationTime"]);
        }

        /// <summary>
        /// Hiển thị thông báo
        /// </summary>
        /// <param name="message">Thông báo ra màn hình</param>
        /// <param name="type">Loại thông báo:
        ///     1. Success.
        ///     2. Error.
        ///     3. Proccessing.
        ///     4. Warning.
        /// </param>
        /// <param name="time">Thời gian hiển thông báo, mặc định thời gian lấy từ Web.config: DefaultNotificationTime</param>
        public virtual void Notification(string message, NotificationType? type, int? time)
        {
            TempData[GetNotificationType(type)] = message;
            TempData["NotificationTime"] = time ?? _defautNotificationTime;
        }

        /// <summary>
        /// Thông báo thành công
        /// </summary>
        /// <param name="message"></param>
        public virtual void SuccessNotification(string message)
        {
            TempData["SuccessNotification"] = message;
            TempData["NotificationTime"] = _defautNotificationTime;
        }

        /// <summary>
        /// Thông báo lỗi
        /// </summary>
        /// <param name="message"></param>
        public virtual void ErrorNotification(string message)
        {
            TempData["ErrorNotification"] = message;
            TempData["NotificationTime"] = _defautNotificationTime;
        }

        /// <summary>
        /// Lấy ra tên thông điệp theo loại
        /// </summary>
        /// <param name="type">Loại thông báo:
        ///     1. Success.
        ///     2. Error.
        ///     3. Proccessing.
        ///     4. Warning.
        /// </param>
        /// <returns>Tên thông điệp</returns>
        private string GetNotificationType(NotificationType? type)
        {
            switch (type)
            {
                case NotificationType.Error:
                    return "ErrorNotification";
                case NotificationType.Processing:
                    return "ProcessingNotification";
                case NotificationType.Warning:
                    return "WarningNotification";
                default:
                    return "SuccessNotification";
            }
        }
    }
}