using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace peer7
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1",
                    new OpenApiInfo
                    {
                        Title = "Message service by Me",
                        Description = @"Данное приложение выполняет весь, заданный в тз функционал:
Программа обладает следующей функциональностью:
1. Механизм чтения и записи списка пользователей и сообщений из
соответствующих JSON-файлов (в соответствующие JSON-файлы).
Список пользователей хранится в упорядоченном виде, т.е. сортируется 
лексикографически по почтовому адресу (Email) по возрастанию.
Пользователи обладают двумя свойствами – string UserName, string Email.
Сообщения обладают четырьмя свойствами: Subject, Message, SenderId, ReceiverId.
Данный функционал должен использоваться обработчиками, перечисленными ниже.
2. Реализовать обработчик, доступный по методу POST для инициализации
(т.е. первоначального заполнения) списка пользователей и списка
сообщений случайным образом (с использованием Random). С
использованием указанного обработчика заполнить списки.
3. Реализовать два обработчика, доступных только через метод GET:
a) для получения информации о пользователе по его идентификатору
(Email), учитывая, что при отсутствии пользователя код ответа должен
быть HTTP 404 (not found);
b) для получения всего списка пользователей.
4. Реализовать обработчик GET для получения списка сообщений по
идентификатору отправителя и получателя.
5. Реализовать обработчики GET для получения списка сообщений:
a) по идентификатору отправителя (получатель – любой);
b) по идентификатору получателя (отправитель – любой).
6. Использовать Swagger для получения автоматически генерируемой
документации по реализованным обработчикам.
7. [Дополнительный функционал] Предусмотреть возможность
регистрации новых пользователей: создать обработчик (POST),
добавляющий информацию о новом пользователе в систему (через поля
формы или JSON – решать вам).
8. [Дополнительный функционал] Предусмотреть возможность отправки
сообщений: создать обработчик (POST), добавляющий информацию о
новом сообщении. Предусмотреть проверку того, что отправитель и
получатель сообщения – зарегистрированные пользователи (т.е.
существуют в списке пользователей). Вернуть сообщение об ошибке,
если хотя бы одного из пользователей нет в списке (само сообщение при
этом не сохраняется).
9. [Дополнительный функционал] Доработать обработчик для получения
списка всех пользователей (п. 3b), реализовав поддержку функционала
для постраничной выборки – добавить поддержку параметров Limit и
Offset:
• int Limit – количество пользователей, которое необходимо вернуть
(максимальное);
• int Offset – порядковый номер пользователя, начиная с которого 
необходимо получать информацию (другими словами – количество
пользователей, которые необходимо пропустить с начала списка).
При отрицательных значениях Offset или неположительных значениях
Limit – возвращать HTTP 400 (bad request).
Список пользователей для определения порядкового номера
сортируется лексикографически по Email (по возрастанию).
JSON-файлы находятся в папке data. Удачной проверки!)",
                        Version = "v1"
                    }
                );

                var filePath = Path.Combine(AppContext.BaseDirectory, "peer7.xml");
                c.IncludeXmlComments(filePath);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "peer7 v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
