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
    public partial class DaugtherGUI : Form
    {
        private MainGUI mother;
        internal int tripId;
        public DaugtherGUI(MainGUI main)
        {
            mother = main;
            tripId = -1;
            InitializeComponent();
            labelIdValue.Text = null;
            dateTimePicker1.Format = DateTimePickerFormat.Custom;
            dateTimePicker1.CustomFormat = "MM/dd/yyyy hh:mm:ss";
            dateTimePicker2.Format = DateTimePickerFormat.Custom;
            dateTimePicker2.CustomFormat = "MM/dd/yyyy hh:mm:ss";
        }



        private async void buttonConfirm_Click(object sender, EventArgs e)
        {
            Trip trip=new Trip();
            trip.To = this.textBoxTo.Text;
            trip.From = this.textBoxFrom.Text;
            trip.Takeof = this.dateTimePicker1.Value;
            trip.Arrival = this.dateTimePicker2.Value;
            if (tripId >= 0)
            {
                trip.TripId = tripId;
                await mother.client.ModifyTripAsync(trip);
            }
             else
                await mother.client.CreateTripAsync(trip);
      

            this.Close();

        }
    }
}
