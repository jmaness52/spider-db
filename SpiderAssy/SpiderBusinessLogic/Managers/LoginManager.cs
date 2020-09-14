using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SpiderBusinessLogic.Exceptions;
using SpiderBusinessLogic.Models;
using SpiderDatabase;
using SpiderDatabase.Procedures;

namespace SpiderBusinessLogic.Managers
{
    public class LoginManager
    {

        //Dependency Injected services
        private IDataAccess _dataAccess;
        private readonly ILogger<LoginManager> _logger;

        //Tracks current users
        private const int maxConcurrentUsers = 10;
        private ConcurrentBag<UserModel> loggedInUsers;


        public LoginManager(IDataAccess dataAccess, ILogger<LoginManager> logger)
        {
            _dataAccess = dataAccess;
            _logger = logger;
            loggedInUsers = new ConcurrentBag<UserModel>();
        }

        /// <summary>
        /// Attempts to log in the user. Redirects to an appropriate page based on the result
        /// </summary>
        /// <param name="emailAddress">User Email Address</param>
        /// <returns>A redirect URL</returns>
        public async Task<string> AttemptUserLogin(string emailAddress)
        {
            try
            {
                Tuple<bool, UserModel> result = await CheckUserExists(emailAddress);
                
                //One user exists, UserModel is in Item2
                if (result.Item1 == true)
                {
                    if (loggedInUsers.Count < maxConcurrentUsers)
                    {
                        //Add user to list of logged in users and redirect to logged in home page
                        loggedInUsers.Add(result.Item2);
                        return "~/";
                    }
                    else
                    {
                        _logger.LogInformation($"Login attempted failed for user {emailAddress}. System is at capacity, please try to log in later");
                        return "Logout OR capacity reached page";
                    }
                }
                else //User doesn't exist in Spider, redirect to add new user page
                {
                    _logger.LogInformation($"Creating user {emailAddress} in Spider Database");
                    return "~/NewUser";
                }
            }
            catch (UsersException ex) //Shouldn't happen but this occurs if multiple users exist under the email address
            {
                _logger.LogError(ex.Message);
                return "logout or error page";
            }   
        }

        public async Task AddNewUser(AddUserModel newUser)
        {

            Tuple<bool, UserModel> duplicateUserCheck = await CheckUserExists(newUser.EmailAdress);

            //Check that a user doesn't already exist with the provided email address
            if (!duplicateUserCheck.Item1 == true)
            {
                await _dataAccess.SaveData<dynamic>(InsertData.Insert_User, new
                {

                    emailAddress = newUser.EmailAdress,
                    firstName = newUser.FirstName,
                    lastName = newUser.LastName,
                    companyName = newUser.CompanyName,
                    permissionLevelID = newUser.PermissionLevelID
                });
            }
        }

        private async Task<Tuple<bool, UserModel>> CheckUserExists(string emailAddress)
        {
            //Cant have out parameter with async method,  using tuple as a workaround
            //If the user doesnt exist (or more than 1 user exists) false is returned with a null UserModel

            List<UserModel> users = await _dataAccess.LoadData<UserModel, dynamic>(FetchData.Get_User, new { userEmail = emailAddress });

            //only one user should exist per email address.
            if (users.Count > 1)
            {
                throw new UsersException($"More than one user exists with the email address: {emailAddress}. Please contact system admin");
            }
            else if (users.Count == 1)
            {
                return new Tuple<bool, UserModel>(true, users[0]);
            }
            else
            {
                return new Tuple<bool, UserModel>(false, null);
            }
        }

    }
}
