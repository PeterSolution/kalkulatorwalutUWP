using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;/*
using Windows.Data.Xml.Dom;*/
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.Net.Http;
using System.Threading.Tasks;
using System.Diagnostics;


namespace waluty
{
    public sealed partial class MainPage : Page
    {
        
        float valueofmoney;
        float moneyconvertvalue;
        string s1, s2;
        float v1, v2;
        List<string> money=new List<string>();

        List<string> value = new List<string>();


        const string link = "https://static.nbp.pl/dane/kursy/xml/a049z240308.xml";
        public MainPage()
        {
            this.InitializeComponent();


           
        }
        string kodzwartoscia="";
        private async void Grid_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            HttpClient client = new HttpClient();

            try
            {
                string xmlData = await client.GetStringAsync(new Uri(link));

                using (XmlReader reader = XmlReader.Create(new StringReader(xmlData)))
                {
                    while (reader.Read())
                    {
                        

                        reader.ReadToFollowing("kod_waluty");
                        money.Add(reader.ReadElementContentAsString());
                        reader.ReadToFollowing("kurs_sredni");
                        value.Add(reader.ReadElementContentAsString());

                    }
                }
            }
            catch (Exception ex)
            {
            
            }
            
            for (int i = 0; i < money.Count; i++)
            {
                l1.Items.Add(money[i]+" : " + value[i]);
                l2.Items.Add(money[i] + " : " + value[i]);
            }

        }

        private void l1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (l1.SelectedIndex != -1 && l2.SelectedIndex != -1)
            {
                l3.Items.Clear();
                l3.Items.Add(s1 + " -> " + s2);
                v1 = valueofmoney;
                v2 = moneyconvertvalue;
                textbox2.Text = v2.ToString();

            }
            int txt=l1.SelectedIndex;
            v1 = valueofmoney;
            valueofmoney = float.Parse(value[txt]);
            s1 = l1.SelectedItem.ToString();
            calculate();

        }

        private void l2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (l1.SelectedIndex!=-1&&l2.SelectedIndex!=-1&&s1!=null&&s2!=null)
            {
                l3.Items.Clear();
                l3.Items.Add(s1 + " -> " + s2);
                v1=valueofmoney;
                v2 = moneyconvertvalue;
                textbox2.Text=v2.ToString();
            }
            int txt = l2.SelectedIndex;
            v2 = moneyconvertvalue;
            moneyconvertvalue = float.Parse(value[txt]);
            s2=l2.SelectedItem.ToString();

            calculate();

        }

        private void textbox1_TextChanged(object sender, TextChangedEventArgs e)
        {
            calculate();
        }
        private void calculate()
        {
            if (moneyconvertvalue != 0 && valueofmoney != 0&&textbox1.Text.Length!=0)
            {
                try
                {
                    float amountofmoney = float.Parse(textbox1.Text);
                    float x = valueofmoney / moneyconvertvalue;
                    textbox2.Text = (x * amountofmoney).ToString();
                }
                catch {
                    int pointer=textbox1.Text.IndexOf(".");
                    int amountofmoney = int.Parse(textbox1.Text.Substring(0,pointer));
                    float x = valueofmoney / moneyconvertvalue;
                    textbox2.Text = (x * amountofmoney).ToString();

                }
            }
        }

        private void l3_Tapped_1(object sender, TappedRoutedEventArgs e)
        {
            valueofmoney = v1;
            moneyconvertvalue = v2;
            calculate();


        }


    }
}
