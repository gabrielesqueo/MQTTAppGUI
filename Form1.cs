using System.Threading.Tasks;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Options;

namespace MQTTAppGUI
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Shown += Form1_Shown;
        }

        public void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private void Form1_Shown(Object sender, EventArgs e)
        {
            MessageBox.Show("ciao");
        }


        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            var factory = new MqttFactory();
            var _Client = factory.CreateMqttClient();
            string clientid = Guid.NewGuid().ToString();

            Console.WriteLine("Confiruando Opzioni...");
            //Configurazione Opzioni
            var _options = new MqttClientOptionsBuilder()
                .WithClientId(clientid)
                .WithTcpServer("broker.mqttdashboard.com", 1883)
                .WithCredentials("User1", "123")
                .WithCleanSession()
                .Build();

            //Connessione
            _Client.ConnectAsync(_options).Wait();

            Console.WriteLine("Pubblicando Messaggio...");

            //Pubblicazione Messaggio
            var applicationMessage = new MqttApplicationMessageBuilder()
               .WithTopic("panetti/test")
               .WithPayload(textBox1.Text)
               .Build();

            _Client.PublishAsync(applicationMessage, CancellationToken.None);
            
            textBox1.ResetText();
            
        }
    }
}