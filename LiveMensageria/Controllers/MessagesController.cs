using LiveMensageria.InputModels;
using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace LiveMensageria.Controllers
{
    [Route("api/messages")]
    public class MessagesController : ControllerBase
    {
        private const string QUEUE_NAME = "messages";
        private readonly ConnectionFactory _factory;
        public MessagesController()
        {
            _factory = new ConnectionFactory
            {
                HostName = "localhost"
            };
        }

        [HttpPost]
        public IActionResult SendMessage([FromBody] SendMessageInputModel sendMessageInputModel)
        {
            // #somostodosdto

            using (var connection = _factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    // Declara a fila para que caso ela não exista ainda, eu crie ela
                    channel.QueueDeclare(
                        queue: QUEUE_NAME,
                        durable: false,
                        exclusive: false,
                        autoDelete: false,
                        arguments: null
                        );

                    // Formatar os dados para envio para a fila
                    var stringMessage = JsonSerializer.Serialize(sendMessageInputModel);
                    var byteArray = Encoding.UTF8.GetBytes(stringMessage);

                    channel.BasicPublish(
                        exchange: "",
                        routingKey: QUEUE_NAME,
                        basicProperties: null,
                        body: byteArray
                        );
                }
            }

            return Accepted();
        }
    }
}
