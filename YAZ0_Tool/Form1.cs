using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using YAZ0Library;
using YAZ0Library.Format;

namespace YAZ0_Tool
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            comboBox1.Items.Add("BigEndian");
            comboBox1.Items.Add("LittleEndian");
            comboBox1.SelectedIndex = 0;
        }

        public EndianConvert.Endian Endian { get; set; }
        public int CompressLevelValue { get; set; } = 0;
        //public int CompressLevelValue => int.Parse(YAZ0_CompressLevel_TXT.Text);
        public int SearchRangeValue { get; set; } = 0x400;
        //public int SearchRangeValue => Convert.ToInt32(YAZ0_SearchRangeValue_TXT.Text.Replace("0x", ""), 16);

        private void DecompYAZ0_Click(object sender, EventArgs e)
        {
            OpenFileDialog Open_YAZ0 = new OpenFileDialog()
            {
                Title = "Decompress YAZ0",
                InitialDirectory = Environment.CurrentDirectory,
                Filter = "All file|*.*"
            };
            if (Open_YAZ0.ShowDialog() == DialogResult.OK)
            {
                FileStream fs = new FileStream(Open_YAZ0.FileName, FileMode.Open, FileAccess.Read);
                BinaryReader br = new BinaryReader(fs);

                YAZ0 yaz0 = new YAZ0();
                yaz0.ReadYAZ0(br, Endian);

                br.Close();
                fs.Close();

                byte[] data = Converter.Decompress(yaz0);

                FileStream fs2 = new FileStream(Open_YAZ0.FileName + ".DecompYAZ0", FileMode.Create, FileAccess.Write);
                BinaryWriter bw = new BinaryWriter(fs2);

                bw.Write(data);

                bw.Close();
                fs2.Close();
            }
            else return;
        }

        private void CompYAZ0_Click(object sender, EventArgs e)
        {
            OpenFileDialog Open_Data = new OpenFileDialog()
            {
                Title = "Compress YAZ0",
                InitialDirectory = Environment.CurrentDirectory,
                Filter = "All file|*.*"
            };
            if (Open_Data.ShowDialog() == DialogResult.OK)
            {
                FileStream fs = new FileStream(Open_Data.FileName, FileMode.Open, FileAccess.Read);
                BinaryReader br = new BinaryReader(fs);

                byte[] Data = new byte[br.BaseStream.Length];
                for(int i = 0; i < br.BaseStream.Length; i++)
                {
                    Data[i] = br.ReadByte();
                }

                br.Close();
                fs.Close();

                FileStream fs2 = new FileStream(Open_Data.FileName + "_" + SearchRangeValue.ToString() + "_" + CompressLevelValue.ToString() + "_" + ".CompYAZ0", FileMode.Create, FileAccess.Write);
                BinaryWriter bw = new BinaryWriter(fs2);

                YAZ0 yaz0 = new YAZ0((uint)Data.Length, Data, YAZ0.DataTarget.NoCompressData, SearchRangeValue, CompressLevelValue);
                yaz0.WriteYAZ0(bw, Endian); //TODO : Enable compression with Big Endian

                bw.Close();
                fs2.Close();
            }
            else return;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Endian = (EndianConvert.Endian)Enum.Parse(typeof(EndianConvert.Endian), comboBox1.Items[comboBox1.SelectedIndex].ToString());
        }

        private void YAZ0_CompressLevelTXT_Leave(object sender, EventArgs e)
        {
            if (YAZ0_CompressLevel_TXT.Text == "") YAZ0_CompressLevel_TXT.Text = "0";
            else
            {
                int value;
                bool b = int.TryParse(YAZ0_CompressLevel_TXT.Text, System.Globalization.NumberStyles.Integer, CultureInfo.CurrentCulture, out value);
                if (b)
                {
                    if ((value >= 0 && value <= 9) == true) CompressLevelValue = value;
                    else
                    {
                        CompressLevelValue = 0;
                        YAZ0_CompressLevel_TXT.Text = "0";
                        MessageBox.Show("Error : 0, 1 - 9");
                    }
                }
                else
                {
                    CompressLevelValue = 0;
                    YAZ0_CompressLevel_TXT.Text = "0";
                    MessageBox.Show("Error : Please enter a valid compress level number. [0, 1 - 9]");
                }
            }
        }

        private void YAZ0_SearchRangeValue_TXT_Leave(object sender, EventArgs e)
        {
            if (YAZ0_SearchRangeValue_TXT.Text == "") YAZ0_SearchRangeValue_TXT.Text = "0x400";
            else
            {
                if (YAZ0_SearchRangeValue_TXT.Text.StartsWith("0x"))
                {
                    int res;
                    bool b = int.TryParse(YAZ0_SearchRangeValue_TXT.Text.Replace("0x", ""), System.Globalization.NumberStyles.HexNumber, CultureInfo.CurrentCulture, out res);
                    if (b)
                    {
                        SearchRangeValue = res;
                    }
                    else
                    {
                        SearchRangeValue = 0x400;
                        YAZ0_SearchRangeValue_TXT.Text = "0x400";
                        MessageBox.Show("Error : Please enter a valid hexadecimal number.");
                    }
                }
                else
                {
                    int res;
                    bool b = int.TryParse(YAZ0_SearchRangeValue_TXT.Text, System.Globalization.NumberStyles.HexNumber, CultureInfo.CurrentCulture, out res);
                    if (b)
                    {
                        YAZ0_SearchRangeValue_TXT.Text = "0x" + YAZ0_SearchRangeValue_TXT.Text;
                        SearchRangeValue = res;
                    }
                    else
                    {
                        SearchRangeValue = 0x400;
                        YAZ0_SearchRangeValue_TXT.Text = "0x400";
                        MessageBox.Show("Error : Please enter a valid hexadecimal number.");
                    }
                }
            }
        }
    }
}
