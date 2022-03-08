using System.Text;
using System;
using System.ComponentModel;
using System.Windows.Forms;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Options;

namespace MQTTAppGUI
{
    public partial class Form1 : Form
    {
        IMqttClient _Client = null;
        public Form1()
        {
            InitializeComponent();
        }

        public void Form1_Load(object sender, EventArgs e)
        {
            var factory = new MqttFactory();
            _Client = factory.CreateMqttClient();
            string clientid = Guid.NewGuid().ToString();

            //Configurazione Opzioni
            var _options = new MqttClientOptionsBuilder()
                .WithClientId(clientid)
                .WithTcpServer("broker.mqttdashboard.com", 1883)
                .WithCredentials("User1", "123")
                .WithCleanSession()
                .Build();

            //Connessione
            _Client.ConnectAsync(_options).Wait();

            var mqttSubscribeOptions = factory.CreateSubscribeOptionsBuilder()
                               .WithTopicFilter(f => { f.WithTopic("panetti/topic"); })
                               .Build();

            _Client.SubscribeAsync(mqttSubscribeOptions).Wait();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
         
            //Pubblicazione Messaggio
            var applicationMessage = new MqttApplicationMessageBuilder()
               .WithTopic("panetti/topic")
               .WithPayload(textBox1.Text)
               .Build();

            _Client.PublishAsync(applicationMessage, CancellationToken.None);
           
            textBox1.ResetText();
            
        }

        private void timer1_Tick(object sender, EventArgs e)
        {

            //Lettura Messaggi
            _Client.UseApplicationMessageReceivedHandler(e =>
            {
                try
                {
                    string topic = e.ApplicationMessage.Topic;

                    if (string.IsNullOrWhiteSpace(topic) == false)
                    {
                        string payload = Encoding.UTF8.GetString(e.ApplicationMessage.Payload);
                        //Da completare
                        MessageBox.Show($"Topic: {topic}. Message Received: {payload}");
                        
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message, ex);
                }
            });
            
        }
    }
}