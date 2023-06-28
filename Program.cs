// See https://aka.ms/new-console-template for more information
using System.IO.Ports;
using ComDe;

System.Console.WriteLine(20 >> 4);
Console.WriteLine("Hello, World!");
SerialPort serialPort = new SerialPort();

serialPort.PortName = "COM1";
serialPort.BaudRate = 9600;
serialPort.DataBits = 8;
serialPort.Parity = Parity.None;
serialPort.StopBits = StopBits.One;

serialPort.Open();
// while(true)
// {



short startAdd = 0;
short writeLen = 10;
short data=-256;

List<byte> bytelst = new List<byte>();
bytelst.Add(0x01);
//bytelst.Add(0x01);
//bytelst.Add(0x03);

// bytelst.Add(BitConverter.GetBytes(startAdd)[1]);
// bytelst.Add(BitConverter.GetBytes(startAdd)[0]);

// bytelst.Add(BitConverter.GetBytes(writeLen)[1]);
// bytelst.Add(BitConverter.GetBytes(writeLen)[0]);

#region  write single coil
// bytelst.Add(0x05);//写单个线圈寄存器
// bytelst.Add(0x00);
// bytelst.Add(0x04);
// bytelst.Add(0xFF);//写单个线圈时,写值只能为0xFF00(on),0x0000 (off),其他均为非法值
// bytelst.Add(0x00);
#endregion
#region write single register
//bytelst.Add(0x06);//写单个保持寄存器
// bytelst.Add(0x00);
// bytelst.Add(0x01);
//  bytelst.Add(BitConverter.GetBytes(startAdd)[1]);
//  bytelst.Add(BitConverter.GetBytes(startAdd)[0]);

// bytelst.Add(0x0F);
// bytelst.Add(0x10);
//  bytelst.Add(BitConverter.GetBytes(data)[1]);
//  bytelst.Add(BitConverter.GetBytes(data)[0]);
#endregion 

#region write many coils
// //03 0F 00 00 00 0B 02 FF FF
// bytelst.Add(0x0F);//写多个线圈寄存器    
// //起始地址
// bytelst.Add(0x00);
// bytelst.Add(0x00);
// //写入线圈个数
// bytelst.Add(0x00);
// bytelst.Add(0x0B);
// //写入字节数
// bytelst.Add(0x02);
// //写入字节数
// bytelst.Add(0xFF);
// bytelst.Add(0xFF);


#endregion
//03 10 00 00 00 02 04 42 B5 33 33 A9 6C 
bytelst.Add(0x10);//写多个保持寄存器 
//起始地址
bytelst.Add(0x00);
bytelst.Add(0x02);
//写入寄存器数量
bytelst.Add(0x00);
bytelst.Add(0x02);
//写入字节数
bytelst.Add(0x04);
//写入字节数
bytelst.Add(0x42);
bytelst.Add(0xB5);
bytelst.Add(0x33);
bytelst.Add(0x33);

#region write many registers

#endregion

var bytes = CRC.CRC16_1(bytelst.ToArray());
bytelst.Add(bytes[0]);
bytelst.Add(bytes[1]);


//byte[] bytes = { 0x01, 0x01, 0x00, 0x00, 0x00, 0x0A, 0xBC, 0x0D };
System.Console.WriteLine("发送:");
foreach (var item in bytelst)
{
    System.Console.WriteLine(string.Format("{0:X2}", item));
}
while (true)
{
    serialPort.Write(bytelst.ToArray(), 0, bytelst.Count);
    serialPort.DataReceived += SerialPort_DataReceived;
    Thread.Sleep(1000);
}



void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
{

    int length = serialPort.BytesToRead;
    byte[] bytes = new byte[length];
    serialPort.Read(bytes, 0, length);
    // string text= Encoding.UTF8.GetString(bytes);
    System.Console.WriteLine("接收:");
    foreach (var item in bytes)
    {
        System.Console.WriteLine(string.Format("{0:X2}", item));
    }

}


