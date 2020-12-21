using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Brickwork
{
    class Program
    {
        public static void outputArray(int[,] arr, int rowLength, int colLength)// method for outputing layouts
        {
            for (int i = 0; i < rowLength; i++)
            {
                for (int j = 0; j < colLength; j++)
                {
                    Console.Write(string.Format("{0} ", arr[i, j]));
                }
                Console.Write(Environment.NewLine + Environment.NewLine);
            }
        }

        public static void generateNewLayout(int[,] arr, int rowLenght, int colLenght)// method for generating second layout
        {
            int[,] secondLayout = new int[rowLenght, colLenght];// second layout initialisation
            int element = 1;//number of bricks that need to be placed on each 2 rows
            int counter = 0;//keeping track on the current brick
            int a = 0;// row we are currently on 
            int pos = 0;// position of the element in the array
            bool flag = false;// flag if there is any possible solution
            while (a < rowLenght && flag == false)//while there are rows to be filled out and checks flag
            {
                while (element < colLenght && flag == false)//while there are bricks that need placing ot the rows and checks flag
                {
                    if (colLenght - pos == 0) //we check if we are at the end of the row
                    {
                        pos = colLenght - 1;//we set the position to be 1 point away from the end
                    }
                    if (arr[a, pos] == arr[a + 1, pos] && pos != colLenght-1)//if first brick must be horizontal
                    {                    
                        secondLayout[a, pos] = element + counter;//we place one brick horizontal so it doesnt stay on top of the first one
                        secondLayout[a, pos + 1] = element + counter;//second index of the brick
                        element++;//we set the next number for the next brick
                        secondLayout[a + 1, pos] = element + counter;//since there is no other way for the brick to be layed down me place another brick horizontaly
                        secondLayout[a + 1, pos + 1] = element + counter;//second index of the brick
                        element++;
                        pos += 2;//add 2 points to position
                    }
                    else//if first brick must be vertical
                    {
                        if (pos == colLenght - 1 && arr[a,pos] == arr[a+1,pos])// we check if we the brick is compatible with its placing for last brick
                        {
                            flag = true;// if the brick is incompatible we set the flag for no solution found to true so we know there is no solution found
                            break;//breaking out of loop to continue to print N-1
                        }
                        secondLayout[a, pos] = element + counter;//we set the brick laying vertically
                        secondLayout[a + 1, pos] = element + counter;//we set the second index of the brick vertically
                        element++;//we set the next number for the next brick
                        if (pos+1 == colLenght - 1 && arr[a, pos] == arr[a + 1, pos])// we check if the next brick is compatible with its placing for last brick
                        {
                            flag = true;// if the brick is incompatible we set the flag for no solution found to true so we know there is no solution found
                            break;//breaking out of loop to continue to print N-1
                        }
                        secondLayout[a, pos + 1] = element + counter;//if its compatible we set the second brick
                        secondLayout[a + 1, pos + 1] = element + counter;//second index of the brick
                        element++;//we add the next element for placing
                        pos += 2;//increment the position 
                    }
                }
                pos = 0;// we reset the position for the next rows we are gonna work on
                counter += element;//we add the element number so we can keep the element we are working on
                element = 0;//we reset the element so the while can work properly
                a += 2;//we continue with the next 2 rows 
            }
            if (flag == false)//if there is a solution for the current solution 
            {
                Console.WriteLine("Layout two : ");
                outputArray(secondLayout, rowLenght, colLenght);//calling function for output
            }
            else // otherwise
            {
                Console.WriteLine("No solution found !!! ");
                Console.WriteLine("0'es are not compatible with layout ");
                outputArray(secondLayout, rowLenght, colLenght);
            }
        }

        static bool validateLayout(int[,] arr, int rowLenght, int colLenght) // method that validates given layout
        {
            List<int> counter = new List<int>();//intializing an empty list
            foreach (int i in arr) //for each element in our layout we insert in the array
            {
                counter.Add(i);
            }
            var most = counter.GroupBy(i => i).OrderByDescending(grp => grp.Count())
            .Select(grp => grp.Key).First();//we get the number with most occurances in our layout se we can check if there are more than 2
            var count = counter.Where(x => x.Equals(most)).Count();
            if (count > 2) { return false; }//if there are more than 2 occurances that means that 1 brick spreads around more than 2 spaces in our layout therefore invalid
            return true;
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Size M : ");
            int m = 0;
            int n = 0;
            bool success= false;
            while (success != true) //while the input is wrong
            {
                success = int.TryParse(Console.ReadLine(), out m);
                if (m == 0 || m > 100 || m<0) //if the number is either 0 or exceding 100 or a negative
                {
                    Console.WriteLine("Input cannot be 0 or >100 enter new input : ");
                    success = false;
                }
                if (success == true && m % 2 != 0) //if the number is uneven and succeded in parsing
                {
                    Console.WriteLine("Enter even number : ");
                    success = false;
                }
            }

            success = false;//we reset the flag
            Console.WriteLine(" Size N : ");
            while (success != true)//while the input is wrong
            {
                success = int.TryParse(Console.ReadLine(), out n);
                if (n == 0 || n > 100||n<0) //if the number is either 0 or exceding 100 or a negative
                {
                    Console.WriteLine("Input cannot be 0 or >100 enter new input : ");
                    success = false;
                }
                if (success == true && n % 2 != 0)//if the number is uneven and succeded in parsing
                {
                    Console.WriteLine("Enter even number : ");
                    success = false;
                }
            }

            int[,] layoutOne = new int[m, n];//intializing the layout 2D array with the given specifications of N and M
            int a = 0;//the number of rows starting at 0
            int rowLenght = 0;
            string[] digits= new string[0];
            while (a < m)
            {
                Console.WriteLine("Enter " + a + " row : ");// Enter the row
                while (digits.Length != n) // while the lenght of the number array is different thant the lenght we expect
                { 
                digits = Regex.Split(Console.ReadLine(), @"\D+");//we get the numbers from the input
                }
                foreach (string value in digits) //foreach element that we got we try and parse it
                {
                    int number;
                    if (int.TryParse(value, out number))
                    {
                        layoutOne[a, rowLenght] = number;
                        rowLenght++;
                    }
                }
                digits = new string[0];//we clear the array and start over for the next rows
                rowLenght = 0;// we reset the lenght
                a++;//we set the value to the next row
            }
            if (validateLayout(layoutOne, m, n))
            {
                Console.WriteLine("Layout one : ");
                outputArray(layoutOne, m, n);
                generateNewLayout(layoutOne, m, n);
            }
            else 
            {
                Console.WriteLine("Layout invalid !!! ");
            }         
        }
    }
}
