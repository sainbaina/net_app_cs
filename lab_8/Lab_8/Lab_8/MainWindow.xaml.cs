using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;
using Lab_8.CalculatorService;
using Lab_8.MyCalculatorService;
using System.Data;

namespace Lab_8
{
    public partial class MainWindow : Window
    {
        public string num1 = "";
        public string num2 = "";
        public string operation = "";

        public int currNum = 1;
        DataTable dt = new DataTable();

        BasicHttpBinding binding;
        EndpointAddress address;
        CalculatorService.CalculatorSoapClient csc;

        BasicHttpBinding myBinding;
        EndpointAddress myAddress;
        CalculatorServiceSoapClient myCsc;

        public MainWindow()
        {
            InitializeComponent();

            binding = new BasicHttpBinding();
            binding.MaxReceivedMessageSize = 20000000;

            address = new EndpointAddress("http://www.dneonline.com/calculator.asmx");
            
            csc = new CalculatorSoapClient(binding, address);


            myBinding = new BasicHttpBinding();
            myBinding.MaxReceivedMessageSize = 20000000;

            myAddress = new EndpointAddress("http://localhost:52484/WebService1.asmx");

            myCsc = new CalculatorServiceSoapClient(myBinding, myAddress);
        }

        private void AddNumber(int number)
        {
            switch(currNum)
            {
                case 1:
                    if (num1 != "0")
                        num1 += number.ToString();
                    else return;
                    break;
                case 2:
                    if (num2 != "0")
                        num2 += number.ToString();
                    else return;
                    break;
            }

            InputTextBox.Text += number.ToString();
        }

        private void AddOperation(string op)
        {
            if (currNum != 1) return;
            if (operation != "") return;

            currNum++;

            operation = op;

            InputTextBox.Text += op;
        }

        private void GetResult()
        {
            float res = 0.0f;
             
            try
            {
                switch (operation)
                {
                    case "+":
                        res = csc.Add(int.Parse(num1), int.Parse(num2));
                        break;
                    case "-":
                        res = csc.Subtract(int.Parse(num1), int.Parse(num2));
                        break;
                    case "*":
                        res = csc.Multiply(int.Parse(num1), int.Parse(num2));
                        break;
                    case "/":
                        res = myCsc.Divide(int.Parse(num1), int.Parse(num2));
                        //res = csc.Divide(int.Parse(num1), int.Parse(num2));
                        break;
                }
            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message);
            }

            OutputTextBox.Text = res.ToString();
        }

        private void ClearInput()
        {
            num1 = "";  
            num2 = "";
            currNum = 1;
            InputTextBox.Text = "";
            operation = "";
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            AddNumber(0);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            AddNumber(1);
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            AddNumber(2);
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            AddNumber(3);
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            AddNumber(4);
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            AddNumber(5);
        }

        private void Button_Click_6(object sender, RoutedEventArgs e)
        {
            AddNumber(6);
        }

        private void Button_Click_7(object sender, RoutedEventArgs e)
        {
            AddNumber(7);
        }

        private void Button_Click_8(object sender, RoutedEventArgs e)
        {
            AddNumber(8);
        }

        private void Button_Click_9(object sender, RoutedEventArgs e)
        {
            AddNumber(9);
        }

        private void Button_Click_10(object sender, RoutedEventArgs e)
        {
            AddOperation("+");
        }

        private void Button_Click_11(object sender, RoutedEventArgs e)
        {
            AddOperation("-");
        }

        private void Button_Click_12(object sender, RoutedEventArgs e)
        {
            AddOperation("*");
        }

        private void Button_Click_13(object sender, RoutedEventArgs e)
        {
            AddOperation("/");
        }

        private void Button_Click_14(object sender, RoutedEventArgs e)
        {
            GetResult();
        }

        private void Button_Click_15(object sender, RoutedEventArgs e)
        {
            ClearInput();
        }
    }
}
