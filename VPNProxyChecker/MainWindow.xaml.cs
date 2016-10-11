using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace VPNProxyChecker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnCheck_Click(object sender, RoutedEventArgs e)
        {
            lblResponse.Content = "";
            if (txtIP.Text == "")
            {
                lblResponse.Content = "IP Field Is Empty!";
                return;
            }
            else
            {

                if(txtAPI.Text == "")
                {
                    //NO API KEY
                    string remoteUri = "http://tools.xioax.com/networking/ip/" + txtIP.Text;

                    // Create a new WebClient instance.
                    WebClient myWebClient = new WebClient();
                    // Download the Web resource and save it into a data buffer.
                    byte[] myDataBuffer = myWebClient.DownloadData(remoteUri);

                    // Display the downloaded data.
                    string download = Encoding.ASCII.GetString(myDataBuffer);

                    VPN vpn = JsonConvert.DeserializeObject<VPN>(download);
                    if(vpn.status != "success")
                    {
                        lblResponse.Content = "Couldn't Resolve Address. " + vpn.msg;
                        return;
                    }
                    else
                    {
                        //Resolve completed.
                        if(download.Contains("true"))
                        {
                            //This is a vpn
                            lblResponse.Content = "This IP is a VPN / Proxy.\nIt belongs to the company: \n" + vpn.org + "\n It's hosted in:\n" + vpn.cc;
                        }
                        else
                        {
                            //This is not a vpn
                            lblResponse.Content = "This IP does not seem to belong to a VPN / Proxy company.";
                        }
                    }
                    

                }
                else
                {
                    //HAS API KEY
                    string remoteUri = "http://tools.xioax.com/networking/ip/" + txtIP.Text + "/" + txtAPI;

                    // Create a new WebClient instance.
                    WebClient myWebClient = new WebClient();
                    // Download the Web resource and save it into a data buffer.
                    byte[] myDataBuffer = myWebClient.DownloadData(remoteUri);

                    // Display the downloaded data.
                    string download = Encoding.ASCII.GetString(myDataBuffer);

                    VPN vpn = JsonConvert.DeserializeObject<VPN>(download);
                    if (vpn.status != "success")
                    {
                        lblResponse.Content = "Couldn't Resolve Address. " + vpn.msg;
                        return;
                    }
                    else
                    {
                        //Resolve completed.
                        if (vpn.org != "" && vpn.cc != "")
                        {
                            //This is a vpn
                            lblResponse.Content = "This IP is a VPN / Proxy. It belongs to the company: " + vpn.org + ". It's hosted in: " + vpn.cc;
                        }
                        else
                        {
                            //This is not a vpn
                            lblResponse.Content = "This IP does not seem to belong to a VPN / Proxy company.";
                        }
                    }
                }

            }

          
        }
    }
    public class VPN
    {
        public string status { get; set; }
        public string msg { get; set; }
        public string org { get; set; }
        public string cc { get; set; }
    }
}
