using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace SerialCommunication
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                string[] portNames = SerialPort.GetPortNames().Distinct().ToArray();
                comboBoxPoort.Items.Clear();
                comboBoxPoort.Items.AddRange(portNames);
                if (comboBoxPoort.Items.Count > 0) comboBoxPoort.SelectedIndex = 0;

                comboBoxBaudrate.SelectedIndex = comboBoxBaudrate.Items.IndexOf("115200");
            }
            catch (Exception)
            { }
        }

        private void cboPoort_DropDown(object sender, EventArgs e)
        {
            try
            {
                string selected = (string)comboBoxPoort.SelectedItem;
                string[] portNames = SerialPort.GetPortNames().Distinct().ToArray();

                comboBoxPoort.Items.Clear();
                comboBoxPoort.Items.AddRange(portNames);

                comboBoxPoort.SelectedIndex = comboBoxPoort.Items.IndexOf(selected);
            }
            catch (Exception)
            {
                if (comboBoxPoort.Items.Count > 0) comboBoxPoort.SelectedIndex = 0;
            }
        }

        private void buttonConnect_Click(object sender, EventArgs e)
        {
            try
            {
                if (serialPort1Arduino.IsOpen)
                {
                    //wilt verbreken
                    serialPort1Arduino.Close();
                    radioButtonVerbonden.Checked = false;
                    buttonConnect.Text = "Connect";
                    labelStatus.Text = "Status: Disconected";
                }
                else
                {
                    //wilt verbinden
                    serialPort1Arduino.PortName = (string)comboBoxPoort.SelectedItem;
                    serialPort1Arduino.BaudRate = Int32.Parse((string)comboBoxBaudrate.SelectedItem);
                    serialPort1Arduino.DataBits = (int)numericUpDownDatabits.Value;

                    if (radioButtonParityEven.Checked) serialPort1Arduino.Parity = Parity.Even;
                    else if (radioButtonParityOdd.Checked) serialPort1Arduino.Parity = Parity.Odd;
                    else if (radioButtonParityNone.Checked) serialPort1Arduino.Parity = Parity.None;
                    else if (radioButtonParityMark.Checked) serialPort1Arduino.Parity = Parity.Mark;
                    else if (radioButtonParitySpace.Checked) serialPort1Arduino.Parity = Parity.Space;

                    if (radioButtonStopbitsNone.Checked) serialPort1Arduino.StopBits = StopBits.None;
                    else if (radioButtonStopbitsOne.Checked) serialPort1Arduino.StopBits = StopBits.One;
                    else if (radioButtonStopbitsOnePointFive.Checked) serialPort1Arduino.StopBits = StopBits.OnePointFive;
                    else if (radioButtonStopbitsTwo.Checked) serialPort1Arduino.StopBits = StopBits.Two;

                    if (radioButtonHandshakeNone.Checked) serialPort1Arduino.Handshake = Handshake.None;
                    else if (radioButtonHandshakeRTS.Checked) serialPort1Arduino.Handshake = Handshake.RequestToSend;
                    else if (radioButtonHandshakeRTSXonXoff.Checked) serialPort1Arduino.Handshake = Handshake.RequestToSendXOnXOff;
                    else if (radioButtonHandshakeXonXoff.Checked) serialPort1Arduino.Handshake = Handshake.XOnXOff;

                    serialPort1Arduino.RtsEnable = checkBoxRtsEnable.Checked;
                    serialPort1Arduino.DtrEnable = checkBoxDtrEnable.Checked;

                    serialPort1Arduino.Open();
                    string commando = "ping";
                    serialPort1Arduino.WriteLine(commando);
                    string antwoord = serialPort1Arduino.ReadLine();
                    antwoord = antwoord.TrimEnd();
                    if (antwoord == "pong")
                    {
                        radioButtonVerbonden.Checked = true;
                        buttonConnect.Text = "Disconnect";
                        labelStatus.Text = "Status: Connected";
                    }
                    else
                    {
                        serialPort1Arduino.Close();
                        labelStatus.Text = "Error: verkeerd antwoord";
                        radioButtonVerbonden.Checked = false;
                    }
                }
            }
            catch (Exception exception)
            {
                labelStatus.Text = "Error:" + exception.Message;
                serialPort1Arduino.Close();
                radioButtonVerbonden.Checked = false;
                buttonConnect.Text = "Connect";
            }
        }

        private void checkBoxDigital2_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (serialPort1Arduino.IsOpen)
                {
                    string commando; //set d2 high/low
                    if (checkBoxDigital2.Checked) commando = "set d2 high";
                    else commando = "set d2 low";
                    serialPort1Arduino.WriteLine(commando);
                }
            }
            catch (Exception exception) 
            {
                labelStatus.Text = "Error: " + exception.Message;
                serialPort1Arduino.Close();
                radioButtonVerbonden.Checked = false;
                buttonConnect.Text = "connect";
            }

        }

        private void checkBoxDigital3_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (serialPort1Arduino.IsOpen)
                {
                    string commando; //set d3 high/low
                    if (checkBoxDigital3.Checked) commando = "set d3 high";
                    else commando = "set d3 low";
                    serialPort1Arduino.WriteLine(commando);
                }
            }
            catch (Exception exception)
            {
                labelStatus.Text = "Error: " + exception.Message;
                serialPort1Arduino.Close();
                radioButtonVerbonden.Checked = false;
                buttonConnect.Text = "connect";
            }
        }

        private void checkBoxDigital4_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (serialPort1Arduino.IsOpen)
                {
                    string commando; //set d4 high/low
                    if (checkBoxDigital4.Checked) commando = "set d4 high";
                    else commando = "set d4 low";
                    serialPort1Arduino.WriteLine(commando);
                }
            }
            catch (Exception exception)
            {
                labelStatus.Text = "Error: " + exception.Message;
                serialPort1Arduino.Close();
                radioButtonVerbonden.Checked = false;
                buttonConnect.Text = "connect";
            }
        }

        private void tabPageOefening5_Click(object sender, EventArgs e)
        {
            timerOef5.Interval = 1000;

            timerOef5.Start();
          
            try
            {
                if (serialPort1Arduino.IsOpen)
                {

                    // === GEWENSTE TEMPERATUUR (analoge pin 0) ===
                    // Herschaling: 0..1023 → 5..45 °C
                    // Richtingscoëfficiënt: (45 - 5) / (1023 - 0) = 40 / 1023 ≈ 0,0391
                    // Offset: 5 (wanneer x = 0, moet y = 5)

                    double richtingscoefficientGewenst = 40.0 / 1023.0;
                    double offsetGewenst = 5.0;

                    // Lees analoge pin 0 uit via seriële communicatie
                    // Stuur commando naar Arduino en lees antwoord
                    serialPort1Arduino.WriteLine("READ_A0");
                    string antwoordA0 = serialPort1Arduino.ReadLine().Trim();

                    int waardeA0 = int.Parse(antwoordA0);
                    double gewensteTemp = richtingscoefficientGewenst * waardeA0 + offsetGewenst;

                    labelGewensteTemp.Text = gewensteTemp.ToString("F1") + " °C";


                    // === HUIDIGE TEMPERATUUR (analoge pin 1) ===
                    // Herschaling: 0..1023 → 0..500 °C
                    // Richtingscoëfficiënt: (500 - 0) / (1023 - 0) = 500 / 1023 ≈ 0,4888
                    // Offset: 0 (wanneer x = 0, moet y = 0)

                    double richtingscoefficientHuidig = 500.0 / 1023.0;
                    double offsetHuidig = 0.0;

                    // Lees analoge pin 1 uit via seriële communicatie
                    serialPort1Arduino.WriteLine("READ_A1");
                    string antwoordA1 = serialPort1Arduino.ReadLine().Trim();

                    int waardeA1 = int.Parse(antwoordA1);
                    double huidigeTemp = richtingscoefficientHuidig * waardeA1 + offsetHuidig;

                    labelHuidigeTemp.Text = huidigeTemp.ToString("F1") + " °C";

                    if (huidigeTemp < gewensteTemp)
                    {
                        // LED AAN
                        serialPort1Arduino.WriteLine("SET_D2:HIGH");

                    }
                    else
                    {
                        // LED UIT
                        serialPort1Arduino.WriteLine("SET_D2:LOW");
                    }

            }   }
            catch (Exception exception)
            {
                labelStatus.Text = "Error: " + exception.Message;
                serialPort1Arduino.Close();
                radioButtonVerbonden.Checked = false;
                buttonConnect.Text = "connect";
            }

            timerOef5.Stop();
        }
    }
}
