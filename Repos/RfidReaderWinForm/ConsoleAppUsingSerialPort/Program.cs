using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;
namespace Test_Serial
{
    class Program
    {
        //https://stackoverflow.com/questions/37238903/trouble-in-getting-rfid-tag-value-from-serial-port-of-computer

        static void Main(string[] args)
        {
            try
            {
                SerialPort myPort = new SerialPort();
                myPort.BaudRate = 9600;
                myPort.PortName = "COM4";
                myPort.Open();

                myPort.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);
                Console.WriteLine("Press any key to continue...");
                Console.WriteLine();
                Console.ReadKey();
                myPort.Close();
            }

            catch (Exception ex)
            {
                Console.WriteLine("Com port Disconnected!");
            }
        }

        private static void DataReceivedHandler(
                        object sender,
                        SerialDataReceivedEventArgs e)
        {
            SerialPort sp = (SerialPort)sender;
            string indata = sp.ReadLine();
            Console.WriteLine("Data Received:");
            Console.WriteLine(indata);

            if (String.Equals(indata, "160975869190"))
            {
                Console.WriteLine("Matched");
                sp.WriteLine("Matched");

            }
            else
            {
                Console.WriteLine("Not Matched");
                sp.WriteLine("Not Matched");
            }

            Console.WriteLine();

        }


    }
}




















//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Net;
//using System.Net.Sockets;
//using System.IO.Ports;
//using System.Threading;
////using System.Windows.Forms;
////using Gtk;

//namespace ConsoleAppUsingSerialPort
//{
//    //https://stackoverflow.com/questions/23799474/usb-rfid-reading-tag-using-c-sharp-connected-through-serial-port

//    class Program
//    {
//        public static SerialPort iSerialPort = new SerialPort();

//        static int Main(string[] args)
//        {
//            string strException = string.Empty;
//            string strComPort = "COM4";
//            int nBaudrate = Convert.ToInt32(9600);

//            int nRet = OpenCom(strComPort, nBaudrate, out strException);
//            if (nRet != 0)
//            {
//                string strLog = "Connect reader failed, due to: " + strException;
//                Console.WriteLine(strLog);
//                //return;
//            }
//            else
//            {
//                string strLog = "Reader connected " + strComPort + "@" + nBaudrate.ToString();
//                Console.WriteLine(strLog);
//            }
//            Console.WriteLine("Press any key to exit.");
//            Console.ReadKey();

//            iSerialPort.Close();
//            return 0;
//        }
//        public static int OpenCom(string strPort, int nBaudrate, out string strException)
//        {

//            strException = string.Empty;

//            if (iSerialPort.IsOpen)
//            {
//                iSerialPort.Close();
//            }

//            try
//            {
//                iSerialPort.PortName = strPort;
//                iSerialPort.BaudRate = nBaudrate;
//                iSerialPort.ReadTimeout = 3000;
//                iSerialPort.DataBits = 8;
//                iSerialPort.Parity = Parity.None;
//                iSerialPort.StopBits = StopBits.One;
//                iSerialPort.Open();
//            }
//            catch (System.Exception ex)
//            {
//                strException = ex.Message;
//                return -1;
//            }

//            return 0;
//        }
//    }
//}



































//using System;
//using System.IO.Ports;

//class PortDataReceived
//{
//  https://docs.microsoft.com/en-us/dotnet/api/system.io.ports.serialport.datareceived?redirectedfrom=MSDN&view=netframework-4.8
//    public static void Main()
//    {
//        SerialPort mySerialPort = new SerialPort("COM4");

//        mySerialPort.BaudRate = 9600;
//        mySerialPort.Parity = Parity.None;
//        mySerialPort.StopBits = StopBits.One;
//        mySerialPort.DataBits = 8;
//        mySerialPort.Handshake = Handshake.None;
//        mySerialPort.RtsEnable = true;

//        mySerialPort.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);

//        mySerialPort.Open();

//        Console.WriteLine("Press any key to continue...");
//        Console.WriteLine();
//        Console.ReadKey();
//        mySerialPort.Close();
//    }

//    private static void DataReceivedHandler(
//                        object sender,
//                        SerialDataReceivedEventArgs e)
//    {
//        SerialPort sp = (SerialPort)sender;
//        string indata = sp.ReadExisting();
//        Console.WriteLine("Data Received:");
//        Console.Write(indata);
//    }
//}
