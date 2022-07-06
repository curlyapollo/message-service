using System.ComponentModel.DataAnnotations;

namespace peer7.Helpers
{
    /// <summary>
    /// Класс сообщений.
    /// </summary>
    public class MessageClass
    {
        /// <summary>
        /// Тема сообщения.
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// Текст сообщения.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Идентификатор отправителя.
        /// </summary>
        public string SenderId { get; set; }

        /// <summary>
        /// Идентификатор получателя.
        /// </summary>
        public string ReceiverId { get; set; }

    }
}
