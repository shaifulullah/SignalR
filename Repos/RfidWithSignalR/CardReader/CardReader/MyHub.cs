using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace CardReader
{
    public class MyHub : Hub
    {
        public void Announce(string message)
        {
            Clients.All.Announce(message);
        }

        SerialPort myPort = new SerialPort("COM4");
        string indata = "From my method";
        private void DataReceivedHandler(object sender, SerialDataReceivedEventArgs e)
        {
            SerialPort sp = (SerialPort)sender;
            indata = sp.ReadExisting().Trim();
            Console.WriteLine("Writing from event");
        }
        public string GetSerialPortOutput()
        {
            myPort.BaudRate = 9600;
            myPort.Parity = Parity.None;
            myPort.StopBits = StopBits.One;
            myPort.DataBits = 8;
            myPort.Handshake = Handshake.None;
            myPort.RtsEnable = true;
            myPort.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);
            return indata;
        }
    }
}