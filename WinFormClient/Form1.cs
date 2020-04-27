using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections;
using WinFormClient.ServiceY;
using System.Net;
using System.Diagnostics;
using System.IO;
using Newtonsoft.Json;

namespace WinFormClient
{

    

    public partial class Form1 : Form
    {
        private class Welcome
        {
            public string getURIOfSegmentResult
            {
                get;
                set;
            }
        }
        RequestYClient req;
        VelibStation[] stations;
        VelibStation stationDepart, stationArrivee;

        private string uri_string;

        private Boolean segment1, segment2, segment3;
        VelibStation nearestStationDepart, nearestStationArrivee;
        Position posDepart, posArrivee;

        

        public Form1()
        {
            InitializeComponent();
            req = new RequestYClient();
            Contract[] cList = req.getContracts();
            foreach (Contract c in cList)
                comboboxVille.Items.Add(c.name);

            req.refreshStationList("toulouse");
            testPOST();

            req.refreshStationList("toulouse");
            textBoxAdresseDepart.Text = "135 avenue de rangueil";
            textBoxAdresseArrivee.Text = "2 place du peyrou";
            button1.Visible = false;
            button2.Visible = false;
            button3.Visible = false;
            webBrowserMap.Visible = false;
        }

        

        private void buttonCalcul_Click(object sender, EventArgs e)
        {
            string adresseDepart = textBoxAdresseDepart.Text,
                adresseArrivee = textBoxAdresseArrivee.Text;
            posDepart = req.geocodingAddress(adresseDepart);
            posArrivee = req.geocodingAddress(adresseArrivee);
            

            if (posDepart != null && posArrivee != null)
            {
                nearestStationDepart = req.computeNearestStation(posDepart);
                nearestStationArrivee = req.computeNearestStation(posArrivee);
                if (nearestStationDepart != null && nearestStationArrivee != null)
                {
                    button1.Visible = true;
                    button2.Visible = true;
                    button3.Visible = true;
                    webBrowserMap.Visible = true;
                    segment1 = segment2 = segment3 = false;
                    segment1 = true;

                    uri_string = req.getURIOfSegment(
                        posDepart.lat, posDepart.lng,
                        nearestStationDepart.position.lat, nearestStationDepart.position.lng,
                        "foot");
                    labelInfos.Text = uri_string;
                    textBoxDebug.Text = uri_string;
                    webBrowserMap.Url = new Uri(uri_string);
                    webBrowserMap.Update();

                    
                    webBrowserMap.Navigate(uri_string);
                } else
                {
                    labelInfos.Text = "ERROR: lors du calcul d'itinéraire :(\n";
                }       
            } else {
                labelInfos.Text = "ERROR:\tAdresse(s) non reconnue\n";
                if (posDepart == null)
                {
                     labelInfos.Text += adresseDepart + "\n";
                } 
                if (posArrivee == null)
                {
                    labelInfos.Text += adresseArrivee + "\n";
                }
            }
        }

        private void comboboxVille_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            labelInfos.Text = "Patientez svp...\n";
            string selectedVille = comboboxVille.SelectedItem.ToString();

