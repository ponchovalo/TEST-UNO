

using Lextm.SharpSnmpLib.Messaging;
using Lextm.SharpSnmpLib;
using System.Net;
using System.Net.Http.Headers;
using System.Net.NetworkInformation;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Timers;
using TEST_UNO;
using System.Linq;

namespace Test
{
    class Program
    {
        static void Main()
        {

            IniciarTimer();

            //GetContadores();
                
        }

        // METODOS SNMP CONTADORES

        public static Impresora[] GetImpresoras(){ 
            var item = "DataImpresora";
            string filename = $"C:/TESTING/TEST-UNO/TEST-UNO/{item}.json";
            string jsonString = File.ReadAllText(filename);
            Impresora[] m = JsonSerializer.Deserialize<Impresora[]>(jsonString)!;

            return m;

        }
        
        public static string GetForOID(string Ip, string OID, int TimeOut)
        {
            string valor = "";
            var res = Messenger.Get(VersionCode.V1,
                                       new IPEndPoint(IPAddress.Parse(Ip), 161),
                                       new OctetString("public"),
                                       new List<Variable> { new Variable(new ObjectIdentifier(OID)) },
                                       TimeOut);
            if (res != null)
            {
                valor = res[0].Data.ToString();
                return valor;
            }
            else
            {
                valor = "NO RESPONDE CONTADOR";
                return valor;
            }

        }
        
        public static void GetContadores()
        {
            Impresora[] impresoras = GetImpresoras();
            int interval = 10000;
            foreach (var item in impresoras)
            {
                try
                {
                    if (item.Modelo == "ILBP352DN" || item.Modelo == "MF525DW")
                    {

                        string cont102 = GetForOID(item.Ip, ".1.3.6.1.4.1.1602.1.11.1.3.1.4.101", interval);
                        string cont109 = cont102;
                        string cont124 = "0";
                        Console.WriteLine($"{item.Nombre} : {item.Ip} : 102 : {cont102} : 109 : {cont109} : 124 : {cont124}");
                     
                    }else if( item.Modelo == "IRA4545I")
                    {
                        string cont102 = GetForOID(item.Ip, ".1.3.6.1.4.1.1602.1.11.1.3.1.4.102", interval);
                        string cont109 = cont102;
                        string cont124 = "0";
                        Console.WriteLine($"{item.Nombre} : {item.Ip} : 102 : {cont102} : 109 : {cont109} : 124 : {cont124}");

                    }else if(item.Modelo == "C356IF")
                    {
                        
                        string cont102 = GetForOID(item.Ip, ".1.3.6.1.4.1.1602.1.11.1.3.1.4.102", interval);
                        string cont109 = GetForOID(item.Ip, ".1.3.6.1.4.1.1602.1.11.1.3.1.4.109", interval);
                        string cont124 = GetForOID(item.Ip, ".1.3.6.1.4.1.1602.1.11.1.3.1.4.124", interval);
                        Console.WriteLine($"{item.Nombre} : {item.Ip} : 102 : {cont102} : 109 : {cont109}  : 124 :  {cont124}");
                       
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine($"NOMBRE: {item.Nombre} - IP: {item.Ip} - NO SE PUEDE OBTENER");
                }
            }

        }


 
        // METODOS PING CADA 5 Seg

        public static void Ping(object source, ElapsedEventArgs e)
        {
            var interval = 1000;
            Ping ping = new Ping();

            Impresora[] Impresoras = GetImpresoras();

            Console.WriteLine(" \r{0} ", DateTime.Now);

            foreach (var item in Impresoras)
            {
                try
                {
                    var response = ping.Send(item.Ip, interval);
                    if (response.Status == IPStatus.Success)
                    {
                        Console.WriteLine($"Respuesta desde: {item.Nombre} \t {item.Ip} \t {response.Status}");
                    }
                    else
                    {
                        Console.WriteLine($"No hay respuesta: {item.Nombre} \t {item.Ip} \t {response.Status}");
                    }
                }
                catch (PingException)
                {
                    Console.WriteLine("Error de red");
                }
            }
        }

        private static void IniciarTimer()
        {
            var newTimer = new System.Timers.Timer();
            newTimer.Elapsed += new ElapsedEventHandler(Ping);
            newTimer.Interval = 5000;
            newTimer.Start();
            while (Console.Read() != 'q')
            {
                   
            }
        }

    }
}

