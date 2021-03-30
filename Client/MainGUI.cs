using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AirportModel;

namespace Client
{
    public partial class MainGUI : Form
    {
        internal ClientUI.Client client;
        public MainGUI()
        {
            client= new ClientUI.Client();
            InitializeComponent();
           

        }

        private async void buttonTrips_Click(object sender, EventArgs e)
        {
            var l=await client.GetAllAsync();
            string s = "Available trips: \n";
            foreach (var x in l)
            {
                s += x.From + "--" + x.To + "//" + x.Takeof+"__Id: "+x.TripId+"\n";
                screen.Text = s;
            }

        }

        private async void buttonSelectTrip_Click(object sender, EventArgs e)
        {
            int id;
            if (int.TryParse(textBoxTripSelector.Text, out id))
            {
                var x = await client.GetTrip(id);
                if (x != null)
                    screen.Text = x.From + "--" + x.To + "//" + x.Takeof + "__Id: " + x.TripId + "\n";
                else
                    screen.Text = "Trip with id " + id + " was not found";
            }
            else
            {
                screen.Text = "Wrong id format";
            }
        }

        private async void buttonDeleteTrip_Click(object sender, EventArgs e)
        {
            int id;
            if (int.TryParse(textBoxTripSelector.Text, out id))
            {
                var x = await client.GetTrip(id);
                if (x != null)
                {
                    await client.DeleteTripAsync(id);
                    screen.Text = x.From + "--" + x.To + "//" + x.Takeof + "__Id: " + x.TripId + " was deleted" + "\n";
                }
                    
                else
                    screen.Text = "Trip with id " + id + " was not found";
            }
            else
            {
                screen.Text = "Wrong id format";
            }
        }

        private async void buttonEditTrip_Click(object sender, EventArgs e)
        {
            int id;
            
            if (int.TryParse(textBoxTripSelector.Text, out id))
            {
                var x = await client.GetTrip(id);
                if (x != null)
                {
                    DaugtherGUI creator = new DaugtherGUI(this);
                    creator.Show();
                    creator.textBoxFrom.Text = x.From;
                    creator.textBoxTo.Text = x.To;
                    creator.dateTimePicker1.Value=x.Takeof;
                    creator.dateTimePicker2.Value=x.Arrival;
                    creator.tripId = x.TripId;


                    // screen.Text = x.From + "--" + x.To + "//" + x.Takeof + "__Id: " + x.TripId + " was edited" + "\n";
                }

                else
                    screen.Text = "Trip with id " + id + " was not found";
            }
            else
            {
                screen.Text = "Wrong id format";
            }

        }

        private void buttonCreateTrip_Click(object sender, EventArgs e)
        {
            DaugtherGUI creator = new DaugtherGUI(this);
            creator.Show();

        }
    }
}
