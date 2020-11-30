using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        const int portNo = 25000;
        TcpClient client;
        byte[] data;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void btnSignIn_Click(object sender, EventArgs e)
        {
            if (btnSignIn.Text == "Sign In")
            {
                try
                {
                    TcpClient client = new TcpClient();
                    client.Connect("127.0.0.1", portNo);
                    data = new byte[client.ReceiveBufferSize];

                    sendMessage(txtNick.Text);
                }
            }
        }
    }
}
