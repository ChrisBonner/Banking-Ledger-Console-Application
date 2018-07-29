using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ConsoleLedgerApp
{
    class BankingLedgerApp
    {

        #region Enum & Globals

        /// <summary>
        /// Enum for user options
        /// </summary>
        enum Method
        {
            Invalid,
            CreateAccount,
            Login,
            Deposit,
            Withdraw,
            CheckBalance,
            ViewHistory,
            Logout,
            Towel,
            Help,
            Exit
        };

        /// <summary>
        /// Used for Shorthand Console.Writeline and end with new line.
        /// </summary>
        private Action<string> cw = (string output) => Console.WriteLine("{0}\n", output);

        #endregion

        #region Public Constructor

        /// <summary>
        /// BankingLedgerApp Constructor
        /// </summary>
        public BankingLedgerApp()
        {

            Boolean exit = false;

            cw("Welcome to the World's Greatest Bank Ledger Application!");
            cw("You can type 'Help' for the list of commands!");

            while (! exit)
            {
                string userInput = GetInput();

                Method result = CheckUserInput(userInput.Replace(" ", ""));

                switch (result)
                {
                    case Method.Invalid:
                        cw("You entered an Invalid Command. Please enter a valid Command or type 'Help' for options.");
                        break;
                    case Method.CreateAccount:
                        CreateAccount(); 
                        break;
                    case Method.Login:
                        Login();
                        break;
                    case Method.Deposit:
                        cw("You are not logged in. Please login to an existing user account.");
                        break;
                    case Method.Withdraw:
                        cw("You are not logged in. Please login to an existing user account.");
                        break;
                    case Method.CheckBalance:
                        cw("You are not logged in. Please login to an existing user account.");
                        break;
                    case Method.ViewHistory:
                        cw("You are not logged in. Please login to an existing user account.");
                        break;
                    case Method.Logout:
                        cw("You are not logged in to an account.");
                        break;
                    case Method.Help:
                        GetHelp();
                        break;
                    case Method.Exit:
                        exit = true;
                        break;
                    default: 
                        cw("You entered an Invalid Command. Please enter a valid Command or type 'Help' for options.");
                        break;
                }
                if (! exit)
                {
                    cw("Please enter next command.");
                }
            }
        }

        #endregion

        #region Private Constructor Functions

        /// <summary>
        /// Check the user Input to see if it matches a Method Enum
        /// </summary>
        /// <param name="userInput"></param>
        /// <returns>If exists returns Method Enum else return Method.Invalid</returns>
        private Method CheckUserInput(string userInput)
        {
            Method selectedMethod = Method.Invalid;

            Enum.TryParse(userInput, true, out selectedMethod);
   
            return selectedMethod;
        }

        #endregion
        
        #region Private Create Account Functions

        /// <summary>
        /// Processes user input to create new user account
        /// </summary>
        private void CreateAccount()
        {
            string tempUser = "";
            string newUser = "";
            string tempPassword = "";
            string newPassword = "";
            Boolean exit = false;
            Boolean isValidUser = false;
            Boolean isValidPassword = false;
            
            cw("Creating a new user account.");
            while (!exit)
            {
                cw("Please enter new username or type 'exit' to go back to the main menu.");
                while (! isValidUser)
                {
                    tempUser = ""; // ensuring var is cleared before re-assignment
                    newUser = ""; 

                    tempUser = GetInput();

                    if(tempUser != "")
                    {
                        if (!ToExit(tempUser))
                        {
                            Boolean usernameSuccess = CheckUsername(tempUser);
                            if (usernameSuccess == true)
                            {
                                newUser = tempUser;
                                isValidUser = true;
                                break;
                            }
                        }
                        else
                        {
                            exit = true;
                            break;
                        }
                    }
                    else
                    {
                        cw("Username entered was blank. Please enter a valid username.");
                    }
                }

                if (exit == true) // check for exit break
                {
                    break;
                }

                cw("Please enter new password or type 'exit to go back to the main menu.");
                while (! isValidPassword)
                {

                    tempPassword = "";// ensuring var is cleared before re-assignment
                    newPassword = "";

                    tempPassword = GetInput();
                    if(tempPassword != "")
                    {
                        if (!ToExit(tempPassword))
                        {
                            Boolean passwordSuccess = CheckPassword(tempPassword);
                            if (passwordSuccess == true)
                            {

                                newPassword = PasswordEncryption.Encrypt(tempPassword, tempUser); // Password encryption
                                isValidPassword = true;
                                break;
                            }
                        }
                        else
                        {
                            exit = true;
                            break;
                        }
                    }
                    else
                    {
                        cw("Password entered was blank. Please enter a valid password.");
                    }
                }

                if (exit == true) // check for exit break
                {
                    break;
                }

                if (isValidUser && isValidPassword)
                {
                    exit = CreateAccount(tempUser, newPassword);
                    if(exit == true)
                    {
                        cw("New user has been added. Returning to main menu.");
                    }
                    else
                    {
                        cw("There was an Error adding the new user. Please try again.");
                    }
                    
                }
            }
        }

        /// <summary>
        /// Checks the username entered passes username validation check
        /// </summary>
        /// <param name="tempUser">string</param>
        /// <returns>Boolean if the username entered passes all validation checks</returns>
        private Boolean CheckUsername(string tempUser)
        {
            Boolean isValid = false;

            if (ValidUser(tempUser)) // Valid
            {
                if (! DoesExist(tempUser)) // Not Exist
                {
                    isValid = true;
                }
                else // Exist
                {
                    cw("Username already exists. Please try another Username.");
                }
            }
            else
            {
                cw("Username entered is not Valid. Please try another Username.");
            }
            
            return isValid;
        }
        
        /// <summary>
        /// Checks to make sure the username entered is in a valid format
        /// </summary>
        /// <param name="tempUser">string</param>
        /// <returns>Boolean if the username is in a valid format</returns>
        private Boolean ValidUser(string tempUser)
        {
            Boolean isValidUser = false;
            Regex usernameRegEx = new Regex(@"^(?=.{3,15}$)([A-Za-z0-9][._()\[\]-]?)*$");

            if(! string.IsNullOrEmpty(tempUser) && usernameRegEx.IsMatch(tempUser) && ! tempUser.Any(Char.IsWhiteSpace))
            {
                isValidUser = true;
            }

            return isValidUser;
        }

        /// <summary>
        /// Checks if the username entered already exists or not
        /// </summary>
        /// <param name="newUser">string</param>
        /// <returns>Boolean if user already exists or not.</returns>
        private Boolean DoesExist(string newUser)
        {
            Boolean doesExist = true;
            List<Users> userList = GetUsers();

            if(userList.Count > 0)
            {
                foreach (Users user in userList)
                {
                    if (! CompareStrings(newUser, user.Username, true))
                    {
                        doesExist = false;
                        break;
                    }
                }

            } else
            {
                doesExist = false;
            }

            return doesExist;

        }

        /// <summary>
        /// Checks the password entered passes validation checks.
        /// </summary>
        /// <param name="tempPassword">string</param>
        /// <returns>Boolean if password is valid.</returns>
        private Boolean CheckPassword(string tempPassword)
        {
            //Password Length Constents
            const int MIN_LENGTH = 8;
            const int MAX_LENGHT = 15;

            Boolean isValid = false;
            Boolean noSpace = false;
            Boolean metLength = false;
            Boolean hasUpper = false;
            Boolean hasLower = false;
            Boolean hasDigit = false;

            if(! tempPassword.Any(Char.IsWhiteSpace))
            {
                noSpace = true;

                if (tempPassword.Length >= MIN_LENGTH && tempPassword.Length <= MAX_LENGHT) // Length Check
                {
                    metLength = true;

                    if (tempPassword.Any(Char.IsUpper)) // Upper Case Check
                    {
                        hasUpper = true;
                    }

                    if (tempPassword.Any(Char.IsLower)) // Lower Case Check
                    {
                        hasLower = true;
                    }

                    if (tempPassword.Any(Char.IsDigit)) // Digit Check
                    {
                        hasDigit = true;
                    }
                }
            }

            if (! noSpace)
            {
                cw("Password cannot contain spaces. Please try another password.");
            }
            else if (! metLength)
            {
                cw("Password should be greater than " + MIN_LENGTH + " characters and less than " + MAX_LENGHT + " characters. Please try another password.");
            }
            else if (! hasUpper)
            {
                cw("Password must contain at least one upper case letter. Please try another password.");
            }
            else if (! hasLower)
            {
                cw("Password must contain at least one lower case letter. Please try another password.");
            }
            else if (!hasDigit)
            {
                cw("Password must contain at least one digit. Please try another password.");
            }

            if (noSpace && metLength && hasUpper && hasLower && hasDigit)
            {
                isValid = true;
            }

            return isValid;
        }

        /// <summary>
        /// Gets Current List<Users> then creates a new User account and adds it to the List<Users> of Users then SetUsers()
        /// </summary>
        /// <param name="username">String</param>
        /// <param name="password">String</param>
        private Boolean CreateAccount(string username, string password)
        {
            List<Users> currentUsers = GetUsers();
            Boolean success = false;
            History creationHistory = new History
            {
                TransactionAmount = 0.0M,
                TransactionType = "Account Creation"
            };

            Users newUser = new Users
            {
                Username = username,
                Password = password,
                HistoryList = new List<History>
                {
                    creationHistory
                },
                CurrentBalance = 0.0M
            };

            currentUsers.Add(newUser);
            success = SetUsers(currentUsers);

            return success;

        }

        #endregion

        #region Private Login Functions

        /// <summary>
        /// Login to existing user account validation
        /// </summary>
        private void Login()
        {

            string enteredUsername = "";
            string enteredPassword = "";
            Boolean exit = false;
            Boolean isValidUser = false;
            Boolean isValidPassword = false;
            Boolean logout = false;

            cw("Logging into an existing account.");

            while (!exit)
            {
                cw("Please enter the username.");

                while (! isValidUser)
                {
                    enteredUsername = GetInput();

                    if (enteredUsername != "")
                    {
                        if (! ToExit(enteredUsername))
                        {
                            isValidUser = ValidLoginUser(enteredUsername);
                            if (! isValidUser)
                            {
                                cw("No user with that username found. Please try again.");
                            }
                        }
                        else
                        {
                            exit = true;
                            break;
                        }
                    }
                }

                if (exit == true) // check for exit break
                {
                    break;
                }


                cw("Please enter the password.");
                while (! isValidPassword)
                {
                    enteredPassword = GetInput();

                    if (enteredPassword != "")
                    {
                        if (! ToExit(enteredPassword))
                        {
                            isValidPassword = ValidLoginUserPassword(enteredUsername, enteredPassword);
                            if (! isValidPassword)
                            {
                                cw("Invalid Password. Please try again.");
                            }
                        }
                        else
                        {
                            exit = true;
                            break;
                        }
                    }
                }

                if (exit == true) // check for exit break
                {
                    break;
                }

                if(isValidUser && isValidPassword)
                {
                    Users activeUser = GetActiveUser(enteredUsername); // See Misc Helper Functions
                    logout = LoggedIn(activeUser);

                    if(logout == true)
                    {
                        cw("Logging out of user account and returning to the main menu.");
                        exit = true;
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Validate login username exists
        /// </summary>
        /// <param name="enteredUsername"></param>
        /// <returns>Boolean if it was successful or not</returns>
        private Boolean ValidLoginUser(string enteredUsername)
        {
            Boolean isValid = false;
            List<Users> currentUsers = GetUsers();

            foreach (Users user in currentUsers)
            {
                if(CompareStrings(enteredUsername, user.Username, true))
                {
                    isValid = true;
                }
            }

            return isValid;
        }

        /// <summary>
        /// Validate login username & password
        /// </summary>
        /// <param name="enteredUsername">string</param>
        /// <param name="enteredPassword">string</param>
        /// <returns>Boolean if it was successful or not</returns>
        private Boolean ValidLoginUserPassword(string enteredUsername, string enteredPassword)
        {
            Boolean isValid = false;
            List<Users> currentUsers = GetUsers();

            foreach (Users user in currentUsers)
            {
                if (CompareStrings(enteredUsername, user.Username, true))
                {
                    string username = user.Username;
                    string encryptedPassword = user.Password; // encrypted password
                    string unencryptedPassword = PasswordEncryption.Decrypt(encryptedPassword, username);

                    if(CompareStrings(enteredPassword, unencryptedPassword, false))
                    {
                        isValid = true;
                    }
                    
                }
            }

            return isValid;
        }

        /// <summary>
        /// Runs users commands once logged into an active user account.
        /// </summary>
        /// <param name="activeUser">User</param>
        /// <returns>Boolean user ended session - logout</returns>
        private Boolean LoggedIn(Users activeUser)
        {
            Boolean logout = false;

            cw("You are now logged in as: " + activeUser.Username + ".");
            cw("For a list of options please use the 'Help' command.");

            while (! logout)
            {
                string userInput = GetInput(); // See Misc Helper Functions
                Boolean success = false;

                Method result = CheckUserInput(userInput.Replace(" ", ""));

                switch (result)
                {
                    case Method.Invalid: // done
                        cw("You entered an Invalid Command. Please enter a valid Command or type 'Help' for options.");
                        break;
                    case Method.CreateAccount:
                        cw("You are already logged in to a user account. Please 'Logout' of the account before creating a new account.");
                        break;
                    case Method.Login:
                        cw("You are already logged in to a user account. Please 'Logout' of the account to login to another account.");
                        break;
                    case Method.Deposit:
                        success = Deposit(activeUser);
                        break;
                    case Method.Withdraw:
                        success = Withdraw(activeUser);
                        break;
                    case Method.CheckBalance:
                        success = CheckBalance(activeUser);
                        if (! success)
                        {
                            logout = true;
                        }
                        break;
                    case Method.ViewHistory:
                        ViewHistory(activeUser);
                        break;
                    case Method.Logout:
                        logout = Logout();
                        break;
                    case Method.Towel:
                        Towel();
                        break;
                    case Method.Help:
                        GetHelp(); // done
                        break;
                    case Method.Exit: // done
                        cw("You cannot exit while logged in to a user account.");
                        cw("Loging out of the user account.");
                        logout = Logout();
                        break;
                    default: // done
                        cw("Error processing user Input: " + userInput + ". Please enter a valid Command or type 'Help' for options.");
                        break;
                }
                if (! logout)
                {
                    cw("Please enter next command.");
                }
            }

            return logout;
        }
        
        /// <summary>
        /// Checks to ensure user wishes to logout.
        /// </summary>
        /// <returns>Boolean if user wants to logout.</returns>
        private Boolean Logout()
        {
            Boolean logout = false;

            cw("Are you sure you want to logout of the user account?");
            string response = GetInput(); // See Misc Helper Functions

            if (CompareStrings(response, "yes", true) || CompareStrings(response, "y", true) || CompareStrings(response, "logout", true) || CompareStrings(response, "log out", true))
            {
                logout = true;
            }
            else
            {
                cw("Logout aborted by user.");
            }
            return logout;
        }

        #endregion

        #region Private Deposit Functions

        /// <summary>
        /// Deposits into a user account
        /// </summary>
        /// <param name="activeUser">Users</param>
        /// <returns>Boolean if successful or not</returns>
        private Boolean Deposit(Users activeUser)
        {
            Boolean success = false;
            Decimal depositAmount = 0.0M;
            
            cw("How much is the Deposit for?");
            depositAmount = GetDepositAmount();

            if (depositAmount > 0.0M)
            {
                History myHistory = new History
                {
                    TransactionAmount = depositAmount,
                    TransactionType = "Deposit"
                };

                activeUser.HistoryList.Add(myHistory);
                activeUser.CurrentBalance += depositAmount;

                success = UpdateUserList(activeUser);

                if (success)
                {
                    cw("Current user balance is : $" + activeUser.CurrentBalance);
                }
                else
                {
                    cw("Error updating user account. Please try again.");
                }
            }
            else
            {
                cw("Deposit amount cannot be a negative value.");
            }

            return success;
        }

        /// <summary>
        /// Validates user input for deposit amount.
        /// </summary>
        /// <returns>Decimal</returns>
        private Decimal GetDepositAmount()
        {
            Decimal depositAmount = 0.0M;
            Boolean success = false;
            Boolean exit = false;

            while (!exit)
            {
                while (!success)
                {
                    string response = GetInput(); // See Misc Helper Functions

                    if (!ToExit(response))
                    {
                        success = ValidateAmount(response); // See Misc Helper Functions
                        if (success)
                        {
                            depositAmount = Decimal.Parse(response, System.Globalization.NumberStyles.Currency);
                            exit = true;
                        }
                        else
                        {
                            cw("Please try again.");
                        }
                    }
                    else
                    {
                        exit = true;
                        cw("Deposit was aborted by user.");
                        break;
                    }
                }

                if (exit == true) // check for exit break
                {
                    break;
                }
            }

            return depositAmount;
        }


        #endregion

        #region Private Withdraw Functions

        /// <summary>
        /// Withdraw from a user account
        /// </summary>
        /// <param name="activeUser"> Users</param>
        /// <returns>Boolean if it was successful or not</returns>
        private Boolean Withdraw(Users activeUser)
        {
            Boolean success = false;
            Decimal withdrawAmount = 0.0M;

            cw("How much do you want to withdraw?");
            withdrawAmount = GetWithdrawAmount();

            if (withdrawAmount > 0.0M)
            {
                Decimal tempBalance = activeUser.CurrentBalance;
                if ((tempBalance -= withdrawAmount) >= 0.0M)
                {

                    History myHistory = new History
                    {
                        TransactionAmount = withdrawAmount,
                        TransactionType = "Withdraw"
                    };

                    activeUser.HistoryList.Add(myHistory);
                    activeUser.CurrentBalance -= withdrawAmount;

                    success = UpdateUserList(activeUser);

                    if (success)
                    {
                        cw("Current user balance is : $" + activeUser.CurrentBalance);
                    }
                    else
                    {
                        cw("Error updating user account.");
                    }
                }
                else
                {
                    cw("Cannot withdraw more than is in the user account.");
                }
            }

            return success;
        }

        /// <summary>
        /// Validates user input for withdraw amount.
        /// </summary>
        /// <returns>Decimal</returns>
        private Decimal GetWithdrawAmount()
        {
            Decimal withdrawAmount = 0.0M;
            Boolean success = false;
            Boolean exit = false;

            while (!exit)
            {
                while (!success)
                {
                    string response = GetInput(); // See Misc Helper Functions

                    if (!ToExit(response))
                    {
                        success = ValidateAmount(response); // See Misc Helper Functions
                        if (success)
                        {
                            withdrawAmount = Decimal.Parse(response, System.Globalization.NumberStyles.Currency);
                            exit = true;
                        }
                        else
                        {
                            cw("Invalid withdraw amount. Please try again.");
                        }
                    }
                    else
                    {
                        exit = true;
                        cw("Withdraw was aborted by user.");
                        break;
                    }
                }

                if (exit == true) // check for exit break
                {
                    break;
                }
            }

            return withdrawAmount;
        }

        #endregion

        #region Private Check balance Function

        /// <summary>
        /// Checks a Users current balance.
        /// </summary>
        /// <param name="activeUser">Users</param>
        /// <returns>Boolean if it was successful or not</returns>
        private Boolean CheckBalance(Users activeUser)
        {
            List<Users> currentUsers = GetUsers();
            Boolean exist = false;

            foreach(Users user in currentUsers)
            {
                if(activeUser.Username == user.Username)
                {
                    cw("Current Balance for " + user.Username + " is $" + user.CurrentBalance + ".");
                    exist = true;
                    break;
                }
            }

            if (!exist)
            {
                cw("Error retrieving balance. User no longer exists!.");
            }

            return exist;
        }

        #endregion

        #region Private View History Functions

        /// <summary>
        /// Displays the users transaction history
        /// </summary>
        /// <param name="activeUser">Users</param>
        private void ViewHistory(Users activeUser)
        {

            cw("Viewing the full history for user " + activeUser.Username);

            foreach (History history in activeUser.HistoryList)
            {
                cw(history.TransactionType + " " + history.TransactionAmount);
            }
            cw("Current balance is: $" + activeUser.CurrentBalance);
        }

        #endregion

        #region Private Help Functions

        /// <summary>
        /// Displays the help messages to the user.
        /// </summary>
        private void GetHelp()
        {
            cw("This Bank Ledger Application allows you to do the following:");
            cw("To create a new account please use the command 'Create Account'.");
            cw("Account passwords must be between 8 and 15 characters long and contain at least one upper case letter, one lower case letter and one digit.");
            cw("To login to an existing account please use the command 'Login.");
            cw("To make a deposit, be logged into an existing user account and use the command 'Deposit'.");
            cw("To make a withdraw, be logged into an existing user account and use the command 'Withdraw'.");
            cw("To check users balance, be logged into an existing user account and use the command 'Check Balance'.");
            cw("To view the transaction history for an existing user, be logged into the existing user account and use the command 'View History'.");
            cw("To logout of an existing user account use the command 'Logout'.");
            cw("To exit the application please use the command 'Exit'.");
        }

        #endregion

        #region Misc Private Helper Functions

        /// <summary>
        /// Gets the user input
        /// </summary>
        /// <returns>string</returns>
        private string GetInput()
        {
            return Console.ReadLine().Trim();
        }

        /// <summary>
        /// Checks to see if the user wishes to exit or not.
        /// </summary>
        /// <param name="userInput">string</param>
        /// <returns>Boolean if the user wishes to exit to the main menu</returns>
        private Boolean ToExit(string userInput)
        {
            Boolean exit = false;

            if (CompareStrings(userInput, "q", true) || CompareStrings(userInput, "quit", true) || CompareStrings(userInput, "exit", true))
            {
                cw("Returning to main menu.");
                exit = true;
            }
            return exit;
        }
        
        /// <summary>
        /// Retrieves List<Users> of Users from users.json file.
        /// </summary>
        /// <returns>List<Users> of Users</returns>
        private List<Users> GetUsers()
        {
            List<Users> userList = new List<Users>();
            try
            {
                string userJson = File.ReadAllText("../../users.json");

                userList = JsonConvert.DeserializeObject<List<Users>>(userJson);
            }
            catch (Exception)
            {
                cw("There was an error getting the Users.");
            }

            return userList;
        }

        /// <summary>
        /// Outputs users to users.json file.
        /// </summary>
        /// <param name="usersList">List<Users> of Users</param>
        /// <returns>Boolean if it was successful or not</returns>
        private Boolean SetUsers(List<Users> usersList)
        {
            Boolean success = false;
            try
            {
                File.WriteAllText(@"../../users.json", JsonConvert.SerializeObject(usersList));

                using (StreamWriter file = File.CreateText(@"../../users.json"))
                {
                    JsonSerializer js = new JsonSerializer();
                    js.Serialize(file, usersList);
                }
                success = true;
            }
            catch (Exception)
            {
                cw("There was an error setting the Users.");
            }

            return success;

        }

        /// <summary>
        /// Gets the user that matches username
        /// </summary>
        /// <param name="username">string</param>
        /// <returns>Users object that matches username</returns>
        private Users GetActiveUser(string username)
        {
            Users activeUser = new Users();
            List<Users> currentUsers = GetUsers(); // See Misc Helper Functions

            foreach (Users user in currentUsers)
            {
                if (CompareStrings(username, user.Username, true))
                {
                    activeUser = user;
                    break;
                }
            }

            return activeUser;
        }

        /// <summary>
        /// Updates the local users.json file
        /// </summary>
        /// <param name="activeUser">Users</param>
        /// <returns>Boolean if it was successful or not</returns>
        private Boolean UpdateUserList(Users activeUser)
        {
            Boolean success = false;

            List<Users> allUsers = GetUsers();

            foreach (Users user in allUsers)
            {
                if (activeUser.Username == user.Username)
                {
                    user.CurrentBalance = activeUser.CurrentBalance;
                    user.HistoryList = activeUser.HistoryList;
                    success = SetUsers(allUsers);
                    break;
                }
            }

            return success;
        }

        /// <summary>
        /// Lets the user know how important a towel is.
        /// </summary>
        private void Towel()
        {
            cw("A towel is just about the most massively useful thing any interstellar Hitchhiker can carry. Partly it has great practical value.");
            cw("More importantly, a towel has immense psychological value.");
        }
        /// <summary>
        /// Checks if strings match case insensitive
        /// </summary>
        /// <param name="str1">string</param>
        /// <param name="str2">string</param>
        /// <param name="ignoreCase">Bool - true = ignore case</param>
        /// <returns>Boolean if the strings matched or not.</returns>
        private Boolean CompareStrings(string str1, string str2, bool ignoreCase)
        {
            Boolean isCompared = false;

            if (ignoreCase)
            {
                isCompared = string.Equals(str1, str2, StringComparison.CurrentCultureIgnoreCase);
            }
            else
            {
                isCompared = string.Equals(str1, str2, StringComparison.CurrentCulture);
            }
            
            return isCompared;
        }

        /// <summary>
        /// Validates user input for deposit and withdraw.
        /// </summary>
        /// <param name="response">string</param>
        /// <returns>Boolean if the input was valid or not</returns>
        private Boolean ValidateAmount(string response)
        {
            Boolean isValid = false;

            if (Decimal.TryParse(response, out decimal value))
            {
                cw("Entered Amount: $" + value);
                isValid = true;
            }

            return isValid;
        }

        #endregion

    }
}
