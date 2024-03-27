using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Converters;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Storage;
using Contratos.MAUI.Models;
using System.Globalization;

namespace Contratos.MAUI {
    public partial class MainPage : ContentPage {
        string _mainDir = FileSystem.Current.AppDataDirectory;
        string _cacheDir = FileSystem.Current.CacheDirectory;
        string myDir = "/storage/emulated/0/Documents";

        public MainPage() {
            InitializeComponent();
        }

        private void GenerateContract(Contract_Tenant tenantData) {
            string directory = myDir + "/Contratos/";
            Directory.CreateDirectory(directory);
            string outputDoc = directory + tenantData.GetContractFileName()+".docx";
            string templatePath = FilePathContract.Text;
            var value = tenantData.GetTenantData();
            MiniSoftware.MiniWord.SaveAsByTemplate(outputDoc, templatePath, value);
            //convert to pdf
            string outputPDF = directory + tenantData.GetContractFileName() + ".pdf";
            
        }
        private void GenerateLetter(Contract_Tenant tenantData) {
            string directory = myDir + "/Cartas/";
            Directory.CreateDirectory(directory);
            string outputDoc = directory + tenantData.GetLetterFileName();
            string templatePath = FilePathLetter.Text;
            var value = tenantData.GetTenantData();
            MiniSoftware.MiniWord.SaveAsByTemplate(outputDoc, templatePath, value);
            //convert to pdf
            string outputPDF = directory + tenantData.GetLetterFileName() + ".pdf";
        }
        private void OnSubmitClicked(object sender, EventArgs e) {
            if (FileNameContract.Text == null) return;
            if (FileNameLetter.Text == null) return;
            FormatData();
            Contract_Tenant tenant = new(idContrato.Text, flat.Text, complex.Text, utility.Text,garage.Text, address.Text, price.Text, insurance.Text,duration.Text, startDate.Date.ToString("D",CultureInfo.CreateSpecificCulture("es-MX")),
                        endDate.Date.ToString("D", CultureInfo.CreateSpecificCulture("es-MX")),tenantName.Text,tenantId.Text,tenantPhone.Text,tenantEmail.Text,
                        codeptorName.Text,codeptorId.Text,codeptorPhone.Text,codeptorEmail.Text, startDate.Date.Day);
            GenerateContract(tenant);
            GenerateLetter(tenant);
            ResultLabel.Text = "done";
        }

        private void FormatData() {
            int value = 0;
            if (int.TryParse(price.Text, out value)) {
                price.Text = $"${value:n0}";
            }
            if (int.TryParse(insurance.Text, out int deposit)) {
                if ((deposit % value) == 0) insurance.Text = $"({deposit / value} cánones) ${deposit:n0}";
            } else insurance.Text = $"N/A ({insurance.Text})";
            if (duration.Text == null || duration.Text == "") { duration.Text = "12 MESES"; }
            if (endDate.Date == DateTime.Today) { 
                endDate.Date = startDate.Date.AddYears(1);
                endDate.Date = endDate.Date.AddDays(-1);
            }
        }

        private async void PickFileClickedContract(object sender, EventArgs e) {
            var customFileTypes = new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>
            {
                {
                DevicePlatform.iOS, new[]{
                    "com.microsoft.word.doc",
                    "public.plain-text",
                    "org.openxmlformats.wordprocessingml.document"
                }
            },
            {
                DevicePlatform.Android, new[]{
                    "application/msword",
                    "text-plain",
                    "application/vnd.openxmlformats-officedocument.wordprocessingml.document"
                }
            },
            {
                DevicePlatform.WinUI, new[]{
                    "doc","docx", "txt"
                }
            },
        });
            FileResult result = await FilePicker.PickAsync(new PickOptions {
                PickerTitle = "Pick a File",
                FileTypes = customFileTypes
            });

            if (result == null) return;
            FilePathContract.Text = result.FullPath;
            FileNameContract.Text = result.FileName;
        }
        private async void PickFileClickedLetter(object sender, EventArgs e) {
            var customFileTypes = new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>
            {
                {
                DevicePlatform.iOS, new[]{
                    "com.microsoft.word.doc",
                    "public.plain-text",
                    "org.openxmlformats.wordprocessingml.document"
                }
            },
            {
                DevicePlatform.Android, new[]{
                    "application/msword",
                    "text-plain",
                    "application/vnd.openxmlformats-officedocument.wordprocessingml.document"
                }
            },
            {
                DevicePlatform.WinUI, new[]{
                    "doc","docx", "txt"
                }
            },
        });
            FileResult result = await FilePicker.PickAsync(new PickOptions {
                PickerTitle = "Pick a File",
                FileTypes = customFileTypes
            });

            if (result == null) return;
            FilePathLetter.Text = result.FullPath;
            FileNameLetter.Text = result.FileName;
        }
    }
}