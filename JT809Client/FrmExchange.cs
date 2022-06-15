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
using NewLife.JT809.SubMessages;
using NewLife.Log;
using NewLife.Security;

namespace JT809Client
{
    public partial class FrmExchange : Form
    {
        public JTClient Client { get; set; }

        public ILog Log { get; set; }

        public FrmExchange()
        {
            InitializeComponent();
        }

        private void FrmExchange_Load(Object sender, EventArgs e)
        {
            var dic = Enum.GetValues<JT809VehicleColorType>();
            cbColor.DataSource = dic;
        }

        private async void btnRegister_Click(Object sender, EventArgs e)
        {
            var model = new JT809_0x1200_0x1201
            {
                PlateformId = Rand.NextString(11),
                ProducerId = Rand.NextString(11),
                TerminalModelType = Rand.NextString(30),
                IMIEId = Rand.NextString(15),
                TerminalId = Rand.NextString(30),
                TerminalSimCode = Rand.NextString(13),
            };

            var plate = txtPlate.Text;
            var color = cbColor.SelectedItem.ToInt();

            await Client.RegisterAsync(plate, color, model);
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
