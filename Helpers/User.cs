using System.ComponentModel.DataAnnotations;

namespace peer7.Helpers
{
    /// <summary>
    /// Класс пользователя.
    /// </summary>
    public class User
    {
        /// <summary>
        /// Идентификатор пользователя.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Имя пользователя.
        /// </summary>
        public string UserName { get; set; }
    }
}
