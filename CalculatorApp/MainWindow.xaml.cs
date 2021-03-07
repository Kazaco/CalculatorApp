using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Data;
using System.Windows;
using System.Windows.Controls;

namespace CalculatorApp
{
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        //One_Click
        private string buttonPressed;   // Retrieve the string on the button pressed

        //setMathString
        private DataTable dt;                   // Class used to interpret string entered by User 
        private string mathString = "0";        // Initial String displayed to the User
        private string operandOne = "";              // Registers
        private string operandTwo;   
        private string op = "";                //Last operator used
        private string savedOperand = "";      //Saved operand
        private bool enteredOperand = true;    //Flag that user has just entered an operand (true b/c 0)
        private bool isPositive = true;        //Handle Negation
        private bool endOfCalculation = false; //Handle new calculation or using previous operand data
        private bool UseOperandEquals = false; //User cannot use operandsEquals untill they have done 1 calculation

        //Calculator Main Screen
        public string MathCalculation
        {
            get
            {
                return mathString;
            }
        }

        //Calculator Sub-Screen
        public string PrevCalculation
        {
            get
            {
                return operandOne;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;

            //Used to check/compute for valid strings expressions
            dt = new DataTable();
        }

        private void One_Click(object sender, RoutedEventArgs e)
        {
            //Retrieve the string on the button
            buttonPressed = (sender as Button).Content.ToString();

            //Use this value to create some visual output on calculator
            setMathCalculation(buttonPressed);

            //Update the GUI
            NotifyPropertyChanged("PrevCalculation");
            NotifyPropertyChanged("MathCalculation");
        }

        private void setMathCalculation(string character)
        {
            //User uses a binary operations button
            if (enteredOperand == true && (character == "+" || character == "-" || character == "*" || character == "/") )
            {
                //Convert the current mathString into an int and store it in class
                operandOne = mathString + " " + character + " ";
                mathString = " ";
                op = character;

                //Flag that the last operation was an operator and is no longer an operand
                enteredOperand = false;
                isPositive = true;
                UseOperandEquals = false;   //We are not looping with current data
                endOfCalculation = false;   //We are continuing the previous calculation
            }
            //User uses a unary operation button
            else if(enteredOperand == true && (character == "sqrt" || character == "+/-"))
            {
                if(character == "sqrt")
                {
                    //Parse string into an integer so we can apply sqrt
                    if(double.TryParse(mathString, out double result))
                    {
                        double applySqrt = Math.Sqrt(result);
                        mathString = applySqrt.ToString();

                        //User tried to take sqrt of a negative number
                        if (mathString == "NaN")
                        {
                            //Handle Divide by Zero
                            mathString = "Sqrt of Negative";

                            //Flag variables in the calculator to be overwritten
                            operandOne = "";
                            operandTwo = "";
                            enteredOperand = false;
                            isPositive = true;
                            endOfCalculation = true;
                        }
                    }
                    else
                    {
                        //If you try to take the the sqrt of a negatation of a number twice
                        mathString = "Invalid Input";

                        //Flag variables in the calculator to be overwritten
                        operandOne = "";
                        operandTwo = "";
                        enteredOperand = false;
                        isPositive = true;
                        endOfCalculation = true;
                    }
                }
                else
                {
                    //Switch to negative number
                    if(isPositive == true)
                    {
                        //Flag as negative number and change string
                        mathString = "-" + mathString;
                        isPositive = false;
                    }
                    //Switch to positive number
                    else
                    {
                        //Flag as positive number and change string
                        mathString = mathString.Replace("-", "");
                       isPositive = true;
                    }
                }
            }
            //User uses a special button
            else if(character == "C" || character == "SAVE" || character == "LOAD" || character == ".")
            {
                if(character == "C")
                {
                    //Reset the calculator
                    mathString = "0";
                    operandOne = "";
                    operandTwo = "";
                    UseOperandEquals = false;
                    enteredOperand = true;
                    isPositive = true;
                    endOfCalculation = false;
                }
                else if(enteredOperand == true && character == "SAVE")
                {
                    //Save current written string
                    savedOperand = mathString;
                }
                else if(character == "LOAD" && savedOperand.Length > 0)
                {
                    //Load saved operand
                    mathString = savedOperand;
                    enteredOperand = true;
                }
            }
            else if(enteredOperand == true && (character == "(" || character == ")" || character == "="))
            {
                //Display result of calculation to the user
                if(character == "=")
                {
                    try
                    {
                        //User has entered new data to be calculated
                        if (endOfCalculation == false && UseOperandEquals == false && op != "")
                        {
                            //If the user has pressed the "=" for the first time
                            var result = dt.Compute(operandOne + mathString, "");
                            operandOne = operandOne + mathString;                   //Top number to show what is happening
                            operandTwo = mathString;                                //Keep track of 2nd operand in-case below V
                            mathString = result.ToString();                         //Result                          
                        }
                        else if(UseOperandEquals == true && operandTwo != "")
                        {
                            //Add the second operator value to our current final result if the user keeps pressing the "=" key
                            var result = dt.Compute(mathString + op + operandTwo, "");
                            operandOne = mathString + op + operandTwo;                      //Top number to show what is happening
                            mathString = result.ToString();                                 //Result
                        }

                        //User tried to divide by 0 (Note: Compute does not find this as an error, so we handle it here)
                        if (mathString == "NaN" || mathString == "∞" || mathString == "-∞")
                        {
                            //Handle Divide by Zero
                            mathString = "Cannot Divide Zero!";

                            //Flag variables in the calculator to be overwritten
                            operandOne = "";
                            operandTwo = "";
                            enteredOperand = false;
                            UseOperandEquals = false;
                            isPositive = true;
                            endOfCalculation = true;
                        }
                        else
                        {
                            //Good Solution:
                            //Possible end of current calculation, user can either do an operation on current number or input a new number
                            //---If user hits the "=" again, they will enter here again but use operandTwo (same with negation, works same in Microsoft Calculator)
                            //---If user hits an operator, they will use current calculation in next calculation
                            //---If user hits a number they will start a new calculation
                            endOfCalculation = true;
                            enteredOperand = true;
                            isPositive = true;
                            UseOperandEquals = true;    //User can now use operandEquals
                        }
                    }
                    catch (OverflowException)
                    {
                        //Handle Overflow exception
                        mathString = "Overflow Exception!";

                        //Flag variables in the calculator to be overwritten
                        operandOne = "";
                        operandTwo = "";
                        enteredOperand = false;
                        UseOperandEquals = false;
                        isPositive = true;
                        endOfCalculation = true;
                    }
                }
            }
            else if(character == "0" || character == "1" || character == "2" || character == "3" || character == "4" || character == "5" || character == "6" || character == "7" || character == "8" || character == "9")
            {
                //Replace the zero whenever a new digit is added, or if zero is added to 0, or if user tried to divide by 0
                if(mathString == "0" || mathString == " " || mathString == "-0" || endOfCalculation == true)
                {
                    //Positive number
                    if(isPositive)
                    {
                        //User is starting a new calculation, so we need to overwrite the current value
                        mathString = character;
                    }
                    else
                    {
                        mathString = "-" + character;
                    }
  
                    //Flag that current calculation has ended (BUT, user can still loop with new data)
                    endOfCalculation = false;
                }
                else
                {
                    //User entered a digit and the string isn't currently "0"
                    mathString += character;
                }

                //Flag that the user has entered a number
                enteredOperand = true;
            }
            else
            {
                //Do nothing, user is attempting to put an invalid operand/operator after another
            }
        }

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}