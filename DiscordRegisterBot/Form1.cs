using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DiscordRegisterBot
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
    
        }

        private async void Form1_Load(object sender, System.EventArgs e)
        {
           await Program.MainAsync();
        }



    }
}
