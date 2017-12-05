using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using IdentityModel.Client;
using Newtonsoft.Json.Linq;

namespace IdentityServerWinFormClient
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            //ServicePointManager.ServerCertificateValidationCallback += (sender2, cert, chain, sslPolicyErrors) => true;
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            var disco = await DiscoveryClient.GetAsync("http://localhost/identityserver");
            if (disco.IsError)
            {
                MessageBox.Show("" + disco.Error);
                return;
            }

            var tokenClient = new TokenClient(disco.TokenEndpoint, "ro.client", "secret");
            var tokenResponse = await tokenClient.RequestResourceOwnerPasswordAsync("bob", "password", "api1");

            if (tokenResponse.IsError)
            {
                MessageBox.Show("" + tokenResponse.Error);
                return;
            }
            textBox1.AppendText(tokenResponse.AccessToken + Environment.NewLine + Environment.NewLine);
            //return;

            var client = new HttpClient();
            client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", @"Bearer " + tokenResponse.AccessToken);

            var response = await client.GetAsync("http://localhost/IdentityServerClient/Identity");
            if (!response.IsSuccessStatusCode)
            {
                textBox1.AppendText(response.StatusCode + Environment.NewLine);
            }
            else
            {
                var content = await response.Content.ReadAsStringAsync();
                textBox1.AppendText(content + Environment.NewLine);
            }
        }

        private async void button2_Click_1(object sender, EventArgs e)
        {
            textBox1.Clear();
            var disco = await DiscoveryClient.GetAsync("http://localhost/identityserver");
            if (disco.IsError)
            {
                MessageBox.Show("" + disco.Error);
                return;
            }

            var tokenClient = new TokenClient(disco.TokenEndpoint, "client", "secret");
            var tokenResponse = await tokenClient.RequestClientCredentialsAsync("api1");
            if (tokenResponse.IsError)
            {
                MessageBox.Show("" + tokenResponse.Error);
                return;
            }
            textBox1.AppendText(tokenResponse.AccessToken + Environment.NewLine + Environment.NewLine);
            //return;


            var client = new HttpClient();
            client.SetBearerToken(tokenResponse.AccessToken);
            var response = await client.GetAsync("http://localhost/IdentityServerClient/Identity");
            if (!response.IsSuccessStatusCode)
            {
                textBox1.AppendText(response.StatusCode + Environment.NewLine);
            }
            else
            {
                var content = await response.Content.ReadAsStringAsync();
                textBox1.AppendText(content + Environment.NewLine);
            }
        }
    }
}
