using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.NetworkInformation; // Añadimos esta libreria que viene por defecto para extrar los valores de adaptadores que existen en windows.
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Scanner_MAC_WIFI
{
    public partial class Wifi_Scanner : Form
    {
        public Wifi_Scanner()
        {
            InitializeComponent();
        }

        private void btnScan_Click(object sender, EventArgs e)
        {
            listBoxResults.Items.Clear();
            //mandamos el resultado de GetAllNetworkInterfaces a la variable networkInterfaces.
            var networkInterfaces = NetworkInterface.GetAllNetworkInterfaces();
            string macToCopy = null; // Declaramos una variable para copiar la mac en automatico al portapapeles, la inicializamos en null.



            //Usamos Foreach para recorrer cada elemento del resultado y filtrar los que no necesitamos.
            foreach (var adapter in networkInterfaces)
            {
                // Filtrar solo adaptadores Wi-Fi
                if (adapter.NetworkInterfaceType != NetworkInterfaceType.Wireless80211)
                {
                    continue; //Usamos continue para evitar procesos innecesarios, similar al break pero sin romper todo el ciclo, solo la iteracion en curso.
                }

                // Excluir adaptadores virtuales o tipo loopback.
                if (adapter.Description.ToLower().Contains("virtual") ||adapter.Description.ToLower().Contains("loopback"))
                {
                    continue;
                }

                // Obtener la MAC address en una variable, uniendo despues de los : puntos re resultado referenciado.
                var macAddress = string.Join(":", adapter.GetPhysicalAddress().GetAddressBytes().Select(b => b.ToString("X2")));

                // Agregar a la lista de forma escalonada para mejor presentación y simplificamos usando interpolacion de cadenas
                listBoxResults.Items.Add($"Nombre: {adapter.Description}");
                listBoxResults.Items.Add($"MAC: {macAddress}");
                listBoxResults.Items.Add("-------------------------------------------------------");

                // Guardar la primera MAC encontrada para copiar al portapapeles
                if (macToCopy == null)
                {
                    macToCopy = macAddress;
                }

            }

            if (listBoxResults.Items.Count == 0)
            {
                listBoxResults.Items.Add("No se encontraron adaptadores Wi-Fi.");
            }

            if (macToCopy != null)
            {
                // Copiar la primera MAC al portapapeles
                Clipboard.SetText(macToCopy);

                // Mostrar un mensaje de confirmación (opcional)
                MessageBox.Show($"MAC copiada al portapapeles: {macToCopy}", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Desarrolado por Cesar E. Flores \n https://github.com/TeslaCesar ", "GRACIAS!!!", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.Close();
        }
    }
}
