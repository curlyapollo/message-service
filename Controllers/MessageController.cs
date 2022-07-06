using Microsoft.AspNetCore.Mvc;
using peer7.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;

namespace peer7.Controllers
{
    /// <summary>
    /// Контроллер сервиса
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class MessageController : Controller
    {
        /// <summary>
        /// Статический конструктор с заполнением списков из файлов.
        /// </summary>
        static MessageController()
        {
            using (var fs = new FileStream("data/messages.json", FileMode.Open, FileAccess.Read))
            {
                var formatterMessage = new DataContractJsonSerializer(typeof(List<MessageClass>));
                Messages = (List<MessageClass>) formatterMessage.ReadObject(fs);
            }
            using (var fs = new FileStream("data/users.json", FileMode.Open, FileAccess.Read))
            {
                var formatterUser = new DataContractJsonSerializer(typeof(List<User>));
                _users = (List<User>) formatterUser.ReadObject(fs);
            }
        }
        private static Random rnd = new();
        private static List<User> _users;
        private static readonly  List<MessageClass> Messages;
        private const string Alphabet = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

        /// <summary>
        /// Генерация Email по имени.
        /// </summary>
        /// <param name="userName">Имя пользователя.</param>
        /// <returns>Email пользователя.</returns>
        private static string GenerationOfEmail(string userName)
        {
            return userName + "@edu.hse.ru";
        }

        /// <summary>
        /// Генерация строки.
        /// </summary>
        /// <returns>Искомая строка.</returns>
        private static string GenerationOfText()
        {
            // Генерация длины текста.
            int length = rnd.Next(5, 10);
            string result = "";
            do
            {
                for (int i = 0; i < length; i++)
                {
                    result += Alphabet[rnd.Next(Alphabet.Length)];
                }
                // Генерация уникального текста.
            } while (_users.Exists(x => x.UserName == result));
            return result;
        }
        
        /// <summary>
        /// Сериализация списка пользователей.
        /// </summary>
        private void SerializationOfUsers()
        {
            DataContractJsonSerializer serializerUsers = new DataContractJsonSerializer(typeof(List<User>));
            using (FileStream fs = new FileStream("data/users.json", FileMode.Create))
            {
                serializerUsers.WriteObject(fs, _users);
            }
        }

        /// <summary>
        /// Сериализация списка сообщений.
        /// </summary>
        private static void SerializationOfMessages()
        {
            DataContractJsonSerializer serializerMessages = new DataContractJsonSerializer(typeof(List<MessageClass>));
            using (FileStream fs = new FileStream("data/messages.json", FileMode.Create))
            {
                serializerMessages.WriteObject(fs, Messages);
            }
        }

        /// <summary>
        /// Заполнение списков.
        /// </summary>
        /// <returns>Результат операции.</returns>
        [HttpPost]
        public IActionResult FillingOfLists()
        {
            try
            {
                int usersLength = rnd.Next(1, 11);
                int messagesLength = rnd.Next(1, 11);
                for (int i = 0; i < usersLength; i++)
                {
                    var userName = GenerationOfText();
                    var email = GenerationOfEmail(userName);
                    _users.Add(new User { UserName = userName, Email = email});
                }
                _users = _users.OrderBy(x => x.Email).ToList();
                for (int i = 0; i < messagesLength; i++)
                {
                    var message = GenerationOfText();
                    var subject = GenerationOfText();
                    var senderId = _users[rnd.Next(_users.Count)].Email;
                    var receiverId = _users.Where(x => x.Email != senderId).ToList()[rnd.Next(_users.Count - 1)].Email;
                    Messages.Add(new MessageClass { Message = message, Subject = subject, SenderId = senderId, ReceiverId = receiverId });
                }
                SerializationOfUsers();
                SerializationOfMessages();
                return Ok("Списки созданы!");
            }
            catch (Exception exception)
            {
                return BadRequest(exception.Message);
            }
        }

        /// <summary>
        /// Получение всех пользователей.
        /// </summary>
        /// <returns>Результат операции.</returns>
        [HttpGet("Получение всех пользователей")]
        public IActionResult GetUsers() => Ok(_users);

        /// <summary>
        /// Получение среза пользователей.
        /// </summary>
        /// <param name="limit">Максимальное количество пользователей.</param>
        /// <param name="offset">Индекс первого.</param>
        /// <returns>Результат операции.</returns>
        [HttpGet("Получение среза пользователей")]
        public IActionResult GetSliceOfUsers([Required] int limit, [Required] int offset)
        {
            if (offset < 0 || limit <= 0 || offset >= _users.Count)
            {
                return BadRequest("Некорректные параметры.");
            }
            return Ok(_users.GetRange(offset, Math.Min(limit, _users.Count - offset)));
        }

        /// <summary>
        /// Получение пользователя по Email'у.
        /// </summary>
        /// <param name="email">Email пользователя.</param>
        /// <returns>Результат операции.</returns>
        [HttpGet("Получение пользователя по Email")]
        public IActionResult GetUser([Required] string email)
        {
            if (_users.Exists(x => x.Email == email))
            {
                return Ok(_users.First(x => x.Email == email));
            }
            return NotFound("Пользователь не найден.");
        }

        /// <summary>
        /// Получение сообщений по отправителю и получателю.
        /// </summary>
        /// <param name="senderId">Email отправителя.</param>
        /// <param name="receiverId">Email получателя.</param>
        /// <returns>Результат операции.</returns>
        [HttpGet("Получение сообщений по отправителю и получателю")]
        public IActionResult GetMessagesByReceiverAndSender([Required] string senderId, [Required] string receiverId)
        {
            var messages = 
                Messages.Where(x => x.ReceiverId == receiverId && x.SenderId == senderId).ToList();
            return messages.Count == 0 ? NotFound("Таких сообщений нет.") : Ok(messages);
        }


        /// <summary>
        /// Получение сообщений по получателю.
        /// </summary>
        /// <param name="receiverId">Email получателя.</param>
        /// <returns>Результат операции.</returns>
        [HttpGet("Получение сообщений по получателю")]
        public IActionResult GetMessageByReceiver([Required] string receiverId)
        {
            var messages = Messages.Where(x => x.ReceiverId == receiverId).ToList();
            return messages.Count == 0 ? NotFound("Таких сообщений нет.") : Ok(messages);
        }

        /// <summary>
        /// Получение сообщений по отправителю.
        /// </summary>
        /// <param name="senderId">Email отправителя.</param>
        /// <returns>Результат операции.</returns>
        [HttpGet("Получение сообщений по отправителю")]
        public IActionResult GetMessagesBySender([Required] string senderId)
        {
            var messages = Messages.Where(x => x.SenderId == senderId).ToList();
            return messages.Count == 0 ? NotFound("Таких сообщений нет.") : Ok(messages);
        }
        
        /// <summary>
        /// Добавление нового пользователя
        /// </summary>
        /// <param name="userName">Имя нового пользователя.</param>
        /// <returns>Результат операции.</returns>
        [HttpPost("Добавление пользователя")]
        public IActionResult AdditionOfUser([Required] string userName)
        {
            try
            {
                if (_users.Exists(x => x.UserName == userName))
                {
                    return NotFound("Пользователь с данным именем уже существует.");
                }
                User newUser = new User() {UserName = userName, Email = GenerationOfEmail(userName)};
                _users.Add(newUser);
                _users = _users.OrderBy(x => x.Email).ToList();
                SerializationOfUsers();
                return Ok(newUser);
            }
            catch (Exception exception)
            {
                return BadRequest(exception.Message);
            }
        }
        
        /// <summary>
        /// Добавление нового сообщения.
        /// </summary>
        /// <param name="subject">Тема нового сообщения.</param>
        /// <param name="message">Текст нового сообщения.</param>
        /// <param name="senderId">Email отправителя.</param>
        /// <param name="receiverId">Email получателя.</param>
        /// <returns>Результат операции.</returns>
        [HttpPost("Добавление сообщения")]
        public IActionResult AdditionOfMessage([Required] string subject, [Required] string message, [Required]string senderId, [Required] string receiverId)
        {
            try
            {
                if (!_users.Exists(x => x.Email == senderId) || !_users.Exists(x => x.Email == receiverId))
                {
                    return NotFound("Задан несуществующий email.");
                }
                MessageClass newMessage = new MessageClass() 
                    {Subject = subject, Message = message, ReceiverId = receiverId, SenderId = senderId};
                Messages.Add(newMessage);
                SerializationOfMessages();
                return Ok(newMessage);
            }
            catch (Exception exception)
            {
                return BadRequest(exception.Message);
            }
        }
    }
}
