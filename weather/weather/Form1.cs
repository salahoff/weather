using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace weather
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public class Info //В этом классе прописываем нужные нам переменные с соответствующими типами данных
        {
            public float Temp { get; set; } //Температура
            public string Description { get; set; } //Описание
            public float Speed { get; set; } //Скорость ветра     
        }

        public class WeatherResponse //В этот класс будем записываться ответ c сервера
        {
            public Info Main { get; set; }
            public Info[] Weather { get; set; }
            public Info Wind { get; set; }               
        }

        private void button1_Click(object sender, EventArgs e)//Обрабатываем нажатие на кнопку "Показать" 
        {

            string city = NameBox.Text; //Записываем в переменную название города, полученную из поля ввода
            string response; //Переменная для чтения файла

            if (NameBox.Text != "") //Проверяем поле ввода на пустое значение
            {

                string url = "http://api.openweathermap.org/data/2.5/weather?q=" + city + "&units=metric&appid=c9d9e13aa3174bb31933dbb96d7bb525"; // Создаем запрос {ссылка + название города + ключ}
                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url); //Посылаем запрос на сервер
                HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse(); //Получаем ответ от сервера {файл формата JSON}


                using (StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream()) )//Созадаем поток для чтения файла
                {
                    response = streamReader.ReadToEnd(); //Читаем файл
                }

                WeatherResponse weatherResponse = JsonConvert.DeserializeObject<WeatherResponse>(response); //Записываем в переменную данные из прочитанного файла


                TemperatureBox.Text = weatherResponse.Main.Temp.ToString() + " °C"; //Выводим температуру
                DescriptionBox.Text = weatherResponse.Weather[0].Description.ToString().ToUpper(); //Выводим описание
                SpeedBox.Text = weatherResponse.Wind.Speed.ToString() + " m/s"; //Выводим скорость ветра

            }

                else MessageBox.Show("Введите название города!", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error); //Выводим сообщение об ошибке
        }
    }
}
