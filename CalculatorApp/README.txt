1. ) For this assignment you will create a calculator application. The calculator should have the standard layout and support the following operations:
-- Addition
-- Subtraction
-- Multiplication
-- Division
-- Square Root
-- Negation
Notes:

1. ) We take "Content" string from our button to know what item was pressed

2. ) We pass this data into setMathCalculation to determine which action should be done based on the button pressed. I used One_Click as the handler for all my buttons to avoid a lot of duplicate handler code. This may have been a mistake as it made the setCalculation function much more difficult to write/handle all the calls. I think splitting the input more would create less confusion in creating flags in code.

3.) Once we get our output we display our current number on "MathCalculation" the larger number before the user, and the steps involved in the calculation above this number in "PrevCalculation". Just like the Microsoft calculator.

4.) The user initially can write their own operand, or use any operator on "0". However, use of the 2nd operand stack ("=") will not work here (Shown later). User can make the digit positive or negative. Double Negation works too!
--- We use an endOfCalculation flag here to know when to overwrite the zero or not.
--- For example, if the user spammed "0" at the beginning we still just want "0", 
----and if the user wants to start a new problem, we want them to overwrite the 
----current digit

5.) 1 + 1 / 1 - 1 /...
--- Now the 2nd operand will be saved to a second register allowing for +|/|*|- 1
--- We also test for enterOperand because we do not want the user to make calculations
--- if they get an error / already entered a operator.
--- We put this string into "PrevCalculation" to show the user what happened. We also --- intially flag this as not "UseOperandEquals" so that we know to overwrite the 
--- current data in the register

6) Sqrt/Negation
--- Just takes the sqrt/negation of the number typed by the user

7) Clear
--- Clears all input (not saved data) and basically resets the calculator

8.) Handled Errors
--- 1. Divide by Zero
--- 2. Overflow 
--- 3. Square Root of Negative
------ Forces the user to enter/load an integer. User cannot save this string or use
------ any operators at this point.

8.) EC
--- Programs has a 3rd register to support the loading/saving of entered integers.

9.) Known Issue
--- If the user's input string is larger than 15/16 characters it will no longer fit
on the "MathCalculation" screen. However, this entire number will be visible on the "PrevCalculation" textbox.
