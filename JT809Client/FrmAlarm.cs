using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NewLife.JT809;
using NewLife.JT809.Models;
using NewLife.Log;

namespace JT809Client
{
    public partial class FrmAlarm : Form
    {
        public JTClient Client { get; set; }

        public ILog Log { get; set; }

        public FrmAlarm()
        {
            InitializeComponent();
        }

        private void FrmExchange_Load(Object sender, EventArgs e)
        {
            var dic = Enum.GetValues<JT809VehicleColorType>();
            cbColor.DataSource = dic;
        }

        private void btnJ1201_Click(Object sender, EventArgs e)
        {

        }

        private void btnLocation_Click(Object sender, EventArgs e)
        {

        }

        private void btnLocation2_Click(Object sender, EventArgs e)
        {

        }

        private void btnMonitorStart_Click(Object sender, EventArgs e)
        {

        }

        private void btnMonitorEnd_Click(Object sender, EventArgs e)
        {

        }

        private void btnHistory_Click(Object sender, EventArgs e)
        {

        }

        private void btnDriver_Click(Object sender, EventArgs e)
        {

        }

        private void btnWaybill_Click(Object sender, EventArgs e)
        {

        }

        private void btnDriveLine_Click(Object sender, EventArgs e)
        {

        }
    }
}
