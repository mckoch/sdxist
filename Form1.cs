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
using System.Resources;
using System.Xml;
using System.Runtime.Serialization;
using System.Diagnostics;

namespace STA_Viewer_0._1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public string STADIR = "none";

        public string IMGDIR;

        public string CURRENTFILE;

        public string SDAWTYPE = "generic";

        public string[] VALIDTYPES = new string[] {"STA","FRE", "VMS"};

        public string lat;
        public string lng;
        public string maplink;
        public string imageprefix;

        
        public string splitSdawString(string sdawstring, string sdawfiletype)
            // exempl. usage sx_dcode()
        {   string[] validFilters = new string[] {"KSTA","KFRE","KVMS"};
            foreach (string matchfilter in validFilters) {
                if (sdawfiletype == matchfilter) { 
                    sdx_decode(sdawstring.Substring(5, sdawstring.Length-5), sdawfiletype);
                    // return as string.....
                }
            } return sdawstring; // for now...
        }

        public string splitSdawHeaderString(string sdawheader)
        {
            string fldtxt ="Kopfdaten: \n";
            fldtxt += "Dateiart: "+sdawheader.Substring(1,3) + "\n";
            fldtxt += "Geschäftsjahr: " + sdawheader.Substring(4, 4) + "\n";
            // fldtxt += "Log ID: " + sdawheader.Substring(8,19) + "\n";
            fldtxt += "Senderidentifikation: " + sdawheader.Substring(8,3) + "\n";
            fldtxt += "Versionsnummer: " + sdawheader.Substring(11,3) + "\n";
            fldtxt += "Identnummer: " + sdawheader.Substring(14,13) + "\n";
            if (sdawheader.Length >= 295)
            {
                fldtxt += "Auftragsnummer Mittler: " + sdawheader.Substring(27,16) + "\n";
                fldtxt += "Auftragsnummer Pächter: " + sdawheader.Substring(43,16) + "\n";
                // split
                fldtxt += "Dateierstellungsdatum" + sdawheader.Substring(59,8) + "\n";
                // split
                fldtxt += "A/M/K: " + sdawheader.Substring(73,1) + "\n";
                // split?
                fldtxt += "Pächter: " + sdawheader.Substring(74,40) + "\n";
                fldtxt += "Agentur: " + sdawheader.Substring(114,40) + "\n";
                fldtxt += "Spezialmittler: " + sdawheader.Substring(154,40) + "\n";
                fldtxt += "Kunde: " + sdawheader.Substring(194,40) + "\n";
                fldtxt += "Sachbearbeiter des Pächters: " + sdawheader.Substring(234,20) + "\n";
                fldtxt += "Telefonnumer: " + sdawheader.Substring(254,20) + "\n";
                fldtxt += "Telefax: " + sdawheader.Substring(274,20) + "\n";
            }
            return fldtxt;
        }

        public Dictionary<string, string> sdx_decode(string ostr, string sdx_encoding_type)
        {
            Dictionary<string, string> decdict = new Dictionary<string, string>();
            try
            {
                sdx_encoding_type = sdx_encoding_type.Substring(1, 3);
                var xmlfile = Properties.Resources.ResourceManager.GetObject(sdx_encoding_type);
                XmlDocument xml = new XmlDocument();
                xml.LoadXml(xmlfile.ToString());
                XmlNodeList sdawfieldset = xml.GetElementsByTagName("field");
                foreach (XmlNode sdawfield in sdawfieldset)
                {
                    string startpos = sdawfield["startpos"].InnerText;
                    string length = sdawfield["length"].InnerText;
                    string title = sdawfield["title"].InnerText;
                    string data = ostr.Substring(XmlConvert.ToInt16(startpos) - 1, XmlConvert.ToInt16(length));
                    decdict.Add(title, data);
                } return decdict;
            } catch (Exception ex) { decdict.Add("sdx_exit_code","Nicht unterstützt."); return decdict;}
        }

        
        private void folderBrowserDialog1_HelpRequest(object sender, EventArgs e)
        {
            // DetailsAsText.Text += DialogResult.ToString() + "\n";
        }

        private void StaAuswahlButton_Click(object sender, EventArgs e)
        {
            this.labelStaVerzeichnis.Text = "Verzeichnis für STA Dateien auswählen...\n";
            DialogResult result = this.folderBrowserDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                STADIR = folderBrowserDialog1.SelectedPath;
                webBrowser_MapImage.Visible = false;
                if(!this.checkBoxStaDateienMerken.Checked){this.listBox1.Items.Clear();}
                //string foldername = STADIR;
               labelStaVerzeichnis.Text = STADIR;
               foreach (string f in System.IO.Directory.GetFiles(STADIR))
               {
                       this.listBox1.Items.Add(f);
               }
               this.Text = "STA Viewer - " + STADIR + " ("+ listBox1.Items.Count.ToString() + " Dateien)";
               listBox3.Items.Add("Arbeitsverzeichnis: " + STADIR + "; temporäres Verzeichnis: " +  System.IO.Path.GetTempPath());
            }
        }

        private void ImgAuswahlButton_Click(object sender, EventArgs e)
        {
            DialogResult result = this.folderBrowserDialog2.ShowDialog();
            if (result == DialogResult.OK)
            {
                IMGDIR = folderBrowserDialog2.SelectedPath;
                label2.Text = IMGDIR;
                listBox3.Items.Add("Bilderverzeichnis: " + IMGDIR);
            }
        }

        private void folderBrowserDialog2_HelpRequest(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {
            try { this.Text = Application.UserAppDataRegistry.GetValue("sta_init_dir").ToString(); }
            catch (Exception ex) { Console.WriteLine("{0} Exception caught.", ex); }
        } 

        private void comboBox1_SelectionChangeCommitted(object sender, EventArgs e)
        {
            string txt = comboBox_MapProvider.SelectedItem.ToString();
            if (txt == "Eigener Map URL")
            {
                textBox_ApiKey.Text = "Map-URL. %lat% und %lng% als Platzhalter.";
            }
            else
            {
                textBox_ApiKey.Text = "Ihr API Schlüssel für " + txt + ".";
            }
            textBox_ApiKey.SelectAll();
        }

        private void listBox1_SelectedValueChanged(object sender, EventArgs e)
        {
            int counter = 0;
            string line;
            webBrowser_MapImage.Visible = false;
            pictureBox_Standortfoto.BackgroundImage = pictureBox_Standortfoto.InitialImage;
            // string sdawtype = "init";
            try
            {
                CURRENTFILE = listBox1.SelectedItem.ToString();
                SDAWTYPE = "unknown";
            }
            catch (Exception ex) { return; }
            listBox2.Items.Clear(); 

            if (System.IO.Path.GetExtension(CURRENTFILE) != ".rar"
                & System.IO.Path.GetExtension(CURRENTFILE) != ".zip"
                & System.IO.Path.GetExtension(CURRENTFILE) != ".7z")
            {
                try
                {
                    
                    System.IO.StreamReader file = new System.IO.StreamReader(CURRENTFILE);
                    while ((line = file.ReadLine()) != null)
                    {
                        if (counter == 0)
                        {
                            richTextBox1.Text = splitSdawHeaderString(line);
                            SDAWTYPE = line.Substring(0, 4);
                        }
                        if (line.Substring(0, 1) == "D")
                        {
                            if (SDAWTYPE == "KSTA") listBox2.Items.Add(line);
                            if (SDAWTYPE == "KFRE") listBox2.Items.Add(line);
                            if (SDAWTYPE == "KVMS") listBox2.Items.Add(line);
                        }
                        counter++; if (counter > 5000) { 
                            listBox3.Items.Add("Limit überschritten. Bitte erwerben Sie eine Lizenz.");
                            richTextBox1.Text += "\n -------------- \n Limit überschritten. Bitte erwerben Sie eine Lizenz. \n -------------- \n"; break; 
                        }
                    }
                    file.Close();
                    // if (sdawtype != "KSTA") richTextBox1.Text += line + "\n -------------- \n";
                }
                catch (Exception ex) { listBox3.Items.Add("{0} Konnte Datei nicht öffnen."); richTextBox1.Text = " \n Nicht gefunden oder ins Arbeitsverzeichnis verschoben. "; }
                
            }
            else
            {
                SDAWTYPE = "archive";
                string txt = System.IO.Path.GetExtension(CURRENTFILE.ToLower());
                richTextBox1.Text = txt.ToUpper() + " ARCHIV \n -------------- \n Dateien: \n \n";
                if (txt == ".zip"  | txt == ".rar")
                {
                    using (System.IO.Stream stream = System.IO.File.OpenRead(@listBox1.SelectedItem.ToString()))
                    {
                        var reader =  SharpCompress.Reader.ReaderFactory.Open(stream);
                        while (reader.MoveToNextEntry())
                        {
                            if (!reader.Entry.IsDirectory)
                            {
                                try
                                {
                                    richTextBox1.Text += reader.Entry.FilePath + "\n -------------- \n";
                                    string tempPath = System.IO.Path.GetTempPath();
                                    string zpath = tempPath + reader.Entry.FilePath;
                                    var zfile = System.IO.File.OpenWrite(zpath);
                                    reader.WriteEntryTo(zfile); zfile.Close();
                                    
                                    listBox3.Items.Add(zpath + " extrahiert aus " + CURRENTFILE);
                                    int idx = listBox1.Items.IndexOf(listBox1.SelectedItem);
                                    if (checkBox4MoveUnzipped.Checked)
                                    {
                                        string newfile = STADIR + "\\" + reader.Entry.FilePath;
                                        if (checkBoxImArbeitsverzeichnisUeberschreiben.Checked 
                                            & checkBox4MoveUnzipped.Checked) {System.IO.File.Delete(newfile);}
                                        if (!System.IO.File.Exists(newfile))
                                        {
                                            System.IO.File.Move(zpath, newfile);
                                            listBox1.Items.Insert(idx, newfile);
                                            richTextBox1.Text += "\n Neu: " + newfile + " \n -------------- \n";
                                            listBox3.Items.Add(zpath + " Neue Datei aus Archiv: " + newfile);    
                                        }
                                        else
                                        {
                                            listBox3.Items.Add("Datei bereits vorhanden: " + newfile);
                                            richTextBox1.Text += "Datei bereits vorhanden: " + newfile;
                                        }

                                    }
                                    else 
                                    { 
                                        listBox1.Items.Insert(idx, zpath);
                                        richTextBox1.Text += "\n extrahiert nach " + zpath;
                                    }
                                    
                                }
                                catch (Exception ex) {listBox3.Items.Add(ex.ToString()); }
                            }
                        }
                        listBox1.Refresh();
                        stream.Close();
                    }
                }
            }
            
            try { this.Text = listBox1.SelectedItem.ToString() + " (" + counter + " Datensätze)";}
            catch (Exception ex) { Console.WriteLine("{0} Exception caught.", ex); }
        }

        private void listBox2_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            try
            {
                listBox3.Items.Add(SDAWTYPE + "#" + listBox2.SelectedItem.ToString());
            }
            catch (Exception ex) { Console.WriteLine("{0} Exception caught.", ex); }
        }


        private void listBox3_MouseClick(object sender, MouseEventArgs e)
        {
            try { richTextBox1.Text = splitSdawString(listBox3.SelectedItem.ToString(), listBox3.SelectedItem.ToString().Substring(0,4)); } 
            catch (Exception ex) { Console.WriteLine("{0} Exception caught.", ex);}
        }

        private void listBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            try
            {
                listBox3.Items.Add(listBox1.SelectedItem.ToString());
            }
            catch (Exception ex) { Console.WriteLine("{0} Exception caught.", ex); }
        }

        private void listBox2_SelectedValueChanged(object sender, EventArgs e)

        {
            try
            {
                richTextBox1.Text = ""; webBrowser_MapImage.Visible = false;
                pictureBox_Standortfoto.BackgroundImage = pictureBox_Standortfoto.InitialImage;
                foreach (var t in sdx_decode(listBox2.SelectedItem.ToString(), SDAWTYPE.ToString()))
                {
                    richTextBox1.Text += t.Key + " :: " + t.Value + "\n";
                    if (t.Key == "latitude") lat = t.Value.TrimStart('0');
                    if (t.Key == "longitude") lng = t.Value.TrimStart('0');
                    if (t.Key == "Fotoname") imageprefix = t.Value;
                }
                 maplink = "http://maps.googleapis.com/maps/api/staticmap?center="+lat+","+lng+ "&zoom=15&size=140x140&sensor=false";
                 if (checkBox_MapEinblenden.Checked & SDAWTYPE == "KSTA") { 
                     webBrowser_MapImage.Visible = true;
                     webBrowser_MapImage.Navigate(maplink);
                     richTextBox1.Text += "\n" + maplink + "\n"; ;
                 }
                 else { webBrowser_MapImage.Visible = false;}
                
            }
            catch (Exception ex) { webBrowser_MapImage.Visible = false; return; }

            try
            {
                string imgpath = IMGDIR + "\\" + imageprefix.Trim() + ".jpg";
                if (System.IO.File.Exists(imgpath) == true & SDAWTYPE == "KSTA")
                {
                    pictureBox_Standortfoto.BackgroundImage = new Bitmap(imgpath);
                }
                else
                {
                    pictureBox_Standortfoto.BackgroundImage = pictureBox_Standortfoto.InitialImage;
                }
            }
            catch { pictureBox_Standortfoto.BackgroundImage = pictureBox_Standortfoto.InitialImage; return; }
        }

        private void listBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                listBox3.Items.Add(listBox2.SelectedItem.ToString());
            }
            catch (Exception ex) { Console.WriteLine("{0} Exception caught.", ex); }
        }

        private void listBox3_SelectedValueChanged(object sender, EventArgs e)
        {
            try { richTextBox1.Text = listBox3.SelectedItem.ToString(); }
            catch (Exception ex) { Console.WriteLine("{0} Exception caught.", ex);
            }
        }

        private void richTextBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            try { listBox3.Items.Add(richTextBox1.Text); }
            catch (Exception ex)
            {Console.WriteLine("{0} Exception caught.", ex);}
        }

 
        private void listBox3_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                listBox3.Items.Remove(listBox3.SelectedItem.ToString());
            }
            catch (Exception ex) { Console.WriteLine("{0} Exception caught.", ex); }
        }

       
        private void pictureBox1_DoubleClick(object sender, EventArgs e)
        {
        
        }

        private void checkBox3_Click(object sender, EventArgs e)
        {
            checkBox3.Text = "Das geht nur mit einem Upgrade.";
            listBox3.Items.Add("Upgrades und Informationen direkt vom Entwickler: 0202-2692665.");
        }

        private void linkLabelAblageLoeschen_DoubleClick(object sender, EventArgs e)
        {
            listBox3.Items.Clear();
        }

        private void linkLabel2_DoubleClick(object sender, EventArgs e)
        {
            string clp = "/** Clipboard Export: Zeilen **/";
            foreach (string s in listBox3.Items)
            { clp += s + "\n";}
            Clipboard.SetText(clp, TextDataFormat.UnicodeText);
            MessageBox.Show(clp);
        }

        private void linkLabel5_DoubleClick(object sender, EventArgs e)
        {
            // INSERT INTO STA (P_Id, LastName, FirstName) VALUES (5, 'Tjessem', 'Jakob')

            string clp = "/** Clipboard Export: SQL **/";
            string sqlopen = "INSERT INTO __TABLE__ (";
            string sqlvalues = ") VALUES ";
            string sqlclose = ";";
            string sqlrowopen = "(";
            string sqlrowclose = ")";
            string fieldsep = "'";
            string datasep = ",";
            string fieldset = "";
            string valueset = "";
            clp += sqlopen; 
            int i = listBox3.Items.Count;
            foreach (string s in listBox3.Items) 
            {
                i++;
                var sd = sdx_decode(s.ToString(), SDAWTYPE.ToString());
                foreach (var subset in sd){
                    if (i == 1) fieldset += subset.Key + datasep;
                    valueset += subset.Value + fieldsep;
                }
                ////
                /**

                sdx_decode returns collection....

                **/
                ////
                clp += sqlrowopen + fieldset + sqlvalues + fieldsep + valueset + fieldsep + sqlrowclose + datasep; 
            }
            clp += fieldset + sqlvalues + sqlclose;
            clp += sqlclose;
            Clipboard.SetText(clp, TextDataFormat.UnicodeText);
            string message = "Die Daten wurden erfolgreich in die Zwischenablage kopiert.";
            string caption = "Clipboard Export";
            MessageBoxButtons buttons = MessageBoxButtons.OK;
            DialogResult result;
            result = MessageBox.Show(message, caption, buttons);
        }


        private void linkLabel4_DoubleClick(object sender, EventArgs e)
        {
            int idx = -1;
            int cnt = listBox3.Items.Count;
            int[] idxlist = new int[cnt];
            foreach (string s in listBox3.Items)
            { 
                idx = idx + 1;                
                try
                {
                    if (!string.IsNullOrEmpty(s))
                    {if (s.Substring(0, 1) != "K" & s.Substring(4, 2) != "#D"){idxlist[idx] = idx;}}
                    else {idxlist[idx] = idx; }
                }
                catch (Exception ex) {idxlist[idx] = idx; }
            } 
            foreach (int i in idxlist.Reverse()){if (i > 0) listBox3.Items.RemoveAt(i);}}

        private void checkBoxImArbeitsverzeichnisUeberschreiben_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxImArbeitsverzeichnisUeberschreiben.Checked & checkBox4MoveUnzipped.Checked) 
                MessageBox.Show("Vorsicht! Bereits vorhandene Dateien im aktuellen Arbeitsverzeichnis "+ STADIR +" werden nach dem Entpacken ohne weitere Rückfrage überchrieben!","Vorsicht!");
        }

        private void checkBox4MoveUnzipped_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox4MoveUnzipped.Checked & STADIR!="none") { checkBoxImArbeitsverzeichnisUeberschreiben.Enabled = true; }
            else { checkBoxImArbeitsverzeichnisUeberschreiben.Enabled = false; }
        }

        private void comboBox_MapProvider_DropDownClosed(object sender, EventArgs e)
        {
            try
            {
                if (comboBox_MapProvider.SelectedItem.ToString() == "Eigener URL") { textBox_ApiKey.Enabled = true; }
                else { textBox_ApiKey.Enabled = false; }
            }
            catch  { return; }
        }

        private void checkBox_MapEinblenden_Click(object sender, EventArgs e)
        {
            if (checkBox_MapEinblenden.Checked) { webBrowser_MapImage.Visible = true; } else { webBrowser_MapImage.Visible = false; }
        }

        private void richTextBox1_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            try
            {
                Process.Start(richTextBox1.SelectedText);
            }
            catch { return; }
        }

        private void linkLabel1_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                if (maplink != "") Process.Start(maplink);
            }
            catch { return; }
        }

        private void textBox1_Click(object sender, EventArgs e)
        {
            DialogResult result = this.openFileDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                textBox_StaDateiHinterlegen.Text = openFileDialog1.FileName.ToString();

            }
        }

        private void pictureBox_license_image_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Process.Start("https://creativecommons.org/licenses/by-nd/3.0/deed.de");
        }
    }
}