            req.refreshStationList(selectedVille);
            stations = req.getAllStations();
            labelInfos.Text = "Liste des stations bien récupérée.\n";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (segment1 == false)
            {
                if (nearestStationArrivee != null && posArrivee != null)
                {
                    segment1 = segment2 = segment3 = false;
                    segment1 = true;

                    uri_string = req.getURIOfSegment(
                        posDepart.lat, posDepart.lng,
                        nearestStationDepart.position.lat, nearestStationDepart.position.lng,
                        "foot");
                    labelInfos.Text = uri_string;
                    textBoxDebug.Text = uri_string;
                    webBrowserMap.Url = new Uri(uri_string);
                    webBrowserMap.Update();
                }
                else
                {
                    labelInfos.Text = "ERROR: lors du calcul d'itinéraire :(\n";
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (segment2 == false)
            {
                if (nearestStationDepart != null && nearestStationArrivee != null)
                {
                    segment1 = segment2 = segment3 = false;
                    segment2 = true;

                    uri_string = req.getURIOfSegment(
                        nearestStationDepart.position.lat, nearestStationDepart.position.lng,
                        nearestStationArrivee.position.lat, nearestStationArrivee.position.lng,
                        "bike");
                    labelInfos.Text = uri_string;
                    textBoxDebug.Text = uri_string;
                    webBrowserMap.Url = new Uri(uri_string);
                    webBrowserMap.Update();
                }
                else
                {
                    labelInfos.Text = "ERROR: lors du calcul d'itinéraire :(\n";
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (segment3 == false)
            {
                if (nearestStationArrivee != null && posArrivee != null)
                {
                    segment1 = segment2 = segment3 = false;
                    segment3 = true;

                    uri_string = req.getURIOfSegment(
                        nearestStationArrivee.position.lat, nearestStationArrivee.position.lng,
                        posArrivee.lat, posArrivee.lng,
                        "foot");
                    labelInfos.Text = uri_string;
                    textBoxDebug.Text = uri_string;
                    webBrowserMap.Url = new Uri(uri_string);
                    webBrowserMap.Update();
                }
                else
                {
                    labelInfos.Text = "ERROR: lors du calcul d'itinéraire :(\n";
                }
            }
        }

        private void comboBoxDepart_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedStationName = comboBoxDepart.SelectedItem.ToString();
            stationDepart = getStationWithName(selectedStationName);
        }

        private void comboBoxArrivee_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedStationName = comboBoxArrivee.SelectedItem.ToString();
            stationArrivee = getStationWithName(selectedStationName);
        }

        private VelibStation getStationWithName(string name)
        {
            for (int i = 0; i < stations.Length; i++)
            {
                if (stations[i].name == name)
                    return stations[i];
            }
            return null;
        }

        private void testREST()
        {
            string _url = "http://localhost:8733/Design_Time_Addresses/ServiceY/Service1/JSONendpoint/uriOfSegment?"
                + "dep_lat=43%2E5714&dep_lng=1%2E4650&arr_lat=43%2E6073&arr_lng=1%2E4398&transporation_way=bike";
            WebRequest request;
            WebResponse response;
            Stream dataStream;
            StreamReader reader;
            request = WebRequest.Create(_url);
            response = request.GetResponse();

            dataStream = response.GetResponseStream();
            reader = new StreamReader(dataStream);
            string responseFromServer = reader.ReadToEnd();

            Welcome answerJSON = JsonConvert.DeserializeObject<Welcome>(responseFromServer);
            textBoxDebug.Text = answerJSON.getURIOfSegmentResult;
        }

        private void testPOST()
        {
            WebRequest request = WebRequest.Create(
                "http://localhost:8733/Design_Time_Addresses/ServiceY/Service1/JSONendpoint/computeNearestStation");
            // { "lat":43.569183,"lng":1.466106}
            // string postData = "lat=" + Uri.EscapeDataString("43.569183");
            // postData += "&lng=" + Uri.EscapeDataString("1.466106");
            string postData = "{\"lat\":43.569183, \"lng\":1.466106}";
            byte[] data = Encoding.ASCII.GetBytes(postData);

            request.Method = "POST";
            request.ContentType = "application/json";
            request.ContentLength = data.Length;

            using (var stream = request.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }

            WebResponse response = request.GetResponse();

            string responseFromServer = new StreamReader(response.GetResponseStream()).ReadToEnd();
            VelibStation answerJSON = JsonConvert.DeserializeObject<VelibStation>(responseFromServer);
            labelInfos.Text = answerJSON.ToString();
        }
    }
}

/* BROUILLON */

// https://api.openrouteservice.org/v2/directions/cycling-road?api_key=5b3ce3597851110001cf62488f2206f7d24a4fbd967c3ba5f827961e&start=7.495774,43.778858&end=7.488136,43.80276
// +MAP_API_KEY + "&start=7.495774,43.778858&end=7.488136,43.80276
//  uri_string = "https://api.openrouteservice.org/v2/directions/cycling-road?api_key="
// + MAP_API_KEY + "&start=7.495774,43.778858&end=7.488136,43.802761";
// string OPENSOURCE_MAP_API_KEY = "5b3ce3597851110001cf62488f2206f7d24a4fbd967c3ba5f827961e";

/* 
void browser_Navigating(object sender, System.Windows.Forms.WebBrowserNavigatingEventArgs e)
{
    e.Cancel = true;
    //Open Link
    Process.Start("www.bing.fr");
}

private void WebBrowser1_NewWindow(object sender, CancelEventArgs e)
{
    e.Cancel = true; //stop normal new window activity

    //get the url you were trying to navigate to
    var url = webBrowserMap.Document.ActiveElement.GetAttribute("href");
    //set up the tabs
    TabPage tp = new TabPage();
    var wb = new WebBrowser();
    wb.Navigated += Wb_Navigated;
    wb.Size = this.webBrowserMap.Size;
    tp.Controls.Add(wb);
    wb.Navigate(url);
    this.tabControlMap.Controls.Add(tp);
    tabControlMap.SelectedTab = tp;

    url_index += 1;
}

private void Wb_Navigated(object sender, WebBrowserNavigatedEventArgs e)
{
    tabControlMap.SelectedTab.Text = (sender as WebBrowser).DocumentTitle;
}
*/
